using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RejectArchiveProjectEmail : Email
    {
        #region Constructor

        public RejectArchiveProjectEmail()
        {
            Subject = "ACTION: Archive Project Rejected for - {0} {1}";
            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has rejected the request to Archive the project above in the Aid Management Platform. " +
                    "Please use the link below to review the comments.<br/><br/>{2}<br/><br/>" +
                    "{3}.<br/><br/></span>";
            AMPlink = "{0}/Workflow/Edit/{1}/{2}";

        }

        #endregion

    }
}