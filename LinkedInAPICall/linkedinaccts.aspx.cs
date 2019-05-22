using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Net.Http;
using Newtonsoft.Json;
using LinkedInLibrary.Models;

namespace LinkedInAPICall
{
    public partial class linkedinaccts : Page
    {
        const string CLIENTID = "";
        const string CLIENTSECRET = "";
        const string REDIRECTURL = "http://localhost:1862/linkedinaccts.aspx";
        
        string currentState = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if(Request.QueryString["error"] != null)
                {
                    HandleAuthError();
                    return;
                }
                else if (Request.QueryString["code"] != null)
                {
                    if (Request.Cookies["state"].Value == Convert.ToString(Request.QueryString["state"]))
                    {
                        HandleAuthSuccess(Convert.ToString(Request.QueryString["code"]));
                    }
                    return;
                }
                else if(Request.QueryString["access_token"] != null){
                    return;
                }
                CallAuthURL();
            }
        }

        private string GetState()
        {
            if (Request.Cookies["state"] != null)
            {
               return Request.Cookies["state"].Value.ToString();
            }
            if (currentState != "")
            {
            return currentState;
            }
            throw new Exception("No State");
        }

        private void CallAuthURL()
        {
            string url = LinkedInLibrary.Constants.URLs.AUTH_URL(CLIENTID, REDIRECTURL, GetState(), String.Join(" ", LinkedInLibrary.Constants.Scopes.AllScopes));
            Response.Redirect(url);
        }

        private void HandleAuthSuccess(string code)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("redirect_uri", REDIRECTURL),
                        new KeyValuePair<string, string>("client_id", CLIENTID),
                        new KeyValuePair<string, string>("client_secret", CLIENTSECRET),
                    });

                    HttpResponseMessage response = client.PostAsync("https://www.linkedin.com/oauth/v2/accessToken/", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        var data = JsonConvert.DeserializeObject<LinkedInAccessTokenSuccess>(responseBody);
                        Response.Cookies["access_token"].Value = Convert.ToString(data.access_token);
                        Response.Cookies["expires_in"].Value = Convert.ToString(data.expires_in);
                        Response.Redirect("./linkedinprofile.aspx");
                    }
                    else
                    {
                        string errorResponse = response.Content.ReadAsStringAsync().Result;
                        var errorobject = JsonConvert.DeserializeObject<LinkedInErrorResponse>(errorResponse);
                        divError.Visible = true;
                        lblServiceErrorCode.Text = Convert.ToString(errorobject.serviceErrorCode);
                        lblErrorMessage.Text = errorobject.message;
                        lblErrorStatus.Text = Convert.ToString(errorobject.status);
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

        private void HandleAuthError()
        {
            throw new NotImplementedException();
        }
    }
}