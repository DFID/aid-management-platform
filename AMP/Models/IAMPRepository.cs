using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMP.ViewModels;
using System.Threading.Tasks;
using System.Data;

namespace AMP.Models
{
    /// <summary>
    /// IProjectRepository is the interface to the project repository class and is use to ensure loose coupling between the project controller and
    /// the DBContext (a.k.a AMPEntities) which accesses the database. 
    /// </summary>
    /// <remarks>IProjectRepository also allows the ProjectRepository to be replaced by a MockProjectRepository, allowing the 
    /// Project controller to be unit tested using mock data.
    /// </remarks>
    public interface IAMPRepository : IDisposable
    {

        #region Project Repository Methods
        /// <summary>GetProjects - Gets all projects from AMP Database</summary> 
        IEnumerable<ProjectMaster> GetProjects(String SearchString, String user);

        IQueryable<ProjectMaster> GetProjectsAdvanceSearch(String SearchString, String StageID, string StageChoice, string BenefittingCountryCode, string BudgetCentre, string SRO);
        /// <summary>GetProjects - Gets all projects of the given org unit from AMP Database</summary> 
        IEnumerable<ProjectMaster> GetProjects(String SearchString, String user, string orgUnit);

        ///<summary>GetProject - get a single project from the AMP Database</summary>
        ///<param name="id">id of the project to be returned. Should be six digit, passed as a string.</param>
        ///<remarks>GetProject is used by the Details and Edit View of Project</remarks>
        ProjectMaster GetProject(String id);

        //IEnumerable<vDashboardProject> DashboardProjects(string userId);
        ProjectPlannedEndDate GetProjectPlannedEndDate(string id);

        ProjectPlannedEndDate GetProjectPlannedEndDatForWorkflowHistory(int wfTaskId);

        BudgetCentre GetBudgetCentre(string budgetCentreId);

        void CloseProject(ProjectMaster project, ProjectDate projectdate, Performance projectPerformance, String user);

        void UpdateARIESProjectForApproval(ProjectMaster projectmaster);

        ProjectDate GetProjectDates(String id);

        /// <summary> GetTeam - Gets a list of team members</summary>
        /// <param name="id">The ID of the project to return. Should be a six digit number</param>
        /// <returns>A team object</returns>
        IEnumerable<Team> GetTeam(String id);

        void UpdateTeam(Team team);

        void InsertTeam(Team team);

        void EndTeamMember(Team team, bool runPCMX);

        Team GetTeamMember(Int32 id);

        /// <summary>
        /// ActiveRoleExists - Check that a role is actively filled on a project
        /// </summary>
        /// <param name="roleId">The role ID</param>
        /// <param name="projectId">The project ID</param>
        /// <returns>true or false</returns>
        bool ActiveRoleExists(string roleId, string projectId);

        IEnumerable<ProjectRole> GetProjectRoles();

        /// <summary>
        /// Get Reviews - Get AR and PCR Reviews for a project.
        /// </summary>
        /// <param name="id">The ID of the project to return. Should be a six digit number</param>
        /// <returns>A list of Review Masters and other child objects (AR, PCR, Review Outputs).</returns>
        IEnumerable<ReviewMaster> GetReviews(String id, string user);

        ReviewMaster GetReview(string projectId, int reviewId);
        ReviewARScore GetReviewARScore(string projectId, int reviewId);

        int GetNextReviewID(string projectId);

        void UpdateReviewARScore(ReviewARScore reviewArScore);
        ReviewPCRScore GetReviewPCRScore(string projectId, int reviewId);

        void UpdateReviewPCRScore(ReviewPCRScore reviewpcrScore);
        /// <summary>
        /// GetReviewScores - Get AR and PCR Reviews scores for a review.
        /// </summary>
        /// 
        List<ReviewOutput> GetReviewScores(string projectId, int reviewId);

        /// <summary>
        /// Get Evaluations -
        /// </summary>
        /// <param name="id">The ID of the project to return. Should be a six digit number</param>
        /// <returns></returns>
        Evaluation GetProjectEvaluations(String id);

        /// <summary>
        /// Insert a new review document object into the AMP database.
        /// </summary>
        void InsertReviewDocument(ReviewDocument reviewDocument);

        void InsertRiskDocuments(RiskDocument riskRegister);

        /// <summary>
        /// Delete a review document from AMP database.
        /// </summary>
        void DeleteReviewDocument(int reviewDocumentId);

        void DeleteRiskDocument(string docID, string projectId);
        /// <summary>
        /// Insert a new evaluation document object into the AMP database.
        /// </summary>
        /// <param name="evaluationDocument">Populated EvaluationDocument model object</param>
        void InsertEvaluationDocument(EvaluationDocument evaluationDocument);

        void DeleteEvaluationDocument(Int32 EvaluationID, string DocumentID);

        IEnumerable<EvaluationDocument> GetEvaluationDocuments(Int32 evaluationId);

        Evaluation GetEvaludationById(Int32 evaluationId);

        void UpdateProjectEvaluation(Evaluation evaluation);

        /// <summary>
        /// Get Project Info -
        /// </summary>
        /// <param name="id">The ID of the project. Should be a six digit number</param>
        /// <returns>ProjectInfo object</returns>
        ProjectInfo GetProjectInfo(String id);


        IEnumerable<AuditedFinancialStatement> GetActiveAuditedStatements(String id);

        IEnumerable<AuditedFinancialStatement> GetAllAuditedStatements(string id);
        AuditedFinancialStatement GetAuditedStatement(string projectid, int statementid);
        /// <summary>
        /// Get Performance -
        /// </summary>
        /// <param name="id">The ID of the project to return. Should be a six digit number</param>
        /// <returns></returns>
        Performance GetProjectPerformance(string id);

        void UpdateProjectPerformance(Performance performance);

        /// <summary>
        /// Get audited Statement 
        /// </summary>
        /// <param name="id">The ID of the project to return. Should be a six digit number</param>

        void InsertStatement(AuditedFinancialStatement statement);

        void InsertPlannedEndDate(ProjectPlannedEndDate plannedEndDate);

        void InsertOverallRiskRating(OverallRiskRating overallRiskRating);

        void UpdateStatement(AuditedFinancialStatement statementToUpdate);

        ///<summary>Insert Project - Inserts a project into the database.</summary> 
        ///<param name="project">A ProjectMaster object (for definition of ProjectMaster, see AMP.edmx in solution explorer.</param>
        void InsertProject(ProjectMaster project);


        void UpdateComponentMaster(ComponentMaster componentMaster);//Admin ComponentMaster


        void UpdateProjectMaster(ProjectMaster projectdetails);//Admin ProjectMaster
        
        ///<summary>UpdateProject - Update a project with changes.</summary>
        ///<param name="project">A ProjectMaster object (for definition of ProjectMaster, see AMP.edmx in solution explorer.</param>
        void UpdateProject(ProjectMaster project);

        ///<summary>UpdateProjectInfo - Update the project info table with changes.</summary>
        ///<param name="projectInfo">A ProjectInfo object (for definition of ProjectInfo, see AMP.edmx in solution explorer.</param>
        void UpdateProjectInfo(ProjectInfo projectInfo);

        void UpdateProjectDates(ProjectDate projectDate);


        ///<summary> close off the orphan lines for a project in the planned end date table with a status of A</summary>
        void CloseOffExistingActiveProjectPlannedEndDate(string id);
        ///<summary> - update the workflow planned end date temp table</summary>
        void UpdatePlannedEndDate(ProjectPlannedEndDate projPlannedEndDate);
        /// <summary>InsertError - Inserts an error entry for every catch senario</summary>
        /// <param name="project">A ErrorLog object</param>
        void InsertError(ErrorLog errorlog);

        /// <summary>InsertLogging - Inserts an log entry for every page access</summary>
        /// <param name="project">A ErrorLog object</param>
        void InsertLog(Logging logging);

        /// <summary>InsertCodeLogging - Inserts an Code Performance log entry for method which has it enabled</summary>
        /// <param name="project">A CodePerformance object</param>
        void InsertCodeLog(CodePerformance codePerformance);

        /// <summary>InsertLogging - Inserts an log entry for every page access</summary>
        /// <param name="project">A ErrorLog object</param>


        /// <summary>
        /// GetProjectDocuments - Get documents for a project
        /// </summary>
        /// <param name="ProjectID">id of the project to return documents for</param>
        /// <returns>An IEnumerable list of project documents. This will be mapped to a List of ProjectDocumentVM ViewModels in the Service Layer</returns>
        /// 

        ///<summary>
        ///NextProjectID - Get the next projectID from the database which will be used when saving a new project.
        ///</summary>
        ///<returns>A 6 digit string, all numeric</returns>    
        string NextProjectID();

        ExemptionReason GetSingleExemptionReason(string ExemptionId, string ReviewType);

        void AddProject(Portfolio portfolio);
        Portfolio GetProjectPortfolio(String ProjectID, String userName);
        IEnumerable<Performance> GetPortfolioPerformance(IEnumerable<Portfolio> portfolios);
        void RemoveProject(Portfolio portfolio);
        ///<summary>Save - save project changes.</summary> 
        void Save();
        
        void UpdateARPCRBasicInfo(Performance newPerformanceBasic);
        void UpdatePerformance(Performance newPerformanceBasic);
        
        /// <summary>
        /// Project reviews CRUD
        /// </summary>
        void InsertReview(ReviewMaster reviewMaster);
        ///<summary>update a project review in reviewMaster table </summary>
        void UpdateReview(ReviewMaster reviewMaster);
        ///<summary>Insert a project review output .</summary>
        void InsertReviewOutput(ReviewOutput reviewOutput);
        void RemoveReviewOutput(ReviewOutput reviewOutput);
        void EditReviewOutput(ReviewOutput reveiwOutput);
        ReviewOutput GetReviewOutput(string projectId, int reviewId, int outputId);
        int? GetReviewOutputsWeightSum(string projectId, int reviewId, int outputId);
        IEnumerable<ReviewOutput> GetReviewOutputs(string projectId, int reviewId);
        void InsertReviewARScore(ReviewARScore reviewArScore);
        void InsertReviewPCRScore(ReviewPCRScore reviewpcrScore);
        int GetNextReviewOutputId(string projectId, int reviewId);
        List<ExemptionReason> GetExemptionReasons();
        List<Stage> GetStages();

        List<ReviewDocument> GetReviewDocuments(string projectId, int reviewId);

        List<RiskDocument> GetRiskRegisters(string projectId);

        List<OverallRiskRating> GetOverallRiskRatings(string projectId);

        OverallRiskRating GetOverallRiskRating(string projectId);

        void DeleteReviewARScore(ReviewARScore reviewARScore);
        void DeleteReviewOutputs(IEnumerable<ReviewOutput> reviewOutputs);

        void DeleteReviewDocuments(IEnumerable<ReviewDocument> reviewDocuments);
        
        /// <summary>
        /// Project Reviews Deferrals CRUD 
        /// </summary>
        ReviewDeferral GetReviewDeferral(string ProjectID, int reviewId);
        void InsertReviewDeferral(ReviewDeferral reviewDeferral);
        void UpdateReviewDeferral(ReviewDeferral reviewDeferral);

        ReviewExemption GetReviewExemption(string projectId, string ExemptionType);

        void DeleteReviewDeferral(int deferralId);

        void DeleteReviewExemption(ReviewExemption reviewExemption);
    
        void InsertReviewExemption(ReviewExemption reviewExemption);
        void UpdateReviewExemption(ReviewExemption reviewExemption);
        void DeleteReviewMaster(ReviewMaster reviewMaster);
        void DeleteReviewPCRScore(ReviewPCRScore reviewPCRScore);

        Markers1 GetProjectMarkers(String id);
        void AddProjectMarkers(Markers1 marker);
        void UpdateProjectMarkers(Markers1 marker);

        //void InsertBudgetApproval(BudgetApprovalValue budgetApproval);

        //BudgetApprovalValue GetBudgetApprovedByWorkflow(Int32 workflowId, Int32 workflowStepId);

        #endregion

        #region Component Repository Methods

        IEnumerable<ComponentMaster> GetComponents(String ProjectID);

        IEnumerable<ComponentMaster> GetComponentsUsingComponentSubString(String ProjectID);

        ComponentMaster GetComponent(String id);

        IEnumerable<InputSectorCode> GetInputSectors(String id);

        InputSectorCode GetInputSector(String componentid, int sectorcode);

        void DeleteSectors(List<InputSectorCode> sectorsCodes);

        void InsertSectors(List<InputSectorCode> sectorsCodes);

        ComponentDate GetComponentDates(String id);

        IEnumerable<ImplementingOrganisation> GetImplementingOrg(String id);

        void UpdateComponent(ComponentMaster currentComponentMaster, ComponentDate componentdate);

        void CreateComponent(ComponentMaster currentComponentMaster, ComponentDate componentdate);

        void CreateComponentDate(ComponentDate componentdate);
        void UpdateComponentDate(ComponentDate currentcomponentdate);

        void AddInputSector(InputSectorCode newinputsector);

        void DeleteSector(InputSectorCode sector);

        void AddMarker(Marker marker);
        void UpdateMarker(Marker marker);
        
        //Delivery Chain
        List<DeliveryChain> GetDeliveryChainsByComponent(string componentId);
        DeliveryChain GetDeliveryChain(Int32 id);

       
        List<PartnerMaster> GetDeliveryChainsByIDList(List<int> iDs);

        void InsertDeliveryChain(DeliveryChain deliveryChain);
        void UpdateDeliveryChain(DeliveryChain deliveryChain);
        void DeleteDeliveryChain(DeliveryChain deliveryChain);

        //DeliveryChain GetDeliveryChainThatMatchesDeliveryChainVm(DeliveryChain deliveryChain);

        //Partners

        void InsertPartner(PartnerMaster partnerMaster);

        DeliveryChain GetDeliveryChainThatMatchesDeliveryChainVm(DeliveryChain deliveryChain);

        int NextPartnerID();

        string GetPartnerName(int id);

        void ReplacePartnerInDeliveryChain(DeliveryChain deliveryChain);

        #endregion

        #region Workflow Repository Methods

        IEnumerable<WorkflowTask> GetWorkflowTasks();

        IEnumerable<WorkflowStage> GetWorkflowStages();

        Int32 NextWorkFlowId();

        void InsertWorkFlowMaster(WorkflowMaster workflowMaster);
        void UpdateWorkFlowMaster(WorkflowMaster workflowMaster);

        void DeleteWorkflowMaster(WorkflowMaster workflowMaster);

        IEnumerable<WorkflowMaster> GetWorkflowMasters(int workflowId);
        WorkflowMaster GetWorkflowMaster(int workflowId);
        WorkflowMaster GetWorkflowMaster(int workflowId, int workflowStepId);

        IEnumerable<WorkflowMaster> GetWorkflowMastersByProject(string projectId);
        IEnumerable<WorkflowMaster> GetWorkflowMastersByProjectandTask(string projectId, Int32 taskId);

        WorkflowDocument GetWorkflowDocument(int workflowId);

        IEnumerable<WorkflowDocument> GetWorkflowDocuments(int workflowId);

        void InsertWorkflowDocument(WorkflowDocument workflowDocument);
        
        void DeleteWorkflowDocument(int workflowId);

        IEnumerable<WorkflowDocument> GetAllWorkflowDocuments();

        IEnumerable<vHoDBudCentLookup> GetHoDAlertRecipients(string projectId);

            #endregion

        #region Lookup Methods



        /// <summary>LookUpBudgetCentre - gets a list of budget centre objects.</summary>
        /// <returns>A list of BudgetCentre objects.</returns>
        /// <remarks>Used to return JSON for a typeahead search.</remarks>
        IEnumerable<BudgetCentre> LookUpBudgetCentre();

        IEnumerable<UserLookUp> LookUpUsers();

        IEnumerable<DeliveryChain> LookUpDeliveryChains(string id,string user);

        IEnumerable<PartnerMaster> LookUpPartnerList();
        IEnumerable<PartnerMaster> LookUpAMPPartnerSearchList(string searchString);

        IEnumerable<UserLookUp> LookUpUsers(IEnumerable<String> projectTeam);

        IEnumerable<ProjectMaster> LookUpProjectMaster();

        IEnumerable<FundingMech> LookUpFundingMechs();

        IEnumerable<FundingArrangement> LookUpFundingArrangements();

        IEnumerable<PartnerOrganisation> LookUpPartnerOrganisations();

        IEnumerable<BenefitingCountry> LookUpBenefitingCountrys();

        IEnumerable<Portfolio> GetPortfolios(String User);

        IEnumerable<WorkflowMaster> LookUpWorkFlowMaster();

        //All Sectors
        IEnumerable<InputSector> LookUpInputSector();

        //Filtered by Funding Mech
        IEnumerable<InputSector> LookUpInputSector(String FundingMech);

        //EvaluationTypes 
        IEnumerable<EvaluationType> LookUpEvaluationTypes();
        IEnumerable<Risk> LookUpRiskTypes();
        IEnumerable<EvaluationManagement> LookUpEvaluationManagements();

        List<string> LookUpSRO();

        #endregion
        
        #region Admin Methods

        void AdminUpdateTeamMember(Team team);
        void AdminUpdateProjectDate(ProjectDate projectDate);
        void AdminUpdateProjectInfo(ProjectInfo projectInfo);
        void AdminUpdateProjectMarkers(Markers1 projectMarkers);
        void AdminUpdateWorkflowDocument(WorkflowDocument workflowDocument);
        WorkflowDocument AdminGetWorkflowDocument(int id);
        void AdminEndTeamMember(Team team);

        AdminUser GetAdmin(string empNo);

        void AddAdminUser(AdminUser newAdmin);

        void DeleteAdminUser(AdminUser adminToDelete);

        IEnumerable<AdminUser> GetAdminUsers();

        AdminUser AdminExists(string empNo);

        void UpdateAdminUser(AdminUser adminUserToUpdate);

        ReviewMaster AdminGetReviewMaster(string id, Int32 reviewId);

        void AdminUpdateReviewMaster(ReviewMaster reviewMaster);

        InputSectorCode AdminGetComponentInputSector(string componentId, Int32 lineNoInt);
        void AdminUpdateComponentInputSector(InputSectorCode inputSectorCode);

        EvaluationDocument GetEvaluationDocument(int id);

        void UpdateEvaluationDocument(EvaluationDocument evaluationDocument);

        ReviewDocument GetReviewDocument(int id);

        void UpdateReviewDocument(ReviewDocument reviewDocument);

        FundingMechToSector GetFundingMechToSectorMapping(int id);

        void UpdateFundingMechToSectorMapping(FundingMechToSector mappingToUpdate);

        #endregion

        #region Risk Register Methods

        RiskRegister GetRiskItem(int ID);

        OverallRiskRating GetOverallRisk(int overallRiskId);
        void InsertRiskItem(RiskRegister riskItem);
        void UpdateRiskItem(RiskRegister riskItem);
        IEnumerable<RiskRegister> GetRiskRegister(string ProjectId);

        IEnumerable<RiskCategory> GetRiskCategories();
        IEnumerable<RiskLikelihood> GetRiskLikelihoods();

        IEnumerable<RiskImpact> GetRiskImpacts();

        IEnumerable<Risk> GetRisks();

        IEnumerable<RiskDocument> GetRiskDocuments(string projectId);

        void UpdateOverallRiskRating(OverallRiskRating overallRiskRating);

        RiskDocument GetRiskDocument(int riskRegisterId);

        void UpdateRiskDocument(RiskDocument riskDocument);

        void AdminUpdateDeliveryChain(DeliveryChain deliveryChain);

         ReviewDeferral AdminGetReviewDeferral(Int32 deferralId);

        void AdminUpdateReviewDeferral(ReviewDeferral reviewDeferral);

        InputSector GetIndividualInputSector(string sectorCodeId);

        IEnumerable<FundingMechToSector> GetAllFundingMechMappingsForSectorCode(string sectorCodeID);

        void AddNewFundingMechToSectorMapping(FundingMechToSector newMapping);

        PartnerMaster AdminGetPartner(int id);

        void AdminUpdatePartner(PartnerMaster partnerMaster);
       

        #endregion

    }
}

