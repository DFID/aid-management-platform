using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public partial class AdvanceSearchVM
    {
        public PagedList<AdvanceSearchViewModel> projects { get; set; }
        //public IEnumerable<ProjectAlert> projectalerts { get; set; }
        //public string ARIESWebServiceMessage { get; set; }
        public String ResultMsg { get; set; }

        public String AriesMsg { get; set; }
        public String SearchKeyWord { get; set; }
        //public String Stages { get; set; }
           [Required(ErrorMessage = "Select project stage")]
        public List<Stage> ProjectStages { get; set; }
        public string stage { get; set; }
        public int ProjectCount { get; set; }
        public string StatusChoice { get; set; }

        public string BenefittingCountryID { get; set; }
        public List<BenefitingCountry> BenefitingCountry { get; set; }

        public string BudgetCentreID { get; set; }
      //  public List<BudgetCentreKV> BudgetCentre { get; set; }
        public string BudgetCentreName { get; set; }

        public string SRO { get; set; }

        public string SROName { get; set; }

    }
}