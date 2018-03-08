using System;
using System.Collections.Generic;
using AMP.Models;
using System.ComponentModel.DataAnnotations;
using AMP.Services;

namespace AMP.ViewModels
{
    public class ReviewVM
    {
        public string ProjectID { get; set; }
        public string ProjectPurpose { get; set; }
        public int ReviewID { get; set; }
        public string ReviewType { get; set; }
        public DateTime ReviewDate { get; set; }
        public int ReviewDate_Day { get; set; }
        public int ReviewDate_Month { get; set; }
        public int ReviewDate_Year { get; set; }
        public Nullable<System.DateTime> DeferralDate { get; set; }
        public int DefferalDate_Day { get; set; }
        public int DefferalDate_Month { get; set; }
        public int DefferalDate_Year { get; set; }
        public bool IsDeffered { get; set; }
        public string RiskScore { get; set; }        
        public string OverallScore { get; set; }
        public string Progress { get; set; }
        public string OnTrackTime { get; set; }
        public string OnTrackCost { get; set; }
        public string OffTrackJustification { get; set; }
        public string Status { get; set; }
        public string Approved { get; set; }
        public string Approver { get; set; }   
        public string ApproverName { get; set; }
        public string Requester { get; set; }       
        public string RequesterName { get; set; }
        public string UserGroup { get; set; }
        public string StageID { get; set; }
        public string StageName { get; set; }
        public string ApproveComment { get; set; }
        public string SubmissionComment { get; set; }       
        public string IsApproved { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string UserID { get; set; }
        public ReviewOutputVM ReviewOutputVm { get; set; }
        public IEnumerable<ReviewOutputVM> ReviewOutputs { get; set; }
        public ReviewDeferralVM ReviewDeferralVM { get; set; }
        //public IEnumerable<ReviewDeferralValuesVM> DeferralValues { get; set; }
        public ReviewDocumentVM ReviewDocument { get; set; }
        public IEnumerable<ReviewDocumentVM> ReviewDocuments { get; set; }

        public IEnumerable<RiskRegisterVM> RiskRegister { get; set; }

        public IEnumerable<OverallRiskRatingVM> OverallRiskRatings { get; set; }
    }
}