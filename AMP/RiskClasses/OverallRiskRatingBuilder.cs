using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;
using AutoMapper;

namespace AMP.RiskClasses
{
    public class OverallRiskRatingBuilder
    {
        private IAMPRepository _ampRepository;
        private OverallRiskRatingVM _overallRiskRatingVm;
        private string _user;
        private OverallRiskRating _overallRiskRating;
        public OverallRiskRating OverallRiskRating
        {
            get { return _overallRiskRating; }
        }
        
        public OverallRiskRatingBuilder(IAMPRepository ampRepository, OverallRiskRatingVM overallRiskRatingVm, string user)
        {
            _ampRepository = ampRepository;
            _overallRiskRatingVm = overallRiskRatingVm;
            _user = user;
        }

        public void BuildOverallRiskRating()
        {
            _overallRiskRating = new OverallRiskRating();
            Mapper.CreateMap<OverallRiskRatingVM, OverallRiskRating>();
            Mapper.Map<OverallRiskRatingVM, OverallRiskRating>(_overallRiskRatingVm, _overallRiskRating);
            _overallRiskRating.LastUpdated = DateTime.Now;
            _overallRiskRating.UserID = _user;

        }
    }
}