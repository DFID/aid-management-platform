using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.RiskClasses
{
    public class OverallRiskRatingItem: OverallRiskRatingVM
    {
        private IAMPRepository _ampRepository;
        public OverallRiskRatingItem(IAMPRepository ampRepository)
        {
            _ampRepository = ampRepository;
        }

        public void ConstructOverallRiskRating(int overallriskrating)
        {
            OverallRiskRating overallRiskRating = new OverallRiskRating();
            overallRiskRating = _ampRepository.GetOverallRisk(overallriskrating);
            if (overallRiskRating != null)
            {
                OverallRiskRatingId = overallRiskRating.OverallRiskRatingId;
                ProjectID = overallRiskRating.ProjectID;
                Comments = overallRiskRating.Comments;
                RiskScore = overallRiskRating.RiskScore;
                UserID = overallRiskRating.UserID;
                LastUpdated = overallRiskRating.LastUpdated;
            }
        }
    }
}