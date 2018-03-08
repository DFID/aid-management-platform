using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net.Configuration;
using System.Threading.Tasks;
using AMP.Component_Classes;
using AMP.Models;
using AMP.ViewModels;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace AMP.Services
{
    public interface IAmpServiceLayer : IDisposable
    {
        #region Projects

        Task<AdvanceSearchVM> GetProjectsAdvanceSearch(string SearchString, String StageID, int pageNumber, int pageSize, string StageChoice, string BenefittingCountryCode, string user, string BudgetCentre, string SRO, string IsPagingEnabled);

        Task<DashboardViewModel> GetProjects(String searchString, int pagenumber, int pagesize, String User, string sortOrder);

        //Task<DashboardViewModel> GetProjects2(String SearchString, int pagenumber, int pagesize, String user,string sortOrder);

        Task<ProjectViewModel> GetProject(string ProjectID);

        Task<ProjectVM> GetProjectVM(string ProjectID, string user);

        Task<ProjectMarkersVM> GetProjectMarkers(string ProjectID, string user);

        Task<ProjectFinanceVM> GetProjectFinancials(string ProjectID, string user);

        Task<ProjectProcurementVM> GetProjectProcurement(string ProjectID, string user);

        Task<ProjectEvaluationVM> GetProjectEvaluation(string ProjectID, string user);

        bool AddEvaluationDocument(ProjectEvaluationVM projectEvaluationVm, IValidationDictionary validationDictionary,string user);

        bool DeleteEvaluationDocument(string DocumentID, string EvaluationID, string ProjectID, string user);

        //bool AddProject(string DocumentID, string EvaluationID, string ProjectID, string user);

        bool UpdateEvaluation(ProjectEvaluationVM projectEvaluationVm, IValidationDictionary validationDictionary,string user);

        Task<ProjectStatementVM> GetProjectStatements(string ProjectID, string user);

        bool AddStatement(ProjectStatementVM projectStatementVM, IValidationDictionary validationDictionary, string user);

        bool DeleteStatement(string projectId, int statementId, string user);

        bool AddPlannedEndDate(WorkflowPlannedEndDateVM workflowPlannedEndDateVM, IValidationDictionary validationDictionary, string user);


        Task<ProjectDocumentsVM> GetProjectDocuments(string ProjectID, string user);

        Task<PublishedDocumentsVM> GetPublishedProjectDocumentsInDevTracker(string ProjectID, string user);

        Task<RiskRegisterVM> GetRiskRegister(string ProjectID, string user);


        Task<RiskItemVM> GetRiskregisterNew(string ProjecId, string user);

        Task<ProjectLocationVM> GetGeoCoding(string ProjectID, string user);
        
        Task<ProjectViewModel> GetProjectTasks(string ProjectID);

        Task<ProjectTeamVM> GetProjectTeam(string ProjectID, string user);

        Task<ProjectReviewVM> GetProjectReviews(string ProjectID, string user);

        Task<IEnumerable<ReviewOutputVM>> GetProjectReviewScores(string projectId, int reviewId);

        Task<IEnumerable<ReviewDocumentVM>> GetReviewDocuments(string projectId, int reviewId);

        Task<IEnumerable<RiskRegisterVM>> GetRiskRegisterDocumets(string projectId, string user);

        Task<IEnumerable<OverallRiskRatingVM>> GetOverallRiskRatings(string projectId, string user);


       
        void InsertLog(String ViewName, String user);

        void InsertLog(String ViewName, String user, String ProjectID);

        void InsertCodeLog(String MethodName, String Description, DateTime From, DateTime To, TimeSpan Result, String User);

        void InsertCodeLog(String MethodName, String Description, DateTime From, DateTime To, TimeSpan Result, String User, String Value);

        bool UpdateProject(ProjectVM projectViewModel, IValidationDictionary validationDicitionary, string user);

        Task<Tuple<bool,string>> CreateProject(ProjectVM project, IValidationDictionary validationDicitionary, string user);

        Boolean AddProject(string ProjectID, string userName, DashboardViewModel dashboardViewModel,DashboardViewModel originalVM, IValidationDictionary validationDicitionary);

        Boolean AddProject(String ProjectID, String userName);

        bool UpdateProjectMarkers(ProjectMarkersVM projectMarkersVm, IValidationDictionary validationDictionary,string user);

       Task<bool> ValidateCheckProjectHasInputter(string ProjectID);
        Boolean RemoveProject(string ProjectID, string userName);

        bool AddTeamMember(ProjectTeamVM projectTeam, IValidationDictionary validationDictionary, string user);
        bool UpdateTeamMarker(ProjectTeamVM projectTeamm, IValidationDictionary validationDictionary, string user);

        bool UpdateTeamMember(EditTeamMemberVM editTeamMemberVm, string user);

        bool EndTeamMember(EditTeamMemberVM editTeamMemberVm, string user);

        Task<EditTeamMemberVM> GetTeamMember(string id);
        bool UpdateARPCRBasicInfo(PerformanceVM editPerformanceVm, string user);

        string CreateReview(ProjectReviewVM projectReviewVm,  string username);

        string InsertOverallRisk(string user, string projectId, int reviewId, string overallRisk);

        //Change review approver 
        Task<bool> ChangeReviewApprover(string projectId, int reviewId, string newApproverId, string user,string pageUrl);

        //Insert Review deferral
        Task<bool> RequestReviewDeferral(ReviewDeferralVM reviewDeferralVm, string reviewType,  string username, string reviewsURL);
        
        //Update ReviewDeferral 
        Task<bool> UpdateStageForReviewDeferral(ReviewDeferralVM reviewDeferralVm, string reviewType, string user, string reviewsURL);

        //Delete a review deferral
        Task<bool> DeleteDeferral(ReviewDeferralVM reviewDeferralVm, string user);

        Task<bool> DeleteExemption(ReviewExemptionVM ReviewExemptionVm, string user);
        Task<bool> RequestReviewExemption(ReviewExemptionVM reviewExemptionVm, string user, string reviewsURL);

        Task<bool> ApproverExemptionAction(ReviewExemptionVM ReviewExemption, string user, string reviewsURL);

        //Tuple<string, string, string> CreateReviewOutput(ReviewOutputVM reviewOutputVm, string username, IValidationDictionary validationDictionary);
        Tuple<string, string> CreateReviewOutput(ReviewOutputVM reviewOutputVm, string username, IValidationDictionary validationDictionary);
        Tuple<string, string> RemoveReviewOutput(String projectId, int reviewId, int outputId);

        Tuple<string, string, string, int?> EditReviewOutput(ReviewOutput reviewOutput, string user);
        Task<bool> UpdateReviewStage(ReviewVM reviewVm, string user,string reviewsURL);

        Tuple<string> AddReviewDocument(ReviewDocumentVM reviewDocumentVM, IValidationDictionary validationDictionary,
            string user);

        Tuple<string> AddRiskDocument(RiskRegisterVM riskRegisterVm, IValidationDictionary validationDictionary,
            string user);

        //Tuple<string> AddRiskRegisterItem(RiskRegisterVM riskRegisterVm, IValidationDictionary validationDictionary, string user);

        Tuple<string> DeleteReviewDocument(int docId, string user);

        

        bool UpdateOverallRiskRating(RiskRegisterVM riskRegisterVm, string user);

        //Update AR/PCR date 
        //bool UpdatePerformanceARPCRDueDate(PerformanceVM editPerformanceVm, string username);
        bool DeleteAnnualReview(ReviewVM reviewVm, string user);
        bool DeletePCR(ReviewPCRScoreVM reviewPCRScoreVm, string user);

        List<Stage> GetProjectStages();

        List<BenefitingCountry> GetBenefitingCountry();

        BudgetCentre ReturnBudgetCentre(string BudgetCentreID);

        #endregion

        #region LookUp Methods
        /// <summary>
        /// LookupBudgetCentreKV - Get a List of BudgetCentre Key-Value pairs
        /// </summary>
        /// <returns>A list of BudgetCentreKV objects</returns>
        List<BudgetCentreKV> LookupBudgetCentreKV();

        
        List<ProjectKV> LookUpProjectMaster();

    
        Task<List<CurrencyVM>> LookUpCurrency();

        List<UserLookUp> LookUpUsers();

        Task<List<DeliveryChainListVM>> LookUpPartners(string id, string user);
        Task<AllReturnedPartnerListsVM >LookUpPartnerSearchList(string searchString);


        List<PartnerMaster> LookUpPartnerList();
     


        List<InputSectorKV> LookupInputSectorKV(String FundingMech);

        List<Risk> LookupRisksTypes();

        Task<IEnumerable<PersonDetails>> LookUpSroUser();

        #endregion

        #region Components
        Task<ProjectComponentVM> GetComponents(string ProjectID, string user);

        Task<ComponentVM> GetComponentEdit(string ComponentID, string user);

        Task<ComponentMarkersVM> GetComponentMarkers(string ComponentID, string user);

        Task<ComponentVM> GetCreateComponent(string ProjectID, string user);
        

        Task<ComponentSectorVM> GetSectors(string ComponentID, String user);

        bool DeleteSector(string componentid, int sectorcode, string user);
        

        bool UpdateComponent(ComponentVM componentVm, String FundingMechdd, IValidationDictionary validationDictionary, String user);

        bool CreateComponent(ComponentVM componentVm, IValidationDictionary validationDictionary, string user);

        bool AddSector(ComponentSectorVM componentSectorVm, IValidationDictionary validationDictionary);

        Task<ComponentFinanceVM> GetComponentFinancials(string ComponentID, string user);

        bool UpdateMarkers(ComponentMarkersVM componentMarkerVm,  IValidationDictionary validationDictionary, string user);

        Task<bool> ReplacePartnerInChain(DeliveryChain deliveryChainVm,
            IValidationDictionary validationDictionary, string userId);
        Task<ComponentPartnerVM> GetComponentDeliveryChains(string componentId, string user);

        Task<DeliveryChainVM> GetSpecificDeliveryChainRow(int id);

        Task<bool> InsertChain(DeliveryChainVM deliveryChainVm, IValidationDictionary validationDictionary, string userId);

        Task<bool> InsertNewPartnerAndReplaceExistingInChain(DeliveryChainVM deliveryChainVm, IValidationDictionary validationDictionary, string userId);
        bool DeletePartnerFromDeliveryChain(string id, string componentId, string user);

        DeliveryChainVM CreatePartner(string parentId, string user);
     

        Task<DeliveryChainsVM> GetPartnerTableData(string componentId , string userid);

        // Inserting an existing DFID registered supplier
        Task<bool> InsertFirstTierPartner(string componentId, string supplierId, string user);

        EditInputSectorsVM GetSectorsForEdit(string id, string user);

        bool UpdateSectorCodes(EditInputSectorsVM editInputSectorsVm, ModelStateWrapper modelStateWrapper, string user);

        // inserting an existing Non DFID registered Partner
        Task<bool> InsertFirstTierNonRegisteredPartner(string componentId, string supplierId, string user);

        // inserting a completely new First Tier supplier
        Task<bool> InsertNewFirstTierPartner(string componentId,  string PartnerName, string user);

        bool DeletePartnerAndChildrenFromDeliveryChain(string id, string componentId, string user);
       Task<AddPartnerToChainVM> SetUpPartnerSearch(string componentId, int deliveryChainId, int parentId,
            string addOrReplace, int childId);


        ComponentHeaderVM SetupNewPartnerComponentHeader(string componentId);

        Task<ComponentHeaderVM> SetupSearchFirstTierPartnerComponentHeader(string componentId);


        #endregion

        #region Workflow
        Task<WorkflowVM> GetWorkflow(string projectId, Int32 taskId, string user);

        Task<WorkflowVM> GetWorkflow(Int32 workflowId, string user);
        Task<WorkflowPlannedEndDateVM> GetWorkflowPlannedEndDate(string projectId, string user);

        Task<bool> ActionWorkflowResponse(WorkflowMasterVM workflowResponse, string userAction, string urlBase, string user);

        Task<bool> SendProjectForClosure(WorkflowMasterVM closeProjectRquest, string pageUrl, string user);

        Task<bool> ApproveProjectClosure(WorkflowMasterVM closeProjectResponse, string pageUrl, string user);
        Task<bool> RejectProjectClosure(WorkflowMasterVM closeProjectResponse, string pageUrl, string user);

        Task<bool> SendforWorkflowApproval(WorkflowVM workflowvm, IValidationDictionary validationDictionary, string user, string pageUrl);

        Task<bool> ApproveWorkflow(WorkflowVM workflowvm, IValidationDictionary validationDictionary, string user);

        Task<bool> RejectWorkflow(WorkflowVM workflowvm, string pageUrl, string user);
        Task<bool> PreValidateWorkflowApproval(String projectid, Int32 taskid,IValidationDictionary validationDictionary, string user);

       
        Task<WorkflowsVM> GetProjectWorkflows(string projectId, string user);

        Task<bool> ChangeWorkflowApprover(WorkflowVM workflowVm, string pageUrl, string user);

        Task<bool> CancelWorkflow(WorkflowVM workflowVm, string pageUrl, string user);


        #endregion

        #region Risk Register Methods

        Task<RiskItemVM> GetRiskItem(Int32 Id, string user);

        Task<OverallRiskRatingVM> GetOverallRiskRatingItem(int projectId, string user);
        bool PostRiskRegisterItem(RiskItemVM riskItemVm, string user);

        RiskItemsVM GetRiskTableData(string projectId, string user);

        RiskDocumentsVM GetRiskDocumentTableData(string projectId, string user);

        OverallRiskRatingsVM GetOverallRiskRatingTableData(string projectId, string user);
        RiskItemVM GetNewRiskItem(string projectId,string user);

        OverallRiskRatingVM GetNewOverallRiskRating(string projectId, string user);

        bool PostOverallRiskRating(OverallRiskRatingVM overallRiskRatingVm, string user);

        bool PostRiskDocument(RiskDocumentVM riskDocumentVm, string user);

        Tuple<string> DeleteRiskDocument(string docId, string projectId, string user); //Old one 

        Tuple<bool> RemoveRiskDocument(string projectId, string documentId, string user);

        #endregion

        #region Admin Methods

        bool IsAdmin(string empNo);
        Team AdminGetTeam(string id);
        bool AdminUpdateTeam(Team team , String user);
        bool AddAdmin(string adminToAdd, string user);
        bool RemoveAdmin(string adminToDelete, string user);
        bool AdminCloseProject(string projectId, string user);
        bool AdminUpdateProjectDate(ProjectDate projectDate, string user);
        bool AdminUpdateProjectInfo(ProjectInfo projectInfo, string user);
        bool AdminUpdateProjectMarkers(Markers1 projectMarkers, string user);
        AdminUsersVM GetAdminUsers(string user);
        ProjectDate AdminGetProjectDate(string id);
        ProjectInfo AdminGetProjectInfo(string id);
        Markers1 AdminGetProjectMarkers(string id);
        Performance AdminGetPerformance(string id);
        PerformanceVM AdminGetPerformanceNew(string id);

        EditPerformanceVM AdminGetPerformanceNewEdit(string id);

        bool AdminUpdatePerformance(Performance performance, string user);
        bool AdminUpdatePerformanceNew(PerformanceVM performanceVM, IValidationDictionary validationDictionary, string user);

        bool AdminUpdatePerformanceNewEdit(EditPerformanceVM performanceVM, IValidationDictionary validationDictionary, string user);
        ReviewMaster AdminGetReviewMaster(string id, Int32 reviewId);
        bool AdminUpdateReviewMaster(ReviewMaster reviewMaster, string user);
        WorkflowMaster AdminGetWorkflowMaster(string id);
        bool AdminUpdateWorkflowMaster(WorkflowMaster workflowMaster, string user);
        WorkflowDocument AdminGetWorkflowDocument(string id);
        bool AdminUpdateWorkflowDocument(WorkflowDocument workflowDocument, string user);
        ReviewExemption AdminGetReviewExemption(string id, string reviewType);
        bool AdminUpdateReviewExemption(ReviewExemption reviewExemption, string user);

        ComponentMaster ReturnComponentMaster(string ComponentID);
        bool AdminUpdateComponentMaster(ComponentMaster componentMaster, string user);

        bool AdminUpdateProjectMaster(ProjectMaster projectMaster, string user);

        InputSectorCode AdminGetComponentInputSector(string ComponentId, string lineNo);

        bool AdminUpdateInputSectorCode(InputSectorCode inputSectorCode, string user);

        ProjectMaster ReturnProjectMaster(string ProjectMasterID);

        ComponentDate AdminGetComponentDates(string ComponentID);

        bool AdminUpdateComponentDates(ComponentDate componentDate, string user);

        DeliveryChain AdminGetDeliveryChain(string Id);

        bool AdminUpdateDeliveryChain(DeliveryChain deliveryChain,string user);

        ReviewDeferral AdminGetReviewDeferral(string id);

        bool AdminUpdateReviewDeferral(ReviewDeferral reviewDeferral, string user);

        AuditedFinancialStatement AdminGetAuditedStatement(string projectId, string statementId);

        bool AdminUpdateAuditedStatement(AuditedFinancialStatement statement, string user);

        RiskDocument AdminGetRiskDocument(string riskRegisterId);

        bool AdminUpdateRiskDocument(RiskDocument riskDocument, string user);

        EvaluationDocument AdminGetEvaluationDocument(string id);

        bool AdminUpdateEvaluationDocument(EvaluationDocument evaluationDocument, string user);

        ReviewDocument AdminGetReviewDocument(string id);

        bool AdminUpdateReviewDocument(ReviewDocument reviewDocument, string user);

        FundingMechToSector AdminGetMechSectorMapping(string id);

        bool AdminUpdateMechSectorMapping(FundingMechToSector mechToSector, string user);

        NewFundingMechToSectorVM AdminGetNewMechSectorMapping(string sectorCodeId);

        bool AdminAddNewMechSectorMapping(NewFundingMechToSectorVM newMechToSector, string user);

        PartnerMaster AdminGetPartnerMaster(string id);

        bool AdminUpdatePartnerMaster(PartnerMaster partnerMaster, string user);

        #endregion

        #region Authentication Methods

        Task<bool> IsAuthorised(string userID);

        #endregion

        #region Excel Risk Register Methods

        Task<ExcelPackage> CreateExcelRiskLog(string projectId, string user);

        #endregion

    }
}