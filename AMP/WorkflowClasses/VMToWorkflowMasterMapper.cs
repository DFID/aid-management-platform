using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.Models;
using AMP.ViewModels;
using AutoMapper;

namespace AMP.WorkflowClasses
{
    public class VMToWorkflowMasterMapper
    {
        public WorkflowMaster MapVMToWorkflowMaster(WorkflowMasterVM workflowMasterVm)
        {
            WorkflowMaster workflowMaster = new WorkflowMaster();

            Mapper.CreateMap<WorkflowMasterVM, WorkflowMaster>();

            Mapper.Map<WorkflowMasterVM, WorkflowMaster>(workflowMasterVm, workflowMaster);

            return workflowMaster;

        }

    }
}