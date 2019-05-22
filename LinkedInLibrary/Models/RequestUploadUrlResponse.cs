using System;
using System.Runtime.Serialization;

namespace LinkedInLibrary.Models
{
    [DataContract]
    public class RequestUploadUrlResponse
    {
        [DataMember]
        public UploadValue value { get; set; }
        public RequestUploadUrlResponse()
        {
            value = new UploadValue();
        }
    }

    [DataContract]
    public class UploadValue
    {
        [DataMember]
        public UploadMechanism uploadMechanism { get; set; }
        [DataMember]
        public string mediaArtifact { get; set; }
        [DataMember]
        public string asset { get; set; }

        public string GetAssetID
        {
            get
            {
                var split = asset.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 4)
                {
                    return split[3];
                }
                return "";
            }
        }

        public UploadValue()
        {
            uploadMechanism = new UploadMechanism();
        }
    }

    [DataContract]
    public class UploadMechanism
    {
        [DataMember(Name = "com.linkedin.digitalmedia.uploading.MediaUploadHttpRequest")]
        public MediaUploadHttpRequest mediaUploadHttpRequest { get; set; }
        public UploadMechanism()
        {
            mediaUploadHttpRequest = new MediaUploadHttpRequest();
        }
    }

    [DataContract]
    public class MediaUploadHttpRequest
    {
        [DataMember]
        public object headers { get; set; }
        [DataMember]
        public string uploadUrl { get; set; }
    }
}