using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ProjectStaticDataVM
    {
        public string BudgetCentreDescription { get; set; }

        public string StageDescription { get; set; }

        public IEnumerable<CCOValuesVM> CCOCollection { get; set; }

    }
}