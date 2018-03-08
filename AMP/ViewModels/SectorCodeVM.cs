using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class SectorCodeVM
    {
        public int LineNo { get; set; }
        public string ISCode { get; set; }

        public string ISDescription { get; set; }
        public int Percentage { get; set; }
    }
}
