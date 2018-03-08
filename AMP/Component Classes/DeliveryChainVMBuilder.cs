using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.Component_Classes
{
    public class PartnerVMBuilder
    {
        public DeliveryChain _DeliveryChain;
        private DeliveryChainVM _deliveryChainVm;
        public PartnerVMBuilder(DeliveryChain deliveryChain)
        {
            _DeliveryChain = deliveryChain;
        }

        public DeliveryChainVM BuildPartnerVM()
        {
            _deliveryChainVm = new DeliveryChainVM();

            _deliveryChainVm.ComponentID = _DeliveryChain.ComponentID;
            _deliveryChainVm.ParentID = _DeliveryChain.ChildID;
            _deliveryChainVm.ParentType = _DeliveryChain.ChildType;
            //_deliveryChainVm.ParentName = _DeliveryChain.ChildName;

            return _deliveryChainVm;
        }
    }
}