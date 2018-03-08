using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace AMP.ViewModels
{
    public class ProjectFinanceVM
    {
        public string FinanceWebServiceMessage { get; set; }
        public IEnumerable<ProjectFinanceRecordVM> ProjectFinance { get; set; }
        public ProjectHeaderVM ProjectHeader { get; set; }
    }
}