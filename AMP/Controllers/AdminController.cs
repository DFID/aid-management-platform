using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMP.Models;
using System.Threading.Tasks;
using AMP.Services;
using AMP.ViewModels;
using AMP.Utilities;


namespace AMP.Controllers
{
    public class AdminController : BaseController
    {
        #region Initialise

        private IAmpServiceLayer _ampServiceLayer;
        private IIdentityManager _identityManager;
        private IAMPRepository _ampRepository;

        public AdminController()
            : base()
        {
            this._ampServiceLayer = new AMPServiceLayer();
            this._identityManager = new DemoIdentityManager();
            this._ampRepository = new AMPRepository();
        }

        public AdminController(IAmpServiceLayer serviceLayer, IIdentityManager identityManager)
            : base(serviceLayer, identityManager)
        {
            this._ampServiceLayer = serviceLayer;
            this._identityManager = identityManager;
            this._ampRepository = new AMPRepository();
        }

        public ErrorEngine errorengine = new ErrorEngine();

        #endregion


        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditTeam(string id)
        {

            //Get logon
            String user = GetRealEmpNo();

            try
            {


                //Log user on page
                LogCall(user, "Admin/Edit Team", id);



                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                Team team = _ampServiceLayer.AdminGetTeam(id);


                if (team == null)
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


                return View(team);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditTeam", user);
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditTeam(Team team)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            //Log user on page
            LogCall(user, "Admin/EditTeam (POST)", team.ProjectID);

            if (!_ampServiceLayer.IsAdmin(user))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

                if (_ampServiceLayer.AdminUpdateTeam(team, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditTeam");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditTeam", team);                    
                }

            }


        public ActionResult EditProjectDate(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/Edit ProjectDate", Id);
                
                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                ProjectDate  projectDate = _ampServiceLayer.AdminGetProjectDate(Id);
                
                if (projectDate == null)
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

                return View(projectDate);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit Project Date", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjectDate(ProjectDate projectdate)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditProjectDate (POST)", projectdate.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateProjectDate(projectdate, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditProjectDate", new { id = projectdate.ProjectID });
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditProjectDate", projectdate);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditReviewMaster (POST)", user);
                throw;

            }




        }

        public ActionResult EditReviewMaster(string id1, Int32 id2)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditReviewMaster", id1);

                if (id1 == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ReviewMaster reviewMaster = _ampServiceLayer.AdminGetReviewMaster(id1,id2);

                if (reviewMaster == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

                return View(reviewMaster);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditReviewMaster", user);
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReviewMaster(ReviewMaster reviewMaster)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditReviewMaster (POST)", reviewMaster.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateReviewMaster(reviewMaster, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditReviewMaster", new { id = reviewMaster.ProjectID, reviewId = reviewMaster.ReviewID });
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditReviewMaster", reviewMaster);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditReviewMaster (POST)", user);
                throw;

            }


        }

        public ActionResult EditPerformance(string id)
        {
            //Get logon
            String user = GetRealEmpNo();


            try
            {
                //Log user on page
                LogCall(user, "Admin/EditPerformance", id);


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                Performance performance = _ampServiceLayer.AdminGetPerformance(id);

                if (performance == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

                return View(performance);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditPerformance", user);
                throw;
            }

        }


        public ActionResult SearchProjectForEdit()
        {
            //Get logon
            String user = GetRealEmpNo();
            try
            {
                //Log user on page
                LogCall(user, "Admin/SearchProjectForEdit");

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                ProjectMaster projectmaster = new ProjectMaster();


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

                return View(projectmaster);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/SearchProjectForEdit", user);
                throw;
            }
        }


        public ActionResult EditPerformanceNew(string id) //For User 
        {
            //Get logon
            String user = GetRealEmpNo();


            try
            {
                //Log user on page
                LogCall(user, "Admin/EditPerformance", id);
                
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                EditPerformanceVM performanceVm = new EditPerformanceVM();

                performanceVm = _ampServiceLayer.AdminGetPerformanceNewEdit(id);
                
                if (performanceVm == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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
                return View(performanceVm);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditPerformance", user);
                throw;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPerformance(Performance performance)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();
            //Get current URL
            string performanceURL = Request.UrlReferrer.ToString();

            
            try
            {

                //Log user on page
                LogCall(user, "Admin/EditPerformance (POST)", performance.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdatePerformance(performance, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    //return RedirectToAction(performanceURL);
                    return Redirect(performanceURL);
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";
                    //return View("EditPerformance", performance);
                    //return RedirectToAction(performanceURL);
                    return Redirect(performanceURL);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditPerformance (POST)", user);
                throw;

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPerformanceNew(EditPerformanceVM performanceVm) //new 
        {
            string user;

            //Get logon
            user = GetRealEmpNo();
            //Get current URL
            string performanceURL = Request.UrlReferrer.ToString();

            performanceVm.ExemptionReasons = _ampRepository.GetExemptionReasons();

            List<ExemptionReason> ExemptionReasons = _ampRepository.GetExemptionReasons();

            if (performanceVm.HasAR == "Yes") // Has AR on review exemption table 
            {
                if (performanceVm.ReviewExemptionAR.ExemptionReason != null)
                {
                    performanceVm.ARExcemptReason =
                        ExemptionReasons.FirstOrDefault(
                            x =>
                                x.ID == Convert.ToInt16(performanceVm.ReviewExemptionAR.ExemptionReason) &&
                                x.ExemptionType == "AR").ExemptionReason1;
                }
                else
                {
                    performanceVm.ARExcemptReason = null;
                }
            }
            if(performanceVm.HasPCR == "Yes")
            {
                if (performanceVm.ReviewExemptionPCR.ExemptionReason != null)
                {
                    performanceVm.PCRExcemptReason =
                        ExemptionReasons.FirstOrDefault(
                            x =>
                                x.ID == Convert.ToInt16(performanceVm.ReviewExemptionPCR.ExemptionReason) &&
                                x.ExemptionType == "PCR").ExemptionReason1;

                }
                else
                {
                    performanceVm.PCRExcemptReason = null;
                }
            }

            TempData["UpdateEditPerformanceVM"] = performanceVm;
            try
            {
                //Log user on page
                LogCall(user, "Admin/EditPerformanceNew(POST)", performanceVm.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdatePerformanceNewEdit(performanceVm, new ModelStateWrapper(this.ModelState), user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.                    
                   
                    if (performanceVm.HasAR == "Yes")
                    {
                        ReviewExemption ARreviewExemptionToUpdate = _ampRepository.GetReviewExemption(performanceVm.ProjectID, "Annual Review");
                        ARreviewExemptionToUpdate.ExemptionReason = performanceVm.ReviewExemptionAR.ExemptionReason;
                        ARreviewExemptionToUpdate.SubmissionComment = performanceVm.ARExemptJustification;
                        bool ARTrue = _ampServiceLayer.AdminUpdateReviewExemption(ARreviewExemptionToUpdate, user);
                    }
                    if (performanceVm.HasPCR == "Yes")
                    {
                        ReviewExemption PCRreviewExemptionToUpdate = _ampRepository.GetReviewExemption(performanceVm.ProjectID, "Project Completion Review");
                        PCRreviewExemptionToUpdate.ExemptionReason = performanceVm.ReviewExemptionPCR.ExemptionReason;
                        PCRreviewExemptionToUpdate.SubmissionComment = performanceVm.PCRExemptJustification;
                        bool ARTrue = _ampServiceLayer.AdminUpdateReviewExemption(PCRreviewExemptionToUpdate, user);
                    }

                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    //return RedirectToAction(performanceURL);
                    return Redirect(performanceURL);
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";
                    performanceVm = TempData["UpdateEditPerformanceVM"] as EditPerformanceVM;
         
                    return View(performanceVm);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditPerformance (POST)", user);
                throw;

            }

        }
        public ActionResult Admin()
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            //Log user on page
            LogCall(user, "Admin Page", user);

            if (_ampServiceLayer.IsAdmin(user))
            {
                AdminUsersVM adminUsersVm = _ampServiceLayer.GetAdminUsers(user);

                return View("Admin", adminUsersVm);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        [HttpPost]
        public ActionResult RemoveAdminUser(string adminToRemove)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            //Log user on page
            LogCall(user, "Admin/Remove Admin User (POST)", null);

            if (!_ampServiceLayer.IsAdmin(user))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            try
            {
                //FUDGE: Add a zero onto the front of the adminToRemove string. Keep losing leading zero, really annoying - CJF.
                //Fudge the TeamID. If this is a repost after a validation fail, the hidden field drops the leading zero. If the TeamID is of length 5, stick a zero on it - 28 Aug 2015 C Finnan
                if (adminToRemove != null && adminToRemove.Length < 6)
                {
                    adminToRemove= "0" + adminToRemove;
                }

                if (_ampServiceLayer.RemoveAdmin(adminToRemove, user))
                {
                    //return Redirect("Index");
                    return Json(new { success = true, response = "Successful message" });

                }
                return Json(new { success = false, response = "Failed to remove admin" });
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Admin - remove Admin (POST)", user);
                throw;
            }

        }

        [HttpPost]
        public ActionResult Admin(AdminUsersVM adminUsersVm)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            //Log user on page
            LogCall(user, "Admin/Add Admin User (POST)", null);

            if (!_ampServiceLayer.IsAdmin(user))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            try
            {
                //FUDGE: Add a zero onto the front of the adminToRemove string. Keep losing leading zero, really annoying - CJF.
                //Fudge the TeamID. If this is a repost after a validation fail, the hidden field drops the leading zero. If the TeamID is of length 5, stick a zero on it - 28 Aug 2015 C Finnan
                //if (adminToRemove != null && adminToRemove.Length < 6)
                //{
                //    adminToRemove = "0" + adminToRemove;
                //}

                if (_ampServiceLayer.AddAdmin(adminUsersVm.AdminToAdd, user))
                {
                    //return Redirect("Index");
                    return RedirectToAction("Admin");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

                }
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Admin - Add Admin (POST)", user);
                throw;
            }

        }

        public ActionResult EditWorkflowMaster(string id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditWorkflowMaster", id);


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                WorkflowMaster workflowMaster = _ampServiceLayer.AdminGetWorkflowMaster(id);

                if (workflowMaster == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

                return View(workflowMaster);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditWorkflowMaster", user);
                throw;
            }

            
        }

        [HttpPost]

        public ActionResult EditWorkflowMaster(WorkflowMaster workflowMaster)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditWorkflowMaster (POST)", workflowMaster.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateWorkflowMaster(workflowMaster, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditWorkflowMaster");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditWorkflowMaster", workflowMaster);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditWorkflowMaster (POST)", user);
                throw;

            }

        }


        public ActionResult EditWorkflowDocument(string id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditWorkflowDocument", id);


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                WorkflowDocument workflowDocument = _ampServiceLayer.AdminGetWorkflowDocument(id);

                if (workflowDocument == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

                return View(workflowDocument);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditWorkflowDocument", user);
                throw;
            }


        }

        [HttpPost]

        public ActionResult EditWorkflowDocument(WorkflowDocument workflowDocument)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditWorkflowDocument (POST)", workflowDocument.ID.ToString());

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateWorkflowDocument(workflowDocument, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditWorkflowDocument");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditWorkflowDocument", workflowDocument);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditWorkflowDocument (POST)", user);
                throw;

            }

        }


        public ActionResult EditARReviewExemption(string id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditARReviewExemption", id);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ReviewExemption reviewExemption = _ampServiceLayer.AdminGetReviewExemption(id, "Annual Review");

                if (reviewExemption == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

                return View("EditARReviewExemption", reviewExemption);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditARReviewExemption", user);
                throw;
            }

        }



        public ActionResult EditPCRReviewExemption(string id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditPCRReviewExemption", id);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ReviewExemption reviewExemption = _ampServiceLayer.AdminGetReviewExemption(id, "Project Completion Review");

                if (reviewExemption == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

                return View("EditPCRReviewExemption", reviewExemption);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditPCReviewExemption", user);
                throw;
            }

        }



        [HttpPost]
        public ActionResult EditPCRReviewExemption(ReviewExemption reviewExemption)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditPCRReviewExemption (POST)", reviewExemption.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateReviewExemption(reviewExemption, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditPCRReviewExemption");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditPCRReviewExemption", reviewExemption);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditPCRReviewExemption (POST)", user);
                throw;

            }


        }


        [HttpPost]
        public ActionResult EditARReviewExemption(ReviewExemption reviewExemption)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditARReviewExemption (POST)", reviewExemption.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateReviewExemption(reviewExemption, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditARReviewExemption");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditARReviewExemption", reviewExemption);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditARReviewExemption (POST)", user);
                throw;

            }
        }


        public ActionResult CloseProject()
        {
            //Get logon
            String user = GetRealEmpNo();
            try
            {
                //Log user on page
                LogCall(user, "Admin/Close Project");

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                ProjectMaster projectmaster = new ProjectMaster();


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

                return View(projectmaster);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/CloseProject", user);
                throw;
            }
        }

        [HttpPost]
        public ActionResult CloseProject(ProjectMaster project)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            //Log user on page
            LogCall(user, "Admin/CloseProject (POST)", null);

            if (!_ampServiceLayer.IsAdmin(user))
            {
              
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            try
            {
                //FUDGE: Add a zero onto the front of the adminToRemove string. Keep losing leading zero, really annoying - CJF.
                //Fudge the TeamID. If this is a repost after a validation fail, the hidden field drops the leading zero. If the TeamID is of length 5, stick a zero on it - 28 Aug 2015 C Finnan
                //if (adminToRemove != null && adminToRemove.Length < 6)
                //{
                //    adminToRemove = "0" + adminToRemove;
                //}

                if (_ampServiceLayer.AdminCloseProject(project.ProjectID, user))
                {  // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    //return Redirect("Index");
                    return RedirectToAction("CloseProject");
                }
                else
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

                }
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Admin - Close Project (POST)", user);
                throw;
            }

        }

        public ActionResult EditInputSectorCode(string id1, string id2)
        {
            string user = GetRealEmpNo();
            try
            {
                LogCall(user, "Admin/EditInputSectorCode");

                if (id1 == null || id2 == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                InputSectorCode inputSectorCode = _ampServiceLayer.AdminGetComponentInputSector(id1,id2);

                if (inputSectorCode == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

                return View(inputSectorCode);

            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Admin/EditComponentInputSector", user);
                throw;
            }
           
        }

        [HttpPost]

        public ActionResult EditInputSectorCode(InputSectorCode inputSectorCode)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditInputSectorCode (POST)");

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateInputSectorCode(inputSectorCode, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return View("EditInputSectorCode", inputSectorCode);
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditInputSectorCode", inputSectorCode);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditWorkflowMaster (POST)", user);
                throw;

            }

        }

        public ActionResult EditComponentMaster(string id)
        {
            string user = GetRealEmpNo();
            try
            {
                LogCall(user, "Admin/EditComponentMaster", id);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ComponentMaster componentMaster = _ampServiceLayer.ReturnComponentMaster(id);

                if (componentMaster == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

                return View("EditComponentMaster", componentMaster);

            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Admin/EditComponentMaster", user);
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditComponentMaster(ComponentMaster componentMaster)
        {
            string user = GetRealEmpNo();

            try
            {
                LogCall(user, "Admin/EditComponentMaster (POST)", componentMaster.ComponentID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateComponentMaster(componentMaster, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditComponentMaster");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditComponentMaster", componentMaster);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditComponentMaster (POST)", user);
                throw;

            }

        }

        public ActionResult EditProjectMaster(string id)
        {
            string user = GetRealEmpNo();
            try
            {
                LogCall(user, "Admin/EditProjectMaster", id);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ProjectMaster projectMaster = _ampServiceLayer.ReturnProjectMaster(id);

                if (projectMaster == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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
                return View("EditProjectMaster", projectMaster);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Admin/EditProjectMaster", user);
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditProjectMaster(ProjectMaster projectMaster)
        {
            string user = GetRealEmpNo();
            try
            {
                LogCall(user, "Admin/EditProjectMaster (POST)", projectMaster.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateProjectMaster(projectMaster, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditProjectMaster");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";
                    return View("EditProjectMaster", projectMaster);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditProjectMaster (POST)", user);
                throw;

            }

        }

        public ActionResult EditComponentDates(string id)
        {
            string user = GetRealEmpNo();
            try
            {
                LogCall(user, "Admin/EditComponentDates", id);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ComponentDate componentDates = _ampServiceLayer.AdminGetComponentDates(id);
                if (componentDates == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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
                return View("EditComponentDates", componentDates);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Admin/EditComponentDates", user);
                throw;
            }
        }
        [HttpPost]
        public ActionResult EditComponentDates(ComponentDate componentDates)
        {
            string user = GetRealEmpNo();
            try
            {
                LogCall(user, "Admin/EditComponenetDates (POST)", componentDates.ComponentID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateComponentDates(componentDates, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditComponentDates");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditComponentDates", componentDates);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditComponentDates (POST)", user);
                throw;

            }

        }

        public ActionResult EditProjectInfo(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditProjectInfo", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                ProjectInfo projectInfo = _ampServiceLayer.AdminGetProjectInfo(Id);

                if (projectInfo == null)
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

                return View(projectInfo);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit Project Info", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjectInfo(ProjectInfo projectInfo)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditProjectInfo (POST)", projectInfo.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateProjectInfo(projectInfo, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditProjectInfo");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditProjectInfo", projectInfo);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditProjectInfo (POST)", user);
                throw;

            }

        }


        public ActionResult EditProjectMarkers(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditProjectMarkers", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                Markers1 projectMarkers = _ampServiceLayer.AdminGetProjectMarkers(Id);

                if (projectMarkers == null)
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

                return View(projectMarkers);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit Project Markers", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjectMarkers(Markers1 projectMarkers)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditProjectMarkers (POST)", projectMarkers.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateProjectMarkers(projectMarkers, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditProjectMarkers");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditProjectMarkers", projectMarkers);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditProjectMarkers (POST)", user);
                throw;

            }

        }

        [HttpGet]
        public ActionResult EditDeliveryChain(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditDeliveryMarkers", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                DeliveryChain deliveryChain = _ampServiceLayer.AdminGetDeliveryChain(Id);

                if (deliveryChain == null)
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

                return View(deliveryChain);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit DeliveryChain", user);
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDeliveryChain(DeliveryChain deliveryChain)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/Edit DeliveryChain (POST)", deliveryChain.ID.ToString());

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateDeliveryChain(deliveryChain, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditDeliveryChain");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditDeliveryChain", deliveryChain);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit DeliveryChain (POST)", user);
                throw;

            }
        }

        [HttpGet]
        public ActionResult EditReviewDeferral(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditDeliveryMarkers", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                ReviewDeferral reviewDeferral = _ampServiceLayer.AdminGetReviewDeferral(Id);

                if (reviewDeferral == null)
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

                return View(reviewDeferral);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit reviewDeferral", user);
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReviewDeferral(ReviewDeferral reviewDeferral)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/Edit ReviewDeferral (POST)", reviewDeferral.DeferralID.ToString());

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateReviewDeferral(reviewDeferral, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditReviewDeferral");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditReviewDeferral", reviewDeferral);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit ReviewDeferral (POST)", user);
                throw;

            }
        }



        public ActionResult EditAuditedStatement(string id1, string id2)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditAuditedStatement", id1);


                if (id1 == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                AuditedFinancialStatement auditedStatement = _ampServiceLayer.AdminGetAuditedStatement(id1, id2);

                if (auditedStatement == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

                return View(auditedStatement);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditAuditedStatement", user);
                throw;
            }


        }

        [HttpPost]

        public ActionResult EditAuditedStatement(AuditedFinancialStatement auditedStatement)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditAuditedStatement (POST)", auditedStatement.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateAuditedStatement(auditedStatement, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditAuditedStatement", new { id1 = auditedStatement.ProjectID, id2 = auditedStatement.StatementID });
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditAuditedStatement", auditedStatement);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditAuditedStatement (POST)", user);
                throw;

            }

        }



        public ActionResult EditRiskDocument(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditRiskDocument", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                RiskDocument riskDocument = _ampServiceLayer.AdminGetRiskDocument(Id);

                if (riskDocument == null)
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

                return View(riskDocument);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit Risk Document", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRiskDocument(RiskDocument riskDocument)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditRiskDocument (POST)", riskDocument.ProjectID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateRiskDocument(riskDocument, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditRiskDocument");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditRiskDocument", riskDocument);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditRiskDocument (POST)", user);
                throw;

            }

        }


        public ActionResult EditEvaluationDocument(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditEvaluationDocument", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                EvaluationDocument evaluationDocument = _ampServiceLayer.AdminGetEvaluationDocument(Id);

                if (evaluationDocument == null)
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

                return View(evaluationDocument);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit Evaluation Document", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvaluationDocument(EvaluationDocument evaluationDocument)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditEvaluationDocument (POST)", evaluationDocument.ID.ToString());

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateEvaluationDocument(evaluationDocument, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditEvaluationDocument");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditEvaluationDocument", evaluationDocument);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditEvaluationDocument (POST)", user);
                throw;

            }

        }



        public ActionResult EditReviewDocument(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditReviewDocument", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                ReviewDocument reviewDocument = _ampServiceLayer.AdminGetReviewDocument(Id);

                if (reviewDocument == null)
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

                return View(reviewDocument);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit Review Document", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReviewDocument(ReviewDocument reviewDocument)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditReviewDocument (POST)", reviewDocument.ReviewDocumentsID.ToString());

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateReviewDocument(reviewDocument, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditReviewDocument");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditReviewDocument", reviewDocument);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditReviewDocument (POST)", user);
                throw;

            }

        }


        public ActionResult EditMechSectorMapping(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditMechSectorMapping", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                FundingMechToSector mechSectorMapping = _ampServiceLayer.AdminGetMechSectorMapping(Id);

                if (mechSectorMapping == null)
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

                return View(mechSectorMapping);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit Mech Sector Mapping", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMechSectorMapping(FundingMechToSector mechToSector)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditMechSectorMapping (POST)", mechToSector.ID.ToString());

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdateMechSectorMapping(mechToSector, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditMechSectorMapping");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditMechSectorMapping", mechToSector);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditMechSectorMapping (POST)", user);
                throw;

            }

        }


        public ActionResult NewMechSectorMapping(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/NewMechSectorMapping", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                NewFundingMechToSectorVM newMechSectorMapping = _ampServiceLayer.AdminGetNewMechSectorMapping(Id);

                if (newMechSectorMapping == null)
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
                ViewBag.DropDownList = new SelectList(newMechSectorMapping.UnMappedOptions);
                return View(newMechSectorMapping);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/New Mech Sector Mapping", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewMechSectorMapping(NewFundingMechToSectorVM newMechToSector)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/NewMechSectorMapping (POST)", newMechToSector.InputSector.InputSectorCodeID);

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminAddNewMechSectorMapping(newMechToSector, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("NewMechSectorMapping");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("NewMechSectorMapping", newMechToSector);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/NewMechSectorMapping (POST)", user);
                throw;

            }

        }


        public ActionResult EditPartnerMaster(string Id)
        {
            //Get logon
            String user = GetRealEmpNo();

            try
            {
                //Log user on page
                LogCall(user, "Admin/EditPartnerMaster", Id);

                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                PartnerMaster partnerMaster = _ampServiceLayer.AdminGetPartnerMaster(Id);

                if (partnerMaster == null)
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

                return View(partnerMaster);
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/Edit Partner Master", user);
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartnerMaster(PartnerMaster partnerMaster)
        {
            string user;

            //Get logon
            user = GetRealEmpNo();

            try
            {

                //Log user on page
                LogCall(user, "Admin/EditPartnerMaster(POST)", partnerMaster.ID.ToString());

                if (!_ampServiceLayer.IsAdmin(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (_ampServiceLayer.AdminUpdatePartnerMaster(partnerMaster, user))
                {
                    // If modelState was valid, save will occur, set TempData to success code 1.
                    TempData["Success"] = "1";
                    ViewBag.Success = "1";
                    return RedirectToAction("EditPartnerMaster");
                }
                else
                {
                    //Model was invalid set success code to 0
                    TempData["Success"] = "0";
                    ViewBag.Success = "0";

                    return View("EditPartnerMaster", partnerMaster);
                }

            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Controller Action - Admin/EditPartnerMaster (POST)", user);
                throw;

            }

        }


    }

}
    
