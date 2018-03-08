using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RejectCloseProjectEmail:Email
    {
        #region Constructor

        public RejectCloseProjectEmail()
        {
            Subject = "ACTION: Close Project Rejected for - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has rejected your request to close the above project. " +
                    "Please use the link below to review the comments.<br/><br/>{2}<br/><br/>" +
                    "{3}.<br/><br/>Further guidance can be found in the Smart Rules and the Smart Guide.</span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";

        }

        #endregion

    }
}