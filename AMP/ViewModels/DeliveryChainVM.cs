using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class DeliveryChainVM
    {
        public int ID { get; set; }
        public string ComponentID { get; set; }
        public string ChainID { get; set; }
        public int ParentID { get; set; }
        public string ParentType { get; set; }
        public string ParentName { get; set; }
        public int ChildID { get; set; }
        public string ChildType { get; set; }
        public string ChildName { get; set; }
        public string NewChildName { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }


    //This will help the service layer undestand if a partner or supplier has been selected
        public string PartnerEntry { get; set; }
        public string SupplierEntry { get; set; }

        public string NewPartnerEntry { get; set; }
        public ComponentHeaderVM ComponentHeader { get; set; }

        //This will route the add/replace partner appropriately
        public string AddOrReplace { get; set; }

    }
}