using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class ComponentMasterVM
    {

        public string ComponentID { get; set; }

        [Required(ErrorMessage = "You must enter a title")]
        [MinLength(20, ErrorMessage = "Your component title must contain at least 20 characters")]
        public string ComponentDescription { get; set; }
        public string InputterID { get; set; }
        public string QualityAssurer { get; set; }
        public string BudgetCentreID { get; set; }
        public string ProjectID { get; set; }
        public string DFIDRole { get; set; }
        public string AdminApprover { get; set; }
        public string FundingMechanism { get; set; }
        public string OperationalStatus { get; set; }
        public string BenefittingCountry { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }
        public string Approved { get; set; }


        
    }
}


