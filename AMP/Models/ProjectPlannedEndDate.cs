//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectPlannedEndDate
    {
        public int Identity { get; set; }
        public string ProjectID { get; set; }
        public System.DateTime CurrentPlannedEndDate { get; set; }
        public System.DateTime NewPlannedEndDate { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }
        public Nullable<int> WorkTaskID { get; set; }
    }
}
