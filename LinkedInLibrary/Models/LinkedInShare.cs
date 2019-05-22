using System;

namespace LinkedInLibrary.Models
{
    public class LinkedInShare
    {
        public string id { get; set; }

        public void setShareID(string urn)
        {
            try
            {
                this.id = urn;
            }
            catch (Exception)
            {
                throw new Exception("Invalid ShareURN");
            }
        }

        public string ShareURN
        {
            get {
                return id;
                //return String.Format("urn: li:share:{0}", id);
            }
        }
    }
}