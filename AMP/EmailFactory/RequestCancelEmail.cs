using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RequestCancelEmail:Email
    {
        #region Constructor

        public RequestCancelEmail()
        {
            Subject = "Workflow Request Cancelled - {0} {1}";

            Body =
                "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has cancelled the workflow request in the Aid Management Platform." +
                "  You do not need to take any action.";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";
        }

        #endregion

    }
}