using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LinkedInLibrary.Models
{
    [DataContract]
    public class LinkedInTextShareRequest
    {
        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string lifecycleState { get; set; }
        [DataMember]
        public SpecificContent specificContent { get; set; }
        [DataMember]
        public  Visibility visibility { get; set; }
        
        public LinkedInTextShareRequest()
        {
            lifecycleState = "PUBLISHED";
            specificContent = new SpecificContent();
            visibility = new Visibility();
        }
    }

    [DataContract]
    public class SpecificContent
    {
        [DataMember(Name = "com.linkedin.ugc.ShareContent")]
        public ShareContent shareContent { get; set; }
        public SpecificContent()
        {
            shareContent = new ShareContent();
        }
    }

    [DataContract]
    public class ShareContent
    {
        [DataMember]
        public ShareCommentary shareCommentary { get; set; }
        [DataMember]
        public string shareMediaCategory { get; set; }
        [DataMember]
        public List<Media> media { get; set; }

        public ShareContent()
        {
            shareMediaCategory = "NONE";
            shareCommentary = new ShareCommentary();
        }
    }

    [DataContract]
    public class ShareCommentary
    {
        [DataMember]
        public string text { get; set; }
    }

    [DataContract]
    public class Visibility
    {
        [DataMember(Name = "com.linkedin.ugc.MemberNetworkVisibility")]
        public string memberNetworkVisibility { get; set; }

        public Visibility()
        {
            memberNetworkVisibility = "PUBLIC";
        }
    }

    [DataContract]
    public class Media
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public Description description { get; set; }
        [DataMember]
        public string media { get; set; }
        [DataMember]
        public Title title { get; set; }

        public Media()
        {
            description = new Description();
            title = new Title();
            status = "READY";
        }
    }

    [DataContract]
    public class Description
    {
        [DataMember]
        public string text { get; set; }
    }

    [DataContract]
    public class Title
    {
        [DataMember]
        public string text { get; set; }
    }
}