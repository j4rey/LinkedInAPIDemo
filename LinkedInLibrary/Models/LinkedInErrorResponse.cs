namespace LinkedInLibrary.Models
{
    public class LinkedInErrorResponse
    {
        public long serviceErrorCode { get; set; }
        public string message { get; set; }
        public int status { get; set; }
    }
}