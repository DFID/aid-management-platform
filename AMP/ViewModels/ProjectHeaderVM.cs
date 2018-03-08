using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ProjectHeaderVM
    {
        public string ProjectID { get; set; }
        public string Title { get; set; }
        public string Stage { get; set; }
        public string StageDescription { get; set; }
        public bool ProjectExistsInPortfolio { get; set; }
        public string BudgetCentre { get; set; }
        public string UserID { get; set; }

    }
}