using System;
using System.Threading.Tasks;
using AMP.Models;
using AMP.Services;
using AMP.Utilities;
using AMP.ViewModels;

namespace AMP.WorkflowClasses
{
    public class WorkflowObject:WorkflowVM
    {

        private IAMPRepository _ampRepository;
        private WorkflowConfiguration configuration = new WorkflowConfiguration();
        private IDocumentService _documentService;
        
        public WorkflowObject(IAMPRepository ampRepository, int taskId, IDocumentService documentService)
        {
            _ampRepository = ampRepository;
            TaskID = taskId;
            _documentService = documentService;
        }

        public async Task Construct(WorkflowRequest request, WorkflowResponse response,string userRole)
        {
            string projectStage = _ampRepository.GetProject(request.workflow.ProjectID).Stage;

            SetRequestViewModel(await request.MapWorkflowMasterToWorkflowMasterVm());
            SetResponseViewModel(await response.MapWorkflowMasterToWorkflowMasterVm());
            SetApprovalType(configuration.RequiresDelegatedAuthorityApproval(TaskID));
            SetDocumentRequired(configuration.DocumentRequired(TaskID, projectStage));
            SetTaskDescription(TaskID);
            SetWorkflowDocument();
            SetUserRole(userRole);
        }

        private void SetRequestViewModel(WorkflowMasterVM  requestVM)
        {
            WorkflowRequest = requestVM;
        }
        private void SetResponseViewModel(WorkflowMasterVM responseVM)
        {
            WorkflowResponse = responseVM;
        }

        private void SetApprovalType(bool delegatedAuthorityRequired)
        {
            RequiresDelegatedAuthorityApproval = delegatedAuthorityRequired;
        }

        private void SetDocumentRequired(bool documentRequired)
        {
            IsDocumentRequired = documentRequired;
        }

        private void SetTaskDescription(Int32 taskId)
        {   
            TaskDescription = AMPUtilities.GetEnumDescription((WorkflowType)(taskId));
        }

        private void SetWorkflowDocument()
        {
            Int32 requestId = WorkflowRequest.WorkFlowID;
            WorkflowDocument document = new WorkflowDocument(_ampRepository,requestId, _documentService);
            document.GetDocument();
            DocumentID = document.DocumentID;
            DocumentDescription = document.DocumentDescription;
            DocSource = document.DocSource;
            DocumentLink = document.DocumentLink;

        }

        private void SetUserRole(string role)
        {
            UserRole = role;
        }




    }


}