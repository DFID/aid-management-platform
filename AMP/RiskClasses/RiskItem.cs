using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.RiskClasses
{
    public class RiskItem: RiskItemVM
    {
        private IAMPRepository _ampRepository;
        public RiskItem(IAMPRepository ampRepository)
        {
            _ampRepository = ampRepository;
        }
        
        public void ConstructRiskItem(int Id)
        {
            RiskRegister riskRegister = new RiskRegister();
            riskRegister = _ampRepository.GetRiskItem(Id);

            RiskDescription = riskRegister.RiskDescription;
            RiskID = riskRegister.RiskID;
            ProjectID = riskRegister.ProjectID;
            Comments = riskRegister.Comments;
            ExternalOwner = riskRegister.ExternalOwner;
            MitigationStrategy = riskRegister.MitigationStrategy;
            GrossRisk = riskRegister.GrossRisk;
            Owner = riskRegister.Owner;
            RiskCategory = riskRegister.RiskCategory;
            RiskLikelihood = riskRegister.RiskLikelihood;
            RiskImpact = riskRegister.RiskImpact;
            ResidualLikelihood = riskRegister.ResidualLikelihood;
            ResidualImpact = riskRegister.ResidualImpact;
            ResidualRisk = riskRegister.ResidualRisk;
            Status = riskRegister.Status;
            LastUpdated = riskRegister.LastUpdated;

        }


    }
}