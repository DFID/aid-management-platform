//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReviewExemption
    {
        public int ID { get; set; }
        public string ProjectID { get; set; }
        public string StageID { get; set; }
        public string ExemptionType { get; set; }
        public string ExemptionReason { get; set; }
        public string ApproverComment { get; set; }
        public string Approver { get; set; }
        public string Approved { get; set; }
        public string SubmissionComment { get; set; }
        public string Requester { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    
        public virtual ReviewStage ReviewStage { get; set; }
        public virtual ProjectMaster ProjectMaster { get; set; }
    }
}
