using System.Collections.Generic;
using System.Runtime.Serialization;
namespace LinkedInLibrary.Models
{
    [DataContract]
    public class CheckUploadStatusResponse
    {
        [DataMember]
        public List<ServiceRelationships> serviceRelationships { get; set; }
        [DataMember]
        public List<Recipes> recipes { get; set; }
        [DataMember]
        public string mediaTypeFamily { get; set; }
        [DataMember]
        public long created { get; set; }
        [DataMember]
        public long lastModified { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string status { get; set; }
    }

    [DataContract]
    public class Recipes
    {
        [DataMember]
        public string recipe { get; set; }
        [DataMember]
        public string status { get; set; }
    }
}
