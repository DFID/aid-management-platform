using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class ApproveProjectImplementaionWorkflowEmail : Email
    {
        #region Constructor

        public ApproveProjectImplementaionWorkflowEmail()
        {
            Subject = "ACTION: Workflow Approved for Project - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has approved the above project in the Aid Management Platform. " +
                "<br/><br/> Can the SRO please check that all documents meet DFID transparency standards" +
                "<br/> and are fit for publication. Please check the Smart Guides on " +
                "<a href =\"\">Transparency</a>" +
                "<br/><br/>Please use the link below to see the approval comments.<br/><br/>{2}<br/><br/>{3}.<br/><br/></span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";

        }

        #endregion

    }
}