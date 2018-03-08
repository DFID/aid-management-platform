using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;

namespace AMP.ViewModels
{
    public class AllReturnedPartnerListsVM
    {
            

       public IEnumerable<SupplierMasterKVP> DFIDPartnersSearchResult { get; set; }

        public IEnumerable<PartnerMasterKVP> AMPPartnerSearchResult { get; set; }


        }
    }

