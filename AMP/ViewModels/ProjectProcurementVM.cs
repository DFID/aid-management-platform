using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class ProjectProcurementVM
    {
        public IEnumerable<ProcurementRecordVM> ProjectProcurement { get; set; }
        public string FinanceWebServiceMessage { get; set; }
        public ProjectHeaderVM ProjectHeader { get; set; }
    }
}