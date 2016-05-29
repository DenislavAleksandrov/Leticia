using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Leticia.Classes
{
    public enum CountryEnum
    {
        Bulgaria = 1,
        Germany = 2,
        England = 4,
        Austria = 8,
        Zwizerland = 16,
        Other = 32,
        Unknown = 64
    }

    public enum ControllerFunctions
    {
        Index = 1,
        About = 2,
        Contacts = 3,
        PrivacyPolicy = 4,
        Sofas = 5,
        References = 6,
        Armchair = 7,
        CornerSofa = 8,
        AboutUs = 9,
        Portfolio = 10,
        Services = 11
    }

    public enum NeedMetaTags
    { 
        Need = 1,
        NotNeed = 2
    }
}