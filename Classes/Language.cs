using Leticia.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Leticia.Classes
{
    public class Language:Attribute
    {
        public string LanguageName;
        public CountryEnum Country;
        public ControllerFunctions functionName;

        public Language(ControllerFunctions ControllerFunctionName)
        {
            this.functionName = ControllerFunctionName;
        }

        public HomeModel Init()
        {
            HomeModel result = new HomeModel();

            if (AppStatic.UseTest())
            {
                this.Country = AppStatic.GetCountry(AppStatic.TestIP);

                AppStatic.CurrentCountry = this.Country;

                CountryEnum countryTest = CountryEnum.Bulgaria;

                result.Country = countryTest;

                result.menu = this.PrepareDictionary(this.LoadMenuDictionary(countryTest));

                result.viewInfo = this.PrepareDictionary(this.LoadViewInfoDictionary(this.functionName, countryTest));

                List<string> otherLanguagesKeyWord = this.PrepareList(this.functionName, countryTest);

                if (functionName == ControllerFunctions.Contacts ||
                    functionName == ControllerFunctions.Index || functionName == ControllerFunctions.References ||
                    functionName == ControllerFunctions.AboutUs || functionName == ControllerFunctions.Services)
                {
                    result.metaTags = this.LoadMetaString(this.functionName);
                }
                else
                {
                    foreach (KeyValuePair<string, string> item in result.viewInfo)
                    {
                        result.metaTags += item.Value;

                        result.metaTags += ",";
                    }

                    if (otherLanguagesKeyWord != null && otherLanguagesKeyWord.Count > 0)
                    {
                        foreach (string item in otherLanguagesKeyWord)
                        {
                            result.metaTags += item;

                            result.metaTags += ",";
                        }
                    }
                }

                result.foother = this.PrepareDictionary(this.LoadFootherDictionary(countryTest));
            }
            else
            {
                this.Country = AppStatic.GetCountry(AppStatic.GetUserIP);

                result.Country = this.Country;

                AppStatic.CurrentCountry = this.Country;

                result.menu = this.PrepareDictionary(this.LoadMenuDictionary(this.Country));

                result.viewInfo = this.PrepareDictionary(this.LoadViewInfoDictionary(this.functionName, this.Country));

                List<string> otherLanguagesKeyWord = this.PrepareList(this.functionName, this.Country);

                if (functionName == ControllerFunctions.Contacts || 
                    functionName == ControllerFunctions.Index || functionName == ControllerFunctions.References|| 
                    functionName == ControllerFunctions.AboutUs || functionName == ControllerFunctions.Services)
                {
                    result.metaTags = this.LoadMetaString(this.functionName);
                }
                else
                {
                    foreach (KeyValuePair<string,string> item in result.viewInfo)
                    {
                        result.metaTags += item.Value;

                        result.metaTags += ",";
                    }

                    if (otherLanguagesKeyWord != null && otherLanguagesKeyWord.Count > 0)
                    {
                        foreach (string item in otherLanguagesKeyWord)
                        {
                            result.metaTags += item;

                            result.metaTags += ",";
                        }
                    }
                }

                result.foother = this.PrepareDictionary(this.LoadFootherDictionary(this.Country));
            }

            return result;
        }
       
        private string LoadMetaString(ControllerFunctions functionName)
        {
            StringBuilder str = new StringBuilder();

            XmlTextReader reader = null;

                reader = new XmlTextReader(this.LoadMetaTagsDictionary(functionName));

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "keyWordsAboutUs")
                        {
                            return ExtractMetaTags(functionName, reader.ReadSubtree());
                        }
                    }
                }           
            
            return str.ToString();
        }

        private List<string> PrepareList(ControllerFunctions functionName, CountryEnum country)
        {
            List<string> result = new List<string>();

            Dictionary<string, string> otherTwoLanguagesKeyWords = new Dictionary<string, string>();

            if (functionName == ControllerFunctions.CornerSofa || functionName == ControllerFunctions.Sofas ||
                functionName == ControllerFunctions.Armchair || functionName == ControllerFunctions.Portfolio)
            {
                List<string> files = GetOtherTwoLanguageFileDestinations(functionName, country);

                foreach (var item in files)
                {
                    Dictionary<string, string> keyWords = PrepareDictionary(item);

                    foreach (KeyValuePair<string, string> word in keyWords)
                    {
                        result.Add(word.Value);
                    }
                }
            }          

            return result;
        }

        private Dictionary<string, string> PrepareDictionary(string doc)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (doc != string.Empty)
            {
                XmlTextReader reader = new XmlTextReader(doc);

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "menu")
                        {
                            result = this.MenuDictinary(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "foother")
                        {
                            result = this.FootherDictinary(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "index")
                        {
                            result = this.IndexDictionary(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "policy")//privacipolicy
                        {
                            result = this.PolicyDictionary(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "contacts")//contacts page
                        {
                            result = this.ContactDictionary(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "sofas")//sofas
                        {
                            result = this.SofaDictionary(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "maintext")//references
                        {
                            result = this.ReferenceDictionary(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "armchair")//armchair
                        {
                            result = this.Armchair(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "cornersofa")//cornersofa
                        {
                            result = this.CornerSofa(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "text")//aboutus
                        {
                            result = this.AboutUs(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "portfolio")//aboutus
                        {
                            result = this.PorfolioDictionary(reader.ReadSubtree());

                            break;
                        }
                        else if (reader.Name == "services")
                        {
                            if (this.Country != CountryEnum.Bulgaria)
                            {
                                result = this.ServicesDictionary(reader.ReadSubtree());
                            }
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private string ExtractMetaTags(ControllerFunctions functionName,XmlReader metaTree)
        {
            StringBuilder str = new StringBuilder();

            if (functionName == ControllerFunctions.AboutUs || functionName == ControllerFunctions.Index || functionName == ControllerFunctions.References)
            {
                while (metaTree.Read() && metaTree.IsStartElement())
                {
                    if (metaTree.Name.ToLower() == "first")
                    {
                        str.Append(metaTree.ReadElementContentAsString());
                    }
                    else if (metaTree.Name.ToLower() == "second")
                    {
                        str.Append(metaTree.ReadElementContentAsString());
                    }
                    else if (metaTree.Name.ToLower() == "third")
                    {
                        str.Append(metaTree.ReadElementContentAsString());
                    }
                    else if (metaTree.Name.ToLower() == "mebelbg")
                    {
                        str.Append(metaTree.ReadElementContentAsString());
                    }
                    else if (metaTree.Name.ToLower() == "mebelde")
                    {
                        str.Append(metaTree.ReadElementContentAsString());
                    }
                    else if (metaTree.Name.ToLower() == "mebelen")
                    {
                        str.Append(metaTree.ReadElementContentAsString());
                    }
                    else if (metaTree.Name.ToLower() == "bginfo")
                    {
                        str.Append(metaTree.ReadElementContentAsString());
                    }
                    else if (metaTree.Name.ToLower() == "deinfo")
                    {
                        str.Append(metaTree.ReadElementContentAsString());
                    }
                    else if (metaTree.Name.ToLower() == "eninfo")
                    {
                        str.Append(metaTree.ReadElementContentAsString());
                    }
                }
            }
            return str.ToString();
        }

        #region Dictionaries

        private Dictionary<string, string> PorfolioDictionary(XmlReader menuTree)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "zurich")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "munich")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "sofamonti")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "monti2")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ergofour")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ergo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ergosecond")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ergothird")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "hamm")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "sofa")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }                
                else if (menuTree.Name.ToLower() == "teo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "bonn")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "leticia")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "emi")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "sonya")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "elena")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "graz")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "hagen")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "eu")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }                
                else if (menuTree.Name.ToLower() == "maintextsecond")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "letica1")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "letica2")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "letica3")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "linz")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "letica4")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "letica5")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "iliyana")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "iliyana_s")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "zuricharmchair")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "maintext")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ring")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "nellys")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "nelly")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "aachen")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "aachensecond")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "monti")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "retro")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "carlo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "basel")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "paris")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ulm")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "cubo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "cubosecond")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "round")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                } 
            }

            return result;
        }

        private Dictionary<string, string> MenuDictinary(XmlReader menuTree)
        {
            Dictionary<string, string> menu = new Dictionary<string, string>();

            while (menuTree.Read()&&menuTree.IsStartElement())
            {                
               if (menuTree.Name.ToLower() == "begin")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "forus")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "materials")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "materialssecondline")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "references")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "contacts")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "textmodels")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "armchairs")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "sofas")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "cornersofas")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
               else if (menuTree.Name.ToLower() == "subtitle")
               {
                   menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
               }
            }

            return menu;
        }

        private Dictionary<string, string> FootherDictinary(XmlReader menuTree)
        {
            Dictionary<string, string> menu = new Dictionary<string, string>();

            while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "copywrite")
                {
                    menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "rights")
                {
                    menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "questions")
                {
                    menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "telefontext")
                {
                    menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "telefonvalue")
                {
                    menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "pravacipolicy")
                {
                    menu.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }          
            }
            return menu;
        }

        private Dictionary<string, string> IndexDictionary(XmlReader menuTree)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "about")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ourteam")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "services")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "whatweoffer")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "apartments")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "interiordesign")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "furniture")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "design")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "livingroom")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "designportfolio")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "bathroom")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "designportfoliobath")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "outdoor")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "portfolio")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "partners")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ourclients")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "contactus")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "giveusyourcall")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                if (menuTree.Name.ToLower() == "begin")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "forus")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "materials")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "materialssecondline")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "references")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "contacts")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "textmodels")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "armchairs")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "sofas")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "cornersofas")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
            }
            return result;
        }

        private Dictionary<string, string> PolicyDictionary(XmlReader menuTree)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "title")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "name")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "address")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "country")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "telefon")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "fax")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "email")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "www")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "first")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "second")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "third")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "fourth")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "fifth")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "sixth")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "seventh")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "eight")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "nineth")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "tenth")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "eleventh")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "twelfth")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
            }

            return result;
        }

        private Dictionary<string, string> ContactDictionary(XmlReader menuTree)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "contactaddress")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "contactform")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "title")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "person")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "street")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "tel")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "email")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "firstpart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "secondpart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "thirdpart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "formname")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "formemail")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                 else if (menuTree.Name.ToLower() == "formtext")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                 else if (menuTree.Name.ToLower() == "formtelefon")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "buttontext")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                
            }

            return result;
        }

        private Dictionary<string, string> SofaDictionary(XmlReader menuTree)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "zurich")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "cologne1")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "cologne2")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "comforto")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "comforto1")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "cologne")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "hamm")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "sofa")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ergo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "teo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "bonn")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "monti")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "leticia")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "emi")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "sonya")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "elena")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                 else if (menuTree.Name.ToLower() == "graz")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "hagen")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "eu")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "aachen")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }                
             }

            return result;
        }

        private Dictionary<string, string> ReferenceDictionary(XmlReader menuTree)
        { 
             Dictionary<string, string> result = new Dictionary<string, string>();

             while (menuTree.Read() && menuTree.IsStartElement())
             {
                 if (menuTree.Name.ToLower() == "maintext")
                 {
                     result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                 }
             }
             return result;
        }

        private Dictionary<string, string> Armchair(XmlReader menuTree)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "zurich")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if(menuTree.Name.ToLower() == "charo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if(menuTree.Name.ToLower() == "eclipse")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "munich")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "monti")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "monti2")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "maintext")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ring")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "nellys")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "nelly")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ergo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "aachen")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "monti")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "retro")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "carlo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "basel")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "paris")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ulm")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ergosecond")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "cubo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "cubosecond")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "round")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
            }
            return result;
        }

        private Dictionary<string, string> CornerSofa(XmlReader menuTree)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "letica1")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "cologne")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }                    
                else if (menuTree.Name.ToLower() == "maintextsecond")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "letica2")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "letica3")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "linz")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "ergo")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "letica4")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "letica5")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "iliyana")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "iliyana_s")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
               
            }
            return result;
        }

        private Dictionary<string, string> AboutUs(XmlReader menuTree)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "firstpart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "secondpart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "thirdpart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "fourpart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "main")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
            }
            return result;
        }

        private Dictionary<string, string> ServicesDictionary(XmlReader menuTree)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

             while (menuTree.Read() && menuTree.IsStartElement())
            {
                if (menuTree.Name.ToLower() == "firstpart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "second")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if(menuTree.Name.ToLower() == "thirdpart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if(menuTree.Name.ToLower() == "fourtpard")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if(menuTree.Name.ToLower() == "tel")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if(menuTree.Name.ToLower() == "email")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
                else if (menuTree.Name.ToLower() == "fivepart")
                {
                    result.Add(menuTree.Name, menuTree.ReadElementContentAsString());
                }
             }
            return result;
        }

        #region MetaTags

        private List<string> AboutUsMetaWords(string path)
        {
            List<string> result = new List<string>();

            return result;
        }

        #endregion

        #endregion

        #region LOAD
        private string LoadMenuDictionary(CountryEnum Country)
        {
            try
            {              
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return "" + AppStatic.rootDirectoryMenuXML + "DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return "" + AppStatic.rootDirectoryMenuXML + "BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return "" + AppStatic.rootDirectoryMenuXML + "EN.xml";
                }
            }
            catch (Exception ex)
            {
                return "" + AppStatic.rootDirectoryMenuXML + "EN.xml";
            }
            return "" + AppStatic.rootDirectoryMenuXML + "EN.xml";
        }

        private string LoadFootherDictionary(CountryEnum Country)
        {
            try
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return "" + AppStatic.rootDirectoryFootherXML + "DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return "" + AppStatic.rootDirectoryFootherXML + "BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return "" + AppStatic.rootDirectoryFootherXML + "EN.xml";
                }
            }
            catch (Exception ex)
            {
                return "" + AppStatic.rootDirectoryFootherXML + "EN.xml";
            }
            return "" + AppStatic.rootDirectoryFootherXML + "EN.xml";
        }

        private string LoadViewInfoDictionary(ControllerFunctions functionName, CountryEnum Country)
        {
            if (functionName == ControllerFunctions.Contacts)
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return AppStatic.rootXMLDirectory+"contacts\\DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return AppStatic.rootXMLDirectory + "contacts\\BG.xml";                    
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return AppStatic.rootXMLDirectory + "contacts\\EN.xml";                     
                }
            }
            else if (functionName == ControllerFunctions.Index)
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return AppStatic.rootXMLDirectory + "index\\DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return AppStatic.rootXMLDirectory + "index\\BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return AppStatic.rootXMLDirectory + "index\\EN.xml";
                }
            }
            else if (functionName == ControllerFunctions.PrivacyPolicy)
            {
                return AppStatic.rootXMLDirectory + "privacypolicy\\privacipolicy.xml";
            }
            else if (functionName == ControllerFunctions.Sofas)
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return AppStatic.rootXMLDirectory + "sofas\\DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return AppStatic.rootXMLDirectory + "sofas\\BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return AppStatic.rootXMLDirectory + "sofas\\EN.xml";
                }
            }
            else if (functionName == ControllerFunctions.References)
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return AppStatic.rootXMLDirectory + "references\\DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return AppStatic.rootXMLDirectory + "references\\BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return AppStatic.rootXMLDirectory + "references\\EN.xml";
                }
            }
            else if (functionName == ControllerFunctions.Armchair)
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return AppStatic.rootXMLDirectory + "armchairs\\DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return AppStatic.rootXMLDirectory + "armchairs\\BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return AppStatic.rootXMLDirectory + "armchairs\\EN.xml";
                }
            }
            else if (functionName == ControllerFunctions.CornerSofa)
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return AppStatic.rootXMLDirectory + "cornersofa\\DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return AppStatic.rootXMLDirectory + "cornersofa\\BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return AppStatic.rootXMLDirectory + "cornersofa\\EN.xml";
                }
            }
            else if (functionName == ControllerFunctions.AboutUs)
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return AppStatic.rootXMLDirectory + "aboutus\\DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return AppStatic.rootXMLDirectory + "aboutus\\BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return AppStatic.rootXMLDirectory + "aboutus\\EN.xml";
                }
            }
            else if (functionName == ControllerFunctions.Portfolio)
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return AppStatic.rootXMLDirectory + "portfolio\\DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return AppStatic.rootXMLDirectory + "portfolio\\BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return AppStatic.rootXMLDirectory + "portfolio\\EN.xml";
                }
            }
            else if (functionName == ControllerFunctions.Services)
            {
                if (Country == CountryEnum.Austria || Country == CountryEnum.Germany || Country == CountryEnum.Zwizerland)
                {
                    return AppStatic.rootXMLDirectory + "services\\DE.xml";
                }
                else if (Country == CountryEnum.Bulgaria)
                {
                    return AppStatic.rootXMLDirectory + "services\\BG.xml";
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    return AppStatic.rootXMLDirectory + "services\\EN.xml";
                }
            }
            return string.Empty;
        }

        private string LoadMetaTagsDictionary(ControllerFunctions functionName)
        {
            if (functionName == ControllerFunctions.AboutUs || functionName == ControllerFunctions.Index || functionName == ControllerFunctions.Services 
                || functionName == ControllerFunctions.References || functionName == ControllerFunctions.Contacts)
            {
                return AppDomain.CurrentDomain.BaseDirectory + "XML\\aboutus\\keyWords.xml";   
            }

            return string.Empty;
        }
        #endregion

        private List<string> GetOtherTwoLanguageFileDestinations(ControllerFunctions functionName,CountryEnum country)
        {
            List<string> result = new List<string>();

            if (functionName == ControllerFunctions.Sofas)
            {
                result.Add(AppStatic.rootXMLDirectory + "sofas\\DE.xml");
                result.Add(AppStatic.rootXMLDirectory + "sofas\\BG.xml");
                result.Add(AppStatic.rootXMLDirectory + "sofas\\EN.xml");

                if (country == CountryEnum.Austria || country == CountryEnum.Germany || country == CountryEnum.Zwizerland)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "sofas\\DE.xml");
                }
                else if (country == CountryEnum.Bulgaria)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "sofas\\BG.xml");
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "sofas\\EN.xml");
                }
                return result;
            }
            else if (functionName == ControllerFunctions.CornerSofa)
            {
                result.Add(AppStatic.rootXMLDirectory + "cornersofa\\DE.xml");
                result.Add(AppStatic.rootXMLDirectory + "cornersofa\\BG.xml");
                result.Add(AppStatic.rootXMLDirectory + "cornersofa\\EN.xml");

                if (country == CountryEnum.Austria || country == CountryEnum.Germany || country == CountryEnum.Zwizerland)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "cornersofa\\DE.xml");
                }
                else if (country == CountryEnum.Bulgaria)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "cornersofa\\BG.xml");
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "cornersofa\\EN.xml");
                }
                return result;
            }
            else if (functionName == ControllerFunctions.Armchair)
            {
                result.Add(AppStatic.rootXMLDirectory + "armchairs\\DE.xml");
                result.Add(AppStatic.rootXMLDirectory + "armchairs\\BG.xml");
                result.Add(AppStatic.rootXMLDirectory + "armchairs\\EN.xml");

                if (country == CountryEnum.Austria || country == CountryEnum.Germany || country == CountryEnum.Zwizerland)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "armchairs\\DE.xml");
                }
                else if (country == CountryEnum.Bulgaria)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "armchairs\\BG.xml");
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "armchairs\\EN.xml");
                }
                return result;
            }
            else if (functionName == ControllerFunctions.Portfolio)
            {
                result.Add(AppStatic.rootXMLDirectory + "portfolio\\DE.xml");
                result.Add(AppStatic.rootXMLDirectory + "portfolio\\BG.xml");
                result.Add(AppStatic.rootXMLDirectory + "portfolio\\EN.xml");

                if (country == CountryEnum.Austria || country == CountryEnum.Germany || country == CountryEnum.Zwizerland)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "portfolio\\DE.xml");
                }
                else if (country == CountryEnum.Bulgaria)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "portfolio\\BG.xml");
                }
                else if (Country == CountryEnum.England || Country == CountryEnum.Other)
                {
                    result.Remove(AppStatic.rootXMLDirectory + "portfolio\\EN.xml");
                }
                return result;
            }
            return null;
        }
    }
}