using Leticia.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Leticia.Models.ProjectModels
{
    public abstract class ViewModelBase
    {
        public CountryEnum Country;

        public string metaTags;

        public Dictionary<string, string> menu = new Dictionary<string, string>();

        public Dictionary<string, string> foother = new Dictionary<string, string>();
    }
}