using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LinkedInLibrary.Models
{
    [DataContract]
    public class GetUgcPosts
    {
        [DataMember]
        public List<LinkedInTextShareRequest> elements {get;set;}
        [DataMember]
        public Paging paging { get; set; }

        public GetUgcPosts()
        {
            elements = new List<LinkedInTextShareRequest>();
            paging = new Paging();
        }
    }

    [DataContract]
    public class Paging
    {
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public List<string> links { get; set; }
        [DataMember]
        public int start { get; set; }

        public Paging()
        {
            links = new List<string>();
        }
    }

    [DataContract]
    public class LinkedInTextShareRequest
    {
        [DataMember]
        public string author { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public Created created { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public Created lastModified { get; set; }
        [DataMember]
        public string lifecycleState { get; set; }
        [DataMember]
        public SpecificContent specificContent { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public string ugcOrigin { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public string versionTag { get; set; }
        [DataMember]
        public  Visibility visibility { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public string origin { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public Distribution distribution { get; set; }
        [DataMember(EmitDefaultValue = false, Name = "contentCertificationRecord")]
        public string contentCertificationRecordJsonString { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public string clientApplication { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public long firstPublishedAt { get; set; }

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
        [DataMember(EmitDefaultValue =false)]
        public ShareFeatures shareFeatures { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public List<Media> media { get; set; }

        public ShareContent()
        {
            shareMediaCategory = "NONE";
            shareCommentary = new ShareCommentary();
            shareFeatures = new ShareFeatures();
        }
    }

    [DataContract]
    public class ShareCommentary
    {
        [DataMember]
        public string text { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public List<string> attributes { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public string inferredLocale { get; set; }


        public ShareCommentary()
        {
            attributes = new List<string>();
        }
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
        [DataMember(EmitDefaultValue = false)]
        public string media { get; set; }
        [DataMember]
        public Title title { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string originalUrl { get; set; }

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

    [DataContract]
    public class Created
    {
        [DataMember]
        public string actor { get; set; }
        [DataMember]
        public long time { get; set; }
    }

    [DataContract]
    public class ShareFeatures
    {
        [DataMember(EmitDefaultValue =false)]
        public List<string> hashtags { get; set; }

        public ShareFeatures()
        {
        }
    }

    [DataContract]
    public class Distribution
    {
        [DataMember(EmitDefaultValue =false)]
        public string feedDistribution { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<object> externalDistributionChannels { get; set; }
        [DataMember(EmitDefaultValue =false)]
        public bool distributedViaFollowFeed { get; set; }

        public Distribution()
        {
        }
    }
}