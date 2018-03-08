using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.ARIESModels;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.Component_Classes
{
    public class DeliveryChainBuilder:DeliveryChain
    {
        private string _user;
        private DeliveryChainVM _deliveryChainVm;
        private SupplierVM _supplierVm;
        private DeliveryChain _deliveryChain;
        private string _componentId;
        protected IAMPRepository _ampRepository;
        protected IARIESService _AriesService;


       

        public DeliveryChainBuilder(DeliveryChainVM deliveryChainVM, IAMPRepository ampRepository, IARIESService AriesService, string user)
        {
            _user = user;
            _deliveryChainVm = deliveryChainVM;
            _ampRepository = ampRepository;
            _AriesService = AriesService;
        }

        public DeliveryChainBuilder(SupplierVM supplierVm, IAMPRepository ampRepository, IARIESService AriesService, string componentId, string user)
        {
            _user = user;
            _componentId = componentId;
            _supplierVm = supplierVm;
            _ampRepository = ampRepository;
            _AriesService = AriesService;
        }

        public async Task<DeliveryChain> BuildFromChain()
        {
            _deliveryChain = new DeliveryChain
            {
                ComponentID = _deliveryChainVm.ComponentID,
                ParentID = _deliveryChainVm.ParentID,
                ParentType = _deliveryChainVm.ParentType,
                //ParentName = _deliveryChainVm.ParentName,
               ParentNodeID = _deliveryChainVm.ID,
                LastUpdate = DateTime.Now,
                Status = "A",
                UserID = _user
            };
            await SetChildName();

            return _deliveryChain;

        }

        public DeliveryChain BuildFromSupplier()
        {
            _deliveryChain = new DeliveryChain
            {
                ComponentID = _componentId,
                ParentID = Int32.Parse(_supplierVm.SupplierID),
                ChildID = Int32.Parse(_supplierVm.SupplierID),
             //   ParentName = _supplierVm.SupplierName,
            //    ChildName = _supplierVm.SupplierName,
                ParentType = "S", // First Tier Suppliers will have both Parent and Child Types set to S
                ChildType = "S",
                LastUpdate = DateTime.Now,
                Status = "A",
                UserID = _user
            };

            return _deliveryChain;
        }


        public DeliveryChain BuildFromPartner()
        {
            _deliveryChain = new DeliveryChain
            {
                ComponentID = _componentId,
                ParentID = Int32.Parse(_supplierVm.SupplierID),
                ChildID = Int32.Parse(_supplierVm.SupplierID),           
                ParentType = "P", // First Tier Suppliers will have both Parent and Child Types set to P
                ChildType = "P",
                LastUpdate = DateTime.Now,
                Status = "A",
                UserID = _user
            };

            return _deliveryChain;
        }
        public async Task SetChildName()
        {

            
            if (_deliveryChainVm.SupplierEntry =="True") // supplier alrerady exists
            {
               _deliveryChain.ChildID = _deliveryChainVm.ChildID;
               _deliveryChain.ChildType = "S"; //adding a supplier as a child
            //    _deliveryChain.ChildName = suppliers.FirstOrDefault().SupplierName;
            }


            else if (_deliveryChainVm.PartnerEntry == "True") // partner alrerady exists 
            {
                _deliveryChain.ChildID = _deliveryChainVm.ChildID;
                _deliveryChain.ChildType = "P"; //adding a partner as a child
             //   _deliveryChain.ChildName = _deliveryChainVm.NewChildName;

            }

            else // adding a new row to the partner table
            {
                // Commit on creation of the new row in the table and then
                // retrieve the information based on the NewChildName Entered if needed

                _deliveryChain.ChildType = "P"; //adding a partner
                //_deliveryChain.ChildName = _deliveryChainVm.NewChildName;
                _deliveryChain.ChildID = _ampRepository.NextPartnerID();               
             
                //Partner Builder and insertion of the new row into the pratner master table
                PartnerBuilder partnerBuilder = new PartnerBuilder(_deliveryChainVm, _ampRepository, _AriesService, _user);
                PartnerMaster partnerMaster = await partnerBuilder.Build();
                _ampRepository.InsertPartner(partnerMaster);
                _ampRepository.Save();
            }
        }

    }
}