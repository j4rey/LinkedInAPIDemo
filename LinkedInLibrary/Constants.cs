using System;

namespace LinkedInLibrary
{
    public class Constants
    {
        public class URLs
        {
            public static readonly string _authurl = "https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={0}&redirect_uri={1}&state={2}&scope={3}";
            public static readonly string _getuserprofile = "https://api.linkedin.com/v2/me";
            public static readonly string _getorganization = "https://api.linkedin.com/v2/organizationalEntityAcls";
            public static readonly string _ugcposts = "https://api.linkedin.com/v2/ugcPosts";
            public static readonly string _registerupload = "https://api.linkedin.com/v2/assets?action=registerUpload";
            private static readonly string _checkuploadstatus = "https://api.linkedin.com/v2/assets/{0}";

            public static string AUTH_URL(string client_id, string redirect_uri, string state, string scope)
            {
                return string.Format(_authurl, client_id, redirect_uri, state, scope);
            }

            public static string GetUSERPROFILE(string queryparams = "projection=(id,firstName,lastName)")
            {
                return !string.IsNullOrEmpty(queryparams) ? string.Format("{0}?{1}", _getuserprofile, queryparams) : _getuserprofile;
            }

            public static string GetOrganizations(string queryparams = "q=roleAssignee&role=ADMINISTRATOR&projection=(elements*(*,roleAssignee~(localizedFirstName, localizedLastName), organizationalTarget~(localizedName)))")
            {
                return !string.IsNullOrEmpty(queryparams) ? string.Format("{0}?{1}", _getorganization, queryparams) : _getorganization;
            }

            public static string CheckUploadStatus(string asset_id)
            {
                return !string.IsNullOrEmpty(asset_id) ? String.Format(_checkuploadstatus, asset_id) : _checkuploadstatus;
            }
        }

        public class Scopes
        {

            public static readonly string r_emailaddress = "r_emailaddress";
            //"r_ads",
            public static readonly string w_organization_social = "w_organization_social";
            //"rw_ads",
            public static readonly string r_basicprofile = "r_basicprofile";
            public static readonly string r_liteprofile = "r_liteprofile";
            //"r_fullprofile",
            //"r_ads_reporting",
            public static readonly string r_organization_social = "r_organization_social";
            public static readonly string rw_organization_admin = "rw_organization_admin";
            public static readonly string w_member_social = "w_member_social";
            //, "w_share", "rw_company_admin"

            public static string[] AllScopes
            {
                get
                {
                    return new string[]
                    {
                        r_emailaddress,
                        w_organization_social,
                        r_basicprofile,
                        r_liteprofile,
                        r_organization_social,
                        rw_organization_admin,
                        w_member_social
                    };
                }
            }
        }
        
    }
}
