using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.WorkflowClasses;

namespace AMP.ViewModels
{
    public class WorkflowMasterVM
    {
        public int WorkFlowID { get; set; }
        public int WorkFlowStepID { get; set; }
        public int TaskID { get; set; }
        public int StageID { get; set; }
        public string ProjectID { get; set; }
        public string ActionBy { get; set; }
        public string RequesterName { get; set; }
        public Nullable<System.DateTime> ActionDate { get; set; }
        public string Recipient { get; set; }
        public string ActionComments { get; set; }
        public string CurrentUserMemberOfGroup { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }

 

    }
}