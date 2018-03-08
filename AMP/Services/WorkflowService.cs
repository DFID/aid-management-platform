using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Text;
using AMP.Models;
using AMP.Utilities;
using AMP.ViewModels;
using AMP.WorkflowClasses;
using AutoMapper;
using MoreLinq;

namespace AMP.Services
{
    public class WorkflowService : IWorkflowService
    {
        private IAMPRepository _ampRepository;
        private IPersonService _personService;
        private IErrorEngine _errorengine;
        private ILoggingEngine _loggingengine;
        private IValidationService _validationService;
        private IDocumentService _documentService;

        public WorkflowService(IAMPRepository ampRepository)
        {
            _ampRepository = ampRepository;
            _personService = new DemoPersonService();
            _validationService = new ValidationSevice(ampRepository);
        }

        public WorkflowService(IAMPRepository ampRepository, IPersonService personService,ILoggingEngine loggingEngine, IErrorEngine errorEngine, IDocumentService documentService)
        {
            _ampRepository = ampRepository;
            _personService = personService;
            _loggingengine = loggingEngine;
            _errorengine = errorEngine;
            _documentService = documentService;

        }


        public async Task<WorkflowVM> WorkflowByProjectIdAndTaskId(string projectId, Int32 taskId, string user)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentException("String is null or empty", "projectId");
            }

            if (string.IsNullOrEmpty(user))
            {
                throw new ArgumentException("String is null or empty", "user");
            }

            ExistingWorkflows existingWorkflows = new ExistingWorkflows(_ampRepository.GetWorkflowMastersByProjectandTask(projectId, taskId));
            WorkflowRequest workflowRequest = new WorkflowRequest(_personService);
            WorkflowResponse workflowResponse = new WorkflowResponse(_personService);
            WorkflowConfiguration configuration = new WorkflowConfiguration();
            WorkflowObject workflowObj = new WorkflowObject(_ampRepository,taskId, _documentService);
            
            string projectStage = _ampRepository.GetProject(projectId).Stage;
            string userRole = "";

            if (taskId == Constants.PlannedEndDate) // set up the message to workflow screen for planned end date
            {
                ProjectPlannedEndDate projectPlannedEndDate = _ampRepository.GetProjectPlannedEndDate(projectId);
                string message = WorkflowMessages.PlannedEndDatePendingApprovalMessage(projectPlannedEndDate.NewPlannedEndDate);
                workflowObj.WfMessage = message;
            }

            if (existingWorkflows.HasActiveWorkflow())
            {
                workflowRequest.workflow = existingWorkflows.ActiveWorkflowRequest();
                workflowResponse.CreateResponse(workflowRequest.workflow);
                userRole = SetUserRole(user, projectId, workflowRequest.workflow.Recipient);
                
            }
            else if (existingWorkflows.LastWorkflowWasRejected())
            {
                workflowRequest.workflow = existingWorkflows.LastRejectedWorkflowRequest();
                workflowResponse.workflow = existingWorkflows.LastRejectedWorkflowResponse();
                userRole = SetUserRole(user, projectId, workflowRequest.workflow.Recipient); 
            }
            else
            {
                workflowRequest.CreateRequest(projectId,taskId,user);
                workflowResponse.response = new WorkflowMaster();
                userRole = SetUserRole(user, projectId, null);
                
            }

            await workflowObj.Construct(workflowRequest,workflowResponse,userRole);
           
            return workflowObj;


        }

        public async Task<WorkflowVM> WorkflowByWorkflowId(Int32 workflowId, string user)
        {
            string userRole;
            Int32 taskId;
            string projectId;

            ExistingWorkflows existingWorkflows = new ExistingWorkflows(_ampRepository.GetWorkflowMasters(workflowId));

            if (existingWorkflows.HasWorkflows())
            {
                WorkflowRequest request = new WorkflowRequest(_personService);
                WorkflowResponse response = new WorkflowResponse(_personService);
                WorkflowConfiguration configuration = new WorkflowConfiguration();
                WorkflowObject workflowObj;
                request.workflow = existingWorkflows.WorkflowRequest(workflowId);
                response.workflow = existingWorkflows.WorkflowResponse(workflowId);

                taskId = request.workflow.TaskID;
                projectId = request.workflow.ProjectID;
                string projectStage = _ampRepository.GetProject(projectId).Stage;

                workflowObj = new WorkflowObject(_ampRepository, request.workflow.TaskID, _documentService);
                userRole = SetUserRole(user, request.workflow.ProjectID, request.workflow.Recipient);

                await workflowObj.Construct(request,response,userRole);

                if (taskId == Constants.PlannedEndDate && response.workflow.StageID !=3)// set up the message to workflow screen for planned end date only if approved
                {
                    ProjectPlannedEndDate projectPlannedEndDate = _ampRepository.GetProjectPlannedEndDatForWorkflowHistory(workflowId);
                    string message = WorkflowMessages.PlannedEndDateWorkflowHistoryMessage(projectPlannedEndDate.CurrentPlannedEndDate, projectPlannedEndDate.NewPlannedEndDate);
                    workflowObj.WfMessage = message;
                }




                return workflowObj;
               

            }

            return null;


        }

        public async Task<WorkflowsVM> CompletedWorkflows(string projectId, string user)
        {
            ExistingWorkflows existingWorkflows = new ExistingWorkflows(_ampRepository.GetWorkflowMastersByProject(projectId));

            WorkflowsVM workflowsVm = new WorkflowsVM();
            List<CompletedWorkflowVM> completedWorkflowVms = new List<CompletedWorkflowVM>();
            List<Tuple<WorkflowMaster, WorkflowMaster>> completedTupleList = new List<Tuple<WorkflowMaster, WorkflowMaster>>();

            List<WorkflowMaster> completedRequests = existingWorkflows.CompletedRequests().OrderByDescending(x=>x.WorkFlowID).ToList();
            List<WorkflowMaster> completedResponses = existingWorkflows.CompletedResponses().OrderByDescending(x => x.WorkFlowID).ToList();

            for (int i = 0; i < completedRequests.Count(); i++)
            {
                WorkflowRequest request = new WorkflowRequest(_personService);
                WorkflowResponse response = new WorkflowResponse(_personService);
                request.workflow = completedRequests.ElementAt(i);
                response.workflow = completedResponses.ElementAt(i);

                CompletedWorkflow completed = new CompletedWorkflow(_ampRepository, _documentService);

                await completed.Construct(request,response);

                completedWorkflowVms.Add(completed);

            }

            workflowsVm.workflows = completedWorkflowVms;

            return workflowsVm;
        }

        public async Task<bool> StartWorkflow(WorkflowVM workflowvm, string user)
        {
            try
            {
                VMToWorkflowMasterMapper workflowMapper = new VMToWorkflowMasterMapper();

                WorkflowMaster request = workflowMapper.MapVMToWorkflowMaster(workflowvm.WorkflowRequest);
                RequestBuilder builder = new RequestBuilder(request, _ampRepository, user);

                builder.BuildWorkflowMaster();

                if (workflowvm.DocumentID != null)
                {
                    WorkflowDocumentBuilder documentBuilder = new WorkflowDocumentBuilder(workflowvm.DocumentID, workflowvm.DocumentDescription, builder.workflow.WorkFlowID, user);
                    documentBuilder.Build();
                    _ampRepository.InsertWorkflowDocument(documentBuilder.Document);

                }

                _ampRepository.InsertWorkFlowMaster(builder.workflow);
                _ampRepository.Save();

                // need to handle planned end date - the planned end date table row for the project
                // is inserted befre workflow task is created. Now the workflow is created
                // and being sennt for approval we need to update the newly created row for the project
                // in the planned end date table with the workflowTaskID
                if (workflowvm.WorkflowRequest.TaskID == Constants.PlannedEndDate)
                {
                    ProjectPlannedEndDate projectPlannedEndToUpdate =
                        _ampRepository.GetProjectPlannedEndDate(request.ProjectID);
                    projectPlannedEndToUpdate.WorkTaskID = request.WorkFlowID;
                    _ampRepository.UpdatePlannedEndDate(projectPlannedEndToUpdate);
                    _ampRepository.Save();
                }

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(workflowvm.WorkflowResponse.ProjectID, exception, user);
                return false;

            }

        }

        public void ApproveWorkflow(WorkflowVM workflowVm, string userId)
        {
            try
            {
                VMToWorkflowMasterMapper workflowMapper = new VMToWorkflowMasterMapper();

                WorkflowMaster request = workflowMapper.MapVMToWorkflowMaster(workflowVm.WorkflowRequest);
                WorkflowMaster response = workflowMapper.MapVMToWorkflowMaster(workflowVm.WorkflowResponse);

                ResponseBuilder responseBuilder = new ResponseBuilder(response,_ampRepository,userId);
                RequestModifier requestModifier = new RequestModifier(request);

                responseBuilder.BuildApprovalResponse();
                requestModifier.Modify();

                _ampRepository.InsertWorkFlowMaster(responseBuilder.workflow);
                _ampRepository.UpdateWorkFlowMaster(requestModifier.workflow);
                _ampRepository.Save();
            }
            catch (Exception exception)
            {
            }
        } 

        private string SetUserRole(string currentUser, string projectId, string approver)
        {
            //NOTE: Any changes to this code should be replicated in the class AMPServiceLayer,  Method ReturnCurrentUserMemberOfGroup. The method is almost identical and has been included as part of the
            //redevelopment of Workflow to be a service. Once project becomes a 'service' there will be one method, probably sitting in a single utility class.

            //Are you the approver?
            if (currentUser == approver)
                return "Approver";

            //Are you on the Team?

            //Get Current Team
            IEnumerable<Team> currentTeam = _ampRepository.GetTeam(projectId);

            if (IsTeamMember(currentUser, currentTeam))
            {
                return "Team";
            }
            else
            {
                return "Others";
            }

        }

        private bool IsTeamMember(string empNo, IEnumerable<Team> currentTeam)
        {
            //NOTE: Any changes to this code should be replicated in the class WorkflowService, IsTeamMember Method. The method is almost identical and has been included as part of the
            //redevelopment of Workflow to be a service. Once project becomes a 'service' there will be one method, probably sitting in a single utility class.

            return currentTeam.Any(x => x.TeamID.Trim().Equals(empNo.Trim()) && x.Status.Equals("A"));
        }

        #region Disposal Methods
        // Dispose Methods

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WorkflowService()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        #endregion


    }
}