using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class ReviewExemptionArVM
    {
        public int ID { get; set; }
        public string ProjectID { get; set; }
        public string StageID { get; set; }
        [Required(ErrorMessage = "Select exemption type")]
        public string ExemptionType { get; set; }

        
        public string ExemptionReason { get; set; }
     
        public string ApproverComment { get; set; }
        public string Approver { get; set; }
      
        public string ApproverName { get; set; }
        public string Approved { get; set; }
        public string DeferralJustification { get; set; }

        [Required(ErrorMessage = "You must enter submission comments")]
        public string SubmissionComment { get; set; }
        public string Requester { get; set; }
        public string RequesterName { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

    }
}