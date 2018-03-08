using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMP.Models;
using AMP.Services;
using AMP.Utilities;
using AMP.ViewModels;


namespace AMP.Controllers
{
    public class WorkflowController : BaseController
    {

        #region Initialise

        private IAmpServiceLayer _ampServiceLayer;
        private IIdentityManager _identityManager;

        public WorkflowController()
            : base()
        {
            this._ampServiceLayer = new AMPServiceLayer();
            this._identityManager = new DemoIdentityManager();
        }

        public WorkflowController(IAmpServiceLayer serviceLayer, IIdentityManager identityManager)
            : base(serviceLayer, identityManager)
        {
            this._ampServiceLayer = serviceLayer;
            this._identityManager = identityManager;
        }

        public ErrorEngine errorengine = new ErrorEngine();


        #endregion

       // [Route("~/Workflow")]


        // GET: Workflow
        public async Task<ActionResult> Index(string id)
        {
            string user;

            Int32 intId;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Workflow History", id);

            WorkflowsVM workflowsVm = new WorkflowsVM();

            workflowsVm = await _ampServiceLayer.GetProjectWorkflows(id, user);

            if (workflowsVm == null)
            {
                return HttpNotFound();
            }

            return View(workflowsVm);
        }


        //// GET: Workflow/Details/5

        public async Task<ActionResult> Details(string id)
        {
            string user;

            Int32 intId;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!Int32.TryParse(id, out intId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkflowVM workflowVm = new WorkflowVM();

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Workflow Details", null);

            workflowVm = await _ampServiceLayer.GetWorkflow(intId, user);

            if (workflowVm == null)
            {
                return HttpNotFound();
            }

            //-------------Set Temp Data and ViewBag for WFRequest----------
            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["WFRequest"] == null) { TempData["WFRequest"] = "NA"; ViewBag.WFRequest = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["WFRequest"].ToString() == "1")
            {
                ViewBag.WFRequest = "1";
            }
            if (TempData["WFRequest"].ToString() == "0")
            {
                ViewBag.WFRequest = "0";
            }


            //-------------Set Temp Data and ViewBag for WFResponse----------
            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["WFResponse"] == null) { TempData["WFResponse"] = "NA"; ViewBag.WFResponse = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["WFResponse"].ToString() == "1")
            {
                ViewBag.WFResponse = "1";
            }
            if (TempData["WFResponse"].ToString() == "0")
            {
                ViewBag.WFResponse = "0";
            }

            //Put the ViewModel into TempData so that it can be accessed if validation fails on the POST method.
            TempData["VM"] = workflowVm;


            return View("Edit",workflowVm);

        }
   


        public async Task<ActionResult> Edit(string id1,Int32 id2)
        {
            string user;
            string projectId = id1;
            Int32 taskId = id2;

            WorkflowVM workflowVm = new WorkflowVM();

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Workflow Request " + id2, null);

            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            workflowVm = await _ampServiceLayer.GetWorkflow(projectId,taskId, user);

            if (workflowVm == null)
            {
                return HttpNotFound();
            }

            //-------------Set Temp Data and ViewBag for WFRequest----------
            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["WFRequest"] == null) { TempData["WFRequest"] = "NA"; ViewBag.WFRequest = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["WFRequest"].ToString() == "1")
            {
                ViewBag.WFRequest = "1";
            }
            if (TempData["WFRequest"].ToString() == "0")
            {
                ViewBag.WFRequest = "0";
            }


            //-------------Set Temp Data and ViewBag for WFResponse----------
            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["WFResponse"] == null) { TempData["WFResponse"] = "NA"; ViewBag.WFResponse = "NA"; }

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["WFResponse"].ToString() == "1")
            {
                ViewBag.WFResponse = "1";
            }
            if (TempData["WFResponse"].ToString() == "0")
            {
                ViewBag.WFResponse = "0";
            }

            //Put the ViewModel into TempData so that it can be accessed if validation fails on the POST method.
            TempData["VM"] = workflowVm;


            return View(workflowVm);
        }

     



        [HttpPost]
        public async Task<ActionResult> Edit(WorkflowVM workflowVm, string submitWorkflow )
        {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, String.Format("(POST) WorkFlow Request {0}", workflowVm.WorkflowRequest.TaskID), workflowVm.WorkflowRequest.ProjectID);

            string urlBase = Request.UrlReferrer.ToString();
                switch (submitWorkflow)
                {
                    case "Send for Approval":
                        if (
                            await
                                _ampServiceLayer.SendforWorkflowApproval(workflowVm,
                                    new ModelStateWrapper(this.ModelState), user, urlBase))
                        {
                            TempData["WFRequest"] = "1";
                        }
                        else
                        {
                            TempData["WFRequest"] = "0";
                        }
                        TempData["WFResponse"] = "NA";
                        break;
                    case "Approve Workflow":
                        if (
                            await
                                _ampServiceLayer.ApproveWorkflow(workflowVm, new ModelStateWrapper(this.ModelState),
                                    user))
                        {
                            TempData["WFResponse"] = "1";
                        }
                        else
                        {
                            TempData["WFResponse"] = "0";
                        }
                        TempData["WFRequest"] = "NA";
                        break;
                    case "Reject Workflow":
                        if (await _ampServiceLayer.RejectWorkflow(workflowVm, urlBase, user))
                        {
                            TempData["WFResponse"] = "1";
                        }
                        else
                        {
                            TempData["WFResponse"] = "0";
                        }
                        TempData["WFRequest"] = "NA";
                        break;
                    case "Change Approver":
                        if (await _ampServiceLayer.ChangeWorkflowApprover(workflowVm, urlBase, user))
                        {
                            TempData["WFRequest"] = "1";
                        }
                        else
                        {
                            TempData["WFRequest"] = "0";
                        }
                        TempData["WFResponse"] = "NA";
                        break;
                    case "Cancel Workflow":
                        if (await _ampServiceLayer.CancelWorkflow(workflowVm, urlBase, user))
                        {
                        TempData["WFRequest"] = "1";
                        return RedirectToAction("Action", "Project", new {id= workflowVm.WorkflowRequest.ProjectID});
                    }
                        else
                        {
                            TempData["WFRequest"] = "0";
                        }
                        TempData["WFResponse"] = "NA";
                        break;
                    
                    default:
                        break;
                }

            //TODO - Change this code to call the new Edit/WOrkflowID controller method on approval.
            if (TempData["WFRequest"].ToString() == "1")
            {
                return RedirectToAction("Edit", "Workflow", new { id1 = workflowVm.WorkflowRequest.ProjectID, id2 = workflowVm.WorkflowRequest.TaskID});
            }
            else if (TempData["WFResponse"].ToString() == "1")
            {
                return RedirectToAction("Details", "Workflow", new { id = workflowVm.WorkflowResponse.WorkFlowID});
            }else{
            var OriginalVM = TempData["VM"] as WorkflowVM;
                TempData["VM"] = OriginalVM;

                workflowVm.ProjectHeaderVm = OriginalVM.ProjectHeaderVm;
                workflowVm.TaskDescription = OriginalVM.TaskDescription;
                workflowVm.WorkflowRequest.RequesterName = OriginalVM.WorkflowRequest.RequesterName;
                return View("Edit", workflowVm);                
            }
            
           // return RedirectToAction("Edit", "Workflow", new { id1 = workflowVm.WorkflowRequest.ProjectID, id2 = workflowVm.WorkflowRequest.TaskID});

        }

        [HttpPost]
        public async Task<ActionResult> ActionWorkflowResponse(WorkflowMasterVM workflowResponse, string WFResponseButton)
        {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "(POST) WorkFlow Response " + workflowResponse.TaskID, workflowResponse.ProjectID);

            string urlBase = string.Format("{0}://{1}{2}", ControllerContext.RequestContext.HttpContext.Request.Url.Scheme, ControllerContext.RequestContext.HttpContext.Request.Url.Authority, Url.Content("~"));

            if (await _ampServiceLayer.ActionWorkflowResponse(workflowResponse, WFResponseButton, urlBase, user))
            {
                TempData["WFResponse"] = "1";
            }
            else
            {
                TempData["WFResponse"] = "0";
            }

            return RedirectToAction("Edit", new { id = workflowResponse.WorkFlowID });
        }
        #region Disposable


        ~WorkflowController()
        {
            Dispose(false);
        }


        #endregion

    }
}
