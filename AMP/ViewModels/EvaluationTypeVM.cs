using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class EvaluationTypeVM
    {
        public IEnumerable<EvaluationTypeValuesVM> EvaluationTypeValues{ get; set; }
        public string SelectedEvaluationType { get; set; }
    }
}