using LinkedInLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;

namespace LinkedInAPICall
{
    public partial class linkedinprofile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["access_token"] != null && Request.Cookies["access_token"].Value != "")
            {
                lblAccessToken.Text = Request.Cookies["access_token"].Value;
            }
            if (Request.Cookies["expires_in"] != null && Request.Cookies["expires_in"].Value != "")
            {
                lblExpiresIn.Text = Request.Cookies["expires_in"].Value;
            }
        }

        /* WORKING */
        protected void btnGetUserProfile_Click(object sender, EventArgs e)
        {
            LinkedInLibrary.BaseRequest.Get<LiteProfile, LinkedInErrorResponse>(
                LinkedInLibrary.Constants.URLs.GetUSERPROFILE("projection=(id,firstName,lastName)"),
                lblAccessToken.Text,
                (profile) =>  //on get user profile success
                {
                    lblUserID.Text = profile.id;
                    lblFirstName.Text = profile.firstName.localized.en_US;
                    lblLastName.Text = profile.lastName.localized.en_US;
                    btnGetPosts.Visible = true;
                    btnTextShare.Visible = true;
                    btnGetSharePosts.Visible = true;
                    btnFileUpload.Visible = true;
                    btnGetOrganizations.Visible = true;
                    btnURLShare.Visible = true;
                    return null;
                },
                HandleErrorResponse
                );
        }

        public object HandleErrorResponse(LinkedInErrorResponse errorobject)
        {
            divError.Visible = true;
            lblServiceErrorCode.Text = Convert.ToString(errorobject.serviceErrorCode);
            lblErrorMessage.Text = errorobject.message;
            lblErrorStatus.Text = Convert.ToString(errorobject.status);
            return null;
        }

        /*
         * WORKING
         */

        #region User Text Post
        protected void btnTextShare_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(lblAccessToken.Text) && !String.IsNullOrEmpty(lblUserID.Text))
            {
                LinkedInTextShareRequest requestbody = new LinkedInTextShareRequest();

                requestbody.author = String.Format("urn:li:person:{0}", lblUserID.Text);
                requestbody.specificContent.shareContent.shareCommentary.text = txtTextShare.Text;
                LinkedInLibrary.BaseRequest.Post<LinkedInShare, LinkedInErrorResponse>(LinkedInLibrary.Constants.URLs._ugcposts,
                    lblAccessToken.Text,
                    () =>
                    {
                        string s = JsonConvert.SerializeObject(requestbody);
                        return new StringContent(s, Encoding.UTF8, "application/json");
                    },
                    (LinkedInShare share) =>
                    {
                        divError.Visible = false;
                        divSuccess.Visible = true;
                        lblShareURN.Text = share.ShareURN;
                        return null;
                    },
                    HandleErrorResponse);
            }
        }
        #endregion

        #region URL Share
        protected void btnURLShare_Click(object sender, EventArgs e)
        {
            LinkedInLibrary.BaseRequest.Post<LinkedInShare, LinkedInErrorResponse>(
                LinkedInLibrary.Constants.URLs._ugcposts,
                lblAccessToken.Text,
                () =>
                {
                    LinkedInTextShareRequest requestbody = new LinkedInTextShareRequest();
                    requestbody.author = String.Format("urn:li:person:{0}", lblUserID.Text);
                    requestbody.specificContent.shareContent.shareCommentary.text = txtURLShareComment.Text;
                    requestbody.specificContent.shareContent.shareMediaCategory = "ARTICLE";
                    Media media = new Media();
                    media.description.text = txtURLShareDescription.Text;
                    media.originalUrl = txtURLShareURL.Text;
                    media.title.text = txtURLShareTitle.Text;
                    requestbody.specificContent.shareContent.media = new List<Media>()
                    {
                        media
                    };
                    string s = JsonConvert.SerializeObject(requestbody);
                    return new StringContent(s, Encoding.UTF8, "application/json");
                },
                (LinkedInShare share) =>
                {
                    divError.Visible = false;
                    divSuccess.Visible = true;
                    lblShareURN.Text = share.ShareURN;
                    btnGetPost.Visible = true;
                    return null;
                },
                HandleErrorResponse
                );
        }
        #endregion

        #region User Image Post
        protected void btnFileUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                string fileAddress = "";
                try
                {
                    fileAddress = SaveFile(fileUpload.PostedFile);
                }
                catch (Exception ex)
                {
                    fileAddress = "";
                }
                if (!string.IsNullOrEmpty(fileAddress))
                {
                    RequestUploadUrlResponse uploadDetails = null;
                    if (RequestUploadUrl(out uploadDetails))
                    {
                        //string imagepath = SaveFile(fileUpload.PostedFile);
                        if (UploadImageBinaryFile(uploadDetails, fileAddress))
                        {
                            CreateImageShareRequest requestbody = new CreateImageShareRequest();
                            requestbody.author = String.Format("urn:li:person:{0}", lblUserID.Text);
                            Media media = new Media();
                            media.description.text = ""; //does not render on image post
                            media.title.text = ""; //does not render on image post
                            media.media = uploadDetails.value.asset;
                            requestbody.specificContent.shareContent.shareCommentary.text = txtImageText.Text;
                            requestbody.specificContent.shareContent.media = new List<Media>() { media };

                            LinkedInLibrary.BaseRequest.Post<LinkedInShare, LinkedInErrorResponse>(
                                LinkedInLibrary.Constants.URLs._ugcposts,
                                lblAccessToken.Text,
                                () =>
                                {
                                    string s = JsonConvert.SerializeObject(requestbody);
                                    return new StringContent(s, Encoding.UTF8, "application/json");
                                },
                                (LinkedInShare share) =>
                                {
                                    divError.Visible = false;
                                    divSuccess.Visible = true;
                                    lblShareURN.Text = share.ShareURN;
                                    btnGetPost.Visible = true;
                                    return null;
                                },
                                HandleErrorResponse
                                );
                        }
                    }
                }
            }
        }

        private bool UploadImageBinaryFile(RequestUploadUrlResponse uploadDetails, string fileAddress)
        {
            bool isSuccess = false;
            LinkedInLibrary.BaseRequest.Post<object, LinkedInErrorResponse>(
                uploadDetails.value.uploadMechanism.mediaUploadHttpRequest.uploadUrl,
                lblAccessToken.Text,
                () =>
                {
                    return new ByteArrayContent(File.ReadAllBytes(fileAddress));
                },
                (response) =>
                {
                    isSuccess = CheckUploadStatus(uploadDetails.value.GetAssetID);
                    return null;
                },
                HandleErrorResponse,
                null,
                "PUT",
                false
                );
            return isSuccess;
        }

        private bool CheckUploadStatus(string asset)
        {
            bool isSuccess = false;
            LinkedInLibrary.BaseRequest.Get<CheckUploadStatusResponse, LinkedInErrorResponse>(
                LinkedInLibrary.Constants.URLs.CheckUploadStatus(asset),
                lblAccessToken.Text,
                (CheckUploadStatusResponse success) =>
                {
                    isSuccess = success.recipes.Find((r) => r.status != "AVAILABLE") == null ? true : false;
                    return null;
                },
                HandleErrorResponse);
            return isSuccess;
        }

        private bool RequestUploadUrl(out RequestUploadUrlResponse uploadDetails)
        {
            RequestUploadUrlRequest requestbody = new RequestUploadUrlRequest();
            requestbody.registerUploadRequest.owner = String.Format("urn:li:person:{0}", lblUserID.Text);
            RequestUploadUrlResponse obj = null;
            LinkedInLibrary.BaseRequest.Post<RequestUploadUrlResponse, LinkedInErrorResponse>(
                LinkedInLibrary.Constants.URLs._registerupload,
                lblAccessToken.Text,
                () =>
                {
                    string s = JsonConvert.SerializeObject(requestbody);
                    return new StringContent(s, Encoding.UTF8, "application/json");
                },
                (response) =>
                {
                    obj = response;
                    return null;
                },
                HandleErrorResponse
                );
            uploadDetails = obj;
            return uploadDetails == null ? false : true;
        }

        private string SaveFile(HttpPostedFile file)
        {
            //path to save the uploaded image to.
            string savePath = Server.MapPath("Content");

            // Get the uploaded file name
            string fileName = fileUpload.FileName;

            // Create the path and file name to check for duplicates.
            string pathToCheck = savePath + fileName;

            // Create a temporary file name to use for checking duplicates.
            string tempfileName = "";

            // Check to see if a file already exists with the
            // same name as the file to upload.        
            if (System.IO.File.Exists(pathToCheck))
            {
                int counter = 2;
                while (System.IO.File.Exists(pathToCheck))
                {
                    // if a file with this name already exists,
                    // prefix the filename with a number.
                    tempfileName = counter.ToString() + fileName;
                    pathToCheck = savePath + tempfileName;
                    counter++;
                }

                fileName = tempfileName;

                // Notify the user that the file name was changed.
                lblErrorMessage.Text = "A file with the same name already exists." +
                    "<br />Your file was saved as " + fileName;
            }
            else
            {
                // Notify the user that the file was saved successfully.
                lblErrorMessage.Text = "Your file was uploaded successfully.";
            }

            // Append the name of the file to upload to the path.
            savePath += fileName;

            // Call the SaveAs method to save the uploaded
            // file to the specified directory.
            fileUpload.SaveAs(savePath);
            return savePath;
        }

        #endregion

        protected void btnGetOrganizations_Click(object sender, EventArgs e)
        {
            LinkedInLibrary.BaseRequest.Get<GetOrganizationsResponse, LinkedInErrorResponse>(
                LinkedInLibrary.Constants.URLs.GetOrganizations(),
                lblAccessToken.Text,
                (GetOrganizationsResponse getOrganizations) =>
                {
                    grdGetOrganization.DataSource = getOrganizations.elements;
                    grdGetOrganization.DataBind();
                    btnGetCompanyUgcPosts.Visible = true;
                    btnGetCompanyShares.Visible = true;
                    return null;
                },
                HandleErrorResponse
                );

        }

        #region NOT WORKING: GETTING Resource ugcPosts/shares does not exist Status: 404
        /*
         * Not working as of now
         * Getting Error: Resource ugcPosts does not exist Status: 404
         */
        protected void btnGetPosts_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(lblAccessToken.Text) && !String.IsNullOrEmpty(lblUserID.Text))
            {
                string URN = String.Format("urn:li:person:{0}", lblUserID.Text);
                string URL = "https://api.linkedin.com/v2/ugcPosts?" + HttpUtility.UrlEncode("q=authors&authors=List(" + URN + ")");
                LinkedInLibrary.BaseRequest.Get<GetUgcPosts, LinkedInErrorResponse>(
                    URL,
                    lblAccessToken.Text,
                    (GetUgcPosts posts) =>
                    {
                        return null;
                    },
                    HandleErrorResponse,
                    null,
                    false
                    );

                //using (HttpClient client = new HttpClient())
                //{
                //    try
                //    {
                //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lblAccessToken.Text);
                //        client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");

                //        string URN = String.Format("urn:li:person:{0}", lblUserID.Text);
                //        URN = URN.Replace(":", "%3A");
                //        string URL = "https://api.linkedin.com/v2/ugcPosts?q=authors&authors=List(" + URN + ")";

                //        HttpResponseMessage response = client.GetAsync(URL).Result;
                //        if (response.IsSuccessStatusCode)
                //        {
                //            string responseBody = response.Content.ReadAsStringAsync().Result;
                //        }
                //        else
                //        {
                //            //ErrorResponseHandler(response.Content.ReadAsStringAsync().Result);
                //        }

                //    }
                //    catch (HttpRequestException ex)
                //    {
                //        var s = ex.Data;
                //        Console.WriteLine("\nException Caught!");
                //        Console.WriteLine("Message :{0} ", ex.Message);
                //    }
                //}
            }
        }
        protected void btnGetCompanyPosts_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(lblAccessToken.Text) && !String.IsNullOrEmpty(GetSelectedOrganization()))
            {
                string URN = String.Format("urn:li:organization:{0}", GetSelectedOrganization());
                string URL = "https://api.linkedin.com/v2/ugcPosts?" + HttpUtility.UrlEncode("q=authors&authors=List(" + URN + ")");
                LinkedInLibrary.BaseRequest.Get<GetUgcPosts, LinkedInErrorResponse>(
                    URL,
                    lblAccessToken.Text,
                    (GetUgcPosts posts) =>
                    {
                        return null;
                    },
                    HandleErrorResponse,
                    null,
                    false
                    );
            }
        }

        /*
         * Not working as of now
         * Getting Error: Resource shares does not exist Status: 404
         */
        protected void btnGetSharePosts_Click(object sender, EventArgs e)
        {
            string URN = String.Format("urn:li:person:{0}", lblUserID.Text);
            string URL = "https://api.linkedin.com/v2/shares?" + HttpUtility.UrlEncode(string.Format("q=owners&owners={0}&sharesPerOwner=2", URN));
            LinkedInLibrary.BaseRequest.Get<object, LinkedInErrorResponse>(
                URL,
                lblAccessToken.Text,
                (object obj) =>
                {
                    return null;
                },
                HandleErrorResponse,
                null,
                false
                );
            //using (HttpClient client = new HttpClient())
            //{
            //    try
            //    {
            //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lblAccessToken.Text);
            //        string URN = String.Format("urn:li:person:{0}", lblUserID.Text);
            //        HttpResponseMessage response = client.GetAsync(
            //            String.Format("https://api.linkedin.com/v2/shares?q=owners&owners={0}&sharesPerOwner=100", URN)
            //            ).Result;
            //        if (response.IsSuccessStatusCode)
            //        {
            //            string responseBody = response.Content.ReadAsStringAsync().Result;

            //        }
            //        else
            //        {
            //            ErrorResponseHandler(response.Content.ReadAsStringAsync().Result);
            //        }
            //    }
            //    catch (HttpRequestException ex)
            //    {
            //        Console.WriteLine("\nException Caught!");
            //        Console.WriteLine("Message :{0} ", ex.Message);
            //    }
            //}
        }
        protected void btnGetCompanySharePosts_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GetSelectedOrganization()))
            {
                string URN = String.Format("urn:li:organization:{0}", GetSelectedOrganization());
                string URL = "https://api.linkedin.com/v2/shares?" + HttpUtility.UrlEncode(string.Format("q=owners&owners={0}&sharesPerOwner=2", URN));
                LinkedInLibrary.BaseRequest.Get<object, LinkedInErrorResponse>(
                    URL,
                    lblAccessToken.Text,
                    (object obj) =>
                    {
                        return null;
                    },
                    HandleErrorResponse,
                    null,
                    false
                    );
            }
        }

        #endregion

        #region Get Single Post
        protected void btnGetPost_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblShareURN.Text))
            {
                LinkedInLibrary.BaseRequest.Get<LinkedInTextShareRequest, LinkedInErrorResponse>(
                    LinkedInLibrary.Constants.URLs.GetPost(lblShareURN.Text).Replace(":", "%3A"),
                    lblAccessToken.Text,
                    (LinkedInTextShareRequest share) =>
                    {
                        lblGetPostText.Text = share.specificContent.shareContent.shareCommentary.text;
                        lblGetPostFullResponseString.Text = JsonConvert.SerializeObject(share);
                        divGetPost.Visible = true;
                        return null;
                    },
                    HandleErrorResponse
                    );
            }
        }
        #endregion

        private string GetSelectedOrganization()
        {
            List<string> organization_id = new List<string>();
            foreach (System.Web.UI.WebControls.GridViewRow row in grdGetOrganization.Rows)
            {
                if (row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as System.Web.UI.WebControls.CheckBox);
                    if (chkRow.Checked)
                    {
                        //string name = row.Cells[1].Text;
                        //string country = (row.Cells[2].FindControl("lblCountry") as Label).Text;
                        //dt.Rows.Add(name, country);
                        organization_id.Add(row.Cells[5].Text);
                    }
                }
            }
            return String.Join("", organization_id);
        }
        #region Company Text Share
        protected void btnPostTextToCompany_Click(object sender, EventArgs e)
        {
            string organization = GetSelectedOrganization();
            if (!String.IsNullOrEmpty(lblAccessToken.Text) && !String.IsNullOrEmpty(lblUserID.Text))
            {
                LinkedInTextShareRequest requestbody = new LinkedInTextShareRequest();

                requestbody.author = organization;
                requestbody.specificContent.shareContent.shareCommentary.text = txtPostTextToCompany.Text;
                LinkedInLibrary.BaseRequest.Post<LinkedInShare, LinkedInErrorResponse>(LinkedInLibrary.Constants.URLs._ugcposts,
                    lblAccessToken.Text,
                    () =>
                    {
                        string s = JsonConvert.SerializeObject(requestbody);
                        return new StringContent(s, Encoding.UTF8, "application/json");
                    },
                    (LinkedInShare share) =>
                    {
                        divError.Visible = false;
                        divSuccess.Visible = true;
                        lblShareURN.Text = share.ShareURN;
                        return null;
                    },
                    HandleErrorResponse);
            }
        }
        #endregion

        #region Company URL share
        protected void btnPostUrlToCompany_Click(object sender, EventArgs e)
        {
            string organization = GetSelectedOrganization();
            LinkedInLibrary.BaseRequest.Post<LinkedInShare, LinkedInErrorResponse>(
                LinkedInLibrary.Constants.URLs._ugcposts,
                lblAccessToken.Text,
                () =>
                {
                    LinkedInTextShareRequest requestbody = new LinkedInTextShareRequest();
                    requestbody.author = organization;
                    requestbody.specificContent.shareContent.shareCommentary.text = txtPostUrlToCompanyComment.Text;
                    requestbody.specificContent.shareContent.shareMediaCategory = "ARTICLE";
                    Media media = new Media();
                    media.description.text = txtPostUrlToCompanyDescription.Text;
                    media.originalUrl = txtPostUrlToCompanyURL.Text;
                    media.title.text = txtPostUrlToCompanyTitle.Text;
                    requestbody.specificContent.shareContent.media = new List<Media>()
                    {
                        media
                    };
                    string s = JsonConvert.SerializeObject(requestbody);
                    return new StringContent(s, Encoding.UTF8, "application/json");
                },
                (LinkedInShare share) =>
                {
                    divError.Visible = false;
                    divSuccess.Visible = true;
                    lblShareURN.Text = share.ShareURN;
                    btnGetPost.Visible = true;
                    return null;
                },
                HandleErrorResponse
                );
        }
        #endregion

        #region Company Image Share
        protected void btnPostImageToCompany_Click(object sender, EventArgs e)
        {
            if (fileUploadPostImageToCompany.HasFile)
            {
                string fileAddress = "";
                try
                {
                    fileAddress = SaveFile(fileUploadPostImageToCompany.PostedFile);
                }
                catch (Exception ex)
                {
                    fileAddress = "";
                }
                if (!string.IsNullOrEmpty(fileAddress))
                {
                    RequestUploadUrlResponse uploadDetails = null;
                    if (RequestUploadUrl(out uploadDetails))
                    {
                        //string imagepath = SaveFile(fileUpload.PostedFile);
                        if (UploadImageBinaryFile(uploadDetails, fileAddress))
                        {
                            CreateImageShareRequest requestbody = new CreateImageShareRequest();
                            requestbody.author = GetSelectedOrganization();
                            Media media = new Media();
                            media.description.text = ""; //does not render on image post
                            media.title.text = ""; //does not render on image post
                            media.media = uploadDetails.value.asset;
                            requestbody.specificContent.shareContent.shareCommentary.text = txtPostImageToCompany.Text;
                            requestbody.specificContent.shareContent.media = new List<Media>() { media };

                            LinkedInLibrary.BaseRequest.Post<LinkedInShare, LinkedInErrorResponse>(
                                LinkedInLibrary.Constants.URLs._ugcposts,
                                lblAccessToken.Text,
                                () =>
                                {
                                    string s = JsonConvert.SerializeObject(requestbody);
                                    return new StringContent(s, Encoding.UTF8, "application/json");
                                },
                                (LinkedInShare share) =>
                                {
                                    divError.Visible = false;
                                    divSuccess.Visible = true;
                                    lblShareURN.Text = share.ShareURN;
                                    btnGetPost.Visible = true;
                                    return null;
                                },
                                HandleErrorResponse
                                );
                        }
                    }
                }
            }
        }
        #endregion
    }
}

#region Share upload    
//using (HttpClient client = new HttpClient())
//{
//    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lblAccessToken.Text);
//    client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
//    MultipartFormDataContent form = new MultipartFormDataContent();
//    string fileAddress = @"\LinkedInAPICall\LinkedInAPICall\Content\Facebook changelog 28 29.png";
//    var stream = new FileStream(fileAddress, FileMode.Open);
//    HttpContent content = new StreamContent(stream);
//    form.Add(content, "fileupload", "image.png");
//    HttpResponseMessage response = client.PostAsync(
//            "https://api.linkedin.com/media/upload",
//            form
//    ).Result;
//    if (response.IsSuccessStatusCode)
//    {
//        string responseBody = response.Content.ReadAsStringAsync().Result;
//        UploadRichMediaResponse richmedia = JsonConvert.DeserializeObject<UploadRichMediaResponse>(responseBody);
//    }
//    else
//    {
//        ErrorResponseHandler(response.Content.ReadAsStringAsync().Result);
//    }
//}
//    lblErrorMessage.Text = "Image saved";
//}
#endregion