using Leticia.Classes.Output;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace Leticia.Classes
{
    public class AppStatic
    {
        private static CountryEnum currentCountry;

        public static CountryEnum CurrentCountry 
        {
            get
            {
                if (currentCountry == 0)
                {
                    return CountryEnum.Unknown;
                }
                else
                {
                    return currentCountry;
                }
            }
            set
            {
                currentCountry = value;
            }
        }

        public static string rootDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string rootXMLDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory+"XML\\";
            }
        }

        public static string rootDirectoryMenuXML
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory+"XML\\menu\\";
            }
        }

        public static string rootDirectoryFootherXML
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "XML\\foother\\";
            }
        }

        public static string GetUserIP
        {
            get
            {
                return System.Web.HttpContext.Current.Request.UserHostAddress;
            }
        }

        private static string Separator
        {
            get
            {
                return "------------------------------------------";
            }
        }

        public static void WriteInFile(string message,string country)
        {
            string fileDestination = HostingEnvironment.ApplicationPhysicalPath+"\\History\\history.txt";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileDestination, true))
            {
                file.WriteLine(Separator);

                file.WriteLine("On date: " + DateTime.Now);

                file.WriteLine("User IP: " + message);

                file.WriteLine("Country: " + country);

                file.WriteLine(Separator);

                file.WriteLine();
                file.WriteLine();
            }
        }

        public static CountryEnum GetCountry(string IP)
        {
                try
                {
                    string url = "http://www.geoplugin.net/json.gp?ip=" + IP + "";

                    string json = string.Empty;

                    using (var wb = new WebClient())
                    {
                        json = wb.DownloadString(url);
                    }

                    RootObject ipInformation = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);

                    WriteInFile(IP, ipInformation.geoplugin_countryName);

                    if (ipInformation.geoplugin_countryName.ToLower() == "germany")
                    {
                        return CountryEnum.Germany;
                    }
                    else if (ipInformation.geoplugin_countryName.ToLower() == "austria")
                    {
                        return CountryEnum.Austria;
                    }
                    else if (ipInformation.geoplugin_countryName.ToLower() == "switzerland")
                    {
                        return CountryEnum.Germany;
                    }
                    else if (ipInformation.geoplugin_countryName.ToLower() == "bulgaria")
                    {
                        return CountryEnum.Bulgaria;
                    }
                    return CountryEnum.Other;
                }
                catch (Exception ex)
                {
                    WriteInFile(ex.Message,"error");

                    return CountryEnum.Other;
                }              
        }

        public static bool UseTest()
        {
            return ConfigurationManager.AppSettings["useTest"] == "true" ? true : false; ;
        }

        public static string TestIP
        {
            get
            {
                return ConfigurationManager.AppSettings["IP"];
            }
        }
    }
}