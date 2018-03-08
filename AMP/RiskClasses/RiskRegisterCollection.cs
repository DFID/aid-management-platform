using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;

namespace AMP.RiskClasses
{
    public class RiskRegisterCollection
    {
        private IEnumerable<RiskRegister> _riskItems;

        public RiskRegisterCollection(IEnumerable<RiskRegister> riskItems)
        {
            _riskItems = riskItems;
        }

        public bool HasRiskItems()
        {
            return _riskItems.Any();
        }

        public IEnumerable<RiskRegister> ActiveRisks()
        {
            return _riskItems.Where(x => x.Status == "A");
        }
        public IEnumerable<RiskRegister> ClosedRisks()
        {
            return _riskItems.Where(x => x.Status == "C");
        }
        public IEnumerable<RiskRegister> AllRisks()
        {
            return _riskItems;
        }
    }
}