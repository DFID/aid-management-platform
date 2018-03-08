using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class DeliveryChainListVM
    {
        public int ID { get; set; }
        public string ComponentID { get; set; }
        public string ChainID { get; set; }
        public string ParentNodeID { get; set; }
        public string ParentID { get; set; }

        public string ParentName { get; set; }

        public string ParentType { get; set; }
        public string ChildID { get; set; }

        public string ChildName { get; set; }
        public string ChildType { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }
        public string Status { get; set; }
    }
}