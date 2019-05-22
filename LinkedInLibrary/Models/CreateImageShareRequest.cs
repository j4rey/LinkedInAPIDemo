using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LinkedInLibrary.Models
{
    [DataContract]
    public class CreateImageShareRequest
    {
        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string lifecycleState { get; set; }
        [DataMember]
        public SpecificContent specificContent { get; set; }
        [DataMember]
        public Visibility visibility { get; set; }

        public CreateImageShareRequest()
        {
            lifecycleState = "PUBLISHED";
            specificContent = new SpecificContent();
            specificContent.shareContent.shareMediaCategory = "IMAGE";
            specificContent.shareContent.media = new List<Media>();
            visibility = new Visibility();
        }
    }
}