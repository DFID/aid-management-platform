using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.ViewModels
{
    public class ComponentSectorVM
    {
        public IEnumerable<InputSectorVM> InputSectors { get; set; }
        public InputSectorVM NewInputSector { get; set; }
        public ComponentHeaderVM ComponentHeader { get; set; }

        public string FundingMechanism { get; set; }
        public IEnumerable<FundingMech> FundingMechs { get; set; }
        public string ProjectStage { get; set; }

        public string Approved { get; set; }
        public bool AnyApprovedBudget { get; set; }

        public ProjectWFCheckVM WFCheck { get; set; }

        public bool IsTeamMember { get; set; }

    }
}