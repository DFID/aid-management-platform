using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.Services;
using AMP.Utilities;
using AMP.ViewModels;


namespace AMP.RiskClasses
{
    public class RiskItemVMCollectionBuilder
    {
        private List<RiskRegister> _riskRegisters;
        private List<RiskCategory> _riskCategories;
        private List<RiskLikelihood> _riskLikelihoods;
        private List<RiskImpact> _riskImpacts;
        private List<Risk> _riskRatings;

        public RiskItemVMCollectionBuilder(List<RiskRegister> riskRegisters, List<RiskCategory> riskCategories,
            List<RiskLikelihood> riskLikelihoods, List<RiskImpact> riskImpacts, List<Risk> riskRatings)
        {
            _riskRegisters = riskRegisters;
            _riskCategories = riskCategories;
            _riskLikelihoods = riskLikelihoods;
            _riskImpacts = riskImpacts;
            _riskRatings = riskRatings;
        }
        public List<RiskItemVM> BuildRiskItemVMList()
        {
            List<RiskItemVM> riskItemsVM = new List<RiskItemVM>();

            foreach (var riskRegister in _riskRegisters)
            {
                RiskItemVM riskItemVm = new RiskItemVM
                {
                    RiskID = riskRegister.RiskID,
                    ProjectID = riskRegister.ProjectID,
                    RiskDescription =  riskRegister.RiskDescription,
                    Owner = riskRegister.Owner,
                    RiskCategory = riskRegister.RiskCategory,
                    RiskCategoryDescription = riskRegister.RiskCategory == 0 ? null : _riskCategories.Find(x => x.ID == riskRegister.RiskCategory).RiskCategoryDescription,
                    RiskLikelihood = riskRegister.RiskLikelihood,
                    RiskLikelihoodDescription = riskRegister.RiskLikelihood == null ? null : _riskLikelihoods.Find(x => x.ID == riskRegister.RiskLikelihood).RiskLikelihoodDescription,
                    RiskImpact = riskRegister.RiskImpact,
                    RiskImpactDescription = riskRegister.RiskImpact == null ? null : _riskImpacts.Find(x => x.ID == riskRegister.RiskImpact).RiskImpactDescription,
                    GrossRisk = riskRegister.GrossRisk,
                    GrossRiskDescription = riskRegister.GrossRisk == null ? null : _riskRatings.Find(x => x.RiskValue == riskRegister.GrossRisk).RiskTitle,
                    MitigationStrategy = riskRegister.MitigationStrategy,
                    ResidualLikelihood = riskRegister.ResidualLikelihood,
                    ResidualLikelihoodDescription = riskRegister.ResidualLikelihood == null ? null : _riskLikelihoods.Find(x => x.ID == riskRegister.ResidualLikelihood).RiskLikelihoodDescription,
                    ResidualImpact = riskRegister.ResidualImpact,
                    ResidualImpactDescription = riskRegister.ResidualImpact == null ? null : _riskImpacts.Find(x => x.ID == riskRegister.ResidualImpact).RiskImpactDescription,
                    ResidualRisk = riskRegister.ResidualRisk,
                    ResidualRiskDescription = riskRegister.ResidualRisk == null ? null : _riskRatings.Find(x => x.RiskValue == riskRegister.ResidualRisk).RiskTitle,
                    Comments = riskRegister.Comments,
                    ExternalOwner = riskRegister.ExternalOwner,
                    Status = riskRegister.Status,
                    LastUpdated = riskRegister.LastUpdated,
                    UserID = riskRegister.UserID

                };
                if (riskRegister.Status == "A")
                {
                    riskItemVm.StatusDescription = Constants.StatusActive;
                }
                else
                {
                    riskItemVm.StatusDescription = Constants.StatusClosed;
                }


                riskItemsVM.Add(riskItemVm);
            }

            return riskItemsVM;
        }
    }
}