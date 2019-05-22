using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LinkedInLibrary.Models
{
    [DataContract]
    public class RequestUploadUrlRequest
    {
        [DataMember]
        public RegisterUploadRequest registerUploadRequest {get;set;}
        public RequestUploadUrlRequest()
        {
            registerUploadRequest = new RegisterUploadRequest();
        }
    }

    [DataContract]
    public class RegisterUploadRequest
    {
        [DataMember]
        public List<string> recipes { get; set; }
        [DataMember]
        public string owner { get; set; }
        [DataMember]
        public List<ServiceRelationships> serviceRelationships { get; set; }

        public RegisterUploadRequest(){
            recipes = new List<string>();
            recipes.Add("urn:li:digitalmediaRecipe:feedshare-image");

            serviceRelationships = new List<ServiceRelationships>();
            var sr = new ServiceRelationships();
            sr.relationshipType = "OWNER";
            sr.identifier = "urn:li:userGeneratedContent";
            serviceRelationships.Add(sr);
        }
    }

    [DataContract]
    public class ServiceRelationships
    {
        [DataMember]
        public string relationshipType { get; set; }
        [DataMember]
        public string identifier { get; set; }
    }
}