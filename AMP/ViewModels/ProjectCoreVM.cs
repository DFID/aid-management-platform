using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ProjectCoreVM
    {
        public string ProjectID { get; set; }
        public string OpStatus { get; set; }
        public string TeamMarker { get; set; }

        public string RiskAtApproval { get; set; }
        public string SpecificConditions { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }

    }
}