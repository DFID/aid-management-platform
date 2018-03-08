using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ARIESModels;

namespace AMP.ViewModels
{
    public class ProjectComponentVM
    {
        public ProjectHeaderVM ProjectHeader { get; set; }

        //To be replaced with a Component ViewModel
        public IEnumerable<ComponentMaster> ComponentMaster { get; set; }

        public string ProjectPastEndDate {get; set; }
        public ProjectWFCheckVM ProjectWfCheckVm { get; set; }
        public string InputterPresent { get; set; }

    }
}