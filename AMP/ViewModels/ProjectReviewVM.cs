using System.Collections.Generic;
using AMP.Models;
namespace AMP.ViewModels
{
    public class ProjectReviewVM
    {
        public List<ReviewVM> ProjectReviews { get; set; }
        public ReviewMasterVM ReviewMaster { get; set; }
        public ReviewPCRScoreVM ProjectPcrScore { get; set; }
        public Deferral Deferral { get; set; }
        public PerformanceVM Performance { get; set; }
        public ProjectHeaderVM ProjectHeader { get; set; }
        public ReviewVM ReviewVm { get; set; }
        public List<DeferralReason> DeferralReasons { get; set; }
        public List<ExemptionReason> ExemptionReasons { get; set; }
        public ReviewExemptionVM ReviewExemptionAR { get; set; }
        public ReviewExemptionVM ReviewExemptionPCR { get; set; }
        public string CurrentUserMemberOfGroup { get; set; }
        public ProjectWFCheckVM ProjectWFCheck { get; set; }
    }
} 