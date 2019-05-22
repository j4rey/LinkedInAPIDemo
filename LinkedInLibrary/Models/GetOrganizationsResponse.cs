using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LinkedInLibrary.Models
{
    [DataContract]
    public class GetOrganizationsResponse
    {
        [DataMember]
        public List<OrganizationalElements> elements { get; set; }
    }

    [DataContract]
    public class OrganizationalElements
    {
        [DataMember(Name = "organizationalTarget~")]
        public OrganizationalTarget _organizationalTarget { get; set; }
        [DataMember]
        public string role { get; set; }
        [DataMember]
        public string roleAssignee { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember(Name= "roleAssignee~")]
        public RoleAssignee _roleAssignee { get; set; }
        [DataMember]
        public string organizationalTarget { get; set; }
    }

    [DataContract]
    public class RoleAssignee
    {
        [DataMember]
        public string localizedLastName { get; set; }
        [DataMember]
        public string localizedFirstName { get; set; }
    }

    [DataContract]
    public class OrganizationalTarget
    {
        [DataMember]
        public string localizedName { get; set; }
    }
}