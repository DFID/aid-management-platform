using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.ARIESModels;
using AMP.Models;
using AMP.ViewModels;
using MoreLinq;
using AutoMapper;
using AutoMapper.Mappers;

namespace AMP.Component_Classes
{
    public class ComponentPartnerVMBuilder
    {
        protected IAMPRepository _ampRepository;
        protected IARIESService _AriesService;
        private string _ComponentID;
        private ComponentPartnerVM _componentPartnerVm;
        public ComponentPartnerVMBuilder(IAMPRepository ampRepository,IARIESService ariesService, string componentId)
        {
            _AriesService = ariesService;
            _ampRepository = ampRepository;
            _ComponentID = componentId;


        }

        public async Task<ComponentPartnerVM> Build()
        {
            _componentPartnerVm = new ComponentPartnerVM();

            ComponentMaster componentMaster = _ampRepository.GetComponent(_ComponentID);
            ProjectMaster projectMaster = _ampRepository.GetProject(componentMaster.ProjectID);

            //Get the chain from DB
            List<DeliveryChain> deliveryChains = _ampRepository.GetDeliveryChainsByComponent(_ComponentID).OrderBy(x => x.ChainID).ToList();

            //New Delivery Chain list object
            List<DeliveryChainListVM> deliveryChainListVms;

            //Map Chain to VM
            Mapper.CreateMap<DeliveryChain, DeliveryChainListVM>();
            deliveryChainListVms = Mapper.Map<List<DeliveryChain>, List<DeliveryChainListVM>>(deliveryChains);

            //Get Descriptions for all 
            List<int> listOfPartners = new List<int>();
            List<string> listOfSuppliers = new List<string>();
            

            foreach (DeliveryChainListVM chain in deliveryChainListVms)
            {
                //Check Parent
                if (chain.ParentType=="P")
                {
                    listOfPartners.Add(Int32.Parse(chain.ParentID));
                }
                if (chain.ParentType == "S")
                {
                    listOfSuppliers.Add(chain.ParentID.ToString());
                }

                //Check Child
                if (chain.ChildType == "P")
                {
                    listOfPartners.Add(Int32.Parse(chain.ChildID));
                }
                if (chain.ChildType == "S")
                {
                    listOfSuppliers.Add(chain.ChildID.ToString());
                }
            }

            //Get List of IDs and Names from PartnerMaster based on list of Partners
            List<PartnerMaster> partnerMasters = _ampRepository.GetDeliveryChainsByIDList(listOfPartners);
            
            //

            //Get List of IDs and Names from ARIES Suppliers based on list of Suppliers
            IEnumerable<SupplierVM> SupplierVms = await _AriesService.GetSuppliers(listOfSuppliers, "");


            //Map description 
            foreach (DeliveryChainListVM chainList in deliveryChainListVms)
            {
                if (chainList.ParentType == "P")
                {
                    foreach (PartnerMaster partners in partnerMasters)
                    {
                        if (Int32.Parse(chainList.ParentID) == partners.PartnerID)
                        {
                            chainList.ParentName = partners.PartnerName;
                        }
                    }
                }
                if (chainList.ParentType == "S")
                {
                    foreach (SupplierVM suppliers in SupplierVms)
                    {
                        if (Int32.Parse(chainList.ParentID) == Int32.Parse(suppliers.SupplierID))
                        {
                            chainList.ParentName = suppliers.SupplierName;
                        }
                    }
                }

                //Check Child
                if (chainList.ChildType == "P")
                {
                    foreach (PartnerMaster partners in partnerMasters)
                    {
                        if (Int32.Parse(chainList.ChildID) == partners.PartnerID)
                        {
                            chainList.ChildName = partners.PartnerName;
                        }
                    }
                }
                if (chainList.ChildType == "S")
                {
                    foreach (SupplierVM suppliers in SupplierVms)
                    {
                        if (Int32.Parse(chainList.ChildID) == Int32.Parse(suppliers.SupplierID))
                        {
                            chainList.ChildName = suppliers.SupplierName;
                        }
                    }
                }
            }

            List<PartnersTier1> tier1Partners = await _AriesService.GetTier1Partners(_ComponentID);
            DeliveryChainsVM deliveryChainsVM = new DeliveryChainsVM();

            //Here we would need to map in all the descriptions. 4 loops? 2 for each ID type and 2 for parent and child.
            //We will need 2 get methods. They will take a list of ID's and return ID's and names, one for partner and one for supplier.
            //Then we can set deliveryChains to the view model.

            deliveryChainsVM.deliveryChains = deliveryChainListVms;
            _componentPartnerVm.DeliveryChainsVm = deliveryChainsVM;
            _componentPartnerVm.Tier1Partners = tier1Partners;

            DeliveryChainVM deliveryChainVm = new DeliveryChainVM();

            _componentPartnerVm.DeliveryChainsVm = deliveryChainsVM;

            return _componentPartnerVm;
        }
    }
}