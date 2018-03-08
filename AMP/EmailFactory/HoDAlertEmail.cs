using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class HoDAlertEmail :Email
    {
        #region constructor

        public HoDAlertEmail()
        {
            Subject = "Alert: Project Approved - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has approved the above project in the Aid Management Platform. " +
                "<br/><br/>Please use the link below to access the approval screen.<br/><br/>{2}<br/><br/>{3}.<br/><br/></span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";
        }
        #endregion
    }
}