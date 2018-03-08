using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using AMP.Models;
using AMP.RiskClasses;
using AMP.Services;
using AMP.Utilities;
using AMP.ViewModels;
using OfficeOpenXml;

namespace AMP.Controllers
{
    public class ProjectController : BaseController
    {
        #region Initialise

        private IAmpServiceLayer _ampServiceLayer;
        private IIdentityManager _identityManager;


        public ProjectController()
            : base()
        {
            this._ampServiceLayer = new AMPServiceLayer();
            this._identityManager = new DemoIdentityManager();
        }

        public ProjectController(IAmpServiceLayer serviceLayer, IIdentityManager identityManager)
            : base(serviceLayer, identityManager)
        {
            this._ampServiceLayer = serviceLayer;
            this._identityManager = identityManager;
     
        }

        public ErrorEngine errorengine = new ErrorEngine();

        #endregion

        // Index: AMP Dashboard
        // Index: AMP Dashboard
        // eg: /Project/
        // eg: /Project/Index/ID_desc
        //eg: /Project/Index/ID_desc/2
        [Route("~/Project")]
        [Route("Project/Index/{sortOrder?}/{page?}/{searchString?}/{currentFilter?}")]
        ////[Route("~/")]


        public async Task<ActionResult> Index(string sortOrder, int? page, string searchString, string currentFilter)
        {

            return RedirectToAction("Index", "Dashboard");
            //try
            //{
            //    String user;

            //    //Setup Code Logging
            //    DateTime From;
            //    DateTime To;
            //    String MethodName = "Project/Index";
            //    String Description = "Test Total Project Index Code Performance";
            //    //Start Timing
            //    From = DateTime.Now;


            //    ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID_desc" : "";
            //    ViewBag.ApprovedBudgetParm = sortOrder == "ApprovedBudget" ? "ApprovedBudget_desc" : "ApprovedBudget";
            //    ViewBag.StageParm = sortOrder == "Stage" ? "Stage_desc" : "Stage";
            //    ViewBag.NextReviewParm = sortOrder == "NextReview" ? "NextReview_desc" : "NextReview";

            //    //To match the route a string is being passed if sortOrder is empty
            //    if (String.IsNullOrEmpty(sortOrder))
            //    {
            //        ViewBag.CurrentSort = "Firstload";
            //    }
            //    else
            //    {
            //        ViewBag.CurrentSort = sortOrder;
            //    }

            //    //Get logon
            //    user = GetEmpNo();

            //    ViewBag.User = "R" + user;

            //    //Log user on page
            //    LogCall(user, "Project/Index");

            //    if (searchString != null)
            //    {
            //        page = 1;
            //    }
            //    else
            //    {
            //        searchString = currentFilter;
            //    }

            //    ViewBag.CurrentFilter = searchString;

            //    // Control page size (10 fits the screen rather nice)
            //    //Page size max int value means there wont be any paging

            //    int pageSize = Session["isPagingEnabled"] == "F" ? int.MaxValue : 10;
            //    if (Session["isPagingEnabled"] == "F")
            //        ViewBag.PagingOn = "F";

            //    //If first load or null then page 1 or actual value.
            //    int pageNumber = (page ?? 1);

            //    // Get all Project
            //    DashboardViewModel dashboardVM;
            //    try
            //    {
            //        dashboardVM = await _ampServiceLayer.GetProjects(searchString, pageNumber, pageSize, user, sortOrder);
            //    }
            //    catch (Exception ex)
            //    {
            //        errorengine.LogError(ex, "GetProjects - Project/Index", user);
            //        throw;
            //    }

            //    ViewBag.PageNumber = pageNumber;
            //    ViewBag.PageSize = pageSize;
            //    ViewBag.ProjectFrom = dashboardVM.userprojects.FirstItemOnPage;
            //    ViewBag.ProjectTo = dashboardVM.userprojects.LastItemOnPage;

            //    // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            //    if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

            //    //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            //    if (TempData["Success"].ToString() == "1")
            //    {
            //        ViewBag.Success = "1";
            //    }
            //    if (TempData["Success"].ToString() == "0")
            //    {
            //        ViewBag.Success = "0";
            //    }

            //    TempData["VM"] = dashboardVM;

            //    ViewBag.DFIDTasksServer = ConfigurationManager.AppSettings["DFIDTasksUrl"].ToString();
            //    ViewBag.BaseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
            //    ViewBag.ARIESUrl = ConfigurationManager.AppSettings["ARIESUrl"].ToString();

            //    //End Timing and Record
            //    To = DateTime.Now;
            //    TimeSpan Result = To - From;
            //    _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            //    //Return the view as a PagedList along with page number and size.
            //    return View(dashboardVM);
            //}
            //catch (Exception ex)
            //{
            //    errorengine.LogError(ex, "");
            //    throw;
            //}
        }

        //[HttpPost]
        ////POST :/Project/Index
        //[Route("~/Project")]
        //[Route("Project/Index/{sortOrder?}/{page?}/{searchString?}/{currentFilter?}")]
        //public ActionResult Index(DashboardViewModel dashboardviewmodel, string NewProjectID, string SearchString, string SubmitButton, string id, string pagingEnabled)//, string id
        //{

        //    //Multiple submit buttons Exist. Workout which is incoming and send to the appropriate controller.
        //    if (id == null)
        //    {
        //        return (AddProject(dashboardviewmodel, NewProjectID, SearchString));
        //    }
        //    else
        //    {
        //        return (RemoveProject(id));
        //    }

        //}

        //// Index: Post Add Project
        //[HttpPost]
        //public ActionResult AddProject(DashboardViewModel dashboardviewmodel, string NewProjectID, string SearchString)
        //{

        //    //Get logon
        //    String user = GetEmpNo();


        //    //Log user on page
        //    LogCall(user, "Add Project Dashboard", NewProjectID);

        //    try
        //    {
        //        // Access the Current VM from TempData (May be empty if this is first GET)
        //        var OriginalVM = TempData["VM"] as DashboardViewModel;



        //        if (_ampServiceLayer.AddProject(NewProjectID, user, dashboardviewmodel, OriginalVM, new ModelStateWrapper(this.ModelState)))
        //        {

        //            //serviceLayer.AddSector(componentviewmodel,);
        //            // If modelState was valid, save will occur, set TempData to success code 1.
        //            TempData["Success"] = "1";
        //            ViewBag.ProjectAdded = "1";
        //            TempData["ProjectAdded"] = "1";

        //            //return Redirect("Index");

        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            //Model was invalid set success code to 0
        //            TempData["Success"] = "0";
        //            ViewBag.Success = "0";

        //            // Set TempData to current view model.
        //            TempData["VM"] = OriginalVM;
        //            ViewBag.User = "R" + user;
        //            ViewBag.DFIDTasksServer = ConfigurationManager.AppSettings["DFIDTasksUrl"].ToString();
        //            ViewBag.BaseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        //            ViewBag.ARIESUrl = ConfigurationManager.AppSettings["ARIESUrl"].ToString(); 
        //            //I think this can be removed:
        //            //Hello nUnit!! Beter spoof that redirect then.
        //            //if (this.Request != null)
        //            //{
        //            //    return Redirect(this.Request.RawUrl);
        //            //}
        //            //else
        //            //{
        //            //    //Hello nUnit!! Beter spoof that redirect then.
        //            //    return RedirectToAction("Index");
        //            //}

        //            return View(OriginalVM);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        errorengine.LogError(ex, "Add Project - Index/Post", user);
        //        throw;
        //    }
        //}

        //// Index: Post Add Project
        //[HttpPost]
        //public ActionResult SaveProjectToDashBoard(string id)
        //{

        //    //Get logon
        //    String user = GetEmpNo();


        //    //Log user on page
        //    LogCall(user, "Add Project to Dashboard via Project", id);

        //    try
        //    {
        //        try
        //        {
        //            bool result = _ampServiceLayer.AddProject(id, user);

        //            //return Redirect("Index");
        //            return Json("SUCCESS");
        //        }
        //        catch
        //        {
        //            //return Redirect("Index");
        //            return Json("SUCCESS");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorengine.LogError(ex, "Add Project - Index/Post", user);
        //        throw;
        //    }
        //}

        //// Index: Post
        //[HttpPost]
        //public ActionResult RemoveProject(string id)
        //{
        //    //Get logon
        //    String user = GetEmpNo();

        //    //Log user on page
        //    LogCall(user, "Remove project from dashboard", id);

        //    try
        //    {
        //        string ProjectToRemove = id;
        //        try
        //        {

        //            bool result = _ampServiceLayer.RemoveProject(ProjectToRemove, user);

        //            //return Redirect("Index");
        //            return Json("SUCCESS");
        //        }
        //        catch
        //        {
        //            //return Redirect("Index");
        //            return Json("SUCCESS");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorengine.LogError(ex, "Remove Project - Index/Post", user);
        //        throw;
        //    }
        //}

        //[HttpPost]
        //public ActionResult SetPagingPreference(string pagingEnabled)
        //{
        //    if (pagingEnabled == "F")
        //    {
        //        Session["isPagingEnabled"] = "F";
        //    }
        //    else
        //    {
        //        Session["isPagingEnabled"] = "T";
        //    }


        //    return Json("success");

        //    // return RedirectToAction("Index");
        //}



        // Project/Finance/id
        public async Task<ActionResult> Finance(string id)
        {
            //Get logon
            String user = GetEmpNo();
            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Project/Finance";
                String Description = "Test Total Project Finance Code Performance";

                //Start Timing
                From = DateTime.Now;

                //Log user on page
                LogCall(user, "Finance", id);

                //Get ARIES API This is specifically for the AJAX GET on the javascript
                ViewBag.ARIESAPI = ConfigurationManager.AppSettings["ARIESRestServiceUrl"].ToString();
                ViewBag.FinancialYear = ConfigurationManager.AppSettings["FinancialYear"].ToString();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (! await _ampServiceLayer.IsAuthorised(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }


                ProjectFinanceVM projectFinanceVm = await _ampServiceLayer.GetProjectFinancials(id, user);


                if (projectFinanceVm == null)
                {
                    return HttpNotFound();
                }


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                //Sort
                projectFinanceVm.ProjectFinance = projectFinanceVm.ProjectFinance.OrderBy(s => s.work_order).ThenBy(s => s.Year);

                return View("Finance", projectFinanceVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Project/Finance", user);
                throw;
            }
        }


        // Project/Procurement/id
        public async Task<ActionResult> Procurement(string id)
        {

            //Get logon
            String user = GetEmpNo();

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Project/Procurement";
                String Description = "Test Total Project Procurement Code Performance";

                //Start Timing
                From = DateTime.Now;

                //Log user on page
                LogCall(user, "Procurement", id);

                //Get ARIES API This is specifically for the AJAX GET on the javascript
                ViewBag.ARIESAPI = ConfigurationManager.AppSettings["ARIESRestServiceUrl"].ToString();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!await _ampServiceLayer.IsAuthorised(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ProjectProcurementVM projectProcurementVm = await _ampServiceLayer.GetProjectProcurement(id, user);


                if (projectProcurementVm == null)
                {
                    return HttpNotFound();
                }


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                //Sort
                //projectViewModel.ProjectFinance = projectViewModel.ProjectFinance.OrderBy(s => s.work_order);

                return View("Procurement", projectProcurementVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Project/Procurement", user);
                throw;
            }
        }

        // Project/Evaluation/id
        public async Task<ActionResult> Evaluation(string id)
        {

            //Get logon
            String user = GetEmpNo();

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Project/Evaluation";
                String Description = "Test Total Project Evaluation Code Performance";

                //Start Timing
                From = DateTime.Now;

                //Log user on page
                LogCall(user, "Evaluation", id);



                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProjectEvaluationVM projectEvaluationVm = await _ampServiceLayer.GetProjectEvaluation(id, user);


                if (projectEvaluationVm == null)
                {
                    return HttpNotFound();
                }


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["Success"].ToString() == "1")
                {
                    ViewBag.Success = "1";
                }
                if (TempData["Success"].ToString() == "0")
                {
                    ViewBag.Success = "0";
                }
                TempData["VM"] = projectEvaluationVm;


                return View("Evaluations", projectEvaluationVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Project/Evaluation", user);
                throw;
            }
        }

        [HttpPost]
        public ActionResult RemoveEvaluationDocument(string DocumentID, string EvaluationID, string ProjectID)
        {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Delete Evaluation Document (POST)", ProjectID);

            try
            {

                if (_ampServiceLayer.DeleteEvaluationDocument(DocumentID, EvaluationID, ProjectID, user))
                {
                    //return Redirect("Index");
                    return Json(new { success = true, response = "Successful message" });

                }
                return Json(new { success = true, response = "Successful message" });
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Delete Evaluation Document (POST)", user);
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Evaluation(ProjectEvaluationVM projectEvaluation, string submitButton)
        {
            //Multiple submit buttons Exist. Wrkout which is incoming and send to the appropriate controller.
            switch (submitButton)
            {
                case "Add New Document":
                    return (AddEvaluationDocument(projectEvaluation));
                case "Save Evaluation":
                    return (UpdateEvaluation(projectEvaluation));
                default:
                    return (AddEvaluationDocument(projectEvaluation));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEvaluationDocument(ProjectEvaluationVM projectEvaluationVm)
        {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Add Evaluation Document (POST)", projectEvaluationVm.projectHeader.ProjectID);

            if (_ampServiceLayer.AddEvaluationDocument(projectEvaluationVm, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                ViewBag.Success = "1";
                return RedirectToAction("Evaluation");
            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";

                var OriginalVM = TempData["VM"] as ProjectEvaluationVM;
                TempData["VM"] = OriginalVM;
                ViewBag.Success = "0";

                //State management for returning original view model data.
                //projectEvaluationVm.NewEvaluationDocument.DocumentID = OriginalVM.NewEvaluationDocument.DocumentID;
                //projectEvaluationVm.NewEvaluationDocument.Description = OriginalVM.NewEvaluationDocument.Description;

                //State management for returning original view model data.
                projectEvaluationVm.projectHeader = OriginalVM.projectHeader;
                projectEvaluationVm.EvaluationDocuments = OriginalVM.EvaluationDocuments;
                projectEvaluationVm.EvaluationType = OriginalVM.EvaluationType;
                projectEvaluationVm.EvaluationTypeID = OriginalVM.EvaluationTypeID;
                projectEvaluationVm.EvaluationTypes = OriginalVM.EvaluationTypes;
                projectEvaluationVm.IfOther = OriginalVM.IfOther;
                projectEvaluationVm.ManagementOfEvaluation = OriginalVM.ManagementOfEvaluation;
                projectEvaluationVm.EvaluationManagement = OriginalVM.EvaluationManagement;
                projectEvaluationVm.EvaluationManagements = OriginalVM.EvaluationManagements;
                projectEvaluationVm.ProjectID = OriginalVM.ProjectID;

                return View("Evaluations", projectEvaluationVm);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEvaluation(ProjectEvaluationVM projectEvaluationVm)
        {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Update Evaluation (POST)", projectEvaluationVm.projectHeader.ProjectID);

            if (_ampServiceLayer.UpdateEvaluation(projectEvaluationVm, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                ViewBag.Success = "1";
                return RedirectToAction("Evaluation");

            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";

                var OriginalVM = TempData["VM"] as ProjectEvaluationVM;
                TempData["VM"] = OriginalVM;
                ViewBag.Success = "0";


                //State management for returning original view model data.
                projectEvaluationVm.projectHeader = OriginalVM.projectHeader;
                projectEvaluationVm.EvaluationDocuments = OriginalVM.EvaluationDocuments;
                projectEvaluationVm.EvaluationType = OriginalVM.EvaluationType;
                projectEvaluationVm.EvaluationTypeID = OriginalVM.EvaluationTypeID;
                projectEvaluationVm.EvaluationTypes = OriginalVM.EvaluationTypes;
                projectEvaluationVm.IfOther = OriginalVM.IfOther;
                projectEvaluationVm.ManagementOfEvaluation = OriginalVM.ManagementOfEvaluation;
                projectEvaluationVm.EvaluationManagement = OriginalVM.EvaluationManagement;
                projectEvaluationVm.EvaluationManagements = OriginalVM.EvaluationManagements;
                projectEvaluationVm.ProjectID = OriginalVM.ProjectID;

                return View("Evaluations", projectEvaluationVm);
            }

        }
        // Project/Statements/id
        public async Task<ActionResult> Statements(string id)
        {

            //Get logon
            String user = GetEmpNo();

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Project/Statements";
                String Description = "Test Total Project Statements Code Performance";

                //Start Timing
                From = DateTime.Now;

                //Log user on page
                LogCall(user, "Statements", id);



                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProjectStatementVM projectStatementVm = await _ampServiceLayer.GetProjectStatements(id, user);


                if (projectStatementVm == null)
                {
                    return HttpNotFound();
                }

                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["Success"].ToString() == "1")
                {
                    ViewBag.Success = "1";
                }
                if (TempData["Success"].ToString() == "0")
                {
                    ViewBag.Success = "0";
                }

                projectStatementVm.StatementTypes = projectStatementVm.StatementTypes.OrderByDescending(x => x);

                TempData["VM"] = projectStatementVm;


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                //Sort
                //projectViewModel.ProjectFinance = projectViewModel.ProjectFinance.OrderBy(s => s.work_order);

                return View("Statements", projectStatementVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Project/Statements", user);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Statements(ProjectStatementVM projectStatementVm)
        {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Add Statement (POST)", projectStatementVm.ProjectHeader.ProjectID);

            if (_ampServiceLayer.AddStatement(projectStatementVm, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                ViewBag.Success = "1";
                return RedirectToAction("Statements");
            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";
                ViewBag.Success = "0";

                var origprojstatementvm = TempData["VM"] as ProjectStatementVM;


                //State management for returning original view model data.
                //projectStatementVm.ProjectHeader = OriginalVM.ProjectHeader;
                //projectStatementVm.ProjectStatement = OriginalVM.ProjectStatement;
                //projectStatementVm.NewProjectStatement = OriginalVM.NewProjectStatement;
                //projectStatementVm.StatementTypes = OriginalVM.StatementTypes;
                DateTime parsedReceivedDate;
                DateTime parsedPeriodFromDate;
                DateTime parsedPeriodToDate;
                
                if (DateTime.TryParse(string.Format("{0}/{1}/{2}",projectStatementVm.NewProjectStatement.ReceivedDate_Day, projectStatementVm.NewProjectStatement.ReceivedDate_Month, projectStatementVm.NewProjectStatement.ReceivedDate_Year), out parsedReceivedDate)) 
                {
                    projectStatementVm.NewProjectStatement.ReceivedDate = parsedReceivedDate;
                }

                if (DateTime.TryParse(string.Format("{0}/{1}/{2}", projectStatementVm.NewProjectStatement.PeriodFrom_Day, projectStatementVm.NewProjectStatement.PeriodFrom_Month, projectStatementVm.NewProjectStatement.PeriodFrom_Year), out parsedPeriodFromDate))
                {
                    projectStatementVm.NewProjectStatement.PeriodFrom = parsedPeriodFromDate;
                }

                if (DateTime.TryParse(string.Format("{0}/{1}/{2}", projectStatementVm.NewProjectStatement.PeriodTo_Day, projectStatementVm.NewProjectStatement.PeriodTo_Month, projectStatementVm.NewProjectStatement.PeriodTo_Year), out parsedPeriodToDate))
                {
                    projectStatementVm.NewProjectStatement.PeriodTo = parsedPeriodToDate;
                }


                projectStatementVm.StatementTypes = origprojstatementvm.StatementTypes;
                projectStatementVm.ProjectStatement = origprojstatementvm.ProjectStatement;
             

                TempData["VM"] = projectStatementVm;
               
                return View("Statements", projectStatementVm);
            }
        }
        [HttpPost]

        public ActionResult DeleteStatement(string projectId, int statementid)
        {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Delete Statement (POST)", projectId);

            try
            {

                if (_ampServiceLayer.DeleteStatement(projectId, statementid, user))
                {
                    //return Redirect("Index");
                    return Json(new { success = true, response = "Successful message" });

                }
                return Json(new { success = true, response = "Successful message" });
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Delete Statement (POST)", user);
                throw;
            }

        }

        // Project/Documents/id
        public async Task<ActionResult> Documents(string id)
        {

            //Get logon
            String user = GetEmpNo();

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Project/Documents";
                String Description = "Test Total Project Documents Code Performance";

                //Start Timing
                From = DateTime.Now;

                //Log user on page
                LogCall(user, "Documents", id);

                //Get ARIES API This is specifically for the AJAX GET on the javascript
                ViewBag.ARIESAPI = ConfigurationManager.AppSettings["ARIESRestServiceUrl"].ToString();
                ViewBag.EDRMServiceUrl = ConfigurationManager.AppSettings["EDRMServiceUrl"];
                ViewBag.Logon = GetLogon();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProjectDocumentsVM projectDocumentsVm = await _ampServiceLayer.GetProjectDocuments(id, user);


                if (projectDocumentsVm == null)
                {
                    return HttpNotFound();
                }


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                //Sort
                //projectViewModel.ProjectFinance = projectViewModel.ProjectFinance.OrderBy(s => s.work_order);

                return View("Documents", projectDocumentsVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Project/Documents", user);
                throw;
            }
        }

        // Project/PublishedDocuments/id
        public async Task<ActionResult> Transparency(string id)
        {

            //Get logon
            String user = GetEmpNo();

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Project/Transparency";
                String Description = "Test Total Project Published Documents in DevTracker Code Performance";

                //Start Timing
                From = DateTime.Now;

                //Log user on page
                LogCall(user, "Transparency", id);

                //Get ARIES API This is specifically for the AJAX GET on the javascript
                ViewBag.ARIESAPI = ConfigurationManager.AppSettings["ARIESRestServiceUrl"].ToString();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                PublishedDocumentsVM publishedDocumentsVm = await _ampServiceLayer.GetPublishedProjectDocumentsInDevTracker(id, user);


                if (publishedDocumentsVm == null)
                {
                    return HttpNotFound();
                }


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                //Sort
                //projectViewModel.ProjectFinance = projectViewModel.ProjectFinance.OrderBy(s => s.work_order);

                return View("Transparency", publishedDocumentsVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Project/Transparency", user);
                throw;
            }
        }

        // Project GeoCoding/id
        public async Task<ActionResult> GeoCoding(string id)
        {
            //GeoURL

            string geoUrl = ConfigurationManager.AppSettings["GeoUrl"] + id;

            return Redirect(geoUrl);
            //ViewBag.GeoURL = AMPUtilities.GeoURL();
            //try
            //{


            //    //Setup Code Logging
            //    DateTime From;
            //    DateTime To;
            //    String MethodName = "Project/GeoCoding";
            //    String Description = "Test Total Project GeoCoding Code Performance";

            //    //Start Timing
            //    From = DateTime.Now;

            //    //Get logon
            //    String user = GetEmpNo();

            //    //Log user on page
            //    LogCall(user, "GeoCoding", id);

            //    // change this to get GeoCoding Data!
            //    ProjectLocationVM projectLocationVm = await _ampServiceLayer.GetGeoCoding(id, user);


            //    if (projectLocationVm == null)
            //    {
            //        return HttpNotFound();
            //    }

            //    if (id == null)
            //    {
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //    }

            //    //End Timing and Record
            //    To = DateTime.Now;
            //    TimeSpan Result = To - From;
            //    _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            //    return View("GeoCoding", projectLocationVm);
            //}
            //catch (Exception ex)
            //{
            //    errorengine.LogError(ex, "");
            //    throw;
            //}
        }

        // Project/Tasks/id 
        public async Task<ActionResult> Tasks(string id)
        {

            //Get logon
            String user = GetEmpNo();

            //Log user on page
            LogCall(user, "Tasks", id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProjectViewModel projectViewModel = await _ampServiceLayer.GetProjectTasks(id);

            return View("Tasks", projectViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Team(ProjectTeamVM projectTeamVm, string SubmitButton)
        {

            //Multiple submit buttons Exist. Wrkout which is incoming and send to the appropriate controller.
            switch (SubmitButton)
            {
                case "Add New Team Member":
                    return (AddTeamMember(projectTeamVm));
                case "Save Team Marker":
                    return (SaveTeamMarker(projectTeamVm));
                default:
                    return (SaveTeamMarker(projectTeamVm));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTeamMarker(ProjectTeamVM projectTeamVm)
        {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Save Team Marker (POST)", projectTeamVm.ProjectHeader.ProjectID);


            if (_ampServiceLayer.UpdateTeamMarker(projectTeamVm, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                ViewBag.Success = "1";
                return RedirectToAction("Team");
            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";

                var OriginalVM = TempData["VM"] as ProjectTeamVM;
                TempData["VM"] = OriginalVM;
                ViewBag.Success = "0";

                //State management for returning original view model data.
                projectTeamVm.NewTeamMember.ProjectRolesVm.ProjectRoleValues = OriginalVM.NewTeamMember.ProjectRolesVm.ProjectRoleValues;
                projectTeamVm.CurrentProjectTeam = OriginalVM.CurrentProjectTeam;
                projectTeamVm.FormerProjectTeam = OriginalVM.FormerProjectTeam;
                projectTeamVm.TeamMarker = OriginalVM.TeamMarker;
                projectTeamVm.ProjectHeader = OriginalVM.ProjectHeader;
                projectTeamVm.TeamMarker = OriginalVM.TeamMarker;
                projectTeamVm.OtherProjectTeam = OriginalVM.OtherProjectTeam;
                projectTeamVm.NewTeamMember = null;

                NewTeamMemberVM resetteam = new NewTeamMemberVM();

                ProjectRoleVM newRoles;
                newRoles = OriginalVM.NewTeamMember.ProjectRolesVm;

                resetteam.ProjectRolesVm = newRoles;

                IEnumerable<ProjectRoleValuesVM> projectRoles;
                projectRoles = OriginalVM.NewTeamMember.ProjectRolesVm.ProjectRoleValues;

                resetteam.ProjectRolesVm.ProjectRoleValues = projectRoles;

                //projectTeamVm.NewTeamMember.ProjectRolesVm.ProjectRoleValues = OriginalVM.NewTeamMember.ProjectRolesVm.ProjectRoleValues;
                resetteam.ProjectRolesVm.SelectedRoleValue = OriginalVM.NewTeamMember.ProjectRolesVm.SelectedRoleValue;
                projectTeamVm.NewTeamMember = resetteam;
                projectTeamVm.NewTeamMember.TeamID = OriginalVM.NewTeamMember.TeamID;

                return View("Team", projectTeamVm);
            }


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTeamMember(ProjectTeamVM projectTeamVm)
        {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Add Team Member (POST)", projectTeamVm.ProjectHeader.ProjectID);

            //Fudge the TeamID. If this is a repost after a validation fail, the hidden field drops the leading zero. If the TeamID is of length 5, stick a zero on it - 28 Aug 2015 C Finnan
            if (projectTeamVm.NewTeamMember.TeamID != null && projectTeamVm.NewTeamMember.TeamID.Length < 6)
            {
                projectTeamVm.NewTeamMember.TeamID = "0" + projectTeamVm.NewTeamMember.TeamID;
            }
            if (_ampServiceLayer.AddTeamMember(projectTeamVm, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                ViewBag.Success = "1";
                return RedirectToAction("Team");
            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";

                var OriginalVM = TempData["VM"] as ProjectTeamVM;
                TempData["VM"] = OriginalVM;
                ViewBag.Success = "0";

                //State management for returning original view model data.
                projectTeamVm.NewTeamMember.ProjectRolesVm = OriginalVM.NewTeamMember.ProjectRolesVm;
                projectTeamVm.CurrentProjectTeam = OriginalVM.CurrentProjectTeam;
                projectTeamVm.FormerProjectTeam = OriginalVM.FormerProjectTeam;
                projectTeamVm.TeamMarker = OriginalVM.TeamMarker;
                projectTeamVm.ProjectHeader = OriginalVM.ProjectHeader;
                projectTeamVm.TeamMarker = OriginalVM.TeamMarker;
                projectTeamVm.OtherProjectTeam = OriginalVM.OtherProjectTeam;
                //projectTeamVm.NewTeamMember = null;

                //NewTeamMemberVM resetteam = new NewTeamMemberVM();

                //ProjectRoleVM newRoles;
                //newRoles = OriginalVM.NewTeamMember.ProjectRolesVm;


                //resetteam.ProjectRolesVm = newRoles;

                //IEnumerable<ProjectRoleValuesVM> projectRoles;
                //projectRoles = OriginalVM.NewTeamMember.ProjectRolesVm.ProjectRoleValues;

                //resetteam.ProjectRolesVm.ProjectRoleValues = projectRoles;

                ////projectTeamVm.NewTeamMember.ProjectRolesVm.ProjectRoleValues = OriginalVM.NewTeamMember.ProjectRolesVm.ProjectRoleValues;
                //resetteam.ProjectRolesVm.SelectedRoleValue =  OriginalVM.NewTeamMember.ProjectRolesVm.SelectedRoleValue;
                //projectTeamVm.NewTeamMember = resetteam;
                //projectTeamVm.NewTeamMember.TeamID = OriginalVM.NewTeamMember.TeamID;

                TempData["SelectedTeamID"] = projectTeamVm.NewTeamMember.TeamID;

                return View("Team", projectTeamVm);
            }


        }


        //Project/Team/id
        public async Task<ActionResult> Team(string id)
        {
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/Team";
            String Description = "Test Total Project Team Code Performance";

            //Start Timing
            From = DateTime.Now;


            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Team", id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTeamVM projectTeamVm = await _ampServiceLayer.GetProjectTeam(id, user);

            if (projectTeamVm == null)
            {
                return HttpNotFound();
            }


            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["Success"].ToString() == "1")
            {
                ViewBag.Success = "1";
            }
            if (TempData["Success"].ToString() == "0")
            {
                ViewBag.Success = "0";
            }
            TempData["VM"] = projectTeamVm;

            return View(projectTeamVm);
        }

        //Project/EditTeamMember/id
        public async Task<ActionResult> EditTeamMember(string id)
        {
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/EditTeamMember";
            String Description = "Test Total Edit Project Team Member Code Performance";

            //Start Timing
            From = DateTime.Now;


            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/EditTeamMember", id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditTeamMemberVM editTeamMemberVm = new EditTeamMemberVM();

            editTeamMemberVm = await _ampServiceLayer.GetTeamMember(id);

            if (editTeamMemberVm == null)
            {
                return HttpNotFound();
            }

            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["Success"].ToString() == "1")
            {
                ViewBag.Success = "1";
            }
            if (TempData["Success"].ToString() == "0")
            {
                ViewBag.Success = "0";
            }
            TempData["VM"] = editTeamMemberVm;


            return PartialView(editTeamMemberVm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTeamMember(EditTeamMemberVM teamMemberVm)
        {
            string user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Edit Team Member (POST)", teamMemberVm.ProjectID);

            try
            {
                bool isUpdateTeamMemberSuccessful = _ampServiceLayer.UpdateTeamMember(teamMemberVm, user);
                if (!isUpdateTeamMemberSuccessful)
                {
                    var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                    var message = "Error: " + Environment.NewLine;
                    message = modelStateErrors.Aggregate(message, (current, modelStateError) => current + (modelStateError.ErrorMessage + Environment.NewLine));
                    return Json(new { success = false, response = message });
                }
            }

            catch (BusinessLogicException Ex)
            {
                ViewBag.ResultMessage = Ex.Message;
                ViewBag.IsSuccess = "0";
            }


            //Set a success message in TempData
            TempData["Success"] = "1";
            return Json(new { success = true, response = "Successful message" });

        }


        [HttpPost]

        public ActionResult EndTeamMember(EditTeamMemberVM teamMemberVm)
        {
            string user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/End Team Member (POST)", null);

            try
            {
                bool isUpdateTeamMemberSuccessful = _ampServiceLayer.EndTeamMember(teamMemberVm, user);
                if (!isUpdateTeamMemberSuccessful)
                {
                    var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                    var message = "Error: " + Environment.NewLine;
                    message = modelStateErrors.Aggregate(message, (current, modelStateError) => current + (modelStateError.ErrorMessage + Environment.NewLine));

                    return Json(new { success = false, response = message });
                }
                ViewBag.Success = "1";
                TempData["Success"] = "1";
            }

            catch (BusinessLogicException Ex)
            {
                ViewBag.ResultMessage = Ex.Message;
                TempData["Success"] = "0";
                ViewBag.Success = "0";
            }

            return Json(new { success = true, response = "Successful message" });

        }

        // Project/Reviews/id 
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public async Task<ActionResult> Reviews(string id)
        {
            //Get logon
            String user = GetEmpNo();
            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Project/Reviews";
                String Description = "Test Total Project Reviews Code Performance";

                //Start Timing
                From = DateTime.Now;
                //Log user on page
                LogCall(user, "Reviews", id);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProjectReviewVM projectReviewVm = await _ampServiceLayer.GetProjectReviews(id, user);

                if(projectReviewVm==null)
                {
                    return HttpNotFound();
                }

                // Get review related Quest documents 
                // projectReviewVm.ReviewDocuments = await _ampServiceLayer.GetReviewDocuments(id, projectReviewVm.ReviewVm.ReviewID, user);

                var reviewVm = new ReviewVM();
                ReviewOutputVM ReviewOutputVM = new ReviewOutputVM();
                reviewVm.ReviewOutputVm = ReviewOutputVM;
                projectReviewVm.ReviewVm = reviewVm;

                if (projectReviewVm == null)
                {
                    return HttpNotFound();
                }

                //populate default values for null reviews
                ProjectViewModel pvm = await _ampServiceLayer.GetProject(projectReviewVm.ProjectHeader.ProjectID);
                if (projectReviewVm.Performance == null)
                {
                    PerformanceVM perfVM = new PerformanceVM();
                    perfVM.ARDueDate = pvm.ProjectDates.ActualStartDate.Value.AddYears(1);
                    perfVM.ARPromptDate = perfVM.ARDueDate.Value.AddMonths(-3);
                    perfVM.PCRDueDate = pvm.ProjectDates.OperationalEndDate.Value.AddMonths(3);
                    perfVM.PCRPrompt = perfVM.PCRDueDate.Value.AddMonths(-6);
                    int projectDurationInMonth = pvm.ProjectDates.OperationalEndDate.Value.Month - pvm.ProjectDates.ActualStartDate.Value.Month;


                    perfVM.ARRequired = "Yes";
                    perfVM.PCRRequired = "Yes";

                    //From John : 2.	Exempt Admin Projects - admin budget centre (Axxx) and should also apply for Front Line delivery budget centres (APxxx) and admin capital budget centres (C0xxx).
                    if (pvm.ProjectMaster.BudgetCentreID.Trim().ToUpper().StartsWith("A") || pvm.ProjectMaster.BudgetCentreID.Trim().ToUpper().StartsWith("C0")) //Axxx covers APxxx
                    {
                        perfVM.ARRequired = "No";
                        perfVM.PCRRequired = "No";

                    }

                    if (projectDurationInMonth <= 15)
                        perfVM.ARRequired = "No";

                    projectReviewVm.Performance = perfVM;
                }


                if (projectReviewVm.ReviewMaster == null)
                {
                    ReviewMasterVM reviewMasteVM = new ReviewMasterVM();
                    reviewMasteVM.ReviewDate = DateTime.Now;
                    projectReviewVm.ReviewMaster = reviewMasteVM;
                }

                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["Success"].ToString() == "1")
                {
                    ViewBag.Success = "1";
                }
                if (TempData["Success"].ToString() == "0")
                {
                    ViewBag.Success = "0";
                }

                //----------------------------Another form for authorisation and so another success message---------------
                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["ARSuccessAuth"] == null) { TempData["ARSuccessAuth"] = "NA"; ViewBag.ARSuccessAuth = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["ARSuccessAuth"].ToString() == "1")
                {
                    ViewBag.ARSuccessAuth = "1";
                }
                if (TempData["ARSuccessAuth"].ToString() == "0")
                {
                    ViewBag.ARSuccessAuth = "0";
                }

                //----------------------------Another form for review submission and so another success message---------------
                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["ARSuccessSubmission"] == null) { TempData["ARSuccessSubmission"] = "NA"; ViewBag.ARSuccessSubmission = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["ARSuccessSubmission"].ToString() == "1")
                {
                    ViewBag.ARSuccessSubmission = "1";
                }
                if (TempData["ARSuccessSubmission"].ToString() == "0")
                {
                    ViewBag.ARSuccessSubmission = "0";
                }

                //----------------------------Another form for review PCR submission and so another success message---------------
                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["PCRSuccessSubmission"] == null) { TempData["PCRSuccessSubmission"] = "NA"; ViewBag.PCRSuccessSubmission = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["PCRSuccessSubmission"].ToString() == "1")
                {
                    ViewBag.PCRSuccessSubmission = "1";
                }
                if (TempData["PCRSuccessSubmission"].ToString() == "0")
                {
                    ViewBag.PCRSuccessSubmission = "0";
                }

                if (TempData["PCRSuccessSubmission"].ToString() == "2") // no inputter for project so cannot send for PCR approval
                {
                    ViewBag.PCRSuccessSubmission = "2";
                }
                //----------------------------Another form for PCR authorisation and so another success message---------------
                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["PCRSuccessAuth"] == null) { TempData["PCRSuccessAuth"] = "NA"; ViewBag.PCRSuccessAuth = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["PCRSuccessAuth"].ToString() == "1")
                {
                    ViewBag.PCRSuccessAuth = "1";
                }
                if (TempData["PCRSuccessAuth"].ToString() == "0")
                {
                    ViewBag.PCRSuccessAuth = "0";
                }

                //ViewBag.ExemptApproveSuccess
                //----------------------------success message for AR/PCR Exclusion---------------
                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["ExemptApproveSuccess"] == null) { TempData["ExemptApproveSuccess"] = "NA"; ViewBag.ExemptApproveSuccess = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["ExemptApproveSuccess"].ToString() == "1")
                {
                    ViewBag.ExemptApproveSuccess = "1";
                }
                if (TempData["ExemptApproveSuccess"].ToString() == "0")
                {
                    ViewBag.ExemptApproveSuccess = "0";
                }

                //----------------------------Another form for review submission and so another success message---------------
                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["ReviewDeleted"] == null) { TempData["ReviewDeleted"] = "NA"; ViewBag.ReviewDeleted = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["ReviewDeleted"].ToString() == "1")
                {
                    ViewBag.ReviewDeleted = "1";
                }
                if (TempData["ReviewDeleted"].ToString() == "0")
                {
                    ViewBag.ReviewDeleted = "0";
                }


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                return View("Reviews", projectReviewVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Project/Reviews", user);
                throw;
            }

        }

        //Review output scoring table data
        [Route("Project/ReviewOutputScoringTableData/{projectId}/{reviewId}")]
        [Route("Project/ReviewOutputScoringTableData/{projectId}/{reviewId}/{tableIndex}")]
        [Route("Project/ReviewOutputScoringTableData/{projectId}/{reviewId}/{tableIndex}/{TableTypePcr}")]
        [OutputCache(Duration = 1, VaryByParam = "*")]
        public async Task<ActionResult> ReviewOutputScoringTableData(string projectId, int reviewId, int? tableIndex, string TableTypePcr)
        {

            ViewBag.tabletype = TableTypePcr;

            var projectViewModel = new ProjectViewModel();
            var reviewVm = new ReviewVM();
            
            reviewVm.ReviewID = reviewId;
            reviewVm.ProjectID = projectId;

            //if (tableIndex.HasValue)
            //{
            //    reviewVm.ReviewID = reviewId;
            //    reviewVm.ProjectID = projectId;
            //}

            IEnumerable<ReviewOutputVM> reviewOutputs = await _ampServiceLayer.GetProjectReviewScores(projectId, reviewId);
            reviewVm.ReviewOutputs = reviewOutputs;
            projectViewModel.ReviewVm = reviewVm;
            return PartialView("_ReviewOutputScoringTableData", projectViewModel);
        }

        [Route("Project/ReviewDocuments/{projectId}/{reviewId}")]
        [Route("Project/ReviewDocuments/{projectId}/{reviewId}/{tableIndex}")]
        [OutputCache(Duration = 1, VaryByParam = "*")]
        public async Task<ActionResult> ReviewDocuments(string projectId, int reviewId, int? tableIndex)
        {
            var projectViewModel = new ProjectViewModel();
            var reviewVm = new ReviewVM();
            //The review id is wrong how ever we need to pass the index value to the partial view. Need to figure out how can we achieve this. 

            //if (tableIndex.HasValue)
            //{
            //    ViewBag.tabletype = (int)tableIndex;
            //}
            //else
            //{
            //    ViewBag.tabletype = "PCR";
            //}
            
            reviewVm.ReviewID = reviewId;
            reviewVm.ProjectID = projectId;
            IEnumerable<ReviewDocumentVM> reviewDocuments = await _ampServiceLayer.GetReviewDocuments(projectId, reviewId);
            reviewVm.ReviewDocuments = reviewDocuments;
            projectViewModel.ReviewVm = reviewVm;
            return PartialView("_ReviewDocuments", projectViewModel);
        }


        public async Task<ActionResult> RiskRegister(string id)
        {
            String user = GetEmpNo();
            LogCall(user, "Project/RiskRegister", id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskRegisterVM riskRegisterVm = await _ampServiceLayer.GetRiskRegister(id, user);
            //riskRegisterVm = await _ampServiceLayer.GetRiskRegister(id, user);

            //Bind the Dropdown boxes 
            ViewBag.RiskCategorydd = new SelectList(riskRegisterVm.RiskCategoryValues, "ID", "RiskCategoryDescription", riskRegisterVm.RiskItemVm.RiskCategory);
            ViewBag.RiskLikelihooddd = new SelectList(riskRegisterVm.RiskLikelihoodValues, "ID", "RiskLikelihoodDescription", riskRegisterVm.RiskItemVm.RiskLikelihood);
            ViewBag.RiskImpact = new SelectList(riskRegisterVm.RiskImpactValues, "ID", "RiskImpactDescription", riskRegisterVm.RiskItemVm.RiskImpact);
            ViewBag.GrossRiskList = new SelectList(riskRegisterVm.RiskValues, "RiskValue", "RiskTitle", riskRegisterVm.RiskItemVm.GrossRiskDescription);

            ViewBag.ResidualRiskLikelihooddd = new SelectList(riskRegisterVm.RiskLikelihoodValues, "ID", "RiskLikelihoodDescription", riskRegisterVm.RiskItemVm.ResidualLikelihood);
            ViewBag.ResidualRiskImpact = new SelectList(riskRegisterVm.RiskImpactValues, "ID", "RiskImpactDescription", riskRegisterVm.RiskItemVm.ResidualImpact);
            ViewBag.ResidualRiskList = new SelectList(riskRegisterVm.RiskValues, "RiskValue", "RiskTitle", riskRegisterVm.RiskItemVm.ResidualRiskDescription);

            return View("RiskRegister", riskRegisterVm);
        }

        [HttpPost]
        public string PostRiskRegisterItem(RiskItemVM riskItemVm)
        {
            String user = GetEmpNo();
            if (riskItemVm.ProjectID == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }
            if (ModelState.IsValid)
            {
                //AddRiskRegisterItem
                if(_ampServiceLayer.PostRiskRegisterItem(riskItemVm, user))
                {
                    return "Saved";
                }
                else
                {
                    return "Failed";
                }
            }

            string messages = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
            return messages;

        }
        
        [HttpPost]
        public string PostOverallRiskRating(OverallRiskRatingVM overallRiskRatingVm)
        {
            String user = GetEmpNo();
            if (overallRiskRatingVm.ProjectID == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }
            if (ModelState.IsValid)
            {
                if (_ampServiceLayer.PostOverallRiskRating(overallRiskRatingVm, user))
                {
                    return "Saved";
                }
                else
                {
                    return "Failed";
                }
            }

            string messages = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
            return messages;

        }

        public async Task<ActionResult> RefreshRiskTable(string id)
        {

            String user = GetEmpNo();
            LogCall(user, "Project/RefreshRiskTable", id);

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                RiskItemsVM riskItemsVm = new RiskItemsVM();
                riskItemsVm = _ampServiceLayer.GetRiskTableData(id, user);

                return PartialView("_RiskTable", riskItemsVm);

            }
            catch (Exception exception)
            {
                errorengine.LogError(id, exception, user);
                throw;
            }
        }

        public async Task<ActionResult> RefreshOverallRiskRatingTable(string id)
        {

            String user = GetEmpNo();
            LogCall(user, "Project/RefreshOverallRiskRatingTable", id);

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                OverallRiskRatingsVM overallRiskRatingsVm = new OverallRiskRatingsVM();
                overallRiskRatingsVm = _ampServiceLayer.GetOverallRiskRatingTableData(id, user);

                return PartialView("_OverallRiskRatingTable", overallRiskRatingsVm);

            }
            catch (Exception exception)
            {
                errorengine.LogError(id, exception, user);
                throw;
            }
        }

        public async Task<ActionResult> RefreshRiskDocumentTable(string id)
        {
            String user = GetEmpNo();
            LogCall(user, "Project/RefreshRiskDocTable", id);

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RiskDocumentsVM riskDocumentsVm= new RiskDocumentsVM();
                riskDocumentsVm = _ampServiceLayer.GetRiskDocumentTableData(id, user);
                
                return PartialView("_riskDocumentTable", riskDocumentsVm);
                
               
            }
            catch (Exception exception)
            {
                errorengine.LogError(id, exception, user);
                throw;
            }
        }


        //[HttpPost]
        //public string AddRiskRatings(RiskRegisterVM riskRegisterVm)
        //{
        //    String user = GetEmpNo();
        //    if (riskRegisterVm.ProjectHeader.ProjectID == null)
        //    {
        //        return HttpStatusCode.BadRequest.ToString();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        //AddRiskRegisterItem
        //        Tuple<string> result = _ampServiceLayer.AddRiskRegisterItem(riskRegisterVm, new ModelStateWrapper(ModelState), user);
        //        if (result.Item1 == "Failed")
        //        {
        //            return "Failed";
        //        }
        //        else
        //        {
        //            return "Saved";
        //        }
        //    }

        //    string messages = string.Join("; ", ModelState.Values
        //                               .SelectMany(x => x.Errors)
        //                               .Select(x => x.ErrorMessage));
        //    return messages;
        //}

        //public async Task<ActionResult> RiskRegisterDocuments(string projectId)
        //{
        //    String user = GetEmpNo();

        //    var projectViewModel = new ProjectViewModel();
        //    var reviewVm = new ReviewVM();

        //    IEnumerable<RiskRegisterVM> riskRegister = await _ampServiceLayer.GetRiskRegisterDocumets(projectId, user);
        //    reviewVm.RiskRegister = riskRegister;
        //    projectViewModel.ReviewVm = reviewVm;

        //    return PartialView("_RiskRegisterDocuments", projectViewModel);
        //}

        //GetOverallRiskRatings
        //public async Task<ActionResult> OverallRiskRatings(string projectId)
        //{
        //    String user = GetEmpNo();

        //    var projectViewModel = new ProjectViewModel();
        //    var reviewVm = new ReviewVM();

        //    IEnumerable<OverallRiskRatingVM> overallRiskRating = await _ampServiceLayer.GetOverallRiskRatings(projectId, user);

        //    reviewVm.OverallRiskRatings = overallRiskRating;

        //    projectViewModel.ReviewVm = reviewVm;


        //    return PartialView("_OverallRiskRatings", projectViewModel);
        //}

        [HttpPost]
        public string PostRiskRegisterDocument(RiskDocumentVM riskDocumentVm)
        {
            String user = GetEmpNo();
            if (riskDocumentVm.ProjectID == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }
            if (ModelState.IsValid)
            {
                long intOut;
                if (!String.IsNullOrEmpty(riskDocumentVm.DocumentID) && !String.IsNullOrEmpty(riskDocumentVm.Description))
                {
                    if (riskDocumentVm.DocumentID.Length >5 && riskDocumentVm.DocumentID.Length <13 && Int64.TryParse(riskDocumentVm.DocumentID, out intOut))
                    {
                        if (_ampServiceLayer.PostRiskDocument(riskDocumentVm, user))
                        {
                            return "Saved";
                        }
                        else
                        {
                            return "Failed";
                        }
                    }
                    else
                    {
                        return "Wrong document id";
                    }
                }
                else if (riskDocumentVm.DocumentID == null)
                {
                    return "Document id cannot be empty";
                }
            
                else if (riskDocumentVm.Description == null)
                {
                    return "Description cannot be empty";
                }
                else
                {
                    return "Please complete the form.";
                }
            }
            return "Failed";
        }

        [HttpPost]
        public string RemoveRiskDocument(string documentId, string projectId)
        {
            //Get logon
            String user = GetEmpNo();
            LogCall(user, "Project/RemoveRiskDocument");
            try
            {
                //Tuple<string> result = _ampServiceLayer.DeleteRiskDocument(documentId, projectId, user);
                Tuple<bool> result = _ampServiceLayer.RemoveRiskDocument(projectId, documentId, user);
                return result.Item1.ToString();
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Remove risk doc error", user);
                return "Failed";
            }
        }
        //
        //[HttpPost]
        //public string AddOverallRiskRating(RiskRegisterVM riskRegisterVm)
        //{
        //    String user = GetEmpNo();
        //    if (ModelState.IsValid)
        //    {
        //        if (riskRegisterVm.OverallRiskRatingVm.Comments.Length < 20)
        //        {
        //            return "Comments must be at least 20 characters.";
        //        }
        //        else
        //        {
        //            bool result = _ampServiceLayer.UpdateOverallRiskRating(riskRegisterVm, user);
        //            if (result)
        //            {
        //                return "Saved";
        //            }
        //            else
        //            {
        //                return "Failed";
        //            }

        //        }

        //    }
        //    string messages = string.Join("; ", ModelState.Values
        //                                .SelectMany(x => x.Errors)
        //                                .Select(x => x.ErrorMessage));
        //    return messages;
        //}


        [HttpGet]
        public async Task<ActionResult> ExportRiskLogToExcel(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            String user = GetEmpNo();
            _ampServiceLayer.InsertLog("Generate Risk Spreadsheet", user, id);

            ExcelPackage package = await _ampServiceLayer.CreateExcelRiskLog(id, user);

            if (package != null)
            {
                string filname = "RiskLog-" + id;
                //Read the Excel file in a byte array
                Byte[] fileBytes = package.GetAsByteArray();

                //Clear the response
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Cookies.Clear();
                //Add the header & other information 
                Response.Cache.SetCacheability(HttpCacheability.Private);
                Response.CacheControl = "private";
                Response.Charset = Encoding.UTF8.WebName;
                Response.ContentEncoding = Encoding.UTF8;
                
                Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
                Response.AppendHeader("Pragma", "cache");
                Response.AppendHeader("Expires", "60");

                Response.AppendHeader("Content-Disposition", "attachment; " + "filename=\"" + filname + ".xlsx\"");
                //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filname +  "\"");

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Write it back to the client
                Response.BinaryWrite(fileBytes);
                Response.End();

            }
            else
            {
                TempData["Message"] = "No risk data to export";
            }

            return RedirectToAction("RiskRegister", new { id = id });

        }



         [HttpGet]
        public async Task<ActionResult> EditRiskItem(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/EditRiskItem";
            String Description = "Test Total Edit Risk Item Code Performance";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            user = GetEmpNo();

            Int32 riskInt;

            if(Int32.TryParse(id, out riskInt))
            {
                //Log user on page
                LogCall(user, "Project/EditRiskItem", id);
                
                RiskItemVM riskItemVm = new RiskItemVM();

                riskItemVm = await _ampServiceLayer.GetRiskItem(riskInt, user);


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                ViewData["RiskCategoryList"] = AMPUtilities.RiskCategoryList();
                ViewData["RiskLikelihoodList"] = AMPUtilities.RiskLikelihoodList();
                ViewData["RiskImpactList"] = AMPUtilities.RiskImpactList();
                ViewData["StatusList"] = AMPUtilities.StatusList();
                ViewData["RiskRatingList"] = AMPUtilities.RiskRatingList();

                return PartialView("EditorTemplates/EditRiskItem", riskItemVm);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

        }

        [HttpGet]
        public async Task<ActionResult> DisplayOverallRiskRating(string id)
        {
            //return null;
            //return PartialView("EditorTemplates/DisplayOverallRiskRating", riskItemVm); GetOverallRiskRatingItem

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/DisplayOverallRiskRating";
            String Description = "Test Total Display Risk rating Code Performance";

            //Start Timing
            From = DateTime.Now;
            //Get logon
            user = GetEmpNo();

            Int32 riskInt;

            if (Int32.TryParse(id, out riskInt))
            {
                //Log user on page
                LogCall(user, "Project/DisplayOverallRiskRating", id);

                OverallRiskRatingVM overallriskItemVm = new OverallRiskRatingVM();
                overallriskItemVm = await _ampServiceLayer.GetOverallRiskRatingItem(riskInt, user);

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                ViewData["RiskRatingList"] = AMPUtilities.RiskRatingList();

                return PartialView("EditorTemplates/DisplayOverallRiskRating", overallriskItemVm);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

        }

        [HttpGet]
        public async Task<ActionResult> DisplayRiskItem(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/DisplayRiskItem";
            String Description = "Test Total Display Risk Item Code Performance";

            //Start Timing
            From = DateTime.Now;
            //Get logon
            user = GetEmpNo();

            Int32 riskInt;

            if (Int32.TryParse(id, out riskInt))
            {
                //Log user on page
                LogCall(user, "Project/DisplayRiskItem", id);

                RiskItemVM riskItemVm = new RiskItemVM();
                riskItemVm = await _ampServiceLayer.GetRiskItem(riskInt, user);
                
                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                ViewData["RiskCategoryList"] = AMPUtilities.RiskCategoryList();
                ViewData["RiskLikelihoodList"] = AMPUtilities.RiskLikelihoodList();
                ViewData["RiskImpactList"] = AMPUtilities.RiskImpactList();
                ViewData["RiskRatingList"] = AMPUtilities.RiskRatingList();
                ViewData["StatusList"] = AMPUtilities.StatusList();

                return PartialView("EditorTemplates/EditRiskItem", riskItemVm);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

        }
        
        [HttpGet]
        public async Task<ActionResult> CreateRiskItem(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/CreateRiskItem";
            String Description = "Test Total Create Risk Item Code Performance";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            user = GetEmpNo();

            //Int32 riskInt;

            //if (Int32.TryParse(id, out riskInt))
            //{
                //Log user on page
                LogCall(user, "Project/CreateRiskItem",id);

                RiskItemVM riskItemVm = new RiskItemVM();

                riskItemVm = _ampServiceLayer.GetNewRiskItem(id,user);

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            ViewData["RiskCategoryList"] = AMPUtilities.RiskCategoryList();
            ViewData["RiskLikelihoodList"] = AMPUtilities.RiskLikelihoodList();
            ViewData["RiskImpactList"] = AMPUtilities.RiskImpactList();
            ViewData["RiskRatingList"] = AMPUtilities.RiskRatingList();
            ViewData["StatusList"] = AMPUtilities.StatusList();

            return PartialView("EditorTemplates/EditRiskItem", riskItemVm);

        }
        [HttpGet]
        public async Task<ActionResult> CreateOverallRiskRating(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/CreateRiskRating";
            String Description = "Test Total Create overall risk rating Code Performance";

            //Start Timing
            From = DateTime.Now;
            //Get logon
            user = GetEmpNo();

            LogCall(user, "Project/CreateOverallRiskRating", id);
            OverallRiskRatingVM riskRatingVm = new OverallRiskRatingVM();

            riskRatingVm = _ampServiceLayer.GetNewOverallRiskRating(id, user);

            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            ViewData["RiskRatingList"] = AMPUtilities.RiskRatingList();
            return PartialView("EditorTemplates/EditOverallRiskRating", riskRatingVm);

        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddRiskRegisterDocument(ProjectViewModel projectViewModel)
        //{
        //    //Get logon
        //    String user = GetEmpNo();
        //    //Log user on page
        //    LogCall(user, "Project/AddRiskRegisterDocument");
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            Tuple<string> result = _ampServiceLayer.AddReviewDocument(projectViewModel.ReviewVm.ReviewDocument, new ModelStateWrapper(ModelState), user);

        //            if (result.Item1 == "Failed")
        //            {
        //                var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
        //                var message = "Please fix the above: " + Environment.NewLine;
        //                message = modelStateErrors.Aggregate(message, (current, modelStateError) => current + (modelStateError.ErrorMessage + Environment.NewLine));
        //                return Json(new { success = false, response = message });
        //            }

        //            return Json(new { success = true, response = "Successful" });

        //            //return Request.AcceptTypes.Contains("application/json") ? Json("Successful") : Json("Successful", "text/plain");

        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = false, response = ex.ToString() });
        //        }
        //    }

        //    return Json(new { success = false, response = "Failed" });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDocument(ProjectViewModel projectViewModel)
        {
            //Get logon
            String user = GetEmpNo();
            //Log user on page
            LogCall(user, "Project/AddDocument");
            if (ModelState.IsValid)
            {
                try
                {
                    Tuple<string> result = _ampServiceLayer.AddReviewDocument(projectViewModel.ReviewVm.ReviewDocument, new ModelStateWrapper(ModelState), user);

                    if (result.Item1 == "Failed")
                    {
                        var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                        var message = "Please fix the above: " + Environment.NewLine;
                        message = modelStateErrors.Aggregate(message, (current, modelStateError) => current + (modelStateError.ErrorMessage + Environment.NewLine));
                        return Json(new { success = false, response = message });
                    }

                    return Json(new { success = true, response = "Successful" });

                    //return Request.AcceptTypes.Contains("application/json") ? Json("Successful") : Json("Successful", "text/plain");

                }
                catch (Exception ex)
                {
                    return Json(new { success = false, response = ex.ToString() });
                }
            }

            return Json(new { success = false, response = "Failed" });
        }


        [HttpPost]
        public ActionResult RemoveReviewDocument(int documentId)
        {
            //Get logon
            String user = GetEmpNo();
            LogCall(user, "Project/RemoveReviewDocument");
            
            try
            {
                Tuple<string> result = _ampServiceLayer.DeleteReviewDocument(documentId, user);
                return Json(new { success = true, response = "Successful message" });
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Remove ReviewOutput Score -Project/RemoveReviewOutputScore", user);
                return Json(new { success = false, response = "Failed to delete" });
            }
        }
        //Add new Review Output  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReviewOutputScoring(ProjectViewModel projectViewModel)
        {
            //Get logon
            String user = GetEmpNo();
            //Log user on page
            LogCall(user, "Project/AddReviewOutputScoring");

            string overAllBandAfterInsert = "";
            //string aggregatedRisk = "";
            string projectScore = "";

            if (ModelState.IsValid)
            {
                try
                {
                    Tuple<string, string> result = _ampServiceLayer.CreateReviewOutput(projectViewModel.ReviewVm.ReviewOutputVm, user, new ModelStateWrapper(ModelState));
                    overAllBandAfterInsert = result.Item1;
                    //aggregatedRisk = result.Item;
                    projectScore = result.Item2;

                    if (result.Item1 == "false")
                    {
                        var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                        var message = "Please fix the above: " + Environment.NewLine;
                        message = modelStateErrors.Aggregate(message, (current, modelStateError) => current + (modelStateError.ErrorMessage + Environment.NewLine));
                        return Json(new { success = false, response = message });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, response = ex.ToString() });
                }
            }
            else
            {
                var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                var message = "Please fix the above: " + Environment.NewLine;
                message = modelStateErrors.Aggregate(message, (current, modelStateError) => current + (modelStateError.ErrorMessage + Environment.NewLine));
                return Json(new { success = false, response = message });
            }

            return Json(new { success = true, response = "Successful message", OverallScore = overAllBandAfterInsert, ProjScore = projectScore });
        }


        [HttpPost]
        public ActionResult AddReviewOverallRisk(string projectId, string reviewId, string overallRisk)
        {
            //Get logon
            String user = GetEmpNo();
            //Log user on page
            LogCall(user, "Project/AddReviewOverallRisk");
            
            string aggregatedRisk = "";
            aggregatedRisk = _ampServiceLayer.InsertOverallRisk(user,projectId, Convert.ToInt32(reviewId),overallRisk);
            


            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        Tuple<string, string, string> result = _ampServiceLayer.CreateReviewOutput(projectViewModel.ReviewVm.ReviewOutputVm, user, new ModelStateWrapper(ModelState));
            //        overAllBandAfterInsert = result.Item1;
            //        aggregatedRisk = result.Item2;
            //        projectScore = result.Item3;

            //        if (result.Item1 == "false")
            //        {
            //            var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
            //            var message = "Please fix the above: " + Environment.NewLine;
            //            message = modelStateErrors.Aggregate(message, (current, modelStateError) => current + (modelStateError.ErrorMessage + Environment.NewLine));
            //            return Json(new { success = false, response = message });
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new { success = false, response = ex.ToString() });
            //    }
            //}
            //else
            //{
            //    var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
            //    var message = "Please fix the above: " + Environment.NewLine;
            //    message = modelStateErrors.Aggregate(message, (current, modelStateError) => current + (modelStateError.ErrorMessage + Environment.NewLine));
            //    return Json(new { success = false, response = message });
            //}

            return Json(new { success = true, response = "Successful", Risk = aggregatedRisk });
        }



        // Edit Review Output  
        [HttpPost]
        public ActionResult EditReviewOutputScore(string projectId, int reviewId, int outputId, string description, string weight, string outputScore, string risk)
        {
            //Get logon
            String user = GetEmpNo();
            LogCall(user, "Project/EditReviewOutputScore");
            string overAllBandAfterUpdate = "";
            string overAllRiskAfterUpdate = "";
            string ProjectScoreAfterUpdate = "";
            int? TotalWeight;

            try
            {
                int res;
                bool IsOk = Int32.TryParse(weight, out res);
                if (IsOk)
                {
                    ReviewOutput reviewOutput = new ReviewOutput
                    {
                        ProjectID = projectId,
                        ReviewID = reviewId,
                        OutputID = outputId,
                        OutputDescription = description,
                        Weight = Convert.ToInt16(weight),
                        OutputScore = outputScore,
                        Risk = risk
                    };

                    Tuple<string, string, string, int?> result = _ampServiceLayer.EditReviewOutput(reviewOutput, user);

                    if (result.Item4 <= 100)
                    {
                        overAllBandAfterUpdate = result.Item1;
                        overAllRiskAfterUpdate = result.Item2;
                        ProjectScoreAfterUpdate = result.Item3;
                        TotalWeight = result.Item4;
                        if (TotalWeight <= 100)
                        {
                            return Json(new { success = true, response = "Successful", OverallScore = overAllBandAfterUpdate, Risk = overAllRiskAfterUpdate, ProjScore = ProjectScoreAfterUpdate }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = true, response = "UnSuccessfulM", OverallScore = overAllBandAfterUpdate, Risk = overAllRiskAfterUpdate, ProjScore = ProjectScoreAfterUpdate }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = true, response = "UnSuccessfulM", OverallScore = overAllBandAfterUpdate, Risk = overAllRiskAfterUpdate, ProjScore = ProjectScoreAfterUpdate }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = true, response = "UnSuccessfulP", OverallScore = overAllBandAfterUpdate, Risk = overAllRiskAfterUpdate, ProjScore = ProjectScoreAfterUpdate }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Update review output score -Project/EditReviewOutputScore", user);
                return Json(new { success = false, response = "Failed to edit" });
            }
        }
        // Remove new Review Output  
        [HttpPost]
        public ActionResult RemoveReviewOutputScore(string projectId, int reviewId, int outputId)
        {
            //Get logon
            String user = GetEmpNo();
            string overAllBandAfterDelete = "";
          
            string projectScoreAfterDelete = "";

            LogCall(user, "Project/RemoveReviewOutputScore");
            try
            {
                Tuple<string, string> result = _ampServiceLayer.RemoveReviewOutput(projectId, reviewId, outputId);
                overAllBandAfterDelete = result.Item1;
                projectScoreAfterDelete = result.Item2;
                string risk = "-";
                return Json(new { success = true, response = "Successful message", OverallScore = overAllBandAfterDelete,Risk = risk, ProjScore = projectScoreAfterDelete });
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Remove ReviewOutput Score -Project/RemoveReviewOutputScore", user);
                return Json(new { success = false, response = "Failed to delete" });
            }
        }
        //Add new AR/PCR quest documents
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddDocument(ProjectViewModel projectViewModel)
        //{
        //    //Get logon
        //    String user = GetEmpNo();
        //    //Log user on page
        //    LogCall(user, "Project/AddDocument");
        //    if (ModelState.IsValid)
        //    {
        //        //try
        //        //{
        //        //    //Tuple<string> result = _ampServiceLayer.CreateReviewOutput(projectViewModel.ReviewVm.ReviewOutputVm, user, new ModelStateWrapper(ModelState));


        //        //    if (result.Item1 == "false")
        //        //    {
        //        //        var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
        //        //        var message = "Please fix the above: " + Environment.NewLine;
        //        //        message = modelStateErrors.Aggregate(message, (current, modelStateError) => current + (modelStateError.ErrorMessage + Environment.NewLine));
        //        //        return Json(new { success = false, response = message });
        //        //    }
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    return Json(new { success = false, response = ex.ToString() });
        //        //}
        //    }
        //}


        // Project/Components/id 
        public async Task<ActionResult> Components(string id)
        {
            //Get logon
            String user = GetEmpNo();
            try
            {


                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Project/Components";
                String Description = "Test Total Project Components Code Performance";

                //Start Timing
                From = DateTime.Now;



                //Log user on page
                LogCall(user, "Components", id);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ProjectComponentVM projectComponentVm = await _ampServiceLayer.GetComponents(id, user);

                if (projectComponentVm == null)
                {
                    return HttpNotFound();
                }

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["Success"].ToString() == "1")
                {
                    ViewBag.Success = "1";
                }
                if (TempData["Success"].ToString() == "0")
                {
                    ViewBag.Success = "0";
                }


                return View("Components", projectComponentVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Project/Finance", user);
                throw;
            }
        }
        // Project GeoCoding/id
        public ActionResult About()
        {
            //Get logon
            String user = GetEmpNo();

            //Log user on page
            LogCall(user, "About");

            // change this to get GeoCoding Data!
            //   ProjectViewModel projectViewModel = await serviceLayer.GetProjectFinancials(id);
            
            return View("About");
        }

        // Project GeoCoding/id
        public ActionResult HowTo()
        {
            //Get logon
            String user = GetEmpNo();

            //Log user on page
            LogCall(user, "HowTo");

            // change this to get GeoCoding Data!
            //   ProjectViewModel projectViewModel = await serviceLayer.GetProjectFinancials(id);

            return View("HowTo");
        }
        public ActionResult PQCalculationGuideline()
        {
            //Get logon
            String user = GetEmpNo();

            //Log user on page
            LogCall(user, "PQCalculationGuideline");

            // change this to get GeoCoding Data!
            //   ProjectViewModel projectViewModel = await serviceLayer.GetProjectFinancials(id);





            return View("PQCalculationGuideline");
        }

        public ActionResult Help()
        {
            //Get logon
            String user = GetEmpNo();

            //Log user on page
            LogCall(user, "Help");
            return View("Help");
        }

        // Project/AdvanceSeach
        #region Advance_Search

        public async Task<ActionResult> AdvancedSearch(int? page, AdvanceSearchVM advanceSearchVM, string IsPagingEnabled)
        {
            //Get logon
            String user = GetEmpNo();
            //Test People service 
            //IEnumerable<PersonDetails> SROs = await _ampServiceLayer.LookUpSroUser() as IEnumerable<PersonDetails>;
            //Log user on page
            LogCall(user, "Project/AdvancedSearch");
            
            try
            {
                
                int pageNumber = (page ?? 1);
                if (IsPagingEnabled == null)
                {
                    IsPagingEnabled = "T";
                }
                if (Session["isPagingEnabledAS"] == null)
                {
                    Session["isPagingEnabledAS"] = "T";
                }
                if (Session["isPagingEnabled"] == "F")
                    ViewBag.PagingOn = "F";

                string searchKeyword=string.Empty;
                if (!string.IsNullOrEmpty(advanceSearchVM.SearchKeyWord) || !string.IsNullOrWhiteSpace(advanceSearchVM.SearchKeyWord))
                {
                    searchKeyword = advanceSearchVM.SearchKeyWord.ToString().Trim();
                }
                else
                {
                    searchKeyword = null;
                }

                string stage = advanceSearchVM.stage;
                string BenefittingCountry = advanceSearchVM.BenefittingCountryID;
                string BudgetCentre = advanceSearchVM.BudgetCentreID;
                string SRO = advanceSearchVM.SRO;

                string stageChoice = "All";   //advanceSearchVM.StatusChoice; //Status filter removed
                if (stageChoice == null)
                {
                    advanceSearchVM.StatusChoice = "All";
                }

                advanceSearchVM.ProjectStages = _ampServiceLayer.GetProjectStages();
                advanceSearchVM.BenefitingCountry = _ampServiceLayer.GetBenefitingCountry();
            //    advanceSearchVM.BudgetCentre = _ampServiceLayer.LookupBudgetCentreKV();

                if (advanceSearchVM == null)
                {
                    return HttpNotFound();
                }

              
                AdvanceSearchVM advanceSearch;

                if (page == null && TempData["Adv"] == null) //Loading first page and Paging enabled 
                {

                    advanceSearch = await _ampServiceLayer.GetProjectsAdvanceSearch(searchKeyword, stage, 1, 10, stageChoice, BenefittingCountry, user, BudgetCentre, SRO, Session["isPagingEnabledAS"].ToString());

                    advanceSearch.ProjectStages = advanceSearchVM.ProjectStages;
                    advanceSearch.BenefitingCountry = advanceSearchVM.BenefitingCountry;
             //       advanceSearch.BudgetCentre = advanceSearchVM.BudgetCentre;

                    advanceSearch.SearchKeyWord = searchKeyword;
                    advanceSearch.stage = stage;
                    advanceSearch.BenefittingCountryID = BenefittingCountry;
                    advanceSearch.BudgetCentreID = BudgetCentre;
                    advanceSearch.SRO = SRO;

                    advanceSearch.StatusChoice = stageChoice;

                    Session["searchKeyword"] = searchKeyword;
                    Session["stage"] = stage;
                    Session["stageChoice"] = stageChoice; //at all stage A or C
                    Session["BenefittingCountry"] = BenefittingCountry;
                    Session["BudgetCentre"] = BudgetCentre;
                    Session["SRO"] = SRO;

                  

                }
                else if (page != null && TempData["Adv"] == null)
                {
             
                    if (Session["searchKeyword"] != null) { searchKeyword = Session["searchKeyword"].ToString().Trim(); }
                    else { searchKeyword = null; }
                    if (Session["stage"] != null) { stage = Session["stage"].ToString(); }
                    else { stage = null; }
                    if (Session["stageChoice"] != null) { stageChoice = Session["stageChoice"].ToString(); }
                    else { stageChoice = "All"; }
                    if (Session["BenefittingCountry"] != null) { BenefittingCountry = Session["BenefittingCountry"].ToString(); }
                    else { BenefittingCountry = null; }

                    if (Session["BudgetCentre"] != null) { BudgetCentre = Session["BudgetCentre"].ToString(); }
                    else { BudgetCentre = null; }

                    if (Session["SRO"] != null) { SRO = Session["SRO"].ToString(); }
                    else { SRO = null; }


                    advanceSearch = await _ampServiceLayer.GetProjectsAdvanceSearch(searchKeyword, stage, pageNumber, 10, stageChoice, BenefittingCountry, user, BudgetCentre, SRO, Session["isPagingEnabledAS"].ToString());

                    advanceSearch.ProjectStages = advanceSearchVM.ProjectStages;
                    advanceSearch.BenefitingCountry = advanceSearchVM.BenefitingCountry;
                  //  advanceSearch.BudgetCentre = advanceSearchVM.BudgetCentre;

                    advanceSearch.SearchKeyWord = searchKeyword;
                    advanceSearch.stage = stage;
                    advanceSearch.BenefittingCountryID = BenefittingCountry;
                    advanceSearch.BudgetCentreID = BudgetCentre;
                    advanceSearch.SRO = SRO;

                    advanceSearch.StatusChoice = stageChoice;

                   
                }

                else if (TempData["Adv"] != null)
                {
                    advanceSearch = TempData["Adv"] as AdvanceSearchVM;
                    string SK = string.Empty;
                    string PS = string.Empty;
                    string BC = string.Empty;
                    string BdId = string.Empty;

                    if (!string.IsNullOrEmpty(advanceSearch.SearchKeyWord)){
                        Session["SK"] = advanceSearch.SearchKeyWord.ToString().Trim();}
                    else { Session["SK"] = string.Empty; }
                    if (!string.IsNullOrEmpty(advanceSearch.stage)) {
                        Session["PS"] = advanceSearch.stage.ToString().Trim(); }
                    else{ Session["PS"] = string.Empty;}
                    if (!string.IsNullOrEmpty(advanceSearch.BenefittingCountryID)) {
                        Session["BC"] = advanceSearch.BenefittingCountryID.ToString().Trim();}
                    else { Session["BC"] = string.Empty;}
                    if (!string.IsNullOrEmpty(advanceSearch.BudgetCentreID)){
                        Session["BdId"] = advanceSearch.BudgetCentreID.ToString().Trim();}
                    else { Session["BdId"] = string.Empty; }
                    if (!string.IsNullOrEmpty(advanceSearch.SRO)) {
                        Session["Sro"] = advanceSearch.SRO.ToString().Trim(); }
                    else { Session["Sro"] = string.Empty; }

                    advanceSearch = await _ampServiceLayer.GetProjectsAdvanceSearch(Session["SK"].ToString(), Session["PS"].ToString(), pageNumber, 10, stageChoice, Session["BC"].ToString(), user, Session["BdId"].ToString(), Session["Sro"].ToString(), Session["isPagingEnabledAS"].ToString());

                    advanceSearchVM.ProjectStages = _ampServiceLayer.GetProjectStages();
                    advanceSearchVM.BenefitingCountry = _ampServiceLayer.GetBenefitingCountry();
                 

                    advanceSearch.ProjectStages = advanceSearchVM.ProjectStages;
                    advanceSearch.BenefitingCountry = advanceSearchVM.BenefitingCountry;
                 //   advanceSearch.BudgetCentre = advanceSearchVM.BudgetCentre;

                    advanceSearch.SearchKeyWord = Session["SK"].ToString() ?? null;
                    advanceSearch.stage = Session["PS"].ToString() ?? null;
                    advanceSearch.BenefittingCountryID = Session["BC"].ToString() ?? null;
                    advanceSearch.BudgetCentreID = Session["BdId"].ToString() ?? null;
                    advanceSearch.SRO = Session["Sro"].ToString() ?? null;

                    advanceSearch.StatusChoice = stageChoice;
          

                }
                
                //else if (IsPagingEnabled == "F")
                //{
                //    //if (Session["searchKeyword"] != null) { searchKeyword = Session["searchKeyword"].ToString().Trim(); }
                //    //else { searchKeyword = null; }
                //    //if (Session["stage"] != null) { stage = Session["stage"].ToString(); }
                //    //else { stage = null; }
                //    //if (Session["stageChoice"] != null) { stageChoice = Session["stageChoice"].ToString(); }
                //    //else { stageChoice = "Active"; }
                //    //if (Session["BenefittingCountry"] != null) { BenefittingCountry = Session["BenefittingCountry"].ToString(); }
                //    //else { BenefittingCountry = null; }

                //    if (searchKeyword == null)
                //    {
                //        if (Session["searchKeyword"] != null)
                //        {
                //            searchKeyword = Session["searchKeyword"].ToString();
                //        }
                //    }
                //    if (stage == null)
                //    {
                //        if (Session["stage"] != null)
                //        {
                //            stage = Session["stage"].ToString();
                //        }
                //    }
                //    if (BenefittingCountry == null)
                //    {
                //        if (Session["BenefittingCountry"] != null)
                //        {
                //            BenefittingCountry = Session["BenefittingCountry"].ToString();
                //        }
                //    }

                //    advanceSearch = await _ampServiceLayer.GetProjectsAdvanceSearch(searchKeyword, stage, pageNumber, 10, stageChoice, BenefittingCountry, user);

                //    advanceSearch.ProjectStages = advanceSearchVM.ProjectStages;
                //    advanceSearch.BenefitingCountry = advanceSearchVM.BenefitingCountry;
                //    advanceSearch.SearchKeyWord = searchKeyword;
                //    advanceSearch.stage = stage;
                //    advanceSearch.BenefittingCountryID = BenefittingCountry;
                //    advanceSearch.StatusChoice = stageChoice;
                //}
               
                else
                {

                    Session.Remove("searchKeyword");
                    Session.Remove("stage");
                    Session.Remove("BenefittingCountry");
                    Session.Remove("BudgetCentre");
                    Session.Remove("SRO");
                    Session.Remove("isPagingEnabled");
                    advanceSearch = advanceSearchVM;
                }

                return View(advanceSearch);
               
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Project/AdvancedSearch", user);
                throw;
            }
        }

        [HttpPost]
        public ActionResult SetPagingPreferenceAdvSearch(string pagingEnabled, string searchKey, string projStage, string benefittingCountry, string budgetCentreId, string sro)
        {
            if (pagingEnabled == "F")
            {
                Session["isPagingEnabledAS"] = "F";
            }
            else
            {
                Session["isPagingEnabledAS"] = "T";
            }
            AdvanceSearchVM avm = new AdvanceSearchVM();
            avm.SearchKeyWord = searchKey;
            avm.stage = projStage;
            avm.BenefittingCountryID = benefittingCountry;
            avm.BudgetCentreID = budgetCentreId;
            avm.SRO = sro;
        
            TempData["Adv"] = avm;
            //return RedirectToAction("AdvancedSearch", "Project", new { page = 1, IsPagingEnabled = Session["isPagingEnabledAS"].ToString() });
            //return View("AdvancedSearch");

            TempData.Keep("Adv");
            return Json(Session["isPagingEnabledAS"].ToString());
        }


        [HttpPost]
        public ActionResult ClearAdvancedSearch()
        {

            Session.Remove("searchKeyword");
            Session.Remove("stage");
            Session.Remove("BenefittingCountry");
            Session.Remove("isPagingEnabledAS");
            return Json("success");

        }


        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdvancedSearch(AdvanceSearchVM advanceSearchVM)
        {
            //Get logon
            String user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/AdvancedSearch");

            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/AdvancedSearch";
            String Description = "advanced search";

            //Start Timing
            From = DateTime.Now;

            if (ModelState.ContainsKey("ProjectStages"))
                ModelState["ProjectStages"].Errors.Clear();
            if (ModelState.ContainsKey("SearchKeyWord"))
                ModelState["SearchKeyWord"].Errors.Clear();
            if (ModelState.ContainsKey("stage"))
                ModelState["stage"].Errors.Clear();
            
            
            AdvanceSearchVM advanceSearch;

            if (ModelState.IsValid)
            {
                advanceSearch = await _ampServiceLayer.GetProjectsAdvanceSearch(advanceSearchVM.SearchKeyWord, advanceSearchVM.stage,1, 10); //Default pageNumber, pageSize
                if (advanceSearchVM.SearchKeyWord != null)
                {
                    advanceSearch.SearchKeyWord = advanceSearchVM.SearchKeyWord.ToString().Trim();
                    Session["KeyWord"] = advanceSearch.SearchKeyWord.ToString();
                }
                if (advanceSearchVM.stage != null)
                {
                    advanceSearch.stage = advanceSearchVM.stage.ToString();
                    Session["Stage"] = advanceSearch.stage.ToString();
                }
                TempData["VM"] = advanceSearch;
                TempData["Success"] = "1";

                

                //advanceSearch = TempData["VM"] as AdvanceSearchVM;
                
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);
                //return RedirectToAction("AdvanceSearch", "Project");
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return View(advanceSearchVM);
            }
           
        }
        */

        #endregion  
        // Project/Create
        public async Task<ActionResult> Create()
        {
            //Get logon
            String user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Create");

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Project/Create";
                String Description = "Test Total Create Project GET Code Performance";

                //Start Timing
                From = DateTime.Now;

                ProjectVM ProjectVM = new ProjectVM();
                ProjectDateVM ProjectDate = new ProjectDateVM();

                ProjectVM.ProjectDates = ProjectDate;
                ProjectVM.BudgetCentreDescription = "";

                ProjectVM.RiskLookups = _ampServiceLayer.LookupRisksTypes();

                //This approach is good for drop downs (Replaced with Bloodhound)
                //ViewBag.BudgetCentreID = new SelectList(componentdetailsviewmodel.BudgetCentre, "BudgetCentreID","BudgetCentreDescription");

                if (ProjectVM == null)
                {
                    return HttpNotFound();
                }

                _ampServiceLayer.InsertLog("Project/CreateGET", user);

                // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
                if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

                //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
                if (TempData["Success"].ToString() == "1")
                {
                    ViewBag.Success = "1";
                }
                if (TempData["Success"].ToString() == "0")
                {
                    ViewBag.Success = "0";
                }

                //Manage state

                if (TempData["Success"] as string != "NA")
                {
                ProjectVM = TempData["VM"] as ProjectVM;
                TempData["VM"] = ProjectVM;
                }
                else
                {
                    TempData["VM"] = ProjectVM;
                }
                //Pass user in the view bag for photo of you as inputter.
                ViewBag.User = user;

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                return View(ProjectVM);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Project/CreateGET", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectVM projectVM)
        {
            //Get logon
            String user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/CreatePost");

            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/CreatePOST";
            String Description = "Test Total Project create post Code Performance";

            //Start Timing
            From = DateTime.Now;

            string createFolderResult = "";

            if (this.ModelState.IsValid)
            {
                Tuple<bool,string> createProjecTuple = await _ampServiceLayer.CreateProject(projectVM, new ModelStateWrapper(this.ModelState), user);

                if (createProjecTuple.Item1 == true)
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    TempData["CreateFolder"] = createProjecTuple.Item2;
                    ViewBag.CreateFolder = createProjecTuple.Item2;

                    //End Timing and Record
                    To = DateTime.Now;
                    TimeSpan successResult = To - From;
                    _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, successResult, user);

                    return RedirectToAction("Edit", "Project", new { id = projectVM.ProjectID });
                }
            }

            //Model was invalid set success code to 0
            TempData["Success"] = "0";
            ViewBag.Success = "0";

            TempData["CreateFolder"] = "";
            ViewBag.CreateFolder = "";

            //var OriginalVM = TempData["VM"] as ProjectVM;
            TempData["VM"] = projectVM;
            //Pass user in the view bag for photo of you as inputter.
            ViewBag.User = user;

            //End Timing and Record
            To = DateTime.Now;
            TimeSpan failResult = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, failResult, user);

            projectVM.RiskLookups = _ampServiceLayer.LookupRisksTypes();

            return View(projectVM);
        }


        // GET: /Project/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/Edit";
            String Description = "Test Total Project Edit Code Performance";

            //Start Timing
            From = DateTime.Now;
            

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Edit", id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectVM projectVm = await _ampServiceLayer.GetProjectVM(id, user);

            if (projectVm == null)
            {
                return HttpNotFound();
            }

            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["Success"] == null)
            {
                TempData["Success"] = "NA";
            }

            //-------------Set Temp Data and ViewBag resulting from SubmitProjectForClosure (POST)----------
            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["ProjectSentForClosure"] == null) { TempData["ProjectSentForClosure"] = "NA"; ViewBag.ProjectSentForClosure = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["ProjectSentForClosure"].ToString() == "1")
            {
                ViewBag.ProjectSentForClosure = "1";
            }
            if (TempData["ProjectSentForClosure"].ToString() == "0")
            {
                ViewBag.ProjectSentForClosure = "0";
            }

                        //-------------Set Temp Data and ViewBag resulting from AuthoriseProjectClose (POST)----------
            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["CPAuthSuccess"] == null) { TempData["CPAuthSuccess"] = "NA"; ViewBag.CPAuthSuccess = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["CPAuthSuccess"].ToString() == "1")
            {
                ViewBag.CPAuthSuccess = "1";
            }
            if (TempData["CPAuthSuccess"].ToString() == "0")
            {
                ViewBag.CPAuthSuccess = "0";
            }

            //-------------Set Temp Data and ViewBag resulting from SubmitProjectForApproval (POST)----------
            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["ProjectSentForApproval"] == null) { TempData["ProjectSentForApproval"] = "NA"; ViewBag.ProjectSentForApproval = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["ProjectSentForApproval"].ToString() == "1")
            {
                ViewBag.ProjectSentForApproval = "1";
            }
            if (TempData["ProjectSentForApproval"].ToString() == "0")
            {
                ViewBag.ProjectSentForApproval = "0";
            }


            //Put the ViewModel into TempData so that it can be accessed if validation fails on the POST method.
            TempData["VM"] = projectVm;

            


            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            return View(projectVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProjectVM projectViewModel, string submitButton)
        {
            projectViewModel.RiskLookups = _ampServiceLayer.LookupRisksTypes();
            //Multiple submit buttons Exist. Work out which is incoming and send to the appropriate controller.

            switch (submitButton)
            {
                case "1": //Close Project
                    return (await PreValidateWorkflow(projectViewModel, Constants.CloseProjectTaskId));
                case "2": //Re-Approve Project
                    return (await PreValidateWorkflow(projectViewModel, Constants.ReApproveProjectTaskId));
                case "7": //Approve A&D
                    return (await PreValidateWorkflow(projectViewModel, Constants.ApproveAD));
                case "8": //Fast track
                    return (await PreValidateWorkflow(projectViewModel, Constants.FastTrack));
                case "9": //Archive
                    return (await PreValidateWorkflow(projectViewModel, Constants.ArchiveProject));
                case "10": //Re-open Project
                    return (await PreValidateWorkflow(projectViewModel, Constants.ReOpenProject));
                case "11": //Approve Project
                    return (await PreValidateWorkflow(projectViewModel, Constants.ApproveProjectTask));

                case "Save Project":
                    return (SaveProject(projectViewModel));
                default:
                    return (SaveProject(projectViewModel));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> PreValidateWorkflow(ProjectVM projectViewModel, Int32 taskId)
        {
            //Get logon
            string user = GetEmpNo();

            if (await _ampServiceLayer.PreValidateWorkflowApproval(projectViewModel.ProjectID, taskId, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                return RedirectToAction("Edit", "Workflow", new { id1 = projectViewModel.ProjectID, id2 = taskId });
            }
            else
            {
                TempData["Success"] = "0";
                var OriginalVM = TempData["VM"] as ProjectVM;
                TempData["VM"] = OriginalVM;

                //Bit of state management. Set the CCOValue IEnumerables from the ViewModel stored in TempData.
                projectViewModel.ProjectHeader = OriginalVM.ProjectHeader;
                projectViewModel.WFCheck = OriginalVM.WFCheck;
                projectViewModel.CanSendForApproval = OriginalVM.CanSendForApproval;
                projectViewModel.CanSendForClosure = OriginalVM.CanSendForClosure;
                projectViewModel.IsProjectTeam = OriginalVM.IsProjectTeam;
                projectViewModel.RiskAtApproval = OriginalVM.RiskAtApproval;
                projectViewModel.ProjectSRO = OriginalVM.ProjectSRO;
                projectViewModel.ProjectDates = OriginalVM.ProjectDates;
                projectViewModel.Title = OriginalVM.Title;
                projectViewModel.Description = OriginalVM.Description;
                projectViewModel.BudgetCentreID = OriginalVM.BudgetCentreID;
                projectViewModel.BudgetCentreDescription = OriginalVM.BudgetCentreDescription;

               
                return View("Action", projectViewModel);
            }
        }



        // POST: /Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "Id,ProjectID,ProjectDescription,InputterID,ESNAdvisorID,BudgetCentreID,Status,Stage,proj_value,ProjectSuspended,WorkflowStatus,Purpose,PurposeRevised,Purpose_ChangeReason,OVIS,TeamMarker,RiskAtApproval,SpecificConditions,GenderEquality,HIVAIDS,LastUpdate,UserID")]
        //[Bind(Include = "ProjectDetailsViewModel")]
        public  ActionResult SaveProject(ProjectVM projectViewModel)
        {
                //Get logon
                string user = GetEmpNo();
                    if (_ampServiceLayer.UpdateProject(projectViewModel, new ModelStateWrapper(this.ModelState), user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    return RedirectToAction("Edit");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";

                    var OriginalVM = TempData["VM"] as ProjectVM;
                    TempData["VM"] = OriginalVM;

                    //ProjectHeaderVM headerVm = new ProjectHeaderVM();

                    //headerVm.ProjectID = projectViewModel.ProjectID;
                    //headerVm.Stage = projectViewModel.Stage;
                    //headerVm.Title = projectViewModel.Title;
                    //headerVm.StageDescription = projectViewModel.StageDescription;
                    //headerVm.BudgetCentre = projectViewModel.BudgetCentreID;



                    projectViewModel.ProjectHeader = OriginalVM.ProjectHeader;
                    projectViewModel.WFCheck = OriginalVM.WFCheck;
                    projectViewModel.CanSendForApproval = OriginalVM.CanSendForApproval;
                    projectViewModel.CanSendForClosure = OriginalVM.CanSendForClosure;
                    projectViewModel.IsProjectTeam = OriginalVM.IsProjectTeam;
                    projectViewModel.RiskAtApproval = OriginalVM.RiskAtApproval;
                    projectViewModel.ProjectSRO = OriginalVM.ProjectSRO;


                    return View("Edit", projectViewModel);
                }
            
        }

        // GET: /Project/Markers/id
        public async Task<ActionResult> Markers(string id)
        {
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Project/Markers";
            String Description = "Test Total Project Markers Code Performance";

            //Start Timing
            From = DateTime.Now;


            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Markers", id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMarkersVM projectMarkerVm = await _ampServiceLayer.GetProjectMarkers(id, user);

            if (projectMarkerVm == null)
            {
                return HttpNotFound();
            }

            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["Success"].ToString() == "1")
            {
                ViewBag.Success = "1";
            }
            if (TempData["Success"].ToString() == "0")
            {
                ViewBag.Success = "0";
            }
            TempData["VM"] = projectMarkerVm;

            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            return View(projectMarkerVm);
        }

        [HttpPost]
        public ActionResult Markers(ProjectMarkersVM projectMarkersVm)
        {
            //Get logon
            string user = GetEmpNo();
          if (_ampServiceLayer.UpdateProjectMarkers(projectMarkersVm, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                return RedirectToAction("Markers");
            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";

                var OriginalVM = TempData["VM"] as ProjectMarkersVM;
                TempData["VM"] = OriginalVM;

                //State management for returning original view model data.
                projectMarkersVm.BudgetCentreID = OriginalVM.BudgetCentreID;
                projectMarkersVm.GenderCCO = OriginalVM.GenderCCO;
                projectMarkersVm.BiodiversityCCO = OriginalVM.BiodiversityCCO;
                projectMarkersVm.MitigationCCO = OriginalVM.MitigationCCO;
                projectMarkersVm.AdaptationCCO = OriginalVM.AdaptationCCO;
                projectMarkersVm.DesertificationCCO = OriginalVM.DesertificationCCO;
                projectMarkersVm.HIVCCO = OriginalVM.HIVCCO;
                projectMarkersVm.DisabilityCCO = OriginalVM.DisabilityCCO;
                projectMarkersVm.DisabilityPercentage = OriginalVM.DisabilityPercentage;
                return View("Markers", projectMarkersVm);
            }

         
        }

        [HttpPost]
        public ActionResult ARPCRBasicEdit(ProjectReviewVM newProjectVM, string submitButton)
        {
            //Get logon
            string user = GetEmpNo();

            if (_ampServiceLayer.UpdateARPCRBasicInfo(newProjectVM.Performance, user))
            {
                TempData["Success"] = "1";
            }
            else
            {
                TempData["Success"] = "0";
            }
            return Redirect(Request.UrlReferrer.ToString());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReviewAuthorisation(ProjectReviewVM newProjectReviewVM)
        {

            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            //Log user on page
            LogCall(user, "Project/ReviewAuthorisation");

            if (newProjectReviewVM.ProjectReviews != null)
            {
                newProjectReviewVM.ProjectReviews[0].ReviewType = "Annual Review";

                if (await _ampServiceLayer.UpdateReviewStage(newProjectReviewVM.ProjectReviews[0], user, reviewsURL))
                {
                    TempData["ARSuccessAuth"] = "1";
                }
                else
                {
                    TempData["ARSuccessAuth"] = "0";
                }

            }
            return Redirect(reviewsURL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitReviewForApproval(ProjectReviewVM newProjectReviewVM)
        {
            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            //Log user on page
            LogCall(user, "Project/ReviewAuthorisation");


            if (newProjectReviewVM.ProjectReviews != null)
            {
                //set the stage as this function can be triggered only by clickign submit for authorisation
                newProjectReviewVM.ProjectReviews[0].StageID = "1"; // Set to 'Awaiting Approval'
                newProjectReviewVM.ProjectReviews[0].ReviewType = "Annual Review";
                
                if (await _ampServiceLayer.UpdateReviewStage(newProjectReviewVM.ProjectReviews[0], user, reviewsURL))
                {
                    TempData["ARSuccessSubmission"] = "1";
                }
                else
                {
                    TempData["ARSuccessSubmission"] = "0";
                }

            }
            return Redirect(reviewsURL);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitPCRForApproval(ProjectReviewVM newProjectReviewVM)
        {
            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            //Log user on page
            LogCall(user, "Project/PCRAuthorisation");

            ReviewVM pcrReview = new ReviewVM();
            pcrReview.ProjectID = newProjectReviewVM.ProjectPcrScore.ProjectID;
            pcrReview.ReviewID = newProjectReviewVM.ProjectPcrScore.ReviewID;
            pcrReview.Approver = newProjectReviewVM.ProjectPcrScore.Approver;
            pcrReview.SubmissionComment = newProjectReviewVM.ProjectPcrScore.SubmissionComment;
            pcrReview.ReviewType = "Project Completion Review";
            //set the stage as this function can be triggered only by clickign submit for authorisation
            pcrReview.StageID = "1"; // Set to 'Awaiting Approval'
                                     // check that the project has an inputter

            /// *** could change this her eto just return true or false rather than a validation method message and 
            /// instead pass it to the TempData as a new paramwnter on erro message 
            /// and handle in the javascript
            /// 
           
            if (await _ampServiceLayer.ValidateCheckProjectHasInputter(pcrReview.ProjectID) == false)
            // need somehow to pass the validation message up to the voew
            {


               TempData["PCRSuccessSubmission"] = "2"; // have something specific to not having a project inputter
                return Redirect(reviewsURL);

            }



            if (await _ampServiceLayer.UpdateReviewStage(pcrReview, user, reviewsURL))
            {
                TempData["PCRSuccessSubmission"] = "1";
            }
            else
            {
                TempData["PCRSuccessSubmission"] = "0";
            }

            return Redirect(reviewsURL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeReviewApprover(string projectID, int reviewID, string newApproverID)
        {
            //Get logon
            string user = GetEmpNo();
            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            if (await _ampServiceLayer.ChangeReviewApprover(projectID, reviewID, newApproverID, user,  reviewsURL))
            {
                TempData["ARSuccessSubmission"] = "1";
            }
            else
            {
                TempData["ARSuccessSubmission"] = "0";
            }
            return Redirect(reviewsURL);
        }
       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthorisePCR(ProjectReviewVM newProjectReviewVM)
        {

            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            //Log user on page
            LogCall(user, "Project/PCRAuthorisation");

            ReviewVM pcrReview = new ReviewVM();
            pcrReview.ProjectID = newProjectReviewVM.ProjectPcrScore.ProjectID;
            pcrReview.ReviewID = newProjectReviewVM.ProjectPcrScore.ReviewID;
            pcrReview.ApproveComment = newProjectReviewVM.ProjectPcrScore.ApproveComment;
            pcrReview.IsApproved = newProjectReviewVM.ProjectPcrScore.IsApproved;
            pcrReview.ReviewType = "Project Completion Review";

            if (await _ampServiceLayer.UpdateReviewStage(pcrReview, user, reviewsURL))
            {
                TempData["PCRSuccessAuth"] = "1";
            }
            else
            {
                TempData["PCRSuccessAuth"] = "0";
            }
            return Redirect(reviewsURL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReviewCreate(ProjectReviewVM projectReviewVm)
        {
            //Get logon
            String user = GetEmpNo();

            string result;

            //Log user on page
            LogCall(user, "Project/ReviewCreate");


            if (ModelState.IsValid)
            {
                result = _ampServiceLayer.CreateReview(projectReviewVm, user);
                //return Json(new { success = true, response = "Successful message" });
                if (result == "Success")
                {
                    return RedirectToAction("Reviews/"+projectReviewVm.ReviewMaster.ProjectID); //To satisfy Unit Testing we need to redirect to Action - need to take a look after hosting 
                }
            }

            return Redirect(Request.UrlReferrer.ToString());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitDeferralForApproval(ProjectReviewVM projectReviewVM , string submitButton)
        {
            String user = GetEmpNo();
            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            bool result;
            LogCall(user, "Project/SubmitDeferralForApproval");

           //var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);

            if (ModelState.ContainsKey("ReviewMaster.ReviewType"))
                ModelState["ReviewMaster.ReviewType"].Errors.Clear();



            if (ModelState.IsValid)
            {
                if (projectReviewVM.ProjectReviews!= null ) //AR 
                {
                    result =
                        await
                            _ampServiceLayer.RequestReviewDeferral(projectReviewVM.ProjectReviews[0].ReviewDeferralVM,
                                projectReviewVM.ProjectReviews[0].ReviewType, user, reviewsURL);
                }

                else //PCR Deferral
                {
                    result =
                            await
                                _ampServiceLayer.RequestReviewDeferral(projectReviewVM.ProjectPcrScore.ReviewDeferralVM,
                                   "Project Completion Review", user, reviewsURL);
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeferralAuthorisation(ProjectReviewVM projectReviewVM)
        {
            String user = GetEmpNo();
            string reviewsURL = Request.UrlReferrer.ToString();
            bool result;
            LogCall(user, "Project/DeferralAuthorisation");

            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);

            if (ModelState.ContainsKey("ReviewMaster.ReviewType"))
                ModelState["ReviewMaster.ReviewType"].Errors.Clear();

            if (projectReviewVM.ProjectReviews != null) //AR deferrals 
            {
                if (ModelState.ContainsKey("ProjectReviews[0].ReviewDeferralVM.DeferralJustification"))
                    ModelState["ProjectReviews[0].ReviewDeferralVM.DeferralJustification"].Errors.Clear();

                if (ModelState.ContainsKey("ProjectReviews[0].ReviewDeferralVM.DeferralTimescale"))
                    ModelState["ProjectReviews[0].ReviewDeferralVM.DeferralTimescale"].Errors.Clear();
            }
            else
            {
                if (ModelState.ContainsKey("ProjectPcrScore.ReviewDeferralVM.DeferralJustification"))
                    ModelState["ProjectPcrScore.ReviewDeferralVM.DeferralJustification"].Errors.Clear();

                if (ModelState.ContainsKey("ProjectPcrScore.ReviewDeferralVM.DeferralTimescale"))
                    ModelState["ProjectPcrScore.ReviewDeferralVM.DeferralTimescale"].Errors.Clear();
            
            
            }


            if (ModelState.IsValid)
            {
                if (projectReviewVM.ProjectReviews != null) //AR Deferral
                {

                    result =
                        await
                            _ampServiceLayer.UpdateStageForReviewDeferral(
                                projectReviewVM.ProjectReviews[0].ReviewDeferralVM,
                                projectReviewVM.ProjectReviews[0].ReviewType, user, reviewsURL);

                }


                else //PCR Deferral
                {
                    result =
                           await
                               _ampServiceLayer.UpdateStageForReviewDeferral(
                                   projectReviewVM.ProjectPcrScore.ReviewDeferralVM,
                                    "Project Completion Review", user, reviewsURL);
                
                }


            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDeferral(ProjectReviewVM projectReviewVM)
        {
            //Get logon
            string user = GetEmpNo();
            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();
            //Log user on page
            LogCall(user, "Project/DeleteDeferral");
            if (projectReviewVM.ProjectReviews != null) //AR Deferral
            {
                if (await _ampServiceLayer.DeleteDeferral(projectReviewVM.ProjectReviews[0].ReviewDeferralVM, user))
                {
                    TempData["ReviewDeleted"] = "1";
                }
                else
                {
                    TempData["ReviewDeleted"] = "0";
                }
            }
            else
            {
                if (await _ampServiceLayer.DeleteDeferral(projectReviewVM.ProjectPcrScore.ReviewDeferralVM, user))
                {
                    TempData["ReviewDeleted"] = "1";
                }
                else
                {
                    TempData["ReviewDeleted"] = "0";

                }
            }

            return Redirect(reviewsURL);
        
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteExemption(ReviewExemptionVM ReviewExemptionAR)
        {
            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();
            //Log user on page
            LogCall(user, "Project/DeleteARExemption");  
            if (ModelState.IsValid && await _ampServiceLayer.DeleteExemption(ReviewExemptionAR,user))        
            {
                TempData["ReviewDeleted"] = "1";
            }
            else
            {
                TempData["ReviewDeleted"] = "0";
            }
            return Redirect(reviewsURL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePCRExemption(ReviewExemptionVM ReviewExemptionPCR)
        {
            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();
            //Log user on page
            LogCall(user, "Project/DeletePCRExemption");
            if (ModelState.IsValid && await _ampServiceLayer.DeleteExemption(ReviewExemptionPCR, user))
            {
                TempData["ReviewDeleted"] = "1";
            }
            else
            {
                TempData["ReviewDeleted"] = "0";
            }
            return Redirect(reviewsURL);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitExemptionForApproval(ReviewExemptionVM ReviewExemptionAR)
        {
            String user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();
            //string result;
            LogCall(user, "Project/CreateARExemptionRequest");


            if (ModelState.IsValid && await _ampServiceLayer.RequestReviewExemption(ReviewExemptionAR, user, reviewsURL))
            {
                TempData["Success"] = "1";
            }
            else
            {
                TempData["Success"] = "0";
            }
            return Redirect(Request.UrlReferrer.ToString());

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitExemptionForApprovalPCR(ReviewExemptionVM ReviewExemptionPCR)
        {      
            String user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            string result;
            LogCall(user, "Project/CreateExemptionPCR");

            if (ModelState.IsValid  && await _ampServiceLayer.RequestReviewExemption(ReviewExemptionPCR, user, reviewsURL) )
            {
                TempData["Success"] = "1";
            }
            else
            {
                TempData["Success"] = "0";
            }
            return Redirect(Request.UrlReferrer.ToString());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApproveExemption(ReviewExemptionVM ReviewExemptionAR) //Approve/Reject Exemption AR
        {
            String user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();
            LogCall(user, "Project/AuthoriseARExemption");

            if (ReviewExemptionAR != null) //AR Exemptions  
            {
                if (ModelState.ContainsKey("ReviewExemptionAR.ExemptionType"))
                    ModelState["ReviewExemptionAR.ExemptionType"].Errors.Clear();

                if (ModelState.ContainsKey("ReviewExemptionAR.SubmissionComment"))
                    ModelState["ReviewExemptionAR.SubmissionComment"].Errors.Clear();
            }

            if (ModelState.IsValid && await _ampServiceLayer.ApproverExemptionAction(ReviewExemptionAR, user, reviewsURL))
            {
                TempData["Success"] = "1";
            }
            else
            {
                TempData["Success"] = "0";
            }
            return Redirect(Request.UrlReferrer.ToString());


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApproveExemptionPCR(ReviewExemptionVM ReviewExemptionPcr) //Approve/Reject Exemption
        {
            String user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            LogCall(user, "Project/ApproveExemptionPCR");

            if (ModelState.IsValid && await _ampServiceLayer.ApproverExemptionAction(ReviewExemptionPcr, user, reviewsURL))
            {
                TempData["Success"] = "1";
            }
            else
            {
                TempData["Success"] = "0";
            }
            return Redirect(Request.UrlReferrer.ToString());


        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteAnnualReview(ProjectReviewVM projectReviewVm)
        {
            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            //Log user on page
            LogCall(user, "Project/DeleteAR");

            if (_ampServiceLayer.DeleteAnnualReview(projectReviewVm.ProjectReviews[0], user))
            {
                TempData["ReviewDeleted"] = "1";
            }
            else
            {
                TempData["ReviewDeleted"] = "0";
            }

            return Redirect(reviewsURL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DeletePCR(ProjectReviewVM projectReviewVm)
        {
            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string reviewsURL = Request.UrlReferrer.ToString();

            //Log user on page
            LogCall(user, "Project/DeletePCR");

            if (_ampServiceLayer.DeletePCR(projectReviewVm.ProjectPcrScore, user))
            {
                TempData["ReviewDeleted"] = "1";
            }
            else
            {
                TempData["ReviewDeleted"] = "0";
            }

            return Redirect(reviewsURL);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitProjectForClosure(WorkflowMasterVM closeProjectRequest)
        {
            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string pageUrl = Request.UrlReferrer.ToString();

            //Log user on page
            LogCall(user, "Project/CloseProject");

            if (await _ampServiceLayer.SendProjectForClosure(closeProjectRequest, pageUrl, user))
            {
                TempData["ProjectSentForClosure"] = "1";
            }
            else
            {
                TempData["ProjectSentForClosure"] = "0";
            }

            return Redirect(pageUrl);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthoriseProjectClose(WorkflowMasterVM closeProjectResponse, string CPAuthButton)
        {
            switch (CPAuthButton)
            {
                case "Approve Project Closure":
                    return(await ApproveProjectClosure(closeProjectResponse));
                case "Reject Project Closure": 
                    return (await RejectProjectClosure(closeProjectResponse));
            }
        
            return null;
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> ApproveProjectClosure(WorkflowMasterVM closeProjectResponse)
        {
            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string pageUrl = Request.UrlReferrer.ToString();

            //Log user on page
            LogCall(user, "Project/Approve Project Closure");

            if (await _ampServiceLayer.ApproveProjectClosure(closeProjectResponse, pageUrl, user))
            {
                TempData["CPAuthSuccess"] = "1";
            }
            else
            {
                TempData["CPAuthSuccess"] = "0";
            }

            return Redirect(pageUrl);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> RejectProjectClosure(WorkflowMasterVM closeProjectResponse)
        {
            //Get logon
            string user = GetEmpNo();

            //Get current URL
            string pageUrl = Request.UrlReferrer.ToString();

            //Log user on page
            LogCall(user, "Project/Reject Project Closure");

            if (await _ampServiceLayer.RejectProjectClosure(closeProjectResponse, pageUrl, user))
            {
                TempData["CPAuthSuccess"] = "1";
            }
            else
            {
                TempData["CPAuthSuccess"] = "0";
            }

            return Redirect(pageUrl);


        }


        #region Lookup ActionResults - used by typeahead controls within View objects

        public ActionResult BudgetCentreLookup()
        {
            List<BudgetCentreKV> BudgetCentreList = _ampServiceLayer.LookupBudgetCentreKV();

            return Json(BudgetCentreList, JsonRequestBehavior.AllowGet);
        }
        //
        public async Task<ActionResult> PeopleDetailsSRO()
        {
            try
            {
                IEnumerable<PersonDetails> SROs = await _ampServiceLayer.LookUpSroUser() as IEnumerable<PersonDetails>;
                return Json(SROs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "GetEmp - Project/AdvancedSearch");
                throw;
            }
        }

        //
        public ActionResult ProjectLookUp()
        {
            try
            {
                List<ProjectKV> ProjectList = _ampServiceLayer.LookUpProjectMaster();

                return Json(ProjectList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "GetEmpNo - Project/Index");
                throw;
            }
        }


        public async Task<ActionResult> CurrencyLookUp()
        {
            try
            {
                List<CurrencyVM> Currency = await _ampServiceLayer.LookUpCurrency() as List<CurrencyVM>;

                return Json(Currency, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "GetEmpNo - Project/Index");
                throw;
            }
        }
        public ActionResult UserLookUp()
        {
            try
            {
                List<UserLookUp> Users = _ampServiceLayer.LookUpUsers();

                return Json(Users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "GetEmpNo - Project/Index");
                throw;
            }
        }

        #endregion


        // Video Log: Post
        [HttpPost]
        public ActionResult LogVideo(string video)
        {
            //Get logon
            String user = GetEmpNo();
            try
            {

                try
                {

                    _ampServiceLayer.InsertLog("Video:" + video, user);

                    //return Redirect("Index");
                    return Json("SUCCESS");
                }
                catch
                {
                    //return Redirect("Index");
                    return Json("SUCCESS");
                }
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Log Video - Post", user);
                throw;
            }
        }

        // Used for logging usage of actions which are not new pages.
        [HttpPost]
        public ActionResult Logger(string action)
        {
            //Get logon
            String user = GetEmpNo();
            try
            {

                try
                {

                    _ampServiceLayer.InsertLog(action, user);

                    //return Redirect("Index");
                    return Json("SUCCESS");
                }
                catch
                {
                    //return Redirect("Index");
                    return Json("SUCCESS");
                }
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Logger - Post", user);
                throw;
            }
        }


        #region WorkflowLinks
        public async Task<ActionResult> PlannedEndDate(string id1)
        {
            string user;
            string projectId = id1;
          
            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Get logon
            user = GetEmpNo();
            //Log user on page
            LogCall(user, "Workflow Action" + id1, null);


            WorkflowPlannedEndDateVM workflowPlannedEndDateVm = await _ampServiceLayer.GetWorkflowPlannedEndDate(projectId, user);


            if (workflowPlannedEndDateVm == null)
            {
                return HttpNotFound();
            }


            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["Success"].ToString() == "1")
            {
                ViewBag.Success = "1";
            }
            if (TempData["Success"].ToString() == "0")
            {
                ViewBag.Success = "0";
            }

            TempData["VM"] = workflowPlannedEndDateVm;

            return View(workflowPlannedEndDateVm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PlannedEndDate(WorkflowPlannedEndDateVM workflowPlannedEndDateVm)
        {
            //Get logon
            string user = GetEmpNo();

            //Log user on page
            LogCall(user, "Project/Add New Planned End Date (POST)", workflowPlannedEndDateVm.ProjectHeaderVm.ProjectID);

            //first perform some client side validation on the new planned end date
            //Post the data into table via repository call to save 
            if (_ampServiceLayer.AddPlannedEndDate(workflowPlannedEndDateVm, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                ViewBag.Success = "1";
                return RedirectToAction("Edit", "Workflow", new { id1 = workflowPlannedEndDateVm.ProjectHeaderVm.ProjectID, id2 = 12 });
            }
            else
            {

                //Model was invalid set success code to 0
                TempData["Success"] = "0";
                ViewBag.Success = "0";

                // if valid new planned end date entered then put it back together
                DateTime parsedNewPlannedEndDate;

                if (DateTime.TryParse(string.Format("{0}/{1}/{2}", workflowPlannedEndDateVm.NewPlannedEndDate_Day, workflowPlannedEndDateVm.NewPlannedEndDate_Month, workflowPlannedEndDateVm.NewPlannedEndDate_Year), out parsedNewPlannedEndDate))
                {
                    workflowPlannedEndDateVm.NewPlannedEndDate = parsedNewPlannedEndDate;
                }


                TempData["VM"] = workflowPlannedEndDateVm;

                return View("PlannedEndDate", workflowPlannedEndDateVm);
            }
        }
        public async Task<ActionResult> Action(string id)
        {
            string user;


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Get logon
            user = GetEmpNo();
            //Log user on page
            LogCall(user, "Workflow Action Menu", id);

            ProjectVM projectVm = await _ampServiceLayer.GetProjectVM(id, user);

            if (projectVm == null)
            {
                return HttpNotFound();
            }
            TempData["VM"] = projectVm;
            return View(projectVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Action(ProjectVM projectViewModel, string submitButton)
        {
            projectViewModel.RiskLookups = _ampServiceLayer.LookupRisksTypes();
            //Multiple submit buttons Exist. Work out which is incoming and send to the appropriate controller.

            switch (submitButton)
            {
                case "1": //Close Project
                    return (await PreValidateWorkflow(projectViewModel, Constants.CloseProjectTaskId));
                case "2": //Re-Approve Project
                    return (await PreValidateWorkflow(projectViewModel, Constants.ReApproveProjectTaskId));
                case "7": //Approve A&D
                    return (await PreValidateWorkflow(projectViewModel, Constants.ApproveAD));
                case "8": //Fast track
                    return (await PreValidateWorkflow(projectViewModel, Constants.FastTrack));
                case "9": //Archive
                    return (await PreValidateWorkflow(projectViewModel, Constants.ArchiveProject));
                case "10": //Re-open Project
                    return (await PreValidateWorkflow(projectViewModel, Constants.ReOpenProject));
                case "11": //Approve Project
                    return (await PreValidateWorkflow(projectViewModel, Constants.ApproveProjectTask));
                case "12":// Planned end date extension
                    return (await PreValidateWorkflowActions(projectViewModel, Constants.PlannedEndDate));
                default:
                    return (await PreValidateWorkflow(projectViewModel, Constants.ReApproveProjectTaskId));
            }
        }





        // *** This is currently only handling the new workflow 12 Planned end date and actions associated with this
        // *** everything else is going through original PreValidateWorkflow
        public async Task<ActionResult> PreValidateWorkflowActions(ProjectVM projectViewModel, Int32 taskId)
        {
            //Get logon
            string user = GetEmpNo();

            if (await _ampServiceLayer.PreValidateWorkflowApproval(projectViewModel.ProjectID, taskId, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";

             
              if (projectViewModel.WFCheck.Status)
                    // is in workflow so send straight to the woekflow edit page and let that page handle access etc
                    //otherwise send the user to the planned end date entry date screen to anemd the date
               {
                    return RedirectToAction("Edit",  "Workflow", new { id1 = projectViewModel.ProjectID, id2 = taskId });
                    
               }
                else
                {
                    return RedirectToAction("PlannedEndDate", "Project", new { id1 = projectViewModel.ProjectID, id2 = taskId });
                }
            }
            else
            {
                TempData["Success"] = "0";
                var OriginalVM = TempData["VM"] as ProjectVM;
           
                TempData["VM"] = OriginalVM;


                ////Bit of state management. Set the CCOValue IEnumerables from the ViewModel stored in TempData.
                projectViewModel.ProjectHeader = OriginalVM.ProjectHeader;
                projectViewModel.WFCheck = OriginalVM.WFCheck;
                projectViewModel.CanSendForApproval = OriginalVM.CanSendForApproval;
                projectViewModel.CanSendForClosure = OriginalVM.CanSendForClosure;
                projectViewModel.IsProjectTeam = OriginalVM.IsProjectTeam;


                //projectViewModel.RiskAtApproval = OriginalVM.RiskAtApproval;
                //projectViewModel.ProjectSRO = OriginalVM.ProjectSRO;
                //projectViewModel.ProjectDates = OriginalVM.ProjectDates;
                //projectViewModel.Title = OriginalVM.Title;
                //projectViewModel.Description = OriginalVM.Description;
                //projectViewModel.BudgetCentreID = OriginalVM.BudgetCentreID;
                //projectViewModel.BudgetCentreDescription = OriginalVM.BudgetCentreDescription;


                return View("Action", projectViewModel);
            }
        }

        #endregion

        #region Disposable


        ~ProjectController()
        {
            Dispose(false);
        }


        #endregion


    }
}




