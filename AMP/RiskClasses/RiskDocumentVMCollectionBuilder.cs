using System;
using System.Collections.Generic;
using AMP.Models;
using AMP.ViewModels;
using AMP.Services;


namespace AMP.RiskClasses
{
    public class RiskDocumentVMCollectionBuilder
    {
        private List<RiskDocument>_riskDcDocuments;
        private IDocumentService _documentService;
        public RiskDocumentVMCollectionBuilder(List<RiskDocument> riskDocuments, IDocumentService documentService)
        {
            _riskDcDocuments = riskDocuments;
            _documentService = documentService;
        }
        public List<RiskDocumentVM> BuildRiskDocumentVMList()
        {
            List<RiskDocumentVM> RiskDocumentsVM = new List<RiskDocumentVM>();

            foreach (var riskDcDocument in _riskDcDocuments)
            {
                RiskDocumentVM riskDocumentVm = new RiskDocumentVM
                {
                    RiskRegisterID = riskDcDocument.RiskRegisterID,
                    ProjectID = riskDcDocument.ProjectID,
                    DocumentID = riskDcDocument.DocumentID,
                    DocSource = riskDcDocument.DocSource,
                    DocumentLink = _documentService.ReturnDocumentUrl(riskDcDocument.DocumentID, riskDcDocument.DocSource),
                    Description = riskDcDocument.Description,
                    LastUpdate = riskDcDocument.LastUpdate,
                    UserID = riskDcDocument.UserID
                };

                RiskDocumentsVM.Add(riskDocumentVm);
            }

            return RiskDocumentsVM;
        }
    }
}
