using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RequestCloseProjectEmail:Email
    {
        #region Constructor

        public RequestCloseProjectEmail()
        {
            Subject = "ACTION: Close Project - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has sent you a request to close the above project in the Aid Management Platform with the following comments." +
            "<br/><br/>\"{2}\"" +
            "<br/><br/>Please review the request comments, and ensure the necessary financial actions have been taken and you are content for this project to be closed without a project completion report before approving or rejecting the request." +
            "<br/><br/>Use the link below to access the project; the close section can be found at the bottom of the page under ‘Further options'" +
            "<br/><br/>{3}<br/><br/>{4}<br/><br/>Further guidance on closing a project can be found in the Smart Rules and the Smart Guide.</span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";
        }

        #endregion

    }
}