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
    
    public partial class BenefitingCountry
    {
        public BenefitingCountry()
        {
            this.ComponentMasters = new HashSet<ComponentMaster>();
        }
    
        public string BenefitingCountryID { get; set; }
        public string BenefitingCountryDescription { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }
    
        public virtual ICollection<ComponentMaster> ComponentMasters { get; set; }
    }
}