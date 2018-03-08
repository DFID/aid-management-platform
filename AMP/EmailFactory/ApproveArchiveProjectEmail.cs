using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class ApproveArchiveProjectEmail : Email
    {
        #region Constructor

        public ApproveArchiveProjectEmail()
        {
            Subject = "ACTION: Archive Project Approved - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has approved your request to archive the above project. " +
                    "Please use the link below to review the comments.<br/><br/>{2}<br/><br/>" +
                    "<br/><br/>By approving this request the approver confirms and is satisfied that:" +
                    "<br/>- There are no outstanding financial transactions;" +
                    "<br/>- All financial audit statements have been received, with all funds accounted for appropriately" +
                    "<br/>-All assets have been transferred or disposed of according to the SmartRules." +
                    "<br/><br/>You will no longer be able to edit or carry out transactions for this project. " +
                    "<br/><br/>{3}.<br/><br/></span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";

        }

        #endregion

    }
}