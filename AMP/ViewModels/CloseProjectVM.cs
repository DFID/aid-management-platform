using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class CloseProjectVM
    {
        public string ProjectID { get; set; }
        public int WorkFlowID { get; set; }
        public int WorkFlowStepID { get; set; }
        public string Description { get; set; }
        public int TaskID { get; set; }
        public string ActionBy { get; set; }
        public Nullable<System.DateTime> ActionDate { get; set; }
        public string ActionComments { get; set; }
        public string Recipient { get; set; }
        public string RequesterName { get; set; }
        public bool IsSubmissionStage { get; set; }
        public bool IsApprovalStage { get; set; }
        public bool IsApproved { get; set; }

    }
}