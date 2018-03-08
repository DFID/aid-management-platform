using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.Services;

namespace AMP.WorkflowClasses
{
    public class WorkflowDocument
    {
        public string DocumentID { get; set; }
        public string DocumentDescription { get; set; }
        public string DocSource { get; set; }
        public string DocumentLink { get; set;}

        private IAMPRepository _ampRepository;
        private Int32 _workflowId;
        private IDocumentService _documentService;

        public WorkflowDocument(IAMPRepository ampRepository, Int32 workflowId, IDocumentService documentService)
        {
            _ampRepository = ampRepository;
            _workflowId = workflowId;
            _documentService = documentService;
        }

        public void GetDocument()
        {
            Models.WorkflowDocument workflowDocument = _ampRepository.GetWorkflowDocument(_workflowId);

            if (workflowDocument != null)
            {
                DocumentID = workflowDocument.DocumentID;
                DocumentDescription = workflowDocument.Description;
                DocSource = workflowDocument.DocSource;
                DocumentLink = _documentService.ReturnDocumentUrl(workflowDocument.DocumentID,
                    workflowDocument.DocSource);
            }
        }

    }
}