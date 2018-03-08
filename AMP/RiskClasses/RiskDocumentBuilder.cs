using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;
using AutoMapper;

namespace AMP.RiskClasses
{
    public class RiskDocumentBuilder
    {
        private IAMPRepository _ampRepository;
        private RiskDocumentVM _riskDocumentVm;
        private string _user;
        private RiskDocument _riskDocument;
        public RiskDocument RiskDocument
        {
            get { return _riskDocument; }
        }
        public RiskDocumentBuilder(IAMPRepository ampRepository, RiskDocumentVM riskDocumentVm, string user)
        {
            _ampRepository = ampRepository;
            _riskDocumentVm = riskDocumentVm;
            _user = user;
        }
        public void BuildRiskDocument()
        {
            _riskDocument = new RiskDocument();
            Mapper.CreateMap<RiskDocumentVM, RiskDocument>();
            Mapper.Map<RiskDocumentVM, RiskDocument>(_riskDocumentVm, _riskDocument);
            _riskDocument.DocSource = "V";
            _riskDocument.LastUpdate = DateTime.Now;
            _riskDocument.UserID = _user;
        }
    }
}
