using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.Services;
using AMP.ViewModels;
using AutoMapper;

namespace AMP.WorkflowClasses
{
    public class WorkflowResponse: WorkflowBase
    {

        #region Constructors
        public WorkflowResponse(IPersonService personService)
        {
            _personService = personService;
        }

        #endregion

        #region properties

        //private WorkflowMaster workflowResponse;

        public WorkflowMaster response
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

        #endregion

        #region methods


        public void CreateResponse(WorkflowMaster request)
        {
            WorkflowMaster response = new WorkflowMaster
            {
                ProjectID = request.ProjectID,
                TaskID = request.TaskID,
                ActionBy = request.Recipient
            };

            _workflow = response;
        }
        #endregion

    }


}