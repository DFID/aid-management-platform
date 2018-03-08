using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMP.Models;
using PagedList; // Enable PagedList.MVC NuGet Package
using System.Threading.Tasks;
using System.Web.Services.Description;
using AMP.ARIESModels;
using AMP.Services;
using AMP.ViewModels;
using AMP.Utilities;
using AutoMapper.Internal;
using Microsoft.Ajax.Utilities;
using Microsoft.SqlServer.Server;
using MoreLinq;

namespace AMP.Controllers
{
    public class ComponentController : BaseController
    {
        #region Initilise
        private IAmpServiceLayer _ampServiceLayer;
        private IIdentityManager _identityManager;

        public ComponentController():base()
        {
            this._ampServiceLayer = new AMPServiceLayer();
            this._identityManager = new DemoIdentityManager();
        }

        public ComponentController(IAmpServiceLayer serviceLayer, IIdentityManager identityManager):base(serviceLayer, identityManager)
        {
            this._ampServiceLayer = serviceLayer;
            this._identityManager = identityManager;
        }

        public ErrorEngine errorengine = new ErrorEngine();


        #endregion

        // Component/Edit/id
        public async Task<ActionResult> Edit(string id)
        {
            //Get logon
            String user = GetEmpNo();

         try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Component/Edit";
                String Description = "Test Total Project Index Code Performance";

                //Start Timing
                From = DateTime.Now;


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ComponentVM componentVm = await _ampServiceLayer.GetComponentEdit(id, user);

            
            //This approach is good for drop downs (Replaced with Bloodhound)
            //ViewBag.BudgetCentreID = new SelectList(componentdetailsviewmodel.BudgetCentre, "BudgetCentreID","BudgetCentreDescription");

                if (componentVm == null)
                {
                    return HttpNotFound();
                }

           

            _ampServiceLayer.InsertLog("Component/Edit", user, id);

                if (componentVm.FundingMechanism == "HUMANITASSISTANCE")
                {
                    //Bind the CCO Dropdown boxes
                    ViewBag.FundingMechdd = new SelectList(componentVm.FundingMechs, "FundingMechID", "FundingMechDescription", componentVm.FundingMechanism);
                }
                else
                {
                    //!!!!!!!!!!Hack remove Humanitarian Funding Mech
                    List<FundingMech> mechs = componentVm.FundingMechs.ToList();

                    FundingMech mech = mechs.Where(x => x.FundingMechID.Equals("HUMANITASSISTANCE")).FirstOrDefault();

                    mechs.Remove(mech);

                    componentVm.FundingMechs = mechs;

                    //Bind the CCO Dropdown boxes
                    ViewBag.FundingMechdd = new SelectList(componentVm.FundingMechs, "FundingMechID", "FundingMechDescription", componentVm.FundingMechanism);
                    
                }

                //Bind the Dropdown boxes 
                ViewBag.FundingArrangementdd = new SelectList(componentVm.FundingArrangements, "FundingArrangementValue", "FundingArrangementType", componentVm.FundingArrangementValue);
                //Bind the Partner Oragnisation Dropdown boxes 
                ViewBag.PartnerOrgdd = new SelectList(componentVm.PartnerOrganisations, "PartnerOrganisationValue", "PartnerOrganisationType", componentVm.PartnerOrganisationValue);



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

            TempData["VM"] = componentVm;

            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            return View(componentVm);
            }
         catch (Exception ex)
         {
             errorengine.LogError(ex, "Component/Edit", user);
             throw;
         }
        }

        // POST: /Component/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ComponentVM componentVm, string FundingMechdd, string FundingArrangementdd, string PartnerOrgdd)
        {
            String user;

            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/Edit";
            String Description = "Test Total Component Edit Code Performance";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            user = GetEmpNo();

            componentVm.FundingArrangementValue = FundingArrangementdd;
            componentVm.PartnerOrganisationValue = PartnerOrgdd;

            if (_ampServiceLayer.UpdateComponent(componentVm, FundingMechdd, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                ViewBag.Success = 1;

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                return RedirectToAction("Edit");
            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";
                ViewBag.Success = 0;

                var OriginalVM = TempData["VM"] as ComponentVM;
                TempData["VM"] = OriginalVM;
                
                componentVm.FundingMechs = OriginalVM.FundingMechs;
                componentVm.ComponentHeader = OriginalVM.ComponentHeader;
                componentVm.FundingMechanism = OriginalVM.FundingMechanism;
                componentVm.FundingArrangements = OriginalVM.FundingArrangements;
                componentVm.FundingArrangementValue = OriginalVM.FundingArrangementValue;
                componentVm.PartnerOrganisationValue = OriginalVM.PartnerOrganisationValue;
                componentVm.PartnerOrganisations = OriginalVM.PartnerOrganisations;
                componentVm.ProjectPastEndDate = OriginalVM.ProjectPastEndDate;
                componentVm.AdminApproverDescription = OriginalVM.AdminApproverDescription;
                componentVm.AdminApprover = OriginalVM.AdminApprover;
                componentVm.BudgetCentreDescription = OriginalVM.BudgetCentreDescription;

                //Bind the CCO Dropdown boxes
                ViewBag.FundingMechdd = new SelectList(componentVm.FundingMechs, "FundingMechID", "FundingMechDescription", componentVm.FundingMechanism);
                //Bind the Dropdown boxes 
                ViewBag.FundingArrangementdd = new SelectList(componentVm.FundingArrangements, "FundingArrangementValue", "FundingArrangementType", componentVm.FundingArrangementValue);
                //Bind the Partner Oragnisation Dropdown boxes 
                ViewBag.PartnerOrgdd = new SelectList(componentVm.PartnerOrganisations, "PartnerOrganisationValue", "PartnerOrganisationType", componentVm.PartnerOrganisationValue);


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                return View(componentVm);
            }

         

        }

        // Component/Sectors/id
        public async Task<ActionResult> Sectors(string id)
        {
            //Get logon
            String user = GetEmpNo();
         try
            {


            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/Sectors";
            String Description = "Test Total Project Index Code Performance";

            //Start Timing
            From = DateTime.Now;


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ComponentSectorVM componentSectorVm = await _ampServiceLayer.GetSectors(id, user);

            //This approach is good for drop downs (Replaced with Bloodhound)
            //ViewBag.BudgetCentreID = new SelectList(componentdetailsviewmodel.BudgetCentre, "BudgetCentreID","BudgetCentreDescription");

            if (componentSectorVm == null)
            {
                return HttpNotFound();
            }

          

            _ampServiceLayer.InsertLog("Component/Sectors", user, id);

            //Bind the CCO Dropdown boxes
            //ViewBag.FundingMechdd = new SelectList(componentviewmodel.FundingMechs, "FundingMechID", "FundingMechDescription", componentviewmodel.ComponentMaster.FundingMechanism);


            TempData["ComponentID"] = componentSectorVm.ComponentHeader.ComponentID;


            // If this page loads for the first time, TempData will be empty, set it to NA as no save has happened yet.
            if (TempData["Success"] == null) { TempData["Success"] = "NA"; ViewBag.Success = "NA";}

            //If TempData has been set to 1 meaning a succesfull save, set the ViewBag.Sucess status accordingly. This will be used by javascript on the edit view to show/hide save messages.
            if (TempData["Success"].ToString() == "1")
            {
                ViewBag.Success = "1";
            }
            if (TempData["Success"].ToString() == "0")
            {
                ViewBag.Success = "0";
            }
            TempData["VM"] = componentSectorVm;

            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            return View(componentSectorVm);
              }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Component/Sectors",user);
                throw;
            }
        }

        // POST: /Component/Sectors/
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sectors(ComponentSectorVM componentSectorVm)
        {

            String user;

            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/Sectors/POST";
            String Description = "Test Total Component Sector Code Performance";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            user = GetEmpNo();


            // Access the Current VM from TempData (May be empty if this is first GET)
            var OriginalVM = TempData["VM"] as ComponentSectorVM;

            // Check Model State if its valid then update else return with validation errors.
            //if (ModelState.IsValid)

            if (_ampServiceLayer.AddSector(componentSectorVm,new ModelStateWrapper(this.ModelState)))
            {
                
                //serviceLayer.AddSector(componentviewmodel,);
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                return RedirectToAction("Sectors");
            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";
                ViewBag.Success = "0";
                
                // Set TempData to current view model.
                TempData["VM"] = OriginalVM;

                //return View(componentviewmodel);
                //Return the original view model with validation errors.

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);


                return View(OriginalVM);
            }

        }

        [HttpPost]
        public ActionResult DeleteSector(string componentid, int sectorcode)
                {
            string user;

            //Get logon
            user = GetEmpNo();

            //Log user on page
            LogCall(user, "Component/Delete Sector (POST)", componentid);

            try
            {

                if (_ampServiceLayer.DeleteSector(componentid, sectorcode, user))
                {
                    //return Redirect("Index");
                    return Json(new { success = true, response = "Successful message" });

                }
                return Json(new { success = true, response = "Successful message" });
            }
            catch (Exception exception)
            {
                errorengine.LogError(exception, "Delete Sector (POST)", user);
                throw;
            }

        }

        [HttpGet]
        public async Task<ActionResult> EditSectorCodes(string id)
        {
            //Get logon
            String user = GetEmpNo();
            try
            {


                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Component/EditSectorCodes";
                String Description = "Test EditSectorCodes Performance";

                //Start Timing
                From = DateTime.Now;


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                EditInputSectorsVM editInputSectorsVm = _ampServiceLayer.GetSectorsForEdit(id, user);

                //This approach is good for drop downs (Replaced with Bloodhound)
                //ViewBag.BudgetCentreID = new SelectList(componentdetailsviewmodel.BudgetCentre, "BudgetCentreID","BudgetCentreDescription");

                if (editInputSectorsVm == null)
                {
                    return HttpNotFound();
                }

                _ampServiceLayer.InsertLog("Component/Get EditSectorCodes", user, id);

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                return PartialView("_EditInputSectorCodes", editInputSectorsVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Component/EditSectorCodes", user);
                throw;
            }

        }

        [HttpPost]
        public async Task<String> UpdateInputSectorCodes(EditInputSectorsVM postedVm)
        {
            String user = GetEmpNo();
            if (postedVm.CompID == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }

            ModelStateWrapper modelStateWrapper = new ModelStateWrapper(this.ModelState);

            _ampServiceLayer.InsertLog("Component/POST EditSectorCodes", user, postedVm.CompID);


            if (_ampServiceLayer.UpdateSectorCodes(postedVm, modelStateWrapper, user))
            {
                return "Saved";
            }
            else
            {
               // string message = this.ModelState.Values.FirstOrDefault(x => x.Errors.Count > 0).Value.ToString();

                IEnumerable<ModelErrorCollection> errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0);

                var modelErrorCollection = errors.FirstOrDefault();
                if (modelErrorCollection != null)
                {
                    string error = modelErrorCollection[0].ErrorMessage;

                    if (error != null)
                    {
                        return error;
                    }
                    else
                    {
                        return "An Error occurred. Please try again.";
                    }
                }
                else
                {
                    return "An Error occurred. Please try again.";
                }
            }

        }


        public async Task<ActionResult> Finance(string id)
        {
            //Get logon
            String user = GetEmpNo();
            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Component/Finance";
                String Description = "Test Total Component Finance Code Performance";

                //Start Timing
                From = DateTime.Now;

           

                //Log user on page
                LogCall(user, "Component\\Finance", id);

                //Get ARIES API This is specifically for the AJAX GET on the javascript
                ViewBag.ARIESAPI = ConfigurationManager.AppSettings["ARIESRestServiceUrl"].ToString();
                ViewBag.FinancialYear = ConfigurationManager.AppSettings["FinancialYear"].ToString();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!await _ampServiceLayer.IsAuthorised(user))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ComponentFinanceVM componentFinanceVm = await _ampServiceLayer.GetComponentFinancials(id, user);


                if (componentFinanceVm == null)
                {
                    return HttpNotFound();
                }


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                //Sort
                componentFinanceVm.ComponentFinance = componentFinanceVm.ComponentFinance.OrderBy(s => s.Year);

                return View("Finance", componentFinanceVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Controller Action - Component/Finance", user);
                throw;
            }
        }

        // Component/CreateComponent/id
        public async Task<ActionResult> CreateComponent(string id)
        {
            //Get logon
            String user = GetEmpNo();

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Component/CreateComponent";
                String Description = "Test Total Create Component GET Code Performance";

                //Start Timing
                From = DateTime.Now;


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ComponentVM componentVm = await _ampServiceLayer.GetCreateComponent(id, user);


                //This approach is good for drop downs (Replaced with Bloodhound)
                //ViewBag.BudgetCentreID = new SelectList(componentdetailsviewmodel.BudgetCentre, "BudgetCentreID","BudgetCentreDescription");

                if (componentVm == null)
                {
                    return HttpNotFound();
                }



                _ampServiceLayer.InsertLog("Component/CreateGET", user, id);

                


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
                TempData["VM"] = componentVm;

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                //!!!!!!!!!!Hack remove Humanitarian Funding Mech
                List<FundingMech> mechs = componentVm.FundingMechs.ToList();

                FundingMech mech = mechs.Where(x => x.FundingMechID.Equals("HUMANITASSISTANCE")).FirstOrDefault();

                mechs.Remove(mech);

                componentVm.FundingMechs = mechs;

                //Bind the CCO Dropdown boxes
                ViewBag.FundingMechdd = new SelectList(componentVm.FundingMechs, "FundingMechID", "FundingMechDescription", componentVm.FundingMechanism);
                
                //Bind the Dropdown boxes 
                ViewBag.FundingArrangementdd = new SelectList(componentVm.FundingArrangements, "FundingArrangementValue", "FundingArrangementType", componentVm.FundingArrangementValue);

                //Bind the Partner Oragnisation Dropdown boxes 
                ViewBag.PartnerOrgdd = new SelectList(componentVm.PartnerOrganisations, "PartnerOrganisationValue", "PartnerOrganisationType", componentVm.PartnerOrganisationValue);



                return View(componentVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Component/CreateGET", user);
                throw;
            }
        }

        // POST: /Component/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComponent(ComponentVM componentVm, string FundingMechdd, string FundingArrangementdd, string PartnerOrgdd)
        {
            String user;

            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/CreatePOST";
            String Description = "Test Total Component create post Code Performance";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            user = GetEmpNo();

            componentVm.FundingMechanism = FundingMechdd;
            componentVm.FundingArrangementValue = FundingArrangementdd;
            componentVm.PartnerOrganisationValue = PartnerOrgdd;

            TempData["CreateComponentVM"] = componentVm;

            if (_ampServiceLayer.CreateComponent(componentVm, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";

                ViewBag.Success = "1";

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                _ampServiceLayer.InsertLog("Create Component (POST)", user, componentVm.ProjectID);

                return RedirectToAction("Edit", "Component", new { id = componentVm.ComponentID });
            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";
                ViewBag.Success = "0";

                componentVm = TempData["CreateComponentVM"] as ComponentVM;
                
                var OriginalVM = TempData["VM"] as ComponentVM;

                componentVm.FundingMechs = OriginalVM.FundingMechs;
                componentVm.FundingMechanism = FundingMechdd;

                ViewBag.FundingMechdd = new SelectList(OriginalVM.FundingMechs, "FundingMechID", "FundingMechDescription", FundingMechdd);

                //Bind the Dropdown boxes 
                ViewBag.FundingArrangementdd = new SelectList(OriginalVM.FundingArrangements, "FundingArrangementValue", "FundingArrangementType", FundingArrangementdd);

                //Bind the Partner Oragnisation Dropdown boxes 
                ViewBag.PartnerOrgdd = new SelectList(OriginalVM.PartnerOrganisations, "PartnerOrganisationValue", "PartnerOrganisationType", PartnerOrgdd);

                TempData["VM"] = componentVm;

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                return View(componentVm);
            }



        }


        // Component/Markers/id
        [HttpGet]
        public async Task<ActionResult> Markers(string id)
        {
            //Get logon
            String user = GetEmpNo();

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Component/Markers";
                String Description = "Test Total Component Markers Code Performance";

                //Start Timing
                From = DateTime.Now;


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ComponentMarkersVM componentMarkerVm = await _ampServiceLayer.GetComponentMarkers(id, user);


                if (componentMarkerVm == null)
                {
                    return HttpNotFound();
                }

                _ampServiceLayer.InsertLog("Component/Markers", user, id);

                //Bind the CCO Dropdown boxes
                ViewBag.BenefitingCountrydd = new SelectList(componentMarkerVm.BenefitingCountrys, "BenefitingCountryID", "BenefitingCountryDescription", componentMarkerVm.BenefitingCountry);

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

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                List<String> options = new List<String>();

                options.Add("Yes");
                options.Add("No");

                ViewBag.Options = options as IEnumerable<String>;

                return View(componentMarkerVm);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Component/Markers", user);
                throw;
            }
        }


        // POST: /Component/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Markers(ComponentMarkersVM componentMarkerVm, string BenefitingCountrydd)
        {
            String user;

            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/MarkersPOST";
            String Description = "Test Total Component post Markers Performance";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            user = GetEmpNo();

            componentMarkerVm.BenefitingCountry = BenefitingCountrydd;

            if (_ampServiceLayer.UpdateMarkers(componentMarkerVm, new ModelStateWrapper(this.ModelState), user))
            {
                // If modelState was valid, save will occur, set TempData to success code 1.
                TempData["Success"] = "1";
                ViewBag.Success = "1";

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                return RedirectToAction("Markers", "Component", new { id = componentMarkerVm.ComponentID });
             //           return RedirectToAction("Markers");
            }
            else
            {
                //Model was invalid set success code to 0
                TempData["Success"] = "0";
                ViewBag.Success = "0";

                var OriginalVM = TempData["VM"] as ComponentMarkersVM;
                TempData["VM"] = OriginalVM;

                componentMarkerVm = OriginalVM;

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

                return View(componentMarkerVm);
            }
        }

        #region Delivery Chain
        // Component/Partners/id
        [HttpGet]
        public async Task<ActionResult> DeliveryChain(string id)
        {
            //Get logon
            String user = GetEmpNo();

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Component/Partners";
                String Description = "Test Total Component Partners Code Performance";

                //Start Timing
                From = DateTime.Now;


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ComponentPartnerVM componentPartnerVM = await _ampServiceLayer.GetComponentDeliveryChains(id, user);


                if (componentPartnerVM == null)
                {
                    return HttpNotFound();
                }

                _ampServiceLayer.InsertLog("Component/Partners", user, id);

              

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

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

               

                return View(componentPartnerVM);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Component/partners", user);
                throw;
            }
        }

        // Component/Partners/id
        [HttpGet]
        public async Task<ActionResult> PartnersAlpha(string id)
        {
            //Get logon
            String user = GetEmpNo();

            try
            {

                //Setup Code Logging
                DateTime From;
                DateTime To;
                String MethodName = "Component/Partners";
                String Description = "Test Total Component Partners Code Performance";

                //Start Timing
                From = DateTime.Now;


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ComponentPartnerVM componentPartnerVM = await _ampServiceLayer.GetComponentDeliveryChains(id, user);


                if (componentPartnerVM == null)
                {
                    return HttpNotFound();
                }

                _ampServiceLayer.InsertLog("Component/Partners", user, id);



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

                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                //String MethodName, String Description, DateTime From, DateTime To, DateTime Result, String User);
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);



                return View(componentPartnerVM);
            }
            catch (Exception ex)
            {
                errorengine.LogError(ex, "Component/PartnersAlpha", user);
                throw;
            }
        }

        //***
        // For the search delivery partner screen - passing in the component ID, ID, ChildID and ParentID
        //on the loading of the screen from pressing the add button on the DeliveryChain.cshtml
        // also determining if adding a partner or replacing an existing partner on the chain
        [HttpGet]
        public async Task<ActionResult> SearchPartner(string id1, string id2, string id3, string id4, string id5)
        {
           // id1 = componentId, id2 = deliveryChainId, id3 = parentId, id5 = child id (we need this for replacing  first tier parnter check)
          //  id4 = addOrReplace - pass through if REPLACE, otherwise NULL so assign as ADD
   
            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (id4 != "REPLACE")
            {
                id4 = "ADD";}
            //Get logon
            string user = GetEmpNo();
            //Log user on page
            LogCall(user, "Search for a Partner (GET) " + id1, null);

            Int32 intId2;
            Int32 intId3;
            Int32 intId5;

            Int32.TryParse(id2, out intId2);
            Int32.TryParse(id3, out intId3);
            Int32.TryParse(id5, out intId5);


            AddPartnerToChainVM addPartnerToChainVm = await _ampServiceLayer.SetUpPartnerSearch(id1, intId2, intId3, id4, intId5);

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

            TempData["VM"] = addPartnerToChainVm;

            return View(addPartnerToChainVm);
        }
    
        [HttpPost]
        public async Task<ActionResult> SearchPartner(string searchString)
        {
       
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/SearchForAPartner";
            String Description = "Test  Search For A Partner (GET) time";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            String user = GetEmpNo();


            //Log user on page
            LogCall(user, "Component/SearchForAPartner");
            // perform a seach on the partner table based on search term entered 

            var OriginalVM = TempData["VM"] as AddPartnerToChainVM;
            TempData["VM"] = OriginalVM;
            AddPartnerToChainVM addPartnerToChainVm = OriginalVM;

            // first clean the text up 
            searchString = AMPUtilities.CleanCharsFromBeginningofText(searchString);

            AllReturnedPartnerListsVM searchPartnerResults = await _ampServiceLayer.LookUpPartnerSearchList(searchString);
            addPartnerToChainVm.SearchResults = searchPartnerResults;
            
  
            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);
            TempData["Success"] = "1";
            ViewBag.Success = "1";


            return View("SelectPartner", addPartnerToChainVm);
        }


        //***
        // For the  loading of the search first tier supplier screen from pressing the add first tier partners button on the DeliveryChain.cshtml 
        //Input: passing in the component ID 
        //Output: navigating to page with ComponentHeader populated

        [HttpGet]
        public async Task<ActionResult> SearchForAFirstTierPartner(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Get logon
            string user = GetEmpNo(); ;

            //Log user on page
            LogCall(user, "Search for a FIrst Tier Partner (GET) " + id, null);


            ComponentPartnerVM addNewfirstTier = new ComponentPartnerVM();


            ComponentHeaderVM compheader = await _ampServiceLayer.SetupSearchFirstTierPartnerComponentHeader(id);
            addNewfirstTier.ComponentHeaderVm = compheader;
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

            TempData["VM"] = addNewfirstTier;
            return View(addNewfirstTier);
        }

        [HttpPost]
        public async Task<ActionResult> SearchForFirstTierPartner(string searchString)
        {

            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/SearchForFirstTierPartner";
            String Description = "Test  Search For A First Tier Partner (GET) time";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            String user = GetEmpNo();


            //Log user on page
            LogCall(user, "Component/SearchForFirstTierPartner");

            var OriginalVM = TempData["VM"] as ComponentPartnerVM;
            TempData["VM"] = OriginalVM;
            ComponentPartnerVM compHeader = OriginalVM;

            // perform a seach  based on search term entered 

            AddPartnerToChainVM addNewFirstTierAddPartner = new AddPartnerToChainVM();
            
             // first clean the text up 
             searchString = AMPUtilities.CleanCharsFromBeginningofText(searchString);

            AllReturnedPartnerListsVM searchPartnerResults = await _ampServiceLayer.LookUpPartnerSearchList(searchString);
            addNewFirstTierAddPartner.SearchResults = searchPartnerResults;
            addNewFirstTierAddPartner.ComponentHeader = compHeader.ComponentHeaderVm;

            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);
            TempData["Success"] = "1";
            ViewBag.Success = "1";


            return View("SelectFirstTierPartner", addNewFirstTierAddPartner);
        }

        [HttpPost]
        public ActionResult SelectPartner(String id, AddPartnerToChainVM addPartnerToChainVm)
        {
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/SelectPartner";
            String Description = "Test Total Select Partner (GET) time";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            String user = GetEmpNo();


            //Log user on page
            LogCall(user, "Component/SelectPartner", id);

            _ampServiceLayer.InsertChain(addPartnerToChainVm.NewChainToBeAdded, new ModelStateWrapper(this.ModelState), user);

            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);


            return RedirectToAction("DeliveryChain");
        }

        // On loading of the add new partner screen
        [HttpPost]
        public async Task<ActionResult> AddNewPartner(AddPartnerToChainVM addPartnerToChainVm)
        {
               string user;
            //Get logon

            user = GetEmpNo();

            var OriginalVM = TempData["VM"] as AddPartnerToChainVM;
            TempData["VM"] = OriginalVM;

            addPartnerToChainVm.NewChainToBeAdded = OriginalVM.NewChainToBeAdded;
            addPartnerToChainVm.ChainToBeAddedTo = OriginalVM.ChainToBeAddedTo;
            addPartnerToChainVm.ComponentHeader = OriginalVM.ComponentHeader;

            return View("AddNewPartner", addPartnerToChainVm);
        }

        // On loading of the add new partner screen
        [HttpGet]
        public async Task<ActionResult> AddNewFirstTierPartner( )
        {
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/SelectPartner";
            String Description = "Test Total Create New First Tier Partner (GET) time";

            //Start Timing
            From = DateTime.Now;
            //Get logon

            string user = GetEmpNo();

            var OriginalVM = TempData["VM"] as ComponentPartnerVM;
            TempData["VM"] = OriginalVM;
            ComponentPartnerVM compHeader = OriginalVM;
            AddPartnerToChainVM addNewFirstTierAddPartner = new AddPartnerToChainVM();        
            addNewFirstTierAddPartner.ComponentHeader = compHeader.ComponentHeaderVm;

            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);
            TempData["Success"] = "1";
            ViewBag.Success = "1";


            return View("AddNewFirstTierPartner", addNewFirstTierAddPartner);
        }


        [HttpPost]
        public async Task<string> SaveNewFirstTierPartner(string componentId, string partnerName)
        {
            String user = GetEmpNo();
            if (componentId == null)
            {

            }

            if (await _ampServiceLayer.InsertNewFirstTierPartner(componentId, partnerName, user))
            {
                return "Saved";
            }
            else
            {
                return "Failed";
            }

        }
        [HttpGet]
        public async Task<ActionResult> CreatePartner(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/CreatePartner";
            String Description = "Test Total Create Partner (GET) time";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            user = GetEmpNo();


                //Log user on page
                LogCall(user, "Component/CreatePartner", id);

                DeliveryChainVM deliveryChainVm = new DeliveryChainVM();

                deliveryChainVm =  _ampServiceLayer.CreatePartner(id, user);


                //End Timing and Record
                To = DateTime.Now;
                TimeSpan Result = To - From;
                _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);             

                return PartialView("EditorTemplates/EditPartner", deliveryChainVm);
        }

        [HttpGet]
        public async Task<ActionResult> CreatePartnerData(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/CreatePartner";
            String Description = "Test Total Create Partner (GET) time";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            user = GetEmpNo();


            //Log user on page
            LogCall(user, "Component/CreatePartner", id);

            DeliveryChainVM DeliveryChainVm = new DeliveryChainVM();

            DeliveryChainVm = _ampServiceLayer.CreatePartner(id, user);


            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

            return  Json(DeliveryChainVm);
        }


        // Adding an existing DFID Registered Supplier as a First Tier
        [HttpPost]
        public async Task<string> AddFirstTierSupplier(string componentId, string supplierId)
        {
            String user = GetEmpNo();
            if (componentId == null || supplierId == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }

            if (await _ampServiceLayer.InsertFirstTierPartner(componentId,supplierId, user))
            {
                return "Saved";
            }
            else
            {
                return "Failed";
            }

        }

        // Adding an existing NON DFID Registered Partner as a First Tier

        [HttpPost]
        public async Task<string> AddFirstTierParter(string componentId, string supplierId)
        {
            String user = GetEmpNo();
            if (componentId == null || supplierId == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }

            if (await _ampServiceLayer.InsertFirstTierNonRegisteredPartner(componentId, supplierId, user))
            {
                return "Saved";
            }
            else
            {
                return "Failed";
            }

        }

        // Adding a new NON DFID Registered Partner as a First Tier

        [HttpPost]
        public async Task<string> AddNewFirstTierPartner(string componentId, string partnerName)
        {
            String user = GetEmpNo();
            if (componentId == null )
            {
                return HttpStatusCode.BadRequest.ToString();
            }

            if (await _ampServiceLayer.InsertNewFirstTierPartner(componentId, partnerName, user))
            {
                return "Saved";
            }
            else
            {
                return "Failed";
            }

        }
        public async Task<string> PostPartner(string partnerName, string existingType, string Id)
        {


            String user = GetEmpNo();
            AddPartnerToChainVM deliverychainvm = new AddPartnerToChainVM();

            var OriginalVM = TempData["VM"] as AddPartnerToChainVM;
            TempData["VM"] = OriginalVM;

            if (OriginalVM != null)
            {
                deliverychainvm.ComponentHeader = OriginalVM.ComponentHeader;
                deliverychainvm.ChainToBeAddedTo = OriginalVM.ChainToBeAddedTo;
                // we need to set the Partner or Supplier entry to true
                if (existingType == "P")
                {
                    deliverychainvm.ChainToBeAddedTo.PartnerEntry = "True";

                }
                else
                {
                    deliverychainvm.ChainToBeAddedTo.SupplierEntry = "True";

                }

                deliverychainvm.ChainToBeAddedTo.ChildName = partnerName;
                deliverychainvm.ChainToBeAddedTo.ChildID = Int32.Parse(Id);
                deliverychainvm.ChainToBeAddedTo.ComponentID = OriginalVM.ComponentHeader.ComponentID;
            }
         


        ModelStateWrapper modelStateWrapper = new ModelStateWrapper(this.ModelState);

            if (await _ampServiceLayer.InsertChain(deliverychainvm.ChainToBeAddedTo, modelStateWrapper, user))
            {
                return "Saved";
            }
            else
            {
                return "Failed";
            }

        }
        // Called when the user wants to replace a partner in the Delivery chain with another partenr
        // input: the current RowId and the new partner ID and Type that will replace the current child ID 
        // and type in the Row 
        public async Task<string> ReplacePartner(string existingType, string PartnerId, string Id)
        {


            String user = GetEmpNo();
           // AddPartnerToChainVM deliverychainvm = new AddPartnerToChainVM();
           // DeliveryChain deliverychainCurrentDetails = new DeliveryChain();
            DeliveryChain deliverychainReplaceDetails= new DeliveryChain();
            var OriginalVM = TempData["VM"] as AddPartnerToChainVM;
            TempData["VM"] = OriginalVM;

            if (OriginalVM != null)
            {

                // replacing the existing parent information with the newly selcted information
                // need to handle also check for first tier supplier - if ChainID Id = ParentNodeID and parent type = child type

                deliverychainReplaceDetails.ID = Int32.Parse(Id);
                deliverychainReplaceDetails.ChildID = Int32.Parse(PartnerId);
                deliverychainReplaceDetails.ChildType= existingType;
                deliverychainReplaceDetails.ComponentID = OriginalVM.ComponentHeader.ComponentID;

                // Check if chain to be deleted is a 1st tier partner
                if (
                    OriginalVM.ChainToBeAddedTo.ParentID == OriginalVM.ChainToBeAddedTo.ChildID)
                {
                    deliverychainReplaceDetails.ParentID = Int32.Parse(PartnerId); 
                    deliverychainReplaceDetails.ParentType =  existingType;
                    
                }
            }



            ModelStateWrapper modelStateWrapper = new ModelStateWrapper(this.ModelState);

            // now we just need to update the existing chain
            if (await _ampServiceLayer.ReplacePartnerInChain(deliverychainReplaceDetails, modelStateWrapper, user))
            {
                return "Saved";
            }
            else
            {
                return "Failed";
            }

        }


        [HttpPost]
      // This is used when a person adds a new partner to the delivery chain list
      // Creates new entry in the partner master table in AMP and then adds the info into the delivery chain
        public async Task<string> PostNewPartner(string NewPartnerName)
        {
            AddPartnerToChainVM deliverychainvm = new AddPartnerToChainVM();
            var OriginalVM = TempData["VM"] as AddPartnerToChainVM;
          

            String user = GetEmpNo();
           
            deliverychainvm.ComponentHeader = OriginalVM.ComponentHeader;
            deliverychainvm.ChainToBeAddedTo = OriginalVM.ChainToBeAddedTo;
           
            // first clean the text up 
            NewPartnerName = AMPUtilities.CleanCharsFromBeginningofText(NewPartnerName);

            deliverychainvm.ChainToBeAddedTo.NewChildName = NewPartnerName;
            deliverychainvm.ChainToBeAddedTo.ComponentID = OriginalVM.ComponentHeader.ComponentID;
            if (deliverychainvm.ComponentHeader.ComponentID == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }
            ModelStateWrapper modelStateWrapper = new ModelStateWrapper(this.ModelState);

            // if adding then do an insert new row in partner table and add to the chain for the component          
                if (await _ampServiceLayer.InsertChain(deliverychainvm.ChainToBeAddedTo, modelStateWrapper, user))

                {
                    return "Saved";

                }
                else
                {
                    return "Failed";
                }

        }

        [HttpPost]

        // This is used when a person adds a new partner to the delivery chain list and wants to use it to replace
        // an entry that is onteh delivery chain. Creates new entry in the partner master table in AMP and
        // then replaces the current info in the delivery chain with the new partner info
        public async Task<string> PostReplaceNewPartner(string NewPartnerName)
        {
            AddPartnerToChainVM deliverychainvm = new AddPartnerToChainVM();
            var OriginalVM = TempData["VM"] as AddPartnerToChainVM;


            String user = GetEmpNo();

            deliverychainvm.ComponentHeader = OriginalVM.ComponentHeader;
            deliverychainvm.ChainToBeAddedTo = OriginalVM.ChainToBeAddedTo;

            // first clean the text up 
            NewPartnerName = AMPUtilities.CleanCharsFromBeginningofText(NewPartnerName);

            deliverychainvm.ChainToBeAddedTo.NewChildName = NewPartnerName;
            deliverychainvm.ChainToBeAddedTo.ComponentID = OriginalVM.ComponentHeader.ComponentID;
            if (deliverychainvm.ComponentHeader.ComponentID == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }
            ModelStateWrapper modelStateWrapper = new ModelStateWrapper(this.ModelState);

            //  replacing - do an insert new row in partner table and replace with the entry currentl in the chain      
            if (await _ampServiceLayer.InsertNewPartnerAndReplaceExistingInChain(deliverychainvm.ChainToBeAddedTo, modelStateWrapper, user))

            {
                return "Saved";

            }
            else
            {
                return "Failed";
            }

        }

        // This deletes and promotes up the child partners
        [HttpPost]
        public async Task<string> DeleteChainAndPromoteChildren(string componentId, string chainid)
        {
            String user = GetEmpNo();
            if (componentId == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }

            if (_ampServiceLayer.DeletePartnerFromDeliveryChain(chainid, componentId, user))
            {
                return "Saved";
            }
            else
            {
                return "Failed";
            }

        }

        // This deletes and also deletes all the child partners
        [HttpPost]
        public async Task<string> DeleteChainAndDeleteChildren(string componentId, string supplierId)
        {
            String user = GetEmpNo();
            if (componentId == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }

            if (_ampServiceLayer.DeletePartnerAndChildrenFromDeliveryChain(supplierId, componentId, user))
            {
                return "Saved";
            }
            else
            {
                return "Failed";
            }

        }

        // This deletes and replaces with a different partnewr
        [HttpPost]
        public async Task<string> DeleteAndReplacePartnerInChain(string componentId, string supplierId)
        {
            String user = GetEmpNo();
            if (componentId == null)
            {
                return HttpStatusCode.BadRequest.ToString();
            }

            if (_ampServiceLayer.DeletePartnerFromDeliveryChain(supplierId, componentId, user))
            {
                return "Saved";
            }
            else
            {
                return "Failed";
            }

        }

        [HttpPost]
   
        //handles the two different types of delete options available
        public async Task<string> DeleteFromChainOptions(string Id, string supplierId, string componentId, string submitButton)
        {
            
            String user = GetEmpNo();

            switch (submitButton)
            {
               // case "1": //Replace
               //    return View("SearchPartner", componentId, Id, supplierId, user);
                    //return (await DeleteAndReplacePartnerInChain(componentId, Id));
                case "2": //Delete & delete all children
                    return (await DeleteChainAndDeleteChildren(componentId, Id));
                case "3":// Delete & Promote all Children
                    return (await DeleteChainAndPromoteChildren(componentId, Id));
                default:
                    return (await DeleteChainAndPromoteChildren(componentId, Id));
            }
        }


        // called when the delete modal form is displayed. Passing in the Chain Id
        [HttpGet]
        public async Task<ActionResult> DeletePartner(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String user;
            //Setup Code Logging
            DateTime From;
            DateTime To;
            String MethodName = "Component/DeletePartner";
            String Description = "Test Total Delete Partner (GET) time";

            //Start Timing
            From = DateTime.Now;

            //Get logon
            user = GetEmpNo();

            var OriginalVM = TempData["VM"] as DeliveryChainVM;
                TempData["VM"] = OriginalVM;

            //Log user on page
            LogCall(user, "Component/DeletePartner", id);

             DeliveryChainVM deliveryChainVm = new DeliveryChainVM();

            deliveryChainVm = await _ampServiceLayer.GetSpecificDeliveryChainRow(Int32.Parse(id));


            //End Timing and Record
            To = DateTime.Now;
            TimeSpan Result = To - From;
            _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

           

            return PartialView("EditorTemplates/DeletePartner", deliveryChainVm);
        }


    

        public async Task<ActionResult> RefreshPartnerList(string id)
        {

            String user = GetEmpNo();
            LogCall(user, "Component/RrefreshPartnerList", id);

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                DeliveryChainsVM deliveryChainsVm = new DeliveryChainsVM();
                deliveryChainsVm = await _ampServiceLayer.GetPartnerTableData(id, user);

                return PartialView("_DeliveryChainNestedList", deliveryChainsVm);

            }
            catch (Exception exception)
            {
                errorengine.LogError(id, exception, user);
                throw;
            }
        }

        #endregion

        // POST: /Component/Sectors/
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Partner(ComponentPartnerVM componentPartnerVM)
        //{

        //    String user;

        //    //Setup Code Logging
        //    DateTime From;
        //    DateTime To;
        //    String MethodName = "Component/Partner/POST";
        //    String Description = "Test Total Component Partner Performance";

        //    //Start Timing
        //    From = DateTime.Now;

        //    //Get logon
        //    user = GetEmpNo();


        //    // Access the Current VM from TempData (May be empty if this is first GET)
        //    var OriginalVM = TempData["VM"] as ComponentPartnerVM;

        //    // Check Model State if its valid then update else return with validation errors.
        //    //if (ModelState.IsValid)

        //    if (_ampServiceLayer.AddPartner(componentPartnerVM, new ModelStateWrapper(this.ModelState)))
        //    {

        //        //serviceLayer.AddSector(componentviewmodel,);
        //        // If modelState was valid, save will occur, set TempData to success code 1.
        //        TempData["Success"] = "1";

        //        //End Timing and Record
        //        To = DateTime.Now;
        //        TimeSpan Result = To - From;
        //        _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);

        //        return RedirectToAction("Partners");
        //    }
        //    else
        //    {
        //        //Model was invalid set success code to 0
        //        TempData["Success"] = "0";
        //        ViewBag.Success = "0";

        //        // Set TempData to current view model.
        //        TempData["VM"] = OriginalVM;

        //        //return View(componentviewmodel);
        //        //Return the original view model with validation errors.

        //        //End Timing and Record
        //        To = DateTime.Now;
        //        TimeSpan Result = To - From;
        //        _ampServiceLayer.InsertCodeLog(MethodName, Description, From, To, Result, user);


        //        return View(OriginalVM);
        //    }

        //}

       

        #region Lookup ActionResults - used by typeahead controls within View objects

        public ActionResult BudgetCentreLookup()
        {
            List<BudgetCentreKV> BudgetCentreList = _ampServiceLayer.LookupBudgetCentreKV();

            return Json(BudgetCentreList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InputSectorCodeLookUp(String id)
        {
            List<InputSectorKV> InputSectorCodeList = _ampServiceLayer.LookupInputSectorKV(id);

            return Json(InputSectorCodeList, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> PartnerLookUp(String id)
        {

            List<DeliveryChainListVM> DeliveryChainVms = await _ampServiceLayer.LookUpPartners(id, "028984");

          
            return Json(DeliveryChainVms, JsonRequestBehavior.AllowGet);
        }


        //**
        public async Task<ActionResult> PartnerSearchLookUp(String searchString)
        {

           AllReturnedPartnerListsVM partnerListVms =   await _ampServiceLayer.LookUpPartnerSearchList(searchString);


            return Json(partnerListVms, JsonRequestBehavior.AllowGet);
        }

        //
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult PartnerListLookUp()
        {

            List<PartnerMaster> partnervm = _ampServiceLayer.LookUpPartnerList();


            return Json(partnervm, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PartnerListSearchLookUp()
        {

            List<PartnerMaster> partnervm = _ampServiceLayer.LookUpPartnerList();


            return Json(partnervm, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
