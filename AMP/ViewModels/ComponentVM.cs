using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;

namespace AMP.ViewModels
{
    public class ComponentVM
    {
        public string ComponentID { get; set; }
        public string ComponentDescription { get; set; }
        public string BudgetCentreID { get; set; }
        public string BudgetCentreDescription { get; set; }

        public string ProjectID { get; set; }
        public string FundingMechanism { get; set; }

        public string FundingArrangementValue { get; set; }

        public string PartnerOrganisationValue { get; set; }

        public string AdminApprover { get; set; }
        public string AdminApproverDescription { get; set; }

        public string Approved { get; set; }
        public IEnumerable<FundingMech> FundingMechs { get; set; }
        public IEnumerable<FundingArrangement> FundingArrangements { get; set; }
        public IEnumerable<PartnerOrganisation> PartnerOrganisations { get; set; }

        public ComponentDateVM ComponentDate { get; set; }

        public virtual IEnumerable<BudgetCentre> BudgetCentre { get; set; }

        public ComponentHeaderVM ComponentHeader { get; set; }

        public string ProjectStage {get; set; }

        public bool AnyApprovedBudget { get; set; }

        public string ProjectPastEndDate {get; set; }

        public ProjectWFCheckVM WFCheck { get; set; }
    }
}