using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMP.Models;

//using AMP.Models;

namespace AMP.ViewModels
{
    public class RiskRegisterVM
    {
        public ProjectHeaderVM ProjectHeader { get; set; }
        public int RiskRegisterID { get; set; }
        public string ProjectID { get; set; }
        public string DocumentID { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }
        public bool IsTeamMember { get; set; }
        //public virtual ReviewMaster ReviewMaster { get; set; }
        public Risk Risk { get; set; }
        public RiskItemVM RiskItemVm { get; set; }
        public RiskItemsVM RiskItemsVm { get; set; }
        public List<RiskCategory> RiskCategoryValues { get; set; }
        public List<RiskLikelihood> RiskLikelihoodValues { get; set; }
        public List<RiskImpact> RiskImpactValues { get; set; }
        public List<Risk> RiskValues { get; set; }
        public List<RiskDocumentVM> RiskDocuments   {get;set;}
        public List<OverallRiskRatingVM> OverallRiskRatings { get; set; }
        public OverallRiskRatingVM OverallRiskRatingVm { get; set; }
        public OverallRiskRatingsVM OverallRiskRatingsVm { get; set; }

        public RiskDocumentVM RiskDocumentVm { get; set; }
        public RiskDocumentsVM RiskDocumentsVm { get; set; }
    }
}
