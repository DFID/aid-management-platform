using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.Models;
using AMP.RiskClasses;
using AMP.Utilities;
using AMP.ViewModels;
using AutoMapper;
using MoreLinq;

namespace AMP.Services
{
    public class RiskService : IRiskService
    {
        private IAMPRepository _ampRepository;
        private IPersonService _personService;
        private IErrorEngine _errorengine;
        private ILoggingEngine _loggingengine;
        private IDocumentService _documentService;

        public RiskService(IAMPRepository ampRepository, IDocumentService documentService)
        {
            _ampRepository = ampRepository;
            _personService = new DemoPersonService();
            _documentService = documentService;
        }
        public RiskService(IAMPRepository ampRepository, IPersonService personService, ILoggingEngine loggingEngine, IErrorEngine errorEngine, IDocumentService documentService)
        {
            _ampRepository = ampRepository;
            _personService = personService;
            _loggingengine = loggingEngine;
            _errorengine = errorEngine;
            _documentService = documentService;
        }

        public async Task<RiskItemVM> GetRiskItem(Int32 Id, string user)
        {
            RiskItem riskItem = new RiskItem(_ampRepository);
            riskItem.ConstructRiskItem(Id);

            return riskItem;
        }

        public async Task<OverallRiskRatingVM> GetOverallRiskRatingItem(int overallriskratingId)
        {
            OverallRiskRatingItem overallRiskRating = new OverallRiskRatingItem(_ampRepository);
            overallRiskRating.ConstructOverallRiskRating(overallriskratingId);
            return overallRiskRating;
        }

        public RiskItemVM NewRiskItem(string projectId)
        {
            RiskItemVM riskItemVm = new RiskItemVM();
            riskItemVm.ProjectID = projectId;
            return riskItemVm;
        }

        public OverallRiskRatingVM NewOverallRiskRating(string projectId)
        {
            OverallRiskRatingVM overallRiskRatingVm = new OverallRiskRatingVM();
            overallRiskRatingVm.ProjectID = projectId;
            return overallRiskRatingVm;
        }

        public async Task<IEnumerable<RiskRegisterVM>> GetRiskRegisterDocuments(string projectId, string user)
        {
            IEnumerable<RiskRegisterVM> riskDocumentVms = new List<RiskRegisterVM>();
            List<RiskDocument> riskDocuments = _ampRepository.GetRiskRegisters(projectId);

            Mapper.CreateMap<RiskDocument, RiskRegisterVM>();
            riskDocumentVms = Mapper.Map<List<RiskDocument>, List<RiskRegisterVM>>(riskDocuments);
            return riskDocumentVms;
        }


        public async Task<RiskRegisterVM> RiskDetailsByProject(string projectId, string user)
        {
            RiskRegisterVM riskRegisterVm = new RiskRegisterVM();

            List<RiskCategory> riskCategories = _ampRepository.GetRiskCategories().ToList();
            List<RiskLikelihood> riskLikelihoods = _ampRepository.GetRiskLikelihoods().ToList();
            List<RiskImpact> riskImpacts = _ampRepository.GetRiskImpacts().ToList();
            List<Risk> riskRatings = _ampRepository.GetRisks().ToList();

            RiskRegisterCollection projectRiskCollection = new RiskRegisterCollection(_ampRepository.GetRiskRegister(projectId));

            if (projectRiskCollection.HasRiskItems())
            {
                List<RiskRegister> projectRisks = projectRiskCollection.AllRisks().ToList();
                RiskItemVMCollectionBuilder riskItemVmCollectionBuilder = new RiskItemVMCollectionBuilder(projectRiskCollection.AllRisks().ToList(), riskCategories, riskLikelihoods, riskImpacts, riskRatings);
                RiskItemsVM riskItemsVm = new RiskItemsVM();
                riskItemsVm.projectRisks = riskItemVmCollectionBuilder.BuildRiskItemVMList();
                riskRegisterVm.RiskItemsVm = riskItemsVm;
            }
            
            RiskDocumentCollection riskDocumentCollection = new RiskDocumentCollection(_ampRepository.GetRiskDocuments(projectId), _documentService);
            if (riskDocumentCollection.HasRiskDocuments())
            {
                riskRegisterVm.RiskDocuments = riskDocumentCollection.CreateRiskDocumentsVM();

                RiskDocumentVMCollectionBuilder riskDocumentVmCollectionBuilder = new RiskDocumentVMCollectionBuilder(riskDocumentCollection.AllRiskDocuments().ToList(), _documentService);
                RiskDocumentsVM riskDocumentsVm = new RiskDocumentsVM();
                riskDocumentsVm.ProjectRiskDocuments = riskDocumentVmCollectionBuilder.BuildRiskDocumentVMList();
                riskRegisterVm.RiskDocumentsVm = riskDocumentsVm;
            }
            //
            OverallRiskRatingCollection overallRiskCollection = new OverallRiskRatingCollection(_ampRepository.GetOverallRiskRatings(projectId));
            if (overallRiskCollection.HasOverallRiskRating())
            {
                riskRegisterVm.OverallRiskRatings = overallRiskCollection.CreateOverallRiskRatingVms();

                OverallRiskRatingVMCollectionBuilder overallRiskratingVmCollectionBuilder = new OverallRiskRatingVMCollectionBuilder(overallRiskCollection.AllOverallRiskRatingsRisks().ToList(), riskRatings);
                OverallRiskRatingsVM overallRatingsVm = new OverallRiskRatingsVM();
                overallRatingsVm.OverallRiskRatings = overallRiskratingVmCollectionBuilder.BuildOverallRiskRatingVMList();
                riskRegisterVm.OverallRiskRatingsVm = overallRatingsVm;
            }

            riskRegisterVm.RiskCategoryValues = _ampRepository.GetRiskCategories().ToList();
            riskRegisterVm.RiskLikelihoodValues = _ampRepository.GetRiskLikelihoods().ToList();
            riskRegisterVm.RiskImpactValues = _ampRepository.GetRiskImpacts().ToList();
            riskRegisterVm.RiskValues = _ampRepository.GetRisks().ToList();
            riskRegisterVm.ProjectID = projectId;

            return riskRegisterVm;
        }

        public bool PostRiskRegisterItem(RiskItemVM riskItemVm, string user)
        {
            riskItemVm.MitigationStrategy = AMPUtilities.CleanText(riskItemVm.MitigationStrategy);
            riskItemVm.Comments = AMPUtilities.CleanText(riskItemVm.Comments);
            riskItemVm.RiskDescription = AMPUtilities.CleanText(riskItemVm.RiskDescription);
            riskItemVm.ExternalOwner = AMPUtilities.CleanText(riskItemVm.ExternalOwner);
            RiskRegisterBuilder builder = new RiskRegisterBuilder(riskItemVm,user);
            builder.BuildRiskRegisterItem();
            RiskRegister riskRegister = builder.RiskRegister;
            if (riskRegister.RiskID == 0)
            {
                _ampRepository.InsertRiskItem(riskRegister);
            }
            else
            {
                _ampRepository.UpdateRiskItem(riskRegister);
            }
            _ampRepository.Save();

            return true;
        }

        public RiskItemsVM GetRiskTableData(string projectId, string user)
        {
            List<RiskCategory> riskCategories = _ampRepository.GetRiskCategories().ToList();
            List<RiskLikelihood> riskLikelihoods = _ampRepository.GetRiskLikelihoods().ToList();
            List<RiskImpact> riskImpacts = _ampRepository.GetRiskImpacts().ToList();
            List<Risk> riskRatings = _ampRepository.GetRisks().ToList();

            RiskRegisterCollection projectRiskCollection = new RiskRegisterCollection(_ampRepository.GetRiskRegister(projectId));

            if (projectRiskCollection.HasRiskItems())
            {
                List<RiskRegister> projectRisks = projectRiskCollection.AllRisks().ToList();
                RiskItemVMCollectionBuilder riskItemVmCollectionBuilder =
                    new RiskItemVMCollectionBuilder(projectRiskCollection.AllRisks().ToList(), riskCategories,
                        riskLikelihoods, riskImpacts, riskRatings);
                RiskItemsVM riskItemsVm = new RiskItemsVM();
                riskItemsVm.projectRisks = riskItemVmCollectionBuilder.BuildRiskItemVMList();
                return riskItemsVm;
            }
            else
            {
                return null;
            }
        }

        public OverallRiskRatingsVM GetOverallRiskRatingTableData(string projectId, string user)
        {
            List<Risk> riskRatings = _ampRepository.GetRisks().ToList();
            OverallRiskRatingCollection overallRiskCollection = new OverallRiskRatingCollection(_ampRepository.GetOverallRiskRatings(projectId));
            if (overallRiskCollection.HasOverallRiskRating())
            {
                List<OverallRiskRating> projectOverallRiskRatings = overallRiskCollection.AllOverallRiskRatingsRisks().ToList();
                OverallRiskRatingVMCollectionBuilder overallRiskratingVmCollectionBuilder = new OverallRiskRatingVMCollectionBuilder(overallRiskCollection.AllOverallRiskRatingsRisks().ToList(), riskRatings);
                OverallRiskRatingsVM overallRiskRatingsVm = new OverallRiskRatingsVM();
                overallRiskRatingsVm.OverallRiskRatings = overallRiskratingVmCollectionBuilder.BuildOverallRiskRatingVMList();
                return overallRiskRatingsVm;
            }
            else
            {
                return null;
            }
            
        }
        public bool PostOverallRiskRating(OverallRiskRatingVM overallRiskRatingVm, string user)
        {
            OverallRiskRatingBuilder builder = new OverallRiskRatingBuilder(_ampRepository, overallRiskRatingVm, user);
            builder.BuildOverallRiskRating();
            OverallRiskRating overallRiskRating = builder.OverallRiskRating;
            if (overallRiskRating.OverallRiskRatingId == 0)
            {
                _ampRepository.InsertOverallRiskRating(overallRiskRating);
            }
            else
            {
                _ampRepository.UpdateOverallRiskRating(overallRiskRating);
            }
            _ampRepository.Save();

            return true;
        }

        public RiskDocumentsVM GetRiskDocumentTableData(string projectId, string user)
        { 
            RiskDocumentCollection projectRiskDocumentCollection = new RiskDocumentCollection(_ampRepository.GetRiskDocuments(projectId), _documentService);

            if (projectRiskDocumentCollection.HasRiskDocuments())
            {
                List<RiskDocument> projectRiskDocuments = projectRiskDocumentCollection.AllRiskDocuments().ToList();
                RiskDocumentVMCollectionBuilder riskDocumentVmCollectionBuilder =
                    new RiskDocumentVMCollectionBuilder(projectRiskDocumentCollection.AllRiskDocuments().ToList(), _documentService);
                RiskDocumentsVM riskDocumentsVm = new RiskDocumentsVM();
                riskDocumentsVm.ProjectRiskDocuments = riskDocumentVmCollectionBuilder.BuildRiskDocumentVMList();
                return riskDocumentsVm;
            }
            else
            {
                return null;
            }
        }

        public bool PostRiskDocument(RiskDocumentVM riskDocumentVm, string user)
        {
            RiskDocumentBuilder builder = new RiskDocumentBuilder(_ampRepository, riskDocumentVm, user);
            builder.BuildRiskDocument();
            RiskDocument riskDocument = builder.RiskDocument;
            if (riskDocument.RiskRegisterID == 0)
            {
                _ampRepository.InsertRiskDocuments(riskDocument);
            }
           
            _ampRepository.Save();

            return true;
        }

        public bool RemoveRiskDocument(string projectId, string documentId)
        {
            try
            {
                _ampRepository.DeleteRiskDocument(documentId, projectId);
                _ampRepository.Save();
                //Tuple<string> valuesTuple = new Tuple<string>("Success");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}