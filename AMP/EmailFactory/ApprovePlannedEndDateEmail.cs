using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class ApprovePlannedEndDateEmail:Email
    {
        #region Constructor

        public ApprovePlannedEndDateEmail()
        {
            Subject = "ACTION: Workflow Approved for Project - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has approved your request to amend the planned end date for the above project. " +
                   "<br/><br/>By approving this request following information has been updated:" +
                    "<br/>- The Planned End Date." +
                    "<br/>- The Financial End Date - updated to 6 months from the new planned end date." +
                    "<br/>- The Project Closure Reminder Date."+
                    "<br/>- The PCR due date (if required)."+
                    "<br/>- If your project has an AR exemption, this may have been lifted." +
                    "<br/><br/>Please use the link below to review the comments.<br/><br/>{2}<br/><br/>" +
                    "{3}.<br/><br/>Further guidance can be found in the Smart Rules and the Smart Guide.</span>";

            AMPlink = "{0}/Workflow/Details/{1}";

        }

        #endregion


    }
}