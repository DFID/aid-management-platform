using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class InputSectorVM
    {
        public string ComponentID { get; set; }
        public int LineNo { get; set; }

         [Required(ErrorMessage = "You must enter an Input Sector")]
         public string InputSectorCode1 { get; set; }

         public string InputSectorCodeDescription { get; set; }

         [Required(ErrorMessage = "You must enter a percentage")]
         [Range(1, 100, ErrorMessage = "Percentage must be between 1 and 100")]
        
        public Nullable<int> Percentage { get; set; }
    }
}