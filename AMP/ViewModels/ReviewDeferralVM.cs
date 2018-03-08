using System;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class ReviewDeferralVM
    {
        public int DeferralID { get; set; }
        public string ProjectID { get; set; }
        public int ReviewID { get; set; }
        public string StageID { get; set; }
        public string ReviewType { get; set; }
        [Required(ErrorMessage = "Please select deferral time scale")]
        public string DeferralTimescale { get; set; }
        
        [Required(ErrorMessage = "You must enter deferral justification")]
        public string DeferralJustification { get; set; }
        public string ApproverComment { get; set; }
       
        public string Approver { get; set; }
        public string ApproverName { get; set; }
        public string Approved { get; set; }
        public string Requester { get; set; }
        public string RequesterName { get; set; }
        public string IsApproved { get; set; }
        public DateTime? PreviousReviewDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

    }
}