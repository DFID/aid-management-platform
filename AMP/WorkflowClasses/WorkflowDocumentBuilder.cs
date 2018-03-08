using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.WorkflowClasses
{
    public class WorkflowDocumentBuilder
    {
        private string _documentId;
        private string _description;
        private Int32 _workflowId;
        private string _userId;
        public Models.WorkflowDocument Document { get; private set; }


        public WorkflowDocumentBuilder(string documentId, string description, Int32 workflowId, string userId)
        {
            _documentId = documentId;
            _description = description;
            _workflowId = workflowId;
        }

        public void Build()
        {
            Document = new Models.WorkflowDocument
            {
                DocumentID = _documentId,
                Description = _description,
                WorkflowID = _workflowId,
                DocSource = "V",
                LastUpdate = DateTime.Now,
                Status = "A",
                UserID = _userId

            };
        }

    }
}