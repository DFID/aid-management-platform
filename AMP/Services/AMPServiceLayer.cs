using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AMP.ARIESModels;
using AMP.Component_Classes;
using AMP.EmailFactory;
using AMP.Models;
using AMP.RiskClasses;
using AMP.Utilities;
using AMP.ViewModels;
using AutoMapper;
using AutoMapper.Mappers;
using Microsoft.Ajax.Utilities;
using PagedList;
using MoreLinq;
using OfficeOpenXml;
using Configuration = AMP.Utilities.Configuration;
using IConfiguration = AMP.Utilities.IConfiguration;

namespace AMP.Services
{
    public class AMPServiceLayer : IAmpServiceLayer, IDisposable
    {
        private IAMPRepository _projectrepository;
        private IARIESService _ariesService;
        private IPersonService _personService;
        private IErrorEngine _errorengine;
        private ILoggingEngine _loggingengine;
        private IEmailService _emailService;
        private IEDRMService _edrmService;
        private IWorkflowService _workflowService;
        private IValidationService _validationService;
        private IDocumentService _documentService;
        private IIATIWebService _iatiWebService;

        //private FeedbackEngine feedbackengine;

        private IIdentityManager _identitymanager;
        private IConfiguration _Configuration;

        //Real method 

        public AMPServiceLayer()
        {
            _projectrepository = new AMPRepository();
            _ariesService = new ARIESService();
            _personService = new DemoPersonService();
            _errorengine = new ErrorEngine();
            _loggingengine = new LoggingEngine();
            //  feedbackengine = new FeedbackEngine();
            _identitymanager = new DemoIdentityManager();
            _emailService = new EmailService();
            _edrmService = new EDRMService();
            _Configuration = new Configuration();
            _workflowService = new WorkflowService(_projectrepository, _personService, _loggingengine, _errorengine, _documentService);
            _validationService = new ValidationSevice(_projectrepository);
            _documentService = new DocumentService(_projectrepository);
            _iatiWebService = new IATIWebService();
        }

        //Overloader method for testing
        public AMPServiceLayer(IAMPRepository projectrepository, IARIESService ariesService, IPersonService personService, ILoggingEngine loggingEngine, IErrorEngine errorEngine, IIdentityManager identityManager, IEmailService emailService, IEDRMService edrmService, IIATIWebService iatiWebService)
        {
            _projectrepository = projectrepository;
            _ariesService = ariesService;
            _personService = personService;
            _loggingengine = loggingEngine;
            _errorengine = errorEngine;
            _identitymanager = identityManager;
            _emailService = emailService;
            _edrmService = edrmService;
            _Configuration = new Configuration();
            _workflowService = new WorkflowService(projectrepository, _personService, _loggingengine, _errorengine, _documentService);
            _validationService = new ValidationSevice(projectrepository);
            _documentService = new DocumentService(projectrepository);
            _iatiWebService = iatiWebService;
        }


        #region Project related Methods

        public async Task<AdvanceSearchVM> GetProjectsAdvanceSearch(string SearchString, String StageID, int pageNumber, int pageSize, string stageChoice, string benefittingCountryCode, string user, string BudgetCentreId, string SRO, string IsPagingEnabled)
        {
            //IEnumerable<ProjectMaster> projects = null;
            IQueryable<ProjectMaster> projects = null;
            projects = _projectrepository.GetProjectsAdvanceSearch(SearchString, StageID, stageChoice, benefittingCountryCode, BudgetCentreId, SRO);
            Int32 ProjectCount = 0;
            if (projects != null)
            {
                ProjectCount = projects.ToList().Count;
                if (ProjectCount == 0 || ProjectCount == null)
                {
                    pageSize = 1;
                }
            }
            else
            {
                pageSize = 1;
                ProjectCount = 0;
            }

            if (IsPagingEnabled == "F")
            {
                pageNumber = 1;
                if (ProjectCount == 0)
                {
                    pageSize = 1;
                }
                else
                {
                    pageSize = ProjectCount;
                }
            }

            AdvanceSearchVM advanceSearchVM = new AdvanceSearchVM();

            if (!String.IsNullOrEmpty(SRO))
            {
                PersonDetails SRODetails =
                    await _personService.GetPersonDetails(SRO.Trim());
                if (SRODetails != null)
                {
                    advanceSearchVM.SROName = SRODetails.Forename + " " +
                                                     SRODetails.Surname;
                }
            }

            if (!String.IsNullOrEmpty(BudgetCentreId))
            {
                BudgetCentre BudgetCetre = ReturnBudgetCentre(BudgetCentreId);
                if (BudgetCetre != null)
                {
                    advanceSearchVM.BudgetCentreName = BudgetCetre.BudgetCentreDescription + " " + BudgetCetre.BudgetCentreID;
                }
            }


            Mapper.CreateMap<ProjectMaster, AdvanceSearchViewModel>();
            IEnumerable<AdvanceSearchViewModel> projectviewmodel;

            projectviewmodel = Mapper.Map<IEnumerable<ProjectMaster>, IEnumerable<AdvanceSearchViewModel>>(projects);


            PagedList<AdvanceSearchViewModel> pagedprojects = new PagedList<AdvanceSearchViewModel>(projectviewmodel, pageNumber, pageSize); // pageNumber, pageSize

            /*
            IEnumerable<ProjectApprovedBudget> projectApprovedBudgets;
            try
            {
                if (ProjectCount > 0)
                {
                    IEnumerable<String> projectList = pagedprojects.Select(x => x.ProjectID);
                    projectApprovedBudgets = await ariesService.GetProjectApprovedBudgetsByPortfolioAsync(projectList, user); // Change this to named method

                    if (projectApprovedBudgets != null)
                    {
                        // Loop through projectApprovedBudgets and map the approved budget value
                        // Start loop
                        foreach (AdvanceSearchViewModel userproject in projectviewmodel)
                        {
                            var searchresult = projectviewmodel.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID));
                            if (projectApprovedBudgets.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID)) != null)
                            {
                                var projectbudget = projectApprovedBudgets.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID));
                                searchresult.ApprovedBudget = projectbudget.ApprovedBudget;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                advanceSearchVM.AriesMsg = "C";
                projectApprovedBudgets = new List<ProjectApprovedBudget>();
                errorengine.LogError(ex, user);
            
            }
            */
            //PagedList<AdvanceSearchViewModel> pagedprojects = new PagedList<AdvanceSearchViewModel>(projectviewmodel, pageNumber, pageSize); // pageNumber, pageSize

            if (ProjectCount > 0)
            {
                advanceSearchVM.ResultMsg = "A";
            }
            else { advanceSearchVM.ResultMsg = "B"; }

            advanceSearchVM.projects = pagedprojects;
            advanceSearchVM.ProjectCount = ProjectCount;
            return advanceSearchVM;

        }

        //public async Task<DashboardViewModel> GetProjects2(String SearchString, int pagenumber, int pagesize, String user, string sortOrder)
        //{
        //    //TO DO: Need to filter out duplicate projects from the View. Either at source or once the list has returned.
        //    DashboardViewModel dashboardViewModel = new DashboardViewModel();

        //    using (_projectrepository)
        //    {
        //        List<vDashboardProject> dashboardProjects = _projectrepository.DashboardProjects(user).ToList();

        //        IEnumerable<ProjectApprovedBudget> projectApprovedBudgets;
        //        List<ProjectApprovedBudget> approvedBudgetsList;
        //        List<UserProjectsViewModel> userProjectsViewModels;
        //        IEnumerable<String> projectList = dashboardProjects.Select(x => x.ProjectID);

        //        // Find if user is authorised
        //        // Check if the user is authorised & if not, set budget to 0 & display message about restricted access

        //        Mapper.CreateMap<vDashboardProject, UserProjectsViewModel>();

        //        //Create an empty ProjectDashBoardModel
        //        List<UserProjectsViewModel> userprojectviewmodels = new List<UserProjectsViewModel>();

        //        // First map -- Populate Project Master data
        //        userprojectviewmodels = Mapper.Map<List<vDashboardProject>, List<UserProjectsViewModel>>(dashboardProjects);


        //        try
        //        {
        //            if (await IsAuthorised(user))
        //            {
        //                using (_ariesService)
        //                {
        //                    //Get all approved budgets.
        //                    projectApprovedBudgets = await _ariesService.GetProjectApprovedBudgetsByPortfolioAsync(projectList, user);
        //                    approvedBudgetsList = projectApprovedBudgets.ToList();
        //                    for (int i = 0; i < userprojectviewmodels.Count; i++)
        //                    {
        //                        string projectId = userprojectviewmodels.ElementAt(i).ProjectID;
        //                        if (approvedBudgetsList.FirstOrDefault(x => x.ProjectID == projectId) != null)
        //                        {
        //                            userprojectviewmodels.ElementAt(i).ApprovedBudget = approvedBudgetsList.FirstOrDefault(x => x.ProjectID == projectId).ApprovedBudget;
        //                        }

        //                    }
        //                }

        //            }
        //            else
        //            {
        //                userprojectviewmodels.Select(c => { c.ApprovedBudget = 0; return c; }).ToList();
        //                // Add a message confirming budgets show as 0 due to restricted access
        //                dashboardViewModel.ARIESWebServiceMessage = "As you have restricted access to AMP, some budget information will be unavailable or show as £0.";
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            dashboardViewModel.ARIESWebServiceMessage =
        //                "Sorry, ARIES doesn't seem to be available at the moment. Your financial data cannot be shown.";
        //            projectApprovedBudgets = new List<ProjectApprovedBudget>();
        //            _errorengine.LogError(ex, user);
        //        }
        //        //Sorting

        //        switch (sortOrder)
        //        {
        //            case "ID_desc":
        //                userprojectviewmodels.OrderByDescending(a => a.ProjectID);

        //                break;
        //            case "ApprovedBudget_desc":
        //                userprojectviewmodels.OrderByDescending(c => c.ApprovedBudget);

        //                break;
        //            case "ApprovedBudget":
        //                userprojectviewmodels.OrderBy(c => c.ApprovedBudget);
        //                break;
        //            case "Stage":
        //                userprojectviewmodels.OrderBy(c => c.Stage);
        //                break;

        //            case "Stage_desc":
        //                userprojectviewmodels.OrderByDescending(c => c.Stage);
        //                break;

        //            case "NextReview":
        //                userprojectviewmodels.OrderBy(c => c.NextReview);
        //                break;
        //            case "NextReview_desc":
        //                userprojectviewmodels.OrderByDescending(c => c.NextReview);
        //                break;
        //        }

        //        //Convert the IEnumerable<userprojectviewmodel into a PagedList. Now that's magic! Not a lot. sorry.......
        //        PagedList<UserProjectsViewModel> pagedprojects =
        //            new PagedList<UserProjectsViewModel>(userprojectviewmodels, pagenumber, pagesize);

        //        dashboardViewModel.userprojects = pagedprojects;
        //        return dashboardViewModel;
        //    }
        //}

        public async Task<DashboardViewModel> GetProjects(String SearchString, int pagenumber, int pagesize, String User, string sortOrder)
        {


            using (_projectrepository)
            {
                //Get org unit of user if HoD-----------start----------            
                List<string> users = new List<string>();
                users.Add(User);
                IEnumerable<string> resources = users;

                IEnumerable<ProjectMaster> projects = null;

                using (_personService)
                {
                    //Get Employee details from the PersonService
                    IEnumerable<PersonDetails> personDetails = await _personService.GetPeopleDetails(resources);
                    PersonDetails person = new PersonDetails();
                    if (personDetails.Count() != 0)
                        person = personDetails.First();

                }

                //if (personDetails.Count() != 0 && person.IS_HOD == "T")
                //{
                //    string orgUnit = person.ORG_UNIT_CODE;
                //    if (!String.IsNullOrEmpty(orgUnit))
                //        projects = projectrepository.GetProjects(SearchString, User, orgUnit);
                //    else
                //    {                   
                //        projects = projectrepository.GetProjects(SearchString, User);
                //    }
                //}

                //else
                projects = _projectrepository.GetProjects(SearchString, User);

                //--------end-------------- -

                //IEnumerable <ProjectMaster> projects = projectrepository.GetProjects(SearchString, User);



                //Create the DashboardViewModel object that will wrap up the projects and alerts.
                DashboardViewModel dashboardVM = new DashboardViewModel();


                // Start Automapping ProjectApprovedBudget to ProjectMaster to make ProjectDashBoardModel
                Mapper.CreateMap<ProjectMaster, UserProjectsViewModel>();

                //Create an empty ProjectDashBoardModel
                IEnumerable<UserProjectsViewModel> userprojectviewmodel;




                // First map -- Populate Project Master data
                userprojectviewmodel =
                    Mapper.Map<IEnumerable<ProjectMaster>, IEnumerable<UserProjectsViewModel>>(projects);

                //Sorting

                switch (sortOrder)
                {
                    case "ID_desc":
                        userprojectviewmodel = userprojectviewmodel.OrderByDescending(a => a.ProjectID);

                        break;
                    case "ApprovedBudget_desc":
                        userprojectviewmodel = userprojectviewmodel.OrderByDescending(c => c.ApprovedBudget);

                        break;
                    case "ApprovedBudget":
                        userprojectviewmodel = userprojectviewmodel.OrderBy(c => c.ApprovedBudget);
                        break;
                    case "Stage":
                        userprojectviewmodel = userprojectviewmodel.OrderBy(c => c.Stage);
                        break;

                    case "Stage_desc":
                        userprojectviewmodel = userprojectviewmodel.OrderByDescending(c => c.Stage);
                        break;

                    case "NextReview":
                        userprojectviewmodel = userprojectviewmodel.OrderBy(c => c.NextReview);
                        break;
                    case "NextReview_desc":
                        userprojectviewmodel = userprojectviewmodel.OrderByDescending(c => c.NextReview);
                        break;
                }



                IEnumerable<ProjectApprovedBudget> projectApprovedBudgets;

                // Update the model for projects which are a portfolio, This will control if you can delete them or not. 
                //Get list of portfolios



                IEnumerable<Portfolio> portfolio = _projectrepository.GetPortfolios(User);
                // Get AR and PCR Data for portfolio of projects.
                //IEnumerable<Performance> performance = projectrepository.GetPortfolioPerformance(portfolio);



                List<Performance> performance = portfolio.Select(x => x.ProjectMaster.Performance).ToList();

                //DEFECT: Now get the performance objects from the initial project list. We only got them from the Portfolio and as a result no performance data was present for projects where the user has a role.
                performance.AddRange(projects.Select(x => x.Performance).ToList());

                performance = performance.Distinct().ToList();

                // Optimisation probably only need one loop for budgets, portfolio (true, false) and AR/PCR/
                foreach (UserProjectsViewModel userprojectportfolio in userprojectviewmodel)
                {
                    //Set a short version of the title for the dashboard.
                    if (userprojectportfolio.Title.Length > 65)
                    {
                        userprojectportfolio.TitleShort = userprojectportfolio.Title.Substring(0, 65) + "..";
                    }
                    else
                    {
                        userprojectportfolio.TitleShort = userprojectportfolio.Title;
                    }

                    //Get current row of ProjectData
                    var project =
                        userprojectviewmodel.FirstOrDefault(x => x.ProjectID.Equals(userprojectportfolio.ProjectID));

                    if (portfolio.FirstOrDefault(x => x.ProjectID.Equals(userprojectportfolio.ProjectID)) != null)
                    {
                        project.Portfolio = true;
                    }
                    else
                    {
                        project.Portfolio = false;
                    }

                    //Set Next AR or PCR
                    try
                    {

                        var nextReview =
                            performance.FirstOrDefault(
                                x => x != null && x.ProjectID.Equals(userprojectportfolio.ProjectID));

                        //If there is no next review dont work out and map
                        if (nextReview != null)
                        {

                            if (nextReview.ARRequired == "No" && nextReview.PCRRequired == "No")
                            {
                                project.NextReview = null;
                            }
                            else if (nextReview.ARRequired == "Yes" && nextReview.PCRRequired == "No")
                            {
                                project.NextReview = nextReview.ARDueDate;
                            }
                            else if (nextReview.ARRequired == "No" && nextReview.PCRRequired == "Yes")
                            {
                                project.NextReview = nextReview.PCRDueDate;
                            }
                            else if (nextReview.ARRequired == "Yes" && nextReview.PCRRequired == "Yes")
                            {
                                if (nextReview.PCRDueDate > nextReview.ARDueDate)
                                {
                                    project.NextReview = nextReview.ARDueDate;
                                }
                                else
                                {
                                    project.NextReview = nextReview.PCRDueDate;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _errorengine.LogError(ex, "AR/PCR for Dashboard", User);
                    }
                }



                try
                {

                    //IEnumerable<String> resources = project.Teams.Select(x => x.TeamID.TrimStart('R'));
                    IEnumerable<String> projectList = userprojectviewmodel.Select(x => x.ProjectID);

                    using (_ariesService)
                    {
                        //Get all approved budgets.
                        projectApprovedBudgets =
                            await _ariesService.GetProjectApprovedBudgetsByPortfolioAsync(projectList, User);

                    }

                    // Find if user is authorised
                    // Check if the user is authorised & if not, set budget to 0 & display message about restricted access
                    bool userAuthorised = await IsAuthorised(User);

                    if (projectApprovedBudgets != null && userAuthorised == true)
                    {
                        // Loop through projectApprovedBudgets and map the approved budget value
                        // Start loop
                        foreach (UserProjectsViewModel userproject in userprojectviewmodel)
                        {
                            //Get current row of ProjectData
                            var projectdashboard =
                                userprojectviewmodel.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID));

                            //Get Current Row of ProjectBudget
                            // Check to see if the result will be null, if it is then there is no budget so dont map, mapping will throw exception
                            if (projectApprovedBudgets.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID)) !=
                                null)
                            {
                                var projectbudget =
                                    projectApprovedBudgets.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID));

                                // Update the approved budget to the projectdashboard model.
                                projectdashboard.ApprovedBudget = projectbudget.ApprovedBudget;
                            }

                        }
                    }
                    else if (projectApprovedBudgets != null && userAuthorised == false)
                    {
                        // Loop through projectApprovedBudgets and set budget value to 0
                        // Start loop
                        foreach (UserProjectsViewModel userproject in userprojectviewmodel)
                        {
                            //Get current row of ProjectData
                            var projectdashboard =
                                userprojectviewmodel.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID));

                            //Get Current Row of ProjectBudget
                            // Check to see if the result will be null, if it is then there is no budget so dont map, mapping will throw exception
                            if (projectApprovedBudgets.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID)) !=
                                null)
                            {
                                // Set the approved budget to 0 for the projectdashboard model.
                                projectdashboard.ApprovedBudget = 0;
                            }

                        }

                        // Add a message confirming budgets show as 0 due to restricted access
                        string message =
                            "As you have restricted access to AMP, some budget information will be unavailable or show as £0.";
                        dashboardVM.ARIESWebServiceMessage = message;
                    }


                }
                catch (Exception ex)
                {
                    dashboardVM.ARIESWebServiceMessage =
                        "Sorry, ARIES doesn't seem to be available at the moment. Your financial data cannot be shown.";
                    projectApprovedBudgets = new List<ProjectApprovedBudget>();
                    // Executes the error engine (ProjectID is optional, exception)

                    //if (userprojectviewmodel.Count() == 0)
                    //{
                    //  //  errorengine.LogError(ex, "User Has No Projects In Dashboard");
                    //}
                    //else
                    //{
                    _errorengine.LogError(ex, User);
                    // }
                }




                //Convert the IEnumerable<userprojectviewmodel into a PagedList. Now that's magic! Not a lot. sorry.......
                PagedList<UserProjectsViewModel> pagedprojects =
                    new PagedList<UserProjectsViewModel>(userprojectviewmodel, pagenumber, pagesize);

                dashboardVM.userprojects = pagedprojects;



                return dashboardVM;
            }
        }

        /// <summary>GetProjectDocuments - Method to get a single detailed project record and the Project Documents.</summary>
        /// <param name="ProjectID">6 digit number (passed as a string) which uniquely identifies the project.</param>
        /// <returns>A ViewModel of type ProjectDetailsViewModel.</returns>
        /// <remarks>Returns a ProjectMaster and ProjectDocuments</remarks>

        public Boolean AddProject(String ProjectID, String userName, DashboardViewModel dashboardViewModel, DashboardViewModel originalVM, IValidationDictionary validationDictionary)
        {

            if (!ValidateAddProject(ProjectID, dashboardViewModel, originalVM, validationDictionary))
                return false;

            Portfolio portfolio = new Portfolio();
            portfolio.ProjectID = ProjectID;
            portfolio.UserID = userName;
            portfolio.LastUpdated = DateTime.Now;
            portfolio.Status = "A";

            try
            {

                _projectrepository.AddProject(portfolio);
                _projectrepository.Save();
                return true;

            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(ProjectID, ex, userName);
                return false;
            }
        }

        public Boolean AddProject(String ProjectID, String userName)
        {


            Portfolio portfolio = new Portfolio();
            portfolio.ProjectID = ProjectID;
            portfolio.UserID = userName;
            portfolio.LastUpdated = DateTime.Now;
            portfolio.Status = "A";

            try
            {

                _projectrepository.AddProject(portfolio);
                _projectrepository.Save();
                return true;

            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(ProjectID, ex, userName);
                return false;
            }
        }

        public Boolean RemoveProject(String ProjectID, String userName)
        {
            try
            {
                //Get Portfolio
                Portfolio portfolio = new Portfolio();

                portfolio = _projectrepository.GetProjectPortfolio(ProjectID, userName);

                if (portfolio != null)
                {

                    _projectrepository.RemoveProject(portfolio);
                    _projectrepository.Save();
                }
                return true;

            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(ProjectID, ex, userName);
                return false;
            }
        }

        /// <summary>GetProject - Method to get a single detailed project record </summary>
        /// <param name="ProjectID">6 digit number (passed as a string) which uniquely identifies the project.</param>
        /// <returns>A ViewModel of type ProjectDetailsViewModel.</returns>
        /// <remarks>Returns a ProjectMaster within the ViewModel</remarks>
        public async Task<ProjectViewModel> GetProject(string ProjectID)
        {

            //Get the project master data and populate the ViewModel
            ProjectViewModel projectViewModel = await PopulateViewModelWithProjectMasterData(ProjectID);

            if (projectViewModel.ProjectMaster != null)
            {
                return projectViewModel;
            }
            else
            {
                //The project doesn't exist. return a null to the Project Controller which will throw an HTTPNotFound Exception.
                return null;
            }

        }

        public async Task<ProjectReviewVM> GetProjectReviews(string ProjectID, string user)
        {
            ProjectReviewVM projectReviewVm = new ProjectReviewVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);

            if (project != null)
            {

                projectReviewVm = await returnProjectReviewVm(project, user);

                ProjectHeaderVM headerVm = ReturnProjectHeaderVm(project, user);

                projectReviewVm.ProjectHeader = headerVm;

                return projectReviewVm;
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> ValidateCheckProjectHasInputter(string ProjectID)
        {
            ProjectMaster projectMaster = _projectrepository.GetProject(ProjectID);
            //Check for Inputter

            int InputterExists = 0;

            foreach (Team team in projectMaster.Teams.Where(x => x.Status == "A"))
            {
                if (team.RoleID == "PI")
                {
                    InputterExists = 1;
                }

            }

            if (InputterExists != 1)
            {
                return false;
            }

            return true;
        }


        public async Task<IEnumerable<ReviewOutputVM>> GetProjectReviewScores(string projectId, int reviewId)
        {
            IEnumerable<ReviewOutputVM> reviewOutputVms = new List<ReviewOutputVM>();
            List<ReviewOutput> reviewOutputs = _projectrepository.GetReviewScores(projectId, reviewId);

            Mapper.CreateMap<ReviewOutput, ReviewOutputVM>();
            reviewOutputVms = Mapper.Map<List<ReviewOutput>, List<ReviewOutputVM>>(reviewOutputs);
            ////projectViewModel.ProjectReviews =   Mapper.Map<List<ReviewOutput>, List<ReviewVM>>(reviewOutputs);

            return reviewOutputVms;
        }


        public async Task<IEnumerable<ReviewDocumentVM>> GetReviewDocuments(string projectId, int reviewId)
        {
            IEnumerable<ReviewDocumentVM> reviewDocumentVms = new List<ReviewDocumentVM>();
            List<ReviewDocument> reviewDocuments = _projectrepository.GetReviewDocuments(projectId, reviewId);
            Mapper.CreateMap<ReviewDocument, ReviewDocumentVM>();
            reviewDocumentVms = Mapper.Map<List<ReviewDocument>, List<ReviewDocumentVM>>(reviewDocuments);
            foreach (ReviewDocumentVM reviewDoc in reviewDocumentVms)
            {
                reviewDoc.DocumentLink = _documentService.ReturnDocumentUrl(reviewDoc.DocumentID,
                    reviewDoc.DocSource);
            }
            return reviewDocumentVms;
        }




        public async Task<RiskRegisterVM> GetRiskRegister(String ProjectID, String user)
        {
            RiskRegisterVM riskRegisterVm = new RiskRegisterVM();
            ProjectMaster project = ReturnProjectMaster(ProjectID);
            //OverallRiskRatingVM overallRiskRatingVm = new OverallRiskRatingVM();
            if (project != null)
            {
                try
                {
                    RiskService riskService = new RiskService(_projectrepository, _personService, _loggingengine, _errorengine, _documentService);

                    //Add Header Data
                    riskRegisterVm = await riskService.RiskDetailsByProject(ProjectID, user);
                    riskRegisterVm.ProjectHeader = ReturnProjectHeaderVm(project, user);
                    riskRegisterVm.RiskItemVm = new RiskItemVM();

                    if (await ReturnCurrentUserMemberOfGroup(user, ProjectID, null, null) == "Team")
                    {
                        riskRegisterVm.IsTeamMember = true;
                    }
                }
                catch (Exception ex)
                {
                    _errorengine.LogError(ProjectID, ex, user);
                }
                return riskRegisterVm;
            }
            else
            {
                return null;
            }
        }
        public bool UpdateOverallRiskRating(RiskRegisterVM riskRegisterVm, string user)
        {
            try
            {
                OverallRiskRating overallRiskRatingtoInsert = new OverallRiskRating();
                overallRiskRatingtoInsert.ProjectID = riskRegisterVm.ProjectHeader.ProjectID;
                overallRiskRatingtoInsert.Comments = riskRegisterVm.OverallRiskRatingVm.Comments;
                overallRiskRatingtoInsert.RiskScore = riskRegisterVm.OverallRiskRatingVm.RiskScore;
                overallRiskRatingtoInsert.UserID = user;
                overallRiskRatingtoInsert.LastUpdated = DateTime.Now;

                _projectrepository.InsertOverallRiskRating(overallRiskRatingtoInsert);
                _projectrepository.Save();
                return true;

            }
            catch (Exception ex)
            {
                _errorengine.LogError(riskRegisterVm.OverallRiskRatingVm.ProjectID, ex, user);
                return false;
            }
        }
        public async Task<IEnumerable<RiskRegisterVM>> GetRiskRegisterDocumets(string projectId, string user)
        {
            IEnumerable<RiskRegisterVM> riskDocumentVms = new List<RiskRegisterVM>();
            List<RiskDocument> riskDocuments = _projectrepository.GetRiskRegisters(projectId);

            Mapper.CreateMap<RiskDocument, RiskRegisterVM>();
            riskDocumentVms = Mapper.Map<List<RiskDocument>, List<RiskRegisterVM>>(riskDocuments);
            return riskDocumentVms;
        }
        //Add RiskregisterItem 
        /*
        public Tuple<string> AddRiskRegisterItem(RiskRegisterVM riskRegisterVm, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                RiskRegister riskRegister = new RiskRegister();
                Mapper.CreateMap<RiskItemVM, RiskRegister>();
                Mapper.Map<RiskItemVM, RiskRegister>(riskRegisterVm.RiskItemVm, riskRegister);
                riskRegister.ProjectID = riskRegisterVm.ProjectHeader.ProjectID;
                riskRegister.LastUpdated = DateTime.Now;
                riskRegister.UserID = user;

                 _projectrepository.UpdateRiskItem(riskRegister);
                _projectrepository.Save();
            }
            catch (Exception exception)
            {
                _errorengine.LogError(riskRegisterVm.ProjectHeader.ProjectID, exception, user);
                Tuple<string> failedvaluesTuple = new Tuple<string>("Failed");
                return failedvaluesTuple;
            }
            Tuple<string> valuesTuple = new Tuple<string>("Success");
            return valuesTuple;
        }*/

        //AddRiskDocument
        public Tuple<string> AddRiskDocument(RiskRegisterVM riskRegisterVm, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                RiskDocument riskRegister = new RiskDocument();
                riskRegister.DocumentID = riskRegisterVm.DocumentID;
                riskRegister.Description = riskRegisterVm.Description;
                riskRegister.ProjectID = riskRegisterVm.ProjectHeader.ProjectID;
                riskRegister.LastUpdate = DateTime.Now;
                riskRegister.UserID = user;
                _projectrepository.InsertRiskDocuments(riskRegister);
                _projectrepository.Save();
            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(riskRegisterVm.ProjectHeader.ProjectID, exception, user);
                Tuple<string> failedvaluesTuple = new Tuple<string>("Failed");
                return failedvaluesTuple;
            }
            Tuple<string> valuesTuple = new Tuple<string>("Success");
            return valuesTuple;
        }



        //GetOverallRiskRatings
        public async Task<IEnumerable<OverallRiskRatingVM>> GetOverallRiskRatings(string projectId, string user)
        {
            IEnumerable<OverallRiskRatingVM> overallRiskRatingVms = new List<OverallRiskRatingVM>();
            List<OverallRiskRating> overallRisk = _projectrepository.GetOverallRiskRatings(projectId);

            Mapper.CreateMap<OverallRiskRating, OverallRiskRatingVM>();
            overallRiskRatingVms = Mapper.Map<List<OverallRiskRating>, List<OverallRiskRatingVM>>(overallRisk);
            return overallRiskRatingVms;
        }
        public async Task<RiskItemVM> GetRiskregisterNew(string ProjectId, string user)
        {
            RiskItemVM riskItemVm = new RiskItemVM();
            ProjectMaster project = ReturnProjectMaster(ProjectId);
            if (project != null)
            {
                riskItemVm.ProjectHeader = ReturnProjectHeaderVm(project, user);

                return riskItemVm;
            }
            else
            {
                return null;
            }
        }
        public async Task<ProjectVM> GetProjectVM(string ProjectID, string user)
        {
            ProjectVM projectVm = new ProjectVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);

            if (project != null)
            {


                ProjectHeaderVM headerVm = ReturnProjectHeaderVm(project, user);

                projectVm.ProjectHeader = headerVm;


                //MapProjectMasterToProjectMasterViewModel(projectVm, project);

                //Define a map between the ProjectMaster object and the ProjectMasterVM
                Mapper.CreateMap<ProjectMaster, ProjectVM>();

                //Map the objects
                Mapper.Map(project, projectVm);

                ProjectTeamMemberVM projectSRO = new ProjectTeamMemberVM();
                projectSRO = await ReturnProjectSRO(project);

                projectVm.ProjectSRO = projectSRO;



                //Get the project dates
                //PopulateViewModelWithProjectDates(projectVm, project.ProjectDate);

                ProjectDateVM projectDateVM = new ProjectDateVM();

                if (project.ProjectDate != null)
                {
                    //Define a mapping between the ProjectDate data object and the ProjectDateVM ViewModel.
                    Mapper.CreateMap<ProjectDate, ProjectDateVM>();


                    //Do the mapping.
                    Mapper.Map(project.ProjectDate, projectDateVM);

                    projectVm.ProjectDates = projectDateVM;

                    DecomposeDateTimesToDateParts(projectVm);

                }

                //Populate static project data into the ViewModel
                if (project.BudgetCentre != null)
                {
                    projectVm.BudgetCentreDescription = project.BudgetCentre.BudgetCentreDescription + " - " + project.BudgetCentre.BudgetCentreID;
                }

                if (project.Stage1 != null)
                {
                    projectVm.StageDescription = project.Stage1.StageDescription;
                }


                //Populate Core Project Data into the ViewModel
                if (project.ProjectInfo != null)
                {

                    projectVm.RiskAtApproval = project.ProjectInfo.RiskAtApproval;
                }
                projectVm.RiskLookups = _projectrepository.LookUpRiskTypes();

                //PopulateViewModelWithProjectCoreData(projectVm, project);

                //TODO - Business Logic that calculates the Operational Status. For Now, set it to Active.
                projectVm.OpStatus = "Active";



                //Close Project Workflow Section

                //Does the project have any workflows?
                IEnumerable<WorkflowMaster> projectWorkflows = project.WorkflowMasters;

                //Find the latest Close Project Workflow
                WorkflowMaster latestWorkflow = new WorkflowMaster();
                if (projectWorkflows.Any(x => x.TaskID == 1))
                {
                    latestWorkflow = projectWorkflows.Where(x => x.TaskID == 1).MaxBy(y => y.ActionDate);
                }

                IEnumerable<WorkflowTask> workflowTasks = _projectrepository.GetWorkflowTasks();

                projectVm.CloseProjectRequest = await GetLatestWorkflowRequest(ProjectID, projectWorkflows, workflowTasks.FirstOrDefault(x => x.Description == "Close Project").TaskID, user);
                projectVm.CloseProjectResponse = await GetLatestWorkflowResponse(ProjectID, projectWorkflows, workflowTasks.FirstOrDefault(x => x.Description == "Close Project").TaskID, user);
                projectVm.CloseProjectRole = await GetWorkflowRole(ProjectID, projectVm.CloseProjectRequest, user);

                ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

                projectwfvm = await IsProjectinWorkflow(project, user);

                projectVm.WFCheck = projectwfvm;


                projectVm.CanSendForClosure = CanProjectBeSentForClosure(project);

                //Can the project be sent for approval?
                projectVm.CanSendForApproval = CanProjectBeSentForApproval(projectVm);

                if (await ReturnCurrentUserMemberOfGroup(user, project.ProjectID, null, null) == "Team")
                {
                    projectVm.IsProjectTeam = true;
                }

                return projectVm;
            }
            else
            {
                return null;
            }
        }

        private bool CanProjectBeSentForAandDApproval(ProjectMaster project)
        {
            Int32 intStage;

            bool result = Int32.TryParse(project.Stage, out intStage);

            if (intStage == 0 || intStage == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanProjectBeSentForFastTrackApproval(ProjectMaster project)
        {
            Int32 intStage;

            bool result = Int32.TryParse(project.Stage, out intStage);

            if (intStage == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanProjectBeSentForApprovalorReApproval(ProjectMaster project)
        {
            Int32 intStage;

            bool result = Int32.TryParse(project.Stage, out intStage);

            if (intStage > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanProjectBeSentForClosure(ProjectMaster project)
        {
            Int32 intStage;

            bool result = Int32.TryParse(project.Stage, out intStage);

            if (project.Performance != null && project.Performance.PCRRequired == "No" || intStage < 5 || project.ReviewMasters.Any(x => x.ReviewType == "Project Completion Review" && x.Approved == "1"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanProjectBeSentForApproval(ProjectVM projectVm)
        {
            //TODO ***I think this method can be deleted and the boolean removed from the ViewModel. Investigate when you have some time - 10/02/2016 CJF***
            //What stage is the project?
            switch (projectVm.Stage)
            {
                case "1":
                case "2":
                case "6":
                case "7":
                    return false;

            }

            //Is the project already in workflow?
            if (projectVm.WFCheck.Status == true)
            {
                return false;
            }

            //Should we check if you are in the team? Or make that part of the overall ViewModel and lock down every page using it? 
            return true;
        }

        private async Task<string> GetWorkflowRole(string projectId, WorkflowMasterVM workflowRequestVM, string user)
        {
            return await ReturnCurrentUserMemberOfGroup(user, projectId, workflowRequestVM.Recipient, workflowRequestVM.StageID.ToString());
        }

        private async Task<WorkflowMasterVM> GetLatestWorkflowResponse(string ProjectID, IEnumerable<WorkflowMaster> projectWorkflows, int workflowTypeId, string user)
        {
            WorkflowMaster latestWorkflow = new WorkflowMaster();
            WorkflowMaster workflowResponse = new WorkflowMaster();
            WorkflowMaster workflowRequest = new WorkflowMaster();
            WorkflowMasterVM WorkflowResponseVM = new WorkflowMasterVM();

            Mapper.CreateMap<WorkflowMaster, WorkflowMasterVM>();


            if (projectWorkflows.Any(x => x.TaskID == workflowTypeId))
            {
                latestWorkflow = projectWorkflows.Where(x => x.TaskID == workflowTypeId).MaxBy(y => y.ActionDate);
            }

            if (latestWorkflow.Status == null)
            {
                return null;
            }
            else if (latestWorkflow.StageID == 2 && latestWorkflow.Status == "C")
            {
                //The workflow was approved. 
                workflowRequest = projectWorkflows.FirstOrDefault(x => x.TaskID == latestWorkflow.TaskID && x.WorkFlowID == latestWorkflow.WorkFlowID && x.StageID == 1);

                //Map 
                Mapper.Map<WorkflowMaster, WorkflowMasterVM>(latestWorkflow, WorkflowResponseVM);

                WorkflowResponseVM.RequesterName = await PopulateDisplayName(WorkflowResponseVM.ActionBy);
            }
            else if (latestWorkflow.StageID == 3 && latestWorkflow.Status == "C")
            {
                //The workflow was rejected.
                workflowRequest = projectWorkflows.FirstOrDefault(x => x.TaskID == latestWorkflow.TaskID && x.WorkFlowID == latestWorkflow.WorkFlowID && x.StageID == 1);

                Mapper.Map<WorkflowMaster, WorkflowMasterVM>(latestWorkflow, WorkflowResponseVM);

                WorkflowResponseVM.RequesterName = await PopulateDisplayName(WorkflowResponseVM.ActionBy);
            }
            else if (latestWorkflow.StageID == 1 && latestWorkflow.Status == "A")
            {
                //There is an active project Workflow.
                WorkflowResponseVM.RequesterName = await PopulateDisplayName(latestWorkflow.Recipient);
            }

            return WorkflowResponseVM;
        }

        private async Task<WorkflowMasterVM> GetLatestWorkflowRequest(string projectId, IEnumerable<WorkflowMaster> projectWorkflows, int workflowTypeId, string user)
        {
            WorkflowMaster latestWorkflow = new WorkflowMaster();
            WorkflowMaster workflowRequest = new WorkflowMaster();
            WorkflowMasterVM WorkflowRequestVM = new WorkflowMasterVM();

            Mapper.CreateMap<WorkflowMaster, WorkflowMasterVM>();

            if (projectWorkflows.Any(x => x.TaskID == workflowTypeId))
            {
                latestWorkflow = projectWorkflows.Where(x => x.TaskID == workflowTypeId).MaxBy(y => y.ActionDate);
            }
            if (latestWorkflow.Status == null)
            {
                //No workflow submitted, approved or rejected.
                WorkflowRequestVM.RequesterName = await PopulateDisplayName(user);
            }
            else if (latestWorkflow.StageID == 2 && latestWorkflow.Status == "C")
            {
                //The workflow has been approved. 
                workflowRequest = projectWorkflows.FirstOrDefault(x => x.TaskID == latestWorkflow.TaskID && x.WorkFlowID == latestWorkflow.WorkFlowID && x.StageID == 1);

                //Map 
                Mapper.Map<WorkflowMaster, WorkflowMasterVM>(workflowRequest, WorkflowRequestVM);

                WorkflowRequestVM.RequesterName = await PopulateDisplayName(WorkflowRequestVM.ActionBy);
            }
            else if (latestWorkflow.StageID == 3 && latestWorkflow.Status == "C")
            {
                //The project closure was rejected.
                workflowRequest = projectWorkflows.FirstOrDefault(x => x.TaskID == latestWorkflow.TaskID && x.WorkFlowID == latestWorkflow.WorkFlowID && x.StageID == 1);

                Mapper.Map<WorkflowMaster, WorkflowMasterVM>(workflowRequest, WorkflowRequestVM);

                WorkflowRequestVM.RequesterName = await PopulateDisplayName(workflowRequest.ActionBy);
            }
            else if (latestWorkflow.StageID == 1 && latestWorkflow.Status == "A")
            {
                //There is a request in Workflow.

                Mapper.Map<WorkflowMaster, WorkflowMasterVM>(latestWorkflow, WorkflowRequestVM);

                WorkflowRequestVM.RequesterName = await PopulateDisplayName(WorkflowRequestVM.ActionBy);
            }

            return WorkflowRequestVM;

        }

        public async Task<ProjectMarkersVM> GetProjectMarkers(string ProjectID, string user)
        {
            ProjectMarkersVM projectMarkersVm = new ProjectMarkersVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);

            if (project != null)
            {
                ProjectHeaderVM headerVm = ReturnProjectHeaderVm(project, user);

                projectMarkersVm.ProjectHeader = headerVm;

                ////Define a map between the ProjectMaster object and the ProjectMasterVM
                //Mapper.CreateMap<ProjectMaster, ProjectHeaderVM>();

                ////Map the objects
                //Mapper.Map(project, projectMarkersVm);

                //Populate static project data into the ViewModel
                if (project.BudgetCentre != null)
                {
                    projectMarkersVm.BudgetCentreDescription = project.BudgetCentre.BudgetCentreDescription;
                    projectMarkersVm.BudgetCentreID = project.BudgetCentre.BudgetCentreID;
                }

                if (project.Stage1 != null)
                {
                    projectMarkersVm.StageDescription = project.Stage1.StageDescription;
                }


                //Populate Core Project Data into the ViewModel
                if (project.Markers1 != null)
                {
                    projectMarkersVm.GenderEquality = project.Markers1.GenderEquality;
                    projectMarkersVm.HIVAIDS = project.Markers1.HIVAIDS;
                    projectMarkersVm.Biodiversity = project.Markers1.Biodiversity;
                    projectMarkersVm.Mitigation = project.Markers1.Mitigation;
                    projectMarkersVm.Adaptation = project.Markers1.Adaptation;
                    projectMarkersVm.Desertification = project.Markers1.Desertification;
                    projectMarkersVm.Disability = project.Markers1.Disability;
                    projectMarkersVm.DisabilityPercentage = project.Markers1.DisabilityPercentage;
                }

                //TODO - Business Logic that calculates the Operational Status. For Now, set it to Active.
                projectMarkersVm.OpStatus = "Active";

                //Create the HIV/AIDS CCOObjectiveVM
                CrossCuttingObjectiveVM HIVCCO = new CrossCuttingObjectiveVM();
                HIVCCO.CCOType = "HIVAIDS";
                List<CCOValuesVM> HIVAIDSCCOValues = new List<CCOValuesVM>();
                HIVAIDSCCOValues.Add(ReturnCCOValuesVM("Principal", "PRINCIPAL"));
                HIVAIDSCCOValues.Add(ReturnCCOValuesVM("Significant", "SIGNIFICANT"));
                HIVAIDSCCOValues.Add(ReturnCCOValuesVM("Not Targeted", "NOTTARGETED"));
                HIVCCO.CCOValues = HIVAIDSCCOValues;
                HIVCCO.SelectedCCOValue = projectMarkersVm.HIVAIDS;
                projectMarkersVm.HIVCCO = HIVCCO;


                //Create the Gender CCOObjectiveVM
                CrossCuttingObjectiveVM GenderCCO = new CrossCuttingObjectiveVM();
                GenderCCO.CCOType = "GenderEquality";
                List<CCOValuesVM> GenderCCOValues = new List<CCOValuesVM>();
                GenderCCOValues.Add(ReturnCCOValuesVM("Principal", "PRINCIPAL"));
                GenderCCOValues.Add(ReturnCCOValuesVM("Significant", "SIGNIFICANT"));
                GenderCCOValues.Add(ReturnCCOValuesVM("Gender considered but not targeted", "NOTTARGETED"));
                GenderCCO.CCOValues = GenderCCOValues;
                GenderCCO.SelectedCCOValue = projectMarkersVm.GenderEquality;
                projectMarkersVm.GenderCCO = GenderCCO;


                //Create the Biodiversity CCOObjectiveVM
                CrossCuttingObjectiveVM BiodiversityCCO = new CrossCuttingObjectiveVM();
                BiodiversityCCO.CCOType = "Biodiversity";
                List<CCOValuesVM> BiodiversityCCOValues = new List<CCOValuesVM>();
                BiodiversityCCOValues.Add(ReturnCCOValuesVM("Principal", "PRINCIPAL"));
                BiodiversityCCOValues.Add(ReturnCCOValuesVM("Significant", "SIGNIFICANT"));
                BiodiversityCCOValues.Add(ReturnCCOValuesVM("Not Targeted", "NOTTARGETED"));
                BiodiversityCCO.CCOValues = BiodiversityCCOValues;
                BiodiversityCCO.SelectedCCOValue = projectMarkersVm.Biodiversity;
                projectMarkersVm.BiodiversityCCO = BiodiversityCCO;


                //Create the Mitigation CCOObjectiveVM
                CrossCuttingObjectiveVM MitigationCCO = new CrossCuttingObjectiveVM();
                BiodiversityCCO.CCOType = "Mitigation";
                List<CCOValuesVM> MitigationCCOValues = new List<CCOValuesVM>();
                MitigationCCOValues.Add(ReturnCCOValuesVM("Principal", "PRINCIPAL"));
                MitigationCCOValues.Add(ReturnCCOValuesVM("Significant", "SIGNIFICANT"));
                MitigationCCOValues.Add(ReturnCCOValuesVM("Not Targeted", "NOTTARGETED"));
                MitigationCCO.CCOValues = MitigationCCOValues;
                MitigationCCO.SelectedCCOValue = projectMarkersVm.Mitigation;
                projectMarkersVm.MitigationCCO = MitigationCCO;


                //Create the Adaptation CCOObjectiveVM
                CrossCuttingObjectiveVM AdaptationCCO = new CrossCuttingObjectiveVM();
                BiodiversityCCO.CCOType = "Adaptation";
                List<CCOValuesVM> AdaptationCCOValues = new List<CCOValuesVM>();
                AdaptationCCOValues.Add(ReturnCCOValuesVM("Principal", "PRINCIPAL"));
                AdaptationCCOValues.Add(ReturnCCOValuesVM("Significant", "SIGNIFICANT"));
                AdaptationCCOValues.Add(ReturnCCOValuesVM("Not Targeted", "NOTTARGETED"));
                AdaptationCCO.CCOValues = AdaptationCCOValues;
                AdaptationCCO.SelectedCCOValue = projectMarkersVm.Adaptation;
                projectMarkersVm.AdaptationCCO = AdaptationCCO;


                //Create the Desertification CCOObjectiveVM
                CrossCuttingObjectiveVM DesertificationCCO = new CrossCuttingObjectiveVM();
                BiodiversityCCO.CCOType = "Desertification";
                List<CCOValuesVM> DesertificationCCOValues = new List<CCOValuesVM>();
                DesertificationCCOValues.Add(ReturnCCOValuesVM("Targeted", "TARGETED"));
                DesertificationCCOValues.Add(ReturnCCOValuesVM("Not Targeted", "NOTTARGETED"));
                DesertificationCCO.CCOValues = DesertificationCCOValues;
                DesertificationCCO.SelectedCCOValue = projectMarkersVm.Desertification;
                projectMarkersVm.DesertificationCCO = DesertificationCCO;


                // ****** Create the Disability inclusion CCOObjectiveVM
                CrossCuttingObjectiveVM DisabilityCCO = new CrossCuttingObjectiveVM();
                DisabilityCCO.CCOType = "DisabilityInclusion";
                List<CCOValuesVM> DisabilityCCOValues = new List<CCOValuesVM>();
                DisabilityCCOValues.Add(ReturnCCOValuesVM("Principal", "PRINCIPAL"));
                DisabilityCCOValues.Add(ReturnCCOValuesVM("Significant", "SIGNIFICANT"));
                DisabilityCCOValues.Add(ReturnCCOValuesVM("Will not address disability inclusion", "NOTTARGETED"));
                DisabilityCCO.CCOValues = DisabilityCCOValues;
                DisabilityCCO.SelectedCCOValue = projectMarkersVm.Disability;
                projectMarkersVm.DisabilityCCO = DisabilityCCO;


                ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

                projectwfvm = await IsProjectinWorkflow(project, user);

                projectMarkersVm.WFCheck = projectwfvm;

                return projectMarkersVm;
            }
            else
            {
                return null;
            }
        }

        private bool IsUserWorkflowApprover(string workflowApprover, string user)
        {
            if (workflowApprover.Trim() == user.Trim())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<ProjectWFCheckVM> IsProjectinWorkflow(ProjectMaster project, string user)
        {
            IEnumerable<WorkflowMaster> workflowMasters;
            IEnumerable<ReviewMaster> reviewMasters;
            IEnumerable<ReviewDeferral> reviewDeferrals;
            IEnumerable<ReviewExemption> reviewExemptions;

            ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

            //Check for Workflow in ARIES
            projectwfvm = await _ariesService.ProjectWFCheck(project.ProjectID, user);

            if (projectwfvm.Status == true)
            {
                return projectwfvm;
            }

            projectwfvm = IsProjectInAMPWorkflow(project, user);

            return projectwfvm;
        }


        private ProjectWFCheckVM IsProjectInAMPWorkflow(ProjectMaster project, string user)
        {
            IEnumerable<WorkflowMaster> workflowMasters;
            IEnumerable<ReviewMaster> reviewMasters;
            IEnumerable<ReviewDeferral> reviewDeferrals;
            IEnumerable<ReviewExemption> reviewExemptions;

            ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

            workflowMasters = project.WorkflowMasters;

            //Check for Active Workflow Objects in AMP
            if (project.WorkflowMasters.Count > 0)
            {
                WorkflowMaster projectInWorkflow = new WorkflowMaster();
                if (workflowMasters.Any(x => x.Status == "A"))
                {
                    projectInWorkflow = workflowMasters.First(x => x.Status == "A");
                    projectwfvm.Status = true;
                    projectwfvm.TaskId = projectInWorkflow.TaskID;
                    projectwfvm.WorkFlowDescription = projectInWorkflow.WorkflowTask.Description;
                    projectwfvm.IsWorkFlowApprover = IsUserWorkflowApprover(projectInWorkflow.Recipient, user);
                    return projectwfvm;
                }
            }

            reviewMasters = project.ReviewMasters;
            reviewDeferrals = project.ReviewDeferrals;
            reviewExemptions = project.ReviewExemptions;
            ReviewMaster review = new ReviewMaster();

            //Check for AR awaiting approval
            if (reviewMasters.Any(x => x.StageID == "1" && x.ReviewType == "Annual Review"))
            {
                review = reviewMasters.FirstOrDefault(x => x.StageID == "1" && x.ReviewType == "Annual Review");
                projectwfvm.WorkFlowDescription = "Annual Review";
                projectwfvm.TaskId = Constants.AnnualReviewTaskId;
                projectwfvm.Status = true;
                projectwfvm.IsWorkFlowApprover = IsUserWorkflowApprover(review.Approver, user);
                return projectwfvm;
            }

            //Check for PCR awaiting approval
            if (reviewMasters.Any(x => x.StageID == "1" && x.ReviewType == "Project Completion Review"))
            {
                review = reviewMasters.FirstOrDefault(x => x.StageID == "1" && x.ReviewType == "Project Completion Review");
                projectwfvm.WorkFlowDescription = "Project Completion Review";
                projectwfvm.TaskId = Constants.PCRTaskId;
                projectwfvm.Status = true;
                projectwfvm.IsWorkFlowApprover = IsUserWorkflowApprover(review.Approver, user);
                return projectwfvm;
            }

            //Check for Deferral awaiting approval
            if (reviewDeferrals.Any(x => x.StageID == "1"))
            {
                ReviewDeferral reviewDeferral = reviewDeferrals.FirstOrDefault(x => x.StageID == "1");
                projectwfvm.WorkFlowDescription = "Review Deferral";
                projectwfvm.TaskId = Constants.ReviewDeferralTaskId;
                projectwfvm.Status = true;
                projectwfvm.IsWorkFlowApprover = IsUserWorkflowApprover(reviewDeferral.Approver, user);
                return projectwfvm;
            }

            //Check for Exemptions awaiting approval
            if (reviewExemptions.Any(x => x.StageID == "1"))
            {
                ReviewExemption reviewExemption = reviewExemptions.FirstOrDefault(x => x.StageID == "1");
                projectwfvm.WorkFlowDescription = "Review Exemption";
                projectwfvm.TaskId = Constants.reviewExemptionTaskId;
                projectwfvm.Status = true;
                projectwfvm.IsWorkFlowApprover = IsUserWorkflowApprover(reviewExemption.Approver, user);
                return projectwfvm;
            }

            projectwfvm.WorkFlowDescription = "";
            projectwfvm.Status = false;
            projectwfvm.IsWorkFlowApprover = false;
            return projectwfvm;

        }


        private async Task<string> PopulateDisplayName(string empNo)
        {
            PersonDetails personDetails = await _personService.GetPersonDetails(empNo);

            String displayName = personDetails.Forename + " " + personDetails.Surname;

            return displayName;

        }
        private async Task<ProjectTeamMemberVM> ReturnProjectSRO(ProjectMaster project)
        {
            Team sro = project.Teams.FirstOrDefault(x => x.RoleID == "SRO" && x.Status == "A");
            if (sro != null)
            {
                //Get Employee details from the PersonService
                PersonDetails personDetails = await _personService.GetPersonDetails(sro.TeamID.TrimStart('R'));
                if (personDetails != null)
                {

                    ProjectTeamMemberVM sroVm = ReturnPopulatedProjectTeamMemberVm(sro, personDetails);
                    return sroVm;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public BudgetCentre ReturnBudgetCentre(string BudgetCentreID)
        {
            //BudgetCentre budgetCentre = new BudgetCentre();
            //budgetCentre = projectrepository.GetBudgetCentre(BudgetCentreID);
            return _projectrepository.GetBudgetCentre(BudgetCentreID);
        }

        public ProjectMaster ReturnProjectMaster(string ProjectID)
        {
            ProjectMaster project = new ProjectMaster();

            project = _projectrepository.GetProject(ProjectID);
            return project;
        }

        public ComponentMaster ReturnComponentMaster(string ComponentID)
        {
            ComponentMaster component = new ComponentMaster();
            component = _projectrepository.GetComponent(ComponentID);
            return component;
        }



        public async Task<ProjectViewModel> GetProjectTasks(String ProjectID)
        {

            //Get the project master data and populate the ViewModel
            ProjectViewModel projectViewModel = await PopulateViewModelWithProjectMasterData(ProjectID);

            //If the ProjectMaster exists, continue. Otherwise pass a null back to the controller which will handle the exception
            if (projectViewModel.ProjectMaster != null)
            {

                //Now get the project alerts and add them to the ProjectDashboardViewModel.
                //projectdetailsviewmodel.ProjectAlerts = CheckForAlerts();
                //projectdetailsviewmodel.ProjectAlerts = GetProjectAlerts(ProjectID);



                return projectViewModel;
            }
            else
            {
                return null;
            }

        }

        public async Task<ProjectLocationVM> GetGeoCoding(String ProjectID, String user)
        {
            //Get the project master data and populate the ViewModel
            ProjectLocationVM projectLocationVm = new ProjectLocationVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);

            if (project != null)
            {
                projectLocationVm.ProjectHeader = ReturnProjectHeaderVm(project, user);

                return projectLocationVm;
            }
            else
            {
                //The project doesn't exist. return a null to the Project Controller which will throw an HTTPNotFound Exception.
                return null;
            }
        }

        public async Task<ProjectFinanceVM> GetProjectFinancials(String ProjectID, String user)
        {
            ProjectFinanceVM projectFinanceVm = new ProjectFinanceVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);

            if (project != null)
            {

                try
                {
                    //Add Header data
                    projectFinanceVm.ProjectHeader = ReturnProjectHeaderVm(project, user);
                    //Get the Project Financials for the financials tab
                    projectFinanceVm.ProjectFinance = await PopulateViewModelWithProjectFinancials(ProjectID, user);

                }
                catch (Exception ex)
                {

                    projectFinanceVm.ProjectFinance = new List<ProjectFinanceRecordVM>();

                    //Add an error message
                    projectFinanceVm.FinanceWebServiceMessage = "Sorry, ARIES doesn't seem to be available just now. Your finance data cannot be shown.";

                    // Executes the error engine (ProjectID is optional, exception)
                    _errorengine.LogError(ProjectID, ex, user);
                }



                return projectFinanceVm;
            }
            else
            {
                //The project doesn't exist. return a null to the Project Controller which will throw an HTTPNotFound Exception.
                return null;
            }
        }
        public async Task<ProjectProcurementVM> GetProjectProcurement(String ProjectID, String user)
        {
            ProjectProcurementVM projectProcurementVm = new ProjectProcurementVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);


            if (project != null)
            {


                try
                {
                    //Add Header Data
                    projectProcurementVm.ProjectHeader = ReturnProjectHeaderVm(project, user);

                    //Get the Project Financials for the financials tab
                    projectProcurementVm.ProjectProcurement = await PopulateViewModelWithProjectProcurement(ProjectID, user);

                }
                catch (Exception ex)
                {
                    projectProcurementVm.ProjectProcurement = new List<ProcurementRecordVM>();

                    //Add an error message
                    projectProcurementVm.FinanceWebServiceMessage = "Sorry, ARIES doesn't seem to be available just now. Your finance data cannot be shown.";

                    // Executes the error engine (ProjectID is optional, exception)
                    _errorengine.LogError(ProjectID, ex, user);
                }



                return projectProcurementVm;
            }
            else
            {
                //The project doesn't exist. return a null to the Project Controller which will throw an HTTPNotFound Exception.
                return null;
            }
        }


        public async Task<ProjectDocumentsVM> GetProjectDocuments(String ProjectID, String user)
        {
            ProjectDocumentsVM projectDocumentsVm = new ProjectDocumentsVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);


            if (project != null)
            {


                try
                {
                    //Add Header Data
                    projectDocumentsVm.ProjectHeader = ReturnProjectHeaderVm(project, user);

                    //Get the Project Financials for the financials tab
                    projectDocumentsVm.ProjectDocument = await PopulateViewModelWithProjectDocuments(ProjectID, user);

                }
                catch (Exception ex)
                {
                    projectDocumentsVm.ProjectDocument = new List<DocumentRecordVM>();

                    //Add an error message
                    projectDocumentsVm.WebServiceMessage = "Sorry, the Document Service doesn't seem to be available just now. Your documents cannot be shown.";

                    // Executes the error engine (ProjectID is optional, exception)
                    _errorengine.LogError(ProjectID, ex, user);
                }



                return projectDocumentsVm;
            }
            else
            {
                //The project doesn't exist. return a null to the Project Controller which will throw an HTTPNotFound Exception.
                return null;
            }
        }

        public async Task<PublishedDocumentsVM> GetPublishedProjectDocumentsInDevTracker(String ProjectID, String user)
        {
            PublishedDocumentsVM publishedDocumentsVm = new PublishedDocumentsVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);


            if (project != null)
            {


                try
                {
                    //Add Header Data
                    publishedDocumentsVm.ProjectHeader = ReturnProjectHeaderVm(project, user);

                    publishedDocumentsVm.ProjectID = ProjectID;

                    //Get the Project Financials for the financials tab
                    publishedDocumentsVm.PublishedDocument = await PopulateViewModelWithPublishedDocuments(ProjectID, user);

                }
                catch (Exception ex)
                {
                    publishedDocumentsVm.PublishedDocument = new List<PublishedDocumentVM>();

                    publishedDocumentsVm.ProjectID = ProjectID;

                    //Add an error message
                    publishedDocumentsVm.WebServiceMessage = "Sorry, the Document Service doesn't seem to be available just now. Your documents cannot be shown.";

                    // Executes the error engine (ProjectID is optional, exception)
                    _errorengine.LogError(ProjectID, ex, user);
                }



                return publishedDocumentsVm;
            }
            else
            {
                //The project doesn't exist. return a null to the Project Controller which will throw an HTTPNotFound Exception.
                return null;
            }
        }

        public async Task<ProjectStatementVM> GetProjectStatements(String ProjectID, String user)
        {

            //Get the project master data and populate the ViewModel
            ProjectStatementVM projectStatementVm = new ProjectStatementVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);

            List<String> statementypes = new List<String>();

            statementypes.Add("Qualified Audited");
            statementypes.Add("Unqualified Audited");
            projectStatementVm.StatementTypes = statementypes;



            if (project != null)
            {


                try
                {
                    //Get the Project Financials for the financials tab
                    projectStatementVm.ProjectStatement = await PopulateViewModelWithProjectStatements(ProjectID, user);

                    projectStatementVm.NewProjectStatement = new NewStatement();

                    //Populate the header data
                    projectStatementVm.ProjectHeader = ReturnProjectHeaderVm(project, user);

                    ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

                    projectwfvm = await IsProjectinWorkflow(project, user);

                    projectStatementVm.WFCheck = projectwfvm;


                }
                catch (Exception ex)
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    _errorengine.LogError(ProjectID, ex, user);
                }



                return projectStatementVm;
            }
            else
            {
                //The project doesn't exist. return a null to the Project Controller which will throw an HTTPNotFound Exception.
                return null;
            }
        }

        public bool AddStatement(ProjectStatementVM projectStatementVM, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                //Validate the statement radio button, comments fields & document id
                if (!ValidateAddStatement(projectStatementVM, validationDictionary))
                    return false;

                // validate the dates entered
                if (!AuditedStatementDatesAreValid(projectStatementVM, validationDictionary))
                    return false;

                ////So the day, month and year are valid values. 
                // //Build dates to be added  on Save
                DateTime ReceivedDate = new DateTime(projectStatementVM.NewProjectStatement.ReceivedDate_Year.Value,
                 projectStatementVM.NewProjectStatement.ReceivedDate_Month.Value, projectStatementVM.NewProjectStatement.ReceivedDate_Day.Value);

                DateTime PeriodFrom = new DateTime(projectStatementVM.NewProjectStatement.PeriodFrom_Year.Value,
                projectStatementVM.NewProjectStatement.PeriodFrom_Month.Value, projectStatementVM.NewProjectStatement.PeriodFrom_Day.Value);

                DateTime PeriodTo = new DateTime(projectStatementVM.NewProjectStatement.PeriodTo_Year.Value,
                projectStatementVM.NewProjectStatement.PeriodTo_Month.Value, projectStatementVM.NewProjectStatement.PeriodTo_Day.Value);



                AuditedFinancialStatement newStatement = new AuditedFinancialStatement();

                //Get current statements
                IEnumerable<AuditedFinancialStatement> currentprojectStatement = _projectrepository.GetAllAuditedStatements(projectStatementVM.ProjectHeader.ProjectID) as IEnumerable<AuditedFinancialStatement>;

                newStatement.Currency = projectStatementVM.NewProjectStatement.Currency;
                newStatement.ProjectID = projectStatementVM.ProjectHeader.ProjectID;
                newStatement.DocumentID = projectStatementVM.NewProjectStatement.DocumentID;
                newStatement.DocSource = "V";
                newStatement.ReceivedDate = ReceivedDate;
                newStatement.PeriodFrom = PeriodFrom;
                newStatement.PeriodTo = PeriodTo;
                newStatement.reason_action = projectStatementVM.NewProjectStatement.reason_action;
                newStatement.StatementType = projectStatementVM.NewProjectStatement.StatementType;
                newStatement.Value = projectStatementVM.NewProjectStatement.Value;



                int statementid;
                try
                {
                    statementid = currentprojectStatement.Max(x => x.StatementID) + 1;
                }
                catch
                {
                    statementid = 0;
                }


                newStatement.StatementID = statementid;
                newStatement.Status = "A";
                newStatement.LastUpdated = DateTime.Today;
                newStatement.UserID = user;


                _projectrepository.InsertStatement(newStatement);

                _projectrepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(projectStatementVM.ProjectHeader.ProjectID, ex, user);
                return false;
            }

            ////Temp
            //return true;
        }

        public bool DeleteStatement(string projectId, int statementId, string user)
        {
            try
            {
                //Get the project statement.
                AuditedFinancialStatement statementToDelete = _projectrepository.GetAuditedStatement(projectId, statementId);

                //Set to Deleted using the Active Column.
                statementToDelete.Status = "C";
                statementToDelete.LastUpdated = DateTime.Now;
                statementToDelete.UserID = user;

                _projectrepository.UpdateStatement(statementToDelete);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(projectId, exception, user);
                return false;
            }
        }

        public async Task<ProjectEvaluationVM> GetProjectEvaluation(String ProjectID, String user)
        {
            ProjectEvaluationVM projectEvaluationVm = new ProjectEvaluationVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);

            if (project != null)
            {
                projectEvaluationVm.projectHeader = ReturnProjectHeaderVm(project, user);

                //Get the Project Finance Data 
                Evaluation projectEvaluation = _projectrepository.GetProjectEvaluations(ProjectID);

                if (projectEvaluation != null)
                {
                    //Define a mapping between the ProjectDate data object and the ProjectDateVM ViewModel.
                    Mapper.CreateMap<Evaluation, ProjectEvaluationVM>()
                        .ForMember(d => d.EvaluationDocuments, opt => opt.Ignore());
                    ;

                    //Do the mapping.
                    Mapper.Map(projectEvaluation, projectEvaluationVm);

                    //Manually map ID to UniqueID
                    //projectEvaluationVm.UniqueID = projectEvaluation.ID.ToString();

                    List<EvaluationDocumentVM> evaluationDocumentVms = new List<EvaluationDocumentVM>();

                    //Map the Evaluation documents to the ViewModel
                    foreach (EvaluationDocument document in projectEvaluation.EvaluationDocuments)
                    {
                        EvaluationDocumentVM evaluationDocumentVm = new EvaluationDocumentVM();

                        evaluationDocumentVm.DocumentID = document.DocumentID;
                        evaluationDocumentVm.Description = document.Description;
                        evaluationDocumentVm.EvaluationID = document.EvaluationID;
                        evaluationDocumentVm.ID = document.ID;
                        evaluationDocumentVm.DocSource = document.DocSource;
                        evaluationDocumentVm.DocumentLink = _documentService.ReturnDocumentUrl(document.DocumentID,
                            document.DocSource);
                        evaluationDocumentVm.LastUpdate = document.LastUpdate;
                        evaluationDocumentVm.Status = document.Status;
                        evaluationDocumentVm.UserID = evaluationDocumentVm.UserID;

                        evaluationDocumentVms.Add(evaluationDocumentVm);
                    }

                    projectEvaluationVm.EvaluationDocuments = evaluationDocumentVms;

                    //Populate the EvaluationType ViewModel with the values that can be selected. 
                    EvaluationTypeVM evaluationTypeVm = new EvaluationTypeVM();

                    IEnumerable<EvaluationType> evaluationTypes = _projectrepository.LookUpEvaluationTypes();

                    List<EvaluationTypeValuesVM> evaluationTypeValues = new List<EvaluationTypeValuesVM>();

                    foreach (EvaluationType evaluationType in evaluationTypes)
                    {
                        EvaluationTypeValuesVM evaluationTypeValue = new EvaluationTypeValuesVM();

                        evaluationTypeValue.EvaluationTypeID = evaluationType.EvaluationTypeID;
                        evaluationTypeValue.EvaluationTypeDescription = evaluationType.EvaluationDescription;
                        evaluationTypeValues.Add(evaluationTypeValue);
                    }
                    //Order the evaluation types as Impact, Performance,Process, None. Start with an alphabetical sort....
                    evaluationTypeValues = evaluationTypeValues.OrderBy(x => x.EvaluationTypeDescription).ToList();

                    //Then move none...
                    Int32 index = evaluationTypeValues.FindIndex(x => x.EvaluationTypeDescription.ToUpper() == "NONE");
                    EvaluationTypeValuesVM objectToMove = evaluationTypeValues[index];
                    evaluationTypeValues.RemoveAt(index);
                    evaluationTypeValues.Add(objectToMove);

                    evaluationTypeVm.EvaluationTypeValues = evaluationTypeValues;
                    evaluationTypeVm.SelectedEvaluationType = projectEvaluation.EvaluationTypeID;

                    projectEvaluationVm.EvaluationTypes = evaluationTypeVm;

                    //Populate the EvaluationManagement ViewModel with values that can be selected.
                    EvaluationManagementVM evaluationManagementVm = new EvaluationManagementVM();

                    IEnumerable<EvaluationManagement> evaluationManagements =
                        _projectrepository.LookUpEvaluationManagements();

                    List<EvaluationManagementValuesVM> evaluationManagementValues =
                        new List<EvaluationManagementValuesVM>();

                    foreach (EvaluationManagement evaluationManagement in evaluationManagements)
                    {
                        EvaluationManagementValuesVM evaluationManagementValue = new EvaluationManagementValuesVM();

                        evaluationManagementValue.EvaluationManagementID = evaluationManagement.EvaluationManagementID;
                        evaluationManagementValue.EvaluationManagementDescription =
                            evaluationManagement.EvaluationManagementDescription;
                        evaluationManagementValues.Add(evaluationManagementValue);
                    }

                    evaluationManagementVm.EvaluatioNManagementValues = evaluationManagementValues;

                    evaluationManagementVm.SelectedEvaluationManagement = projectEvaluation.ManagementOfEvaluation;

                    projectEvaluationVm.EvaluationManagements = evaluationManagementVm;

                }

                ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

                projectwfvm = await IsProjectinWorkflow(project, user);

                projectEvaluationVm.WFCheck = projectwfvm;

            }
            else
            {
                return null;
            }
            return projectEvaluationVm;
        }

        /// <summary>
        /// Add a new evaluation document. The document is an ID (used in the View to generate a hypelink to the Document management system) and a description. 
        /// The document is linked to the Evaluation ID, which is linked to the project.
        /// </summary>
        /// <param name="projectEvaluationVm">Project Evaluation ViewModel. Contains a NewEvaluationDocument ViewModel which contains the details of the document to be created.</param>
        /// <param name="validationDictionary">Validation Dictionary for the ProjectEvaluation ViewModel</param>
        /// <param name="user">User ID for logging</param>
        /// <returns></returns>
        public bool AddEvaluationDocument(ProjectEvaluationVM projectEvaluationVm, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                if (!ValidateAddDocument(projectEvaluationVm, validationDictionary))
                    return false;

                EvaluationDocument evaluationDocument = new EvaluationDocument();
                evaluationDocument.DocumentID = projectEvaluationVm.NewEvaluationDocument.DocumentID;
                evaluationDocument.Description = projectEvaluationVm.NewEvaluationDocument.Description;
                evaluationDocument.EvaluationID = Convert.ToInt32(projectEvaluationVm.EvaluationID);
                evaluationDocument.DocSource = "V";
                evaluationDocument.Status = "A";
                evaluationDocument.LastUpdate = DateTime.Now;
                evaluationDocument.UserID = user;

                //Repository Method.
                _projectrepository.InsertEvaluationDocument(evaluationDocument);

                //Save.
                _projectrepository.Save();



            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(projectEvaluationVm.ProjectID, exception, user);
                return false;

            }
            return true;
        }

        /// <summary>
        /// Delete an Evaluation document from a project evaluation
        /// </summary>
        /// <param name="DocumentID">6 or 7 digit document ID</param>
        /// <param name="EvaluationID">The unique ID of the evaluation (not sure if a single document could refer to more than one evaluation).</param>
        /// <param name="ProjectID">The project that the evaluation and document refer to (for logging)</param>
        /// <param name="user">user ID (for logging)</param>
        /// <returns></returns>
        public bool DeleteEvaluationDocument(string DocumentID, string EvaluationID, string ProjectID, string user)
        {
            try
            {
                Int32 EvalID;
                if (Int32.TryParse(EvaluationID, out EvalID))
                {
                    _projectrepository.DeleteEvaluationDocument(EvalID, DocumentID);
                    _projectrepository.Save();
                    return true;

                }
                return false;
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool UpdateEvaluation(ProjectEvaluationVM projectEvaluationVm, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                if (!ValidateEvaluationUpdate(projectEvaluationVm, validationDictionary))
                    return false;

                Int32 EvalID;
                if (Int32.TryParse(projectEvaluationVm.EvaluationID, out EvalID))
                {
                    Evaluation evaluationToUpdate = _projectrepository.GetEvaludationById(EvalID);

                    //If the evaluation type is 'none', then only update the evaluation type. Should the rest of the details be blanked if none is selected? What about additional comments? These could be a composite from the ARIES rationalisation? 
                    if (projectEvaluationVm.EvaluationTypes.SelectedEvaluationType != "5")
                    {
                        //Build Dates.
                        DateTime evaluationStartDate = new DateTime(projectEvaluationVm.StartDate_Year.Value,
                            projectEvaluationVm.StartDate_Month.Value, projectEvaluationVm.StartDate_Day.Value);

                        DateTime evaluationEndDate = new DateTime(projectEvaluationVm.EndDate_Year.Value,
                            projectEvaluationVm.EndDate_Month.Value, projectEvaluationVm.EndDate_Day.Value);


                        evaluationToUpdate.StartDate = evaluationStartDate;
                        evaluationToUpdate.EndDate = evaluationEndDate;
                        evaluationToUpdate.EvaluationTypeID = projectEvaluationVm.EvaluationTypes.SelectedEvaluationType;
                        evaluationToUpdate.ManagementOfEvaluation =
                            projectEvaluationVm.EvaluationManagements.SelectedEvaluationManagement;
                        evaluationToUpdate.EstimatedBudget = projectEvaluationVm.EstimatedBudget;
                        evaluationToUpdate.AdditionalInfo = AMPUtilities.CleanText(projectEvaluationVm.AdditionalInfo);
                    }
                    else
                    {
                        evaluationToUpdate.EvaluationTypeID = projectEvaluationVm.EvaluationTypes.SelectedEvaluationType;
                        evaluationToUpdate.StartDate = null;
                        evaluationToUpdate.EndDate = null;
                        evaluationToUpdate.ManagementOfEvaluation = null;
                        evaluationToUpdate.EstimatedBudget = null;

                    }

                    _projectrepository.UpdateProjectEvaluation(evaluationToUpdate);

                    //Update the Evaluation and Save.
                    _projectrepository.Save();
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    _errorengine.LogError(projectEvaluationVm.ProjectID, exception, user);
                    return false;
                }
            }
        }


        /// <summary>
        /// Populate the ProjectDetailsViewModel with ProjectMaster data
        /// </summary>
        /// <param name="ProjectID">ID of the project to return</param>
        /// <returns>A ProjectDetailsViewModel populated with a ProjectMaster object</returns>
        private async Task<ProjectViewModel> PopulateViewModelWithProjectMasterData(String ProjectID)
        {


            ProjectViewModel projectViewModel = new ProjectViewModel();

            ProjectMaster project = _projectrepository.GetProject(ProjectID);

            if (project != null)
            {


                MapProjectMasterToProjectMasterViewModel(projectViewModel, project);



                //Get the project dates
                PopulateViewModelWithProjectDates(projectViewModel, project.ProjectDate);

                DecomposeDateTimesToDateParts(projectViewModel);

                //Populate static project data into the ViewModel
                PopulateViewModelWithProjectStaticData(projectViewModel, project);

                //Populate Core Project Data into the ViewModel
                PopulateViewModelWithProjectCoreData(projectViewModel, project);



                //if (AMPUtilities.ReviewsEnabled())
                //{
                //    //Get the Review data and map it into the ViewModel
                //    returnProjectReviewVm(projectViewModel, project);
                //}



                //await PopulateViewModelWithProjectTeam(projectViewModel, project);

                //BRANCH - Populate the HeaderVM. Will refactor later to populate the HeaderVM more efficiently.
                //HeaderVM header = new HeaderVM();

                //header.ProjectID = projectViewModel.ProjectMaster.ProjectID;
                //header.Stage = projectViewModel.ProjectMaster.Stage;
                //header.Title = projectViewModel.ProjectMaster.Title;
                //header.StageDescription = projectViewModel.ProjectStatic.StageDescription;

            }
            return projectViewModel;
        }

        public async Task<ProjectTeamVM> GetProjectTeam(string ProjectID, string user)
        {
            ProjectMaster project = _projectrepository.GetProject(ProjectID);

            if (project != null)
            {
                ProjectHeaderVM headerVm = ReturnProjectHeaderVm(project, user);
                ProjectTeamVM teamVm = new ProjectTeamVM();
                teamVm.ProjectHeader = headerVm;
                teamVm.TeamMarker = project.ProjectInfo.TeamMarker;

                if (project.Teams != null && project.Teams.Count != 0)
                {
                    //TODO - Remember to remove the TrimStart('R') once the database table changes. It will be redundant.
                    IEnumerable<String> StaffIDs = project.Teams.Select(x => x.TeamID.TrimStart('R'));
                    //Get Employee details from the PersonService
                    IEnumerable<PersonDetails> personDetails = await _personService.GetPeopleDetails(StaffIDs);

                    List<ProjectTeamMemberVM> projectTeamMembersVm =
                        ReturnPopulatedProjectTeamMemberVmList(project.Teams, personDetails);

                    //There are multiple roles in the team and people have held them at various times. Assign the people with the latest start date for each role as the current role holder.

                    IEnumerable<ProjectTeamMemberVM> currentTeam =
                        projectTeamMembersVm.Where(x => x.EndDate == new DateTime(0001, 01, 01));

                    IEnumerable<String> currentRoles = currentTeam.Select(x => x.RoleId).Distinct();


                    foreach (String role in currentRoles)
                    {
                        projectTeamMembersVm.FindAll(x => x.RoleId == role && x.EndDate == new DateTime(0001, 01, 01))
                            .OrderByDescending(y => y.StartDate)
                            .First()
                            .CurrentRoleHolder = true;
                    }

                    //Split the project team into 3 different ViewModels.
                    teamVm.CurrentProjectTeam =
                        projectTeamMembersVm.Where(x => x.CurrentRoleHolder == true && x.RoleId != "TM" && x.IsEmployed == "T").ToList();

                    teamVm.OtherProjectTeam =
                        projectTeamMembersVm.Where(
                            x =>
                            x.EndDate == new DateTime(0001, 01, 01) &&
                                (x.RoleId == "TM" || (x.RoleId != "TM" && !x.CurrentRoleHolder)) &&
                                x.IsEmployed == "T"
                                ).ToList();

                    List<ProjectTeamMemberVM> formerProjectTeam =
                            projectTeamMembersVm.Where(
                                x =>
                                    x.EndDate < DateTime.Now && x.EndDate != new DateTime(0001, 01, 01) &&
                                    x.IsEmployed == "T").ToList();

                    List<ProjectTeamMemberVM> formerDFIDStaff =
                        projectTeamMembersVm.Where(x => x.IsEmployed == "F" || x.IsEmployed == null).ToList();

                    teamVm.FormerProjectTeam = formerProjectTeam.Union(formerDFIDStaff).OrderByDescending(y => y.EndDate).ToList();



                }

                ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

                projectwfvm = await IsProjectinWorkflow(project, user);

                teamVm.WFCheck = projectwfvm;

                //Has the project exceeded the Financial End Date? If it has, the team page can no longer be edited.
                teamVm.ReadOnly = FinancialEndDateReached(project.ProjectDate.FinancialEndDate);

                //Add a blank TeamViewModel onto ProjectViewModel so that a new Team Member can be added.
                teamVm.NewTeamMember = new NewTeamMemberVM();

                teamVm.NewTeamMember.ProjectRolesVm = new ProjectRoleVM();
                teamVm.NewTeamMember.ProjectRolesVm.ProjectRoleValues = ConstructProjectRoleValuesVM();

                //Business rule. If an active SRO exists on the project, remove SRO from the role list that is used in Add Team Member - 30 July 2015 CJF.
                if (RoleExists("SRO", project.Teams))
                {
                    teamVm.NewTeamMember.ProjectRolesVm.ProjectRoleValues =
                        removeRoleFromRoleList(teamVm.NewTeamMember.ProjectRolesVm.ProjectRoleValues.ToList(),
                            "SRO");
                }
                //Business rule. If an active QA exists on the project, remove QA from the role list that is used in Add Team Member - 14 Aug 2015 CJF.
                if (RoleExists("QA", project.Teams))
                {
                    teamVm.NewTeamMember.ProjectRolesVm.ProjectRoleValues =
                        removeRoleFromRoleList(teamVm.NewTeamMember.ProjectRolesVm.ProjectRoleValues.ToList(),
                            "QA");
                }

                teamVm.NewTeamMember.StartDate = DateTime.Today;

                return teamVm;
            }
            else
            {
                return null;
            }

        }

        private bool RoleExists(string role, ICollection<Team> teams)
        {
            Team roleExists = teams.FirstOrDefault(x => x.RoleID == role && x.Status == "A");

            if (roleExists != null)
            {
                return true;
            }

            return false;
        }

        private bool FinancialEndDateReached(DateTime? financialEndDate)
        {
            if (!financialEndDate.HasValue)
            {
                //Think this is the right logic. If the Financial End Date doesn't exist yet, must be early in the project creation cycle.
                return false;
            }
            if (financialEndDate.Value.Date <= DateTime.Now.Date)
            {
                return true;
            }
            return false;
        }

        private List<ProjectTeamMemberVM> ReturnPopulatedProjectTeamMemberVmList(ICollection<Team> teams, IEnumerable<PersonDetails> personDetails)
        {
            List<ProjectTeamMemberVM> projectTeamMembersVm = new List<ProjectTeamMemberVM>();
            foreach (Team team in teams)
            {
                ProjectTeamMemberVM projectTeamMemberVm = new ProjectTeamMemberVM();
                PersonDetails person = personDetails.FirstOrDefault(p => p.EmpNo.Trim() == team.TeamID.TrimStart('R'));
                projectTeamMemberVm = ReturnPopulatedProjectTeamMemberVm(team, person);
                projectTeamMembersVm.Add(projectTeamMemberVm);
            }

            return projectTeamMembersVm;
        }

        private ProjectTeamMemberVM ReturnPopulatedProjectTeamMemberVm(Team team, PersonDetails person)
        {
            ProjectTeamMemberVM teamMember = new ProjectTeamMemberVM();

            // Map the Team Model to the project ViewModel.
            Mapper.CreateMap<PersonDetails, ProjectTeamMemberVM>();
            Mapper.Map<PersonDetails, ProjectTeamMemberVM>(person, teamMember);

            Mapper.CreateMap<Team, ProjectTeamMemberVM>().ForMember(x => x.CurrentRoleHolder, opt => opt.UseValue(false));
            Mapper.Map<Team, ProjectTeamMemberVM>(team, teamMember);

            teamMember.RoleDescription = team.ProjectRole.ProjectRoleDescription;
            teamMember.ProjectID = team.ProjectID;

            return teamMember;
        }

        private List<ProjectRoleValuesVM> ConstructProjectRoleValuesVM()
        {
            List<ProjectRoleValuesVM> projectRoleValues = new List<ProjectRoleValuesVM>();
            IEnumerable<ProjectRole> projectRoles = _projectrepository.GetProjectRoles();

            foreach (ProjectRole role in projectRoles)
            {
                ProjectRoleValuesVM rolesVM = new ProjectRoleValuesVM();
                rolesVM.ProjectRoleID = role.ProjectRoleID;
                rolesVM.ProjectRoleDescription = role.ProjectRoleDescription;
                projectRoleValues.Add(rolesVM);
            }

            return projectRoleValues;
        }


        public async Task<EditTeamMemberVM> GetTeamMember(string id)
        {
            //Start here.

            EditTeamMemberVM editTeamMemberVm = new EditTeamMemberVM();

            int ParsedId;
            bool result = Int32.TryParse(id, out ParsedId);
            if (result)
            {
                Team team = _projectrepository.GetTeamMember(ParsedId);

                if (team != null)
                {

                    editTeamMemberVm.ID = team.ID;
                    editTeamMemberVm.ProjectID = team.ProjectID;
                    editTeamMemberVm.RoleID = team.RoleID;
                    editTeamMemberVm.TeamID = team.TeamID.TrimStart('R');

                    //Get Employee details from the PersonService
                    PersonDetails personDetails = await _personService.GetPersonDetails(editTeamMemberVm.TeamID);

                    //TODO - The TrimStart in this code block will also be redundant.
                    if (personDetails.EmpNo.Trim() == editTeamMemberVm.TeamID)
                    {
                        editTeamMemberVm.DISPLAY_NAME_FORENAME_FIRST = personDetails.DisplayName;
                    }

                    ProjectRoleVM projectRoleVm = new ProjectRoleVM();
                    projectRoleVm.ProjectRoleValues = ConstructProjectRoleValuesVM();

                    editTeamMemberVm.ProjectRolesVm = projectRoleVm;

                    //Business Rule - Should not be able to change someone to an SRO if an SRO already exists. - 20/07/2015 CJF
                    if (_projectrepository.ActiveRoleExists("SRO", team.ProjectID))
                    {
                        editTeamMemberVm.ProjectRolesVm.ProjectRoleValues =
                            removeRoleFromRoleList(editTeamMemberVm.ProjectRolesVm.ProjectRoleValues.ToList(), "SRO");
                        editTeamMemberVm.ProjectRolesVm.SelectedRoleValue = editTeamMemberVm.RoleID;
                    }

                    //Business Rule - Should not be able to change someone to a QA if a QA already exists.  - 14/08/2015 CJF
                    if (_projectrepository.ActiveRoleExists("QA", team.ProjectID))
                    {
                        editTeamMemberVm.ProjectRolesVm.ProjectRoleValues =
                            removeRoleFromRoleList(editTeamMemberVm.ProjectRolesVm.ProjectRoleValues.ToList(), "QA");
                        editTeamMemberVm.ProjectRolesVm.SelectedRoleValue = editTeamMemberVm.RoleID;
                    }


                    return editTeamMemberVm;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private List<ProjectRoleValuesVM> removeRoleFromRoleList(IEnumerable<ProjectRoleValuesVM> projectRolesList, String roleID)
        {
            ProjectRoleValuesVM roleToRemove = projectRolesList.FirstOrDefault(x => x.ProjectRoleID == roleID);
            List<ProjectRoleValuesVM> rolesList = projectRolesList.ToList();
            if (roleToRemove != null)
            {
                rolesList.Remove(roleToRemove);
            }
            return rolesList;
        }

        protected ProjectHeaderVM ReturnProjectHeaderVm(ProjectMaster project, string user)
        {
            ProjectHeaderVM headerVm = new ProjectHeaderVM();

            headerVm.ProjectID = project.ProjectID;
            headerVm.Stage = project.Stage;
            headerVm.Title = project.Title;
            headerVm.StageDescription = project.Stage1.StageDescription;
            headerVm.BudgetCentre = project.BudgetCentreID;
            headerVm.ProjectExistsInPortfolio = IsProjectInPortfolio(project.ProjectID, user);
            headerVm.UserID = user.Trim();
            return headerVm;
        }

        protected bool IsProjectInPortfolio(string projectId, string user)
        {
            IEnumerable<ProjectMaster> projects = _projectrepository.GetProjects(null, user);

            ProjectMaster projectToFind = projects.FirstOrDefault(x => x.ProjectID == projectId);
            if (projectToFind == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        protected static ComponentHeaderVM ReturnComponentHeaderVm(ComponentMaster component)
        {
            ComponentHeaderVM componentHeaderVm = new ComponentHeaderVM();

            componentHeaderVm.ProjectID = component.ProjectID;
            componentHeaderVm.ComponentID = component.ComponentID;
            componentHeaderVm.ComponentDescription = component.ComponentDescription;
            componentHeaderVm.BudgetCentreID = component.BudgetCentreID;
            //componentHeaderVm.InputterID = component.InputterID;
            //componentHeaderVm.InputterName = component.User.UserName;

            return componentHeaderVm;
        }


        //private async Task PopulateViewModelWithProjectTeam(ProjectViewModel projectViewModel, ProjectMaster project)
        //{
        //    if(project.Teams != null && project.Teams.Count != 0)
        //        { 
        //        //Create a new project Team View Model object.
        //        List<ProjectTeamMemberVM> projectTeamVm;

        //        // Map the Team Model to the project ViewModel.
        //        Mapper.CreateMap<Team, ProjectTeamMemberVM>().ForMember(x=>x.CurrentRoleHolder, opt => opt.UseValue(false));

        //        projectTeamVm = Mapper.Map<IEnumerable<Team>,List<ProjectTeamMemberVM>>(project.Teams);

        //        //TODO - Remember to remove the TrimStart('R') once the database table changes. It will be redundant.
        //        IEnumerable<String> StaffIDs = projectTeamVm.Select(x => x.TeamId.TrimStart('R'));

        //        //Get Employee details from the PersonService
        //        IEnumerable<PersonDetails> personDetails = await _personService.GetPeopleDetails(StaffIDs);

        //        foreach (ProjectTeamMemberVM teamMember in projectTeamVm)
        //        {
        //            //TODO - The TrimStart in this code block will also be redundant.
        //            if(personDetails.Any(x => x.EMP_NO.Trim() == teamMember.TeamId.TrimStart('R')))
        //            {
        //                PersonDetails person = personDetails.First(p => p.EMP_NO.Trim() == teamMember.TeamId.TrimStart('R'));
        //                teamMember.EMP_NO = person.EMP_NO;
        //                teamMember.IS_CURRENT = person.IS_CURRENT;
        //                teamMember.TITLE = person.TITLE;
        //                teamMember.FORENAMES = person.FORENAMES;
        //                teamMember.PREFERRED_NAME = person.PREFERRED_NAME;
        //                teamMember.SURNAME = person.SURNAME;
        //                teamMember.DISPLAY_NAME_FORENAME_FIRST = person.DISPLAY_NAME_FORENAME_FIRST;
        //                teamMember.DISPLAY_NAME_SURNAME_FIRST = person.DISPLAY_NAME_SURNAME_FIRST;
        //                teamMember.PHONE_SAME_OFFICE = person.PHONE_SAME_OFFICE;
        //                teamMember.PHONE_GLOBAL = person.PHONE_GLOBAL;
        //                teamMember.FAX_SAME_OFFICE = person.FAX_SAME_OFFICE;
        //                teamMember.FAX_GLOBAL = person.FAX_GLOBAL;
        //                teamMember.MOBILE = person.MOBILE;
        //                teamMember.LOCATION_NAME = person.LOCATION_NAME;
        //                teamMember.ROOM = person.ROOM;
        //                teamMember.EMAIL_INTERNAL = person.EMAIL_INTERNAL;
        //                teamMember.EMAIL_EXTERNAL = person.EMAIL_EXTERNAL;
        //                teamMember.EMAIL = person.EMAIL;
        //                teamMember.LOGON = person.LOGON;
        //                teamMember.IS_JOB_CURRENT = person.IS_JOB_CURRENT;
        //                teamMember.JOB_DESCRIPTION = person.JOB_DESCRIPTION;
        //                teamMember.CURRENT_GRADE_FULL_CODE = person.CURRENT_GRADE_FULL_CODE;
        //                teamMember.MANAGER_EMP_NO = person.MANAGER_EMP_NO;
        //                teamMember.MANAGER_PREFERRED_NAME = person.MANAGER_PREFERRED_NAME;
        //                teamMember.MANAGER_SURNAME = person.MANAGER_SURNAME;
        //                teamMember.MANAGER_DISPLAY_NAME_FN_FIRST = person.MANAGER_DISPLAY_NAME_FN_FIRST;
        //                teamMember.IS_INTERNAL = person.IS_INTERNAL;
        //                teamMember.IS_HOD = person.IS_HOD;
        //                teamMember.RoleDescription = AssignRoleDescription(teamMember.RoleId);
        //                teamMember.ProjectID = project.ProjectID;
        //            }
        //        }

        //        //There are multiple roles in the team and people have held them at various times. Assign the people with the latest start date for each role as the current role holder.

        //        IEnumerable<ProjectTeamMemberVM> currentTeam = projectTeamVm.Where(x => x.EndDate >= DateTime.Now);

        //        IEnumerable<String> currentRoles = currentTeam.Select(x => x.RoleId).Distinct();


        //        foreach (String role in currentRoles)
        //        {
        //            projectTeamVm.FindAll(x => x.RoleId == role && x.EndDate >= DateTime.Now)
        //                .OrderByDescending(y => y.StartDate)
        //                .First()
        //                .CurrentRoleHolder = true;
        //        }

        //        //Split the project team into 3 different ViewModels.
        //        projectViewModel.CurrentProjectTeam = projectTeamVm.Where(x => x.CurrentRoleHolder == true && x.RoleId != "TM").ToList();
        //        projectViewModel.OtherProjectTeam = projectTeamVm.Where(x => x.EndDate >= DateTime.Now && (x.RoleId == "TM" || (x.RoleId != "TM" && !x.CurrentRoleHolder))).ToList();
        //        projectViewModel.FormerProjectTeam = projectTeamVm.Where(x => x.EndDate < DateTime.Now).OrderByDescending(y => y.EndDate).ToList();

        //        //Add a blank TeamViewModel onto ProjectViewModel so that a new Team Member can be added.
        //        projectViewModel.NewTeamMember = new ProjectTeamVM();

        //        List<ProjectRoleValuesVM> projectRoleValues = new List<ProjectRoleValuesVM>();

        //        IEnumerable<ProjectRole> projectRoles = projectrepository.GetProjectRoles();



        //        foreach (ProjectRole role in projectRoles)
        //        {
        //            ProjectRoleValuesVM rolesVM = new ProjectRoleValuesVM();
        //            rolesVM.ProjectRoleID = role.ProjectRoleID;
        //            rolesVM.ProjectRoleDescription = role.ProjectRoleDescription;
        //            projectRoleValues.Add(rolesVM);
        //        }

        //        projectViewModel.NewTeamMember.ProjectRolesVm = new ProjectRoleVM();
        //        projectViewModel.NewTeamMember.ProjectRolesVm.ProjectRoleValues = projectRoleValues;



        //        }
        //}

        private string AssignRoleDescription(string roleID)
        {
            switch (roleID)
            {
                case "SRO":
                    return "Senior Responsible Owner";
                case "QA":
                    return "Quality Assurer";
                case "PI":
                    return "Project Inputter";
                case "LA":
                    return "Lead Adviser";
                case "TM":
                    return "Team Member";
                default:
                    return "Unknown Role";
            }
        }

        //TODO: This is horrible! My first attempt at abstracting this failed. I want to have another go at it later....CJF
        // We might be able to delete this method as the helper class now does something similar. FA
        private static void DecomposeDateTimesToDateParts(ProjectViewModel projectViewModel)
        {
            DateTime date = new DateTime();

            //date = projectViewModel.ProjectDates.ActualStartDate ?? DateTime.Now;
            //projectViewModel.ProjectDates.ActualStartDate_Day = date.Day;
            //projectViewModel.ProjectDates.ActualStartDate_Month = date.Month;
            //projectViewModel.ProjectDates.ActualStartDate_Year = date.Year;

            date = projectViewModel.ProjectDates.Created_date ?? DateTime.Now;
            projectViewModel.ProjectDates.Created_Date_Day = date.Day;
            projectViewModel.ProjectDates.Created_Date_Month = date.Month;
            projectViewModel.ProjectDates.Created_Date_Year = date.Year;

            date = projectViewModel.ProjectDates.FinancialEndDate ?? DateTime.Now;
            projectViewModel.ProjectDates.FinancialEndDate_Day = date.Day;
            projectViewModel.ProjectDates.FinancialEndDate_Month = date.Month;
            projectViewModel.ProjectDates.FinancialEndDate_Year = date.Year;

            date = projectViewModel.ProjectDates.FinancialStartDate ?? DateTime.Now;
            projectViewModel.ProjectDates.FinancialStartDate_Day = date.Day;
            projectViewModel.ProjectDates.FinancialStartDate_Month = date.Month;
            projectViewModel.ProjectDates.FinancialStartDate_Year = date.Year;

            date = projectViewModel.ProjectDates.OperationalEndDate ?? DateTime.Now;
            projectViewModel.ProjectDates.OperationalEndDate_Day = date.Day;
            projectViewModel.ProjectDates.OperationalEndDate_Month = date.Month;
            projectViewModel.ProjectDates.OperationalEndDate_Year = date.Year;

            date = projectViewModel.ProjectDates.OperationalStartDate ?? DateTime.Now;
            projectViewModel.ProjectDates.OperationalStartDate_Day = date.Day;
            projectViewModel.ProjectDates.OperationalStartDate_Month = date.Month;
            projectViewModel.ProjectDates.OperationalStartDate_Year = date.Year;

            date = projectViewModel.ProjectDates.PromptCompletionDate ?? DateTime.Now;
            projectViewModel.ProjectDates.PromptCompletionDate_Day = date.Day;
            projectViewModel.ProjectDates.PromptCompletionDate_Month = date.Month;
            projectViewModel.ProjectDates.PromptCompletionDate_Year = date.Year;
        }

        private static void DecomposeDateTimesToDateParts(ProjectVM projectVm)
        {
            DateTime date = new DateTime();

            //date = projectVm.ProjectDates.ActualStartDate ?? DateTime.Now;
            //projectVm.ProjectDates.ActualStartDate_Day = date.Day;
            //projectVm.ProjectDates.ActualStartDate_Month = date.Month;
            //projectVm.ProjectDates.ActualStartDate_Year = date.Year;

            date = projectVm.ProjectDates.Created_date ?? DateTime.Now;
            projectVm.ProjectDates.Created_Date_Day = date.Day;
            projectVm.ProjectDates.Created_Date_Month = date.Month;
            projectVm.ProjectDates.Created_Date_Year = date.Year;

            date = projectVm.ProjectDates.FinancialEndDate ?? DateTime.Now;
            projectVm.ProjectDates.FinancialEndDate_Day = date.Day;
            projectVm.ProjectDates.FinancialEndDate_Month = date.Month;
            projectVm.ProjectDates.FinancialEndDate_Year = date.Year;

            date = projectVm.ProjectDates.FinancialStartDate ?? DateTime.Now;
            projectVm.ProjectDates.FinancialStartDate_Day = date.Day;
            projectVm.ProjectDates.FinancialStartDate_Month = date.Month;
            projectVm.ProjectDates.FinancialStartDate_Year = date.Year;

            date = projectVm.ProjectDates.OperationalEndDate ?? DateTime.Now;
            projectVm.ProjectDates.OperationalEndDate_Day = date.Day;
            projectVm.ProjectDates.OperationalEndDate_Month = date.Month;
            projectVm.ProjectDates.OperationalEndDate_Year = date.Year;

            date = projectVm.ProjectDates.OperationalStartDate ?? DateTime.Now;
            projectVm.ProjectDates.OperationalStartDate_Day = date.Day;
            projectVm.ProjectDates.OperationalStartDate_Month = date.Month;
            projectVm.ProjectDates.OperationalStartDate_Year = date.Year;

            date = projectVm.ProjectDates.PromptCompletionDate ?? DateTime.Now;
            projectVm.ProjectDates.PromptCompletionDate_Day = date.Day;
            projectVm.ProjectDates.PromptCompletionDate_Month = date.Month;
            projectVm.ProjectDates.PromptCompletionDate_Year = date.Year;
        }
        private async Task<ProjectReviewVM> returnProjectReviewVm(ProjectMaster project, string currentUser)
        {
            ProjectReviewVM projectReviewVm = new ProjectReviewVM();

            List<ReviewVM> reviewsVM = new List<ReviewVM>();

            if (project.ReviewMasters != null)
            {
                //Try Looping round the ReviewMaster objects and create a mapping to a ReviewVM for each?
                DateTime? lastReviewDate = null;
                foreach (ReviewMaster reviewMaster in project.ReviewMasters)
                {
                    //Map the ReviewMaster and ReviewARScore to the ViewModel
                    Mapper.CreateMap<ReviewMaster, ReviewVM>().ForMember(d => d.ReviewOutputs, opt => opt.Ignore()).ForMember(d => d.ReviewDocuments, opt => opt.Ignore());
                    Mapper.CreateMap<ReviewARScore, ReviewVM>().ForMember(d => d.LastUpdated, opt => opt.Ignore());

                    ReviewVM reviewVM = new ReviewVM();
                    List<ReviewOutputVM> reviewOutputsVM = new List<ReviewOutputVM>();

                    ReviewARScore ARScore = project.ReviewARScores.FirstOrDefault(x => x.ProjectID.Equals(reviewMaster.ProjectID) && x.ReviewID.Equals((reviewMaster.ReviewID)));
                    reviewVM = Mapper.Map<ReviewMaster, ReviewVM>(reviewMaster);
                    reviewVM = Mapper.Map<ReviewARScore, ReviewVM>(ARScore, reviewVM);

                    //reviewVM.ReviewDocuments = projectrepository.GetReviewDocuments(reviewMaster.ProjectID, reviewMaster.ReviewID);

                    //process ARs only
                    if (reviewVM != null)
                    {
                        if (reviewVM.StageID == "2") reviewVM.IsApproved = "Y";
                        if (reviewVM.StageID == "3") reviewVM.IsApproved = "N";

                        //Get Employee details from the PersonService                         
                        List<string> users = new List<string>();
                        if (!string.IsNullOrEmpty(reviewVM.Approver)) users.Add(reviewVM.Approver); //Approver's emp_no
                        if (!string.IsNullOrEmpty(reviewVM.Requester))
                            users.Add(reviewVM.Requester); //Requester's emp_no                           
                        IEnumerable<string> empIds = users;
                        IEnumerable<PersonDetails> personDetails;
                        if (users.Count != 0)
                        {

                            personDetails = await _personService.GetPeopleDetails(empIds);
                            PersonDetails approver = new PersonDetails();
                            PersonDetails requester = new PersonDetails();

                            foreach (PersonDetails pd in personDetails)
                            {

                                if ((!string.IsNullOrEmpty(reviewVM.Approver)) &&
                                    pd.EmpNo.Trim() == reviewVM.Approver.Trim())
                                {
                                    approver = pd;
                                    reviewVM.ApproverName = approver.Forename + " " + approver.Surname;
                                }
                                if ((!string.IsNullOrEmpty(reviewVM.Requester)) &&
                                         pd.EmpNo.Trim() == reviewVM.Requester.Trim())
                                {
                                    requester = pd;
                                    reviewVM.RequesterName = requester.Forename + " " + requester.Surname;

                                }
                            }
                        }

                        //set stage name
                        reviewVM.StageName = reviewMaster.ReviewStage == null
                            ? ""
                            : reviewMaster.ReviewStage.StageDescription;

                        //set user group
                        projectReviewVm.CurrentUserMemberOfGroup = await ReturnCurrentUserMemberOfGroup(currentUser, project.ProjectID, reviewVM.Approver, reviewVM.StageID);

                        //Add deferral to reviewVM if there is any
                        //Check if there is any deferral for this review
                        ReviewDeferral reviewDeferral = _projectrepository.GetReviewDeferral(reviewVM.ProjectID,
                            reviewVM.ReviewID);


                        //For AR deferrals add with  reviewsVM
                        if (reviewDeferral != null)
                        {
                            //Set current user approver if currently logged in if stage id 1 (awaiting for approval)
                            if (reviewDeferral.StageID.Trim() == "1")
                            {
                                projectReviewVm.CurrentUserMemberOfGroup =
                                    await
                                        ReturnCurrentUserMemberOfGroup(currentUser, project.ProjectID,
                                            reviewDeferral.Approver.Trim(), reviewDeferral.StageID.Trim());
                            }
                            Mapper.CreateMap<ReviewDeferral, ReviewDeferralVM>();
                            ReviewDeferralVM reviewDeferralVM = new ReviewDeferralVM();
                            reviewDeferralVM = Mapper.Map<ReviewDeferral, ReviewDeferralVM>(reviewDeferral);
                            //Add deferral approver and requester full name to view model 
                            if ((reviewDeferralVM.Approver != null))
                            {
                                PersonDetails approverDetails =
                                    await _personService.GetPersonDetails(reviewDeferralVM.Approver.Trim());
                                if (approverDetails != null)
                                {
                                    reviewDeferralVM.ApproverName = approverDetails.Forename + " " +
                                                                    approverDetails.Surname;
                                }
                            }
                            if (reviewDeferralVM.Requester != null)
                            {
                                PersonDetails requesterDetails =
                                    await _personService.GetPersonDetails(reviewDeferralVM.Requester.Trim());
                                if (requesterDetails != null)
                                {
                                    reviewDeferralVM.RequesterName = requesterDetails.Forename + " " +
                                                                     requesterDetails.Surname;
                                }
                            }
                            //Add deferral with review
                            reviewVM.ReviewDeferralVM = reviewDeferralVM;
                        }



                        //Add review to list
                        reviewsVM.Add(reviewVM);

                        foreach (ReviewOutput reviewOutput in reviewMaster.ReviewOutputs)
                        {
                            Mapper.CreateMap<ReviewOutput, ReviewOutputVM>();
                            ReviewOutputVM reviewOutputVM = new ReviewOutputVM();

                            reviewOutputVM = Mapper.Map<ReviewOutput, ReviewOutputVM>(reviewOutput);
                            reviewOutputsVM.Add(reviewOutputVM);
                        }
                        if (reviewMaster.ReviewType == "Annual Review")
                            lastReviewDate = reviewMaster.ReviewDate;

                        reviewVM.ReviewOutputs = reviewOutputsVM;
                    }//end AR block

                }//end foreach on ReviewMaster

                //BUG: There can be zero ReviewMasters on a new project, so Group Membership is never set and a review cannot be created. If ReviewMaster count is zero, check whether the person is part of the team - 20/10/15 CJF
                //set user group
                if (project.ReviewMasters != null && project.ReviewMasters.Count == 0)
                {
                    projectReviewVm.CurrentUserMemberOfGroup = await ReturnCurrentUserMemberOfGroup(currentUser, project.ProjectID, null, null);
                }

                //Get Exemption Reasons from ReviewExemptions table.
                //projectReviewVm.ExemptionReasons = projectrepository.GetExemptionReasons();

                ////Get Deferral Reasons from DeferralReason table 
                //projectReviewVm.DeferralReasons = projectrepository.GetDeferralReason();

                //Get Exemption Reasons from ReviewExemptions table.
                projectReviewVm.ExemptionReasons = _projectrepository.GetExemptionReasons();

                //Show latest record on top of accordion
                var newList = reviewsVM.OrderByDescending(x => x.ReviewID).ToList();
                projectReviewVm.ProjectReviews = newList;

                #region Load_Exemption_AR_PCR

                ReviewExemption reviewExemptions = _projectrepository.GetReviewExemption(project.ProjectID, "Annual Review");
                ReviewExemption reviewExemptionsPCR = _projectrepository.GetReviewExemption(project.ProjectID, "Project Completion Review");

                if (reviewExemptionsPCR != null)
                {
                    // for PCR
                    Mapper.CreateMap<ReviewExemption, ReviewExemptionVM>();
                    ReviewExemptionVM reviewExemptionVM = new ReviewExemptionVM();
                    reviewExemptionVM = Mapper.Map<ReviewExemption, ReviewExemptionVM>(reviewExemptionsPCR);

                    //projectReviewVm.WorkFlowDescription = await ReturnWorkFlowDescription(project.ProjectID, currentUser);
                    //if (reviewExemptionsPCR.Approver.ToString().Trim() == currentUser.Trim())
                    //    projectReviewVm.CurrentUserMemberOfGroup = "Approver";


                    //if (IsTeamMember(currentUser, project.ProjectID))
                    //{
                    //    projectReviewVm.CurrentUserMemberOfGroup = "Team";
                    //}


                    if (reviewExemptionsPCR.StageID.Trim() == "1")
                    {
                        projectReviewVm.CurrentUserMemberOfGroup =
                            await
                                ReturnCurrentUserMemberOfGroup(currentUser, project.ProjectID,
                                    reviewExemptionsPCR.Approver.Trim(), reviewExemptionsPCR.StageID.Trim());
                    }

                    if ((reviewExemptionVM.Approver != null))
                    {

                        PersonDetails approverDetails =
                            await _personService.GetPersonDetails(reviewExemptionVM.Approver.Trim());
                        if (approverDetails != null)
                        {
                            reviewExemptionVM.ApproverName = approverDetails.Forename + " " +
                                                             approverDetails.Surname;
                        }
                    }
                    if (reviewExemptionVM.Requester != null)
                    {
                        PersonDetails requesterDetails =
                            await _personService.GetPersonDetails(reviewExemptionVM.Requester.Trim());
                        if (requesterDetails != null)
                        {
                            reviewExemptionVM.RequesterName = requesterDetails.Forename + " " +
                                                              requesterDetails.Surname;

                        }
                    }
                    projectReviewVm.ReviewExemptionPCR = reviewExemptionVM;
                }
                else
                {
                    ReviewExemptionVM reviewExemptionPcrVm = new ReviewExemptionVM();
                    projectReviewVm.ReviewExemptionPCR = reviewExemptionPcrVm;
                    reviewExemptionPcrVm.Requester = currentUser;
                    PersonDetails requesterDetails =
                        await _personService.GetPersonDetails(reviewExemptionPcrVm.Requester.Trim());
                    if (requesterDetails != null)
                    {
                        reviewExemptionPcrVm.RequesterName = requesterDetails.Forename + " " +
                                                          requesterDetails.Surname;

                    }
                    //projectReviewVm.CurrentUserMemberOfGroup = await  ReturnCurrentUserMemberOfGroup(currentUser, project.ProjectID,null, null);

                }


                if (reviewExemptions != null)
                {

                    Mapper.CreateMap<ReviewExemption, ReviewExemptionVM>();
                    ReviewExemptionVM reviewExemptionVM = new ReviewExemptionVM();
                    reviewExemptionVM = Mapper.Map<ReviewExemption, ReviewExemptionVM>(reviewExemptions);

                    //projectReviewVm.WorkFlowDescription = await ReturnWorkFlowDescription(project.ProjectID, currentUser);

                    //if (reviewExemptions.Approver.ToString().Trim() == currentUser.Trim())
                    //    projectReviewVm.CurrentUserMemberOfGroup = "Approver";

                    //if (IsTeamMember(currentUser, project.ProjectID))
                    //{
                    //    projectReviewVm.CurrentUserMemberOfGroup = "Team";
                    //}

                    if (reviewExemptions.StageID.Trim() == "1")
                    {
                        projectReviewVm.CurrentUserMemberOfGroup =
                            await
                                ReturnCurrentUserMemberOfGroup(currentUser, project.ProjectID,
                                    reviewExemptions.Approver.Trim(), reviewExemptions.StageID.Trim());
                    }

                    if ((reviewExemptionVM.Approver != null))
                    {
                        PersonDetails approverDetails =
                            await _personService.GetPersonDetails(reviewExemptionVM.Approver.Trim());
                        if (approverDetails != null)
                        {
                            reviewExemptionVM.ApproverName = approverDetails.Forename + " " +
                                                             approverDetails.Surname;
                        }
                    }
                    if (reviewExemptionVM.Requester != null)
                    {
                        PersonDetails requesterDetails =
                            await _personService.GetPersonDetails(reviewExemptionVM.Requester.Trim());
                        if (requesterDetails != null)
                        {
                            reviewExemptionVM.RequesterName = requesterDetails.Forename + " " +
                                                              requesterDetails.Surname;

                        }
                    }
                    projectReviewVm.ReviewExemptionAR = reviewExemptionVM;

                }
                else
                {
                    ReviewExemptionVM reviewExemptionVm = new ReviewExemptionVM();
                    projectReviewVm.ReviewExemptionAR = reviewExemptionVm;
                    reviewExemptionVm.Requester = currentUser;
                    PersonDetails requesterDetails =
                        await _personService.GetPersonDetails(reviewExemptionVm.Requester.Trim());
                    if (requesterDetails != null)
                    {
                        reviewExemptionVm.RequesterName = requesterDetails.Forename + " " +
                                                          requesterDetails.Surname;

                    }

                }

                #endregion 

                ////get pcr
                ReviewPCRScore PCRScore = project.ReviewPCRScores.FirstOrDefault(x => x.ProjectID.Equals(project.ProjectID));

                Mapper.CreateMap<ReviewPCRScore, ReviewPCRScoreVM>();
                ReviewPCRScoreVM PCRScoreVM = new ReviewPCRScoreVM();
                PCRScoreVM = Mapper.Map<ReviewPCRScore, ReviewPCRScoreVM>(PCRScore);

                if (PCRScore != null)
                {
                    PCRScoreVM.RiskScore = PCRScore.ReviewMaster.RiskScore;
                    PCRScoreVM.SubmissionComment = PCRScore.ReviewMaster.SubmissionComment;
                    PCRScoreVM.Approver = PCRScore.ReviewMaster.Approver;
                    PCRScoreVM.Requester = PCRScore.ReviewMaster.Requester;
                    PCRScoreVM.StageID = PCRScore.ReviewMaster.StageID;
                    PCRScoreVM.ApproveComment = PCRScore.ReviewMaster.ApproveComment;
                    PCRScoreVM.ReviewDate = PCRScore.ReviewMaster.ReviewDate;
                    PCRScoreVM.DueDate = PCRScore.ReviewMaster.DueDate;
                    //set stage name
                    PCRScoreVM.StageName = PCRScore.ReviewMaster.ReviewStage == null ? "" : PCRScore.ReviewMaster.ReviewStage.StageDescription;
                    if (PCRScoreVM.StageID == "2") PCRScoreVM.IsApproved = "Y";
                    if (PCRScoreVM.StageID == "3") PCRScoreVM.IsApproved = "N";


                    //Get Employee details from the PersonService                         
                    List<string> users = new List<string>();
                    if (!string.IsNullOrEmpty(PCRScoreVM.Approver)) users.Add(PCRScoreVM.Approver);  //Approver's emp_no
                    if (!string.IsNullOrEmpty(PCRScoreVM.Requester)) users.Add(PCRScoreVM.Requester); //Requester's emp_no                           
                    IEnumerable<string> empIds = users;

                    if (users.Count != 0)
                    {
                        IEnumerable<PersonDetails> personDetails = await _personService.GetPeopleDetails(empIds);
                        PersonDetails approver = new PersonDetails();
                        PersonDetails requester = new PersonDetails();
                        foreach (PersonDetails pd in personDetails)
                        {
                            if ((!string.IsNullOrEmpty(PCRScoreVM.Approver)) && pd.EmpNo.Trim() == PCRScoreVM.Approver.Trim())
                            {
                                approver = pd;
                                PCRScoreVM.ApproverName = approver.Forename + " " + approver.Surname;
                            }
                            if ((!string.IsNullOrEmpty(PCRScoreVM.Requester)) && pd.EmpNo.Trim() == PCRScoreVM.Requester.Trim())
                            {
                                requester = pd;
                                PCRScoreVM.RequesterName = requester.Forename + " " + requester.Surname;
                            }
                        }
                    }


                    //set user group (Default to Others)
                    PCRScoreVM.UserGroup = "Others";
                    projectReviewVm.CurrentUserMemberOfGroup = "Others";

                    projectReviewVm.CurrentUserMemberOfGroup = await ReturnCurrentUserMemberOfGroup(currentUser, project.ProjectID, PCRScoreVM.Approver, PCRScoreVM.StageID);

                    //projectReviewVm.WorkFlowDescription = await ReturnWorkFlowDescription(project.ProjectID, currentUser);

                    projectReviewVm.ProjectPcrScore = PCRScoreVM;

                    //check if PCR has Deferral 
                    ReviewDeferral PCRreviewDeferral = _projectrepository.GetReviewDeferral(PCRScore.ProjectID, PCRScore.ReviewID);

                    if (PCRreviewDeferral != null)
                    {
                        //Set current user approver if currently logged in 

                        //Set current user approver if currently logged in if stage id 1 (awaiting for approval)
                        if (PCRreviewDeferral.StageID.Trim() == "1")
                        {
                            projectReviewVm.CurrentUserMemberOfGroup =
                                await
                                    ReturnCurrentUserMemberOfGroup(currentUser, project.ProjectID,
                                        PCRreviewDeferral.Approver.Trim(), PCRreviewDeferral.StageID.Trim());
                        }



                        Mapper.CreateMap<ReviewDeferral, ReviewDeferralVM>();
                        ReviewDeferralVM reviewDeferralVM = new ReviewDeferralVM();
                        reviewDeferralVM = Mapper.Map<ReviewDeferral, ReviewDeferralVM>(PCRreviewDeferral);
                        //Add deferral approver and requester full name to view model 
                        if ((reviewDeferralVM.Approver != null))
                        {
                            PersonDetails approverDetails =
                                await _personService.GetPersonDetails(reviewDeferralVM.Approver.Trim());
                            if (approverDetails != null)
                            {
                                reviewDeferralVM.ApproverName = approverDetails.Forename + " " +
                                                                approverDetails.Surname;
                            }
                        }
                        if (reviewDeferralVM.Requester != null)
                        {
                            PersonDetails requesterDetails =
                                await _personService.GetPersonDetails(reviewDeferralVM.Requester.Trim());
                            if (requesterDetails != null)
                            {
                                reviewDeferralVM.RequesterName = requesterDetails.Forename + " " +
                                                                 requesterDetails.Surname;
                            }
                        }
                        //Add deferral with PCR
                        projectReviewVm.ProjectPcrScore.ReviewDeferralVM = reviewDeferralVM;
                    }



                }
                //Permormance
                Mapper.CreateMap<Performance, PerformanceVM>();

                PerformanceVM performanceVM = new PerformanceVM();
                performanceVM = Mapper.Map<Performance, PerformanceVM>(project.Performance);
                projectReviewVm.Performance = performanceVM;
            }

            ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

            //Check if the project has an active workflow.
            projectwfvm = await IsProjectinWorkflow(project, currentUser);

            projectReviewVm.ProjectWFCheck = projectwfvm;

            return projectReviewVm;
        }

        private async Task<string> ReturnCurrentUserMemberOfGroup(string currentUser, string ProjectID, string Approver, string reviewApprovalStage)
        {
            //NOTE: Any changes to this code should be replicated in the class WorkflowService, SetUserRole Method. The method is almost identical and has been included as part of the
            //redevelopment of Workflow to be a service. Once project becomes a 'service' there will be one method, probably sitting in a single utility class.

            //Are you the approver?
            if (currentUser == Approver)
                return "Approver";

            //Are you on the Team?

            //Get Current Team
            IEnumerable<Team> currentTeam = _projectrepository.GetTeam(ProjectID);

            if (IsTeamMember(currentUser, currentTeam))
            {
                return "Team";
            }
            else
            {
                return "Others";
            }
        }

        private async Task<bool> IsProjectSROandAlsoHOD(string currentUser, IEnumerable<Team> currentTeam, string approver)
        {

            if (!String.IsNullOrEmpty(approver))
            {
                PersonDetails thispersondetails = await _personService.GetPersonDetails(currentUser.Trim());

                bool isSRO = false || currentTeam.Any(x => x.TeamID.Trim().Equals(currentUser.Trim()) && x.Status.Equals("A") && x.RoleID.Equals("SRO"));

                if (thispersondetails.IsHeadofDepartment.Equals("T") && currentUser.Trim().Equals(approver) && isSRO)
                    return true;
            }

            return false;
        }

        private async Task<bool> IsProjectTeamMemberAlsoHOD(string currentUser, IEnumerable<Team> currentTeam, string approver)
        {

            if (!String.IsNullOrEmpty(approver))
            {
                PersonDetails thispersondetails = await _personService.GetPersonDetails(currentUser.Trim());

                bool isTeamMember = false || currentTeam.Any(x => x.TeamID.Trim().Equals(currentUser.Trim()) && x.Status.Equals("A"));

                if (thispersondetails.IsHeadofDepartment.Equals("T") && currentUser.Trim().Equals(approver) && isTeamMember)
                    return true;
            }

            return false;
        }



        public Tuple<string, string> CreateReviewOutput(ReviewOutputVM reviewOutputVM, string user, IValidationDictionary validationDictionary)
        {
            string overallBand;
            //string aggregatedRisk;
            string projectScore;



            if (!ValidateReviewOutputScore(reviewOutputVM, validationDictionary))
            {
                Tuple<string, string> errorvalueTuple = new Tuple<string, string>("false", "");
                return errorvalueTuple;
            }
            try
            {
                //InsertLog("Create Project Output", username);
                ReviewOutput reviewoutput = new ReviewOutput();
                //resultVm.ProjectOutput.OutputID = projectrepository.GetNextOutputId(resultVm.ProjectOutput.ProjectID);
                reviewOutputVM.LastUpdated = DateTime.Now;
                reviewOutputVM.Status = "A";
                reviewOutputVM.ImpactScore = CalculateImpactScore((int)reviewOutputVM.Weight, reviewOutputVM.OutputScore);
                reviewOutputVM.UserID = string.Format(user);
                reviewOutputVM.OutputID = _projectrepository.GetNextReviewOutputId(reviewOutputVM.ProjectID, reviewOutputVM.ReviewID);
                Mapper.CreateMap<ReviewOutputVM, ReviewOutput>();
                Mapper.Map(reviewOutputVM, reviewoutput);
                _projectrepository.InsertReviewOutput(reviewoutput);
                _projectrepository.Save();

                /*Determine from ReviewMaster table the type of review. For AR, update ReviewARScore table. 
                  For PCR update ReviewPCRScore table*/
                ReviewMaster review = _projectrepository.GetReview(reviewOutputVM.ProjectID, reviewOutputVM.ReviewID);
                string typeOfReview = review.ReviewType;

                IEnumerable<ReviewOutput> reviewoutputs = _projectrepository.GetReviewOutputs(reviewOutputVM.ProjectID, reviewOutputVM.ReviewID);


                IEnumerable<ReviewOutput> reviewOutputs = reviewoutputs as IList<ReviewOutput> ?? reviewoutputs.ToList();
                string band = CalculateaReviewOverallScoreAndGetBand(reviewOutputs);
                //4 Manual risk rating has been introduced
                //string risk = CalculateaReviewAggregatedRisk(reviewOutputs);
                projectScore = CalculateProjectScore(reviewOutputs, reviewOutputVM.ProjectID, reviewOutputVM.ReviewID);

                overallBand = band;
                //aggregatedRisk = risk;

                //Update aggregated Risk score on ReviewMaster table 
                //review.RiskScore = aggregatedRisk;
                _projectrepository.UpdateReview(review);
                _projectrepository.Save();

                //Update OverallScore in the ReviewARScore/ReviewPCRScore table to reflect this new Output
                if (typeOfReview.Equals("Annual Review"))
                {
                    ReviewARScore reviewArScore = _projectrepository.GetReviewARScore(review.ProjectID, review.ReviewID);
                    reviewArScore.OverallScore = band;
                    _projectrepository.UpdateReviewARScore(reviewArScore);
                    _projectrepository.Save();
                }
                else
                {
                    ReviewPCRScore reviewPCRScore = _projectrepository.GetReviewPCRScore(review.ProjectID, review.ReviewID);
                    reviewPCRScore.FinalOutputScore = band;
                    _projectrepository.UpdateReviewPCRScore(reviewPCRScore);
                    _projectrepository.Save();
                }
            }
            catch (Exception ex)
            {
                _errorengine.LogError(ex, user);
                Tuple<string, string> errorvaluesTuple = new Tuple<string, string>(ex.ToString(), "");
                return errorvaluesTuple;

            }


            //Tuple<string, string, string> valuesTuple = new Tuple<string, string, string>(overallBand, aggregatedRisk, projectScore);
            Tuple<string, string> valuesTuple = new Tuple<string, string>(overallBand, projectScore);
            return valuesTuple;
        }

        //Remove single Review Output 
        public Tuple<string, string> RemoveReviewOutput(String projectId, int reviewId, int outputId)
        {
            string overallBand = "";
            const string aggregatedRisk = null;
            string projectScore;
            try
            {
                //Get Portfolio
                ReviewOutput reviewOutput = new ReviewOutput();
                reviewOutput = _projectrepository.GetReviewOutput(projectId, reviewId, outputId);
                _projectrepository.RemoveReviewOutput(reviewOutput);
                _projectrepository.Save();

                IEnumerable<ReviewOutput> reviewoutputs = _projectrepository.GetReviewOutputs(projectId, reviewId);


                //Update OverallScore in the ReviewARScore/ReviewPCRScore table to reflect this new Output
                string band = CalculateaReviewOverallScoreAndGetBand(reviewoutputs);
                //string risk = CalculateaReviewAggregatedRisk(reviewoutputs);
                projectScore = CalculateProjectScore(reviewoutputs, projectId, reviewId);

                overallBand = band;
                //aggregatedRisk = risk;

                ReviewMaster review = _projectrepository.GetReview(projectId, reviewId);

                //Update aggregated Risk score on ReviewMaster table 
                review.RiskScore = aggregatedRisk;
                _projectrepository.UpdateReview(review);
                _projectrepository.Save();

                /*Determine from ReviewMaster table the type of review. For AR, update ReviewARScore table. 
                For PCR update ReviewPCRScore table*/
                string typeOfReview = review.ReviewType;
                if (typeOfReview.Equals("Annual Review"))
                {
                    ReviewARScore reviewArScore = _projectrepository.GetReviewARScore(review.ProjectID, review.ReviewID);
                    reviewArScore.OverallScore = band;
                    _projectrepository.UpdateReviewARScore(reviewArScore);
                    _projectrepository.Save();
                }
                else
                {
                    ReviewPCRScore reviewPCRScore = _projectrepository.GetReviewPCRScore(review.ProjectID, review.ReviewID);
                    reviewPCRScore.FinalOutputScore = band;
                    _projectrepository.UpdateReviewPCRScore(reviewPCRScore);
                    _projectrepository.Save();
                }

                Tuple<string, string> valuesTuple = new Tuple<string, string>(overallBand, projectScore);
                return valuesTuple;

            }
            catch (Exception ex)
            {
                // errorengine.LogError(ex, user);
                Tuple<string, string> errorvaluesTuple = new Tuple<string, string>(ex.ToString(), "");
                return errorvaluesTuple;
            }
        }


        /// <summary>
        /// Insert overallrisk Score
        /// </summary>
        /// <returns>OverallRisk</returns>
        public string InsertOverallRisk(string user, string projectId, int reviewId, string overallRisk)
        {
            ReviewMaster reviewMaster = new ReviewMaster();
            reviewMaster = _projectrepository.GetReview(projectId, reviewId);
            reviewMaster.RiskScore = overallRisk;
            _projectrepository.UpdateReview(reviewMaster);
            _projectrepository.Save();

            //Risk code - Minor 	R1, Moderate	R2,Major	R3,Severe	R4
            switch (overallRisk)
            {
                case "R1":
                    overallRisk = "Minor";
                    break;

                case "R2":
                    overallRisk = "Moderate";
                    break;

                case "R3":
                    overallRisk = "Major";
                    break;

                case "R4":
                    overallRisk = "Severe";
                    break;
                default:
                    break;
            }

            return overallRisk;
        }

        /// <summary>
        /// Update review score
        /// </summary>
        /// <returns>True or False</returns>
        public Tuple<string, string, string, int?> EditReviewOutput(ReviewOutput reviewOutput, string user)
        {
            string overallBand = "";
            const string aggregatedRisk = null;
            string projectScore = "";

            int? sum = 0;

            int? totalSum = _projectrepository.GetReviewOutputsWeightSum(reviewOutput.ProjectID, reviewOutput.ReviewID, reviewOutput.OutputID);

            if (totalSum != null)
            {
                sum = reviewOutput.Weight + totalSum;
            }
            else
            {
                sum = reviewOutput.Weight;
            }

            if (sum <= 100)
            {
                try
                {
                    ReviewOutput existingReviewOutput = _projectrepository.GetReviewOutput(reviewOutput.ProjectID, reviewOutput.ReviewID, reviewOutput.OutputID);
                    existingReviewOutput.OutputDescription = reviewOutput.OutputDescription;
                    existingReviewOutput.Weight = reviewOutput.Weight;
                    existingReviewOutput.OutputScore = reviewOutput.OutputScore;
                    //recalculate impact score
                    existingReviewOutput.ImpactScore = CalculateImpactScore((int)reviewOutput.Weight, reviewOutput.OutputScore);
                    existingReviewOutput.Risk = reviewOutput.Risk;
                    existingReviewOutput.UserID = user;
                    existingReviewOutput.LastUpdated = DateTime.Now;
                    _projectrepository.EditReviewOutput(existingReviewOutput);
                    _projectrepository.Save();

                    IEnumerable<ReviewOutput> reviewoutputs = _projectrepository.GetReviewOutputs(reviewOutput.ProjectID, reviewOutput.ReviewID);


                    //Update OverallScore in the ReviewARScore/ReviewPCRScore table to reflect this new Output
                    string band = CalculateaReviewOverallScoreAndGetBand(reviewoutputs);
                    //string risk = CalculateaReviewAggregatedRisk(reviewoutputs);
                    projectScore = CalculateProjectScore(reviewoutputs, reviewOutput.ProjectID, reviewOutput.ReviewID);


                    overallBand = band;
                    //aggregatedRisk = risk;
                    int? TotalWeight = sum;

                    ReviewMaster review = _projectrepository.GetReview(reviewOutput.ProjectID, reviewOutput.ReviewID);

                    //Update aggregated Risk score on ReviewMaster table 
                    review.RiskScore = aggregatedRisk;
                    _projectrepository.UpdateReview(review);
                    _projectrepository.Save();

                    /*Determine from ReviewMaster table the type of review. For AR, update ReviewARScore table. 
                      For PCR update ReviewPCRScore table*/
                    string typeOfReview = review.ReviewType;
                    if (typeOfReview.Equals("Annual Review"))
                    {
                        ReviewARScore reviewArScore = _projectrepository.GetReviewARScore(review.ProjectID, review.ReviewID);
                        reviewArScore.OverallScore = band;
                        _projectrepository.UpdateReviewARScore(reviewArScore);
                        _projectrepository.Save();
                    }
                    else
                    {
                        ReviewPCRScore reviewPCRScore = _projectrepository.GetReviewPCRScore(review.ProjectID, review.ReviewID);
                        reviewPCRScore.FinalOutputScore = band;
                        _projectrepository.UpdateReviewPCRScore(reviewPCRScore);
                        _projectrepository.Save();
                    }


                    Tuple<string, string, string, int?> valuesTuple = new Tuple<string, string, string, int?>(overallBand, aggregatedRisk, projectScore, TotalWeight);
                    return valuesTuple;

                }
                catch (Exception ex)
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    _errorengine.LogError(reviewOutput.ProjectID, ex, user);
                    Tuple<string, string, string, int?> errorvaluesTuple = new Tuple<string, string, string, int?>(ex.ToString(), "", "", 0);
                    return errorvaluesTuple;
                }
            }
            else
            {
                Tuple<string, string, string, int?> valuesTuple = new Tuple<string, string, string, int?>("", "", "", sum);
                return valuesTuple;
            }

        }

        /// <summary>
        /// Add a new review document. The document is an ID (used in the View to generate a hypelink to the Document management system) and a description. 
        /// </summary>
        /// <param name="user">User ID for logging</param>
        /// <returns></returns>
        public Tuple<string> AddReviewDocument(ReviewDocumentVM reviewDocumentVM, IValidationDictionary validationDictionary, string user)
        {
            try
            {

                ReviewDocument reviewDocument = new ReviewDocument();
                reviewDocument.DocumentID = reviewDocumentVM.DocumentID;
                reviewDocument.Description = reviewDocumentVM.Description;
                reviewDocument.DocSource = "V";
                reviewDocument.ProjectID = reviewDocumentVM.ProjectID;
                reviewDocument.ReviewID = reviewDocumentVM.ReviewID;
                reviewDocument.LastUpdate = DateTime.Now;
                reviewDocument.UserID = user;
                //Repository Method.
                _projectrepository.InsertReviewDocument(reviewDocument);
                //Save.
                _projectrepository.Save();
            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(reviewDocumentVM.ProjectID, exception, user);
                Tuple<string> failedvaluesTuple = new Tuple<string>("Failed");
                return failedvaluesTuple;
            }
            Tuple<string> valuesTuple = new Tuple<string>("Success");
            return valuesTuple;
        }


        ///// <summary>
        ///// Delete a review document from a project evaluation
        ///// </summary>
        ///// <param name="DocumentID">6 or 7 digit document ID</param>
        ///// <param name="EvaluationID">The unique ID of the evaluation (not sure if a single document could refer to more than one evaluation).</param>
        ///// <param name="ProjectID">The project that the evaluation and document refer to (for logging)</param>
        ///// <param name="user">user ID (for logging)</param>
        ///// <returns></returns>
        public Tuple<string> DeleteReviewDocument(int docId, string user)
        {
            try
            {
                _projectrepository.DeleteReviewDocument(docId);
                _projectrepository.Save();
                Tuple<string> valuesTuple = new Tuple<string>("Success");
                return valuesTuple;


            }
            catch (Exception exception)
            {
                Tuple<string> failedvaluesTuple = new Tuple<string>("Failed");
                return failedvaluesTuple;
            }
        }

        private static void PopulateViewModelWithProjectCoreData(ProjectViewModel projectViewModel, ProjectMaster project)
        {
            ProjectCoreVM projectCoreVM = new ProjectCoreVM();

            if (project.ProjectInfo != null)
            {

                //Define a Mapping between ProjectInfo Model and the ProjectCoreVM
                Mapper.CreateMap<ProjectInfo, ProjectCoreVM>();

                //Do the mapping
                Mapper.Map(project.ProjectInfo, projectCoreVM);

                //Calculate the current Operational Status of the Project
                //TODO - Business Logic that calculates the Operational Status. For Now, set it to Active.
                projectCoreVM.OpStatus = "Active";

            }

            //Append the ProjectCoreVM to the ViewModel
            projectViewModel.ProjectCore = projectCoreVM;

        }

        private static void PopulateViewModelWithProjectStaticData(ProjectViewModel projectViewModel, ProjectMaster project)
        {
            ProjectStaticDataVM staticProjectData = new ProjectStaticDataVM();

            if (project.BudgetCentre != null)
            {
                staticProjectData.BudgetCentreDescription = project.BudgetCentre.BudgetCentreDescription;
            }

            if (project.Stage1 != null)
            {
                staticProjectData.StageDescription = project.Stage1.StageDescription;
            }

            projectViewModel.ProjectStatic = staticProjectData;
        }


        private static CCOValuesVM ReturnCCOValuesVM(string description, string value)
        {
            CCOValuesVM ccoValuesVm = new CCOValuesVM();
            ccoValuesVm.CCODescription = description;
            ccoValuesVm.CCOValue = value;
            return ccoValuesVm;
        }

        private static void PopulateViewModelWithProjectDates(ProjectViewModel projectViewModel, ProjectDate projectDate)
        {
            ProjectDateVM projectDateVM = new ProjectDateVM();

            if (projectDate != null)
            {
                //Define a mapping between the ProjectDate data object and the ProjectDateVM ViewModel.
                Mapper.CreateMap<ProjectDate, ProjectDateVM>();


                //Do the mapping.
                Mapper.Map(projectDate, projectDateVM);

            }
            //Append projectDateVM to the ProjectViewModel
            projectViewModel.ProjectDates = projectDateVM;
        }

        private static void MapProjectMasterToProjectMasterViewModel(ProjectViewModel projectViewModel, ProjectMaster project)
        {
            ProjectMasterVM projectMasterVm = new ProjectMasterVM();

            //Define a map between the ProjectMaster object and the ProjectMasterVM
            Mapper.CreateMap<ProjectMaster, ProjectMasterVM>();

            //Map the objects
            Mapper.Map(project, projectMasterVm);

            projectViewModel.ProjectMaster = projectMasterVm;
        }

        public async Task<Tuple<bool, string>> CreateProject(ProjectVM projectvm, IValidationDictionary validationDictionary, string user)
        {
            try
            {

                //New project object
                ProjectMaster projectMaster = new ProjectMaster();

                // Build and Validate dates
                //Take the day, month and year for each date and put them back together in the ViewModel.
                projectvm.ProjectDates.Created_date = DateTime.Now;
                projectvm.ProjectDates.FinancialStartDate = DateTime.Now;
                if (!IsValidDate(projectvm.ProjectDates.OperationalStartDate_Day, projectvm.ProjectDates.OperationalStartDate_Month, projectvm.ProjectDates.OperationalStartDate_Year, "ProjectDates.OperationalStartDate", validationDictionary))
                    return new Tuple<bool, string>(validationDictionary.IsValid, "");
                if (!IsValidDate(projectvm.ProjectDates.OperationalEndDate_Day, projectvm.ProjectDates.OperationalEndDate_Month, projectvm.ProjectDates.OperationalEndDate_Year, "ProjectDates.OperationalEndDate", validationDictionary))
                    return new Tuple<bool, string>(validationDictionary.IsValid, "");
                projectvm.ProjectDates.OperationalStartDate = new DateTime(projectvm.ProjectDates.OperationalStartDate_Year, projectvm.ProjectDates.OperationalStartDate_Month, projectvm.ProjectDates.OperationalStartDate_Day);
                projectvm.ProjectDates.OperationalEndDate = new DateTime(projectvm.ProjectDates.OperationalEndDate_Year, projectvm.ProjectDates.OperationalEndDate_Month, projectvm.ProjectDates.OperationalEndDate_Day);
                projectvm.ProjectDates.FinancialEndDate = new DateTime(projectvm.ProjectDates.OperationalEndDate_Year, projectvm.ProjectDates.OperationalEndDate_Month, projectvm.ProjectDates.OperationalEndDate_Day).AddMonths(6);
                projectvm.ProjectDates.PromptCompletionDate = new DateTime(projectvm.ProjectDates.OperationalEndDate_Year, projectvm.ProjectDates.OperationalEndDate_Month, projectvm.ProjectDates.OperationalEndDate_Day).AddMonths(-3);
                projectvm.ProjectDates.ActualEndDate = null;

                if (!ValidateCreateProject(projectvm, validationDictionary))
                    return new Tuple<bool, string>(false, "");

                //Validate that the day, month and year are valid values. 
                if (!ProjectDatesAreValid(projectvm, validationDictionary))
                    return new Tuple<bool, string>(false, "");

                //Map in the data from the view model
                Mapper.CreateMap<ProjectVM, ProjectMaster>();

                Mapper.Map(projectvm, projectMaster);

                projectMaster.Title = AMPUtilities.CleanText(projectMaster.Title);
                projectMaster.Description = AMPUtilities.CleanText(projectMaster.Description);

                projectMaster.UserID = user;
                projectMaster.Status = "A";
                projectMaster.Stage = "0";
                projectMaster.LastUpdate = DateTime.Now;

                //Get the next ProjectID and append it to the project record to be saved.
                projectMaster.ProjectID = _projectrepository.NextProjectID();
                projectvm.ProjectID = projectMaster.ProjectID;

                //Create date model
                ProjectDate projectdates = new ProjectDate();

                projectdates.Created_date = DateTime.Now;
                projectdates.ProjectID = projectMaster.ProjectID;
                projectdates.OperationalStartDate = projectvm.ProjectDates.OperationalStartDate;
                projectdates.OperationalEndDate = projectvm.ProjectDates.OperationalEndDate;
                projectdates.FinancialStartDate = projectvm.ProjectDates.FinancialStartDate;
                projectdates.FinancialEndDate = projectvm.ProjectDates.FinancialEndDate;
                projectdates.PromptCompletionDate = projectvm.ProjectDates.PromptCompletionDate;
                projectdates.UserID = user;
                projectdates.LastUpdate = DateTime.Now;

                projectMaster.ProjectDate = projectdates;

                //Project info
                ProjectInfo projectinfo = new ProjectInfo();
                projectinfo.ProjectID = projectMaster.ProjectID;
                projectinfo.TeamMarker = "";
                projectinfo.UserID = user;
                projectinfo.LastUpdate = DateTime.Now;

                projectMaster.ProjectInfo = projectinfo;

                //Insert the person who is creating this project as a team member
                Team teammember = new Team();

                teammember.TeamID = user;
                teammember.ProjectID = projectMaster.ProjectID;
                teammember.Status = "A";
                teammember.RoleID = "PI";
                teammember.ProjectID = projectvm.ProjectID;
                teammember.StartDate = DateTime.Now;
                teammember.EndDate = null;
                teammember.LastUpdated = DateTime.Now;
                teammember.UserID = user;

                List<Team> teams = new List<Team>();
                teams.Add(teammember);

                projectMaster.Teams = teams;

                //Empty Evaluation
                Evaluation evaluation = new Evaluation();
                evaluation.ProjectID = projectMaster.ProjectID;
                evaluation.EvaluationID = 0;
                evaluation.EvaluationTypeID = "5";
                evaluation.ManagementOfEvaluation = "5";
                evaluation.UserID = user;
                evaluation.LastUpdated = DateTime.Now;
                //Does type need to be set to none as default?

                List<Evaluation> evaluations = new List<Evaluation>();

                evaluations.Add(evaluation);

                projectMaster.Evaluations = evaluations;

                //Performance data
                Performance performance = new Performance();
                performance.ProjectID = projectMaster.ProjectID;
                performance.UserID = user;
                performance.LastUpdated = DateTime.Now;
                performance.PCRRequired = "No";
                performance.ARRequired = "No";

                projectMaster.Performance = performance;


                // Create Markers Data (empty unless admin projects which have climate markers set to Not Targeted)
                Markers1 markers = new Markers1();
                markers.GenderEquality = null;
                markers.HIVAIDS = null;
                markers.Biodiversity = null;
                markers.Mitigation = null;
                markers.Adaptation = null;
                markers.Desertification = null;
                markers.Disability = null;
                markers.DisabilityPercentage = null;
                markers.Status = "A";
                markers.LastUpdated = DateTime.Now;
                markers.UserID = user;

                projectMaster.Markers1 = markers;


                //Insert and save
                _projectrepository.InsertProject(projectMaster);
                _projectrepository.Save();

                if (_Configuration.CreateProjectFolderInVault == "true")
                {
                    try
                    {
                        //PersonDetails personDetails = await _personService.GetPersonDetails(user);

                        //if (personDetails == null)
                        //{
                        //    throw new NullReferenceException("No personDetails returned");
                        //}

                        //Call Vault and create a project folder.
                        Task<string> vaultResult = _edrmService.CreateProjectFolder(projectMaster.Title, projectMaster.BudgetCentreID, projectMaster.ProjectID);
                        _loggingengine.InsertLog("Create project. VaultAPI returned " + vaultResult.Result, user, projectMaster.ProjectID);
                        return new Tuple<bool, string>(true, vaultResult.Result);
                    }
                    catch (HttpRequestException exception)
                    {
                        _errorengine.LogError(exception, "Create Vault Folder. ProjectID " + projectMaster.ProjectID + ": " + exception.Message, user);
                        return new Tuple<bool, string>(true, "0");
                    }

                }

                return new Tuple<bool, string>(true, "");
            }
            catch (Exception Ex)
            {
                _errorengine.LogError("NewProject", Ex, user);
                throw;
            }



        }

        public string CreateReview(ProjectReviewVM projectReviewVm, string username)
        {
            ReviewMaster reviewmaster = new ReviewMaster();

            try
            {

                projectReviewVm.ReviewMaster.ReviewID =
                    _projectrepository.GetNextReviewID(projectReviewVm.ReviewMaster.ProjectID);
                projectReviewVm.ReviewMaster.ReviewDate = new DateTime(projectReviewVm.ReviewMaster.ReviewDate_Year,
                    projectReviewVm.ReviewMaster.ReviewDate_Month, projectReviewVm.ReviewMaster.ReviewDate_Day);
                projectReviewVm.ReviewMaster.Status = "A";
                projectReviewVm.ReviewMaster.UserID = username;
                //projectReviewVm.ReviewMaster.RiskScore = "-";
                projectReviewVm.ReviewMaster.Approved = "0"; //Just Submitted not approved yet
                projectReviewVm.ReviewMaster.LastUpdated = DateTime.Now;


                //Save in Review Master table 
                Mapper.CreateMap<ReviewMasterVM, ReviewMaster>();
                Mapper.Map(projectReviewVm.ReviewMaster, reviewmaster);
                reviewmaster.StageID = "0";

                //Get review due date to save historical review due date. 
                Performance performance = AdminGetPerformance(projectReviewVm.ReviewMaster.ProjectID);
                DateTime? dateTime = projectReviewVm.ReviewMaster.ReviewType == "Annual Review"
                    ? performance.ARDueDate
                    : performance.PCRDueDate;
                if (dateTime != null)
                {
                    DateTime duedate = (DateTime)dateTime;
                    reviewmaster.DueDate = duedate;
                }


                _projectrepository.InsertReview(reviewmaster);
                _projectrepository.Save();


                //Save in reviewARScore or reviewPCRScore based on Dropdown value 

                if (projectReviewVm.ReviewMaster.ReviewType == "Annual Review")
                {
                    ReviewARScore reviewArScore = new ReviewARScore();
                    Mapper.CreateMap<ReviewMasterVM, ReviewARScore>();
                    Mapper.Map(projectReviewVm.ReviewMaster, reviewArScore);
                    _projectrepository.InsertReviewARScore(reviewArScore);
                    _projectrepository.Save();
                }

                else if (projectReviewVm.ReviewMaster.ReviewType == "Project Completion Review")
                {

                    ReviewPCRScore reviewPcrScore = new ReviewPCRScore();
                    Mapper.CreateMap<ReviewMasterVM, ReviewPCRScore>();
                    Mapper.Map(projectReviewVm.ReviewMaster, reviewPcrScore);
                    _projectrepository.InsertReviewPCRScore(reviewPcrScore);
                    _projectrepository.Save();
                }

            }


            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(projectReviewVm.ReviewMaster.ProjectID, ex, username);
                return "Failed";
            }

            return "Success";

        }

        //Insert Deferral into ReviewDeferral table
        public async Task<bool> RequestReviewDeferral(ReviewDeferralVM reviewDeferralVm, string reviewType, string user, string reviewsURL)
        {
            ReviewDeferral reviewdeferral = new ReviewDeferral();

            try
            {
                reviewDeferralVm.Requester = user;
                reviewDeferralVm.StageID = "1"; //Awaiting approval

                reviewDeferralVm.LastUpdated = DateTime.Now;
                reviewDeferralVm.UpdatedBy = user;

                //Mapper.CreateMap(reviewDeferralVm, reviewdeferral); //Changed to test U.Test

                reviewDeferralVm.DeferralJustification = AMPUtilities.CleanText(reviewDeferralVm.DeferralJustification);

                Mapper.CreateMap<ReviewDeferralVM, ReviewDeferral>();
                Mapper.Map<ReviewDeferralVM, ReviewDeferral>(reviewDeferralVm, reviewdeferral);


                //update review deferral if already exists for this review 
                ReviewDeferral reviewDeferralExisting = _projectrepository.GetReviewDeferral(reviewDeferralVm.ProjectID, reviewDeferralVm.ReviewID);
                //Get due date from performance table to show in the email body.
                Performance performance = _projectrepository.GetProjectPerformance(reviewDeferralVm.ProjectID);
                string reviewDueDate;



                if (reviewDeferralExisting != null)
                //There is already a review deferral exists in the database for this project review. So update the existing.
                {
                    var config = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);

                    config.CreateMap<ReviewDeferralVM, ReviewDeferral>()
                        .ForMember(d => d.DeferralID, opt => opt.Ignore())
                        .ForMember(d => d.ProjectID, opt => opt.Ignore())
                        .ForMember(d => d.ReviewID, opt => opt.Ignore());

                    var engine = new MappingEngine(config);

                    engine.Map(reviewDeferralVm, reviewDeferralExisting);

                    if (reviewType.Trim() == "Annual Review")
                    {
                        reviewDeferralExisting.PreviousReviewDate = performance.ARDueDate;
                    }
                    else
                    {
                        reviewDeferralExisting.PreviousReviewDate = performance.PCRDueDate;
                    }
                    _projectrepository.UpdateReviewDeferral(reviewDeferralExisting);
                }
                //Else Insert the deferral for this review 
                else
                {
                    if (reviewType.Trim() == "Annual Review")
                    {
                        reviewdeferral.PreviousReviewDate = performance.ARDueDate;
                    }
                    else
                    {
                        reviewdeferral.PreviousReviewDate = performance.PCRDueDate;
                    }
                    _projectrepository.InsertReviewDeferral(reviewdeferral);
                }
                //projectrepository.UpdateARPCRDueDate(PerformanceToUpdate);


                if (reviewType.Trim() == "Annual Review")
                {
                    reviewDueDate = performance.ARDueDate != null ? performance.ARDueDate.Value.ToShortDateString() : "n/a";
                }
                else
                {
                    reviewDueDate = performance.PCRDueDate != null ? performance.PCRDueDate.Value.ToShortDateString() : "n/a";
                }


                _projectrepository.Save();
                //Send Authorization email for existing deferral 
                if (reviewDeferralExisting != null)
                {
                    await SendDeferralEmails(reviewDeferralExisting, reviewDueDate, user, reviewsURL, 1); // 1 for awaiting approval stage 
                }
                else
                {
                    await SendDeferralEmails(reviewdeferral, reviewDueDate, user, reviewsURL, 1); //Awaiting Approval 
                }

                //Need to change this to a boolean
                return true;
            }

            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(reviewDeferralVm.ProjectID, ex, user);
                return false;
            }
        }

        //Delete Deferal from ReviewDeferral table
        public async Task<bool> DeleteDeferral(ReviewDeferralVM reviewDeferralVm, string user)
        {

            ReviewDeferral reviewDeferral = new ReviewDeferral();

            //Mapper.Map(reviewDeferralVm, reviewDeferral);
            //Ammend the code for Mapping 
            Mapper.CreateMap<ReviewDeferralVM, ReviewDeferral>();
            Mapper.Map<ReviewDeferralVM, ReviewDeferral>(reviewDeferralVm, reviewDeferral);



            ReviewDeferral existingDeferral = new ReviewDeferral();
            existingDeferral = _projectrepository.GetReviewDeferral(reviewDeferral.ProjectID, reviewDeferral.ReviewID);

            try
            {
                if (string.IsNullOrEmpty(existingDeferral.Approved)) // Check if already been approved or rejected or not. 
                {
                    _projectrepository.DeleteReviewDeferral(reviewDeferral.DeferralID);
                    _projectrepository.Save();
                    await SendDeferralEmails(reviewDeferral, string.Empty, user, string.Empty, 0);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(reviewDeferral.ProjectID, ex, user);
                return false;
            }
        }

        public async Task<bool> DeleteExemption(ReviewExemptionVM ReviewExemptionVm, string user)
        {
            ReviewExemption reviewExemption = _projectrepository.GetReviewExemption(ReviewExemptionVm.ProjectID.ToString(), ReviewExemptionVm.ExemptionType.ToString().Trim());
            if (reviewExemption != null)
            {
                try
                {
                    //Send Exemption Email
                    await SendExemptionEmail(reviewExemption, string.Empty, user, string.Empty, 0);

                    _projectrepository.DeleteReviewExemption(reviewExemption);
                    _projectrepository.Save();


                    return true;
                }
                catch (Exception ex)
                {
                    _errorengine.LogError(ReviewExemptionVm.ProjectID, ex, user);
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> ApproverExemptionAction(ReviewExemptionVM reviewExemptionVm, string user, string reviewsURL)
        {
            try
            {
                //Get the existing Review Exemption record for the project
                ReviewExemption reviewExemption = _projectrepository.GetReviewExemption(reviewExemptionVm.ProjectID, reviewExemptionVm.ExemptionType);
                Performance performance = _projectrepository.GetProjectPerformance(reviewExemptionVm.ProjectID);

                string reviewDuedate;

                if (reviewExemption.ExemptionType.Trim().ToUpper() == "ANNUAL REVIEW")
                {

                    reviewDuedate = performance.ARDueDate != null ? performance.ARDueDate.Value.ToShortDateString() : "n/a";
                }
                else
                {
                    reviewDuedate = performance.PCRDueDate != null ? performance.PCRDueDate.Value.ToShortDateString() : "n/a";
                }
                //Update the Exemption with the decision
                reviewExemption.ApproverComment = reviewExemptionVm.ApproverComment;
                if (reviewExemptionVm.Approved == "1") // 1 = Y
                {
                    reviewExemption.Approved = "1";
                    reviewExemption.StageID = "2";

                    UpdatePerformanceForExemption(reviewExemption, reviewExemption.ExemptionType, user); //if approved then update performance
                }
                else
                {
                    reviewExemption.Approved = "0";
                    reviewExemption.StageID = "3";
                }

                reviewExemption.LastUpdated = DateTime.Now;
                reviewExemption.UpdatedBy = user;

                try
                {
                    _projectrepository.UpdateReviewExemption(reviewExemption);
                    _projectrepository.Save();
                    //Send Authorizarion Email
                    await SendExemptionEmail(reviewExemption, reviewDuedate, user, reviewsURL, Convert.ToInt16(reviewExemption.StageID));
                }
                catch (Exception ex)
                {
                    _errorengine.LogError(reviewExemptionVm.ProjectID, ex, user);
                    return false;
                }


                return true;

            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(reviewExemptionVm.ProjectID, exception, user);
                return false;
            }
        }
        public async Task<bool> RequestReviewExemption(ReviewExemptionVM reviewExemptionVm, string user, string reviewsURL)
        {
            try
            {
                ReviewExemption reviewExemptionExisting = _projectrepository.GetReviewExemption(reviewExemptionVm.ProjectID.ToString(), reviewExemptionVm.ExemptionType);
                if (reviewExemptionExisting != null)
                {
                    Performance performance = _projectrepository.GetProjectPerformance(reviewExemptionVm.ProjectID);

                    string reviewDuedate;

                    if (reviewExemptionVm.ExemptionType.Trim().ToUpper() == "ANNUAL REVIEW")
                    {
                        reviewDuedate = performance.ARDueDate != null ? performance.ARDueDate.Value.ToShortDateString() : "n/a";
                    }
                    else
                    {
                        reviewDuedate = performance.PCRDueDate != null ? performance.PCRDueDate.Value.ToShortDateString() : "n/a";
                    }

                    reviewExemptionVm.Requester = user;
                    reviewExemptionVm.StageID = "1"; //Awaiting approval

                    reviewExemptionVm.LastUpdated = DateTime.Now;
                    reviewExemptionVm.UpdatedBy = user;
                    //
                    await SendExemptionEmail(reviewExemptionExisting, reviewDuedate, user, reviewsURL, 4); //Send Email to the old approver 

                    var config = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);
                    config.CreateMap<ReviewExemptionVM, ReviewExemption>()
                        .ForMember(d => d.ID, opt => opt.Ignore())
                        .ForMember(d => d.ProjectID, opt => opt.Ignore())
                        .ForMember(d => d.SubmissionComment, opt => opt.Ignore())
                        .ForMember(d => d.ExemptionReason, opt => opt.Ignore());

                    var engine = new MappingEngine(config);
                    engine.Map(reviewExemptionVm, reviewExemptionExisting);

                    _projectrepository.UpdateReviewExemption(reviewExemptionExisting);
                    _projectrepository.Save();
                    //Send Emails for approval exempt

                    await SendExemptionEmail(reviewExemptionExisting, reviewDuedate, user, reviewsURL, Convert.ToInt16(reviewExemptionExisting.StageID));
                }
                else
                {
                    //Create new model object for database insertion.
                    ReviewExemption reviewExemption = new ReviewExemption();
                    Performance performance = _projectrepository.GetProjectPerformance(reviewExemptionVm.ProjectID);

                    string reviewDuedate;

                    if (reviewExemptionVm.ExemptionType.Trim().ToUpper() == "ANNUAL REVIEW")
                    {

                        reviewDuedate = performance.ARDueDate != null ? performance.ARDueDate.Value.ToShortDateString() : "n/a";
                    }
                    else
                    {
                        reviewDuedate = performance.PCRDueDate != null ? performance.PCRDueDate.Value.ToShortDateString() : "n/a";
                    }

                    reviewExemptionVm.Requester = user;
                    reviewExemptionVm.StageID = "1"; //Awaiting approval

                    reviewExemptionVm.LastUpdated = DateTime.Now;
                    reviewExemptionVm.UpdatedBy = user;
                    //
                    // Mapper.Map(reviewExemptionVm, reviewExemption);


                    Mapper.CreateMap<ReviewExemptionVM, ReviewExemption>();
                    Mapper.Map<ReviewExemptionVM, ReviewExemption>(reviewExemptionVm, reviewExemption);


                    _projectrepository.InsertReviewExemption(reviewExemption);
                    _projectrepository.Save();

                    //Send out the e-mails.
                    //Send Authorization email 
                    await SendExemptionEmail(reviewExemption, reviewDuedate, user, reviewsURL, Convert.ToInt16(reviewExemption.StageID));

                }
                return true;

            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(reviewExemptionVm.ProjectID, exception, user);
                return false;

            }


        }

        //Send Emails for Exemptions
        private async Task SendExemptionEmail(ReviewExemption reviewExemption, string reviewDueDate, string user, string reviewsURL, int StageId)
        {
            //construct team members, approvers, sro's, current users, 
            string from = AMPUtilities.SenderEmail();
            string to = "";
            string cc = "";
            string subject = "";
            string body = "";
            string ACTION_REQUIRED = @"<span style=""background-color:yellow;color:red;font-weight:bold;font-family:Arial;font-size: 12pt;"">Action Required</span><br/><br/>";
            string NOTIFICATION = @"<span style=""background-color:Green;color:WHITE;font-weight:bold;font-family:Arial;font-size: 12pt;"">Exemption Approved</span><br/><br/>";
            string REJECTED = @"<span style=""background-color:red;color:white;font-weight:bold;font-family:Arial;font-size: 12pt;"">Exemption Rejected</span><br/><br/>";
            string FOR_INFO = @"<span style=""background-color:yellow;color:red;font-weight:bold;font-family:Arial;font-size: 12pt;"">For Information</span><br/><br/>";
            //----

            ProjectMaster pm = _projectrepository.GetProject(reviewExemption.ProjectID);
            Performance projectPerformance = _projectrepository.GetProjectPerformance(reviewExemption.ProjectID);
            string projID = reviewExemption.ProjectID.Trim().ToString();
            Team sro = _projectrepository.GetTeam(reviewExemption.ProjectID).FirstOrDefault(x => x.RoleID == "SRO" && x.Status == "A");

            //Get Employee details from the PersonService                         
            List<string> users = new List<string>();
            if (reviewExemption.Approver != null)
                users.Add(reviewExemption.Approver);  //Approver's emp_no
            if (reviewExemption.Requester != null)
                users.Add(reviewExemption.Requester);  //Requester's emp_no
            if (sro != null)
                users.Add(sro.TeamID);          //SRO
            users.Add(user);                //Current User
            IEnumerable<string> empIds = users;

            IEnumerable<PersonDetails> personDetails = await _personService.GetPeopleDetails(empIds);
            PersonDetails approver = new PersonDetails();
            PersonDetails sroPerson = new PersonDetails();
            PersonDetails currUser = new PersonDetails();
            PersonDetails requester = new PersonDetails();


            foreach (PersonDetails pd in personDetails)
            {
                if (pd.EmpNo.Trim() == reviewExemption.Approver.Trim())
                    approver = pd;
                else if (sro != null && pd.EmpNo.Trim() == sro.TeamID.Trim())
                    sroPerson = pd;
                else if (pd.EmpNo.Trim() == user.Trim())
                    currUser = pd;
            }

            requester = personDetails.FirstOrDefault(x => x.EmpNo.Trim() == reviewExemption.Requester.Trim());

            if (sro != null && sro.TeamID.Trim() == user.Trim()) //since identity lite sends only one if two emp_ids are identitycal
                currUser = sroPerson;

            if (reviewExemption.Approver != null && (sro != null && sro.TeamID.Trim() == reviewExemption.Approver.Trim())) //HDO also SRO-  since identity lite sends only one if two emp_ids are identitycal
                sroPerson = approver;


            //get all active team members' email
            IEnumerable<Team> allActiveMembers = _projectrepository.GetTeam(projID);
            users = new List<string>();

            foreach (Team team in allActiveMembers)
            {
                if (team.TeamID.Trim() != requester.EmpNo.Trim())
                // the email will be going to requester so no need to add him/her in the cc list
                {
                    users.Add(team.TeamID);
                }
            }
            empIds = users;
            if (empIds.Any())
            {
                IEnumerable<PersonDetails> teamDetails = await _personService.GetPeopleDetails(empIds);
                cc = GetTemmembersEmails(teamDetails);
            }

            //Set an SRO string.

            string SRODisplay;

            if (sro == null)
            {
                SRODisplay = "This project does not have an SRO and does not comply with Smart Rules.";
            }
            else
            {
                SRODisplay = "The SRO is " + sroPerson.Forename + ' ' + sroPerson.Surname;
            }

            switch (StageId.ToString())
            {
                case "0": //Exempt request deleted - notify approver
                    to = approver.Email;
                    body = string.Format("<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>You may have received an e-mail asking you to approve {3} Exempt request for {1}. However, {2} has now withdrawn the request for exempt in the Aid Management Platform (AMP) and as a result you do not need to approve it."
                       , approver.Forename, pm.Title, currUser.Forename, reviewExemption.ExemptionType);
                    subject = string.Format("Exemption request deleted for {0}", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "1": //sent for approval - notify Approver
                    to = approver.Email;
                    body = string.Format(ACTION_REQUIRED + "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has sent you a {5} exemption request for your approval in the Aid Management Platform." +
                        "  Please review the request and log your comments before approving or rejecting the exemption.<br/><br/>{2}<br/><br/>The due date of the review is {3}.  " +
                        " {4}.<br/><br/>Further guidance on Review Exemptions can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, currUser.Forename, reviewsURL, reviewDueDate, SRODisplay, reviewExemption.ExemptionType);
                    subject = string.Format("ACTION: " + "Exemption request for {0}", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "2": //Approved - notify SRO
                    to = requester.Email;
                    body = string.Format(NOTIFICATION + "<span style='font-family:Arial;font-size: 12pt;'>{0} has approved {4} exemption request in the Aid Management Platform. Please click on the link below to review the approvers comments.<br/><br/>{1}<br/><br/>" +
                        "The date the exemption was approved {2}. {3}.<br/><br/>" +
                        "Further guidance on Review Exemptions can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, reviewsURL, System.DateTime.Now.ToShortDateString(), SRODisplay, reviewExemption.ExemptionType);
                    subject = string.Format("Exemption approved for {0}", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "3": //Rejected - notify SRO
                    to = requester.Email;
                    body = string.Format(REJECTED + "<span style='font-family:Arial;font-size: 12pt;'>{0} has rejected {4} exemption request in the Aid Management Platform (AMP). Please click on the link below to review the approvers comments. <br/><br/>{1}<br/><br/>" +
                       "The date the exemption was rejected  {2}. {3}.<br/><br/>" +
                       "Further guidance on Review Exemptions can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, reviewsURL, System.DateTime.Now.ToShortDateString(), SRODisplay, reviewExemption.ExemptionType);
                    subject = string.Format("Exemption rejected for {0}", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "4": // Notify Old approver - No longer need to take any action
                    to = approver.Email;
                    body = string.Format(FOR_INFO + "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} sent you a {5} exemption request for your approval in the Aid Management Platform." +
                        " The approver has been changed for this exemption so you are not required to take any action.<br/><br/>{2}<br/><br/>The due date of the review is {3}.  " +
                        " {4}.<br/><br/>Further guidance on Review Exemptions can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, currUser.Forename, reviewsURL, reviewDueDate, SRODisplay, reviewExemption.ExemptionType);
                    subject = string.Format("For Info: " + "No action required for Exemption request - {0}", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                default:
                    break;
            }

        }

        //Send emails for Deferral 
        private async Task SendDeferralEmails(ReviewDeferral reviewDeferral, string reviewDueDate, string user, string reviewsURL, int StageId)
        {
            //construct team members, approvers, sro's, current users, 
            string from = AMPUtilities.SenderEmail();
            string to = "";
            string cc = "";
            string subject = "";
            string body = "";
            string ACTION_REQUIRED = @"<span style=""background-color:yellow;color:red;font-weight:bold;font-family:Arial;font-size: 12pt;"">Action Required</span><br/><br/>";
            string NOTIFICATION = @"<span style=""background-color:Green;color:WHITE;font-weight:bold;font-family:Arial;font-size: 12pt;"">Deferral Approved</span><br/><br/>";
            string REJECTED = @"<span style=""background-color:red;color:white;font-weight:bold;font-family:Arial;font-size: 12pt;"">Deferral Rejected</span><br/><br/>";

            ProjectMaster pm = _projectrepository.GetProject(reviewDeferral.ProjectID);


            //ReviewMaster revMaster = pm.ReviewMasters.FirstOrDefault(x => x.ProjectID == reviewDeferralVm.ProjectID && x.ReviewID == reviewdeferral.DeferralID);
            //if (reviewVm.StageID == "1") revMaster.Approver = reviewVm.Approver; //approver has been selected from UI not the one in the database.
            Team sro = _projectrepository.GetTeam(reviewDeferral.ProjectID).FirstOrDefault(x => x.RoleID == "SRO" && x.Status == "A");

            //Get Employee details from the PersonService                         
            List<string> users = new List<string>();
            if (reviewDeferral.Approver != null)
                users.Add(reviewDeferral.Approver);  //Approver's emp_no
            if (reviewDeferral.Requester != null)
                users.Add(reviewDeferral.Requester);  //Requester's emp_no
            if (sro != null)
                users.Add(sro.TeamID);          //SRO
            users.Add(user);                //Current User
            IEnumerable<string> empIds = users;

            IEnumerable<PersonDetails> personDetails = await _personService.GetPeopleDetails(empIds);
            PersonDetails approver = new PersonDetails();
            PersonDetails sroPerson = new PersonDetails();
            PersonDetails currUser = new PersonDetails();
            PersonDetails requester = new PersonDetails();

            foreach (PersonDetails pd in personDetails)
            {
                if (reviewDeferral.Approver != null && pd.EmpNo.Trim() == reviewDeferral.Approver.Trim())
                    approver = pd;
                else if (sro != null && pd.EmpNo.Trim() == sro.TeamID.Trim())
                    sroPerson = pd;
                else if (pd.EmpNo.Trim() == user.Trim())
                    currUser = pd;
            }

            if (sro != null && sro.TeamID.Trim() == user.Trim()) //since identity lite sends only one if two emp_ids are identitycal
                currUser = sroPerson;

            if (reviewDeferral.Approver != null && (sro != null && sro.TeamID.Trim() == reviewDeferral.Approver.Trim())) //HDO also SRO-  since identity lite sends only one if two emp_ids are identitycal
                sroPerson = approver;


            requester = personDetails.FirstOrDefault(x => x.EmpNo.Trim() == reviewDeferral.Requester.Trim());


            //get all active team members' email
            IEnumerable<Team> allActiveMembers = _projectrepository.GetTeam(reviewDeferral.ProjectID);
            users = new List<string>();

            foreach (Team team in allActiveMembers)
            {
                if (team.TeamID.Trim() != requester.EmpNo.Trim())
                // the email will be going to requester so no need to add him/her in the cc list
                {

                    users.Add(team.TeamID);
                }
            }
            empIds = users;
            if (empIds.Any())
            {
                IEnumerable<PersonDetails> teamDetails = await _personService.GetPeopleDetails(empIds);
                cc = GetTemmembersEmails(teamDetails);
            }
            //end all team members


            //Set an SRO string.

            string SRODisplay;

            if (sro == null)
            {
                SRODisplay = "This project does not have an SRO and does not comply with Smart Rules.";
            }
            else
            {
                SRODisplay = "The SRO is " + sroPerson.Forename + ' ' + sroPerson.Surname;
            }

            switch (StageId.ToString())
            {
                case "0": //Deferral request deleted - notify approver
                    to = approver.Email;
                    body = string.Format("<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>You may have received an e-mail asking you to approve deferral request for {1}. However, {2} has now withdrawn the request for deferral in the Aid Management Platform (AMP) and as a result you do not need to approve it."
                       , approver.Forename, pm.Title, currUser.Forename);
                    subject = string.Format("Deferral request deleted for {0}", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "1": //sent for approval - notify Approver
                    to = approver.Email;
                    body = string.Format(ACTION_REQUIRED + "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has sent you a deferral request for your approval in the Aid Management Platform." +
                        "  Please review the request and log your comments before approving or rejecting the deferral.<br/><br/>{2}<br/><br/>The due date of the review is {3}.  " +
                        "{4}.<br/><br/>Further guidance on Review Deferrals can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, currUser.Forename, reviewsURL, reviewDueDate, SRODisplay);
                    subject = string.Format("ACTION: " + "Deferral request for {0}", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "2": //Approved - notify SRO
                    to = requester.Email;
                    body = string.Format(NOTIFICATION + "<span style='font-family:Arial;font-size: 12pt;'>{0} has approved deferral request in the Aid Management Platform. Please click on the link below to review the approvers comments.<br/><br/>{1}<br/><br/>" +
                        "The date the review was approved {2}. {3}.<br/><br/>" +
                        "Further guidance on Project Reviews can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, reviewsURL, System.DateTime.Now.ToShortDateString(), SRODisplay);
                    subject = string.Format("Deferral approved for {0}", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "3": //Rejected - notify SRO
                    to = requester.Email;
                    body = string.Format(REJECTED + "<span style='font-family:Arial;font-size: 12pt;'>{0} has rejected deferral request in the Aid Management Platform (AMP). Please click on the link below to review the approvers comments. <br/><br/>{1}<br/><br/>" +
                       "The date the review was rejected  {2}. {3}.<br/><br/>" +
                       "Further guidance on Project Reviews can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, reviewsURL, System.DateTime.Now.ToShortDateString(), SRODisplay);
                    subject = string.Format("Deferral rejected for {0}", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                default:
                    break;
            }
        }

        //Send Email for Reviews 
        private async Task SendReveiwEmails(ReviewVM reviewVm, string StageID, string user, string reviewsURL)
        {
            string from = AMPUtilities.SenderEmail();
            string to = "";
            string cc = "";
            string subject = "";
            string body = "";
            ProjectMaster pm = _projectrepository.GetProject(reviewVm.ProjectID);
            Performance projectPerformance = _projectrepository.GetProjectPerformance(reviewVm.ProjectID);
            ReviewMaster revMaster = pm.ReviewMasters.FirstOrDefault(x => x.ProjectID == reviewVm.ProjectID && x.ReviewID == reviewVm.ReviewID);
            if (reviewVm.StageID == "1") revMaster.Approver = reviewVm.Approver; //approver has been selected from UI not the one in the database.
            Team sro = _projectrepository.GetTeam(reviewVm.ProjectID).FirstOrDefault(x => x.RoleID == "SRO" && x.Status == "A");

            //Get Employee details from the PersonService                         
            List<string> users = new List<string>();
            if (revMaster.Approver != null)
                users.Add(revMaster.Approver);  //Approver's emp_no
            if (revMaster.Requester != null)
                users.Add(revMaster.Requester);  //Requester's emp_no
            if (sro != null)
                users.Add(sro.TeamID);          //SRO
            users.Add(user);                //Current User
            IEnumerable<string> empIds = users;


            IEnumerable<PersonDetails> personDetails = await _personService.GetPeopleDetails(empIds);
            PersonDetails approver = new PersonDetails();
            PersonDetails requester = new PersonDetails();
            PersonDetails sroPerson = new PersonDetails();
            PersonDetails currUser = new PersonDetails();

            foreach (PersonDetails pd in personDetails)
            {
                if (pd.EmpNo.Trim() == revMaster.Approver.Trim())
                    approver = pd;
                else if (pd.EmpNo.Trim() == sro.TeamID.Trim())
                    sroPerson = pd;
                else if (pd.EmpNo.Trim() == user.Trim())
                    currUser = pd;
            }

            requester = personDetails.FirstOrDefault(x => x.EmpNo.Trim() == revMaster.Requester.Trim());


            if (sro != null && sro.TeamID.Trim() == user.Trim()) //since identity lite sends only one if two emp_ids are identitycal
                currUser = sroPerson;

            if (revMaster.Approver != null && (sro != null && sro.TeamID.Trim() == revMaster.Approver.Trim())) //HDO also SRO-  since identity lite sends only one if two emp_ids are identitycal
                sroPerson = approver;

            //get all active team members' email
            IEnumerable<Team> allActiveMembers = _projectrepository.GetTeam(reviewVm.ProjectID);
            users = new List<string>();
            foreach (Team team in allActiveMembers)
            {
                if (team.TeamID.Trim() != requester.EmpNo) // the email will be going to requester so no need to add him/her in the cc list
                {
                    users.Add(team.TeamID);
                }
            }
            empIds = users;
            {
                IEnumerable<PersonDetails> teamDetails = await _personService.GetPeopleDetails(empIds);
                cc = GetTemmembersEmails(teamDetails);
            }
            //end all team members

            //customize email body and subject differently for AR and PCR:    
            string emailSubjectFirstPart;
            string emailBodyFragment;
            string dueDate;
            if (revMaster.ReviewType.ToUpper().Trim() == "ANNUAL REVIEW")
            {
                emailSubjectFirstPart = "Annual Review";
                emailBodyFragment = "an Annual Review";
                dueDate = projectPerformance.ARDueDate != null ? projectPerformance.ARDueDate.Value.ToShortDateString() : "n/a";

            }
            else
            {
                emailSubjectFirstPart = "PCR";
                emailBodyFragment = "a PCR";
                dueDate = projectPerformance.PCRDueDate != null ? projectPerformance.PCRDueDate.Value.ToShortDateString() : "n/a";
            }

            //log = string.Format("to is {0}, cc is {1}, from is {2}.", to, cc, from);

            //loggingengine.InsertLog(log,user);

            switch (StageID)
            {
                case "1": //sent for approval - notify Approver
                    const string ACTION_REQUIRED = @"<span style=""background-color:yellow;color:red;font-weight:bold;font-family:Arial;font-size: 12pt;"">Action Required</span><br/><br/>";
                    to = approver.Email;

                    body = string.Format(ACTION_REQUIRED + "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has sent you {5} for your approval in the Aid Management Platform (AMP)." +
                        "  Please review the proposed scoring and log your comments before approving or rejecting the review." +
                        "<br/> Once approved, the Annual Review will be published and made publicly available on the Development Tracker. Along with the SRO, you must make sure that it is fit for publication. " +
                        " Please check the Smart Guides on " + "<a href =\"\">Transparency</a>" +
                        "<br/><br/> Please use the link below to access the approval screen" +

                        "<br/><br/>{2}<br/><br/>The due date of the review is {3}.  " +
                        "The SRO is {4}.<br/><br/>Further guidance on Project Reviews can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, currUser.Forename, reviewsURL, dueDate, sroPerson.Forename + " " + sroPerson.Surname, emailBodyFragment);

                    subject = string.Format("ACTION: {0} Approval Request - {1}", emailSubjectFirstPart, pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "2": //Approved - notify SRO
                    const string NOTIFICATION = @"<span style=""background-color:Green;color:WHITE;font-weight:bold;font-family:Arial;font-size: 12pt;"">Review Approved</span><br/><br/>";
                    to = requester.Email;

                    body = string.Format(NOTIFICATION + "<span style='font-family:Arial;font-size: 12pt;'>{5}<br/><br/>{0} has approved {4} in the Aid Management Platform (AMP). Please can the SRO check the " + "<a href =\"\">Transparency</a>" + "Smart Guide" +
                        "<br/><br/> Please click on the link below to review the approvers comments.<br/><br/>{1}<br/><br/>" +
                        "The date the review was approved {2}. The SRO is {3}.<br/><br/>" +
                        "Further guidance on Project Reviews can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, reviewsURL, System.DateTime.Now.ToShortDateString(), sroPerson.Forename + " " + sroPerson.Surname, emailBodyFragment, requester.Forename);

                    subject = string.Format("{0} Approved - {1}", emailSubjectFirstPart, pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "3": //Rejected - notify SRO
                    const string REJECTED = @"<span style=""background-color:red;color:white;font-weight:bold;font-family:Arial;font-size: 12pt;"">Review Rejected</span><br/><br/>";
                    to = requester.Email;
                    body = string.Format(REJECTED + "<span style='font-family:Arial;font-size: 12pt;'>{5}<br/><br/>{0} has rejected {4} in the Aid Management Platform (AMP). Please click on the link below to review the approvers comments. The AR can be resent for approval in AMP<br/><br/>{1}<br/><br/>" +
                       "The date the review was rejected  {2}. The SRO is {3}.<br/><br/>" +
                       "Further guidance on Project Reviews can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, reviewsURL, System.DateTime.Now.ToShortDateString(), sroPerson.Forename + " " + sroPerson.Surname, emailBodyFragment, requester.Forename);
                    subject = string.Format("{0} Rejected - {1}", emailSubjectFirstPart, pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.High);
                    break;

                case "4": //Change approver - notify Approver
                    const string FOR_INFO = @"<span style=""background-color:yellow;color:red;font-weight:bold;font-family:Arial;font-size: 12pt;"">For Information</span><br/><br/>";
                    to = approver.Email;
                    body = string.Format(FOR_INFO + "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} sent you {5} for your approval in the Aid Management Platform (AMP)." +
                        "  The approver has been changed for this review so you are not required to take any action.<br/><br/>{2}<br/><br/>The due date of the review is {3}.  " +
                        "The SRO is {4}.<br/><br/>Further guidance on Project Reviews can be found in the Smart Rules and the Smart Guide on Reviewing and Scoring Projects.</span>", approver.Forename, currUser.Forename, reviewsURL, dueDate, sroPerson.Forename+ " " + sroPerson.Surname, emailBodyFragment);
                    subject = string.Format("FOR INFO: {0} - {1}", "No Action Required for ", pm.Title);
                    _emailService.SendEmail(to, cc, "", from, subject, body, System.Net.Mail.MailPriority.Normal);
                    break;

                default:
                    break;
            }

        }



        // This will insert a log entry for a user accessing a controller action/view
        public void InsertLog(String ViewName, String user)
        {
            try
            {
                //Execute logging engine to insert a log entry
                _loggingengine.InsertLog(ViewName, user);
            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                //    errorengine.LogError(ex);
            }
        }

        // Overloader for where ProjectID is available.
        public void InsertLog(String ViewName, String user, String ProjectID)
        {
            try
            {
                //Execute logging engine to insert a log entry
                _loggingengine.InsertLog(ViewName, user, ProjectID);
            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                //     errorengine.LogError(ex);
            }
        }

        // Insert a Code log entry
        public void InsertCodeLog(String MethodName, String Description, DateTime From, DateTime To, TimeSpan Result, String User)
        {
            try
            {
                //Execute logging engine to insert a log entry
                _loggingengine.InsertCodeLog(MethodName, Description, From, To, Result, User);
            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                //  errorengine.LogError(ex);
            }
        }

        // Insert a Code log entry Overloader 

        public void InsertCodeLog(String MethodName, String Description, DateTime From, DateTime To, TimeSpan Result, String User, String Value)
        {
            try
            {
                //Execute logging engine to insert a log entry
                _loggingengine.InsertCodeLog(MethodName, Description, From, To, Result, User, Value);
            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                //         errorengine.LogError(ex);
            }
        }

        private static bool ProjectDatesAreValid(ProjectVM projectviewmodel, IValidationDictionary validationDictionary)
        {


            //These are the only dates that the user enters, the others are datetime objects generated by c#
            if (!IsValidDate(projectviewmodel.ProjectDates.OperationalStartDate_Day, projectviewmodel.ProjectDates.OperationalStartDate_Month, projectviewmodel.ProjectDates.OperationalStartDate_Year, "ProjectDates.OperationalStartDate", validationDictionary))
                return validationDictionary.IsValid;
            if (!IsValidDate(projectviewmodel.ProjectDates.OperationalEndDate_Day, projectviewmodel.ProjectDates.OperationalEndDate_Month, projectviewmodel.ProjectDates.OperationalEndDate_Year, "ProjectDates.OperationalEndDate", validationDictionary))
                return validationDictionary.IsValid;


            return validationDictionary.IsValid;

        }



        private static bool AuditedStatementDatesAreValid(ProjectStatementVM projectstatementvm, IValidationDictionary validationDictionary)
        {
            //*** Received Date
            //1. check a received date has been entered
            if (projectstatementvm.NewProjectStatement.ReceivedDate_Year == null || projectstatementvm.NewProjectStatement.ReceivedDate_Month == null || projectstatementvm.NewProjectStatement.ReceivedDate_Day == null)

            {
                validationDictionary.AddError("NewProjectStatement.ReceivedDate", String.Format("Please enter a received date", projectstatementvm.NewProjectStatement.ReceivedDate));
                return validationDictionary.IsValid;
            }

            //2. received date must be valid
            if (!IsValidDate(projectstatementvm.NewProjectStatement.ReceivedDate_Day, projectstatementvm.NewProjectStatement.ReceivedDate_Month, projectstatementvm.NewProjectStatement.ReceivedDate_Year, "NewProjectStatement.ReceivedDate", validationDictionary))
                return validationDictionary.IsValid;

            DateTime ReceivedDate = new DateTime(projectstatementvm.NewProjectStatement.ReceivedDate_Year.Value,
             projectstatementvm.NewProjectStatement.ReceivedDate_Month.Value, projectstatementvm.NewProjectStatement.ReceivedDate_Day.Value);
            //3. received date cannot be in the future
            if (ReceivedDate > DateTime.Today)
            {
                validationDictionary.AddError("NewProjectStatement.ReceivedDate", String.Format("Statements cannot be received in the future", projectstatementvm.NewProjectStatement.ReceivedDate));
                return validationDictionary.IsValid;
            }


            //*** period from date
            //1. check that a period from date has been entered

            if (projectstatementvm.NewProjectStatement.PeriodFrom_Year == null || projectstatementvm.NewProjectStatement.PeriodFrom_Month == null || projectstatementvm.NewProjectStatement.PeriodFrom_Day == null)

            {
                validationDictionary.AddError("NewProjectStatement.PeriodFrom", String.Format("Please enter a period from date", projectstatementvm.NewProjectStatement.PeriodFrom));
                return validationDictionary.IsValid;
            }

            //2.  now check the date is valid
            if (projectstatementvm.NewProjectStatement.PeriodFrom_Year != null || projectstatementvm.NewProjectStatement.PeriodFrom_Month != null || projectstatementvm.NewProjectStatement.PeriodFrom_Day != null)
            {
                if (!IsValidDate(projectstatementvm.NewProjectStatement.PeriodFrom_Day, projectstatementvm.NewProjectStatement.PeriodFrom_Month, projectstatementvm.NewProjectStatement.PeriodFrom_Year, "NewProjectStatement.PeriodFromDate", validationDictionary))
                    return validationDictionary.IsValid;
            }
            DateTime PeriodFrom = new DateTime(projectstatementvm.NewProjectStatement.PeriodFrom_Year.Value,
            projectstatementvm.NewProjectStatement.PeriodFrom_Month.Value, projectstatementvm.NewProjectStatement.PeriodFrom_Day.Value);


            //*** period to date
            //1. check a period to date has been entered
            if (projectstatementvm.NewProjectStatement.PeriodTo_Year == null || projectstatementvm.NewProjectStatement.PeriodTo_Month == null || projectstatementvm.NewProjectStatement.PeriodTo_Day == null)

            {
                validationDictionary.AddError("NewProjectStatement.PeriodTo", String.Format("Please enter a period to date", projectstatementvm.NewProjectStatement.PeriodTo));
                return validationDictionary.IsValid;
            }
            //2. period to date - must be valid and also not before the received date nnd not before the period from date
            if (projectstatementvm.NewProjectStatement.PeriodTo_Year != null || projectstatementvm.NewProjectStatement.PeriodTo_Month != null || projectstatementvm.NewProjectStatement.PeriodTo_Day != null)
            {
                if (!IsValidDate(projectstatementvm.NewProjectStatement.PeriodTo_Day, projectstatementvm.NewProjectStatement.PeriodTo_Month, projectstatementvm.NewProjectStatement.PeriodTo_Year, "NewProjectStatement.PeriodToDate", validationDictionary))
                    return validationDictionary.IsValid;

                DateTime PeriodTo = new DateTime(projectstatementvm.NewProjectStatement.PeriodTo_Year.Value,
                projectstatementvm.NewProjectStatement.PeriodTo_Month.Value, projectstatementvm.NewProjectStatement.PeriodTo_Day.Value);

                if (ReceivedDate < PeriodTo)
                {
                    validationDictionary.AddError("NewProjectStatement.ReceivedDate", String.Format("Received date must be later than period to date", projectstatementvm.NewProjectStatement.ReceivedDate));
                    return validationDictionary.IsValid;
                }

                if (PeriodFrom > PeriodTo)
                {
                    validationDictionary.AddError("NewProjectStatement.PeriodTo", String.Format("Period to date must be later than period from date", projectstatementvm.NewProjectStatement.PeriodTo));
                    return validationDictionary.IsValid;
                }
            }

            return validationDictionary.IsValid;

        }



        private bool PlannedEndDateIsValid(WorkflowPlannedEndDateVM workflowPlannedEndDateVm, IValidationDictionary validationDictionary)
        {
            //*** New Planned End Date
            //1. check that a date has been entered

            if (workflowPlannedEndDateVm.NewPlannedEndDate_Day == 0 || workflowPlannedEndDateVm.NewPlannedEndDate_Month == 0 || workflowPlannedEndDateVm.NewPlannedEndDate_Year == 0)

            {
                validationDictionary.AddError("NewPlannedEndDate", String.Format("Please enter a new planned end date.", workflowPlannedEndDateVm.NewPlannedEndDate));
                return validationDictionary.IsValid;
            }

            //2.  now check the date entered is valid
            if (workflowPlannedEndDateVm.NewPlannedEndDate_Day != 0 || workflowPlannedEndDateVm.NewPlannedEndDate_Month != 0 || workflowPlannedEndDateVm.NewPlannedEndDate_Year != 0)
            {
                if (!IsValidDate(workflowPlannedEndDateVm.NewPlannedEndDate_Day, workflowPlannedEndDateVm.NewPlannedEndDate_Month, workflowPlannedEndDateVm.NewPlannedEndDate_Year, "NewPlannedEndDate", validationDictionary))
                    return validationDictionary.IsValid;
            }

            //////3. Check that the date is in the future **** THIS IS NO LONGER REQUIRED AS WAS CAUSING USER ISSUES ON ALREADY CLOSED PROJECTS
            DateTime NewPlannedDate = new DateTime(workflowPlannedEndDateVm.NewPlannedEndDate_Year,
              workflowPlannedEndDateVm.NewPlannedEndDate_Month, workflowPlannedEndDateVm.NewPlannedEndDate_Day);

            //if (NewPlannedDate < DateTime.Now)
            //{
            //    validationDictionary.AddError("NewPlannedEndDate", String.Format("Planned end date cannot be earlier than today", NewPlannedDate));
            //   return validationDictionary.IsValid;
            //}

            //4. Need to check to ensure that the component end dates for the prohject do not extend beyond the new planned end date
            //
            IEnumerable<ComponentMaster> componentMasters = _projectrepository.GetComponents(workflowPlannedEndDateVm.ProjectHeaderVm.ProjectID);
            if (componentMasters.Any())
            {
                List<ComponentDate> componentDates = new List<ComponentDate>();

                foreach (ComponentMaster componentMaster in componentMasters)
                {
                    if (componentMaster.ComponentDate != null)
                    {
                        componentDates.Add(componentMaster.ComponentDate);
                    }
                }

                //Component Dates
                foreach (ComponentDate componentDate in componentDates)
                {
                    if (componentDate.OperationalEndDate > NewPlannedDate)   //Project End Date cannot be before any Component End Date
                    {
                        validationDictionary.AddError("projectToValidate.OperationalEndDate", String.Format("The Planned End Date of component {0} cannot be later than the new Project Planned End Date. Please update this first.", componentDate.ComponentID));
                    }

                }
                if (!validationDictionary.IsValid)
                {
                    return validationDictionary.IsValid;
                }

            }


            return validationDictionary.IsValid;
        }


        private bool IsTeamMember(string empNo, IEnumerable<Team> currentTeam)
        {
            //NOTE: Any changes to this code should be replicated in the class WorkflowService, IsTeamMember Method. The method is almost identical and has been included as part of the
            //redevelopment of Workflow to be a service. Once project becomes a 'service' there will be one method, probably sitting in a single utility class.

            if (currentTeam.Any(x => x.TeamID.Trim().Equals(empNo.Trim()) && x.Status.Equals("A"))) return true;
            else return false;
        }
        public bool UpdateTeamMarker(ProjectTeamVM projectTeamVm, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                //Dont need any custom validation for Team Marker, yet.
                if (validationDictionary.IsValid)
                {
                    ProjectInfo projectInfo = _projectrepository.GetProjectInfo(projectTeamVm.ProjectHeader.ProjectID);

                    projectInfo.TeamMarker = projectTeamVm.TeamMarker;
                    projectInfo.UserID = user;
                    projectInfo.LastUpdate = DateTime.Now;

                    _projectrepository.UpdateProjectInfo(projectInfo);
                    _projectrepository.Save();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(projectTeamVm.ProjectHeader.ProjectID, ex, user);
                return false;


            }
        }

        public bool AddTeamMember(ProjectTeamVM projectTeamVm, IValidationDictionary validationDictionary, string user)
        {
            try
            {

                //Set the New Team Member Role based on the selection made from the radio buttons.
                if (projectTeamVm.NewTeamMember.ProjectRolesVm != null)
                {
                    projectTeamVm.NewTeamMember.RoleID = projectTeamVm.NewTeamMember.ProjectRolesVm.SelectedRoleValue;
                }
                else
                {

                }

                //Get Current Team
                IEnumerable<Team> currentTeam = _projectrepository.GetTeam(projectTeamVm.ProjectHeader.ProjectID);

                //Validate new team member
                if (!ValidateAddTeam(projectTeamVm, currentTeam, validationDictionary))
                    return false;

                Team newTeam = new Team();

                //DateTime endDate = new DateTime(projectTeamVm.NewTeamMember.EndDate_Year,
                //    projectTeamVm.NewTeamMember.EndDate_Month, projectTeamVm.NewTeamMember.EndDate_Day);

                //Commented out the code below. Now that Start date is read only the date parts dont build into a proper date. Use the StartDate DateTime object instead.
                //DateTime startDate = new DateTime(projectTeamVm.NewTeamMember.StartDate_Year,
                //    projectTeamVm.NewTeamMember.StartDate_Month, projectTeamVm.NewTeamMember.StartDate_Day);


                newTeam.TeamID = projectTeamVm.NewTeamMember.TeamID;
                newTeam.ProjectID = projectTeamVm.ProjectHeader.ProjectID;
                newTeam.RoleID = projectTeamVm.NewTeamMember.RoleID;
                newTeam.Status = "A";
                newTeam.LastUpdated = DateTime.Now;
                newTeam.StartDate = projectTeamVm.NewTeamMember.StartDate;
                newTeam.EndDate = null;
                newTeam.UserID = user;

                _projectrepository.InsertTeam(newTeam);
                _projectrepository.Save();

            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(projectTeamVm.ProjectHeader.ProjectID, ex, user);
                return false;
            }

            //Temp
            return true;
        }

        public bool UpdateTeamMember(EditTeamMemberVM editTeamMemberVm, string user)
        {
            try
            {
                Team existingTeamRecord = _projectrepository.GetTeamMember(editTeamMemberVm.ID);

                existingTeamRecord.EndDate = DateTime.Now;
                existingTeamRecord.Status = "C";
                existingTeamRecord.LastUpdated = DateTime.Now;
                existingTeamRecord.UserID = user;
                //Set the current Team Member record to inactive.

                _projectrepository.EndTeamMember(existingTeamRecord, false);

                Team newTeamRecord = new Team
                {
                    ProjectID = editTeamMemberVm.ProjectID,
                    TeamID = editTeamMemberVm.TeamID,
                    StartDate = DateTime.Now,
                    EndDate = null,
                    RoleID = editTeamMemberVm.ProjectRolesVm.SelectedRoleValue,
                    Status = "A",
                    LastUpdated = DateTime.Now,
                    UserID = user
                };

                //Insert a new Team Member record to reflect the change in role.
                _projectrepository.InsertTeam(newTeamRecord);
                _projectrepository.Save();

                return true;
            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(editTeamMemberVm.ProjectID, ex, user);
                return false;
            }

        }

        public bool EndTeamMember(EditTeamMemberVM editTeamMemberVm, string user)
        {
            try
            {
                Team existingTeamRecord = _projectrepository.GetTeamMember(editTeamMemberVm.ID);

                existingTeamRecord.EndDate = DateTime.Now;
                existingTeamRecord.Status = "C";
                existingTeamRecord.LastUpdated = DateTime.Now;
                existingTeamRecord.UserID = user;

                //Set the current Team Member record to inactive. Trigger a PCMX.
                _projectrepository.EndTeamMember(existingTeamRecord, true);
                _projectrepository.Save();

                return true;
            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(editTeamMemberVm.ProjectID, ex, user);
                return false;
            }

        }

        public bool UpdateProject(ProjectVM projectViewModel, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                if (!ProjectDatesAreValid(projectViewModel, validationDictionary))
                    return false;

                //Validate that the day, month and year are valid values. 
                //Take the day, month and year for each date and put them back together in the ViewModel.
                projectViewModel.ProjectDates.ActualStartDate = projectViewModel.ProjectDates.ActualStartDate;
                projectViewModel.ProjectDates.Created_date =
                    new DateTime(projectViewModel.ProjectDates.Created_Date_Year,
                        projectViewModel.ProjectDates.Created_Date_Month, projectViewModel.ProjectDates.Created_Date_Day);
                projectViewModel.ProjectDates.FinancialStartDate =
                    new DateTime(projectViewModel.ProjectDates.FinancialStartDate_Year,
                        projectViewModel.ProjectDates.FinancialStartDate_Month,
                        projectViewModel.ProjectDates.FinancialStartDate_Day);
                projectViewModel.ProjectDates.FinancialEndDate =
                    new DateTime(projectViewModel.ProjectDates.FinancialEndDate_Year,
                        projectViewModel.ProjectDates.FinancialEndDate_Month,
                        projectViewModel.ProjectDates.FinancialEndDate_Day);
                projectViewModel.ProjectDates.OperationalStartDate =
                    new DateTime(projectViewModel.ProjectDates.OperationalStartDate_Year,
                        projectViewModel.ProjectDates.OperationalStartDate_Month,
                        projectViewModel.ProjectDates.OperationalStartDate_Day);
                projectViewModel.ProjectDates.OperationalEndDate =
                    new DateTime(projectViewModel.ProjectDates.OperationalEndDate_Year,
                        projectViewModel.ProjectDates.OperationalEndDate_Month,
                        projectViewModel.ProjectDates.OperationalEndDate_Day);
                projectViewModel.ProjectDates.PromptCompletionDate =
                    new DateTime(projectViewModel.ProjectDates.PromptCompletionDate_Year,
                        projectViewModel.ProjectDates.PromptCompletionDate_Month,
                        projectViewModel.ProjectDates.PromptCompletionDate_Day);

                projectViewModel.ProjectDates.FinancialEndDate =
                    projectViewModel.ProjectDates.OperationalEndDate.Value.AddMonths(6);

                if (!ValidateProject(projectViewModel, validationDictionary))
                    return false;

                if (!ValidatePlannedEndDateChange(projectViewModel, validationDictionary))
                    return false;


                //Get the current Project objects ready for mapping
                ProjectMaster currentProjectMaster = _projectrepository.GetProject(projectViewModel.ProjectID);
                ProjectDate currentprojectdates = currentProjectMaster.ProjectDate;
                ProjectInfo currentProjectInfo = currentProjectMaster.ProjectInfo;


                //I think autmoapper is more bother than it is worth. Stop using it and manually map.

                //Map Project Master data
                currentProjectMaster.Title = AMPUtilities.CleanText(projectViewModel.Title);
                currentProjectMaster.Description = AMPUtilities.CleanText(projectViewModel.Description);
                currentProjectMaster.BudgetCentreID = projectViewModel.BudgetCentreID;
                currentProjectMaster.UserID = user;
                currentProjectMaster.LastUpdate = DateTime.Now;

                //Map ProjectInfo data
                if (currentProjectMaster.Stage == "5" || currentProjectMaster.Stage == "7")
                {
                    currentProjectInfo.RiskAtApproval = currentProjectMaster.ProjectInfo.RiskAtApproval;

                }
                else
                {
                    currentProjectInfo.RiskAtApproval = projectViewModel.RiskAtApproval;
                }

                currentProjectInfo.LastUpdate = DateTime.Now;
                currentProjectInfo.UserID = user;


                //TODO Map ProjectDate data - Some of these dates might be read only on the project screen. Need to find and remove them form the list below.
                currentprojectdates.ActualStartDate = projectViewModel.ProjectDates.ActualStartDate;
                currentprojectdates.Created_date = projectViewModel.ProjectDates.Created_date;
                currentprojectdates.FinancialStartDate = projectViewModel.ProjectDates.FinancialStartDate;
                currentprojectdates.FinancialEndDate = projectViewModel.ProjectDates.FinancialEndDate;
                currentprojectdates.OperationalStartDate = projectViewModel.ProjectDates.OperationalStartDate;
                currentprojectdates.OperationalEndDate = projectViewModel.ProjectDates.OperationalEndDate;
                currentprojectdates.PromptCompletionDate =
                    projectViewModel.ProjectDates.OperationalEndDate.Value.AddMonths(-3);
                currentprojectdates.ActualEndDate = projectViewModel.ProjectDates.ActualEndDate;
                currentprojectdates.LastUpdate = DateTime.Now;
                currentprojectdates.UserID = user;

                //MapProjectMasterVMToProjectMasterEntity(projectViewModel, currentProjectMaster, user);

                //MapProjectDateVMToProjectDateEntity(projectViewModel, currentprojectdates);

                // MapProjectCoreVMToProjectInfoEntity(projectViewModel, currentProjectInfo);


                //  CHANGING OF PCR DATES & UNEXEMPTING AR CODE COMMENTED OUT HERE AS SHOULD ONLY BE DONE IN IMPLEMENTATION VIA PLANNED END DATE WORKFLOW

                //Performance performance = _projectrepository.GetProjectPerformance(projectViewModel.ProjectID);

                //if (performance.PCRDueDate != null)
                //{
                //    performance.PCRDueDate = currentProjectMaster.ProjectDate.OperationalEndDate.Value.AddMonths(3);
                //    performance.PCRPrompt = currentProjectMaster.ProjectDate.OperationalEndDate.Value.AddMonths(-3);
                //}

                //// Checks if ar was exempt as project length was under 15 months - if now over 15 months, unexempts AR
                //if (performance.ARExcemptReason ==
                //    _projectrepository.GetSingleExemptionReason("3", "AR").ExemptionReason1 && performance.ARRequired == "No")
                //{
                //    DateTime actualPlus15Months = currentProjectMaster.ProjectDate.ActualStartDate.Value.AddMonths(15);
                //    //Compares actual end date plus 15 months to planned end date (if planned end date is later, would give an int greater than 0)
                //    if (
                //        DateTime.Compare(currentProjectMaster.ProjectDate.OperationalEndDate.Value, actualPlus15Months) >
                //        0)
                //    {
                //        UnexemptARDetails(currentProjectMaster, user);
                //    }

                //}

                //// Checks if project was exempt as AR due less than 3 months before the PCR - if AR is now due more than 3 months before PCR, unexempts AR
                //else if (performance.ARExcemptReason ==
                //         _projectrepository.GetSingleExemptionReason("7", "AR").ExemptionReason1 && performance.ARRequired == "No")
                //{
                //    DateTime arPlus3Months;
                //    // Checks if any existing reviews - if not, assumes AR was due 12 months after actual start date & adds 3 months to this for comparison (15 months in total)
                //    int existingReviews = currentProjectMaster.ReviewMasters.Count;
                //    if (existingReviews == 0)
                //    {
                //        arPlus3Months = currentProjectMaster.ProjectDate.ActualStartDate.Value.AddMonths(15);
                //    }
                //    // If existing review, takes latest & adds 3 months for the comparison
                //    else
                //    {
                //        DateTime lastReviewDate =
                //            currentProjectMaster.ReviewMasters.MaxBy(x => x.ReviewDate.Value).ReviewDate.Value;
                //        arPlus3Months = lastReviewDate.AddMonths(3);
                //    }
                //    //Compares ar due date plus 3 months to pcr due date (if pcr due date is later, would give an int greater than 0)
                //    if (DateTime.Compare(currentProjectMaster.ProjectDate.OperationalEndDate.Value.AddMonths(3), arPlus3Months) > 0)
                //    {
                //        UnexemptARDetails(currentProjectMaster, user);
                //    }
                //}


                // Update then save!
                //_projectrepository.UpdatePerformance(performance);
                _projectrepository.UpdateProject(currentProjectMaster);
                _projectrepository.UpdateProjectInfo(currentProjectInfo);
                _projectrepository.UpdateProjectDates(currentprojectdates);
                _projectrepository.Save();

                return true;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(projectViewModel.ProjectID, ex, user);
                return false;
            }
        }

        private ProjectMaster UnexemptARDetails(ProjectMaster currentProjectMaster, string user)
        {
            // Set ARreq to Yes & due date to 1 year after Actual Start Date if no existing approved reviews or to 1 year after last review 
            // Gathers existing approved reviews
            List<ReviewMaster> approvedReviews = new List<ReviewMaster>();
            foreach (var review in currentProjectMaster.ReviewMasters)
            {
                if (review.Approved == "1")
                    approvedReviews.Add(review);
            }
            int existingReviews = approvedReviews.Count;
            if (existingReviews == 0)
            {
                // Set AR Req to Yes
                currentProjectMaster.Performance.ARRequired = "Yes";
                // Clear AR Exempt Reason
                currentProjectMaster.Performance.ARExcemptReason = "";
                // Set AR Due Date
                currentProjectMaster.Performance.ARDueDate =
                    currentProjectMaster.ProjectDate.ActualStartDate.Value.AddYears(1);
                // Set AR Prompt Date
                currentProjectMaster.Performance.ARPromptDate =
                    currentProjectMaster.Performance.ARDueDate.Value.AddMonths(-3);
            }
            else
            {
                // Get Last Updated date of last review (approved date)
                DateTime lastReviewApprovalDate =
                    approvedReviews.MaxBy(x => x.LastUpdated.Value).LastUpdated.Value;
                // Get Review Date of last review
                DateTime lastReviewDate =
                    approvedReviews.MaxBy(x => x.ReviewDate.Value).ReviewDate.Value;

                // Set AR Req to Yes
                currentProjectMaster.Performance.ARRequired = "Yes";
                // Clear AR Exempt Reason & Justification
                currentProjectMaster.Performance.ARExcemptReason = "";
                currentProjectMaster.Performance.ARExemptJustification = "";
                // Set AR Due Date
                TimeSpan dateDiffActualAndDue = lastReviewDate - lastReviewApprovalDate;
                int dateDiffinDays = dateDiffActualAndDue.Days;
                if (dateDiffinDays >= 0 && dateDiffinDays <= 21)
                //Business Rule: done withn 3 weeks of AR due date the next due date will not be changed
                {
                    currentProjectMaster.Performance.ARDueDate = lastReviewDate.AddYears(1);
                }
                else
                {
                    currentProjectMaster.Performance.ARDueDate = lastReviewApprovalDate.AddYears(1);
                }
                currentProjectMaster.Performance.ARPromptDate =
                    currentProjectMaster.Performance.ARDueDate.Value.AddMonths(-3);
            }
            return currentProjectMaster;
        }

        private static void MapProjectDateVMToProjectDateEntity(ProjectVM projectViewModel, ProjectDate currentprojectdates)
        {
            //Second run for Dates
            Mapper.CreateMap<ProjectDateVM, ProjectDate>();

            //Create a blank project date.
            ProjectDate projectdates = new ProjectDate();


            // Map
            Mapper.Map(projectViewModel.ProjectDates, projectdates);

            //Second mapping run.
            Mapper.CreateMap<ProjectDate, ProjectDate>().ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

            Mapper.Map(projectdates, currentprojectdates);
        }

        private static void MapProjectMasterVMToProjectMasterEntity(ProjectVM projectViewModel, ProjectMaster currentProjectMaster, string user)
        {
            //Define a mapping between ProjectViewModel.ProjectMasterVM and a generic Project
            //Mapper.CreateMap<ProjectMasterVM, ProjectMaster>();

            ProjectMaster projectMaster = new ProjectMaster();
            ProjectInfo projectInfo = new ProjectInfo();

            projectMaster.ProjectInfo = projectInfo;

            projectMaster.ProjectID = projectViewModel.ProjectID;
            projectMaster.Title = projectViewModel.Title;
            projectMaster.BudgetCentreID = projectViewModel.BudgetCentreID;
            projectMaster.Stage = projectViewModel.Stage;
            projectMaster.Status = "A";
            projectMaster.LastUpdate = DateTime.Today;
            projectMaster.UserID = user;
            projectMaster.ProjectInfo.RiskAtApproval = projectViewModel.RiskAtApproval;

            //HACK - Set ComponentMaster and Portfolio to null - otherwise they Auto Map as zero and the deletes the existing Components and entries in the Portfolio table.
            //HACK - Add Project Reviews to that list.
            //HACK - Need to look at a better solution long term - CJF 07/01/2015.

            projectMaster.Portfolios = null;
            projectMaster.ReviewARScores = null;
            projectMaster.ReviewPCRScores = null;
            projectMaster.ReviewOutputs = null;
            projectMaster.AuditedFinancialStatements = null;
            projectMaster.ComponentMasters = null;
            projectMaster.ConditionalityReview = null;
            projectMaster.DSOMarker = null;
            projectMaster.ComponentMasters = null;
            projectMaster.Reports = null;
            projectMaster.Teams = null;
            projectMaster.TeamExternals = null;
            projectMaster.Performance = null;
            projectMaster.Deferral = null;
            projectMaster.AuditedFinancialStatements = null;
            projectMaster.ProjectBudgetCentreOrgUnits = null;
            projectMaster.Evaluations = null;
            projectMaster.ReviewMasters = null;

            //Define a Mapping between ProjectMasterViewModel and the Project Master
            Mapper.CreateMap<ProjectMaster, ProjectMaster>()
                .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

            //Do the mapping
            Mapper.Map(projectMaster, currentProjectMaster);
        }

        public bool UpdatePerformanceForExemption(ReviewExemption reviewExemption, string reviewType, string user)
        {
            try
            {
                Performance PerformanceToUpdate = new Performance();
                ExemptionReason SingleExemptionReason = new ExemptionReason();

                PerformanceToUpdate = _projectrepository.GetProjectPerformance(reviewExemption.ProjectID);

                if (reviewType.ToUpper() == "ANNUAL REVIEW")
                {
                    SingleExemptionReason = _projectrepository.GetSingleExemptionReason(reviewExemption.ExemptionReason, "AR");

                    PerformanceToUpdate.ARRequired = "No";
                    PerformanceToUpdate.ARDueDate = null;
                    PerformanceToUpdate.ARPromptDate = null;
                    PerformanceToUpdate.ARExcemptReason = SingleExemptionReason.ExemptionReason1.ToString();
                    PerformanceToUpdate.ARExemptJustification = AMPUtilities.CleanText(reviewExemption.SubmissionComment);
                }
                else
                {
                    SingleExemptionReason = _projectrepository.GetSingleExemptionReason(reviewExemption.ExemptionReason, "PCR");

                    PerformanceToUpdate.PCRRequired = "No";
                    PerformanceToUpdate.PCRDueDate = null;
                    PerformanceToUpdate.PCRPrompt = null;
                    PerformanceToUpdate.PCRExcemptReason = SingleExemptionReason.ExemptionReason1.ToString();
                    PerformanceToUpdate.PCRExemptJustification = AMPUtilities.CleanText(reviewExemption.SubmissionComment);
                }
                PerformanceToUpdate.LastUpdated = DateTime.Now;
                PerformanceToUpdate.UserID = user;

                _projectrepository.UpdatePerformance(PerformanceToUpdate);
                return true;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(reviewExemption.ProjectID, ex, user);
                return false;
            }
        }

        public bool UpdateARPCRBasicInfo(PerformanceVM editPerformanceVm, string user)
        {
            try
            {
                Performance newPerformanceBasic = new Performance();
                newPerformanceBasic.ProjectID = editPerformanceVm.ProjectID;
                newPerformanceBasic.ARRequired = editPerformanceVm.ARRequired;
                newPerformanceBasic.ARDueDate = new DateTime(editPerformanceVm.ARDueDate_Year, editPerformanceVm.ARDueDate_Month, editPerformanceVm.ARDueDate_Day);
                newPerformanceBasic.ARPromptDate = new DateTime(editPerformanceVm.ARPromptDate_Year, editPerformanceVm.ARPromptDate_Month, editPerformanceVm.ARPromptDate_Day);
                newPerformanceBasic.PCRRequired = editPerformanceVm.PCRRequired;
                newPerformanceBasic.PCRDueDate = new DateTime(editPerformanceVm.PCRDueDate_Year, editPerformanceVm.PCRDueDate_Month, editPerformanceVm.PCRDueDate_Day);
                newPerformanceBasic.PCRPrompt = new DateTime(editPerformanceVm.PCRPrompt_Year, editPerformanceVm.PCRPrompt_Month, editPerformanceVm.PCRPrompt_Day);

                _projectrepository.UpdateARPCRBasicInfo(newPerformanceBasic);
                _projectrepository.Save();

                return true;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(editPerformanceVm.ProjectID, ex, user);
                return false;
            }

        }

        public bool UpdatePerformanceForDeferral(ReviewDeferral reviewDeferral, string reviewType, string user)
        {
            try
            {
                Performance PerformanceToUpdate = new Performance();

                PerformanceToUpdate = _projectrepository.GetProjectPerformance(reviewDeferral.ProjectID);

                if (reviewType == "Annual Review")
                {
                    PerformanceToUpdate.ARDueDate = reviewDeferral.PreviousReviewDate.Value.AddMonths(Convert.ToInt16(reviewDeferral.DeferralTimescale));
                    PerformanceToUpdate.ARPromptDate = PerformanceToUpdate.ARPromptDate.Value.AddMonths(Convert.ToInt16(reviewDeferral.DeferralTimescale));
                }
                else
                {
                    PerformanceToUpdate.PCRDueDate = reviewDeferral.PreviousReviewDate.Value.AddMonths(Convert.ToInt16(reviewDeferral.DeferralTimescale));
                    PerformanceToUpdate.PCRPrompt = PerformanceToUpdate.PCRPrompt.Value.AddMonths(Convert.ToInt16(reviewDeferral.DeferralTimescale));
                }
                PerformanceToUpdate.LastUpdated = DateTime.Now;
                PerformanceToUpdate.UserID = user;

                _projectrepository.UpdatePerformance(PerformanceToUpdate);
                _projectrepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(reviewDeferral.ProjectID, ex, user);
                return false;
            }
        }

        public async Task<bool> UpdateStageForReviewDeferral(ReviewDeferralVM reviewDeferralVm, string reviewType, string user, string reviewsURL)
        {
            try
            {
                ReviewDeferral existingReviewDeferral = new ReviewDeferral();
                existingReviewDeferral = _projectrepository.GetReviewDeferral(reviewDeferralVm.ProjectID, reviewDeferralVm.ReviewID);



                if (reviewDeferralVm.Approved == "1")
                {
                    //Performance performance = new Performance();
                    //performance = projectrepository.GetProjectPerformance(reviewDeferralVm.ProjectID.ToString());

                    existingReviewDeferral.StageID = "2";
                    existingReviewDeferral.Approved = "1";
                    existingReviewDeferral.ApproverComment = AMPUtilities.CleanText(reviewDeferralVm.ApproverComment);

                    if (UpdatePerformanceForDeferral(existingReviewDeferral, reviewType, user)) //IF APPROVED THEN UPDATE PERFORMANCE AR/PCR DUE DATE
                    {
                        SendDeferralEmails(existingReviewDeferral, string.Empty, user, reviewsURL, 2); //Approved 
                    }

                }
                else
                {
                    existingReviewDeferral.StageID = "3";
                    existingReviewDeferral.Approved = "0";
                    existingReviewDeferral.ApproverComment = AMPUtilities.CleanText(reviewDeferralVm.ApproverComment);
                    SendDeferralEmails(existingReviewDeferral, string.Empty, user, reviewsURL, 3); //Rejected
                }

                _projectrepository.UpdateReviewDeferral(existingReviewDeferral);
                _projectrepository.Save();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateReviewStage(ReviewVM reviewVm, string user, string reviewsURL)
        {
            try
            {
                ReviewMaster reviewMaster = new ReviewMaster();
                reviewMaster.ReviewType = reviewVm.ReviewType;

                if (reviewVm.StageID == "1") //someone in the team has submitted review and stage goes from 'In Preparation'/'Rejected' to 'Awaiting Approval'
                {
                    reviewMaster.StageID = "1";
                    reviewMaster.SubmissionComment = AMPUtilities.CleanText(reviewVm.SubmissionComment);
                    reviewMaster.Approver = reviewVm.Approver;
                    reviewMaster.Requester = user;
                    reviewMaster.Approved = "0"; //Not Approved yet
                }
                else //Approver has approved/rejected the review
                {
                    if (reviewVm.IsApproved == "Y")
                    {
                        reviewMaster.StageID = "2";
                        reviewMaster.ApproveComment = AMPUtilities.CleanText(reviewVm.ApproveComment);
                        reviewMaster.Approved = "1"; //Approved
                    }
                    else
                    {
                        reviewMaster.StageID = "3";
                        reviewMaster.ApproveComment = AMPUtilities.CleanText(reviewVm.ApproveComment);
                        reviewMaster.Approved = "0"; //Rejected
                    }
                }

                reviewMaster.ReviewID = reviewVm.ReviewID;
                reviewMaster.ProjectID = reviewVm.ProjectID;

                reviewMaster.UserID = user;
                UpdateReviewStage(reviewMaster);
                _projectrepository.Save();

                if (reviewVm.IsApproved == "Y" && reviewVm.ReviewType == "Project Completion Review")
                {
                    //Close Project
                    CloseProject(reviewVm.ProjectID, user);
                }

                //send emails
                await SendReveiwEmails(reviewVm, reviewMaster.StageID, user, reviewsURL);
                return true;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(reviewVm.ProjectID, ex, user);
                return false;
            }

        }

        private string GetTemmembersEmails(IEnumerable<PersonDetails> personDetailsList)
        {
            string cc = "";
            bool firstElement = true;
            if (personDetailsList != null)
            {
                foreach (PersonDetails personDetail in personDetailsList)
                {
                    if (firstElement) //add email without a comma
                    {
                        cc = personDetail.Email;
                        firstElement = false;
                    }
                    else
                    {
                        cc = cc + "," + personDetail.Email;
                    }

                }
            }
            return cc;
        }



        private void UpdateReviewStage(ReviewMaster newReviewMaster)
        {
            try
            {
                ReviewMaster existingReviewMaster = _projectrepository.GetReview(newReviewMaster.ProjectID, newReviewMaster.ReviewID);
                existingReviewMaster.StageID = newReviewMaster.StageID;
                existingReviewMaster.ApproveComment = newReviewMaster.ApproveComment;
                existingReviewMaster.Approved = newReviewMaster.Approved;


                if (newReviewMaster.StageID == "1")
                {
                    existingReviewMaster.Requester = newReviewMaster.Requester;
                    existingReviewMaster.Approver = newReviewMaster.Approver;
                    existingReviewMaster.SubmissionComment = newReviewMaster.SubmissionComment;
                    existingReviewMaster.Approved = "0"; //Not Approved - In Awaiting Approval stage
                }

                if (newReviewMaster.StageID == "2" && newReviewMaster.ReviewType == "Annual Review") // if approved
                {
                    existingReviewMaster.Approved = "1";//Approved
                    existingReviewMaster.ReviewDate = DateTime.Today; //Review approved date
                    //change the next AR due date in performance table:
                    Performance existingPerformance = _projectrepository.GetProjectPerformance(newReviewMaster.ProjectID);
                    TimeSpan dateDiffActualAndDue = existingPerformance.ARDueDate.Value - DateTime.Today;
                    int dateDiffinDays = dateDiffActualAndDue.Days;
                    if (dateDiffinDays >= 0 && dateDiffinDays <= 21)  //Business Rule: done withn 3 weeks of AR due date the next due date will not be changed
                    {
                        existingPerformance.ARDueDate = existingPerformance.ARDueDate.Value.AddYears(1);
                    }
                    else
                    {
                        existingPerformance.ARDueDate = DateTime.Today.AddYears(1);
                    }
                    existingPerformance.ARPromptDate = existingPerformance.ARDueDate.Value.AddMonths(-3);
                }

                existingReviewMaster.Status = "A";
                existingReviewMaster.LastUpdated = DateTime.Now;
                existingReviewMaster.UserID = newReviewMaster.UserID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool DeleteAnnualReview(ReviewVM reviewVm, string user)
        {
            string projectId = reviewVm.ProjectID;

            try
            {
                Int32 reviewId = reviewVm.ReviewID;

                //Delete the Review from ReviewARScore
                ReviewARScore reviewARScoreToRemove = _projectrepository.GetReviewARScore(projectId, reviewId);
                _projectrepository.DeleteReviewARScore(reviewARScoreToRemove);
                //Delete the Review from ReviewOutputs
                IEnumerable<ReviewOutput> reviewOutputs = _projectrepository.GetReviewOutputs(projectId, reviewId);
                _projectrepository.DeleteReviewOutputs(reviewOutputs);
                //Delete the Review Documents
                IEnumerable<ReviewDocument> reviewDocuments = _projectrepository.GetReviewDocuments(projectId, reviewId);
                _projectrepository.DeleteReviewDocuments(reviewDocuments);

                //Delete Deferral if any 
                ReviewDeferral reviewDeferralExisting = _projectrepository.GetReviewDeferral(projectId, reviewId);
                if (reviewDeferralExisting != null)
                {
                    _projectrepository.DeleteReviewDeferral(reviewDeferralExisting.DeferralID);
                }
                //Delete the Review from ReviewMaster
                ReviewMaster reviewMaster = _projectrepository.GetReview(projectId, reviewId);
                _projectrepository.DeleteReviewMaster(reviewMaster);

                _projectrepository.Save();
                return true;
            }
            catch (Exception exception)
            {

                _errorengine.LogError(projectId, exception, user);
                return false;

            }
        }

        public bool DeletePCR(ReviewPCRScoreVM reviewPCRScoreVm, string user)
        {
            string projectId = reviewPCRScoreVm.ProjectID;


            try
            {
                Int32 reviewId = reviewPCRScoreVm.ReviewID;
                //Delete the Review from ReviewPCRScore
                ReviewPCRScore reviewPCRScoreToRemove = _projectrepository.GetReviewPCRScore(projectId, reviewId);
                _projectrepository.DeleteReviewPCRScore(reviewPCRScoreToRemove);
                //Delete the Review from ReviewOutputs
                IEnumerable<ReviewOutput> reviewOutputs = _projectrepository.GetReviewOutputs(projectId, reviewId);
                _projectrepository.DeleteReviewOutputs(reviewOutputs);
                //Delete the Review Documents
                IEnumerable<ReviewDocument> reviewDocuments = _projectrepository.GetReviewDocuments(projectId, reviewId);
                _projectrepository.DeleteReviewDocuments(reviewDocuments);
                //Delete the Review from ReviewMaster
                ReviewMaster reviewMaster = _projectrepository.GetReview(projectId, reviewId);
                _projectrepository.DeleteReviewMaster(reviewMaster);
                //Delete Deferral if any 
                ReviewDeferral reviewDeferralExisting = _projectrepository.GetReviewDeferral(projectId, reviewId);
                if (reviewDeferralExisting != null)
                {
                    _projectrepository.DeleteReviewDeferral(reviewDeferralExisting.DeferralID);
                }

                _projectrepository.Save();

                return true;

            }
            catch (Exception exception)
            {
                _errorengine.LogError(projectId, exception, user);
                return false;
            }

        }

        public async Task<bool> ChangeReviewApprover(string projectId, int reviewId, string newApproverId, string user, string pageUrl)
        {
            try
            {
                ReviewMaster existingReviewMaster = _projectrepository.GetReview(projectId, reviewId);

                //To pass in the SendReviewEmails method new instance of ReviewVM has been created and assigned project ID and reveiw ID into this.
                ReviewVM reviewVm = new ReviewVM();
                reviewVm.ProjectID = projectId;
                reviewVm.ReviewID = reviewId;

                Team sro = _projectrepository.GetTeam(reviewVm.ProjectID).FirstOrDefault(x => x.RoleID == "SRO" && x.Status == "A");

                if (sro == null)
                {
                    existingReviewMaster.Approver = newApproverId.Trim();
                    _projectrepository.Save();

                    return false;
                }
                else
                {
                    //existingReviewMaster.Approver = newApproverId.Trim();
                    if (!(string.IsNullOrEmpty(existingReviewMaster.Approver)))
                    {
                        await SendReveiwEmails(reviewVm, "4", user, pageUrl); //Instead of 1. where 4 = Approver changed
                    }

                    existingReviewMaster.Approver = newApproverId.Trim();
                    _projectrepository.Save();

                    await SendReveiwEmails(reviewVm, "1", user, pageUrl); //Send the new approver to take action email

                    return true;


                }
            }
            catch (Exception ex)
            {
                _errorengine.LogError(projectId, ex, user);
                return false;
            }
        }

        private AMPUtilities.AMPEmail returnEmailContainer(PersonDetails approver, AMPUtilities.EmailPeople peopleToEmail, string subject, string body)
        {
            string from = AMPUtilities.SenderEmail();
            string to = approver.Email;
            string cc = GetTemmembersEmails(peopleToEmail.projectTeam);

            AMPUtilities.AMPEmail EmailContainer = new AMPUtilities.AMPEmail();

            EmailContainer.To = to;
            EmailContainer.Cc = cc;
            EmailContainer.Bcc = "";
            EmailContainer.From = @from;
            EmailContainer.Subject = subject;
            EmailContainer.Body = body;
            EmailContainer.Priority = System.Net.Mail.MailPriority.High;
            return EmailContainer;
        }

        private async Task<AMPUtilities.EmailPeople> returnPeopleToEmail(string projectId, string recipent, string user)
        {
            string cc;
            AMPUtilities.EmailPeople emailPeople = new AMPUtilities.EmailPeople();

            IEnumerable<Team> projectTeam = _projectrepository.GetTeam(projectId);

            IEnumerable<String> empNoList = projectTeam.Select(x => x.TeamID).Distinct();

            Team TeamSRO = projectTeam.FirstOrDefault(x => x.RoleID == "SRO");

            string sroEmpNo = "";

            if (TeamSRO != null)
            {
                sroEmpNo = TeamSRO.TeamID;
            }
            //projectTeam.FirstOrDefault(x => x.RoleID == "SRO").TeamID;
            IEnumerable<PersonDetails> teamDetails = await _personService.GetPeopleDetails(empNoList);
            cc = GetTemmembersEmails(teamDetails);

            PersonDetails recipient = await _personService.GetPersonDetails(recipent);
            PersonDetails sender = await _personService.GetPersonDetails(user);
            PersonDetails sro = new PersonDetails();

            if (sroEmpNo != "")
            {
                sro = teamDetails.FirstOrDefault(x => x.EmpNo.Trim() == sroEmpNo);
            }

            emailPeople.Recipient = recipient;
            emailPeople.SRO = sro;
            emailPeople.Sender = sender;
            emailPeople.projectTeam = teamDetails;

            return emailPeople;
        }

        public bool UpdateProjectMarkers(ProjectMarkersVM projectMarkersVm, IValidationDictionary validationDictionary, string user)
        {

            try
            {


                Markers1 projectMarkers = _projectrepository.GetProjectMarkers(projectMarkersVm.ProjectHeader.ProjectID);

                //disability markers and % field
                if (projectMarkersVm.DisabilityCCO != null && !string.IsNullOrEmpty(projectMarkersVm.DisabilityCCO.SelectedCCOValue))
                {
                    // first check to see if the percentage value is entered and valid if SIGNIFICANT marker selected for disability inclusiuon
                    if (!ValidateDisabilityMarkerPercentage(projectMarkersVm, validationDictionary))
                    {
                        return false;
                    }
                    else
                    {
                        projectMarkers.Disability = projectMarkersVm.DisabilityCCO.SelectedCCOValue;
                    }
                }
                // *** need to get the value of the text box on the markers page and pass it into the project marker
                projectMarkers.DisabilityPercentage = projectMarkersVm.DisabilityPercentage;


                if (projectMarkersVm.HIVCCO != null && !string.IsNullOrEmpty(projectMarkersVm.HIVCCO.SelectedCCOValue))
                {
                    projectMarkers.HIVAIDS = projectMarkersVm.HIVCCO.SelectedCCOValue;
                }

                if (projectMarkersVm.GenderCCO != null && !string.IsNullOrEmpty(projectMarkersVm.GenderCCO.SelectedCCOValue))
                {
                    projectMarkers.GenderEquality = projectMarkersVm.GenderCCO.SelectedCCOValue;
                }

                if (projectMarkersVm.BiodiversityCCO != null && !string.IsNullOrEmpty(projectMarkersVm.BiodiversityCCO.SelectedCCOValue))
                {
                    projectMarkers.Biodiversity = projectMarkersVm.BiodiversityCCO.SelectedCCOValue;
                }

                if (projectMarkersVm.MitigationCCO != null && !string.IsNullOrEmpty(projectMarkersVm.MitigationCCO.SelectedCCOValue))
                {
                    projectMarkers.Mitigation = projectMarkersVm.MitigationCCO.SelectedCCOValue;
                }

                if (projectMarkersVm.AdaptationCCO != null && !string.IsNullOrEmpty(projectMarkersVm.AdaptationCCO.SelectedCCOValue))
                {
                    projectMarkers.Adaptation = projectMarkersVm.AdaptationCCO.SelectedCCOValue;
                }

                if (projectMarkersVm.DesertificationCCO != null && !string.IsNullOrEmpty(projectMarkersVm.DesertificationCCO.SelectedCCOValue))
                {
                    projectMarkers.Desertification = projectMarkersVm.DesertificationCCO.SelectedCCOValue;
                }


                projectMarkers.UserID = user;
                projectMarkers.LastUpdated = DateTime.Now;

                _projectrepository.UpdateProjectMarkers(projectMarkers);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(projectMarkersVm.ProjectHeader.ProjectID, exception, user);
                return false;
            }
        }


        public bool AddPlannedEndDate(WorkflowPlannedEndDateVM workflowPlannedEndDateVM, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                // validate the dates entered
                if (!PlannedEndDateIsValid(workflowPlannedEndDateVM, validationDictionary))
                    return false;

                //So the day, month and year are valid values. 
                // /Build date to be added on Next
                DateTime NewPlannedDate = new DateTime(workflowPlannedEndDateVM.NewPlannedEndDate_Year,
                 workflowPlannedEndDateVM.NewPlannedEndDate_Month, workflowPlannedEndDateVM.NewPlannedEndDate_Day);

                ProjectPlannedEndDate newPlannedEndDates = new ProjectPlannedEndDate();
                // do a check first to see if there are any rows already in table for project with status of A
                // as we need to close them off - can't have any orphans hanging about!
                _projectrepository.CloseOffExistingActiveProjectPlannedEndDate(workflowPlannedEndDateVM.ProjectHeaderVm.ProjectID);

                // ok so we have handled any orphans now we set up and isert the new row
                newPlannedEndDates.ProjectID = workflowPlannedEndDateVM.ProjectHeaderVm.ProjectID;
                newPlannedEndDates.CurrentPlannedEndDate = workflowPlannedEndDateVM.ExistingPlannedEndDate;
                newPlannedEndDates.NewPlannedEndDate = NewPlannedDate;
                newPlannedEndDates.Status = "A";
                newPlannedEndDates.LastUpdate = DateTime.Now;
                newPlannedEndDates.UserID = user;
                // will need to save the workflowID for this but has the workflow Id hasn't been generated so will
                // perform an update on the planned end date table when sending for approval

                //insert into the repository
                _projectrepository.InsertPlannedEndDate(newPlannedEndDates);

                //save
                _projectrepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(ex, user);
                return false;
            }


        }



        #endregion 

        #region Component Related Methods

        public async Task<ProjectComponentVM> GetComponents(string ProjectID, string user)
        {
            //Get the project master data and populate the ViewModel
            ProjectComponentVM projectComponentVm = new ProjectComponentVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);

            if (project.ProjectDate.OperationalEndDate < DateTime.Now)
            {
                projectComponentVm.ProjectPastEndDate = "true";
            }
            else
            {
                projectComponentVm.ProjectPastEndDate = "false";
            }

            // Check For PI
            IEnumerable<Team> projectTeam = _projectrepository.GetTeam(project.ProjectID);

            int InputterExists = 0;

            foreach (Team team in projectTeam.Where(x => x.Status == "A"))
            {

                if (team.RoleID == "PI")
                {
                    InputterExists = 1;
                }

            }

            if (InputterExists != 1)
            {
                projectComponentVm.InputterPresent = "false";
            }
            else
            {
                projectComponentVm.InputterPresent = "true";
            }

            if (project != null)
            {
                //Get Component data for this project
                projectComponentVm.ComponentMaster = project.ComponentMasters;

                projectComponentVm.ProjectHeader = ReturnProjectHeaderVm(project, user);

                ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

                projectwfvm = await IsProjectinWorkflow(project, user);

                projectComponentVm.ProjectWfCheckVm = projectwfvm;

                return projectComponentVm;

            }
            else
            {
                //The project doesn't exist. return a null to the Project Controller which will throw an HTTPNotFound Exception.
                return null;
            }

        }

        /// <summary>GetComponent - Method to get a single detailed Component record </summary>
        /// <param name="ComponentID">6 digit number - 3 digit number (passed as a string) which uniquely identifies the component.</param>
        /// <returns>A ViewModel of type ComponentVM.</returns>
        /// <remarks>Returns a ComponentVM ViewModel</remarks>
        public async Task<ComponentVM> GetComponentEdit(string ComponentID, string User)
        {
            //New View model
            ComponentVM componentVm = new ComponentVM();

            ComponentMaster component = ReturnComponentMaster(ComponentID);



            if (component != null)
            {
                //Populate the Component ViewModel with data from Component Master and it's child objects.
                componentVm = ReturnComponentVM(component);

                //Add component dates.
                componentVm.ComponentDate = PopulateViewModelWithComponentDates(component);

                //Add the Component Header
                componentVm.ComponentHeader = ReturnComponentHeaderVm(component);

                //Get the list of available funding mechs
                componentVm.FundingMechs = _projectrepository.LookUpFundingMechs();

                //Get the list of available partner organisation 
                componentVm.PartnerOrganisations = _projectrepository.LookUpPartnerOrganisations();

                //Get list of funding arrangement
                componentVm.FundingArrangements = _projectrepository.LookUpFundingArrangements();

                //Update Project stage and check for project being passed end date
                ProjectMaster project = ReturnProjectMaster(component.ProjectID);
                componentVm.ProjectStage = project.Stage1.StageID;

                if (project.ProjectDate.FinancialEndDate < DateTime.Now)
                {
                    componentVm.ProjectPastEndDate = "true";
                }
                else
                {
                    componentVm.ProjectPastEndDate = "false";
                }
                componentVm.Approved = component.Approved ?? "";
                componentVm.BudgetCentreDescription = componentVm.BudgetCentreDescription + " - " + componentVm.BudgetCentreID;

                //Check if the project has an active workflow.
                componentVm.AnyApprovedBudget = await _ariesService.DoesComponentHaveApprovedBudget(ComponentID, User);

                ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

                projectwfvm = await IsProjectinWorkflow(project, User);

                componentVm.WFCheck = projectwfvm;
                componentVm.AdminApprover = component.AdminApprover ?? "";

                if (componentVm.AdminApprover.ToString().Trim() != "")
                {

                    PersonDetails person = new PersonDetails();

                    person = await _personService.GetPersonDetails(component.AdminApprover.TrimStart('R'));

                    componentVm.AdminApproverDescription = person.DisplayName;

                }
                else
                {
                    componentVm.AdminApproverDescription = "";
                }



                return componentVm;
            }
            else
            {
                return null;
            }
        }

        public async Task<ComponentMarkersVM> GetComponentMarkers(string ComponentID, string User)
        {

            ComponentMarkersVM componentMarkerVm = new ComponentMarkersVM();

            ComponentMaster component = ReturnComponentMaster(ComponentID);

            if (component != null)
            {
                Mapper.CreateMap<Marker, ComponentMarkersVM>();

                Marker marker = component.Marker;



                componentMarkerVm = Mapper.Map<Marker, ComponentMarkersVM>(marker);

                //Add the Component Header
                componentMarkerVm.ComponentHeader = ReturnComponentHeaderVm(component);

                //Get the list of available benefitting countries
                componentMarkerVm.BenefitingCountrys = _projectrepository.LookUpBenefitingCountrys();

                IEnumerable<ImplementingOrganisation> imporgs = component.ImplementingOrganisations; //projectrepository.GetImplementingOrg(ComponentID);

                IEnumerable<String> supplierList = imporgs.Select(x => x.ImplementingOrganisation1);

                //Call ARIES for supplier names and map them up

                //IEnumerable<SupplierVM> suppliers;

                //suppliers = await _ariesService.GetSuppliers(supplierList, User); // Change this to named method

                //Map Suppliers In for Implementing Organisation
               // componentMarkerVm.ImplementingOrganisation = suppliers;

                //Set stage and check if project is passed end date
                ProjectMaster project = ReturnProjectMaster(component.ProjectID);

                componentMarkerVm.ProjectStage = project.Stage1.StageID;
                componentMarkerVm.BudgetCentreID = project.BudgetCentreID;

                if (DateTime.Today > component.ComponentDate.OperationalEndDate)
                {
                    componentMarkerVm.IsPlannedEndDateOver = "T";
                }
                else
                {
                    componentMarkerVm.IsPlannedEndDateOver = "F";
                }

                if (project.ProjectDate.OperationalEndDate < DateTime.Now)
                {
                    componentMarkerVm.ProjectPastEndDate = "true";
                }
                else
                {
                    componentMarkerVm.ProjectPastEndDate = "false";
                }

                componentMarkerVm.Approved = component.Approved ?? "";
                componentMarkerVm.BenefitingCountry = component.BenefittingCountry;

                ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();


                projectwfvm = await IsProjectinWorkflow(project, User);

                componentMarkerVm.WFCheck = projectwfvm;

                return componentMarkerVm;
            }
            else
            {
                return null;
            }
        }

        public async Task<ComponentVM> GetCreateComponent(string ProjectID, string User)
        {

            ComponentVM componentVm = new ComponentVM();
            ComponentDateVM componentdate = new ComponentDateVM();

            ProjectMaster project = _projectrepository.GetProject(ProjectID);

            Int32 dateCompare = DateTime.Compare(project.ProjectDate.OperationalStartDate.Value.Date, DateTime.Now.Date);
            if (dateCompare < 0)
            {
                componentdate.OperationalStartDate = DateTime.Today;
            }
            else
            {
                componentdate.OperationalStartDate = project.ProjectDate.OperationalStartDate;
            }


            componentdate.OperationalEndDate = project.ProjectDate.OperationalEndDate;


            componentVm.ComponentDate = componentdate;

            componentVm.ProjectID = ProjectID;
            componentVm.FundingMechs = _projectrepository.LookUpFundingMechs();
            //Get the list of available partner organisation 
            componentVm.PartnerOrganisations = _projectrepository.LookUpPartnerOrganisations();
            //Get list of funding arrangement
            componentVm.FundingArrangements = _projectrepository.LookUpFundingArrangements();

            return componentVm;
        }

        private ComponentVM ReturnComponentVM(ComponentMaster component)
        {
            //Auto Mapper blows up on Component Dates. Do this manually instead. C Finnan 23/07/15

            ComponentVM componentVm = new ComponentVM
            {
                ProjectID = component.ProjectMaster.ProjectID,
                ComponentID = component.ComponentID,
                ComponentDescription = component.ComponentDescription,
                BudgetCentreID = component.BudgetCentreID,
                BudgetCentreDescription = component.BudgetCentre.BudgetCentreDescription,
                FundingMechanism = component.FundingMechanism,
                FundingArrangementValue = component.FundingArrangementValue,
                PartnerOrganisationValue = component.PartnerOrganisationValue,
                Approved = component.Approved
            };
            return componentVm;
        }

        /// <summary>GetComponent - Method to get a single detailed Component record </summary>
        /// <param name="ComponentID">6 digit number - 3 digit number (passed as a string) which uniquely identifies the component.</param>
        /// <returns>A ViewModel of type ComponentSectorVM.</returns>
        /// <remarks>Returns a ComponentSectorVM ViewModel</remarks>
        public async Task<ComponentSectorVM> GetSectors(string ComponentID, String User)
        {

            ComponentSectorVM componentSectorVm = new ComponentSectorVM();

            ComponentMaster component = ReturnComponentMaster(ComponentID);


            if (component != null)
            {
                //Map the current Funding Mechanism to the ViewModel
                componentSectorVm.FundingMechanism = component.FundingMechanism;

                //Populate the Component Header information
                componentSectorVm.ComponentHeader = ReturnComponentHeaderVm(component);

                // Map Input Sector codes to view InputSectorVM

                IEnumerable<InputSectorCode> inputSectorCode = component.InputSectorCodes;  //projectrepository.GetInputSectors(ComponentID);

                IEnumerable<InputSectorVM> inputsectorVM;

                //Define a mapping between the InputSectorCode data object and the InputSectorVM ViewModel.
                Mapper.CreateMap<InputSectorCode, InputSectorVM>();

                //Map
                inputsectorVM = Mapper.Map<IEnumerable<InputSectorCode>, IEnumerable<InputSectorVM>>(inputSectorCode);

                // Get and Map Input Sector Code Descriptions
                IEnumerable<InputSector> inputsectorlookup = _projectrepository.LookUpInputSector();

                foreach (InputSectorVM s in inputsectorVM)
                {
                    //Get current row of inputsectorVM from the current position of the for each
                    var sectors = inputsectorVM.FirstOrDefault(x => x.InputSectorCode1.Equals(s.InputSectorCode1));

                    //Get Current Row of inputsectorlookup
                    // Check to see if the result will be null, if it is then there is no sector so dont map, mapping will throw exception
                    if (inputsectorlookup.FirstOrDefault(x => x.InputSectorCodeID.Equals(s.InputSectorCode1)) != null)
                    {
                        var sectorsdescription = inputsectorlookup.FirstOrDefault(x => x.InputSectorCodeID.Equals(s.InputSectorCode1));

                        // Update the input sector description to the inputsectorVM model.
                        sectors.InputSectorCodeDescription = sectorsdescription.InputSectorCodeDescription;
                    }

                }

                //Append inputsectorVM to the ComponentViewModel
                componentSectorVm.InputSectors = inputsectorVM;
                componentSectorVm.Approved = component.Approved ?? "";

                //Set stage of the project
                ProjectMaster projectstage = ReturnProjectMaster(component.ProjectID);

                componentSectorVm.ProjectStage = projectstage.Stage1.StageID;
                componentSectorVm.FundingMechs = _projectrepository.LookUpFundingMechs();

                //Check if the project has an active workflow.
                componentSectorVm.AnyApprovedBudget = await _ariesService.DoesComponentHaveApprovedBudget(ComponentID, User);

                ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

                ProjectMaster project = _projectrepository.GetProject(componentSectorVm.ComponentHeader.ProjectID);

                projectwfvm = await IsProjectinWorkflow(project, User);

                componentSectorVm.WFCheck = projectwfvm;

                //Get Current Team
                IEnumerable<Team> currentTeam = _projectrepository.GetTeam(project.ProjectID);

                componentSectorVm.IsTeamMember = IsTeamMember(User, currentTeam);

                return componentSectorVm;

            }
            else
            {
                return null;
            }
        }

        public ComponentViewModel PopulateViewModelWithComponentMasterData(String Component)
        {
            ComponentViewModel componentViewModel = new ComponentViewModel();

            ComponentMaster component = _projectrepository.GetComponent(Component);

            if (component != null)
            {
                MapComponentMasterTComponentMasterViewModel(componentViewModel, component);

                //Populate static project data into the ViewModel
                PopulateViewModelWithComponentStaticData(componentViewModel, component);

                //Get the project dates
                PopulateViewModelWithComponentDates(component);

                DecomposeComponentDateTimesToDateParts(componentViewModel);
            }
            return componentViewModel;
        }

        //Component Static Helper Methods
        private static void DecomposeComponentDateTimesToDateParts(ComponentViewModel componentViewModel)
        {
            DateTime date = new DateTime();

            date = componentViewModel.ComponentDate.StartDate ?? DateTime.Now;
            componentViewModel.ComponentDate.OperationalStartDate_Day = date.Day;
            componentViewModel.ComponentDate.OperationalStartDate_Month = date.Month;
            componentViewModel.ComponentDate.OperationalStartDate_Year = date.Year;

            date = componentViewModel.ComponentDate.EndDate ?? DateTime.Now;
            componentViewModel.ComponentDate.OperationalEndDate_Day = date.Day;
            componentViewModel.ComponentDate.OperationalEndDate_Month = date.Month;
            componentViewModel.ComponentDate.OperationalEndDate_Year = date.Year;
        }


        private static void MapComponentMasterTComponentMasterViewModel(ComponentViewModel componentViewModel, ComponentMaster component)
        {
            ComponentMasterVM componentMasterVM = new ComponentMasterVM();

            //Define a map between the ProjectMaster object and the ProjectMasterVM
            Mapper.CreateMap<ComponentMaster, ComponentMasterVM>();

            //Map the objects
            Mapper.Map(component, componentMasterVM);

            componentViewModel.ComponentMaster = componentMasterVM;
        }

        private static void PopulateViewModelWithComponentStaticData(ComponentViewModel componentViewModel, ComponentMaster component)
        {
            ComponentStaticData staticComponentData = new ComponentStaticData();

            staticComponentData.BudgetCentreDescription = component.BudgetCentre.BudgetCentreDescription;

            //staticComponentData.InputterName = component.User.UserName;

            componentViewModel.ComponentStaticData = staticComponentData;
        }

        private ComponentDateVM PopulateViewModelWithComponentDates(ComponentMaster component)
        {
            ComponentDateVM componentDateVM = new ComponentDateVM();

            //Define a mapping between the ProjectDate data object and the ProjectDateVM ViewModel.
            Mapper.CreateMap<ComponentDate, ComponentDateVM>();

            //Do the mapping.
            Mapper.Map(component.ComponentDate, componentDateVM);

            DateTime date = new DateTime();

            date = componentDateVM.StartDate ?? DateTime.Now;
            componentDateVM.OperationalStartDate_Day = date.Day;
            componentDateVM.OperationalStartDate_Month = date.Month;
            componentDateVM.OperationalStartDate_Year = date.Year;

            date = componentDateVM.EndDate ?? DateTime.Now;
            componentDateVM.OperationalEndDate_Day = date.Day;
            componentDateVM.OperationalEndDate_Month = date.Month;
            componentDateVM.OperationalEndDate_Year = date.Year;

            return componentDateVM;
        }

        // Component Update Methods
        private static bool ComponentDatesAreValid(ComponentDateVM componentDateVm, IValidationDictionary validationDictionary)
        {
            if (!IsValidDate(componentDateVm.OperationalStartDate_Day, componentDateVm.OperationalStartDate_Month, componentDateVm.OperationalStartDate_Year, "ComponentDate.OperationalStartDate", validationDictionary))
                return validationDictionary.IsValid;
            if (!IsValidDate(componentDateVm.OperationalEndDate_Day, componentDateVm.OperationalEndDate_Month, componentDateVm.OperationalEndDate_Year, "ComponentDate.OperationalEndDate", validationDictionary))
                return validationDictionary.IsValid;

            return validationDictionary.IsValid;

        }

        public bool UpdateComponent(ComponentVM componentVm, String FundingMechdd, IValidationDictionary validationDictionary, String user)
        {
            try
            {

                //Get the current ComponentMaster Data ready for mapping
                ComponentMaster currentComponentMaster = _projectrepository.GetComponent(componentVm.ComponentID);
                ComponentDate currentcomponentdates = currentComponentMaster.ComponentDate;

                ComponentMaster componentMaster = new ComponentMaster();

                string stageId = currentComponentMaster.ProjectMaster.Stage;

                // Validate dates
                //Validate that the day, month and year are valid values. 

                if (stageId != Constants.CompletionStage)
                {
                    if (!ComponentDatesAreValid(componentVm.ComponentDate, validationDictionary))
                        return false;
                }

                if (stageId != Constants.CompletionStage)
                {
                    //Start date is set once on create always override with whats in the database to by pass validation
                    //componentVm.ComponentDate.OperationalStartDate = currentcomponentdates.OperationalStartDate;
                    componentVm.ComponentDate.OperationalStartDate = new DateTime(componentVm.ComponentDate.OperationalStartDate_Year, componentVm.ComponentDate.OperationalStartDate_Month, componentVm.ComponentDate.OperationalStartDate_Day);
                    componentVm.ComponentDate.OperationalEndDate = new DateTime(componentVm.ComponentDate.OperationalEndDate_Year, componentVm.ComponentDate.OperationalEndDate_Month, componentVm.ComponentDate.OperationalEndDate_Day);

                    componentVm.ComponentDate.EndDate = componentVm.ComponentDate.OperationalEndDate.Value.AddMonths(6);

                }
                else
                {
                    //Leave the dates as they are.
                    componentVm.ComponentDate.OperationalStartDate = currentcomponentdates.OperationalStartDate;
                    componentVm.ComponentDate.OperationalEndDate = currentcomponentdates.OperationalEndDate;
                }

                //If funding mech is different delete sectors:
                string updatedSector = FundingMechdd ?? componentVm.FundingMechanism;

                if (currentComponentMaster.FundingMechanism != updatedSector)
                {
                    if (currentComponentMaster.InputSectorCodes.Count() != 0)
                    {

                        //Delete all sectors
                        foreach (InputSectorCode sector in currentComponentMaster.InputSectorCodes.ToList())
                        {
                            _projectrepository.DeleteSector(sector);
                        }
                    }
                }

                //This line should be removed I think
                currentComponentMaster.FundingMechanism = updatedSector;

                componentVm.FundingMechanism = updatedSector;


                //Main Validation method
                if (!ValidateCreateComponent(componentVm, validationDictionary))
                    return false;

                //HACK - Set ComponentMaster and Portfolio to null - otherwise they Auto Map as zero and the deletes the existing Components and entries in the Portfolio table.
                //HACK - Need to look at a better solution long term - CJF 07/01/2015.

                componentMaster.InputSectorCodes = null;
                componentMaster.ImplementingOrganisations = null;
                componentMaster.AdminApprover = componentVm.AdminApprover ?? "";
                componentMaster.DeliveryChains = null;
                //More Auto Mapper woe! The mapping doesn't work between the new ComponentVm and the ComponentMaster. Manual mapping requried.
                //Define a mapping between ComponentMasterVM.ComponentMasterVM and a generic ComponentMaster
                //Mapper.CreateMap<ComponentMasterVM, ComponentMaster>();

                //Mapper.Map(componentVm, componentMaster);

                //Map the component description and BudgetCentreID. 
                currentComponentMaster.ComponentDescription = AMPUtilities.CleanText(componentVm.ComponentDescription);
                currentComponentMaster.BudgetCentreID = componentVm.BudgetCentreID;

                //Define a Mapping between ComponentMasterViewModel and the ComponentMaster
                Mapper.CreateMap<ComponentMaster, ComponentMaster>()
                    .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));


                //Do the mapping
                Mapper.Map(componentMaster, currentComponentMaster);



                //Update Funding Mech
                //currentComponentMaster.FundingMechanism = FundingMechdd;

                //Second run for Dates
                Mapper.CreateMap<ComponentDateVM, ComponentDate>();

                //Create a blank project date.
                ComponentDate componentdates = new ComponentDate();

                // Map
                Mapper.Map(componentVm.ComponentDate, componentdates);

                //Second mapping run.
                Mapper.CreateMap<ComponentDate, ComponentDate>().ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull)); ;

                Mapper.Map(componentdates, currentcomponentdates);

                componentdates = currentComponentMaster.ComponentDate;

                //Add FundingArrangement if changed 
                if (currentComponentMaster.FundingArrangementValue != componentVm.FundingArrangementValue)
                {
                    currentComponentMaster.FundingArrangementValue = componentVm.FundingArrangementValue;
                }
                //Add PartnerOrganisation if changed 
                if (currentComponentMaster.PartnerOrganisationValue != componentVm.PartnerOrganisationValue)
                {
                    currentComponentMaster.PartnerOrganisationValue = componentVm.PartnerOrganisationValue;
                }

                // Update then save!
                _projectrepository.UpdateComponent(currentComponentMaster, componentdates);
                _projectrepository.UpdateComponentDate(componentdates);
                _projectrepository.Save();

                return true;

            }
            catch (Exception ex)
            {
                _errorengine.LogError(componentVm.ProjectID, ex, user);
                throw;
            }
        }


        public bool CreateComponent(ComponentVM componentVm, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                ComponentMaster componentMaster = new ComponentMaster();
                ComponentDate componentdate = new ComponentDate();

                //Set start date to today (This is not posted back from the screen as the field is read only)
                //componentVm.ComponentDate.OperationalStartDate_Day = DateTime.Today.Day;
                //componentVm.ComponentDate.OperationalStartDate_Month = DateTime.Today.Month;
                //componentVm.ComponentDate.OperationalStartDate_Year = DateTime.Today.Year;

                // Validate dates
                //**Moved this validation up the list, forming the OpStart and OpEnd Date first opens up a possible exception if one of the date parts wasn't entered (comes through as zero in the VM) - CJF 02/02/2016 **//
                //Validate that the day, month and year are valid values. 
                if (!ComponentDatesAreValid(componentVm.ComponentDate, validationDictionary))
                    return false;

                //Take the day, month and year for each date and put them back together in the ViewModel.
                componentVm.ComponentDate.OperationalStartDate = new DateTime(componentVm.ComponentDate.OperationalStartDate_Year, componentVm.ComponentDate.OperationalStartDate_Month, componentVm.ComponentDate.OperationalStartDate_Day);
                componentVm.ComponentDate.OperationalEndDate = new DateTime(componentVm.ComponentDate.OperationalEndDate_Year, componentVm.ComponentDate.OperationalEndDate_Month, componentVm.ComponentDate.OperationalEndDate_Day);

                //Validations (This one is outside the main validation method as it conflicts with the edit component validation check.)
                //Component Start Date cannot be before today.
                if (componentVm.ComponentDate.OperationalStartDate.Value.Date < DateTime.Now.Date)
                {
                    validationDictionary.AddError("ComponentDate.OperationalStartDate", String.Format("Start date cannot be earlier than today", componentVm.ComponentDate.OperationalStartDate));
                    return validationDictionary.IsValid;
                }

                if (!ValidateCreateComponent(componentVm, validationDictionary))
                    return false;

                //Get all components and fix the max id

                //Removed this method (some components were moved in aries against different projects, finding max based on project id returned incorrect results)
                //IEnumerable<ComponentMaster> componentmaster = projectrepository.GetComponents(componentVm.ProjectID);
                IEnumerable<ComponentMaster> componentmaster = _projectrepository.GetComponentsUsingComponentSubString(componentVm.ProjectID);

                int newComponentID;

                if (componentmaster.FirstOrDefault() == null)
                {
                    newComponentID = 101;
                }
                else
                {
                    string maxComponent = componentmaster.Max(x => x.ComponentID);
                    maxComponent = maxComponent.Substring(7);

                    newComponentID = int.Parse(maxComponent) + 1;

                }

                //Build Dates
                componentdate.OperationalStartDate = componentVm.ComponentDate.OperationalStartDate;
                componentdate.OperationalEndDate = componentVm.ComponentDate.OperationalEndDate;
                componentdate.StartDate = DateTime.Now;
                componentdate.EndDate = componentVm.ComponentDate.OperationalEndDate.Value.AddMonths(6);
                componentdate.LastUpdate = DateTime.Now;
                componentdate.UserID = user;
                componentdate.ComponentID = componentVm.ProjectID + '-' + newComponentID.ToString();

                //Set the current view model with the planned new component id so we can refer straight to the edit page
                componentVm.ComponentID = componentdate.ComponentID;

                //Build Component
                componentMaster.ComponentID = componentVm.ProjectID + '-' + newComponentID.ToString();
                componentMaster.ProjectID = componentVm.ProjectID;
                componentMaster.FundingMechanism = componentVm.FundingMechanism;
                componentMaster.FundingArrangementValue = componentVm.FundingArrangementValue;
                componentMaster.PartnerOrganisationValue = componentVm.PartnerOrganisationValue;
                componentMaster.UserID = user;
                componentMaster.LastUpdate = DateTime.Now;
                componentMaster.BudgetCentreID = componentVm.BudgetCentreID;
                componentMaster.ComponentDescription = AMPUtilities.CleanText(componentVm.ComponentDescription);
                componentMaster.AdminApprover = componentVm.AdminApprover ?? "";
                componentMaster.OperationalStatus = "P";
                componentMaster.Approved = "N";

                //Build Marker object
                Marker marker = new Marker();

                marker.ComponentID = componentMaster.ComponentID;
                marker.UserID = user;
                marker.Status = "A";
                marker.LastUpdate = DateTime.Now;

                //If Admin of any type:
                if (componentVm.BudgetCentreID.StartsWith("A0") || componentVm.BudgetCentreID.StartsWith("C0") || componentVm.BudgetCentreID.StartsWith("AP"))
                {
                    marker.SWAP = "No";
                    marker.PBA = "No";
                    componentMaster.BenefittingCountry = "AC";
                }
                else
                {
                    marker.SWAP = "";
                    marker.PBA = "";

                }
                _projectrepository.AddMarker(marker);

                // Free sectors 
                //if admin insert sector @ 100%
                if (componentVm.BudgetCentreID.StartsWith("A0"))
                {
                    InputSectorCode NewInputSector = new InputSectorCode();

                    //Build Risk Model
                    NewInputSector.ComponentID = componentMaster.ComponentID;
                    NewInputSector.InputSectorCode1 = "91010";
                    NewInputSector.Percentage = 100;
                    NewInputSector.UserID = "028984";
                    NewInputSector.LastUpdate = DateTime.Now;
                    NewInputSector.LineNo = 1; //Increment line number

                    _projectrepository.AddInputSector(NewInputSector);

                }
                //if admin capital insert sector @ 100%
                if (componentVm.BudgetCentreID.StartsWith("C0"))
                {
                    InputSectorCode NewInputSector = new InputSectorCode();

                    //Build Risk Model
                    NewInputSector.ComponentID = componentMaster.ComponentID;
                    NewInputSector.InputSectorCode1 = "91030";
                    NewInputSector.Percentage = 100;
                    NewInputSector.UserID = "028984";
                    NewInputSector.LastUpdate = DateTime.Now;
                    NewInputSector.LineNo = 1; //Increment line number

                    _projectrepository.AddInputSector(NewInputSector);
                }
                //if FLD insert sector @ 100%
                if (componentVm.BudgetCentreID.StartsWith("AP"))
                {
                    InputSectorCode NewInputSector = new InputSectorCode();

                    //Build Risk Model
                    NewInputSector.ComponentID = componentMaster.ComponentID;
                    NewInputSector.InputSectorCode1 = "91020";
                    NewInputSector.Percentage = 100;
                    NewInputSector.UserID = "028984";
                    NewInputSector.LastUpdate = DateTime.Now;
                    NewInputSector.LineNo = 1; //Increment line number

                    _projectrepository.AddInputSector(NewInputSector);
                }



                // Create then save!
                _projectrepository.CreateComponent(componentMaster, componentdate);
                _projectrepository.CreateComponentDate(componentdate);
                _projectrepository.Save();

                return true;

            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(componentVm.ProjectID, ex, user);
                throw;
            }
        }

        public bool AddSector(ComponentSectorVM componentSectorVm, IValidationDictionary validationDictionary)
        {
            //Get Component and current sectors
            ComponentMaster component = _projectrepository.GetComponent(componentSectorVm.ComponentHeader.ComponentID);


            if (!ValidateSector(componentSectorVm, component, validationDictionary))
                return false;



            //Global Sector Count
            int SectorCount;

            // Try required here as if you .Max a null object you get an exception.
            try
            {
                SectorCount = component.InputSectorCodes.Max(x => x.LineNo);
            }
            catch
            {
                SectorCount = 0;
            }

            try
            {
                InputSectorCode NewInputSector = new InputSectorCode();

                //Build Risk Model
                NewInputSector.ComponentID = componentSectorVm.ComponentHeader.ComponentID;
                NewInputSector.InputSectorCode1 = componentSectorVm.NewInputSector.InputSectorCode1;
                NewInputSector.Percentage = componentSectorVm.NewInputSector.Percentage;
                NewInputSector.UserID = "028984";
                NewInputSector.LastUpdate = DateTime.Now;
                NewInputSector.LineNo = SectorCount + 1; //Increment line number

                _projectrepository.AddInputSector(NewInputSector);
                _projectrepository.Save();

                return true;

            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                //  errorengine.LogError(ex);
                throw;
            }
        }

        public bool DeleteSector(string componentid, int sectorcode, string user)
        {
            try
            {
                //Get the sector to be deleted.
                InputSectorCode sector = _projectrepository.GetInputSector(componentid, sectorcode);

                _projectrepository.DeleteSector(sector);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(componentid, exception, user);
                return false;
            }
        }

        public async Task<ComponentFinanceVM> GetComponentFinancials(String ComponentID, string user)
        {

            ComponentFinanceVM componentFinanceVm = new ComponentFinanceVM();

            ComponentMaster component = ReturnComponentMaster(ComponentID);


            if (component != null)
            {


                try
                {
                    //Get the Project Financials for the financials tab
                    componentFinanceVm.ComponentFinance = await PopulateViewModelWithComponentFinancials(ComponentID, user);

                    //Populate the Component Header
                    componentFinanceVm.ComponentHeader = ReturnComponentHeaderVm(component);

                    return componentFinanceVm;

                }
                catch (Exception ex)
                {
                    //Set the Financials to null and return the ViewModel
                    componentFinanceVm.ComponentFinance = new List<ComponentFinanceRecordVM>();

                    //Extract this to a constants file or similar.
                    componentFinanceVm.FinanceWebServiceMessage = "Sorry, ARIES doesn't seem to be available just now. Your finance data cannot be shown.";

                    // Executes the error engine (ProjectID is optional, exception)
                    _errorengine.LogError(ComponentID, ex, user);

                    return componentFinanceVm;

                }

            }
            else
            {
                //The project doesn't exist. return a null to the Project Controller which will throw an HTTPNotFound Exception.
                return null;
            }
        }

        public bool UpdateMarkers(ComponentMarkersVM componentMarkerVm, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                //Validate new data
                if (!ValidateMarkers(componentMarkerVm, validationDictionary))
                    return false;

                //Get the current ComponentMaster Data ready for mapping
                ComponentMaster componenetmaster = _projectrepository.GetComponent(componentMarkerVm.ComponentID);

                Marker currentMarker = componenetmaster.Marker;

                //Update Current Marker
                currentMarker.PBA = componentMarkerVm.PBA ?? "";
                currentMarker.SWAP = componentMarkerVm.SWAP ?? "";

                _projectrepository.UpdateMarker(currentMarker);

                //ImplementingOrganisation imporg = componenetmaster.ImplementingOrganisations.FirstOrDefault();

                //what am i doing here?
                // imporg.ImplementingOrganisation1 = componentVm.();

                //Although not part of this save, ARIES Integration takes dates
                ComponentDate currentcomponentdates = _projectrepository.GetComponentDates(componentMarkerVm.ComponentID);

                //Update the new benefittingCountry
                componenetmaster.BenefittingCountry = componentMarkerVm.BenefitingCountry;

                // Update then save!
                _projectrepository.UpdateComponent(componenetmaster, currentcomponentdates);

                _projectrepository.Save();
                return true;

            }
            catch (Exception ex)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(ex, user);
                throw;
            }
        }
        //****
        #region Delivery chain
      // return a single row from the database for a component - return the details of a specific partner on the chain to use it
      // as the parent for the new partner info to be added
        public async Task<DeliveryChainVM> GetSpecificDeliveryChainRow(int id)
        {
            DeliveryChain deliverychain = _projectrepository.GetDeliveryChain(id);
            DeliveryChainVM deliverychainVm = new DeliveryChainVM();
         
            //Define a map between the DeliveryChain object and theDeliveryChainVM
            Mapper.CreateMap<DeliveryChain, DeliveryChainVM>();
 
            //Map the objects
            Mapper.Map(deliverychain, deliverychainVm);

            // Get the supplier/partner name & add to the vm
            if(deliverychainVm.ChildType == "S")
            {
                deliverychainVm.ChildName = await _ariesService.GetSupplierName(deliverychainVm.ChildID.ToString());
            }

            else if (deliverychainVm.ChildType == "P")
            {
                deliverychainVm.ChildName = _projectrepository.GetPartnerName(deliverychainVm.ChildID);
            }      

            return deliverychainVm;
        }

        public async Task<ComponentPartnerVM> GetComponentDeliveryChains(string componentId, string user)
        {
            ComponentPartnerVM componentPartnerVm = new ComponentPartnerVM();
            ComponentPartnerVMBuilder componentPartnerVmBuilder = new ComponentPartnerVMBuilder(_projectrepository, _ariesService, componentId);

            ComponentMaster componentMaster = _projectrepository.GetComponent(componentId);
            ProjectMaster projectMaster = _projectrepository.GetProject(componentMaster.ProjectID);
            ProjectVM projectVm = await GetProjectVM(componentMaster.ProjectID, user);
            componentPartnerVm = await componentPartnerVmBuilder.Build();

            componentPartnerVm.ComponentHeaderVm = ReturnComponentHeaderVm(componentMaster);
            componentPartnerVm.ProjectHeaderVm = ReturnProjectHeaderVm(projectMaster, user);
            componentPartnerVm.ProjectVm = projectVm;

            return componentPartnerVm;
        }

        // this is called when the user wants to add an existing Alito DFID registered partner as a first tier supplier
        public async Task<bool> InsertFirstTierPartner(string componentId, string supplierId, string user)
        {
            try
            {
                IEnumerable<SupplierVM> suppliers;
                List<String> supplierList = new List<string>();

                supplierList.Add(supplierId);

                suppliers = await _ariesService.GetSuppliers(supplierList, user); // Change this to named method

                SupplierVM supplier = suppliers.FirstOrDefault();

                DeliveryChainBuilder deliveryChainBuilder = new DeliveryChainBuilder(supplier, _projectrepository, _ariesService, componentId, user);

                DeliveryChain deliveryChain = deliveryChainBuilder.BuildFromSupplier();

                ChainList chainList = new ChainList(_projectrepository.GetDeliveryChainsByComponent(componentId).ToList());

                deliveryChain.ChainID = chainList.NextFirstTierPartnerChainID();
                _projectrepository.InsertDeliveryChain(deliveryChain);
                _projectrepository.Save();

                // Retrieves delivery chain in order to determine & add PartnerNodeID
                DeliveryChain retrievedChain = _projectrepository.GetDeliveryChainThatMatchesDeliveryChainVm(deliveryChain);
                retrievedChain.ParentNodeID = retrievedChain.ID;
                _projectrepository.UpdateDeliveryChain(retrievedChain);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(exception, user);
                return false;
            }
        }
        // this is called when the user wants to add an existing Non DFID registered partner as a first tier supplier
        public async Task<bool> InsertFirstTierNonRegisteredPartner(string componentId, string supplierId, string user)
        {
            try
            {
                // here we are setting up the existing partner as a "supplier" entry so we can reuse existing code
                 SupplierVM supplier =new SupplierVM();
                supplier.CountryCode = "";
                supplier.SupplierID = supplierId;
                supplier.SupplierName = _projectrepository.GetPartnerName(int.Parse(supplierId));


                DeliveryChainBuilder deliveryChainBuilder = new DeliveryChainBuilder(supplier, _projectrepository, _ariesService, componentId, user);

                //DeliveryChain deliveryChain = deliveryChainBuilder.BuildFromSupplier();
                DeliveryChain deliveryChain = deliveryChainBuilder.BuildFromPartner();

                ChainList chainList = new ChainList(_projectrepository.GetDeliveryChainsByComponent(componentId).ToList());

                deliveryChain.ChainID = chainList.NextFirstTierPartnerChainID();
                _projectrepository.InsertDeliveryChain(deliveryChain);
                _projectrepository.Save();

                // Retrieves delivery chain in order to determine & add PartnerNodeID
                DeliveryChain retrievedChain = _projectrepository.GetDeliveryChainThatMatchesDeliveryChainVm(deliveryChain);
                retrievedChain.ParentNodeID = retrievedChain.ID;
                _projectrepository.UpdateDeliveryChain(retrievedChain);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(exception, user);
                return false;
            }
        }

        // this is called when the user wants to add an entirely new first tier partner to the chain
        //Need to firstly add the partner to the table in AMP and then add it to the chain

        public async Task<bool> InsertNewFirstTierPartner(string componentId,  string partnerName, string user)
        {
            try
            {

                // Firstly create a delivery chain view model and add the entry to the Partner Table
                DeliveryChainVM newFirstTierPartner = new DeliveryChainVM();

                newFirstTierPartner.ChildType = "P"; //adding a partner
                newFirstTierPartner.NewChildName = partnerName;
                newFirstTierPartner.ChildID = _projectrepository.NextPartnerID();

                //Partner Builder and insertion of the new row into the pratner master table
                PartnerBuilder partnerBuilder = new PartnerBuilder(newFirstTierPartner, _projectrepository, _ariesService,
                    user);
                PartnerMaster partnerMaster = await partnerBuilder.Build();
                _projectrepository.InsertPartner(partnerMaster);
                _projectrepository.Save();

                string newPartnerID = newFirstTierPartner.ChildID.ToString();
                // Now add the new partner to the delivery chain as a first tier partner 
                await InsertFirstTierNonRegisteredPartner(componentId, newPartnerID, user);
                return true;
            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(exception, user);
                return false;
            }
        }

        public  async Task<bool> InsertChain(DeliveryChainVM deliveryChainVm, IValidationDictionary validationDictionary, string userId)
        {
            try
            {
                    DeliveryChainBuilder deliveryChainBuilder = new DeliveryChainBuilder(deliveryChainVm,
                        _projectrepository, _ariesService, userId);

                    DeliveryChain deliveryChain = await deliveryChainBuilder.BuildFromChain();

                    ChainList chainList =
                        new ChainList(
                            _projectrepository.GetDeliveryChainsByComponent(deliveryChainVm.ComponentID).ToList());

                    deliveryChain.ChainID = chainList.NextChainIDFromPartnerHierarchy(deliveryChainVm.ID);

                    _projectrepository.InsertDeliveryChain(deliveryChain);
                    _projectrepository.Save();
                    return true;
              
            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(exception, userId);
                return false;
            }

        }

        public async Task<bool> InsertNewPartnerAndReplaceExistingInChain(DeliveryChainVM deliveryChainVm, IValidationDictionary validationDictionary, string userId)
        {
            try
            {
                DeliveryChainBuilder deliveryChainBuilder = new DeliveryChainBuilder(deliveryChainVm,
                    _projectrepository, _ariesService, userId);

                DeliveryChain deliveryChain = await deliveryChainBuilder.BuildFromChain();

                ChainList chainList =
                    new ChainList(
                        _projectrepository.GetDeliveryChainsByComponent(deliveryChainVm.ComponentID).ToList());

                deliveryChain.ChainID = chainList.NextChainIDFromPartnerHierarchy(deliveryChainVm.ID);
                deliveryChain.ID = deliveryChainVm.ID;
                _projectrepository.ReplacePartnerInDeliveryChain(deliveryChain);
                _projectrepository.Save();
                return true;

            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(exception, userId);
                return false;
            }

        }
        // takes the current partner in the chain and replaces it with a different partner selected by user
        // updates the child ID and Type to the new one selected by the user based on the RowId of the origional partner
        //that is being replaced

        public async Task<bool> ReplacePartnerInChain(DeliveryChain deliveryChainReplaceDetails, IValidationDictionary validationDictionary, string userId)
        {
            try
            {

              DeliveryChain chain = new DeliveryChain();

                //Find the DeliveryChain to be updated with new partner info.
                chain.ID = deliveryChainReplaceDetails.ID;
                chain.ChildID = deliveryChainReplaceDetails.ChildID;
                chain.ChildType = deliveryChainReplaceDetails.ChildType;

                //we need to check to see if first tier partner and if so then update the parentid as well

                if (deliveryChainReplaceDetails.ParentID == deliveryChainReplaceDetails.ChildID)
                {
                    chain.ParentID = deliveryChainReplaceDetails.ChildID;
                    chain.ParentType = deliveryChainReplaceDetails.ChildType;
                }
                _projectrepository.ReplacePartnerInDeliveryChain(chain);
                _projectrepository.Save();
                return true;

            }
            catch (Exception exception)
            {
                // Executes the error engine (ProjectID is optional, exception)
                _errorengine.LogError(exception, userId);
                return false;
            }

        }

        public DeliveryChainVM CreatePartner(string id, string user)
        {
            try
            {

                Int32 intId = 0;
                DeliveryChainVM chainVm = new DeliveryChainVM();

                if (Int32.TryParse(id, out intId))
                {
                    DeliveryChain deliveryChain = _projectrepository.GetDeliveryChain(intId);

                    PartnerVMBuilder partnerVmBuilder = new PartnerVMBuilder(deliveryChain);

                    return partnerVmBuilder.BuildPartnerVM();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, user);
                throw;
            }
        }

        public async Task<DeliveryChainsVM> GetPartnerTableData(string componentId, string userid)
        {
            List<DeliveryChain> deliveryChains = _projectrepository.GetDeliveryChainsByComponent(componentId).ToList();

            DeliveryChainsVM partnersVm = new DeliveryChainsVM();

            //New Delivery Chain list object
            List<DeliveryChainListVM> deliveryChainListVms;

            //Map Chain to VM
            Mapper.CreateMap<DeliveryChain, DeliveryChainListVM>();
            deliveryChainListVms = Mapper.Map<List<DeliveryChain>, List<DeliveryChainListVM>>(deliveryChains);

            //Get Descriptions for all 
            List<int> listOfPartners = new List<int>();
            List<string> listOfSuppliers = new List<string>();


            foreach (DeliveryChainListVM chain in deliveryChainListVms)
            {
                //Check Parent
                if (chain.ParentType == "P")
                {
                    listOfPartners.Add(Int32.Parse(chain.ParentID));
                }
                if (chain.ParentType == "S")
                {
                    listOfSuppliers.Add(chain.ParentID.ToString());
                }

                //Check Child
                if (chain.ChildType == "P")
                {
                    listOfPartners.Add(Int32.Parse(chain.ChildID));
                }
                if (chain.ChildType == "S")
                {
                    listOfSuppliers.Add(chain.ChildID.ToString());
                }
            }

            //Get List of IDs and Names from PartnerMaster based on list of Partners
            List<PartnerMaster> partnerMasters = _projectrepository.GetDeliveryChainsByIDList(listOfPartners);

            //

            //Get List of IDs and Names from ARIES Suppliers based on list of Suppliers
            IEnumerable<SupplierVM> SupplierVms = await _ariesService.GetSuppliers(listOfSuppliers, "");


            //Map description 
            foreach (DeliveryChainListVM chainList in deliveryChainListVms)
            {
                if (chainList.ParentType == "P")
                {
                    foreach (PartnerMaster partners in partnerMasters)
                    {
                        if (Int32.Parse(chainList.ParentID) == partners.PartnerID)
                        {
                            chainList.ParentName = partners.PartnerName;
                        }
                    }
                }
                if (chainList.ParentType == "S")
                {
                    foreach (SupplierVM suppliers in SupplierVms)
                    {
                        if (Int32.Parse(chainList.ParentID) == Int32.Parse(suppliers.SupplierID))
                        {
                            chainList.ParentName = suppliers.SupplierName;
                        }
                    }
                }

                //Check Child
                if (chainList.ChildType == "P")
                {
                    foreach (PartnerMaster partners in partnerMasters)
                    {
                        if (Int32.Parse(chainList.ChildID) == partners.PartnerID)
                        {
                            chainList.ChildName = partners.PartnerName;
                        }
                    }
                }
                if (chainList.ChildType == "S")
                {
                    foreach (SupplierVM suppliers in SupplierVms)
                    {
                        if (Int32.Parse(chainList.ChildID) == Int32.Parse(suppliers.SupplierID))
                        {
                            chainList.ChildName = suppliers.SupplierName;
                        }
                    }
                }
            }

            partnersVm.deliveryChains = deliveryChainListVms;

            return partnersVm;
        }

        public bool DeletePartnerFromDeliveryChain(string id, string componentId, string user)
        {
            try
            {
                Int32 deliveryChainId = Int32.Parse(id);
                ChainList chainList = new ChainList(_projectrepository.GetDeliveryChainsByComponent(componentId));
                DeliveryChain parent = new DeliveryChain();
                DeliveryChain chain = new DeliveryChain();
                List<DeliveryChain> children = new List<DeliveryChain>();
                //Find the DeliveryChain to be deleted.
                chain = chainList.FindDeliveryChainById(deliveryChainId);

                //Find the parent of the partner
                parent = chainList.FindParentByChildId(deliveryChainId);
                // check to see if first tier
                if (chain.ChainID.Length < 3)
                {
                    // promote children to first tier partners
                    chainList.PromoteChildrenToFirstTier(chain);
                }
                else
                {
                   
                    //Update the children of the Delivery Chain to link to the Parent of the Delivery Chain.
                    chainList.LinkChildrenToNewDeliveryChain(chain, parent.ChainID);
                }
              
                //Delete the delivery chain
                chainList.DeleteDeliveryChain(deliveryChainId, parent.ChainID);
                //Somehow go through all of the children in the chain and recursively update the chain id's.
                chainList.UpdateChainIDsAfterChangingTheChain();



                List<DeliveryChain> deliveryChainsToInsert = chainList.ReturnDeliveryChains();

                foreach (DeliveryChain deliveryChain in deliveryChainsToInsert)
                {
                    _projectrepository.UpdateDeliveryChain(deliveryChain);
                }
                //Save all the changes.
                //Reinsert the child records.
                _projectrepository.Save();
                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, user);
                return false;
            }
        }

        public EditInputSectorsVM GetSectorsForEdit(string id, string user)
        {
            try
            {

              IEnumerable<InputSectorCode> inputSectorCodes =  _projectrepository.GetInputSectors(id);
              IEnumerable<InputSector> inputSectors = _projectrepository.LookUpInputSector();

                List<SectorCodeVM> sectorCodesVm = new List<SectorCodeVM>();

                foreach (InputSectorCode inputSectorCode in inputSectorCodes)
                {
                    SectorCodeVM sectorCodeVm = new SectorCodeVM
                    {
                        ISCode = inputSectorCode.InputSectorCode1,
                        ISDescription = String.Format("{0} - {1}", inputSectors.FirstOrDefault(x => x.InputSectorCodeID == inputSectorCode.InputSectorCode1).InputSectorCodeDescription, inputSectorCode.InputSectorCode1),
                        Percentage = 0,
                        LineNo = inputSectorCode.LineNo
                    };
                    if (inputSectorCode.Percentage != null)
                    {
                        sectorCodeVm.Percentage = inputSectorCode.Percentage.Value;
                    }
                    sectorCodesVm.Add(sectorCodeVm);
                }
                
                EditInputSectorsVM editInputSectorsVm = new EditInputSectorsVM();

                editInputSectorsVm.CompID = id;
                editInputSectorsVm.SectorCodesCodeVm = sectorCodesVm.ToArray();

                return editInputSectorsVm;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "GetSectorsForEdit " + id ,user);
                throw;

            }
        }

        public bool UpdateSectorCodes(EditInputSectorsVM editInputSectorsVm, ModelStateWrapper modelStateWrapper, string user)
        {
            try
            {
               SectorCodeService sectorCodeService = new SectorCodeService(_projectrepository,modelStateWrapper);

                if (sectorCodeService.UpdateSectorCodes(editInputSectorsVm, user))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "UpdateSectorCodes " + editInputSectorsVm.CompID, user);
                return false;
            }
        }



        public bool DeletePartnerAndChildrenFromDeliveryChain(string id, string componentId, string user)
        {
            try
            {
                Int32 deliveryChainId = Int32.Parse(id);
                ChainList chainList = new ChainList(_projectrepository.GetDeliveryChainsByComponent(componentId));
                
                DeliveryChain chain = new DeliveryChain();
         
                //Find the DeliveryChain to be deleted.
                chain = chainList.FindDeliveryChainById(deliveryChainId);
               
               
                //Delete the delivery chain amd all children based on the parent node
                chainList.DeleteDeliveryChainAndChildren(deliveryChainId, chain.ChainID);
                //Somehow go through all of the children in the chain and recursively update the chain id's.
                chainList.UpdateChainIDsAfterChangingTheChain();



                List<DeliveryChain> deliveryChainsToInsert = chainList.ReturnDeliveryChains();

                foreach (DeliveryChain deliveryChain in deliveryChainsToInsert)
                {
                    _projectrepository.UpdateDeliveryChain(deliveryChain);
                }
               
                _projectrepository.Save();
                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, user);
                return false;
            }
        }

        public async Task<AddPartnerToChainVM> SetUpPartnerSearch(string componentId, int deliveryChainId, int parentId,
           string addOrReplace, int childId)
        {
            AddPartnerToChainVM addPartnerToChainVm = new AddPartnerToChainVM();

       

            DeliveryChain deliveryChain = _projectrepository.GetDeliveryChain(deliveryChainId);

            DeliveryChainVM deliveryChainVm = new DeliveryChainVM();
            deliveryChainVm.ID = deliveryChain.ID;
            deliveryChainVm.ParentType = deliveryChain.ParentType;
            deliveryChainVm.ParentID = deliveryChain.ParentID;
            deliveryChainVm.AddOrReplace = addOrReplace;
            deliveryChainVm.ChildID = deliveryChain.ChildID;
            deliveryChainVm.ChildType = deliveryChain.ChildType;
            if(deliveryChainVm.ParentType == "P")
            {

                deliveryChainVm.ParentName = _projectrepository.GetPartnerName(deliveryChainVm.ParentID);
            }
            else
            {
               
                // return the supplier - we need to pass th4e ParentID in as a string as supplier Id is held as a string in 
                // the aries_interface
                string parentIDasString = deliveryChainVm.ParentID.ToString();
                string parentname = await _ariesService.GetSupplierName(parentIDasString);
                deliveryChainVm.ParentName = parentname;
               // deliveryChainVm.ParentName = "Supplier Parent Name";
            }


            // return the child name as well to display on the screens so users know what they are replacing/adding to

            if (deliveryChainVm.ChildType == "P")
            {

                deliveryChainVm.ChildName = _projectrepository.GetPartnerName(deliveryChainVm.ChildID);
            }
            else
            {

                // return the supplier - we need to pass th4e ParentID in as a string as supplier Id is held as a string in 
                // the aries_interface
                string parentIDasString = deliveryChainVm.ChildID.ToString();
                string childname = await _ariesService.GetSupplierName(parentIDasString);
                deliveryChainVm.ChildName = childname;
                // deliveryChainVm.ParentName = "Supplier Parent Name";
            }


            addPartnerToChainVm.ChainToBeAddedTo = deliveryChainVm;

            ComponentMaster component = ReturnComponentMaster(componentId);

            ComponentHeaderVM componentHeaderVm = ReturnComponentHeaderVm(component);
            addPartnerToChainVm.ComponentHeader = componentHeaderVm;

            return addPartnerToChainVm;
        }

        public ComponentHeaderVM SetupNewPartnerComponentHeader(string componentId)
        {
            AddPartnerToChainVM addPartnerToChainVm = new AddPartnerToChainVM();

            DeliveryChainVM deliverychainvm = new DeliveryChainVM();
            deliverychainvm.ComponentID = componentId;
           

            addPartnerToChainVm.ChainToBeAddedTo = deliverychainvm;

            ComponentMaster component = ReturnComponentMaster(componentId);

            ComponentHeaderVM componentHeaderVm = ReturnComponentHeaderVm(component);
  

            return componentHeaderVm;
        }

        public async Task<ComponentHeaderVM> SetupSearchFirstTierPartnerComponentHeader(string componentId)
        {

            ComponentMaster component = ReturnComponentMaster(componentId);

            ComponentHeaderVM componentHeaderVm = ReturnComponentHeaderVm(component);


            return componentHeaderVm;
        }


        #endregion

        #region Workflow related Methods


        public async Task<WorkflowPlannedEndDateVM> GetWorkflowPlannedEndDate(string projectId, string user)
        {
            try
            {
                WorkflowPlannedEndDateVM workflowPlannedEndDateVM = new WorkflowPlannedEndDateVM();


                ProjectMaster project = ReturnProjectMaster(projectId);
                workflowPlannedEndDateVM.ProjectHeaderVm = ReturnProjectHeaderVm(project, user);

                //get planned end date
                workflowPlannedEndDateVM.ExistingPlannedEndDate = project.ProjectDate.OperationalEndDate.Value;

                return workflowPlannedEndDateVM;

            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Get Workflow(string projectId, string user)", user);
                return null;
            }
        }
        public async Task<WorkflowVM> GetWorkflow(Int32 workflowId, string user)
        {

            try
            {
                WorkflowService workflowService = new WorkflowService(_projectrepository, _personService, _loggingengine, _errorengine, _documentService);

                WorkflowVM workflowVm = new WorkflowVM();

                workflowVm = await workflowService.WorkflowByWorkflowId(workflowId, user);

                ProjectMaster project = ReturnProjectMaster(workflowVm.WorkflowRequest.ProjectID);
                workflowVm.ProjectHeaderVm = ReturnProjectHeaderVm(project, user);

                //BudgetApprovalValue budgetApprovedByWorkflow = _projectrepository.GetBudgetApprovedByWorkflow(workflowId,1);
                //workflowVm.budgetApprovedByWorkflow = budgetApprovedByWorkflow;
                return workflowVm;

            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Get Workflow(Int32 workflowId, string user)", user);
                return null;

            }
        }

        public async Task<WorkflowVM> GetWorkflow(string projectId, Int32 taskId, string user)
        {
            try
            {
                WorkflowService workflowService = new WorkflowService(_projectrepository, _personService, _loggingengine, _errorengine, _documentService);

                WorkflowVM workflowVm = new WorkflowVM();

                // Decimal approvalValue = await _ariesService.GetProjBBudget(projectId);

                workflowVm = await workflowService.WorkflowByProjectIdAndTaskId(projectId, taskId, user);

                ProjectMaster project = ReturnProjectMaster(projectId);
                workflowVm.ProjectHeaderVm = ReturnProjectHeaderVm(project, user);
                //workflowVm.BudgetValueToBeApproved = approvalValue;
                return workflowVm;

            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Get Workflow(string projectId, Int32 taskId, string user)", user);
                return null;
            }

        }

        public async Task<WorkflowsVM> GetProjectWorkflows(string projectId, string user)
        {
            try
            {
                ProjectMaster project = ReturnProjectMaster(projectId);

                if (project != null)
                {
                    WorkflowService workflowService = new WorkflowService(_projectrepository, _personService, _loggingengine, _errorengine, _documentService);

                    WorkflowsVM workflowsVm = new WorkflowsVM();

                    workflowsVm = await workflowService.CompletedWorkflows(projectId, user);

                    workflowsVm.ProjectHeaderVm = ReturnProjectHeaderVm(project, user);
                    return workflowsVm;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "GetProjectWorkflows", user);
                return null;

            }

        }

        public async Task<bool> ActionWorkflowResponse(WorkflowMasterVM workflowResponseVm, string userAction, string urlBase, string user)
        {
            try
            {
                bool result = false;

                switch (workflowResponseVm.TaskID)
                {
                    case 1: //Close Project

                        string url = string.Format("{0}/{1}/{2}/{3}", urlBase, "Project", "Edit", workflowResponseVm.ProjectID);
                        if (userAction == "Approve Workflow")
                        {
                            result = await ApproveProjectClosure(workflowResponseVm, url, user);
                        }
                        else
                        {
                            result = await RejectProjectClosure(workflowResponseVm, url, user);
                        }
                        break;
                }

                return result;

            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Action Workflow Response", user);
                return false;
            }
        }

        public async Task<bool> SendProjectForClosure(WorkflowMasterVM closeProjectRequest, string pageUrl, string user)
        {
            try
            {
                //Validations

                //Create Workflow
                IEnumerable<WorkflowTask> workflowTasks = _projectrepository.GetWorkflowTasks();
                IEnumerable<WorkflowStage> workflowStages = _projectrepository.GetWorkflowStages();
                WorkflowMaster workflowMaster = new WorkflowMaster();

                workflowMaster.WorkFlowID = _projectrepository.NextWorkFlowId();

                workflowMaster.ActionBy = user;
                workflowMaster.ProjectID = closeProjectRequest.ProjectID;
                workflowMaster.ActionComments = closeProjectRequest.ActionComments;
                workflowMaster.Recipient = closeProjectRequest.Recipient;
                workflowMaster.ActionDate = DateTime.Now;

                workflowMaster.TaskID = workflowTasks.FirstOrDefault(x => x.Description == "Close Project").TaskID;
                workflowMaster.StageID = workflowStages.FirstOrDefault(x => x.Description == "Awaiting Approval").StageID;

                workflowMaster.WorkFlowStepID = 0; //Set to zero. This is the first step in the workflow.
                workflowMaster.LastUpdate = DateTime.Now;
                workflowMaster.Status = "A";
                workflowMaster.UserID = user;

                _projectrepository.InsertWorkFlowMaster(workflowMaster);
                _projectrepository.Save();

                //E-mails

                ProjectMaster project = ReturnProjectMaster(closeProjectRequest.ProjectID);

                string sroEmpNo;
                IEnumerable<PersonDetails> teamDetails;
                PersonDetails approver;
                PersonDetails sender;
                PersonDetails sro;

                AMPUtilities.EmailPeople peopleToEmail = await returnPeopleToEmail(closeProjectRequest.ProjectID, closeProjectRequest.Recipient, user);

                approver = peopleToEmail.Recipient;
                sender = peopleToEmail.Sender;
                sro = peopleToEmail.SRO;

                string SROText;

                if (sro.EmpNo != null)
                {
                    SROText = String.Format("The SRO is {0} {1}", sro.Forename, sro.Surname);
                }
                else
                {
                    SROText = "This project has no SRO assigned and does not comply with Smart Rules";
                }

                string body = string.Format("<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1}  has sent you a request to close the above project in the Aid Management Platform." +
                    "  Please review the request comments, and ensure the necessary financial actions have been taken and you are content for this project to be closed without a project completion report before approving or rejecting the request." +
                    "<br/><br/> Use the link below to access the project; the close section can be found at the bottom of the page under ‘Further options'" +
                    " <br/><br/>{2}<br/><br/>{3}.<br/><br/>Further guidance on closing a project can be found in the Smart Rules and the Smart Guide.</span>", approver.Forename, sender.Forename, pageUrl, SROText);
                string subject = string.Format("ACTION - Close Project Approval for {0}", project.Title);


                AMPUtilities.AMPEmail emailContainer = new AMPUtilities.AMPEmail();

                emailContainer = returnEmailContainer(approver, peopleToEmail, subject, body);

                EmailService emailService = new EmailService();

                emailService.SendEmail(emailContainer);


                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(closeProjectRequest.ProjectID, exception, user);
                return false;
            }
        }

        public async Task<bool> ApproveProjectClosure(WorkflowMasterVM closeProjectResponse, string pageUrl, string user)
        {
            try
            {
                IEnumerable<WorkflowStage> workflowStages = _projectrepository.GetWorkflowStages();
                WorkflowMaster requestWorkflowMaster = _projectrepository.GetWorkflowMaster(closeProjectResponse.WorkFlowID);
                Int32 workflowTaskId = requestWorkflowMaster.TaskID + 1;

                WorkflowMaster workflowToInsert = WorkflowToInsert(closeProjectResponse, user, workflowTaskId);

                workflowToInsert.StageID = workflowStages.FirstOrDefault(x => x.Description == "Approved").StageID;

                requestWorkflowMaster.Status = "C";
                //requestWorkflowMaster.LastUpdate = DateTime.Now;
                //requestWorkflowMaster.UserID = user;

                _projectrepository.InsertWorkFlowMaster(workflowToInsert);
                _projectrepository.UpdateWorkFlowMaster(requestWorkflowMaster);
                _projectrepository.Save();




                CloseProject(closeProjectResponse.ProjectID, user);

                //Do emails.

                ProjectMaster project = ReturnProjectMaster(closeProjectResponse.ProjectID);

                string sroEmpNo;
                IEnumerable<PersonDetails> teamDetails;
                PersonDetails approver;
                PersonDetails sender;
                PersonDetails sro;

                AMPUtilities.EmailPeople peopleToEmail = await returnPeopleToEmail(closeProjectResponse.ProjectID, closeProjectResponse.Recipient, user);

                approver = peopleToEmail.Recipient;
                sender = peopleToEmail.Sender;
                sro = peopleToEmail.SRO;

                string SROText;

                if (sro.EmpNo != null)
                {
                    SROText = String.Format("The SRO is {0} {1}", sro.Forename, sro.Surname);
                }
                else
                {
                    SROText = "This project has no SRO assigned and does not comply with Smart Rules";
                }

                string body = string.Format("<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has approved your request to close the above project." +
                    "  Please use the link below to review the comments.<br/><br/>{2}<br/><br/>  " +
                    "{3}.<br/><br/>Further guidance can be found in the Smart Rules and the Smart Guide.</span>", approver.Forename, sender.Forename, pageUrl, SROText, project.ProjectID);
                string subject = string.Format("Close Project Approved for {0}", project.Title);


                AMPUtilities.AMPEmail emailContainer = new AMPUtilities.AMPEmail();

                emailContainer = returnEmailContainer(approver, peopleToEmail, subject, body);

                EmailService emailService = new EmailService();

                emailService.SendEmail(emailContainer);

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(closeProjectResponse.ProjectID, exception, user);
                return false;
            }
        }

        private static WorkflowMaster WorkflowToInsert(WorkflowMasterVM closeProjectResponse, string user, int workFlowStepID)
        {
            //Create a new WorkflowMaster and map the ViewModel across.
            WorkflowMaster workflowToInsert = new WorkflowMaster();
            Mapper.CreateMap<WorkflowMasterVM, WorkflowMaster>();
            Mapper.Map<WorkflowMasterVM, WorkflowMaster>(closeProjectResponse, workflowToInsert);

            workflowToInsert.ActionBy = user;
            workflowToInsert.ActionDate = DateTime.Now;
            workflowToInsert.WorkFlowStepID = workFlowStepID;
            workflowToInsert.Status = "C";
            workflowToInsert.LastUpdate = DateTime.Now;
            workflowToInsert.UserID = user;
            return workflowToInsert;
        }

        public async Task<bool> RejectProjectClosure(WorkflowMasterVM closeProjectResponse, string pageUrl, string user)
        {

            try
            {
                IEnumerable<WorkflowStage> workflowStages = _projectrepository.GetWorkflowStages();
                WorkflowMaster requestWorkflowMaster = _projectrepository.GetWorkflowMaster(closeProjectResponse.WorkFlowID);

                Int32 workflowStepId = requestWorkflowMaster.WorkFlowStepID + 1;

                WorkflowMaster workflowToInsert = WorkflowToInsert(closeProjectResponse, user, workflowStepId);

                workflowToInsert.StageID = workflowStages.FirstOrDefault(x => x.Description == "Rejected").StageID;

                requestWorkflowMaster.Status = "C";
                //requestWorkflowMaster.LastUpdate = DateTime.Now;
                //requestWorkflowMaster.UserID = user;

                _projectrepository.InsertWorkFlowMaster(workflowToInsert);
                _projectrepository.UpdateWorkFlowMaster(requestWorkflowMaster);
                _projectrepository.Save();

                //Do emails.

                ProjectMaster project = ReturnProjectMaster(closeProjectResponse.ProjectID);

                string sroEmpNo;
                IEnumerable<PersonDetails> teamDetails;
                PersonDetails approver;
                PersonDetails sender;
                PersonDetails sro;

                AMPUtilities.EmailPeople peopleToEmail = await returnPeopleToEmail(closeProjectResponse.ProjectID, closeProjectResponse.Recipient, user);

                approver = peopleToEmail.Recipient;
                sender = peopleToEmail.Sender;
                sro = peopleToEmail.SRO;

                string SROText;

                if (sro.EmpNo != null)
                {
                    SROText = String.Format("The SRO is {0} {1}", sro.Forename, sro.Surname);
                }
                else
                {
                    SROText = "This project has no SRO assigned and does not comply with Smart Rules";
                }

                string body = string.Format("<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has rejected your request to close the above project." +
                    "  Please use the link below to review the comments.<br/><br/>{2}<br/><br/>  " +
                    "{3}.<br/><br/>Further guidance can be found in the Smart Rules and the Smart Guide.</span>", approver.Forename, sender.Forename, pageUrl, SROText, project.ProjectID);
                string subject = string.Format("Close Project Rejected for {0}", project.Title);


                AMPUtilities.AMPEmail emailContainer = new AMPUtilities.AMPEmail();

                emailContainer = returnEmailContainer(approver, peopleToEmail, subject, body);

                EmailService emailService = new EmailService();

                emailService.SendEmail(emailContainer);

                return true;


            }
            catch (Exception exception)
            {
                _errorengine.LogError(closeProjectResponse.ProjectID, exception, user);
                return false;
            }
        }





        public async Task<bool> PreValidateWorkflowApproval(String projectid, Int32 taskid, IValidationDictionary validationDictionary, String user)
        {

            switch (taskid)
            {
                case 1:
                    //No need to validate close project.                  
                    break;
                case 2:
                    //re approval will use the same validation method
                    if (!await ValidateSendforApproval(projectid, taskid, validationDictionary, user))
                        return false;
                    break;
                case 7:
                    //First is sending the project for projectworkflowmaster valid?
                    //A & D Approval
                    if (!await ValidateSendforApproval(projectid, taskid, validationDictionary, user))
                        return false;
                    break;
                case 8:
                    //First is sending the project for projectworkflowmaster valid?
                    //Humanitarian
                    if (!await ValidateSendforApproval(projectid, taskid, validationDictionary, user))
                        return false;
                    break;
                case 9:
                    //First is sending the project for projectworkflowmaster valid?
                    //Archive Project
                    if (!await ValidateSendforApproval(projectid, taskid, validationDictionary, user))
                        return false;
                    break;
                case 10:
                    //Re-Open project - No validation applied
                    break;
                case 11:
                    //First time approval of a project, moving from A&D to approved.
                    if (!await ValidateSendforApproval(projectid, taskid, validationDictionary, user))
                        return false;
                    break;
                case 12: //project planned end date extension
                    if (!await ValidateSendforApproval(projectid, taskid, validationDictionary, user))
                        return false;
                    break;
            }
            return true;
        }

        public async Task<bool> SendforWorkflowApproval(WorkflowVM workflowvm, IValidationDictionary validationDictionary, string user, string pageUrl)
        {

            //Is this user sending the task to their self?
            if (user == workflowvm.WorkflowRequest.Recipient)
            {
                validationDictionary.AddError("Summary", "You cannot send approval tasks to youself");
                return false;
            }

            //Clean up the workflow comments in case they have been pasted in from Word and contain line feed/carriage return characters.
            workflowvm.WorkflowRequest.ActionComments = AMPUtilities.CleanText(workflowvm.WorkflowRequest.ActionComments);

            try
            {

                //We may want to case here and check different things depending on the workflow route. 

                //Case statement to apply correct validation method
                switch (workflowvm.WorkflowRequest.TaskID)
                {
                    case 1:
                        //Close project validator needs moved here
                        if (!ValidateSendforClosure(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;
                        break;
                    case 2:
                        //Project Re-Approval
                        //Approval and re approval will use the same validation method
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;
                        break;
                    case 7:
                        //First is sending the project for projectworkflowmaster valid?
                        //Approval and re approval will use the same validation method
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;
                        break;
                    case 8:
                        //First is sending the project for projectworkflowmaster valid?
                        //Approval and re approval will use the same validation method
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;
                        break;
                    case 9:
                        //Request project is archived.
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;
                        break;
                    case 10:
                        //First is sending the project for projectworkflowmaster valid?
                        //Approval and re approval will use the same validation method
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;
                        break;
                    case 11:
                        //First time approval
                        //Approval and re approval will use the same validation method
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;
                        break;

                    case 12:
                        //Project Re-Approval for Planned End Date
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;
                        break;
                    default:
                        //Task not implemented yet. Send back a validation fail.
                        validationDictionary.AddError("Summary", "This task is not yet supported via the workflow screen.");
                        return false;
                }

                //Create Workflow
                WorkflowService workflowService = new WorkflowService(_projectrepository, _personService, _loggingengine, _errorengine, _documentService);
                bool startWorkflowSuccess = await workflowService.StartWorkflow(workflowvm, user);
                if (!startWorkflowSuccess)
                {
                    return false;
                }
                //E-mails

                EmailCreator emailCreator = new WorkflowRequestEmail();

                Email email = await emailCreator.CreateEmail(workflowvm.WorkflowRequest.TaskID.ToString(),
                    workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.ActionComments,
                    workflowvm.WorkflowRequest.Recipient, user, "");

                _emailService.SendEmail(email);

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(workflowvm.WorkflowRequest.ProjectID, exception, user);
                return false;
            }

        }




        public async Task<bool> ApproveWorkflow(WorkflowVM workflowvm, IValidationDictionary validationDictionary, string user)
        {
            try
            {
                string pageUrl = AMPUtilities.BaseUrl() + "/Workflow/Details/" + workflowvm.WorkflowResponse.WorkFlowID;

                //We may want to case here and check different things depending on the workflow route. 

                //Clean up the workflow approval comments in case they have been pasted in from Word and contain line feed/carriage return characters.
                workflowvm.WorkflowResponse.ActionComments = AMPUtilities.CleanText(workflowvm.WorkflowResponse.ActionComments);


                //Case statatement to apply correct validation method
                switch (workflowvm.WorkflowRequest.TaskID)
                {
                    case 1:

                        //Any validations here?

                        //Apply business logic to the project
                        CloseProject(workflowvm.WorkflowResponse.ProjectID, user);

                        //Update workflow
                        //workflowService.ApproveWorkflow(workflowvm,user);
                        ApproveWorkflow(workflowvm, user);

                        //Budget Closure - This is already managed via a different bit of code for now.

                        //Send Emails
                        await SendApprovalEmails(workflowvm, user, pageUrl);

                        break;

                    case 2:
                        //Project sent for re-approval

                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;

                        //Business logic for approving a project
                        ApproveProject(workflowvm.WorkflowResponse.ProjectID, user);

                        //Update workflow
                        ApproveWorkflow(workflowvm, user);

                        if (AMPUtilities.ARIESUpdateEnabled() == "true")
                        {
                            await _ariesService.BudgetMovement(workflowvm.WorkflowRequest.ProjectID,
                                workflowvm.WorkflowRequest.ProjectID, "PROJB", "PROJE", user, 0);
                        }
                        //Send Emails
                        await SendApprovalEmails(workflowvm, user, pageUrl);

                        break;
                    case 7:
                        //A&D

                        // Validate project
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;

                        //Update workflow
                        ApproveWorkflow(workflowvm, user);

                        //Approve AD project Business logic
                        ApproveADWithBudget(workflowvm.WorkflowResponse.ProjectID, user);

                        //ARIES Budget Movement
                        if (AMPUtilities.ARIESUpdateEnabled() == "true")
                        {
                            await
                                _ariesService.BudgetMovement(workflowvm.WorkflowRequest.ProjectID,
                                    workflowvm.WorkflowRequest.ProjectID, "PROJA", "PROJB", user, 0);

                            //ARIES Budget Movement
                            await _ariesService.BudgetMovement(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.ProjectID, "PROJB", "PROJC", user, 1);
                        }

                        //Send Emails
                        await SendApprovalEmails(workflowvm, user, pageUrl);

                        break;
                    case 8:
                        //Humaniterian

                        // Validate project
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;

                        //Update workflow
                        ApproveWorkflow(workflowvm, user);

                        //Business logic for approving a project
                        ApproveProject(workflowvm.WorkflowResponse.ProjectID, user);

                        //ARIES Budget Movement
                        if (AMPUtilities.ARIESUpdateEnabled() == "true")
                        {
                            await
                                _ariesService.BudgetMovement(workflowvm.WorkflowRequest.ProjectID,
                                    workflowvm.WorkflowRequest.ProjectID, "PROJA", "PROJB", user, 0);
                            await
                                _ariesService.BudgetMovement(workflowvm.WorkflowRequest.ProjectID,
                                    workflowvm.WorkflowRequest.ProjectID, "PROJB", "PROJC", user, 0);
                        }
                        //Send Emails
                        await SendApprovalEmails(workflowvm, user, pageUrl);
                        await SendHoDAlertEmail(workflowvm, user, pageUrl);

                        break;


                    case 9:
                        //Archive project
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;

                        //Update workflow
                        ApproveWorkflow(workflowvm, user);

                        //Send Emails
                        await SendApprovalEmails(workflowvm, user, pageUrl);

                        //TODO: Prefer to set the business logic before sending e-mail. However that removes everyone from the team and causes an exception. 
                        //Business logic for approving a project
                        ArchiveProject(workflowvm.WorkflowResponse.ProjectID, user);

                        break;

                    case 10:

                        //Approval and re approval will use the same validation method
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;

                        //Business logic for approving a project
                        ReOpenProject(workflowvm.WorkflowResponse.ProjectID, user);

                        //Update workflow
                        ApproveWorkflow(workflowvm, user);

                        //Send Emails
                        await SendApprovalEmails(workflowvm, user, pageUrl);

                        break;
                    case 11:
                        //Send for approval for the first time, from A&D Stage.
                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;

                        //Business logic for approving a project
                        ApproveProject(workflowvm.WorkflowResponse.ProjectID, user);

                        //Update workflow
                        ApproveWorkflow(workflowvm, user);


                        //ARIES Budget Movement - Always B to C for first approval.
                        //SetBudgetApproval(workflowvm,user);
                        if (AMPUtilities.ARIESUpdateEnabled() == "true")
                        {
                            await _ariesService.BudgetMovement(workflowvm.WorkflowRequest.ProjectID,
                                workflowvm.WorkflowRequest.ProjectID, "PROJB", "PROJC", user, 0);
                        }

                        await SendApprovalEmails(workflowvm, user, pageUrl);
                        await SendHoDAlertEmail(workflowvm, user, pageUrl);

                        break;

                    // project re-approval for planned end date 
                    case 12:


                        if (!await ValidateSendforApproval(workflowvm.WorkflowRequest.ProjectID, workflowvm.WorkflowRequest.TaskID, validationDictionary, user))
                            return false;

                        //Business logic for approving a project planned end date
                        // ApproveProject(workflowvm.WorkflowResponse.ProjectID, user);
                        AmendPlannedEndDate(workflowvm.WorkflowResponse.ProjectID, user);
                        //Update workflow
                        ApproveWorkflow(workflowvm, user);


                        //Send Emails
                        await SendApprovalEmails(workflowvm, user, pageUrl);

                        break;
                }

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(workflowvm.WorkflowResponse.ProjectID, exception, user);
                return false;
            }
        }

        //private void SetBudgetApproval(WorkflowVM workflowVm, string user)
        //{
        //    try
        //    {
        //        Int32 budgetType;

        //        if (workflowVm.TaskID == Constants.ApproveAD)
        //        {
        //            budgetType = Constants.PROJABudget;
        //        }
        //        else
        //        {
        //            budgetType = Constants.PROJBBudget;
        //        }
        //        //Set the budgetapproval object

        //        BudgetApprovalValue budgetApproval = new BudgetApprovalValue
        //        {
        //            ProjectID = workflowVm.WorkflowRequest.ProjectID,
        //            Amount = workflowVm.BudgetValueToBeApproved,
        //            LastUpdated = DateTime.Now,
        //            Status = "A",
        //            UserID = user,
        //            BudgetTypeID = budgetType,
        //            WorkflowID = workflowVm.WorkflowResponse.WorkFlowID,
        //            WorkFlowStepID = 1
        //        };

        //        //Save the budgetapproval
        //        _projectrepository.InsertBudgetApproval(budgetApproval);
        //        _projectrepository.Save();

        //    }
        //    catch (Exception exception)
        //    {
        //        _errorengine.LogError(workflowVm.WorkflowRequest.ProjectID, exception, "Budget amount is " + workflowVm.BudgetValueToBeApproved + " WorkflowID is " + workflowVm.WorkflowResponse.WorkFlowID, user);
        //    }
        //}


        private async Task SendApprovalEmails(WorkflowVM workflowvm, string user, string pageUrl)
        {
            EmailCreator emailCreator = new WorkflowApprovalEmail(_personService, _projectrepository);

            Email email = await emailCreator.CreateEmail(workflowvm.TaskID.ToString(), workflowvm.WorkflowResponse.ProjectID, workflowvm.WorkflowResponse.ActionComments, workflowvm.WorkflowResponse.Recipient, user, pageUrl);

            _emailService.SendEmail(email);

        }

        private async Task SendHoDAlertEmail(WorkflowVM workflowvm, string user, string pageUrl)
        {
            EmailCreator emailCreator = new WorkflowHoDAlertEmail(_personService, _projectrepository);

            Email email = await emailCreator.CreateEmail(workflowvm.TaskID.ToString(), workflowvm.WorkflowResponse.ProjectID, workflowvm.WorkflowResponse.ActionComments, workflowvm.WorkflowResponse.Recipient, user, pageUrl);

            _emailService.SendEmail(email);

        }

        private void ApproveWorkflow(WorkflowVM workflowvm, string user)
        {
            IEnumerable<WorkflowStage> workflowStages = _projectrepository.GetWorkflowStages();
            WorkflowMaster requestWorkflowMaster = _projectrepository.GetWorkflowMaster(workflowvm.WorkflowResponse.WorkFlowID);
            Int32 workflowTaskId = requestWorkflowMaster.WorkFlowStepID + 1;

            WorkflowMaster workflowToInsert = WorkflowToInsert(workflowvm.WorkflowResponse, user, workflowTaskId);

            workflowToInsert.StageID = workflowStages.FirstOrDefault(x => x.Description == "Approved").StageID;

            requestWorkflowMaster.Status = "C";
            //requestWorkflowMaster.LastUpdate = DateTime.Now;
            //requestWorkflowMaster.UserID = user;

            _projectrepository.InsertWorkFlowMaster(workflowToInsert);
            _projectrepository.UpdateWorkFlowMaster(requestWorkflowMaster);
            _projectrepository.Save();
        }

        public async Task<bool> RejectWorkflow(WorkflowVM workflowvm, string pageUrl, string user)
        {

            try
            {
                IEnumerable<WorkflowStage> workflowStages = _projectrepository.GetWorkflowStages();
                WorkflowMaster requestWorkflowMaster = _projectrepository.GetWorkflowMaster(workflowvm.WorkflowResponse.WorkFlowID);

                Int32 workflowStepId = requestWorkflowMaster.WorkFlowStepID + 1;

                WorkflowMaster workflowToInsert = WorkflowToInsert(workflowvm.WorkflowResponse, user, workflowStepId);

                workflowToInsert.StageID = workflowStages.FirstOrDefault(x => x.Description == "Rejected").StageID;

                // ** For the planned end date amendment set the status of the row to C
                if (workflowvm.TaskID == 12)
                {
                    RejectPlannedEndDate(workflowToInsert.ProjectID, user);
                    // workflowvm.WfMessage = "";
                }


                requestWorkflowMaster.Status = "C";
                //requestWorkflowMaster.LastUpdate = DateTime.Now;
                //requestWorkflowMaster.UserID = user;


                _projectrepository.InsertWorkFlowMaster(workflowToInsert);
                _projectrepository.UpdateWorkFlowMaster(requestWorkflowMaster);
                _projectrepository.Save();

                //Do emails.

                ProjectMaster project = ReturnProjectMaster(workflowvm.WorkflowResponse.ProjectID);

                string sroEmpNo;
                IEnumerable<PersonDetails> teamDetails;
                PersonDetails approver;
                PersonDetails sender;
                PersonDetails sro;

                EmailCreator emailCreator = new WorkflowRejectionEmail(_personService, _projectrepository);

                Email email = await emailCreator.CreateEmail(workflowvm.TaskID.ToString(), workflowvm.WorkflowResponse.ProjectID, workflowvm.WorkflowResponse.ActionComments, workflowvm.WorkflowResponse.Recipient, user, pageUrl);

                _emailService.SendEmail(email);

                return true;


            }
            catch (Exception exception)
            {
                _errorengine.LogError(workflowvm.WorkflowResponse.ProjectID, exception, user);
                return false;
            }
        }

        //Workflow Actions---------------------------------------------------------------------------------------------------------------------------
        //1
        public void CloseProject(String projectId, String user)
        {
            try
            {
                //Close Project
                ProjectMaster project = _projectrepository.GetProject(projectId);

                project.Stage = "7";
                project.LastUpdate = DateTime.Now;
                project.UserID = user;
                project.Status = "C";

                //Close dates
                ProjectDate projectdate = _projectrepository.GetProjectDates(projectId);

                projectdate.ActualEndDate = DateTime.Now;
                projectdate.FinancialEndDate = DateTime.Now.AddMonths(18);
                projectdate.LastUpdate = DateTime.Now;
                projectdate.UserID = user;

                //Update the Project Performance 
                Performance projectPerformance = _projectrepository.GetProjectPerformance(projectId);

                projectPerformance.ARRequired = "No";
                projectPerformance.PCRRequired = "No";
                projectPerformance.ARDueDate = null;
                projectPerformance.ARPromptDate = null;
                projectPerformance.PCRDueDate = null;
                projectPerformance.PCRPrompt = null;
                projectPerformance.PCRAuthorised = "Yes";
                projectPerformance.Status = "A";
                projectPerformance.LastUpdated = DateTime.Now;
                projectPerformance.UserID = user;

                //Always active components.
                //Probably need to get components and close them here
                IEnumerable<ComponentMaster> components = project.ComponentMasters;

                //EndDate is the Component Financial End Date.
                foreach (ComponentMaster component in components)
                {
                    ComponentDate componentDate = component.ComponentDate;
                    componentDate.EndDate = DateTime.Now.AddMonths(18);
                    _projectrepository.UpdateComponentDate(componentDate);

                }

                //Do the database stuff.
                _projectrepository.CloseProject(project, projectdate, projectPerformance, user);
                _projectrepository.Save();
            }
            catch (Exception ex)
            {
                _errorengine.LogError(projectId, ex, user);
            }
        }

        public void ReOpenProject(string projectId, string user)
        {
            ProjectMaster project = _projectrepository.GetProject(projectId);

            //If project re-opened set excemption text to "Project re-opened" 
            project.Performance.ARExcemptReason = "PCR Completed - project re-opened to finalise payments";
            project.Performance.PCRExcemptReason = "PCR Completed - project re-opened to finalise payments";
            //set both AR/PCR Required to "No" as per requirement
            project.Performance.ARRequired = "No";
            project.Performance.PCRRequired = "No";

            //Set Project data
            project.Stage = "5";
            project.LastUpdate = DateTime.Now;
            project.UserID = user;
            project.Status = "A";

            ActivateComponents(project);

            //Do the database stuff.
            _projectrepository.UpdateProject(project);
            _projectrepository.Save();
            _projectrepository.UpdateARIESProjectForApproval(project);

        }

        //2 
        public void ApproveProject(String projectId, String user)
        {
            try
            {

                //Do we need to check if this is the first or second approval? as i would not like to reset AR and PCR data etc

                ProjectMaster project = _projectrepository.GetProject(projectId);




                Int32 intStage;
                Int32.TryParse(project.Stage, out intStage);

                //Check for current stage 5, if not 5 then this is first approval
                if (intStage < 5)
                {

                    //Dates
                    project.ProjectDate.ActualStartDate = DateTime.Now;
                    project.ProjectDate.LastUpdate = DateTime.Now;
                    project.ProjectDate.UserID = user;

                    //Admin projects do not require AR's and PCR's
                    if (project.BudgetCentreID.Contains("A") || project.BudgetCentreID.Contains("C0"))
                    {
                        //Update the Project Performance 
                        project.Performance.ARRequired = "No";
                        project.Performance.PCRRequired = "No";
                        project.Performance.PCRAuthorised = "";
                        project.Performance.Status = "A";
                        project.Performance.LastUpdated = DateTime.Now;
                        project.Performance.UserID = user;
                    }
                    else
                    {


                        //If the project is less than 15 months it does not require an AR
                        if (((project.ProjectDate.OperationalEndDate.Value.Year - project.ProjectDate.ActualStartDate.Value.Year) * 12) + project.ProjectDate.OperationalEndDate.Value.Month - project.ProjectDate.ActualStartDate.Value.Month < 15)
                        {
                            project.Performance.ARRequired = "No";
                            project.Performance.ARExcemptReason = "The programme has a duration of less than 15 months";
                        }
                        else
                        {
                            //Update the Project Performance 
                            project.Performance.ARRequired = "Yes";
                            project.Performance.ARDueDate = project.ProjectDate.ActualStartDate.Value.AddMonths(12);
                            project.Performance.ARPromptDate = project.ProjectDate.ActualStartDate.Value.AddMonths(9);
                        }
                        project.Performance.PCRRequired = "Yes";
                        project.Performance.PCRDueDate = project.ProjectDate.OperationalEndDate.Value.AddMonths(3);
                        project.Performance.PCRPrompt = project.ProjectDate.OperationalEndDate.Value.AddMonths(-3);
                        project.Performance.PCRAuthorised = "";
                        project.Performance.Status = "A";
                        project.Performance.LastUpdated = DateTime.Now;
                        project.Performance.UserID = user;

                    }

                }

                //Projects re-approved at completion should remain at completion. 
                if (intStage < 7)
                {
                    //Set Project data
                    project.Stage = "5";
                    project.LastUpdate = DateTime.Now;
                    project.UserID = user;
                    project.Status = "A";

                    ActivateComponents(project);
                }



                //Do the database stuff.
                _projectrepository.UpdateProject(project);
                _projectrepository.Save();
                _projectrepository.UpdateARIESProjectForApproval(project);
            }
            catch (Exception ex)
            {
                _errorengine.LogError(projectId, ex, user);
            }
        }

        private void ArchiveProject(string projectId, string user)
        {
            ProjectMaster project = _projectrepository.GetProject(projectId);

            //Get the active project team then set them all to complete.
            IEnumerable<Team> projectTeam = project.Teams.Where(x => x.Status == "A");

            projectTeam.Select(x =>
            {
                x.Status = "C";
                x.EndDate = DateTime.Now;
                x.LastUpdated = DateTime.Now;
                x.UserID = user;
                return x;
            }).ToList();

            //Set the Project Financial End Date and the Archive Date
            project.ProjectDate.ArchiveDate = DateTime.Now;
            project.ProjectDate.FinancialEndDate = DateTime.Now;
            project.ProjectDate.LastUpdate = DateTime.Now;
            project.ProjectDate.UserID = user;

            //Always active components.
            //Probably need to get components and close them here
            IEnumerable<ComponentMaster> components = project.ComponentMasters;

            //EndDate is the Component Financial End Date.
            foreach (ComponentMaster component in components)
            {
                component.ComponentDate.EndDate = DateTime.Now;
            }

            //Set the project stage
            project.Stage = "8";
            project.LastUpdate = DateTime.Now;
            project.UserID = user;

            _projectrepository.UpdateProject(project);
            _projectrepository.Save();

        }


        private static void ActivateComponents(ProjectMaster project)
        {
            //Always active components.
            //Probably need to get components and activate them here
            IEnumerable<ComponentMaster> components = project.ComponentMasters;

            foreach (ComponentMaster component in components)
            {
                component.OperationalStatus = "A";
                component.Approved = "Y";
            }
        }

        public void ApproveADWithBudget(String projectId, String user)
        {
            try
            {
                ProjectMaster project = _projectrepository.GetProject(projectId);

                project.Stage = "3";
                project.LastUpdate = DateTime.Now;
                project.UserID = user;
                project.Status = "A";


                //Always active components.
                //Probably need to get components and activate them here
                IEnumerable<ComponentMaster> components = project.ComponentMasters;

                foreach (ComponentMaster component in components)
                {
                    component.OperationalStatus = "A";
                    component.Approved = "Y";
                }


                //Do the database stuff.
                _projectrepository.UpdateProject(project);
                _projectrepository.Save();
                _projectrepository.UpdateARIESProjectForApproval(project);
            }
            catch (Exception ex)
            {
                _errorengine.LogError(projectId, ex, user);
            }


        }


        public async Task<bool> ChangeWorkflowApprover(WorkflowVM workflowVm, string pageUrl, string user)
        {
            try
            {
                int workFlowId = workflowVm.WorkflowResponse.WorkFlowID;

                WorkflowMaster workflowToUpdate = _projectrepository.GetWorkflowMaster(workFlowId);

                string originalApprover = workflowToUpdate.Recipient;
                workflowToUpdate.Recipient = workflowVm.WorkflowRequest.Recipient;


                _projectrepository.UpdateWorkFlowMaster(workflowToUpdate);
                _projectrepository.Save();

                //Send e-mail to new approver.

                EmailCreator emailCreator = new WorkflowRequestEmail(_personService, _projectrepository);

                Email email = await emailCreator.CreateEmail(workflowVm.WorkflowRequest.TaskID.ToString(),
                    workflowVm.WorkflowRequest.ProjectID, workflowVm.WorkflowRequest.ActionComments,
                    workflowVm.WorkflowRequest.Recipient, user, pageUrl);

                _emailService.SendEmail(email);

                return true;

            }
            catch (Exception exception)
            {
                _errorengine.LogError(workflowVm.WorkflowRequest.ProjectID, exception, user);
                return false;
            }


        }

        public static string ClearWorkflowMessage(WorkflowVM workflow)
        {
            workflow.WfMessage = "";
            return workflow.WfMessage;
        }

        public async Task<bool> CancelWorkflow(WorkflowVM workflowVm, string pageUrl, string user)
        {
            try
            {
                int workFlowId = workflowVm.WorkflowResponse.WorkFlowID;
                int taskId = workflowVm.WorkflowRequest.TaskID;

                WorkflowMaster workflowMaster = _projectrepository.GetWorkflowMaster(workFlowId);
                //Delete workflow  
                _projectrepository.DeleteWorkflowMaster(workflowMaster);
                //Delete Workflow associated document 
                _projectrepository.DeleteWorkflowDocument(workflowMaster.WorkFlowID);

                _projectrepository.Save();

                // ** For the planned end date amendment set the status of the row to C
                if (workflowVm.TaskID == 12)
                {

                    //  ***** clear down the workflow message 
                    // ClearWorkflowMessage(workflowVm);

                    RejectPlannedEndDate(workflowVm.WorkflowRequest.ProjectID, user);
                }


                EmailCreator emailCreator = new WorkflowRequestEmail(_personService, _projectrepository);

                Email email = await emailCreator.CreateEmail("0", workflowVm.WorkflowRequest.ProjectID, workflowVm.WorkflowRequest.ActionComments, workflowVm.WorkflowRequest.Recipient, user, pageUrl);

                _emailService.SendEmail(email);

                return true;

            }
            catch (Exception exception)
            {
                _errorengine.LogError(workflowVm.WorkflowRequest.ProjectID, exception, user);
                return false;
            }
        }

        // this will be called as part of the Approve/ Planned End Date Task 12
        public void AmendPlannedEndDate(string projectId, String user)
        {

            try
            {
                // get the project that you need to amend
                ProjectMaster project = _projectrepository.GetProject(projectId);
                //get the active row in the workflow planned end date temp table that needs to be closed off
                ProjectPlannedEndDate projPlannedEndDate = _projectrepository.GetProjectPlannedEndDate(projectId);

                //update the operational end date on the project date taable
                project.ProjectDate.OperationalEndDate = projPlannedEndDate.NewPlannedEndDate;
                project.ProjectDate.LastUpdate = DateTime.Now;
                project.ProjectDate.UserID = user;

                //** AMEND PCR DUE DATE update the PCR Due date if necessary
                Performance performance = _projectrepository.GetProjectPerformance(project.ProjectID);

                if (performance.PCRDueDate != null)
                {
                    performance.PCRDueDate = project.ProjectDate.OperationalEndDate.Value.AddMonths(3);
                    performance.PCRPrompt = project.ProjectDate.OperationalEndDate.Value.AddMonths(-3);
                }

                //*** AMEND FINANCIAL END DATE - add six months to op end date
                DateTime newFinancialEndDate = project.ProjectDate.OperationalEndDate.Value.AddMonths(6);
                project.ProjectDate.FinancialEndDate = newFinancialEndDate;

                // *** AMEND PROJECT CLOSURE PROMPT DATE - 3 months before planned end date .
                DateTime newPromptCompletionDate = project.ProjectDate.OperationalEndDate.Value.AddMonths(-3);
                project.ProjectDate.PromptCompletionDate = newPromptCompletionDate;

                // CHECK IF ANY AR EXEMPTION IS STILL VALID - IF NOT, UNEXEMPT
                bool isARExemptionValid = _validationService.IsTheARExemptionValid(user, project, performance);

                if (isARExemptionValid == false)
                {
                    project = UnexemptARDetails(project, user);
                }

                // update the status of the workflow planned end date end date row to C
                projPlannedEndDate.Status = "C";
                projPlannedEndDate.LastUpdate = DateTime.Now;
                projPlannedEndDate.UserID = user;

                //save to the database
                _projectrepository.UpdateProject(project);
                _projectrepository.UpdatePlannedEndDate(projPlannedEndDate);
                _projectrepository.Save();
            }
            catch (Exception ex)
            {
                _errorengine.LogError(projectId, ex, user);
            }

        }


        // ***** Move this to the validationservice.cs file ******
        //private bool IsARExemptionValid(string user, ProjectMaster project, Performance performance)
        //{
        //    Checks if ar was exempt as project length was under 15 months - if now over 15 months, returns false
        //    if (performance.ARExcemptReason ==
        //        _projectrepository.GetSingleExemptionReason("3", "AR").ExemptionReason1 && performance.ARRequired == "No")
        //    {
        //        DateTime actualPlus15Months = project.ProjectDate.ActualStartDate.Value.AddMonths(15);
        //        Compares actual end date plus 15 months to planned end date(if planned end date is later, would give an int greater than 0)
        //        if (
        //            DateTime.Compare(project.ProjectDate.OperationalEndDate.Value, actualPlus15Months) >
        //            0)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }

        //    }

        //    Checks if project was exempt as AR due less than 4 months before the project end date (3 months per rule plus a one month tolerance) - if AR is now due more than 4 months before new project planned end date, unexempts AR
        //    else if (performance.ARExcemptReason ==
        //            _projectrepository.GetSingleExemptionReason("7", "AR").ExemptionReason1 && performance.ARRequired == "No")
        //    {
        //        DateTime nextARPlus4Months;
        //        Gathers existing approved reviews
        //        ICollection<ReviewMaster> approvedReviews = null;
        //        foreach (var review in project.ReviewMasters)
        //        {
        //            if (review.Approved == "1")
        //                approvedReviews.Add(review);
        //        }

        //        Checks if any existing approved reviews - if not, assumes AR was due 12 months after actual start date & adds 4 months to this for comparison(16 months in total)

        //       int existingReviews = approvedReviews.Count;
        //        if (existingReviews == 0)
        //            {
        //                nextARPlus4Months = project.ProjectDate.ActualStartDate.Value.AddMonths(16);
        //            }
        //        If existing approved review, takes latest & adds 4 months for the comparison
        //        else
        //        {
        //            DateTime lastReviewDate =
        //                approvedReviews.MaxBy(x => x.ReviewDate.Value).ReviewDate.Value;
        //            nextARPlus4Months = lastReviewDate.AddMonths(16);
        //        }

        //        Compares ar due date plus 3 months to new planned end date (if planned end date is later, would give an int greater than 0)
        //        if (DateTime.Compare(project.ProjectDate.OperationalEndDate.Value, nextARPlus4Months) > 0)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}


        // this will be called as part of the Reject/Cancel Planned End Date Task 12
        public void RejectPlannedEndDate(string projectId, String user)
        {

            try
            {
                // get the project that you need to amend
                ProjectMaster project = _projectrepository.GetProject(projectId);
                //get the active row in the workflow planned end date temp table that needs to be closed off
                ProjectPlannedEndDate projPlannedEndDate = _projectrepository.GetProjectPlannedEndDate(projectId);

                // *** Need to add a way to clear down the workflow message associated with the task

                // update the status of the workflow planned end date end date row to C
                projPlannedEndDate.Status = "C";
                projPlannedEndDate.LastUpdate = DateTime.Now;
                projPlannedEndDate.UserID = user;




                //save to the database

                _projectrepository.UpdatePlannedEndDate(projPlannedEndDate);
                _projectrepository.Save();
            }
            catch (Exception ex)
            {
                _errorengine.LogError(projectId, ex, user);
            }

        }

        //End Workflow Actions---------------------------------------------------------------------------------------------------------------------------

        #endregion

        #region Risk Register Methods

        public async Task<RiskItemVM> GetRiskItem(Int32 Id, string user)
        {
            RiskService riskService = new RiskService(_projectrepository, _documentService);
            RiskItemVM riskItemVm = await riskService.GetRiskItem(Id, user);

            ProjectMaster project = ReturnProjectMaster(riskItemVm.ProjectID);

            ProjectHeaderVM projectHeaderVm = ReturnProjectHeaderVm(project, user);
            riskItemVm.ProjectHeader = projectHeaderVm;


            if (riskItemVm.Owner != null)
            {
                PersonDetails requesterDetails = await _personService.GetPersonDetails(riskItemVm.Owner);
                if (requesterDetails != null)
                {
                    riskItemVm.OwnerName = requesterDetails.Forename + " " +
                                                          requesterDetails.Surname;

                }
            }

            return riskItemVm;
        }

        public async Task<OverallRiskRatingVM> GetOverallRiskRatingItem(int projectId, string user)
        {
            RiskService riskService = new RiskService(_projectrepository, _documentService);
            OverallRiskRatingVM overallRiskRatingVm = await riskService.GetOverallRiskRatingItem(projectId);
            ProjectMaster project = ReturnProjectMaster(projectId.ToString());
            ProjectHeaderVM projectHeaderVm = ReturnProjectHeaderVm(project, user);

            overallRiskRatingVm.ProjectHeader = projectHeaderVm;
            return overallRiskRatingVm;
        }


        public bool PostRiskRegisterItem(RiskItemVM riskItemVm, string user)
        {
            try
            {
                RiskService riskService = new RiskService(_projectrepository, _documentService);
                riskService.PostRiskRegisterItem(riskItemVm, user);
            }
            catch (Exception exception)
            {
                _errorengine.LogError(riskItemVm.ProjectID, exception, user);
                return false;
            }
            return true;

        }

        public bool PostOverallRiskRating(OverallRiskRatingVM overallRiskRatingVm, string user)
        {
            try
            {
                RiskService riskService = new RiskService(_projectrepository, _documentService);
                riskService.PostOverallRiskRating(overallRiskRatingVm, user);
            }
            catch (Exception ex)
            {
                _errorengine.LogError(overallRiskRatingVm.ProjectID, ex, user);
                return false;
            }
            return true;
        }

        public RiskItemsVM GetRiskTableData(string projectId, string user)
        {
            RiskItemsVM riskItemsVm = new RiskItemsVM();
            RiskService riskService = new RiskService(_projectrepository, _documentService);

            riskItemsVm = riskService.GetRiskTableData(projectId, user);

            return riskItemsVm;
        }

        public RiskDocumentsVM GetRiskDocumentTableData(string projectId, string user)
        {
            RiskDocumentsVM riskDocumentsVm = new RiskDocumentsVM();
            RiskService riskService = new RiskService(_projectrepository, _documentService);

            riskDocumentsVm = riskService.GetRiskDocumentTableData(projectId, user);
            return riskDocumentsVm;
        }

        //
        public bool PostRiskDocument(RiskDocumentVM riskDocumentVm, string user)
        {
            try
            {
                RiskService riskService = new RiskService(_projectrepository, _documentService);
                riskService.PostRiskDocument(riskDocumentVm, user);
            }
            catch (Exception ex)
            {
                _errorengine.LogError(riskDocumentVm.ProjectID, ex, user);
                return false;
            }
            return true;
        }

        public Tuple<bool> RemoveRiskDocument(string projectId, string documentId, string user)
        {
            try
            {
                RiskService riskServieRiskService = new RiskService(_projectrepository, _documentService);
                //riskServieRiskService.RemoveRiskDocument(projectId, documentId);
                Tuple<bool> valuesTuple = new Tuple<bool>(riskServieRiskService.RemoveRiskDocument(projectId, documentId));
                return valuesTuple;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(projectId, ex, user);
                Tuple<bool> failedvaluesTuple = new Tuple<bool>(false);
                return failedvaluesTuple;
                //return false;
            }
            //return true;
        }
        //DeleteRiskDocument
        public Tuple<string> DeleteRiskDocument(string docId, string projectId, string user)
        {
            try
            {
                _projectrepository.DeleteRiskDocument(docId, projectId);
                _projectrepository.Save();
                Tuple<string> valuesTuple = new Tuple<string>("Success");
                return valuesTuple;
            }
            catch (Exception exception)
            {
                Tuple<string> failedvaluesTuple = new Tuple<string>("Failed");
                return failedvaluesTuple;
            }
        }
        public OverallRiskRatingsVM GetOverallRiskRatingTableData(string projectId, string user)
        {
            RiskService riskService = new RiskService(_projectrepository, _documentService);
            OverallRiskRatingsVM overallRiskRatingsVm = riskService.GetOverallRiskRatingTableData(projectId, user);

            return overallRiskRatingsVm;
        }

        public RiskItemVM GetNewRiskItem(string projectId, string user)
        {
            RiskService riskService = new RiskService(_projectrepository, _documentService);
            RiskItemVM newRiskItem = riskService.NewRiskItem(projectId);

            return newRiskItem;
        }

        public OverallRiskRatingVM GetNewOverallRiskRating(string projectId, string user)
        {
            RiskService riskService = new RiskService(_projectrepository, _documentService);
            OverallRiskRatingVM newOverallRiskRating = riskService.NewOverallRiskRating(projectId);
            return newOverallRiskRating;
        }


        #endregion

        #region Non Specific Methods

        private async Task<IEnumerable<ProjectFinanceRecordVM>> PopulateViewModelWithProjectFinancials(String ProjectID, string user)
        {
            //Get the Project Finance Data 
            IEnumerable<ProjectFinanceRecordVM> projectFinance = await _ariesService.GetProjectFinancialsAsync(ProjectID, user) as IEnumerable<ProjectFinanceRecordVM>;

            return projectFinance;
        }

        private async Task<IEnumerable<ProcurementRecordVM>> PopulateViewModelWithProjectProcurement(String ProjectID, string user)
        {
            //Get the Project Finance Data 
            IEnumerable<ProcurementRecordVM> projectProcurement = await _ariesService.GetProjectProcurementAsync(ProjectID, user) as IEnumerable<ProcurementRecordVM>;

            return projectProcurement;

        }

        private async Task<IEnumerable<DocumentRecordVM>> PopulateViewModelWithProjectDocuments(String ProjectID, string user)
        {
            //Get the Project Finance Data 
            IEnumerable<DocumentRecordVM> projectDocuments = await _ariesService.GetProjectDocumentsAsync(ProjectID, user) as IEnumerable<DocumentRecordVM>;

            return projectDocuments;
        }

        private async Task<IEnumerable<PublishedDocumentVM>> PopulateViewModelWithPublishedDocuments(string ProjectID, string user)
        {
            //Get the Project Finance Data 
            IEnumerable<PublishedDocumentVM> publishedDocuments = await _iatiWebService.GetPublishedDocumentListWithStatusAsync(ProjectID, user) as IEnumerable<PublishedDocumentVM>;

            return publishedDocuments;
        }

        private async Task<ProjectEvaluationVM> PopulateViewModelWithProjectEvaluation(String ProjectID, string user)
        {
            ProjectEvaluationVM projectEvaluationVm = new ProjectEvaluationVM();

            ProjectMaster project = ReturnProjectMaster(ProjectID);

            if (project != null)
            {

                projectEvaluationVm.projectHeader = ReturnProjectHeaderVm(project, user);

                //Get the Project Finance Data 
                Evaluation projectEvaluation = _projectrepository.GetProjectEvaluations(ProjectID);

                if (projectEvaluation != null)
                {
                    //Define a mapping between the ProjectDate data object and the ProjectDateVM ViewModel.
                    Mapper.CreateMap<Evaluation, ProjectEvaluationVM>()
                        .ForMember(d => d.EvaluationDocuments, opt => opt.Ignore());

                    //Do the mapping.
                    Mapper.Map(projectEvaluation, projectEvaluationVm);

                    //Manually map ID to UniqueID
                    //projectEvaluationVm.UniqueID = projectEvaluation.ID.ToString();

                    List<EvaluationDocumentVM> evaluationDocumentVms = new List<EvaluationDocumentVM>();

                    //Map the Evaluation documents to the ViewModel
                    foreach (EvaluationDocument document in projectEvaluation.EvaluationDocuments)
                    {
                        EvaluationDocumentVM evaluationDocumentVm = new EvaluationDocumentVM();

                        evaluationDocumentVm.DocumentID = document.DocumentID;
                        evaluationDocumentVm.Description = document.Description;
                        evaluationDocumentVm.EvaluationID = document.EvaluationID;
                        evaluationDocumentVm.ID = document.ID;
                        evaluationDocumentVm.LastUpdate = document.LastUpdate;
                        evaluationDocumentVm.Status = document.Status;
                        evaluationDocumentVm.UserID = evaluationDocumentVm.UserID;

                        evaluationDocumentVms.Add(evaluationDocumentVm);
                    }

                    projectEvaluationVm.EvaluationDocuments = evaluationDocumentVms;

                    //Populate the EvaluationType ViewModel with the values that can be selected. 
                    EvaluationTypeVM evaluationTypeVm = new EvaluationTypeVM();

                    IEnumerable<EvaluationType> evaluationTypes = _projectrepository.LookUpEvaluationTypes();

                    List<EvaluationTypeValuesVM> evaluationTypeValues = new List<EvaluationTypeValuesVM>();

                    foreach (EvaluationType evaluationType in evaluationTypes)
                    {
                        EvaluationTypeValuesVM evaluationTypeValue = new EvaluationTypeValuesVM();

                        evaluationTypeValue.EvaluationTypeID = evaluationType.EvaluationTypeID;
                        evaluationTypeValue.EvaluationTypeDescription = evaluationType.EvaluationDescription;
                        evaluationTypeValues.Add(evaluationTypeValue);
                    }
                    //Order the evaluation types as Impact, Performance,Process, None. Start with an alphabetical sort....
                    evaluationTypeValues = evaluationTypeValues.OrderBy(x => x.EvaluationTypeDescription).ToList();

                    //Then move none...
                    Int32 index = evaluationTypeValues.FindIndex(x => x.EvaluationTypeDescription.ToUpper() == "NONE");
                    EvaluationTypeValuesVM objectToMove = evaluationTypeValues[index];
                    evaluationTypeValues.RemoveAt(index);
                    evaluationTypeValues.Add(objectToMove);

                    evaluationTypeVm.EvaluationTypeValues = evaluationTypeValues;
                    evaluationTypeVm.SelectedEvaluationType = projectEvaluation.EvaluationTypeID;

                    projectEvaluationVm.EvaluationTypes = evaluationTypeVm;

                    //Populate the EvaluationManagement ViewModel with values that can be selected.
                    EvaluationManagementVM evaluationManagementVm = new EvaluationManagementVM();

                    IEnumerable<EvaluationManagement> evaluationManagements =
                        _projectrepository.LookUpEvaluationManagements();

                    List<EvaluationManagementValuesVM> evaluationManagementValues =
                        new List<EvaluationManagementValuesVM>();

                    foreach (EvaluationManagement evaluationManagement in evaluationManagements)
                    {
                        EvaluationManagementValuesVM evaluationManagementValue = new EvaluationManagementValuesVM();

                        evaluationManagementValue.EvaluationManagementID = evaluationManagement.EvaluationManagementID;
                        evaluationManagementValue.EvaluationManagementDescription =
                            evaluationManagement.EvaluationManagementDescription;
                        evaluationManagementValues.Add(evaluationManagementValue);
                    }

                    evaluationManagementVm.EvaluatioNManagementValues = evaluationManagementValues;

                    evaluationManagementVm.SelectedEvaluationManagement = projectEvaluation.ManagementOfEvaluation;

                    projectEvaluationVm.EvaluationManagements = evaluationManagementVm;

                }

            }

            return projectEvaluationVm;
        }

        private async Task<IEnumerable<StatementRecordVM>> PopulateViewModelWithProjectStatements(String ProjectID, string user)
        {

            IEnumerable<StatementRecordVM> projectStatementVm;

            //Get the Project Finance Data 
            IEnumerable<AuditedFinancialStatement> projectStatement = _projectrepository.GetActiveAuditedStatements(ProjectID) as IEnumerable<AuditedFinancialStatement>;

            //Define a mapping between the ProjectDate data object and the ProjectDateVM ViewModel.
            Mapper.CreateMap<AuditedFinancialStatement, StatementRecordVM>();

            //Do the mapping.
            projectStatementVm = Mapper.Map<IEnumerable<AuditedFinancialStatement>, IEnumerable<StatementRecordVM>>(projectStatement);

            foreach (StatementRecordVM statement in projectStatementVm)
            {
                if (statement.DocumentID != null && statement.DocumentID != "0")
                {
                    statement.DocumentLink = _documentService.ReturnDocumentUrl(statement.DocumentID,
                        statement.DocSource);
                }
                else
                {
                    statement.DocumentLink = "";
                }
            }

            //...and populate the ViewModel
            return projectStatementVm;
        }
        private async Task<IEnumerable<ComponentFinanceRecordVM>> PopulateViewModelWithComponentFinancials(String ComponentID, string user)
        {
            //Get the Project Finance Data 
            IEnumerable<ComponentFinanceRecordVM> componentfinance = await _ariesService.GetComponentFinancialsAsync(ComponentID, user);

            return componentfinance;

        }
        /// <summary>
        /// Calls the ARIES Service and gets the Project Approved Budget
        /// </summary>
        /// <param name="ProjectID">The Project for which budget should be returned</param>
        /// <param name="projectViewModel">The ViewModel to be populated with budget data</param>
        /// <returns>ViewModel populated with Budget data</returns>
        private async Task PopulateViewModelWithApprovedBudget(String ProjectID, ProjectViewModel projectViewModel, string user)
        {
            //Get the project Approved Budget from the AriesService.
            ProjectApprovedBudget projectApprovedBudget = await _ariesService.GetProjectApprovedBudgetAsync(ProjectID, user); // Change this to named method



            //Load the Approved project budget into the ProjectDetailsViewModel.
            projectViewModel.ApprovedBudget = projectApprovedBudget.ApprovedBudget;



        }

        private static ProjectViewModel HandleARIESGetBudgetWebServiceException(ProjectViewModel projectViewModel)
        {
            //Log the Exception. TO DO!!!

            //No need to crash. Set the Budget to zero and return the ViewModel
            projectViewModel.ApprovedBudget = 0;

            //Extract this to a constants file or similar.
            string message = "Sorry, ARIES doesn't seem to be available just now. Your approved budget cannot be shown.";

            AddWebServiceMessage(projectViewModel, message);

            return projectViewModel;
        }

        private static void AddWebServiceMessage(ProjectViewModel projectViewModel, string WebServiceErrorMessage)
        {
            projectViewModel.FinanceWebServiceMessage = WebServiceErrorMessage;
        }

        private static ProjectViewModel HandleARIESGetFinancialsWebServiceException(ProjectViewModel projectViewModel)
        {
            //Log the Exception. TO DO!!!

            //Set the Financials to null and return the ViewModel
            List<ProjectFinanceRecordVM> finances = new List<ProjectFinanceRecordVM>();
            projectViewModel.ProjectFinance = finances;

            //Extract this to a constants file or similar.
            string message = "Sorry, ARIES doesn't seem to be available just now. Your finance data cannot be shown.";

            AddWebServiceMessage(projectViewModel, message);

            return projectViewModel;
        }

        private static ComponentViewModel HandleARIESGetComponentFinancialsWebServiceException(ComponentViewModel componentviewmodel)
        {
            //Log the Exception. TO DO!!!

            //Set the Financials to null and return the ViewModel
            List<ComponentFinanceRecordVM> finances = new List<ComponentFinanceRecordVM>();
            componentviewmodel.ComponentFinance = finances;

            //Extract this to a constants file or similar.
            string message = "Sorry, ARIES doesn't seem to be available just now. Your finance data cannot be shown.";

            componentviewmodel.FinanceWebServiceMessage = message;

            return componentviewmodel;
        }

        private static double CalculateImpactScore(int weight, string OutputScore)
        {

            var outputScoreWeight = 0.0;

            switch (OutputScore.Trim())
            {
                case "A++":
                    outputScoreWeight = 150;
                    break;
                case "A+":
                    outputScoreWeight = 125;
                    break;

                case "A":
                    outputScoreWeight = 100;
                    break;
                case "B":
                    outputScoreWeight = 75;
                    break;
                case "C":
                    outputScoreWeight = 50;
                    break;
            }

            //Formula to calculate Impact Score [weight] * [Output Score]/100
            return weight * outputScoreWeight / 100;

        }

        private string CalculateaReviewOverallScoreAndGetBand(IEnumerable<ReviewOutput> reviewoutputs)
        {
            var totalImpactScore = 0.0;
            string band = "";

            foreach (var reviewoutput in reviewoutputs)
            {
                Debug.Assert(reviewoutput.ImpactScore != null, "reviewoutput.ImpactScore != null in CalculateaReviewOverallScoreAndUpdate");
                totalImpactScore += (double)reviewoutput.ImpactScore;
            }

            if (totalImpactScore >= 137.6)
                band = "A++";
            else if (totalImpactScore >= 112.6 && totalImpactScore <= 137.5)
                band = "A+";
            else if (totalImpactScore >= 87.5 && totalImpactScore <= 112.5)
                band = "A";
            else if (totalImpactScore >= 62.5 && totalImpactScore <= 87.4)
                band = "B";
            else if (totalImpactScore <= 61.4)
                band = "C";

            return band;
        }

        private string CalculateaReviewAggregatedRisk(IEnumerable<ReviewOutput> reviewoutputs)
        {
            var riskScoreTotal = 0.0;
            string aggregatedScore = "";


            foreach (var reviewoutput in reviewoutputs)
            {
                //Risk Low = 1, Medium = 2 , High = 3 
                //Formula AggregatedRisk += (Impact Weight/100 )* Risk
                if (reviewoutput.Risk.Equals("L"))
                {
                    if (reviewoutput.Weight != null) riskScoreTotal += (double)reviewoutput.Weight / 100 * 1;
                }

                if (reviewoutput.Risk.Equals("M"))
                {
                    if (reviewoutput.Weight != null) riskScoreTotal += (double)reviewoutput.Weight / 100 * 2;
                }

                if (reviewoutput.Risk.Equals("H"))
                {
                    if (reviewoutput.Weight != null) riskScoreTotal += (double)reviewoutput.Weight / 100 * 3;
                }
            }

            if (riskScoreTotal < 1.67)
            {
                aggregatedScore = "L";
            }
            else if (1.67 <= riskScoreTotal && riskScoreTotal < 2.33)
            {
                aggregatedScore = "M";
            }

            else if (2.33 <= riskScoreTotal && riskScoreTotal <= 3)
            {
                aggregatedScore = "H";
            }

            return aggregatedScore;
        }


        private string CalculateProjectScore(IEnumerable<ReviewOutput> reviewoutputs, string projectID, int reviewId)
        {


            var ProjectScore = 0.0;

            foreach (var reviewoutput in reviewoutputs)
            {
                if (reviewoutput.Weight != null)
                {
                    double impactWeightingPercentage = (double)reviewoutput.Weight / 100;

                    switch (reviewoutput.OutputScore)
                    {
                        case "A++":
                            ProjectScore += 150 * impactWeightingPercentage;
                            break;

                        case "A+":
                            ProjectScore += 125 * impactWeightingPercentage;
                            break;

                        case "A":
                            ProjectScore += 100 * impactWeightingPercentage;
                            break;

                        case "B":
                            ProjectScore += 75 * impactWeightingPercentage;
                            break;

                        case "C":
                            ProjectScore += 50 * impactWeightingPercentage;
                            break;
                    }

                }

            }

            //save ReviewScore in the Review Master table . This data will be used for reporting purposes. 
            ReviewMaster reviewMaster = new ReviewMaster();
            reviewMaster = _projectrepository.GetReview(projectID, reviewId);
            var reviewScore = String.Format("{0:0.0}", Math.Round(ProjectScore, 1));
            reviewMaster.ReviewScore = Convert.ToDecimal(reviewScore);
            _projectrepository.UpdateReview(reviewMaster);
            _projectrepository.Save();

            return reviewScore;
        }

        #endregion

        #region LookUp Methods
        /// <summary>
        /// LookupBudgetCentreKV - Get a list of BudgetCentre objects from the database and map them to a List of BudgetCentreKV pairs.
        /// </summary>
        /// <returns>
        /// A List of BudgetCentreKV objects that feed a JSON typeahead search.
        /// </returns>
        public List<BudgetCentreKV> LookupBudgetCentreKV()
        {
            List<BudgetCentre> budgetcentres = _projectrepository.LookUpBudgetCentre().ToList();

            List<BudgetCentreKV> budgetcentreKVpairs = new List<BudgetCentreKV>();

            //Map the Entity Date Model <Document> to the ViewModel <ProjectDocumentVM>
            //Mapper.CreateMap<Document, ProjectDocumentVM>().ForMember(
            //    dest => dest.DocumentDescription,
            //    opts => opts.MapFrom(src => string.Format("{0} ({1})", src.DocumentType, src.DocumentID))
            //    );

            //AutoMapper time!

            Mapper.CreateMap<BudgetCentre, BudgetCentreKV>().ForMember(
                dest => dest.BudgetCentreDescription,
                opts => opts.MapFrom(src => string.Format("{0} {1}", src.BudgetCentreDescription, src.BudgetCentreID))
                );

            budgetcentreKVpairs = Mapper.Map<List<BudgetCentre>, List<BudgetCentreKV>>(budgetcentres);

            return budgetcentreKVpairs;
        }

        public async Task<List<DeliveryChainListVM>> LookUpPartners(string id, string user)
        {
            ComponentPartnerVM componentPartnerVm = new ComponentPartnerVM();
            List<DeliveryChain> deliveryChain = _projectrepository.LookUpDeliveryChains(id, user).ToList();

            ComponentPartnerVMBuilder componentPartnerVmBuilder = new ComponentPartnerVMBuilder(_projectrepository, _ariesService, id);
            componentPartnerVm = await componentPartnerVmBuilder.Build();

            return componentPartnerVm.DeliveryChainsVm.deliveryChains;

           

        }


        //**
        public async Task<AllReturnedPartnerListsVM> LookUpPartnerSearchList(string searchString)
        {

   
            // AMP Held Partners
            List<PartnerMaster> partnermaster = _projectrepository.LookUpAMPPartnerSearchList(searchString).ToList();

            List<PartnerMasterKVP> AMPSearchResults = new List<PartnerMasterKVP>();

            foreach (PartnerMaster result in partnermaster)
            {
                PartnerMasterKVP newResult = new PartnerMasterKVP();
                newResult.PartnerID = result.PartnerID;
                newResult.ID = result.ID;
                newResult.PartnerName = result.PartnerName;
                AMPSearchResults.Add(newResult);
            }

            ////DFID Supplier Portal Held Partners  

            List<string> listOfSuppliers = new List<string>(searchString.Split(','));
            IEnumerable<SupplierVM> suppliermaster = await _ariesService.GetSearchSuppliers(listOfSuppliers, "");
                // this returns all suppliers

            List<SupplierMasterKVP> DFIDSearchResults = new List<SupplierMasterKVP>();

            foreach (SupplierVM result1 in suppliermaster)
            {
                SupplierMasterKVP newSupplierResult = new SupplierMasterKVP();
                newSupplierResult.SupplierID = result1.SupplierID;
                newSupplierResult.SupplierName = result1.SupplierName;
                DFIDSearchResults.Add(newSupplierResult);
           }

                // Now we need to merge the two sets of results
                AllReturnedPartnerListsVM MergedSearchResults = new AllReturnedPartnerListsVM();
                MergedSearchResults.AMPPartnerSearchResult = AMPSearchResults;
                MergedSearchResults.DFIDPartnersSearchResult = DFIDSearchResults;

                return MergedSearchResults;
            }
        



        public List<PartnerMaster> LookUpPartnerList()
        {

            List<PartnerMaster> partners = _projectrepository.LookUpPartnerList().ToList();

            return partners;

        }

       
        public List<ProjectKV> LookUpProjectMaster()
        {
            List<ProjectMaster> projectmaster = _projectrepository.LookUpProjectMaster().ToList();

            List<ProjectKV> projectmasterKVpairs = new List<ProjectKV>();

            //AutoMapper time!
            Mapper.CreateMap<ProjectMaster, ProjectKV>();

            projectmasterKVpairs = Mapper.Map<List<ProjectMaster>, List<ProjectKV>>(projectmaster);

            foreach (ProjectKV x in projectmasterKVpairs)
            {
                x.Title = string.Format("{0} - {1}", x.Title, x.ProjectID);

            }

            return projectmasterKVpairs;
        }

        // Loop through projectApprovedBudgets and map the approved budget value
        // Start loop
        //foreach (UserProjectsViewModel userproject in userprojectviewmodel)
        //{
        //    //Get current row of ProjectData
        //    var projectdashboard = userprojectviewmodel.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID));

        //    //Get Current Row of ProjectBudget
        //    // Check to see if the result will be null, if it is then there is no budget so dont map, mapping will throw exception
        //    if (projectApprovedBudgets.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID)) != null)
        //    {
        //        var projectbudget = projectApprovedBudgets.FirstOrDefault(x => x.ProjectID.Equals(userproject.ProjectID));

        //        // Update the approved budget to the projectdashboard model.
        //        projectdashboard.ApprovedBudget = projectbudget.ApprovedBudget;
        //    }

        //}

        public List<UserLookUp> LookUpUsers()
        {
            List<UserLookUp> users = _projectrepository.LookUpUsers().ToList();

            return users;
        }

        public async Task<IEnumerable<PersonDetails>> LookUpSroUser()
        {
            List<string> SROs = _projectrepository.LookUpSRO().ToList();
            IEnumerable<PersonDetails> personDetails = await _personService.GetAllCurrentStaff();
            personDetails = personDetails.Where(x => SROs.Contains(x.EmpNo)).Distinct();
            return personDetails;
        }

        public List<Stage> GetProjectStages()
        {
            List<Stage> stages = _projectrepository.GetStages().ToList();
            return stages;
        }

        public List<BenefitingCountry> GetBenefitingCountry()
        {
            List<BenefitingCountry> benefitingCountries = _projectrepository.LookUpBenefitingCountrys().ToList();
            return benefitingCountries;
        }

        public List<FundingMech> LookUpFundingMechs()
        {
            List<FundingMech> fundingMechs = _projectrepository.LookUpFundingMechs().ToList();

            return fundingMechs;
        }

        public async Task<List<CurrencyVM>> LookUpCurrency()
        {
            List<CurrencyVM> currency = await _ariesService.GetCurrency() as List<CurrencyVM>;

            return currency;
        }

        public List<InputSectorKV> LookupInputSectorKV(String FundingMech)
        {
            List<InputSector> inputsectors = _projectrepository.LookUpInputSector(FundingMech).ToList();

            List<InputSectorKV> inputsectorKVpairs = new List<InputSectorKV>();

            Mapper.CreateMap<InputSector, InputSectorKV>();

            inputsectorKVpairs = Mapper.Map<List<InputSector>, List<InputSectorKV>>(inputsectors);

            foreach (InputSectorKV sector in inputsectorKVpairs)
            {
                sector.InputSectorCodeDescription = sector.InputSectorCodeDescription + " - " + sector.InputSectorCodeID;
            }

            return inputsectorKVpairs;
        }

        public List<Risk> LookupRisksTypes()
        {

            List<Risk> riskLookups = _projectrepository.LookUpRiskTypes().ToList();

            return riskLookups;
        }



        #endregion

        #region Validations
        private bool ValidateProject(ProjectVM projectToValidate, IValidationDictionary validationDictionary)
        {
            if (projectToValidate.Title == null)
            {
                //validationDictionary.AddError("Title", String.Format("Title is required", projectToValidate.Title));
                validationDictionary.AddError("projectToValidate.Title", "Title is required");
                return validationDictionary.IsValid;
            }


            if (projectToValidate.Title.Length < 20 || projectToValidate.Title.Length > 250)
            {
                validationDictionary.AddError("projectToValidate.Title", "Title must be greater than 20 characters and less than 250");
                return validationDictionary.IsValid;
            }

            //Description
            if (projectToValidate.Description == null)
            {
                validationDictionary.AddError("projectToValidate.Description", "Description required");
                return validationDictionary.IsValid;
            }


            if (projectToValidate.Description.Length < 100 || projectToValidate.Description.Length > 1000)
            {
                validationDictionary.AddError("projectToValidate.Description", "Description must be greater than 100 characters and less than 1000");
                return validationDictionary.IsValid;
            }

            //Budget Centre ID
            if (projectToValidate.BudgetCentreID == null)
            {
                validationDictionary.AddError("projectToValidate.BudgetCentreID", "Budget centre required");
                return validationDictionary.IsValid;
            }


            // Check For PI
            IEnumerable<Team> projectTeam = _projectrepository.GetTeam(projectToValidate.ProjectID);

            int InputterExists = 0;

            foreach (Team team in projectTeam.Where(x => x.Status == "A"))
            {

                if (team.RoleID == "PI")
                {
                    InputterExists = 1;
                }

            }

            if (InputterExists != 1)
            {
                validationDictionary.AddError("Summary", "This project has no Inputter - an Inputter must be added before changes can be saved");
            }


            IEnumerable<ComponentMaster> componentMasters = _projectrepository.GetComponents(projectToValidate.ProjectID);

            if (componentMasters.Any())
            {
                List<ComponentDate> componentDates = new List<ComponentDate>();

                foreach (ComponentMaster componentMaster in componentMasters)
                {
                    if (componentMaster.ComponentDate != null)
                    {
                        componentDates.Add(componentMaster.ComponentDate);
                    }
                }

                //Component Dates
                foreach (ComponentDate componentDate in componentDates)
                {
                    if (componentDate.OperationalEndDate > projectToValidate.ProjectDates.OperationalEndDate)
                    {
                        validationDictionary.AddError("projectToValidate.OperationalEndDate", String.Format("Update end date of Component {0} before changing the project Planned End Date", componentDate.ComponentID));
                    }

                }
                if (!validationDictionary.IsValid)
                {
                    return validationDictionary.IsValid;
                }

                //Project End Date cannot be before any Component End Date

            }





            //if (projectToValidate.ProjectDates.OperationalEndDate < DateTime.Now)
            //{
            //    validationDictionary.AddError("ProjectDates.OperationalEndDate", String.Format("Planned end date cannot be earlier than today", projectvm.ProjectDates.OperationalEndDate));
            //    return validationDictionary.IsValid;
            //}


            //if (projectToValidate.ProjectDates.OperationalStartDate < DateTime.Today)
            //{
            //    validationDictionary.AddError("ProjectDates.OperationalStartDate", String.Format("Planned start date cannot be earlier than today", projectvm.ProjectDates.OperationalStartDate));
            //    return validationDictionary.IsValid;
            //}

            //if (projectToValidate.ProjectDates.OperationalEndDate < projectToValidate.ProjectDates.OperationalStartDate)
            //{
            //    validationDictionary.AddError("ProjectDates.OperationalEndDate", String.Format("Planned end date cannot be earlier than today", projectvm.ProjectDates.OperationalEndDate));
            //    return validationDictionary.IsValid;
            //}

            switch (projectToValidate.Stage)
            {
                case "5": //
                    {
                        if (projectToValidate.ProjectDates.OperationalStartDate < DateTime.Today)
                        {
                            validationDictionary.AddError("ProjectDates.OperationalStartDate", "Planned Start Date cannot be earlier than today");
                            return validationDictionary.IsValid;
                        }

                        if (projectToValidate.ProjectDates.OperationalStartDate >= projectToValidate.ProjectDates.OperationalEndDate) //This also covers planned end date must be later than planned start date.
                        {
                            validationDictionary.AddError("ProjectDates.OperationalStartDate", "Planned Start Date cannot be after the Planned End Date");
                            return validationDictionary.IsValid;
                        }

                        if (projectToValidate.ProjectDates.OperationalEndDate < DateTime.Today) //This also covers planned end date must be later than planned start date.
                        {
                            validationDictionary.AddError("ProjectDates.OperationalEndDate", "Planned Start Date cannot be earlier than today");
                            return validationDictionary.IsValid;
                        }

                        return validationDictionary.IsValid;

                    }
                default:
                    { return validationDictionary.IsValid; }
            }
        }
        // Check to see if the disability percentage field has valid entry
        private bool ValidateDisabilityMarkerPercentage(ProjectMarkersVM projectMarkers,
            IValidationDictionary validationDictionary)
        {
            // For PRINCIPAL marker the default percentage value is 100 and no other validation is needed
            if (projectMarkers.DisabilityCCO.SelectedCCOValue == "PRINCIPAL")
            {
                projectMarkers.DisabilityPercentage = 100;
                return validationDictionary.IsValid;
            }
            // For NOT TARGETED marker the default percentage value is 0 and no other validation needed
            if (projectMarkers.DisabilityCCO.SelectedCCOValue == "NOTTARGETED")
            {
                projectMarkers.DisabilityPercentage = 0;
                return validationDictionary.IsValid;
            }

            // if marker is SIGNIFICANT we need to check the percentage entereed by user is populated and valid
            if (projectMarkers.DisabilityCCO.SelectedCCOValue == "SIGNIFICANT")
            {
                if (projectMarkers.DisabilityPercentage == null) // the percentage field must not be blank
                {
                    validationDictionary.AddError("ProjectMarkers.Disability Percentage",
                        "Disability percentage must have a valid value entered if the SIGNIFICANT marker is selected");
                    return validationDictionary.IsValid;
                }
                if (projectMarkers.DisabilityPercentage > 99 || projectMarkers.DisabilityPercentage < 1) // check that it is between 1 and 99              
                {
                    validationDictionary.AddError("ProjectMarkers.DisabilityPercentage",
                       "Disability percentage must be a value between 1 and 99");
                    return validationDictionary.IsValid;
                }
                // check that it is a whole number
                int i = Convert.ToInt32(projectMarkers.DisabilityPercentage);
                if (!int.TryParse(projectMarkers.DisabilityPercentage.ToString(), out i))
                {
                    validationDictionary.AddError("ProjectMarkers.DisabilityPercentage",
                    String.Format(" Disability Percentage needs to be a whole number.", projectMarkers.DisabilityPercentage));
                    return validationDictionary.IsValid;
                }
            }
            return validationDictionary.IsValid;
        }
        private bool ValidatePlannedEndDateChange(ProjectVM projectViewModel,
            IValidationDictionary validationDictionary)
        {
            ProjectDate originalDates = _projectrepository.GetProjectDates(projectViewModel.ProjectID);


            if (projectViewModel.ProjectDates.OperationalEndDate != originalDates.OperationalEndDate)
            {
                if (projectViewModel.ProjectDates.ActualEndDate != null)
                {
                    validationDictionary.AddError("ProjectDates.OperationalEndDate",
    "Planned End Date cannot be changed on a reopened project");
                    return validationDictionary.IsValid;
                }

                if (projectViewModel.ProjectDates.OperationalEndDate < DateTime.Today)
                {
                    validationDictionary.AddError("ProjectDates.OperationalEndDate",
                        "Planned End Date cannot be earlier than today");
                    return validationDictionary.IsValid;
                }
            }

            return validationDictionary.IsValid;
        }

        private bool ValidateCreateProject(ProjectVM projectvm, IValidationDictionary validationDictionary)
        {
            //Title
            if (projectvm.Title == null)
            {
                validationDictionary.AddError("Title", String.Format("Title is required", projectvm.Title));
                return validationDictionary.IsValid;
            }


            if (projectvm.Title.Length < 20 || projectvm.Title.Length > 250)
            {
                validationDictionary.AddError("Title", String.Format("Ttile must be greater than 20 characters and less than 250", projectvm.Title));
                return validationDictionary.IsValid;
            }

            //Description
            if (projectvm.Description == null)
            {
                validationDictionary.AddError("Description", String.Format("Description is required", projectvm.Description));
                return validationDictionary.IsValid;
            }


            if (projectvm.Description.Length < 100 || projectvm.Description.Length > 1000)
            {
                validationDictionary.AddError("Description", String.Format("Description must be greater than 100 characters and less than 1000", projectvm.Description));
                return validationDictionary.IsValid;
            }


            //Budget Centre ID
            if (projectvm.BudgetCentreID == null)
            {
                validationDictionary.AddError("BudgetCentreID", String.Format("Budget Centre cant be blank", projectvm.BudgetCentreID));
                return validationDictionary.IsValid;
            }

            //Dates
            if (projectvm.ProjectDates.OperationalEndDate < DateTime.Now)
            {
                validationDictionary.AddError("ProjectDates.OperationalEndDate", String.Format("Planned end date cannot be earlier than today", projectvm.ProjectDates.OperationalEndDate));
                return validationDictionary.IsValid;
            }


            if (projectvm.ProjectDates.OperationalStartDate < DateTime.Today)
            {
                validationDictionary.AddError("ProjectDates.OperationalStartDate", String.Format("Planned start date cannot be earlier than today", projectvm.ProjectDates.OperationalStartDate));
                return validationDictionary.IsValid;
            }

            if (projectvm.ProjectDates.OperationalEndDate < projectvm.ProjectDates.OperationalStartDate)
            {
                validationDictionary.AddError("ProjectDates.OperationalEndDate", String.Format("Planned end date cannot be earlier than today", projectvm.ProjectDates.OperationalEndDate));
                return validationDictionary.IsValid;
            }

            return validationDictionary.IsValid;
        }

        private bool ValidateAddTeam(ProjectTeamVM teamToValidate, IEnumerable<Team> currentTeam, IValidationDictionary validationDictionary)
        {

            //Since I made the Start Date read only, the date parts aren't coming though, but the full Start Date is. Use that instead for validation since users cant change it. No point in validating it.

            //Validate Start Date day, month and year 
            //if (!IsValidDate(teamToValidate.NewTeamMember.StartDate_Day, teamToValidate.NewTeamMember.StartDate_Month, teamToValidate.NewTeamMember.StartDate_Year, "NewTeamMember.StartDate", validationDictionary))
            //    return validationDictionary.IsValid;

            //Is fully constructed date a real date?
            //DateTime startDate = new DateTime(teamToValidate.NewTeamMember.StartDate_Year, teamToValidate.NewTeamMember.StartDate_Month, teamToValidate.NewTeamMember.StartDate_Day);



            ////Check for Team member + role within the same timeframe
            // int duplicateRoleCheck=0;

            // try
            // {
            // //Do check (If the check fails it will go to the catch, this will be due to not finding a matching team member)
            // Team teamcheck = currentTeam.Where(x => x.RoleID == teamToValidate.NewTeamMember.RoleID && x.TeamID == teamToValidate.NewTeamMember.TeamID && x.StartDate >= startDate).First();
            // duplicateRoleCheck = 1;
            // validationDictionary.AddError("NewTeamMember.RoleID", "Team Member already exists with this role within the selected timeframe.");
            // return validationDictionary.IsValid;
            // }
            // catch
            // {
            // duplicateRoleCheck = 0;
            // }


            //Business Rule: Only one active SRO and one active QA at a time. Other roles can have multiple active staff assigned to them.
            if (teamToValidate.NewTeamMember.RoleID == "SRO" || teamToValidate.NewTeamMember.RoleID == "QA")
            {
                if (currentTeam.Any(x => x.RoleID == teamToValidate.NewTeamMember.RoleID && x.Status == "A"))
                {
                    validationDictionary.AddError("NewTeamMember.RoleID", String.Format("{0} cannot be added as someone already has that role on the project.", teamToValidate.NewTeamMember.RoleID));
                    return validationDictionary.IsValid;
                }

            }

            //Validation: A user must have been selected. Cannot use data annotation on the ViewModel as it triggers validaton when saving a Team Marker.
            if (teamToValidate.NewTeamMember.TeamID == null)
            {
                validationDictionary.AddError("NewTeamMember.TeamID", String.Format("You must select a team member.", teamToValidate.NewTeamMember.TeamID));
                return validationDictionary.IsValid;
            }
            //Validation: A role must have been selected. Cannot do this using Annotation in the ViewModel as the role is selected from radio buttons, the RoleID held in the VM is the current role. Not added until the update has been saved.
            if (teamToValidate.NewTeamMember.ProjectRolesVm == null || teamToValidate.NewTeamMember.ProjectRolesVm.SelectedRoleValue == null)
            {
                validationDictionary.AddError("NewTeamMember.RoleID", String.Format("You must select a role.", teamToValidate.NewTeamMember.RoleID));
                return validationDictionary.IsValid;
            }
            return validationDictionary.IsValid;
        }

        private bool ValidateCreateComponent(ComponentVM componentvm, IValidationDictionary validationDictionary)
        {
            if (Constants.ComponentDescriptionIsRequired && componentvm.ComponentDescription == null)
            {
                validationDictionary.AddError("ComponentDescription", String.Format("Description is required", componentvm.ComponentDescription));
                return validationDictionary.IsValid;
            }

            if (componentvm.ComponentDescription.Length < Constants.ComponentDescriptionMinLength || componentvm.ComponentDescription.Length > Constants.ComponentDescriptionMaxLength)
            {
                validationDictionary.AddError("ComponentDescription", String.Format("Description must be greater than {0} characters and less than {1}", Constants.ComponentDescriptionMinLength, Constants.ComponentDescriptionMaxLength, componentvm.ComponentDescription));
                return validationDictionary.IsValid;
            }


            if (componentvm.BudgetCentreID == null)
            {
                validationDictionary.AddError("BudgetCentreID", String.Format("You must select a Budget Centre", componentvm.BudgetCentreID));
                return validationDictionary.IsValid;
            }

            //If its an admin component check for admin approver.
            if (componentvm.BudgetCentreID.Contains("A0") || componentvm.BudgetCentreID.Contains("C0") || componentvm.BudgetCentreID.Contains("AP"))
            {
                if (componentvm.AdminApprover.IsNullOrWhiteSpace())
                {

                    validationDictionary.AddError("AdminApprover", String.Format("You must select an Admin approver", componentvm.AdminApprover));
                    return validationDictionary.IsValid;
                }
            }

            if (string.IsNullOrEmpty(componentvm.FundingMechanism))
            {
                validationDictionary.AddError("FundingMechanism", String.Format("You must select a Funding Mechanism", componentvm.FundingMechanism));
                return validationDictionary.IsValid;

            }

            ProjectMaster currentproject = _projectrepository.GetProject(componentvm.ProjectID);

            if (componentvm.ComponentDate.OperationalEndDate.Value.Date > currentproject.ProjectDate.OperationalEndDate.Value.Date)
            {
                validationDictionary.AddError("ComponentDate.OperationalEndDate", String.Format("End date cannot be greater than project end date", componentvm.ComponentDate.OperationalEndDate));
                return validationDictionary.IsValid;
            }

            //Component Start Date cannot be before the Project Operational Start Date.
            if (componentvm.ComponentDate.OperationalStartDate.Value.Date < currentproject.ProjectDate.OperationalStartDate.Value.Date)
            {
                validationDictionary.AddError("ComponentDate.OperationalStartDate", String.Format("Start date cannot be before the project operational start date", componentvm.ComponentDate.OperationalStartDate));
                return validationDictionary.IsValid;
            }



            //Component End Date cannot be before start date.
            if (componentvm.ComponentDate.OperationalEndDate.Value.Date < componentvm.ComponentDate.OperationalStartDate.Value.Date)
            {
                validationDictionary.AddError("ComponentDate.OperationalEndDate", String.Format("End date cannot be earlier than start", componentvm.ComponentDate.OperationalEndDate));
                return validationDictionary.IsValid;
            }


            // Check For PI
            IEnumerable<Team> projectTeam = _projectrepository.GetTeam(currentproject.ProjectID);

            int InputterExists = 0;

            foreach (Team team in projectTeam.Where(x => x.Status == "A"))
            {

                if (team.RoleID == "PI")
                {
                    InputterExists = 1;
                }

            }

            if (InputterExists != 1)
            {
                validationDictionary.AddError("Summary", "This project has no Inputter - an Inputter must be added before changes can be saved");
            }

            return validationDictionary.IsValid;
        }

        private bool ValidateComponentOnApproval(ComponentMaster componentMaster, IValidationDictionary validationDictionary)
        {
            //

            return validationDictionary.IsValid;
        }
        private bool ValidateMarkers(ComponentMarkersVM componentMarkervm, IValidationDictionary validationDictionary)
        {
            if (componentMarkervm.BudgetCentreID.StartsWith("A") == false && componentMarkervm.BenefitingCountry == null)
            {
                validationDictionary.AddError("BenefitingCountry", String.Format("Benefiting Country is required", componentMarkervm.BenefitingCountry));
                return validationDictionary.IsValid;
            }

            return validationDictionary.IsValid;
        }

        private bool ValidateAddDocument(ProjectEvaluationVM projectEvaluationVm, IValidationDictionary validationDictionary)
        {
            //Validation. Document ID must be present.
            if (String.IsNullOrEmpty(projectEvaluationVm.NewEvaluationDocument.DocumentID))
            {
                validationDictionary.AddError("NewEvaluationDocument.DocumentID", String.Format("You must enter a document number.", projectEvaluationVm.NewEvaluationDocument.DocumentID));
                return validationDictionary.IsValid;
            }

            //Validation Document ID must be length 6 to 12.
            if (projectEvaluationVm.NewEvaluationDocument.DocumentID.Length < 6 || projectEvaluationVm.NewEvaluationDocument.DocumentID.Length > 12)
            {
                validationDictionary.AddError("NewEvaluationDocument.DocumentID", String.Format("Document ID must be between 6 & 12 digits.", projectEvaluationVm.NewEvaluationDocument.DocumentID));
                return validationDictionary.IsValid;
            }

            //Validation. Document ID must be numeric. Cast it to an Int. - REMOVED DUE TO 12 DIGIT STRINGS NOT CASTING TO INT PROPERLY
            long intOut;
            if (!Int64.TryParse(projectEvaluationVm.NewEvaluationDocument.DocumentID, out intOut))
            {
                validationDictionary.AddError("NewEvaluationDocument.DocumentID", String.Format("Document ID must be a number.", projectEvaluationVm.NewEvaluationDocument.DocumentID));
                return validationDictionary.IsValid;
            }

            //Validation. Description must be a minimum of x characters.
            if (projectEvaluationVm.NewEvaluationDocument.Description == null || projectEvaluationVm.NewEvaluationDocument.Description.Length < 6)
            {
                validationDictionary.AddError("NewEvaluationDocument.Description", String.Format("Document description must be at least 6 characters.", projectEvaluationVm.NewEvaluationDocument.Description));
                return validationDictionary.IsValid;
            }


            return validationDictionary.IsValid;
        }

        private bool ValidateEvaluationUpdate(ProjectEvaluationVM projectEvaluationVm, IValidationDictionary validationDictionary)
        {
            //Is the Evaluation type 'None'? If so, dont bother with any other validations.
            if (projectEvaluationVm.EvaluationTypes.SelectedEvaluationType != "5")
            {

                //Estimated Cost cannot be null if the Evaluation type is anything other than none.
                if (projectEvaluationVm.EstimatedBudget == null)
                {
                    validationDictionary.AddError("EstimatedBudget", String.Format("Estimated cost must be greater than zero.", projectEvaluationVm.EstimatedBudget));
                    return validationDictionary.IsValid;
                }

                //Estimated Cost must be greater than zero.
                if (projectEvaluationVm.EstimatedBudget < 0)
                {
                    validationDictionary.AddError("EstimatedBudget", String.Format("Estimated cost must be greater than zero.", projectEvaluationVm.EstimatedBudget));
                    return validationDictionary.IsValid;
                }

                if (projectEvaluationVm.AdditionalInfo != null && projectEvaluationVm.AdditionalInfo.Length > 1000)
                {
                    validationDictionary.AddError("AdditionalInfo", String.Format("Additional comments must be less than 1000 characters.", projectEvaluationVm.AdditionalInfo));
                    return validationDictionary.IsValid;
                }

                //Validate dates
                if (!IsValidDate(projectEvaluationVm.StartDate_Day, projectEvaluationVm.StartDate_Month, projectEvaluationVm.StartDate_Year, "StartDate", validationDictionary))
                    return validationDictionary.IsValid;

                if (!IsValidDate(projectEvaluationVm.EndDate_Day, projectEvaluationVm.EndDate_Month, projectEvaluationVm.EndDate_Year, "EndDate", validationDictionary))
                    return validationDictionary.IsValid;

                //End Date cannot be before start date
                DateTime StartDate = new DateTime(projectEvaluationVm.StartDate_Year.Value, projectEvaluationVm.StartDate_Month.Value, projectEvaluationVm.StartDate_Day.Value);
                DateTime EndDate = new DateTime(projectEvaluationVm.EndDate_Year.Value, projectEvaluationVm.EndDate_Month.Value, projectEvaluationVm.EndDate_Day.Value);
                if (EndDate < StartDate)
                {
                    validationDictionary.AddError("EndDate", String.Format("The End Date must be after the Start Date.", projectEvaluationVm.EndDate));
                    return validationDictionary.IsValid;

                }
            }

            return validationDictionary.IsValid;
        }

        private bool ValidateAddStatement(ProjectStatementVM projectStatementVM,
            IValidationDictionary validationDictionary)
        {
            //Validation: A statement type must have been selected. Cannot do this using Annotation in the ViewModel as the role is selected from radio buttons, the RoleID held in the VM is the current role. Not added until the update has been saved.
            // Validation: there must be a statement type entered
            //try
            //{
            //    int ReasonLength = projectStatementVM.NewProjectStatement.reason_action.Length;
            //    if  (ReasonLength > 200)
            //    {
            //        validationDictionary.AddError("NewProjectStatement.reason_action", String.Format("Comments cannot be  greater than 200 characters.", projectStatementVM.NewProjectStatement.reason_action));
            //        return validationDictionary.IsValid;
            //    }
            //}
            //catch
            //{
            //    ReasonLength = 0;
            //}

            if (projectStatementVM.NewProjectStatement.StatementType == "" ||
                projectStatementVM.NewProjectStatement.StatementType == null)
            {
                validationDictionary.AddError("NewProjectStatement.StatementType",
                    String.Format("Please select a statement type", projectStatementVM.NewProjectStatement.StatementType));
                return validationDictionary.IsValid;
            }

            if (string.IsNullOrEmpty(projectStatementVM.NewProjectStatement.DocumentID))
            {
                validationDictionary.AddError("NewProjectStatement.DocumentID",
                        String.Format("You must enter a Document ID",
                            projectStatementVM.NewProjectStatement.StatementType));
                return validationDictionary.IsValid;
            }

            if (projectStatementVM.NewProjectStatement.DocumentID.Length < 6 ||
                    projectStatementVM.NewProjectStatement.DocumentID.Length > 12)
                {
                    validationDictionary.AddError("NewProjectStatement.DocumentID",
                        String.Format("Document IDs must be between 6 & 12 characters",
                            projectStatementVM.NewProjectStatement.StatementType));
                    return validationDictionary.IsValid;
                }

            //Validation. Document ID must be numeric. Cast it to an Int.
            long intOut;
            if (!Int64.TryParse(projectStatementVM.NewProjectStatement.DocumentID, out intOut))
            {
                validationDictionary.AddError("NewProjectStatement.DocumentID", String.Format("Document ID must be a number.", projectStatementVM.NewProjectStatement.StatementType));
                return validationDictionary.IsValid;
            }

            return validationDictionary.IsValid;

            //DateTime ReceivedDate;
            //DateTime PeriodFrom;
            //DateTime PeriodTo;


            //Validate dates
            //if (!IsValidDate(projectStatementVM.NewProjectStatement.ReceivedDate_Day, projectStatementVM.NewProjectStatement.ReceivedDate_Month, projectStatementVM.NewProjectStatement.ReceivedDate_Year, "NewProjectStatement.ReceivedDate", validationDictionary))
            //    return validationDictionary.IsValid;

            //if (!IsValidDate(projectStatementVM.NewProjectStatement.PeriodFrom_Day, projectStatementVM.NewProjectStatement.PeriodFrom_Month, projectStatementVM.NewProjectStatement.PeriodFrom_Year, "NewProjectStatement.PeriodFrom", validationDictionary))
            //    return validationDictionary.IsValid;

            //if (!IsValidDate(projectStatementVM.NewProjectStatement.PeriodTo_Day, projectStatementVM.NewProjectStatement.PeriodTo_Month, projectStatementVM.NewProjectStatement.PeriodTo_Year, "NewProjectStatement.PeriodTo", validationDictionary))
            //    return validationDictionary.IsValid;

            //Build dates

            //ReceivedDate = new DateTime(projectStatementVM.NewProjectStatement.ReceivedDate_Year.Value,
            //    projectStatementVM.NewProjectStatement.ReceivedDate_Month.Value, projectStatementVM.NewProjectStatement.ReceivedDate_Day.Value);

            //PeriodFrom = new DateTime(projectStatementVM.NewProjectStatement.PeriodFrom_Year.Value,
            //projectStatementVM.NewProjectStatement.PeriodFrom_Month.Value, projectStatementVM.NewProjectStatement.PeriodFrom_Day.Value);

            //PeriodTo = new DateTime(projectStatementVM.NewProjectStatement.PeriodTo_Year.Value,
            //    projectStatementVM.NewProjectStatement.PeriodTo_Month.Value, projectStatementVM.NewProjectStatement.PeriodTo_Day.Value);

            //if (ReceivedDate > DateTime.Today)
            //{
            //    validationDictionary.AddError("NewProjectStatement.ReceivedDate", String.Format("Statements cannot be received in the future", projectStatementVM.NewProjectStatement.ReceivedDate));
            //    return validationDictionary.IsValid;
            //}

            //if (projectStatementVM.NewProjectStatement.StatementType == "" || projectStatementVM.NewProjectStatement.StatementType == null)
            //{
            //    validationDictionary.AddError("NewProjectStatement.StatementType", String.Format("Please select a statement type", projectStatementVM.NewProjectStatement.StatementType));
            //    return validationDictionary.IsValid;
            //}

            //if (ReceivedDate == null)
            //{
            //    validationDictionary.AddError("NewProjectStatement.ReceivedDate", String.Format("Please select a received date", projectStatementVM.NewProjectStatement.ReceivedDate));
            //    return validationDictionary.IsValid;
            //}

            //if (ReceivedDate < PeriodTo)
            //{
            //    validationDictionary.AddError("NewProjectStatement.ReceivedDate", String.Format("Received date must be later than period to date", projectStatementVM.NewProjectStatement.ReceivedDate));
            //    return validationDictionary.IsValid;
            //}


        }

        private bool ValidateAddProject(String ProjectID, DashboardViewModel dashboardViewModel, DashboardViewModel originalVM, IValidationDictionary validationDictionary)
        {
            if (ProjectID.IsNullOrWhiteSpace())
            {
                validationDictionary.AddError("NewProjectID", "You need to select a project.");
                return validationDictionary.IsValid;
            }
            // Check all current Projects vs the added one, no duplicates allowed.
            int DuplicateProject;
            try
            {
                UserProjectsViewModel DuplicateProjectCheck;
                //SectorCheck = originalVM.InputSectors.Where(x => x.InputSectorCode1.Equals(newsector)).First() as InputSectorVM;
                DuplicateProjectCheck = originalVM.userprojects.Where(x => x.ProjectID.Equals(ProjectID)).First() as UserProjectsViewModel;
                DuplicateProject = 1;
            }
            catch
            {
                DuplicateProject = 0;
            }


            if (DuplicateProject == 1)
            {
                validationDictionary.AddError("NewProjectID", "You can't add duplicate projects.");
                return validationDictionary.IsValid;
            }

            else
            {
                return validationDictionary.IsValid;
            }
        }

        private bool ValidateRemoveProject(String ProjectID, IValidationDictionary validationDictionary)
        {


            return validationDictionary.IsValid;

        }

        private bool ValidateSector(ComponentSectorVM componentToValidate, ComponentMaster component, IValidationDictionary validationDictionary)
        {
            //Calculations on Sectors
            int NumberofCurrentSectors = (component.InputSectorCodes.Count());
            int TotalPecentageCurrentSectors = 0;

            if (NumberofCurrentSectors > 0)
            {
                TotalPecentageCurrentSectors = (component.InputSectorCodes.Sum(x => x.Percentage.Value));
            }

            // test logic
            string newsector = componentToValidate.NewInputSector.InputSectorCode1;

            //InputSectorVM test = originalVM.InputSectors.Where(x => x.InputSectorCode1.Equals(newsector)).First() as InputSectorVM;

            if (componentToValidate.NewInputSector.Percentage == null)
            {
                validationDictionary.AddError("NewInputSector.Percentage", "Percentage cannot be blank");
                return validationDictionary.IsValid;
            }

            int TotalPercentageNewSector = (componentToValidate.NewInputSector.Percentage.Value);

            if (componentToValidate.NewInputSector.InputSectorCode1 == null)
            {
                validationDictionary.AddError("NewInputSector.InputSectorCode1", "Sector cannot be blank");

                return validationDictionary.IsValid;
            }

            if (NumberofCurrentSectors == 8)
            {
                validationDictionary.AddError("NewInputSector.InputSectorCode1", "You can only have a maximum of 8 sectors");
                return validationDictionary.IsValid;
            }
            if (TotalPecentageCurrentSectors == 100)
            {
                validationDictionary.AddError("NewInputSector.Percentage", "You can't add a new Sector as they already sum to 100%");
                return validationDictionary.IsValid;
            }
            if (TotalPecentageCurrentSectors == 100)
            {
                validationDictionary.AddError("NewInputSector.Percentage", "You can't add a new Sector as they already sum to 100%");
                return validationDictionary.IsValid;
            }
            if ((TotalPecentageCurrentSectors + TotalPercentageNewSector) > 100)
            {
                validationDictionary.AddError("NewInputSector.Percentage", "Your Sector Percentage total is greater than 100%");
                return validationDictionary.IsValid;
            }

            if (TotalPecentageCurrentSectors + TotalPercentageNewSector == 100)
            {


                int percentageofNewSector = componentToValidate.NewInputSector.Percentage.Value;

                int SectorWithMaxPercentage;
                try
                {
                    SectorWithMaxPercentage = component.InputSectorCodes.Max(x => x.Percentage).Value;
                }
                catch
                {
                    SectorWithMaxPercentage = 0;
                }


                if (percentageofNewSector == SectorWithMaxPercentage)
                {
                    validationDictionary.AddError("NewInputSector.Percentage", "Sector percentages are equal. One must be higher than the other to identify broad sector for DAC reporting.");
                    return validationDictionary.IsValid;
                }

                int NumberOfDuplicatePercentages =
                    component.InputSectorCodes.Count(x => x.Percentage.Equals(SectorWithMaxPercentage));

                if (NumberOfDuplicatePercentages > 1)
                {
                    validationDictionary.AddError("NewInputSector.Percentage", "Sector percentages are equal. One must be higher than the other to identify broad sector for DAC reporting.");
                    return validationDictionary.IsValid;
                }
            }

            // Check all current Sectors vs the added one, no duplicates allowed.
            int DuplicateSector;
            try
            {
                InputSectorCode SectorCheck;
                SectorCheck = component.InputSectorCodes.Where(x => x.InputSectorCode1.Equals(newsector)).First() as InputSectorCode;
                DuplicateSector = 1;
                validationDictionary.AddError("NewInputSector.InputSectorCode1", "You can't add duplicate sectors.");
                return validationDictionary.IsValid;
            }
            catch
            {
                DuplicateSector = 0;
            }
            return validationDictionary.IsValid;
        }

        private static bool IsValidDate(int? Day, int? Month, int? Year, string ModelProperty, IValidationDictionary validationDictionary)
        {
            //Is Day null?
            if (!Day.HasValue)
            {
                validationDictionary.AddError(ModelProperty, "Day is required.");
                return validationDictionary.IsValid;
            }

            //Is Day null?
            if (!Month.HasValue)
            {
                validationDictionary.AddError(ModelProperty, "Month is required.");
                return validationDictionary.IsValid;
            }

            //Is Day null?
            if (!Year.HasValue)
            {
                validationDictionary.AddError(ModelProperty, "Year is required.");
                return validationDictionary.IsValid;
            }

            return IsValidDate(Day.Value, Month.Value, Year.Value, ModelProperty, validationDictionary);
        }

        private static bool IsValidDate(int Day, int Month, int Year, string ModelProperty, IValidationDictionary validationDictionary)
        {
            //Is the number positive?
            if (!(Year > 0))
            {
                validationDictionary.AddError(ModelProperty, "Year must be a positive number.");
                return validationDictionary.IsValid;
            }
            //Is the year a 4 digit integer?
            if (Year.ToString().Length != 4)
            {
                validationDictionary.AddError(ModelProperty, "Year must be a 4 digit number.");
                return validationDictionary.IsValid;
            }

            //Is the month a value from 1 to 12?
            if (!(Month >= 1 && Month <= 12))
            {
                validationDictionary.AddError(ModelProperty, "The month must be a value from 1 to 12.");
                return validationDictionary.IsValid;
            }

            switch (Month)
            {
                case 2: //February
                    {
                        if (!DateTime.IsLeapYear(Year))
                        {
                            if (!(Day >= 1 && Day <= 28))
                            {
                                validationDictionary.AddError(ModelProperty, "The day must be a value from 1 to 28.");
                            }
                            return validationDictionary.IsValid;
                        }
                        else
                        {
                            if (!(Day >= 1 && Day <= 29))
                            {
                                validationDictionary.AddError(ModelProperty, "The day must be a value from 1 to 29.");
                            }
                            return validationDictionary.IsValid;
                        }
                    }

                case 4: //April
                    {
                        //Day must be between 1 and 30, inclusive. 
                        if (!(Day >= 1 && Day <= 30))
                        {
                            validationDictionary.AddError(ModelProperty, "The day must be a value from 1 to 30.");
                        }
                        return validationDictionary.IsValid;

                    }
                case 6: //June
                    {
                        //Day must be between 1 and 30, inclusive. 
                        if (!(Day >= 1 && Day <= 30))
                        {
                            validationDictionary.AddError(ModelProperty, "The day must be a value from 1 to 30.");
                        }
                        return validationDictionary.IsValid;
                    }
                case 9: //September
                    {
                        //Day must be between 1 and 30, inclusive. 
                        if (!(Day >= 1 && Day <= 30))
                        {
                            validationDictionary.AddError(ModelProperty, "The day must be a value from 1 to 30.");
                        }
                        return validationDictionary.IsValid;
                    }
                case 11: //November
                    {
                        //Day must be between 1 and 30, inclusive. 
                        if (!(Day >= 1 && Day <= 30))
                        {
                            validationDictionary.AddError(ModelProperty, "The day must be a value from 1 to 30.");
                        }
                        return validationDictionary.IsValid;
                    }

                default: //Everything else

                    //Day must be between 1 and 31, inclusive. 
                    if (!(Day >= 1 && Day <= 31))
                    {
                        validationDictionary.AddError(ModelProperty, "The day must be a value from 1 to 31.");
                    }
                    return validationDictionary.IsValid;
            }
        }



        private bool ValidateReviewOutputScore(ReviewOutputVM reviewOutputVm, IValidationDictionary validationDictionary)
        {
            var ampRepository = new AMPRepository();
            int OutPutDescriptionCount = reviewOutputVm.OutputDescription.ToString().Length;


            IEnumerable<ReviewOutput> reviewOutputs = ampRepository.GetReviewOutputs(reviewOutputVm.ProjectID, reviewOutputVm.ReviewID);
            int reviewOutputsCount = ampRepository.GetReviewOutputsCount(reviewOutputVm.ProjectID, reviewOutputVm.ReviewID);

            int? totalPecentageExistings = reviewOutputs.Sum(x => x.Weight);

            int totalPercentageNew = (reviewOutputVm.Weight);

            //Check if total weighting has exceeded 100
            if ((totalPecentageExistings + totalPercentageNew) > 100)
            {
                validationDictionary.AddError("ReviewVm.ReviewOutputVm.Weight", "Impact weight(%) total will exceed 100% for outputs");
                return validationDictionary.IsValid;
            }
            else if (reviewOutputsCount >= 12) //reviewoutput should not be more than 12
            {
                validationDictionary.AddError("ReviewVm.ReviewOutputVm.ReviewID", "Total output should not be more than 12");
                return validationDictionary.IsValid;
            }
            else if (OutPutDescriptionCount > 500)
            {
                validationDictionary.AddError("ReviewVm.ReviewOutputVm.Weight", "Description can not be more than 500 Characters");
                return validationDictionary.IsValid;
            }
            //else
            //{
            //    return validationDictionary.IsValid;
            //}
            // validationDictionary.AddError("ResultVM.ProjectOutput.ImpactWeightingPercentage", "Your weighting percentage total is greater than 100%");
            return true;
        }




        private async Task<bool> ValidateSendforApproval(String projectid, Int32 taskid, IValidationDictionary validationDictionary, string user)
        {
            // This method needs to check the project and component data to ensure it passes the minimum required for moving to implementation

            //First we will need to get a few models such as project master to check its data and related data
            ProjectMaster projectMaster = _projectrepository.GetProject(projectid);

            // *** planned end date workflow check - nothing in here just now but will need validation eventually *** 
            // **** 
            ProjectPlannedEndDate projectPlannedEndDate = _projectrepository.GetProjectPlannedEndDate(projectid);
            if (taskid == Constants.PlannedEndDate)
            { return validationDictionary.IsValid; }


            //BUG FIX: If the project is being reopened, do not perform any validation.
            if (taskid == Constants.ReOpenProject || (projectMaster.Stage == Constants.CompletionStage && taskid == Constants.ReApproveProjectTaskId))
            {
                return validationDictionary.IsValid;
            }

            if ((taskid != Constants.ReOpenProject && taskid != Constants.ArchiveProject) && !projectMaster.ComponentMasters.Any())
            {
                validationDictionary.AddError("Summary", "You need at least one Component (see Components page)");
            }

            if ((taskid == Constants.ApproveProjectTask || taskid == Constants.FastTrack) && (projectMaster.Stage == "0" || projectMaster.Stage == "3"))
            {
                if (string.IsNullOrEmpty(projectMaster.ProjectInfo.RiskAtApproval))
                {
                    validationDictionary.AddError("Summary", "You need to select risk at approval (see Project Info > Details page)");
                }
            }

            if ((taskid != Constants.ReOpenProject && taskid != Constants.ArchiveProject) && (string.IsNullOrEmpty(projectMaster.Description)))
            {
                validationDictionary.AddError("Summary", "You need to enter a description (see Project Info > Details page)");
            }


            // Checks if the Gender Equality, Disability Inclusion & Rio Climate options have been selected for non Admin projects (note that new projects are created with the null entry in the database)
            string budgetCentre = projectMaster.BudgetCentreID;
            string budgetCentrePrefix = budgetCentre.Substring(0, 2);
            if ((taskid == Constants.ApproveProjectTask || taskid == Constants.FastTrack) && (projectMaster.Stage == "0" || projectMaster.Stage == "3") && ((budgetCentrePrefix != "A0" && budgetCentrePrefix != "C0" && budgetCentrePrefix != "AP")))
            {
                if (string.IsNullOrEmpty(projectMaster.Markers1.GenderEquality))
                {
                    validationDictionary.AddError("Summary", "You need to select a Gender Equality Marker option (see Project Info > Markers page)");
                }
            }




            if ((taskid == Constants.ApproveProjectTask || taskid == Constants.FastTrack) && (projectMaster.Stage == "0" || projectMaster.Stage == "3") && ((budgetCentrePrefix != "A0" && budgetCentrePrefix != "C0" && budgetCentrePrefix != "AP")))
            {
                if (string.IsNullOrEmpty(projectMaster.Markers1.Biodiversity) || string.IsNullOrEmpty(projectMaster.Markers1.Mitigation) || string.IsNullOrEmpty(projectMaster.Markers1.Adaptation) || string.IsNullOrEmpty(projectMaster.Markers1.Desertification))
                {
                    validationDictionary.AddError("Summary", "You need to complete the Rio climate markers (see Project Info > Markers page)");
                }
            }

            //check if the Disability inclusion marker has been selected for non Admin projects
            if ((taskid == Constants.ApproveProjectTask || taskid == Constants.FastTrack) && (projectMaster.Stage == "0" || projectMaster.Stage == "3") && ((budgetCentrePrefix != "A0" && budgetCentrePrefix != "C0" && budgetCentrePrefix != "AP")))
            {
                if (string.IsNullOrEmpty(projectMaster.Markers1.Disability))
                {
                    validationDictionary.AddError("Summary", "You need to select a Disability Inclusion Marker option (see Project Info > Markers page)");
                }
            }



            if (taskid != Constants.ReOpenProject && taskid != Constants.ArchiveProject)
            {
                //Just use AddError as many times as you can then return IsValid last 
                //Project level Validation

                //Check for SRO & Inputter
                int SROExists = 0;
                //int QAExists = 0;
                int InputterExists = 0;

                foreach (Team team in projectMaster.Teams.Where(x => x.Status == "A"))
                {


                    if (team.RoleID == "SRO")
                    {
                        SROExists = 1;
                    }

                    //if (team.RoleID == "QA")
                    //{
                    //    QAExists = 1;
                    //}

                    if (team.RoleID == "PI")
                    {
                        InputterExists = 1;
                    }

                    //if (team.RoleID == "TM")
                    //{
                    //    InputterExists = 1;
                    //}
                }
                if (SROExists != 1)
                {
                    validationDictionary.AddError("Summary", "This project has no SRO (see Project Info > Team page)");
                }

                //if (QAExists != 1)
                //{
                //    validationDictionary.AddError("Summary", "This project has no QA ");
                //}

                if (InputterExists != 1)
                {
                    validationDictionary.AddError("Summary", "This project has no Inputter (see Project Info > Team page)");
                }

            }

            //Are you sending this to yourself?
            //if (user == )


            //Gender and HIV Aids?

            //if (string.IsNullOrEmpty(projectMaster.ProjectInfo.GenderEquality) ||
            //    string.IsNullOrEmpty(projectMaster.ProjectInfo.HIVAIDS))
            //{
            //    validationDictionary.AddError("Summary", "This project has missing gender or HIV/Aids markers ");   
            //}


            //Component level validation
            if (taskid != Constants.ReOpenProject && taskid != Constants.ArchiveProject)
            {
                //May want to check other things in this loop in addition to sectors. All component level checking.
                foreach (ComponentMaster componentmaster in projectMaster.ComponentMasters)
                {
                    //Sectors

                    int NumberofCurrentSectors = (componentmaster.InputSectorCodes.Count());
                    int TotalPecentageCurrentSectors = 0;

                    if (NumberofCurrentSectors > 0)
                    {
                        TotalPecentageCurrentSectors = (componentmaster.InputSectorCodes.Sum(x => x.Percentage.Value));
                    }

                    if (TotalPecentageCurrentSectors != 100)
                    {
                        validationDictionary.AddError("Summary",
                            componentmaster.ComponentID + " has sectors which do not add to 100% (see Components > Details page)");
                    }

                    //Benefitting country
                    if (string.IsNullOrEmpty(componentmaster.BenefittingCountry))
                    {
                        validationDictionary.AddError("Summary",
                            componentmaster.ComponentID + " has no benefitting country (see Components > Markers page)");
                    }


                    //Aid effectiveness Marker
                    if (componentmaster.Marker != null)
                    {

                        if (string.IsNullOrEmpty(componentmaster.Marker.PBA) ||
                            string.IsNullOrEmpty(componentmaster.Marker.SWAP))
                        {
                            validationDictionary.AddError("Summary",
                                componentmaster.ComponentID + " has missing aid effectiveness markers (see Components > Markers page)");
                        }
                    }
                    else
                    {
                        validationDictionary.AddError("Summary",
                            componentmaster.ComponentID + " has missing aid effectiveness markers (see Components > Markers page)");
                    }
                }
            }

            //Is the workflow selected appropriate? Have users managed to find a way past the client side validation that stops them doing things they shouldn't???
            if (taskid == Constants.ApproveAD)
            {
                if (!CanProjectBeSentForAandDApproval(projectMaster))
                {
                    validationDictionary.AddError("Summary", "This project cannot be sent for A&D approval as it is no longer at PrePipeline stage.");
                }
            }
            else if (taskid == Constants.FastTrack)
            {
                if (!CanProjectBeSentForFastTrackApproval(projectMaster))
                {
                    validationDictionary.AddError("Summary", "This project cannot be sent for Fast Track approval as it is no longer at PrePipeline stage.");
                }
            }
            else if (taskid == Constants.ApproveProjectTask)
            {
                if (!CanProjectBeSentForApprovalorReApproval(projectMaster))
                {
                    validationDictionary.AddError("Summary", "This project cannot be sent for approval/reapproval as it is at the PrePipeline stage.");
                }
            }

            //Is the project already in workflow?
            ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

            projectwfvm = IsProjectInAMPWorkflow(projectMaster, user);


            if (projectwfvm.TaskId != 0)
            {
                //There is a workflow.
                if (projectwfvm.TaskId != taskid)
                {
                    validationDictionary.AddError("Summary", String.Format("This Project is in {0} workflow. Another workflow cannot be submitted.", projectwfvm.WorkFlowDescription));
                }
            }

            //Archive Workflow
            if (taskid == 9)
            {

                if (await _ariesService.DoesProjectHaveBudget(projectid, user))
                {
                    validationDictionary.AddError("Summary", String.Format("This Project has budget on ARIES. Reduce the budget to zero before Archiving the project.", projectwfvm.WorkFlowDescription));

                }

            }

            return validationDictionary.IsValid;
        }

        private bool ValidateSendforClosure(String projectid, Int32 taskid, IValidationDictionary validationDictionary, string user)
        {
            // This method needs to check the project and component data to ensure it meets minimum requirements for closing the project. It needs less/different validations from other workflows, hence the seperate method.

            //First we will need to get a few models such as project master to check its data and related data
            ProjectMaster projectMaster = _projectrepository.GetProject(projectid);

            //Is the project already closed?
            if (projectMaster.Stage == "7")
            {
                validationDictionary.AddError("Summary", "This project is already closed.");
                return validationDictionary.IsValid;
            }
            //Is this a valid way to close the project? Should it be PCR instead?
            if (!CanProjectBeSentForClosure(projectMaster))
            {
                validationDictionary.AddError("Summary", "This project can only be closed by a PCR.");
                return validationDictionary.IsValid;
            }

            //Is the project already in workflow?
            ProjectWFCheckVM projectwfvm = new ProjectWFCheckVM();

            projectwfvm = IsProjectInAMPWorkflow(projectMaster, user);

            if (projectwfvm.TaskId != 0)
            {
                //There is a workflow.
                if (projectwfvm.TaskId != taskid)
                {
                    validationDictionary.AddError("Summary", String.Format("This Project is in {0} workflow. Another workflow cannot be submitted.", projectwfvm.WorkFlowDescription));
                }
            }

            return validationDictionary.IsValid;
        }

        #endregion

        #region Admin Methods

        public bool IsAdmin(string empNo)
        {
            try
            {
                if (_projectrepository.GetAdmin(empNo) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "IsAdmin", empNo);
                return false;
            }
        }

        public Performance AdminGetPerformance(string id)
        {
            Performance performance = new Performance();

            performance = _projectrepository.GetProjectPerformance(id);

            return performance;
        }

        public ComponentDate AdminGetComponentDates(string id)
        {
            ComponentDate componentdates = _projectrepository.GetComponentDates(id);
            return componentdates;
        }

        public PerformanceVM AdminGetPerformanceNew(string id)
        {
            Performance performance = new Performance();
            performance = _projectrepository.GetProjectPerformance(id);
            Mapper.CreateMap<Performance, PerformanceVM>();
            PerformanceVM performanceVm = new PerformanceVM();
            performanceVm = Mapper.Map<Performance, PerformanceVM>(performance);
            return performanceVm;
        }
        public EditPerformanceVM AdminGetPerformanceNewEdit(string id)
        {
            Performance performance = new Performance();
            performance = _projectrepository.GetProjectPerformance(id);
            Mapper.CreateMap<Performance, EditPerformanceVM>();
            //EditPerformanceVM performanceVm = new EditPerformanceVM();
            EditPerformanceVM performanceVm = Mapper.Map<Performance, EditPerformanceVM>(performance);

            #region Load_Exemption_AR_PCR
            performanceVm.ExemptionReasons = _projectrepository.GetExemptionReasons();

            ReviewExemption reviewExemptions = _projectrepository.GetReviewExemption(performanceVm.ProjectID, "Annual Review"); //id
            ReviewExemption reviewExemptionsPCR = _projectrepository.GetReviewExemption(performanceVm.ProjectID, "Project Completion Review");

            if (reviewExemptionsPCR != null)
            {
                // for PCR
                Mapper.CreateMap<ReviewExemption, ReviewExemptionVM>();
                ReviewExemptionVM reviewExemptionVM = new ReviewExemptionVM();
                reviewExemptionVM = Mapper.Map<ReviewExemption, ReviewExemptionVM>(reviewExemptionsPCR);

                performanceVm.ReviewExemptionPCR = reviewExemptionVM;
                performanceVm.HasPCR = "Yes";
            }
            else
            {
                performanceVm.HasPCR = "No";
            }
            if (reviewExemptions != null)
            {

                Mapper.CreateMap<ReviewExemption, ReviewExemptionVM>();
                ReviewExemptionVM reviewExemptionVM = new ReviewExemptionVM();
                reviewExemptionVM = Mapper.Map<ReviewExemption, ReviewExemptionVM>(reviewExemptions);

                performanceVm.ReviewExemptionAR = reviewExemptionVM;
                performanceVm.HasAR = "Yes";
            }
            else
            {
                performanceVm.HasAR = "No";
            }

            #endregion 

            return performanceVm;
        }

        public bool AdminUpdatePerformance(Performance performance, string user)
        {
            try
            {
                performance.LastUpdated = DateTime.Now;
                performance.UserID = user;

                _projectrepository.UpdateProjectPerformance(performance);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update Performance (POST)", user);
                return false;
            }
        }

        public bool AdminUpdatePerformanceNew(PerformanceVM performanceVM, IValidationDictionary validationDictionary, string user)
        {

            //Validate that the day, month and year are valid values. 

            if (!PerformanceARPCRDatesAreValid(performanceVM.ARDueDate_Day, performanceVM.ARDueDate_Month, performanceVM.ARDueDate_Year, validationDictionary))
                return false;
            if (!PerformanceARPCRDatesAreValid(performanceVM.PCRDueDate_Day, performanceVM.PCRDueDate_Month, performanceVM.PCRDueDate_Year, validationDictionary))
                return false;

            performanceVM.ARDueDate =
               Convert.ToDateTime(performanceVM.ARDueDate_Day + "/" + performanceVM.ARDueDate_Month + "/" +
                                  performanceVM.ARDueDate_Year);

            performanceVM.ARPromptDate = Convert.ToDateTime(performanceVM.ARDueDate).AddMonths(-3);

            performanceVM.PCRDueDate = Convert.ToDateTime(performanceVM.PCRDueDate_Day + "/" + performanceVM.PCRDueDate_Month + "/" +
                                   performanceVM.PCRDueDate_Year);

            performanceVM.PCRPrompt = Convert.ToDateTime(performanceVM.PCRDueDate).AddMonths(-6);


            Performance performance = new Performance();
            Mapper.CreateMap<PerformanceVM, Performance>();
            performance = Mapper.Map<PerformanceVM, Performance>(performanceVM);

            try
            {
                performance.LastUpdated = DateTime.Now;
                performance.UserID = user;
                _projectrepository.UpdateProjectPerformance(performance);
                _projectrepository.Save();
                return true;
            }

            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update new Performance (POST)", user);
                return false;
            }
        }

        public bool AdminUpdatePerformanceNewEdit(EditPerformanceVM performanceVM, IValidationDictionary validationDictionary, string user)
        {

            //Validate that the day, month and year are valid values. 
            if (performanceVM.ARDueDate_Day != 0 && performanceVM.ARDueDate_Month != 0 && performanceVM.ARDueDate_Year != 0)
            {
                //if (!PerformanceARPCRDatesAreValid(performanceVM.ARDueDate_Day, performanceVM.ARDueDate_Month, performanceVM.ARDueDate_Year, validationDictionary))
                //    return false;
                performanceVM.ARDueDate = Convert.ToDateTime(performanceVM.ARDueDate_Day + "/" + performanceVM.ARDueDate_Month + "/" +
                                  performanceVM.ARDueDate_Year);
                performanceVM.ARPromptDate = Convert.ToDateTime(performanceVM.ARDueDate).AddMonths(-3);
            }

            if (performanceVM.PCRDueDate_Day != 0 && performanceVM.PCRDueDate_Month != 0 && performanceVM.PCRDueDate_Year != 0)
            {
                //if (!PerformanceARPCRDatesAreValid(performanceVM.PCRDueDate_Day, performanceVM.PCRDueDate_Month, performanceVM.PCRDueDate_Year, validationDictionary))
                //    return false;   
                performanceVM.PCRDueDate =
                Convert.ToDateTime(performanceVM.PCRDueDate_Day + "/" + performanceVM.PCRDueDate_Month + "/" +
                                   performanceVM.PCRDueDate_Year);
                performanceVM.PCRPrompt = Convert.ToDateTime(performanceVM.PCRDueDate).AddMonths(-6);
            }

            Performance performance = new Performance();
            Mapper.CreateMap<EditPerformanceVM, Performance>();
            performance = Mapper.Map<EditPerformanceVM, Performance>(performanceVM);

            try
            {
                performance.LastUpdated = DateTime.Now;
                performance.UserID = user;
                _projectrepository.UpdateProjectPerformance(performance);

                _projectrepository.Save();
                return true;
            }

            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update new Performance (POST)", user);
                return false;
            }
        }

        private static bool PerformanceARPCRDatesAreValid(int? Day, int? Month, int? Year, IValidationDictionary validationDictionary)
        {
            if (!IsValidDate(Day, Month, Year, "ARDueDate_Day", validationDictionary))
                return validationDictionary.IsValid;
            //if (!IsValidDate(componentDateVm.OperationalEndDate_Day, componentDateVm.OperationalEndDate_Month, componentDateVm.OperationalEndDate_Year, "ComponentDate.OperationalEndDate", validationDictionary))
            //    return validationDictionary.IsValid;
            return validationDictionary.IsValid;
        }


        public bool AdminUpdateProjectMaster(ProjectMaster projectMaster, string user)
        {
            try
            {
                projectMaster.LastUpdate = DateTime.Now;
                projectMaster.UserID = user;
                _projectrepository.UpdateProjectMaster(projectMaster);
                _projectrepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(ex, "Admin Update ProjectMaster (POST)", user);
                return false;
            }
        }

        public bool AdminUpdateComponentMaster(ComponentMaster componentMaster, string user)
        {
            try
            {
                componentMaster.LastUpdate = DateTime.Now;
                componentMaster.UserID = user;
                _projectrepository.UpdateComponentMaster(componentMaster);
                _projectrepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(ex, "Admin Update ComponentMaster (POST)", user);
                return false;
            }
        }

        public bool AdminUpdateComponentDates(ComponentDate componentDates, string user)
        {
            try
            {
                componentDates.LastUpdate = DateTime.Now;
                componentDates.UserID = user;
                _projectrepository.UpdateComponentDate(componentDates);
                _projectrepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                _errorengine.LogError(ex, "Admin Update ComponentDates (POST)", user);
                return false;
            }

        }

        public ReviewExemption AdminGetReviewExemption(string id, string exemptionType)
        {
            ReviewExemption reviewExemption = _projectrepository.GetReviewExemption(id, exemptionType);

            return reviewExemption;
        }

        public bool AdminUpdateReviewExemption(ReviewExemption reviewExemption, string user)
        {
            try
            {
                ReviewExemption reviewExemptionToUpdate = _projectrepository.GetReviewExemption(reviewExemption.ProjectID, reviewExemption.ExemptionType);

                reviewExemptionToUpdate.StageID = reviewExemption.StageID;
                reviewExemptionToUpdate.ExemptionReason = reviewExemption.ExemptionReason;
                reviewExemptionToUpdate.Approver = reviewExemption.Approver;
                reviewExemptionToUpdate.ApproverComment = reviewExemption.ApproverComment;
                reviewExemptionToUpdate.Requester = reviewExemption.Requester;
                reviewExemptionToUpdate.Approved = reviewExemption.Approved;
                reviewExemptionToUpdate.SubmissionComment = reviewExemption.SubmissionComment;

                reviewExemptionToUpdate.LastUpdated = DateTime.Now;
                reviewExemptionToUpdate.UpdatedBy = user;

                _projectrepository.UpdateReviewExemption(reviewExemptionToUpdate);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update ReviewExemption (POST)", user);
                return false;
            }
        }

        public InputSectorCode AdminGetComponentInputSector(string componentId, string lineNo)
        {
            InputSectorCode inputSectorCode = new InputSectorCode();

            Int32 lineNoInt;
            if (Int32.TryParse(lineNo, out lineNoInt))
            {
                inputSectorCode = _projectrepository.AdminGetComponentInputSector(componentId, lineNoInt);
                return inputSectorCode;
            }
            else
            {
                return null;
            }
        }

        public bool AdminUpdateInputSectorCode(InputSectorCode inputSectorCode, string user)
        {
            try
            {
                inputSectorCode.UserID = user;
                inputSectorCode.LastUpdate = DateTime.Now;
                _projectrepository.AdminUpdateComponentInputSector(inputSectorCode);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update Component InputSectorCode (POST)", user);
                return false;
            }
        }

        public WorkflowMaster AdminGetWorkflowMaster(string id)
        {
            WorkflowMaster workflowMaster = new WorkflowMaster();
            Int32 workflowId;
            if (Int32.TryParse(id, out workflowId))
            {
                workflowMaster = _projectrepository.GetWorkflowMaster(workflowId);

                return workflowMaster;
            }
            else
            {
                return null;
            }
        }

        public bool AdminUpdateWorkflowMaster(WorkflowMaster workflowMaster, string user)
        {
            try
            {
                workflowMaster.LastUpdate = DateTime.Now;
                //workflowMaster.UserID = user;

                _projectrepository.UpdateWorkFlowMaster(workflowMaster);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update WorkflowMaster (POST)", user);
                return false;
            }
        }


        public WorkflowDocument AdminGetWorkflowDocument(string id)
        {
            WorkflowDocument workflowDocument = new WorkflowDocument();
            Int32 tableId;
            if (Int32.TryParse(id, out tableId))
            {
                workflowDocument = _projectrepository.AdminGetWorkflowDocument(tableId);

                return workflowDocument;
            }
            else
            {
                return null;
            }
        }

        public bool AdminUpdateWorkflowDocument(WorkflowDocument workflowDocument, string user)
        {
            try
            {
                //workflowDocument.LastUpdate = DateTime.Now;
                //workflowMaster.UserID = user;

                _projectrepository.AdminUpdateWorkflowDocument(workflowDocument);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update WorkflowDocument (POST)", user);
                return false;
            }
        }

        public ReviewMaster AdminGetReviewMaster(string projectId, Int32 reviewId)
        {
            ReviewMaster master = new ReviewMaster();
            master = _projectrepository.AdminGetReviewMaster(projectId, reviewId);

            return master;

        }

        public bool AdminUpdateReviewMaster(ReviewMaster reviewMaster, string user)
        {
            try
            {
                //reviewMaster.LastUpdated = DateTime.Now; - Dont update the LastUpdated, can impact the review date.
                //reviewMaster.UserID = user;

                _projectrepository.AdminUpdateReviewMaster(reviewMaster);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update ReviewMaster (POST)", user);
                return false;
            }

        }

        public bool AdminUpdateProjectDate(ProjectDate projectDate, string user)
        {
            try
            {
                _projectrepository.AdminUpdateProjectDate(projectDate);
                _projectrepository.Save();
                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update Project Date  (POST)", user);
                return false;
            }
        }

        public bool AdminUpdateProjectInfo(ProjectInfo projectInfo, string user)
        {
            try
            {
                projectInfo.LastUpdate = DateTime.Now;
                projectInfo.UserID = user;
                _projectrepository.AdminUpdateProjectInfo(projectInfo);
                _projectrepository.Save();
                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update Project Info  (POST)", user);
                return false;
            }
        }


        public bool AdminUpdateProjectMarkers(Markers1 projectMarkers, string user)
        {
            try
            {
                projectMarkers.LastUpdated = DateTime.Now;
                projectMarkers.UserID = user;
                _projectrepository.AdminUpdateProjectMarkers(projectMarkers);
                _projectrepository.Save();
                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update Project Markers  (POST)", user);
                return false;
            }
        }


        public Team AdminGetTeam(string id)
        {
            Team team = new Team();
            Int32 teamInt;
            if (Int32.TryParse(id, out teamInt))
            {
                team = _projectrepository.GetTeamMember(teamInt);

                return team;
            }
            else
            {
                return null;
            }
        }

        public bool AdminUpdateTeam(Team team, String user)
        {
            try
            {
                team.LastUpdated = DateTime.Now;
                team.UserID = user;

                if (team.Status == "A")
                {
                    _projectrepository.AdminUpdateTeamMember(team);
                }
                else
                {
                    _projectrepository.AdminEndTeamMember(team);
                }

                _projectrepository.Save();

                return true;

            }
            catch (Exception exception)
            {
                {
                    _errorengine.LogError(exception, "Admin EditTeam (POST)", user);
                    return false;
                }

            }
        }


        public ProjectDate AdminGetProjectDate(string id)
        {
            ProjectDate projectDate = new ProjectDate();
            projectDate = _projectrepository.GetProjectDates(id);
            return projectDate;
        }


        public ProjectInfo AdminGetProjectInfo(string id)
        {
            ProjectInfo projectInfo = new ProjectInfo();
            projectInfo = _projectrepository.GetProjectInfo(id);
            return projectInfo;
        }


        public Markers1 AdminGetProjectMarkers(string id)
        {
            Markers1 projectMarkers = new Markers1();
            projectMarkers = _projectrepository.GetProjectMarkers(id);
            return projectMarkers;
        }


        public bool AddAdmin(string adminToAdd, string user)
        {
            try
            {
                AdminUser existingAdminUser = _projectrepository.AdminExists(adminToAdd);
                if (existingAdminUser != null)
                {
                    existingAdminUser.Status = "A";
                    existingAdminUser.LastUpdated = DateTime.Now;
                    existingAdminUser.UserID = user;

                    _projectrepository.UpdateAdminUser(existingAdminUser);
                    _projectrepository.Save();

                }
                else
                {
                    AdminUser newAdminUser = new AdminUser();

                    newAdminUser.AdminUserID = adminToAdd;
                    newAdminUser.Status = "A";
                    newAdminUser.LastUpdated = DateTime.Now;
                    newAdminUser.UserID = user;

                    _projectrepository.AddAdminUser(newAdminUser);
                    _projectrepository.Save();
                }
                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Add Admin (POST)", user);
                return false;
            }
        }

        public bool RemoveAdmin(string adminToDelete, string user)
        {
            try
            {
                AdminUser adminUser = _projectrepository.GetAdmin(adminToDelete);

                adminUser.Status = "C";
                adminUser.LastUpdated = DateTime.Now;
                adminUser.UserID = user;

                _projectrepository.DeleteAdminUser(adminUser);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Delete Admin (POST)", user);
                return false;
            }

        }

        public bool AdminCloseProject(string projectId, string user)
        {
            try
            {
                CloseProject(projectId, user);
                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Close Project (POST)", user);
                return false;
            }
        }

        public AdminUsersVM GetAdminUsers(string user)
        {
            try
            {
                IEnumerable<AdminUser> adminUsers = _projectrepository.GetAdminUsers();

                AdminUsersVM adminUsersVm = new AdminUsersVM();

                adminUsersVm.adminUsers = adminUsers;

                return adminUsersVm;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Get Admin Users", user);

                return null;
            }
        }

        public DeliveryChain AdminGetDeliveryChain(string id)
        {
            Int32 idInt;
            try
            {
                Int32.TryParse(id, out idInt);
                DeliveryChain deliveryChain = _projectrepository.GetDeliveryChain(idInt);

                return deliveryChain;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Get DeliveryChain", id);

                return null;

            }
        }

        public bool AdminUpdateDeliveryChain(DeliveryChain deliveryChain, string user)
        {
            try
            {
                deliveryChain.UserID = user;
                deliveryChain.LastUpdate = DateTime.Now;
                _projectrepository.AdminUpdateDeliveryChain(deliveryChain);
                _projectrepository.Save();
                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update DeliveryChain " + deliveryChain.ID.ToString(), user);
                return false;
            }
        }

        public ReviewDeferral AdminGetReviewDeferral(string id)
        {
            Int32 idInt;
            try
            {
                Int32.TryParse(id, out idInt);
                ReviewDeferral reviewDeferral = _projectrepository.AdminGetReviewDeferral(idInt);

                return reviewDeferral;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Get ReviewDeferral", id);

                return null;

            }
        }

        public bool AdminUpdateReviewDeferral(ReviewDeferral reviewDeferral, string user)
        {
            try
            {
                reviewDeferral.UpdatedBy = user;
                reviewDeferral.LastUpdated = DateTime.Now;
                _projectrepository.AdminUpdateReviewDeferral(reviewDeferral);
                _projectrepository.Save();
                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "Admin Update ReviewDeferral " + reviewDeferral.DeferralID.ToString(), user);
                return false;
            }

        }


        public AuditedFinancialStatement AdminGetAuditedStatement(string projectId, string statementId)
        {
            AuditedFinancialStatement statement = _projectrepository.GetAuditedStatement(projectId, Int32.Parse(statementId));

            return statement;
        }

        public bool AdminUpdateAuditedStatement(AuditedFinancialStatement statement, string user)
        {
            try
            {
                AuditedFinancialStatement statementToUpdate = _projectrepository.GetAuditedStatement(statement.ProjectID, statement.StatementID);

                statementToUpdate.DueDate = statement.DueDate;
                statementToUpdate.PromptDate = statement.PromptDate;
                statementToUpdate.ReceivedDate = statement.ReceivedDate;
                statementToUpdate.PeriodFrom = statement.PeriodFrom;
                statementToUpdate.PeriodTo = statement.PeriodTo;
                statementToUpdate.Value = statement.Value;
                statementToUpdate.Currency = statement.Currency;
                statementToUpdate.StatementType = statement.StatementType;
                statementToUpdate.reason_action = statement.reason_action;
                statementToUpdate.Status = statement.Status;
                statementToUpdate.DocumentID = statement.DocumentID;
                statementToUpdate.DocSource = statement.DocSource;

                statementToUpdate.LastUpdated = statement.LastUpdated;
                statementToUpdate.UserID = statement.UserID;

                _projectrepository.UpdateStatement(statementToUpdate);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "AdminUpdateAuditedStatement (POST)", user);
                return false;
            }
        }

        public RiskDocument AdminGetRiskDocument(string riskRegisterId)
        {
            RiskDocument document = _projectrepository.GetRiskDocument(Int32.Parse(riskRegisterId));

            return document;
        }

        public bool AdminUpdateRiskDocument(RiskDocument riskDocument, string user)
        {
            try
            {
                RiskDocument documentToUpdate = _projectrepository.GetRiskDocument(riskDocument.RiskRegisterID);

                documentToUpdate.DocumentID = riskDocument.DocumentID;
                documentToUpdate.Description = riskDocument.Description;
                documentToUpdate.DocSource = riskDocument.DocSource;

                documentToUpdate.LastUpdate = riskDocument.LastUpdate;
                documentToUpdate.UserID = riskDocument.UserID;

                _projectrepository.UpdateRiskDocument(documentToUpdate);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "AdminUpdateRiskDocument (POST)", user);
                return false;
            }
        }

        public EvaluationDocument AdminGetEvaluationDocument(string id)
        {
            EvaluationDocument document = _projectrepository.GetEvaluationDocument(Int32.Parse(id));

            return document;
        }

        public bool AdminUpdateEvaluationDocument(EvaluationDocument evaluationDocument, string user)
        {
            try
            {
                EvaluationDocument documentToUpdate = _projectrepository.GetEvaluationDocument(evaluationDocument.ID);

                documentToUpdate.DocumentID = evaluationDocument.DocumentID;
                documentToUpdate.Description = evaluationDocument.Description;
                documentToUpdate.DocSource = evaluationDocument.DocSource;
                documentToUpdate.Status = evaluationDocument.Status;

                documentToUpdate.LastUpdate = evaluationDocument.LastUpdate;
                documentToUpdate.UserID = evaluationDocument.UserID;

                _projectrepository.UpdateEvaluationDocument(documentToUpdate);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "AdminUpdateEvaluationDocument (POST)", user);
                return false;
            }
        }

        public ReviewDocument AdminGetReviewDocument(string id)
        {
            ReviewDocument document = _projectrepository.GetReviewDocument(Int32.Parse(id));

            return document;
        }

        public bool AdminUpdateReviewDocument(ReviewDocument reviewDocument, string user)
        {
            try
            {
                ReviewDocument documentToUpdate = _projectrepository.GetReviewDocument(reviewDocument.ReviewDocumentsID);

                documentToUpdate.DocumentID = reviewDocument.DocumentID;
                documentToUpdate.Description = reviewDocument.Description;
                documentToUpdate.DocSource = reviewDocument.DocSource;

                documentToUpdate.LastUpdate = reviewDocument.LastUpdate;
                documentToUpdate.UserID = reviewDocument.UserID;

                _projectrepository.UpdateReviewDocument(documentToUpdate);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "AdminUpdateReviewDocument (POST)", user);
                return false;
            }
        }

        public FundingMechToSector AdminGetMechSectorMapping(string id)
        {
            FundingMechToSector mapping = _projectrepository.GetFundingMechToSectorMapping(Int32.Parse(id));
            return mapping;
        }

        public bool AdminUpdateMechSectorMapping(FundingMechToSector mechToSector, string user)
        {
            try
            {
                FundingMechToSector mappingToUpdate = _projectrepository.GetFundingMechToSectorMapping(mechToSector.ID);

                mappingToUpdate.InputSector = mechToSector.InputSector;
                mappingToUpdate.FundingMech = mechToSector.FundingMech;
                mappingToUpdate.Status = mechToSector.Status;
                mappingToUpdate.LastUpdated = DateTime.Now;
                mappingToUpdate.UserID = user;

                _projectrepository.UpdateFundingMechToSectorMapping(mappingToUpdate);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "AdminUpdateFundingMechSectorMapping (POST)", user);
                return false;
            }
        }

        public NewFundingMechToSectorVM AdminGetNewMechSectorMapping(string sectorCodeId)
        {
            // Find code details
            InputSector code = _projectrepository.GetIndividualInputSector(sectorCodeId);
            // return null if code does not exist
            if (code == null)
            {
                return null;
            }
            else
            {
                // Get required info for VM
                // Get all funding mech options
                IEnumerable<FundingMech> mechs = _projectrepository.LookUpFundingMechs();
                IEnumerable<FundingMech> validMechs = mechs.Where(u => u.Status == "A"); // Removes Closed Mechs
                IEnumerable<string> allMechIds = validMechs.Select(m => m.FundingMechID);
                // Get existing mappings
                IEnumerable<FundingMechToSector> existingMappings =
                    _projectrepository.GetAllFundingMechMappingsForSectorCode(sectorCodeId);
                IEnumerable<string> allMappedMechs = existingMappings.Select(m => m.FundingMech);

                // Put all unmapped mechs together
                IEnumerable<string> unmappedMechs = allMechIds.Except(allMappedMechs);

                NewFundingMechToSectorVM newFundingMechToSectorVm = new NewFundingMechToSectorVM();
                newFundingMechToSectorVm.InputSector = code;
                newFundingMechToSectorVm.UnMappedOptions = unmappedMechs;

                return newFundingMechToSectorVm;
            }
        }

        public bool AdminAddNewMechSectorMapping(NewFundingMechToSectorVM newMechToSector, string user)
        {
            try
            {
                // Get existing mappings & check entry does not already exist
                IEnumerable<FundingMechToSector> existingMappings =
                    _projectrepository.GetAllFundingMechMappingsForSectorCode(newMechToSector.InputSector.InputSectorCodeID);

                if (existingMappings != null)
                {
                    IEnumerable<string> allMappedMechs = existingMappings.Select(m => m.FundingMech);
                    if (existingMappings.Any(i => i.FundingMech == newMechToSector.NewMapping))
                    {
                        return false;
                    }
                }

                // Maps new entry
                FundingMechToSector newMapping = new FundingMechToSector();

                newMapping.SectorCode = newMechToSector.InputSector.InputSectorCodeID;
                newMapping.FundingMech = newMechToSector.NewMapping;
                newMapping.Status = "A";
                newMapping.LastUpdated = DateTime.Now;
                newMapping.UserID = user;

                _projectrepository.AddNewFundingMechToSectorMapping(newMapping);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "AdminNewFundingMechSectorMapping (POST)", user);
                return false;
            }
        }

        public PartnerMaster AdminGetPartnerMaster(string id)
        {
            PartnerMaster partnerMaster = _projectrepository.AdminGetPartner(Int32.Parse(id));
            return partnerMaster;
        }

        public bool AdminUpdatePartnerMaster(PartnerMaster partnerMaster, string user)
        {
            try
            {
                PartnerMaster partnerToUpdate = _projectrepository.AdminGetPartner(partnerMaster.ID);

                partnerToUpdate.ID = partnerMaster.ID;
                partnerToUpdate.PartnerID = partnerMaster.PartnerID;
                partnerToUpdate.PartnerName = partnerMaster.PartnerName;
                partnerToUpdate.Status = partnerMaster.Status;
                partnerToUpdate.LastUpdated = DateTime.Now;
                partnerToUpdate.UserID = user;

                _projectrepository.AdminUpdatePartner(partnerToUpdate);
                _projectrepository.Save();

                return true;
            }
            catch (Exception exception)
            {
                _errorengine.LogError(exception, "AdminUpdatePartnerMaster (POST)", user);
                return false;
            }
        }


        #endregion

        #region Authentication Methods


        public async Task<bool> IsAuthorised(string userId)
        {
            try
            {
                string sectionCode = await _personService.GetUserSectionCode(userId);

                if (sectionCode == "U0276")
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            catch (NullReferenceException nullReferenceException)
            {
                _errorengine.LogError(nullReferenceException, "IsAuthorisedNull", userId);
                throw new NullReferenceException("Section Code not found");
            }

            catch (Exception exception)
            {
                _errorengine.LogError(exception, "IsAuthorised", userId);
                return false;
            }

        }



        #endregion

        #region Excel Risk Register Methods

        public async Task<ExcelPackage> CreateExcelRiskLog(string projectId, string user)
        {
            RiskRegisterVM riskRegisterVm = new RiskRegisterVM();
            riskRegisterVm = await GetRiskRegister(projectId, user);
            Int32 riskHeaderRowStart = 4;
            Int32 riskDataRowStart = riskHeaderRowStart + 1;

            if (riskRegisterVm != null)
            {
                int riskCount = 0;
                if (riskRegisterVm.RiskItemsVm != null)
                {
                    riskCount = riskRegisterVm.RiskItemsVm.projectRisks.Count;
                }

                if (riskCount > 0)
                {
                    //Get the Risk Owner names from PersonService
                    IEnumerable<PersonDetails> personDetails;
                    List<string> empNoList = riskRegisterVm.RiskItemsVm.projectRisks.Select(x => x.Owner).ToList();

                    personDetails = await _personService.GetPeopleDetails(empNoList);

                    ExcelPackage excelPackage = new ExcelPackage();

                    //Create the worksheet
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("RiskLog-" + projectId);

                    //Create the Title Rows
                    worksheet.Cells[1, 1].Value = "Risk Register";
                    worksheet.Cells[2, 1].Value = "Project " + projectId + " - " + riskRegisterVm.ProjectHeader.Title;

                    worksheet.Row(1).Style.Font.Bold = true;
                    worksheet.Row(2).Style.Font.Bold = true;

                    //Create the Risk Register Header
                    worksheet.Cells[riskHeaderRowStart, 1].Value = "Risk Description";
                    worksheet.Cells[riskHeaderRowStart, 2].Value = "Risk Category";
                    worksheet.Cells[riskHeaderRowStart, 3].Value = "Likelihood";
                    worksheet.Cells[riskHeaderRowStart, 4].Value = "Impact";
                    worksheet.Cells[riskHeaderRowStart, 5].Value = "Gross Risk";
                    worksheet.Cells[riskHeaderRowStart, 6].Value = "Mitigation Strategy";
                    worksheet.Cells[riskHeaderRowStart, 7].Value = "Likelihood";
                    worksheet.Cells[riskHeaderRowStart, 8].Value = "Impact";
                    worksheet.Cells[riskHeaderRowStart, 9].Value = "Residual Risk";
                    worksheet.Cells[riskHeaderRowStart, 10].Value = "Risk Owner";
                    worksheet.Cells[riskHeaderRowStart, 11].Value = "External Risk Owner";
                    worksheet.Cells[riskHeaderRowStart, 12].Value = "Comments";
                    worksheet.Cells[riskHeaderRowStart, 13].Value = "Last Updated";
                    worksheet.Cells[riskHeaderRowStart, 14].Value = "Status";

                    worksheet.Row(riskHeaderRowStart).Style.Font.Bold = true;



                    //Create each row
                    foreach (RiskItemVM riskItem in riskRegisterVm.RiskItemsVm.projectRisks)
                    {
                        worksheet.Cells[riskDataRowStart, 1].Value = riskItem.RiskDescription;
                        worksheet.Cells[riskDataRowStart, 1].Style.WrapText = true;
                        worksheet.Cells[riskDataRowStart, 2].Value = riskItem.RiskCategoryDescription;
                        worksheet.Cells[riskDataRowStart, 3].Value = riskItem.RiskLikelihoodDescription;
                        worksheet.Cells[riskDataRowStart, 4].Value = riskItem.RiskImpactDescription;
                        worksheet.Cells[riskDataRowStart, 5].Value = riskItem.GrossRiskDescription;
                        worksheet.Cells[riskDataRowStart, 6].Value = riskItem.MitigationStrategy;
                        worksheet.Cells[riskDataRowStart, 6].Style.WrapText = true;
                        worksheet.Cells[riskDataRowStart, 7].Value = riskItem.ResidualLikelihoodDescription;
                        worksheet.Cells[riskDataRowStart, 8].Value = riskItem.ResidualImpactDescription;
                        worksheet.Cells[riskDataRowStart, 9].Value = riskItem.ResidualRiskDescription;

                        PersonDetails riskOwner = personDetails.FirstOrDefault(x => x.EmpNo.Trim() == riskItem.Owner);
                        if (riskOwner != null)
                        {
                            worksheet.Cells[riskDataRowStart, 10].Value = riskOwner.Forename + ' ' + riskOwner.Surname;
                        }
                        worksheet.Cells[riskDataRowStart, 11].Value = riskItem.ExternalOwner;
                        worksheet.Cells[riskDataRowStart, 11].Style.WrapText = true;
                        worksheet.Cells[riskDataRowStart, 12].Value = riskItem.Comments;
                        worksheet.Cells[riskDataRowStart, 12].Style.WrapText = true;
                        worksheet.Cells[riskDataRowStart, 13].Value = riskItem.LastUpdated.ToShortDateString();
                        //worksheet.Cells[riskDataRowStart, 12].Style.Numberformat.Format = "yyyy-MM-dd";
                        worksheet.Cells[riskDataRowStart, 14].Value = riskItem.StatusDescription;
                        //Increment the row counter
                        riskDataRowStart = riskDataRowStart + 1;
                    }

                    worksheet.Column(1).Width = 50;
                    worksheet.Column(2).AutoFit();
                    worksheet.Column(3).AutoFit();
                    worksheet.Column(4).AutoFit();
                    worksheet.Column(5).AutoFit();
                    worksheet.Column(6).Width = 50;
                    worksheet.Column(7).AutoFit();
                    worksheet.Column(8).AutoFit();
                    worksheet.Column(9).AutoFit();
                    worksheet.Column(10).AutoFit();
                    worksheet.Column(11).Width = 50;
                    worksheet.Column(12).Width = 50;
                    worksheet.Column(13).AutoFit();
                    worksheet.Column(14).AutoFit();


                    return excelPackage;

                }
                else
                {
                    return null;
                }


            }
            else
            {
                return null;
            }




        }

        #endregion

        #region Disposal

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AMPServiceLayer()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }
        #endregion
    }
}
#endregion