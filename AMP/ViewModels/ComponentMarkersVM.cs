using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.ViewModels
{
    public class ComponentMarkersVM
    {
        public ComponentHeaderVM ComponentHeader { get; set; }

        public string ComponentID { get; set; }
        public string BudgetCentreID { get; set; }
        public string PBA { get; set; }
        public string SWAP { get; set; }
        public string Status { get; set; }

        public IEnumerable<BenefitingCountry> BenefitingCountrys {get; set; }

        public string BenefitingCountry {get; set; }

        public string FundingMechanism { get; set; }
        public IEnumerable<FundingMech> FundingMechs { get; set; }

        public string ProjectStage { get; set; }

        public string ProjectPastEndDate { get; set; }

        public string IsPlannedEndDateOver { get; set; } 

        public IEnumerable<SupplierVM> ImplementingOrganisation { get; set; }

        public string Approved { get; set; }

        public ProjectWFCheckVM WFCheck { get; set; }
    }
}