using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class CrossCuttingObjectiveVM
    {
        public string CCOType { get; set; }
        public IEnumerable<CCOValuesVM> CCOValues { get; set; }
        public string SelectedCCOValue { get; set; }
    }
}