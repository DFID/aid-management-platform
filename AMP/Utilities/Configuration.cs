using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace AMP.Utilities
{
    public class Configuration: IConfiguration
    {

        public string EDRMWebServiceUrl
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["EDRMServiceUrl"]; }
        }

        public string IATIWebServiceUrl
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["IATIWebServiceUrl"]; }
        }

        public  string PersonWebServiceUrl
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["PersonServiceUrl"]; }
        }

        public string FinanceWebServiceUrl
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["ARIESRestServiceUrl"]; }
        }
        public string BaseUrl
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["BaseURL"]; }
        }

        public string GeoURL
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["GeoURL"]; }
        }

        public string AppMode
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["Mode"]; }
        }

        public string TestEmail
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["TestEmail"]; }
        }

        public string SMTPClient        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPClient"]; }
        }

        public string SenderEmail
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["SenderEmail"]; }
        }

        public string CreateProjectFolderInVault
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["CreateProjectFolderInVault"]; }
        }

        public string EDRMUser
        {
            get { return System.Web.Configuration.WebConfigurationManager.AppSettings["EDRMUser"]; }   
        }

    }
}