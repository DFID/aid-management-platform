using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ProjectMarkersVM
    {

        public ProjectHeaderVM ProjectHeader { get; set; }

        public IEnumerable<CCOValuesVM> CCOCollection { get; set; }
        public CrossCuttingObjectiveVM HIVCCO { get; set; }
        public CrossCuttingObjectiveVM GenderCCO { get; set; }
        public CrossCuttingObjectiveVM BiodiversityCCO { get; set; }
        public CrossCuttingObjectiveVM MitigationCCO { get; set; }
        public CrossCuttingObjectiveVM AdaptationCCO { get; set; }
        public CrossCuttingObjectiveVM DesertificationCCO { get; set; }

        public CrossCuttingObjectiveVM DisabilityCCO { get; set; }
        public string GenderEquality { get; set; }
        public string HIVAIDS { get; set; }
        public string Biodiversity { get; set; }
        public string Mitigation { get; set; }
        public string Adaptation { get; set; }
        public string Desertification { get; set; }
        public string Status { get; set; }
        public ProjectWFCheckVM WFCheck { get; set; }

        public string BudgetCentreDescription { get; set; }

        public  string BudgetCentreID { get; set; }
        public string StageDescription { get; set; }

        public string OpStatus { get; set; }
        public string Disability { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Percentage must be a whole number between 1 and 99")]
        public int? DisabilityPercentage { get; set; }
     
    }
}