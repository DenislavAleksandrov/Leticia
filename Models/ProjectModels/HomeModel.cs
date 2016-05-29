using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Leticia.Models.ProjectModels
{
    public class HomeModel : ViewModelBase
    {
        public Dictionary<string, string> viewInfo;

        public HomeModel()
        {
            viewInfo = new Dictionary<string, string>();
        }
    }
}