using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMP.Models;
using AMP.Utilities;
using AMP.ViewModels;

namespace AMP.RiskClasses
{
    public class OverallRiskRatingVMCollectionBuilder
    {
        private List<Risk> _riskRatings;
        private List<OverallRiskRating> _overallRiskRatings;
        public OverallRiskRatingVMCollectionBuilder(List<OverallRiskRating> overallRiskRatings, List<Risk> riskRatings)
        {
            _riskRatings = riskRatings;
            _overallRiskRatings = overallRiskRatings;
        }
        public List<OverallRiskRatingVM> BuildOverallRiskRatingVMList()
        {
            List<OverallRiskRatingVM> OverallRiskRatingsVM = new List<OverallRiskRatingVM>();

            foreach (var overallRiskRating in _overallRiskRatings)
            {
                OverallRiskRatingVM overallRiskRatingVm = new OverallRiskRatingVM
                {
                    OverallRiskRatingId = overallRiskRating.OverallRiskRatingId,
                    ProjectID = overallRiskRating.ProjectID,
                    Comments = overallRiskRating.Comments,
                    RiskScore = overallRiskRating.RiskScore == null ? null :  _riskRatings.Find(x => x.RiskValue == overallRiskRating.RiskScore).RiskTitle,
                    UserID = overallRiskRating.UserID,
                    LastUpdated = overallRiskRating.LastUpdated
                };

                OverallRiskRatingsVM.Add(overallRiskRatingVm);
            }

            return OverallRiskRatingsVM;
        }
    }
}
