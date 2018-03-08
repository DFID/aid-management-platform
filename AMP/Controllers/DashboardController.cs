using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMP.Models;
using AMP.Services;
using AMP.Utilities;
using AMP.ViewModels;

namespace AMP.Controllers
{
    public class DashboardController : BaseController
    {
        #region Initialise

        private IAmpServiceLayer _ampServiceLayer;
        private IIdentityManager _identityManager;


        public DashboardController()
            : base()
        {
            this._ampServiceLayer = new AMPServiceLayer();
            this._identityManager = new DemoIdentityManager();
        }

        public DashboardController(IAmpServiceLayer serviceLayer, IIdentityManager identityManager)
            : base(serviceLayer, identityManager)
        {
            this._ampServiceLayer = serviceLayer;
            this._identityManager = identityManager;
        }

        public ErrorEngine errorengine = new ErrorEngine();

        #endregion

        [Route("~/Dashboard")]
        [Route("Dashboard/Index/{sortOrder?}/{page?}/{searchString?}/{currentFilter?}")]
        //[Route("~/")]


        public async Task<ActionResult> Index(string sortOrder, int? page, string searchString, string currentFilter)
        {

            try
            {
                String user;

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Dashboard/Index";
                String Description = "Test Total Dashboard Index Code Performance";
                //Start Timing
                From = DateTime.Now;


                ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID_desc" : "";
                ViewBag.ApprovedBudgetParm = sortOrder == "ApprovedBudget" ? "ApprovedBudget_desc" : "ApprovedBudget";
                ViewBag.StageParm = sortOrder == "Stage" ? "Stage_desc" : "Stage";
                ViewBag.NextReviewParm = sortOrder == "NextReview" ? "NextReview_desc" : "NextReview";

                //To match the route a string is being passed if sortOrder is empty
                if (String.IsNullOrEmpty(sortOrder))
                {
                    ViewBag.CurrentSort = "Firstload";
                }
                else
                {
                    ViewBag.CurrentSort = sortOrder;
                }

                //Get logon
                user = GetEmpNo();

                ViewBag.User = "R" + user;

                //Log user on page
                LogCall(user, "Dashboard/Index");

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                // Control page size (10 fits the screen rather nice)
                //Page size max int value means there wont be any paging

                int pageSize = Session["isPagingEnabled"] == "F" ? int.MaxValue : 10;
                if (Session["isPagingEnabled"] == "F")
                    ViewBag.PagingOn = "F";

                //If first load or null then page 1 or actual value.
                int pageNumber = (page ?? 1);

                // Get all Project
                DashboardViewModel dashboardVM;
                try
                {
                    dashboardVM = await _ampServiceLayer.GetProjects(searchString, pageNumber, pageSize, user, sortOrder);
                }
                catch (Exception ex)
                {
                    errorengine.LogError(ex, "GetProjects - Dashboard/Index", user);
                    throw;
                }

                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.ProjectFrom = dashboardVM.userprojects.FirstItemOnPage;
                ViewBag.ProjectTo = dashboardVM.userprojects.LastItemOnPage;

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

                TempData["VM"] = dashboardVM;

                ViewBag.DFIDTasksServer = ConfigurationManager.AppSettings["DFIDTasksUrl"].ToString();
                ViewBag.BaseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
                ViewBag.ARIESUrl = ConfigurationManager.AppSettings["ARIESUrl"].ToString();

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                //Return the view as a PagedList along with page number and size.
                return View(dashboardVM);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "");
                throw;
            }
        }

        [HttpPost]
        //POST :/Dashboard/Index
        [Route("~/Dashboard")]
        [Route("Dashboard/Index/{sortOrder?}/{page?}/{searchString?}/{currentFilter?}")]
        public ActionResult Index(DashboardViewModel dashboardviewmodel, string NewProjectID, string SearchString, string SubmitButton, string id, string pagingEnabled)//, string id
        {

            //Multiple submit buttons Exist. Workout which is incoming and send to the appropriate controller.
            if (id == null)
            {
                return (AddProject(dashboardviewmodel, NewProjectID, SearchString));
            }
            else
            {
                return (RemoveProject(id));
            }

        }

        // Index: Post Add Project
        [HttpPost]
        public ActionResult AddProject(DashboardViewModel dashboardviewmodel, string NewProjectID, string SearchString)
        {

            //Get logon
            String user = GetEmpNo();


            //Log user on page
            LogCall(user, "Add Project Dashboard", NewProjectID);

            try
            {
                // Access the Current VM from TempData (May be empty if this is first GET)
                var OriginalVM = TempData["VM"] as DashboardViewModel;



                if (_ampServiceLayer.AddProject(NewProjectID, user, dashboardviewmodel, OriginalVM, new ModelStateWrapper(this.ModelState)))
                {

                    //serviceLayer.AddSector(componentviewmodel,);
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.ProjectAdded = "1";
                    TempData["ProjectAdded"] = "1";

                    //return Redirect("Index");

                    return RedirectToAction("Index");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    // Set TempData to current view model.
                    TempData["VM"] = OriginalVM;
                    ViewBag.User = "R" + user;
                    ViewBag.DFIDTasksServer = ConfigurationManager.AppSettings["DFIDTasksUrl"].ToString();
                    ViewBag.BaseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
                    ViewBag.ARIESUrl = ConfigurationManager.AppSettings["ARIESUrl"].ToString();
                    //I think this can be removed:
                    //Hello nUnit!! Beter spoof that redirect then.
                    //if (this.Request != null)
                    //{
                    //    return Redirect(this.Request.RawUrl);
                    //}
                    //else
                    //{
                    //    //Hello nUnit!! Beter spoof that redirect then.
                    //    return RedirectToAction("Index");
                    //}

                    return View(OriginalVM);
                }


            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Add Project - Index/Post", user);
                throw;
            }
        }

        // Index: Post Add Project
        [HttpPost]
        public ActionResult SaveProjectToDashBoard(string id)
        {

            //Get logon
            String user = GetEmpNo();


            //Log user on page
            LogCall(user, "Add Project to Dashboard via Project", id);

            try
            {
                try
                {
                    bool result = _ampServiceLayer.AddProject(id, user);

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
                errorengine.LogError(ex, "Add Project - Index/Post", user);
                throw;
            }
        }

        // Index: Post
        [HttpPost]
        public ActionResult RemoveProject(string id)
        {
            //Get logon
            String user = GetEmpNo();

            //Log user on page
            LogCall(user, "Remove project from dashboard", id);

            try
            {
                string ProjectToRemove = id;
                try
                {

                    bool result = _ampServiceLayer.RemoveProject(ProjectToRemove, user);

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
                errorengine.LogError(ex, "Remove Project - Index/Post", user);
                throw;
            }
        }

        [HttpPost]
        public ActionResult SetPagingPreference(string pagingEnabled)
        {
            if (pagingEnabled == "F")
            {
                Session["isPagingEnabled"] = "F";
            }
            else
            {
                Session["isPagingEnabled"] = "T";
            }


            return Json("success");

            // return RedirectToAction("Index");
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

                string searchKeyword = string.Empty;
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
                //advanceSearchVM.BudgetCentre = _ampServiceLayer.LookupBudgetCentreKV();

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
                    //advanceSearch.BudgetCentre = advanceSearchVM.BudgetCentre;

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
                    //advanceSearch.BudgetCentre = advanceSearchVM.BudgetCentre;

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

                    if (!string.IsNullOrEmpty(advanceSearch.SearchKeyWord))
                    {
                        Session["SK"] = advanceSearch.SearchKeyWord.ToString().Trim();
                    }
                    else { Session["SK"] = string.Empty; }
                    if (!string.IsNullOrEmpty(advanceSearch.stage))
                    {
                        Session["PS"] = advanceSearch.stage.ToString().Trim();
                    }
                    else { Session["PS"] = string.Empty; }
                    if (!string.IsNullOrEmpty(advanceSearch.BenefittingCountryID))
                    {
                        Session["BC"] = advanceSearch.BenefittingCountryID.ToString().Trim();
                    }
                    else { Session["BC"] = string.Empty; }
                    if (!string.IsNullOrEmpty(advanceSearch.BudgetCentreID))
                    {
                        Session["BdId"] = advanceSearch.BudgetCentreID.ToString().Trim();
                    }
                    else { Session["BdId"] = string.Empty; }
                    if (!string.IsNullOrEmpty(advanceSearch.SRO))
                    {
                        Session["Sro"] = advanceSearch.SRO.ToString().Trim();
                    }
                    else { Session["Sro"] = string.Empty; }

                    advanceSearch = await _ampServiceLayer.GetProjectsAdvanceSearch(Session["SK"].ToString(), Session["PS"].ToString(), pageNumber, 10, stageChoice, Session["BC"].ToString(), user, Session["BdId"].ToString(), Session["Sro"].ToString(), Session["isPagingEnabledAS"].ToString());

                    advanceSearchVM.ProjectStages = _ampServiceLayer.GetProjectStages();
                    advanceSearchVM.BenefitingCountry = _ampServiceLayer.GetBenefitingCountry();


                    advanceSearch.ProjectStages = advanceSearchVM.ProjectStages;
                    advanceSearch.BenefitingCountry = advanceSearchVM.BenefitingCountry;
                    //advanceSearch.BudgetCentre = advanceSearchVM.BudgetCentre;

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

    }
}