using System;
using System.ComponentModel.DataAnnotations;
using AMP.Models;

namespace AMP.ViewModels
{
    public class ReviewOutputVM
    {
        public string ProjectID { get; set; }
        public int ReviewID { get; set; }
        public Nullable<int> OutputID { get; set; }

        [Required(ErrorMessage = "You must enter description")]
        [StringLength(500, ErrorMessage = "Output description cannot be more than 500 characters")]
        public string OutputDescription { get; set; }
        
        [Range(1, 100, ErrorMessage = "Please enter value between 1 to 100")]
        [RegularExpression(@"^\d+$", ErrorMessage = "No decimal allowed")]
        [Required(ErrorMessage = "Weight is required")]
        public int Weight { get; set; }
        [Required(ErrorMessage = "Please select performance rating")]
        public string OutputScore { get; set; }        
        public Nullable<double> ImpactScore { get; set; }
        [Required(ErrorMessage = "Please select risk type")]
        public string Risk { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }
        public virtual ReviewScore ReviewScore { get; set; }


    }
}