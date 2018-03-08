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
    public class PartnerBuilder:PartnerMaster
    {
        private string _user;
        private DeliveryChainVM _deliveryChainVm;
        private PartnerMaster _partnerMaster;
        private string _componentId;
        protected IAMPRepository _ampRepository;
        protected IARIESService _AriesService;


        public PartnerBuilder(DeliveryChainVM deliveryChainVM, IAMPRepository ampRepository, IARIESService AriesService, string user)
        {
            _user = user;
            _deliveryChainVm = deliveryChainVM;
            _ampRepository = ampRepository;
            _AriesService = AriesService;
        }


        public async Task<PartnerMaster> Build()
        {
            _partnerMaster = new PartnerMaster
            {
                PartnerID = _ampRepository.NextPartnerID(),
                PartnerName = _deliveryChainVm.NewChildName,
                Status = "A",
                LastUpdated = DateTime.Now,
                UserID = _user
            };
            return _partnerMaster;
        }


    }
}