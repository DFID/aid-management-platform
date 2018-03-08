using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMP.Models;
using AMP.Services;
using AMP.Utilities;

namespace AMP.Controllers
{
    public class BaseController : Controller
    {
        private IAmpServiceLayer _serviceLayer;
        IIdentityManager _identityManager;
        private ErrorEngine _errorEngine = new ErrorEngine();

        

        public BaseController()
        {
            this._serviceLayer = new AMPServiceLayer();
            this._identityManager = new DemoIdentityManager();
        }

        public BaseController(IAmpServiceLayer serviceLayer, IIdentityManager identityManager)
        {
            this._serviceLayer = serviceLayer;
            this._identityManager = identityManager;
        }

        protected string GetLogon()
        {
            return  "A-Inputter";

        }

        protected string GetRealEmpNo()
        {
            //Set Environment Flat (This should move to a dedicate method/general method.)
            ViewBag.Environment = ConfigurationManager.AppSettings["EnvironmentFlag"].ToString();
            ViewBag.ARIESAPI = ConfigurationManager.AppSettings["ARIESRestServiceUrl"].ToString();
            ViewBag.IdentityLite = ConfigurationManager.AppSettings["PersonServiceUrl"].ToString();

            String User;
            Person person;

            User = "A-Inputter";

            //Extract the username from the Identity.UserName
            //string contextuser = HttpContext.User.Identity.Name;
            //string[] splituser = AMPUtilities.SplitLoginName(contextuser);
            //User = splituser[1];

            try
            {
                person = _identityManager.GetPersonByUserName(User);

            }
            catch (Exception ex)
            {
                _errorEngine.LogError(ex, User);
                throw;
            }
            String EmpNo = person.EmpNo;

            return EmpNo;

        }

        protected string GetEmpNo()
        {

            //Set Environment Flat (This should move to a dedicate method/general method.)
            ViewBag.Environment = ConfigurationManager.AppSettings["EnvironmentFlag"].ToString();
            ViewBag.ARIESAPI = ConfigurationManager.AppSettings["ARIESRestServiceUrl"].ToString();
            ViewBag.IdentityLite = ConfigurationManager.AppSettings["PersonServiceUrl"].ToString();

            // Call Logging to record user accessing this action.

            String User;
            Person person;

            User = "A-Inputter";


            //Extract the username from the Identity.UserName
            //string contextuser = HttpContext.User.Identity.Name;
            //string[] splituser = AMPUtilities.SplitLoginName(contextuser);
            //User = splituser[1];


            try
            {
                person = _identityManager.GetPersonByUserName(User);

            }
            catch (Exception ex)
            {
                _errorEngine.LogError(ex, User);
                throw;
            }
            String EmpNo = person.EmpNo;


            //If running in DEV Mode, check for impersonation cookie.
            if (ConfigurationManager.AppSettings["Impersonate"] == "ON")
            {
                HttpCookie impersonationCookie;
                impersonationCookie = HttpContext.Request.Cookies["Impersonation"];
                if (impersonationCookie != null && !string.IsNullOrEmpty(impersonationCookie.Value))
                {
                    EmpNo = impersonationCookie.Value;
                }
            }

            return EmpNo;
            
        }

        protected void LogCall(string user, string view, string projectId)
        {
            //Call Logging.
            _serviceLayer.InsertLog(view, user, projectId );
        }

        protected void LogCall(string user, string view)
        {
            //Call Logging.
            _serviceLayer.InsertLog(view, user);
        }


        #region Disposal

        ~BaseController()
        {
            Dispose(false);
        }

        #endregion

    }
}