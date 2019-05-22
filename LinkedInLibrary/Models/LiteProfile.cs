using System;

namespace LinkedInLibrary.Models
{
    public class LiteProfile
    {
        public String id { get; set; }
        public Name firstName { get; set; }
        public Name lastName { get; set; }

        public LiteProfile()
        {
            firstName = new Name();
            lastName = new Name();
        }
    }
    public class Name
    {
        public localized localized { get; set; }
        public preferredLocale preferredLocale { get; set; }

        public Name()
        {
            localized = new localized();
            preferredLocale = new preferredLocale();
        }
    }
    
    public class localized
    {
         public string en_US { get; set; }
    }
    public class preferredLocale
    {
        public string country { get; set; }
        public string language { get; set; }
    }
}