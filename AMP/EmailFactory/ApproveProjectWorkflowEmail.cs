using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class ApproveProjectWorkflowEmail : Email
    {
        #region Constructor

        public ApproveProjectWorkflowEmail()
        {
            Subject = "ACTION: Workflow Approved for Project - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has approved the above project in the Aid Management Platform. " +
                "<br/><br/> Project title, description and project component details will publish on Development Tracker. Can the  SRO please make sure this information meets " +
                "<a href =\"https://dfid-live.saas.hp.com/saw/ess/viewResult/574979?TENANTID=658803389\"> DFID transparency standards </a>" + " and is fit for publication." +
                "<br/><br/>Please use the link below to see the approval comments.<br/><br/>{2}<br/><br/>{3}.<br/><br/></span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";

        }

        #endregion

    }
}