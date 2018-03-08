using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.ARIESModels;
using AMP.Models;

namespace AMP.ViewModels
{
    public class ComponentPartnerVM
    {
        public ProjectHeaderVM ProjectHeaderVm { get; set; }
        public ComponentHeaderVM ComponentHeaderVm { get; set; }
        public  DeliveryChainsVM DeliveryChainsVm { get; set; }
        public List<PartnersTier1> Tier1Partners { get; set; } 

        public DeliveryChainVM DeliveryChainVm { get; set; }

        // this is needed to determine if the delivery chain 
        // is locked down - only accessible to team members
        public ProjectVM ProjectVm { get; set; }


    }
}