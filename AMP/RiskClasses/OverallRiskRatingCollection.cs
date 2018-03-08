using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.RiskClasses
{
    public class OverallRiskRatingCollection
    {
        private IEnumerable<OverallRiskRating> _overallRiskRatings;

        public OverallRiskRatingCollection(IEnumerable<OverallRiskRating> overallRiskRatings)
        {
            _overallRiskRatings = overallRiskRatings;
        }

        public bool HasOverallRiskRating()
        {
            if (_overallRiskRatings != null && _overallRiskRatings.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<OverallRiskRating> AllOverallRiskRatingsRisks()
        {
            return _overallRiskRatings;
        }

        public List<OverallRiskRatingVM> CreateOverallRiskRatingVms()
        {
            List<OverallRiskRatingVM> overallRiskRatingVms = new List<OverallRiskRatingVM>();

            foreach (OverallRiskRating riskRating in _overallRiskRatings)
            {
                OverallRiskRatingVM overallRiskRatingVm = new OverallRiskRatingVM
                {
                    OverallRiskRatingId = riskRating.OverallRiskRatingId,
                    ProjectID = riskRating.ProjectID,
                    Comments = riskRating.Comments,
                    RiskScore = riskRating.RiskScore,
                    LastUpdated = riskRating.LastUpdated,
                    UserID = riskRating.UserID
                };
                overallRiskRatingVms.Add(overallRiskRatingVm);
            }
            return overallRiskRatingVms;
        } 

    }
}