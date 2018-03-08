using System;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class IndicatorVM
    {
        public int ID { get; set; }
        public string ProjectID { get; set; }
        public int? OutputID { get; set; }
        public int? IndicatorID { get; set; }

        [Required(ErrorMessage = "You must enter a name")]
        [StringLength(500, MinimumLength = 25, ErrorMessage = "Indicator name must contain at least 25 characters")]
        public string IndicatorDescription { get; set; }
        public string Source { get; set; }
        public string Units { get; set; }
        public string Baseline { get; set; }
        public DateTime? BaselineDate { get; set; }
        public int BaselineDate_Day { get; set; }
        public int BaselineDate_Month { get; set; }
        public int BaselineDate_Year { get; set; }
        public string Target { get; set; }
        public DateTime? TargetDate { get; set; }
        public int TargetDate_Day { get; set; }
        public int TargetDate_Month { get; set; }
        public int TargetDate_Year { get; set; }
        public string TargetAchieved { get; set; }
        public string IsDRF { get; set; }
        public string IsCHR { get; set; }
        public string Status { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UserID { get; set; }
    }
}