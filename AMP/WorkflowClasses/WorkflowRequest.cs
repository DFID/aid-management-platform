using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.Models;
using AMP.Services;
using AMP.Utilities;
using AMP.ViewModels;
using AutoMapper;

namespace AMP.WorkflowClasses
{
    public class WorkflowRequest:WorkflowBase
    {
        #region Constructor

        public WorkflowRequest(IPersonService personService)
        {
            _personService = personService;
        }

        public WorkflowRequest(IPersonService personService, WorkflowMasterVM requestVm)
        {
            _personService = personService;
            _WorkflowMasterVm = requestVm;
        }


        #endregion

        #region properties

        //private WorkflowMaster workflowRequest;

        #endregion

        #region methods


        public void CreateRequest(string projectId, int taskId, string user)
        {
            WorkflowMaster request = new WorkflowMaster
            {
                ProjectID = projectId,
                TaskID = taskId,
                ActionBy = user
            };

            _workflow = request;
        }


        #endregion

    }


}