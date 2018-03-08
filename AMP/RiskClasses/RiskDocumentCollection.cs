using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.Services;
using AMP.ViewModels;

namespace AMP.RiskClasses
{
    public class RiskDocumentCollection
    {
        private IEnumerable<RiskDocument> _riskDocuments;
        private IDocumentService _documentService;
        
        public RiskDocumentCollection(IEnumerable<RiskDocument> riskDocuments, IDocumentService documentService)
        {
            _riskDocuments = riskDocuments;
            _documentService = documentService;
        }
        public IEnumerable<RiskDocument> AllRiskDocuments()
        {
            return _riskDocuments;
        }
        public bool HasRiskDocuments()
        {
            if (_riskDocuments != null && _riskDocuments.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
         }

        //Do something that maps the Risk Documents into a View Model.
        public List<RiskDocumentVM> CreateRiskDocumentsVM()
        {
            List<RiskDocumentVM> riskDocumentsVm = new List<RiskDocumentVM>();

            foreach (RiskDocument document in _riskDocuments)
            {
                RiskDocumentVM documentVm = new RiskDocumentVM
                {
                    ProjectID = document.ProjectID,
                    DocumentID = document.DocumentID,
                    Description = document.Description,
                    DocumentLink = _documentService.ReturnDocumentUrl(document.DocumentID, document.DocSource),
                    LastUpdate = document.LastUpdate,
                    RiskRegisterID = document.RiskRegisterID,
                    UserID = document.UserID
                };
                riskDocumentsVm.Add(documentVm);
            }

            return riskDocumentsVm;
        }


    }
}