using System;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class MilestoneVM
    {
        public int ID { get; set; }
        public string ProjectID { get; set;}
        public int? OutputID { get; set; }
        public int? IndicatorID { get; set; }
        public int? MilestoneID { get; set; }
        public int From_Day { get; set; }
        public int From_Month { get; set; }
        public int From_Year { get; set; }
        public DateTime? From { get; set; }
        public int To_Day { get; set; }
        public int To_Month { get; set; }
        public int To_Year { get; set; }
        public DateTime? To { get; set; }
        
        [Required(ErrorMessage = "You must enter the planned milestone value")]        
        public string Planned { get; set; }

        public string Achieved { get; set; }
        public string Status { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UserID { get; set; }
        public string Change { get; set; }
    }
}