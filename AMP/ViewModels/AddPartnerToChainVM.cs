using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;

namespace AMP.ViewModels
{
    public class AddPartnerToChainVM
    {

        public AllReturnedPartnerListsVM SearchResults { get; set; }

        public ComponentHeaderVM ComponentHeader { get; set; }

        public DeliveryChainVM ChainToBeAddedTo { get; set; }

        public DeliveryChainVM NewChainToBeAdded { get; set; }



    }
}