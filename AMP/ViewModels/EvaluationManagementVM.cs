using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Web;

namespace AMP.ViewModels
{
    public class EvaluationManagementVM
    {
        public IEnumerable<EvaluationManagementValuesVM> EvaluatioNManagementValues { get; set; }

        public string SelectedEvaluationManagement { get; set; }
    }
}