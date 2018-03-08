using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.Models;
using AMP.Services;
using AMP.ViewModels;
using AutoMapper;

namespace AMP.WorkflowClasses
{
    public class WorkflowBase
    {
        protected IPersonService _personService;
        protected WorkflowMaster _workflow;
        protected WorkflowMasterVM _WorkflowMasterVm;

        public WorkflowMaster workflow
        {
            get
            {
                return _workflow;
            }

            set
            {
                _workflow = value;
            }
        }

        #region methods

        public async Task<WorkflowMasterVM> MapWorkflowMasterToWorkflowMasterVm()
        {
            WorkflowMasterVM workflowMasterVm = new WorkflowMasterVM();

            Mapper.CreateMap<WorkflowMaster, WorkflowMasterVM>();

            Mapper.Map<WorkflowMaster, WorkflowMasterVM>(_workflow, workflowMasterVm);

            if (workflowMasterVm.ActionBy != null)
            {
                workflowMasterVm.RequesterName = await SetDisplayName(workflowMasterVm.ActionBy);
            }

            return workflowMasterVm;

        }



        private async Task<string> SetDisplayName(string userId)
        {
            string displayName = "";
            PersonDetails personDetails = await _personService.GetPersonDetails(userId);
            if (personDetails != null)
            {
                displayName = personDetails.Forename + " " + personDetails.Surname;
            }

            return displayName;
        }


        #endregion


    }
}