using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class EditInputSectorsVM
    {
        public string CompID { get; set; }
        public SectorCodeVM [] SectorCodesCodeVm { get; set; } 
    }
}