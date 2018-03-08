using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using AMP.Models;
using Microsoft.Ajax.Utilities;

namespace AMP.Utilities
{
    public class AMPUtilities
    {
        public class AMPEmail
        {
            public string To { get; set; }
            public string Cc { get; set; }
            public string Bcc { get; set; }
            public string From { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public MailPriority Priority { get; set; }
        }

        public class EmailPeople
        {
            public PersonDetails Sender { get; set; }
            public PersonDetails Recipient { get; set; }
            public PersonDetails SRO { get; set; }
            public IEnumerable<PersonDetails> projectTeam { get; set; }
        }

        #region Risk Register Select Lists
        public static SelectList RiskCategoryList()
        {
            SelectList selectList = new SelectList(new[]
            {
                new {ID = "1", RiskCategoryDescription = "Context"},
                new {ID = "2", RiskCategoryDescription = "Delivery"},
                new {ID = "3", RiskCategoryDescription = "Safeguarding"},
                new {ID = "4", RiskCategoryDescription = "Operational"},
                new {ID = "5", RiskCategoryDescription = "Fiduciary"},
                new {ID = "6", RiskCategoryDescription = "Reputational"}
            }, "ID", "RiskCategoryDescription");
            return selectList;
        }

        public static SelectList RiskLikelihoodList()
        {
            SelectList selectList = new SelectList(new[]
            {
                new {ID = "1", RiskLikelihoodDescription = "Rare"},
                new {ID = "2", RiskLikelihoodDescription = "Unlikely"},
                new {ID = "3", RiskLikelihoodDescription = "Possible"},
                new {ID = "4", RiskLikelihoodDescription = "Likely"},
                new {ID = "5", RiskLikelihoodDescription = "Almost Certain"}
            }, "ID", "RiskLikelihoodDescription");
            return selectList;
        }

        public static SelectList RiskImpactList()
        {
            SelectList selectList = new SelectList(new[]
            {
                new {ID = "1", RiskImpactDescription = "Insignificant"},
                new {ID = "2", RiskImpactDescription = "Minor"},
                new {ID = "3", RiskImpactDescription = "Moderate"},
                new {ID = "4", RiskImpactDescription = "Major"}, 
                new {ID = "5", RiskImpactDescription = "Severe"}
            }, "ID", "RiskImpactDescription");
            return selectList;
        }

        public static SelectList RiskRatingList()
        {
            SelectList selectList = new SelectList(new[]
            {
                new {ID = "R1", RiskRatingDescription = "Minor"},
                new {ID = "R2", RiskRatingDescription = "Moderate"},
                new {ID = "R3", RiskRatingDescription = "Major"},
                new {ID = "R4", RiskRatingDescription = "Severe"}
            }, "ID", "RiskRatingDescription");
            return selectList;
        }

        public static SelectList StatusList()
        {
            SelectList selectList = new SelectList(new[]
            {
                new {ID = "A", StatusDescription = "Active"},
                new {ID = "C", StatusDescription = "Closed"}
            }, "ID", "StatusDescription");
            return selectList;
        }


        #endregion  

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string getLoginName()
        {

            string loginName = HttpContext.Current.User.Identity.Name;

            string[] logonName = SplitLoginName(loginName);

            return logonName[1];

        }

        public static string CleanText(string text)
        {
            if (text != null)
            {
             //   text.Replace("\r", string.Empty).Replace("\n", string.Empty);

                text = System.Text.RegularExpressions.Regex.Replace(text, @"\r\n+", " ");             

            }
            return text;
        }


        // this is used in the delivery chain section to ensure that 
        // users to not enter or search for partner names beginning with
        // &, #, + or a space
        //just strip these out of the beginning of the string

        public static string CleanCharsFromBeginningofText(string text)
        {

            if (!string.IsNullOrEmpty(text))
            {
                // if text begins with &, #, + or blank text then strip it out
                var outputstring = (!string.IsNullOrEmpty(text) && text[0] == '&' || text[0] == '#' || text[0] == '+')
                    ? "" + text.Substring(1)
                    : text;
                text = outputstring;
            }
            return text;
        }

        public static string[] SplitLoginName(string loginName)
        {
            char[] splitArray = new char[1];
            splitArray[0] = '\\';
            string[] logonName = loginName.Split(splitArray);
            return logonName;
        }

        public static string BaseUrl()
        {
            string URL = System.Web.Configuration.WebConfigurationManager.AppSettings["BaseURL"].ToString();

            return URL;
        }

        public static string FinanceWebServiceUrl()
        {
            string URL = System.Web.Configuration.WebConfigurationManager.AppSettings["ARIESRestServiceUrl"].ToString();

            return URL;
        }

        public static string PersonWebServiceUrl()
        {
            string URL = System.Web.Configuration.WebConfigurationManager.AppSettings["PersonServiceUrl"].ToString();

            return URL;
        }

        public static string EDRMWebServiceUrl()
        {
            string URL = System.Web.Configuration.WebConfigurationManager.AppSettings["EDRMServiceUrl"].ToString();

            return URL;
        }

        public static string GeoURL()
        {
            string URL = System.Web.Configuration.WebConfigurationManager.AppSettings["GeoURL"].ToString();

            return URL;
        }

        public static string AppMode()
        {
            string appMode = System.Web.Configuration.WebConfigurationManager.AppSettings["Mode"].ToString();

            return appMode;
        }
        public static string TestEmail()
        {
            string testEmail = System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmail"].ToString();

            return testEmail;
        }
        public static string SMTPClient()
        {
            string SMTPClient = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPClient"].ToString();

            return SMTPClient;
        }
        public static string SenderEmail()
        {
            string senderEmail = System.Web.Configuration.WebConfigurationManager.AppSettings["SenderEmail"].ToString();

            return senderEmail;
        }

        public static string ARIESUpdateEnabled()
        {
            string ariesLinkEnabled = System.Web.Configuration.WebConfigurationManager.AppSettings["ARIESUpdateEnabled"].ToString();

            return ariesLinkEnabled;
        }

    }
}