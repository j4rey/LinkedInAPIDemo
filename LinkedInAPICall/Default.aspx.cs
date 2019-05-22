using System;
using System.Web.UI;

namespace LinkedInAPICall
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Go(object sender, EventArgs e)
        {
            Random r = new Random(new Guid().GetHashCode());
            string s = Convert.ToString(r.Next(0, 99));
            Response.Cookies["state"].Value = s;
            Response.Redirect("/linkedinaccts.aspx");
        }

        /*Model Test Purposes*/
        protected void Create(object sender, EventArgs e)
        {
            //Models.LinkedInTextShareRequest l = new Models.LinkedInTextShareRequest();
            //l.author = "1234";
            //l.specificContent.shareContent.shareCommentary.text = "abc";
            //string s = JsonConvert.SerializeObject(l);

            //string s = "{\"id\":\"urn: li: share: 12345677775\"}";
            //Models.LinkedInShare l = JsonConvert.DeserializeObject<Models.LinkedInShare>(s);
        }
    }
}