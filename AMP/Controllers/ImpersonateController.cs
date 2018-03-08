using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMP.ViewModels;
using AMP.Services;
using AMP.Utilities;

namespace AMP.Controllers
{
    public class ImpersonateController : BaseController
    {

                #region Initialise

        private IAmpServiceLayer _ampServiceLayer;
        private IIdentityManager _identityManager;


        public ImpersonateController()
            : base()
        {
            this._ampServiceLayer = new AMPServiceLayer();
            this._identityManager = new DemoIdentityManager();
        }

        public ImpersonateController(IAmpServiceLayer serviceLayer, IIdentityManager identityManager)
            : base(serviceLayer, identityManager)
        {
            this._ampServiceLayer = serviceLayer;
            this._identityManager = identityManager;
        }

        public ErrorEngine errorengine = new ErrorEngine();

        #endregion


        // GET: Impersonate
        public ActionResult Impersonate()
        {
            //Get logon
            string user = GetRealEmpNo();

            if (!_ampServiceLayer.IsAdmin(user))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            //Create a cookie
            HttpCookie ImpersonationCookie;
            if (HttpContext.Response.Cookies["Impersonation"] != null)
            {
                ImpersonationCookie = new HttpCookie("Impersonation", user);
                ImpersonationCookie.Expires = DateTime.Now.AddDays(-1);
            }


            ImpersonateVM vm = new ImpersonateVM();

            return View(vm);
        }

        [HttpPost]
        public ActionResult Impersonate(ImpersonateVM impersonatevm)
        {
            string empNo = GetEmpNo();

            string actualLogon = GetLogon();

            string result;

            if (!_ampServiceLayer.IsAdmin(empNo))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }


            //Create a cookie
            HttpCookie ImpersonationCookie;
            if (impersonatevm.ImpersonateLogon.ToUpper() == actualLogon.ToUpper())
            {
                if (HttpContext.Response.Cookies["Impersonation"] != null)
                {
                    ImpersonationCookie = new HttpCookie("Impersonation", impersonatevm.ImpersonateEmpNo);
                    ImpersonationCookie.Expires = DateTime.Now.AddDays(-1);
                }
            }
            else
            {
                ImpersonationCookie = new HttpCookie("Impersonation", impersonatevm.ImpersonateEmpNo);
                ImpersonationCookie.Expires = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                HttpContext.Response.Cookies.Add(ImpersonationCookie);
            }

            result = "You are now impersonating " + impersonatevm.DisplayName;

            ViewBag.message = result;

            return View(impersonatevm);

        }
    }
}