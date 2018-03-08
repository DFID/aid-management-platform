using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ReviewPCRScoreVM
    {
        public string ProjectID { get; set; }
        public int ReviewID { get; set; }
        public string FinalOutputScore { get; set; }
        public string RiskScore { get; set; }        
        public string OutcomeScore { get; set; }
        public string ProgressToImpact { get; set; }
        public string CompletedToTimescales { get; set; }
        public string CompletedToCost { get; set; }
        public string FailedJustification { get; set; }
        public string Approver { get; set; }
        public string ApproverName { get; set; }
        public string UserGroup { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string StageID { get; set; }
        public string StageName { get; set; }
        public string ApproveComment { get; set; }
        public string SubmissionComment { get; set; }
        public string IsApproved { get; set; }
        public string Status { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? DueDate { get; set; }
        public string UserID { get; set; }
        public string Requester { get; set; }
        public string RequesterName { get; set; }
        public ReviewDeferralVM ReviewDeferralVM { get; set; }

        //public virtual ProjectMaster ProjectMaster { get; set; }
            //public virtual ReviewMaster ReviewMaster { get; set; }
        }
}