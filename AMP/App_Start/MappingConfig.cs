using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.App_Start
{
    public class MappingConfig
    {
        public static void AutoMapperStartUp()
        {
            Mapper.CreateMap<ReviewExemptionVM, ReviewExemption>();

            Mapper.CreateMap<ReviewDeferralVM, ReviewDeferral>();

            Mapper.CreateMap<WorkflowMaster, WorkflowMasterVM>();

            Mapper.CreateMap<DeliveryChain, DeliveryChainVM>();


        }
    }
}