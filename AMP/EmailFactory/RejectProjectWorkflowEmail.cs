using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RejectProjectWorkflowEmail : Email
    {
        #region Constructor

        public RejectProjectWorkflowEmail()
        {
            Subject = "ACTION: Project Rejected - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has rejected the above project in the Aid Management Platform. " +
                    "Please use the link below to review the comments.<br/><br/>{2}<br/><br/>" +
                    "{3}.<br/><br/>Further guidance can be found in the Smart Rules and the Smart Guide.</span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";

        }

        #endregion

    }
}