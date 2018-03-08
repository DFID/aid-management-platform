using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.Models;
using AMP.Services;
using AMP.Utilities;
using AMP.ViewModels;

namespace AMP.WorkflowClasses
{
    public class CompletedWorkflow: CompletedWorkflowVM
    {
        private IAMPRepository _ampRepository;
        private IDocumentService _documentService;

        public CompletedWorkflow(IAMPRepository ampRepository, IDocumentService documentService)
        {
            _ampRepository = ampRepository;
            _documentService = documentService;
        }

        public async Task Construct(WorkflowRequest request, WorkflowResponse response)
        {
            SetRequestViewModel(await request.MapWorkflowMasterToWorkflowMasterVm());
            SetResponseViewModel(await response.MapWorkflowMasterToWorkflowMasterVm());
            SetTaskDescription(request.workflow.TaskID);
            SetWorkflowDocument();

        }

        private void SetRequestViewModel(WorkflowMasterVM requestVM)
        {
            WorkflowRequest = requestVM;
        }
        private void SetResponseViewModel(WorkflowMasterVM responseVM)
        {
            WorkflowResponse = responseVM;
        }

        private void SetTaskDescription(Int32 taskId)
        {
            TaskDescription = AMPUtilities.GetEnumDescription((WorkflowType)(taskId));
        }

        private void SetWorkflowDocument()
        {
            Int32 requestId = WorkflowRequest.WorkFlowID;
            WorkflowDocument document = new WorkflowDocument(_ampRepository, requestId, _documentService);
            document.GetDocument();
            DocumentID = document.DocumentID;
            DocumentDescription = document.DocumentDescription;
            DocSource = document.DocSource;
            DocumentLink = document.DocumentLink;
        }

    }
}