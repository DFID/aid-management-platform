using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;
using AutoMapper;

namespace AMP.RiskClasses
{
    public class RiskRegisterBuilder
    {
        private RiskItemVM _riskItemVm;
        private string _user;
        private RiskRegister _riskRegister;
        public RiskRegister RiskRegister
        {
            get { return _riskRegister; }
        }


        public RiskRegisterBuilder( RiskItemVM riskItemVm, string user)
        {
            _riskItemVm = riskItemVm;
            _user = user;
        }

        public void BuildRiskRegisterItem()
        {
            _riskRegister = new RiskRegister();
            Mapper.CreateMap<RiskItemVM, RiskRegister>();
            Mapper.Map<RiskItemVM, RiskRegister>(_riskItemVm, _riskRegister);
            _riskRegister.LastUpdated = DateTime.Now;
            _riskRegister.UserID = _user;

        }


    }
}