using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using AMP.Models;


namespace AMP.ViewModels
{
    public class RiskItemVM
    {
        public ProjectHeaderVM ProjectHeader { get; set; }
        ////public virtual ReviewMaster ReviewMaster { get; set; }
        //public OverallRiskRatingVM OverallRiskRatingVm { get; set; }
        //public Risk Risk { get; set; }
        ////public IEnumerable<RiskRegisterVM> RiskRegister { get; set; }
        //public IEnumerable<OverallRiskRatingVM> OverallRiskRatings { get; set; }
        //public IEnumerable<RiskDocument> RiskDocuments { get; set; }

        public int RiskID { get; set; }
        public string ProjectID { get; set; }
        [Required(ErrorMessage = "You must enter a risk description")]
        [MinLength(20, ErrorMessage = "Risk description must contain at least 20 characters")]
        public string RiskDescription { get; set; }
        public string Owner { get; set; }
        public string OwnerName { get; set; }
        public int RiskCategory { get; set; }
        public string RiskCategoryDescription { get; set; }
        public Nullable<int> RiskLikelihood { get; set; }
        public string RiskLikelihoodDescription { get; set; }
        public Nullable<int> RiskImpact { get; set; }
        public string GrossRisk { get; set; }
        public string GrossRiskDescription { get; set; }
        public string RiskImpactDescription { get; set; }
        [Required(ErrorMessage = "You must enter mitigation strategy")]
        [MinLength(20, ErrorMessage = "Mitigation strategy must contain at least 20 characters")]
        public string MitigationStrategy { get; set; }
        public Nullable<int> ResidualLikelihood { get; set; }
        public string ResidualLikelihoodDescription { get; set; }
        public Nullable<int> ResidualImpact { get; set; }
        public string ResidualImpactDescription { get; set; }
        public string ResidualRisk { get; set; }
        public string ResidualRiskDescription { get; set; }
        public string Comments { get; set; }
        public string ExternalOwner { get; set; }
        public string Status { get; set; }

        public string StatusDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime LastUpdated { get; set; }
        public string UserID { get; set; }
        public bool IsTeamMember { get; set; }

    }
}