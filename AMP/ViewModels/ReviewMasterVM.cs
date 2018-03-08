using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AMP.Models;


namespace AMP.ViewModels
{
    public class ReviewMasterVM
    {
        public string ProjectID { get; set; }
        public int ReviewID { get; set; }
        [Required(ErrorMessage = "Please select review type")]      
        public string ReviewType { get; set; }
        public Nullable<System.DateTime> ReviewDate { get; set; }
        public int ReviewDate_Day { get; set; }
        public int ReviewDate_Month { get; set; }
        public int ReviewDate_Year { get; set; }
        public Nullable<System.DateTime> DeferralDate { get; set; }
        public string RiskScore { get; set; }
        public string Status { get; set; }
        public string Approved { get; set; }
        public string Requester { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }  
        public virtual ReviewARScore ReviewARScore { get; set; }
        public virtual ReviewPCRScore ReviewPCRScore { get; set; }
        public virtual ICollection<ReviewOutput> ReviewOutputs { get; set; }
        public virtual ProjectMaster ProjectMaster { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
    }
}