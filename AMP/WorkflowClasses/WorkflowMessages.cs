using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.ViewModels;

namespace AMP.WorkflowClasses
{
    public class WorkflowMessages
    {


        public static string PlannedEndDatePendingApprovalMessage(DateTime newDate)
        {




            string pedText = "The following dates will change:\n" +
                             "- Planned End Date to: " + newDate.ToShortDateString()
                             + "\n" +
                             "- Financial End Date to: " + newDate.AddMonths(6).ToShortDateString()
                             + "\n" +
                             "- Closure Reminder Date to: " + newDate.AddMonths(-3).ToShortDateString()
                             + "\n" +
                             "\n" +
                             "- If the project requires a PCR, then its due date will be amended." +
                             " The AR exemption status may be altered as a result of this change."; //+ exemptStatus;
            return pedText;

        }

        // show the original and new planned end dates as part of the workflow history and also once the approval has been submitted
        public static string PlannedEndDateWorkflowHistoryMessage(DateTime oldDate, DateTime newDate)
        {
            string pedText = " The planned end date for this project has been changed from " +
                             oldDate.ToShortDateString()
                             + 
                             " to " + newDate.ToShortDateString();
            return pedText;
        }


    }
}