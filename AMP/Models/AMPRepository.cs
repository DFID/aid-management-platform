using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using AMP.ViewModels;
using System.Data.Entity.Validation;
using AMP.ARIESModels;
using AMP.Utilities;


namespace AMP.Models
{
    /// <summary>
    ///  ProjectRepository provides access to the AMP database. 
    /// </summary>
    /// <remarks>
    /// ProjectRepository uses AMPEntities which is an Entity Framework DBContext object and allows access to the AMP database using LINQ methods.
    /// ProjectRepository is accessed through the interface IProjectRepository which enforces loose coupling.
    /// </remarks>
    public class AMPRepository : IAMPRepository, IDisposable
    {

        //Connection for ARIES Sync
        private static String ARIES_con = ConfigurationManager.ConnectionStrings["ARIESConnectionString"].ToString();


        // Create an instance of AMP Entities
        private AMPEntities ampEntities = new AMPEntities();


        //New Instance of the ErrorEngine
        //Utilities.ErrorEngine errorengine = new Utilities.ErrorEngine();

        #region Project Repository Methods

        ///<summary>GetProjects - Gets all projects from the database.</summary>
        ///<remarks>Returns all projects as ProjectMaster objects.</remarks>
        public IEnumerable<ProjectMaster> GetProjects(String SearchString, String user)
        {
            //return ampEntities.ProjectMasters.ToList();

            // lets say a string was passed "International"

            //String SearchString = "International";

            if (SearchString == null)
            {
                //Return all projects
                //return ampEntities.ProjectMasters.ToList();

                // Find Projects where user is inputter (Fix migration remove "R")
                // IEnumerable<Team> team = ampEntities.Teams.Where(x => x.TeamID == "R"+user && x.EndDate > DateTime.Today); 


                //Find Emp no
                IEnumerable<UserLookUp> users = ampEntities.UserLookUps.Where(x => x.UserID == user);



                // Advice was that this could be inefficent. If we have performance problems we may need to revisit. The use of the following was advised:
                ////ListB.Where(x => FilterListA.Contains(x.ClassA));
                // x is ProjectMaster, y is Portfolios, x.Portfolio makes the join?
                //return ampEntities.ProjectMasters.Where(x => x.Portfolios.Any(y => y.UserID == CurrentUser));

                DateTime archiveFilterDate = new DateTime(2015, 07, 1);

                //This version checks Portfolio + if you are the inputter and also return only active projects for the person ( status = "A")                
                return
                    ampEntities.ProjectMasters.Where(
                        x => x.Portfolios.Any(y => y.UserID == user) && x.Stage != Constants.ArchiveStage ||
                             (x.Teams.Any(q => q.TeamID == user && q.Status == "A") && x.Stage != Constants.ArchiveStage &&
                              !((x.Stage == Constants.CompletionStage && !x.ProjectDate.ActualEndDate.HasValue) ||
                                (x.Stage == Constants.CompletionStage &&
                                 x.ProjectDate.ActualEndDate.Value < archiveFilterDate) ||
                                (x.Stage == Constants.CompletionStage && x.LastUpdate.Value < archiveFilterDate))));
                    // UserID replaced with Inputter is wrong. This needs to go to resource table.

                //  return ampEntities.ProjectMasters.Where(x => x.Portfolios.Any(y => y.UserID == CurrentUser) || (x.InputterID == userid)); 

                //This version worked but since moving inputter ID to the resources table you need to do a different call
                //  return ampEntities.ProjectMasters.Where(x => x.Portfolios.Any(y => y.UserID == user) || (x.UserID == user && x.Status =="A")); 
            }
            else
            {
                // Keyword search string exists, checked description and ProjectID
                return ampEntities.ProjectMasters.Where(x => x.Title.ToUpper().Contains(SearchString.ToUpper())
                                                             || x.ProjectID.ToUpper().Contains(SearchString.ToUpper()));
            }
            // FiltereddashboardVM = FiltereddashboardVM.Where(x => x.ProjectDescription.ToUpper().Contains(searchString.ToUpper())) as PagedList<UserProjectsViewModel>;
        }

        ///<summary>GetProjects - Gets all projects of the given org unit from the database.</summary>
        ///<remarks>Returns all projects as ProjectMaster objects.</remarks>
        /// 
        /// 
        public IEnumerable<ProjectMaster> GetProjects(String SearchString, String user, string orgUnit)
        {
            IEnumerable<ProjectMaster> projectPortfolioAndTeams;
            IEnumerable<ProjectMaster> projectInOwnOrgUnit;
            IEnumerable<ProjectMaster> mergedResult;

            if (SearchString == null)
            {
                IEnumerable<UserLookUp> users = ampEntities.UserLookUps.Where(x => x.UserID == user);
                projectPortfolioAndTeams =
                    ampEntities.ProjectMasters.Where(
                        x =>
                            x.Portfolios.Any(y => y.UserID == user) ||
                            (x.Teams.Any(q => q.TeamID == user && q.Status == "A")) && x.Stage != "7");
                    // UserID replaced with Inputter is wrong. This needs to go to resource table.
                projectInOwnOrgUnit =
                    ampEntities.ProjectMasters.Where(x => x.ProjectBudgetCentreOrgUnits.Any(y => y.OrgUnit == orgUnit));
                var merged = projectInOwnOrgUnit.Concat(projectPortfolioAndTeams);
                merged = merged.OrderBy(x => x.ProjectID).ToList();
                merged = merged.Distinct();
                mergedResult = (IEnumerable<ProjectMaster>) merged;

                return mergedResult; //return merged result                
            }
            else
            {
                // Keyword search string exists, checked description and ProjectID
                return ampEntities.ProjectMasters.Where(x => x.Title.ToUpper().Contains(SearchString.ToUpper())
                                                             || x.ProjectID.ToUpper().Contains(SearchString.ToUpper()));
            }

        }

        public IQueryable<ProjectMaster> GetProjectsAdvanceSearch(String SearchString, String StageID,
            string StageChoice, string BenefittingCountryCode, string BudgetCentre, string SRO)
        {
            IQueryable<ProjectMaster> AllProjects;
            if (StageChoice == "All")
            {
                if (SearchString != null || StageID != null || BenefittingCountryCode != null || BudgetCentre != null ||
                    SRO != null)
                {
                    AllProjects = ampEntities.ProjectMasters;
                    if (!string.IsNullOrEmpty(SearchString))
                    {
                        AllProjects =
                            ampEntities.ProjectMasters.Where(
                                x =>
                                    x.Description.ToUpper().Contains(SearchString.ToUpper()) ||
                                    x.ProjectID.Contains(SearchString) ||
                                    x.Title.ToUpper().Contains(SearchString.ToUpper()));
                    }
                    if (!string.IsNullOrEmpty(StageID))
                    {
                        AllProjects = AllProjects.Where(x => x.Stage.Contains(StageID));
                    }
                    if (!string.IsNullOrEmpty(BenefittingCountryCode))
                    {
                        AllProjects =
                            AllProjects.Where(
                                x => x.ComponentMasters.Any(y => y.BenefittingCountry == BenefittingCountryCode));
                    }
                    if (!string.IsNullOrEmpty(BudgetCentre))
                    {
                        AllProjects = AllProjects.Where(x => x.BudgetCentreID.Contains(BudgetCentre));
                    }
                    if (!string.IsNullOrEmpty(SRO))
                    {
                        AllProjects =
                            AllProjects.Where(
                                x => x.Teams.Any(y => y.TeamID.Contains(SRO) && y.RoleID == "SRO" && y.Status == "A"));
                    }
                    return AllProjects;
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

        ///<summary>GetProjects - Get All Projects, skipping <skip> and taking <take> records </summary>
        ///<param name="skip">The number of projects to skip before taking</param>
        ///<param name="take">The number of projects to take.</param>
        ///<remarks>Method has been designed for use in handling paged lists.</remarks>
        public IEnumerable<ProjectMaster> GetProjects(int skip, int take)
        {
            return ampEntities.ProjectMasters.OrderBy(p => p.ProjectID).Skip(skip).Take(take).ToList();
        }

        /// <summary> GetProject - Gets a single project from the database</summary>
        /// <param name="id">The ID of the project to return. Should be a six digit number</param>
        /// <returns>A ProjectMaster object</returns>
        public ProjectMaster GetProject(String id)
        {
            ProjectMaster project = ampEntities.ProjectMasters.Find(id);


            return project;
        }

        //public IEnumerable<vDashboardProject> DashboardProjects(string userId)
        //{
        //    return ampEntities.vDashboardProjects.Where(x => x.UserId.Equals(userId));
        //} 

        public BudgetCentre GetBudgetCentre(string budgetCentreId)
        {
            BudgetCentre budgetCentre = ampEntities.BudgetCentres.Find(budgetCentreId);
            return budgetCentre;
        }

        public ProjectDate GetProjectDates(String id)
        {
            ProjectDate project = ampEntities.ProjectDates.Find(id);

            return project;
        }

        //pass advance search parameters 

        public void CloseProject(ProjectMaster project, ProjectDate projectdate, Performance projectPerformance,
            String user)
        {

            ampEntities.Entry(project).State = System.Data.Entity.EntityState.Modified;
            ampEntities.Entry(projectdate).State = System.Data.Entity.EntityState.Modified;
            ampEntities.Entry(projectPerformance).State = System.Data.Entity.EntityState.Modified;

            //Execute Stored Procedure to Close Project in ARIES.

            if (AMPUtilities.ARIESUpdateEnabled() == "true")
            {
                ARIESPlannerMovement(project.ProjectID, user, "Close");
                CloseARIESProject(project.ProjectID, user);
            }

        }

        #region Team Methods

        /// <summary> GetTeam - Gets a list of team members</summary>
        /// <param name="id">The ID of the project to return. Should be a six digit number</param>
        /// <returns>A Team object</returns>
        public IEnumerable<Team> GetTeam(String id)
        {
            IEnumerable<Team> team = ampEntities.Teams.Where(x => x.ProjectID.Equals(id) && x.Status == "A");

            return team;
        }

        /// <summary>UpdateTeam - Update an existing team record</summary>
        /// <param name="team">A Team object</param>
        public void UpdateTeam(Team team)
        {
            ampEntities.Entry(team).State = System.Data.Entity.EntityState.Modified;

            UpdateARIESTeam(team);
        }

        public void EndTeamMember(Team team, bool runPCMX)
        {
            ampEntities.Entry(team).State = System.Data.Entity.EntityState.Modified;

            EndARIESTeam(team, runPCMX);
        }

        ///<summary>InsertTeam - Inserts a new Team object into the database</summary>
        ///<param name="team">A Team object</param>
        public void InsertTeam(Team team)
        {
            //Update AMP
            ampEntities.Teams.Add(team);

            //Update ARIES
            CreateARIESTeam(team);
        }

        ///<summary>GetTeamMember - Get a Team Member from the database</summary>
        ///<param name="id">value of the id column in the team table (not to be confused with project id or staff id) </param>
        public Team GetTeamMember(Int32 id)
        {
            return ampEntities.Teams.FirstOrDefault(x => x.ID.Equals(id));
        }

        /// <summary>
        /// ActiveRoleExists - Check that a role is actively filled on a project
        /// </summary>
        /// <param name="roleId">The role ID</param>
        /// <param name="projectId">The project ID</param>
        /// <returns>true or false</returns>
        public bool ActiveRoleExists(string roleId, string projectId)
        {
            return ampEntities.Teams.Any(x => x.RoleID == roleId && x.Status == "A" && x.ProjectID == projectId);
        }


        public void CreateARIESTeam(Team team)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ARIES_con))
                {
                    //conn.Open();

                    //// 1.  create a command object identifying the stored procedure
                    //SqlCommand cmd = new SqlCommand("AMP.uspAddTeamMember", conn);

                    //// 2. set the command object so it knows to execute a stored procedure
                    //cmd.CommandType = CommandType.StoredProcedure;

                    //DateTime ARIESEndDate = new DateTime(2099, 12, 31);

                    //// 3. add parameter to command, which will be passed to the stored procedure
                    //cmd.Parameters.Add(new SqlParameter("@ProjectID", team.ProjectID));
                    //cmd.Parameters.Add(new SqlParameter("@StaffID", team.TeamID));
                    //cmd.Parameters.Add(new SqlParameter("@RoleID", team.RoleID));
                    //cmd.Parameters.Add(new SqlParameter("@StartDate", team.StartDate));
                    //cmd.Parameters.Add(new SqlParameter("@EndDate", ARIESEndDate));
                    //cmd.Parameters.Add(new SqlParameter("@User", team.UserID));

                    //// execute the command
                    //SqlDataReader rdr = cmd.ExecuteReader();

                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public void UpdateARIESProjectForApproval(ProjectMaster projectmaster)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ARIES_con))
                {
                    //conn.Open();

                    //// 1.  create a command object identifying the stored procedure
                    //SqlCommand cmd = new SqlCommand("AMP.UpdateProject", conn);

                    //// 2. set the command object so it knows to execute a stored procedure
                    //cmd.CommandType = CommandType.StoredProcedure;

                    //DateTime ARIESEndDate = new DateTime(2099, 12, 31);

                    //// 3. add parameter to command, which will be passed to the stored procedure
                    //cmd.Parameters.Add(new SqlParameter("@ProjectID", projectmaster.ProjectID));
                    //cmd.Parameters.Add(new SqlParameter("@Stage", projectmaster.Stage));
                    //cmd.Parameters.Add(new SqlParameter("@User", projectmaster.UserID));

                    //// execute the command
                    //SqlDataReader rdr = cmd.ExecuteReader();

                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public void CloseARIESProject(String ProjectID, String user)
        {
            using (SqlConnection conn = new SqlConnection(ARIES_con))
            {
                //conn.Open();

                //// 1.  create a command object identifying the stored procedure
                //SqlCommand cmd = new SqlCommand("AMP.uspCloseProject", conn);

                //// 2. set the command object so it knows to execute a stored procedure
                //cmd.CommandType = CommandType.StoredProcedure;

                //// 3. add parameter to command, which will be passed to the stored procedure
                //cmd.Parameters.Add(new SqlParameter("@ProjectID", ProjectID));
                //cmd.Parameters.Add(new SqlParameter("@User", user));

                //// execute the command
                //SqlDataReader rdr = cmd.ExecuteReader();

            }
        }

        public void ARIESPlannerMovement(String ProjectID, String user, String Type)
        {
            using (SqlConnection conn = new SqlConnection(ARIES_con))
            {
                //conn.Open();

                //// 1.  create a command object identifying the stored procedure
                //SqlCommand cmd = new SqlCommand("AMP.uspPlannerMovement", conn);

                //// 2. set the command object so it knows to execute a stored procedure
                //cmd.CommandType = CommandType.StoredProcedure;

                //// 3. add parameter to command, which will be passed to the stored procedure
                //cmd.Parameters.Add(new SqlParameter("@ProjectID", ProjectID));
                //cmd.Parameters.Add(new SqlParameter("@User", user));
                //cmd.Parameters.Add(new SqlParameter("@Type", Type));

                //// execute the command
                //SqlDataReader rdr = cmd.ExecuteReader();

            }
        }

        public void UpdateARIESTeam(Team team)
        {
            using (SqlConnection conn = new SqlConnection(ARIES_con))
            {
                //conn.Open();

                //// 1.  create a command object identifying the stored procedure
                //SqlCommand cmd = new SqlCommand("AMP.uspEditTeamMember", conn);

                //// 2. set the command object so it knows to execute a stored procedure
                //cmd.CommandType = CommandType.StoredProcedure;

                //DateTime ARIESEndDate = new DateTime(2099, 12, 31);

                //// 3. add parameter to command, which will be passed to the stored procedure
                //cmd.Parameters.Add(new SqlParameter("@ProjectID", team.ProjectID));
                //cmd.Parameters.Add(new SqlParameter("@StaffID", team.TeamID));
                //cmd.Parameters.Add(new SqlParameter("@RoleID", team.RoleID));
                //cmd.Parameters.Add(new SqlParameter("@StartDate", team.StartDate));
                //cmd.Parameters.Add(new SqlParameter("@EndDate", ARIESEndDate));
                //cmd.Parameters.Add(new SqlParameter("@User", team.UserID));

                //// execute the command
                //SqlDataReader rdr = cmd.ExecuteReader();

            }
        }


        public void EndARIESTeam(Team team, bool runPCMX)
        {

            using (SqlConnection conn = new SqlConnection(ARIES_con))
            {
                //conn.Open();

                //// 1.  create a command object identifying the stored procedure
                //SqlCommand cmd = new SqlCommand("AMP.uspEndTeamMember", conn);

                //// 2. set the command object so it knows to execute a stored procedure
                //cmd.CommandType = CommandType.StoredProcedure;

                ////3. Determine whether this call will trigger a PCMX. If called as part of an Update, call PCMX when inserting the new record.
                //Int16 bitValue = 0;
                //if (runPCMX)
                //{
                //    bitValue = 1;
                //}

                //// 4. add parameter to command, which will be passed to the stored procedure
                //cmd.Parameters.Add(new SqlParameter("@ProjectID", team.ProjectID));
                //cmd.Parameters.Add(new SqlParameter("@StaffID", team.TeamID));
                //cmd.Parameters.Add(new SqlParameter("@RoleID", team.RoleID));
                //cmd.Parameters.Add(new SqlParameter("@StartDate", team.StartDate));
                //cmd.Parameters.Add(new SqlParameter("@EndDate", team.EndDate));
                //cmd.Parameters.Add(new SqlParameter("@User", team.UserID));
                //cmd.Parameters.Add(new SqlParameter("@RunPCMX", bitValue));
                //// execute the command
                //SqlDataReader rdr = cmd.ExecuteReader();

            }
        }

        #endregion

        ///<summary>GetProjectRoles - Gets a list of Active Project Roles from the database</summary>
        public IEnumerable<ProjectRole> GetProjectRoles()
        {
            return ampEntities.ProjectRoles.Where(x => x.Status == "A");
        }


        /// <summary>
        /// GetReviews - Gets a list of of ReviewMasters for a project. Also gets the ReviewARScore, ReviewPCRScore and ReviewOutput objects.
        /// </summary>
        /// <param name="id">The ID of the project to return. Should be a 6 digit number</param>
        /// <returns>A list of Review Masters and child objects (e.g. AR & PCR Score objects and Review Outputs </returns>

        public IEnumerable<ReviewMaster> GetReviews(String id, string user)
        {
            IEnumerable<ReviewMaster> reviews = ampEntities.ReviewMasters.Where(x => x.ProjectID.Equals(id));
            return reviews;
        }

        /// <summary>
        /// GetReview - Gets a single ReviewMaster for a project. 
        /// </summary>
        /// <returns>A Review Master </returns>

        public ReviewMaster GetReview(string projectId, int reviewId)
        {
            ReviewMaster review =
                ampEntities.ReviewMasters.FirstOrDefault(
                    x => x.ProjectID.Equals(projectId) && x.ReviewID.Equals(reviewId));
            return review;
        }


        public List<ReviewOutput> GetReviewScores(string projectId, int reviewId)
        {
            List<ReviewOutput> reviewScores =
                ampEntities.ReviewOutputs.Where(x => x.ProjectID == projectId && x.ReviewID == reviewId).ToList();
            return reviewScores;
        }


        public int GetNextReviewID(string projectId)
        {

            //Get all projects output
            List<ReviewMaster> reviewMasters = ampEntities.ReviewMasters.Where(m => m.ProjectID == projectId).ToList();

            int nextReviewID;

            if (reviewMasters.Any())
            {
                //Get max output Id and add 1 
                nextReviewID = (int) (reviewMasters.Max(r => r.ReviewID) + 1);
            }
            else
            {
                nextReviewID = 0;
            }

            return nextReviewID;
        }


        public Evaluation GetProjectEvaluations(String id)
        {
            return ampEntities.Evaluations.Where(x => x.ProjectID.Equals(id)).FirstOrDefault();
            // return evaluation;
        }

        public Performance GetProjectPerformance(string projectId)
        {
            return ampEntities.Performances.Where(x => x.ProjectID.Equals(projectId)).FirstOrDefault();
            // return performance;
        }

        public void UpdateProjectPerformance(Performance performance)
        {
            ampEntities.Entry(performance).State = System.Data.Entity.EntityState.Modified;
        }



        /// <summary>
        /// Get all of the Evaluation Documents for an evaluation ID
        /// </summary>
        /// <param name="evaluationId">Evaluation ID (integer).</param>
        /// <returns>IEnumerable of EvaluationDocument objects</returns>
        public IEnumerable<EvaluationDocument> GetEvaluationDocuments(Int32 evaluationId)
        {
            return ampEntities.EvaluationDocuments.Where(x => x.EvaluationID == evaluationId);
        }

        /// <summary>
        /// Insert an Evaluation Document into the AMP database. the Evaluation document is a document number and description.
        /// Best considered as a reference to a document, not actually a bit stream of a word doc or other document type.
        /// </summary>
        /// <param name="evaluationDocument">Populated EvaluationDocument model object.</param>
        public void InsertEvaluationDocument(EvaluationDocument evaluationDocument)
        {
            try
            {
                ampEntities.EvaluationDocuments.Add(evaluationDocument);

                //Insert into ARIES.
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Delete an Evaluation document from the AMP Database. 
        /// </summary>
        /// <param name="EvaluationID">Unique ID of the evaluation</param>
        /// <param name="DocumentID">Document ID</param>
        public void DeleteEvaluationDocument(Int32 EvaluationID, string DocumentID)
        {
            EvaluationDocument documentToDelete = new EvaluationDocument();
            documentToDelete =
                ampEntities.EvaluationDocuments.FirstOrDefault(
                    x => x.DocumentID == DocumentID && x.EvaluationID == EvaluationID);
            ampEntities.EvaluationDocuments.Remove(documentToDelete);
        }

        /// <summary>
        /// Get a populated Evaluation Model, lookup by EvaluationID rather than ProjectID
        /// </summary>
        /// <param name="evaluationId">The unique ID of the evaluation. Integer number, not a string.</param>
        /// <returns>Populated Evaluation Model</returns>
        public Evaluation GetEvaludationById(Int32 evaluationId)
        {
            Evaluation evaluation = ampEntities.Evaluations.Find(evaluationId);

            return evaluation;
        }

        /// <summary>
        /// Update an existing Evaluation record.
        /// </summary>
        /// <param name="evaluation">Populated Evaluation object</param>
        public void UpdateProjectEvaluation(Evaluation evaluation)
        {
            ampEntities.Entry(evaluation).State = System.Data.Entity.EntityState.Modified;

            UpdateARIESEvaluation(evaluation);


        }


        public void UpdateARIESEvaluation(Evaluation evaluation)
        {
            //I dont think ARIES likes a blank budget.
            Decimal? budget;
            string evaluationType;
            string evaluationManagement;
            DateTime? StartDate;
            DateTime? EndDate;

            if (evaluation.EvaluationTypeID == "5")
            {
                budget = 0;
                evaluationType = "5";
                evaluationManagement = "";
                StartDate = new DateTime(1900, 01, 01);
                EndDate = new DateTime(1900, 01, 01);
            }
            else
            {
                budget = evaluation.EstimatedBudget;
                evaluationType = evaluation.EvaluationTypeID;
                evaluationManagement = evaluation.ManagementOfEvaluation;
                StartDate = evaluation.StartDate;
                EndDate = evaluation.EndDate;
            }
            //Dont allow a null Additional Information to be written to ARIES, it will probably crash AMP.
            if (evaluation.AdditionalInfo == null)
            {
                evaluation.AdditionalInfo = "";
            }
            using (SqlConnection conn = new SqlConnection(ARIES_con))
            {
            //    conn.Open();

                // 1.  create a command object identifying the stored procedure
               // SqlCommand cmd = new SqlCommand("AMP.uspUpdateEvaluation", conn);

                // 2. set the command object so it knows to execute a stored procedure
                //cmd.CommandType = CommandType.StoredProcedure;

                //// 3. add parameter to command, which will be passed to the stored procedure
                //cmd.Parameters.Add(new SqlParameter("@ProjectID", evaluation.ProjectID));
                //cmd.Parameters.Add(new SqlParameter("@EvaluationType", evaluationType));
                //cmd.Parameters.Add(new SqlParameter("@EvaluationManagement", evaluationManagement));
                //cmd.Parameters.Add(new SqlParameter("@Budget", budget));
                //cmd.Parameters.Add(new SqlParameter("@StartDate", StartDate));
                //cmd.Parameters.Add(new SqlParameter("@EndDate", EndDate));
                //cmd.Parameters.Add(new SqlParameter("@AdditionalInformation", evaluation.AdditionalInfo));
                //cmd.Parameters.Add(new SqlParameter("@User", evaluation.UserID));

                //// execute the command
                //SqlDataReader rdr = cmd.ExecuteReader();

            }

        }


        /// <summary>
        /// Get Project Info -
        /// </summary>
        /// <param name="id">The ID of the project. Should be a six digit number</param>
        /// <returns>ProjectInfo object</returns>
        public ProjectInfo GetProjectInfo(String id)
        {
            return ampEntities.ProjectInfoes.Find(id);
        }

        public IEnumerable<AuditedFinancialStatement> GetActiveAuditedStatements(String id)
        {

            IEnumerable<AuditedFinancialStatement> audits =
                ampEntities.AuditedFinancialStatements.Where(x => x.ProjectID == id && x.Status == "A");

            //List<AuditedFinancialStatement> list = audits as List<AuditedFinancialStatement>;

            return audits;
        }

        public IEnumerable<AuditedFinancialStatement> GetAllAuditedStatements(string id)
        {

            IEnumerable<AuditedFinancialStatement> audits =
                ampEntities.AuditedFinancialStatements.Where(x => x.ProjectID == id);

            return audits;

        }


        public AuditedFinancialStatement GetAuditedStatement(string projectid, int statementid)
        {
            AuditedFinancialStatement auditedStatement =
                ampEntities.AuditedFinancialStatements.FirstOrDefault(
                    x => x.ProjectID == projectid && x.StatementID == statementid);

            return auditedStatement;
        }


        public void InsertStatement(AuditedFinancialStatement statement)
        {
            try
            {
                ampEntities.AuditedFinancialStatements.Add(statement);

                //** no longer need to pass this to stored procedure for saving to ARIES
                // AddARIESStatement(statement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertPlannedEndDate(ProjectPlannedEndDate plannedEndDate)
        {
            try
            {
                ampEntities.ProjectPlannedEndDates.Add(plannedEndDate);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertOverallRiskRating(OverallRiskRating overallRiskRating)
        {
            try
            {
                ampEntities.OverallRiskRatings.Add(overallRiskRating);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateStatement(AuditedFinancialStatement statementToUpdate)
        {
            ampEntities.Entry(statementToUpdate).State = System.Data.Entity.EntityState.Modified;

            DeleteARIESStatement(statementToUpdate);
        }

        public void AddARIESStatement(AuditedFinancialStatement statement)
        {
            using (SqlConnection conn = new SqlConnection(ARIES_con))
            {
                //conn.Open();

                //// 1.  create a command object identifying the stored procedure
                //SqlCommand cmd = new SqlCommand("AMP.uspAddStatement", conn);

                //// 2. set the command object so it knows to execute a stored procedure
                //cmd.CommandType = CommandType.StoredProcedure;

                //// 3. add parameter to command, which will be passed to the stored procedure
                //cmd.Parameters.Add(new SqlParameter("@ProjectID", statement.ProjectID));


                ////cmd.Parameters.Add(new SqlParameter("@DueDate", statement.DueDate));
                ////cmd.Parameters.Add(new SqlParameter("@PromptDate", statement.PromptDate));
                //cmd.Parameters.Add(new SqlParameter("@ReceivedDate", statement.ReceivedDate));
                //cmd.Parameters.Add(new SqlParameter("@PeriodFrom", statement.PeriodFrom));
                //cmd.Parameters.Add(new SqlParameter("@PeriodTo", statement.PeriodTo));

                //cmd.Parameters.Add(new SqlParameter("@Value", statement.Value ?? 0));
                //cmd.Parameters.Add(new SqlParameter("@Currency", statement.Currency ?? ""));
                //cmd.Parameters.Add(new SqlParameter("@StatementType", statement.StatementType));
                //cmd.Parameters.Add(new SqlParameter("@reason_action", statement.reason_action ?? ""));

                //cmd.Parameters.Add(new SqlParameter("@User", statement.UserID));


                //// execute the command
                //SqlDataReader rdr = cmd.ExecuteReader();

            }
        }

        public void DeleteARIESStatement(AuditedFinancialStatement statement)
        {
            using (SqlConnection conn = new SqlConnection(ARIES_con))
            {
                //conn.Open();

                //// 1.  create a command object identifying the stored procedure
                //SqlCommand cmd = new SqlCommand("AMP.uspDeleteStatement", conn);

                //// 2. set the command object so it knows to execute a stored procedure
                //cmd.CommandType = CommandType.StoredProcedure;

                //// 3. add parameter to command, which will be passed to the stored procedure
                //cmd.Parameters.Add(new SqlParameter("@ProjectID", statement.ProjectID));
                //cmd.Parameters.Add(new SqlParameter("@StatementID", statement.StatementID));
                //cmd.Parameters.Add(new SqlParameter("@User", statement.UserID));

                //// execute the command
                //SqlDataReader rdr = cmd.ExecuteReader();

            }
        }

        /// <summary> GetRisk - Gets a list of Risk entries</summary>


        ///<summary>InsertProject - Inserts a new ProjectMaster object into the database</summary>
        ///<param name="project">A ProjectMaster object</param>
        public void InsertProject(ProjectMaster project)
        {
            ManageARIESProject(project);
            ampEntities.ProjectMasters.Add(project);
        }

        /// <summary>UpdateProject - Update an existing project record</summary>
        /// <param name="project">A project Master object</param>
        public void UpdateProject(ProjectMaster projectdetails)
        {
            ManageARIESProject(projectdetails);
            ampEntities.Entry(projectdetails).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>UpdateProjectInfo - Update an existing Project Info record</summary>
        /// <param name="project">A ProjectInfo object</param>
        public void UpdateProjectInfo(ProjectInfo projectInfo)
        {
            ampEntities.Entry(projectInfo).State = System.Data.Entity.EntityState.Modified;

            UpdateARIESTeamMarker(projectInfo);
        }

        public void UpdateProjectDates(ProjectDate projectDate)
        {
            ampEntities.Entry(projectDate).State = System.Data.Entity.EntityState.Modified;
        }

        public void UpdateARIESTeamMarker(ProjectInfo projectInfo)
        {
            //Null team marker causes an error. Make it blank instead.
            if (projectInfo.TeamMarker == null)
            {
                projectInfo.TeamMarker = "";
            }

            using (SqlConnection conn = new SqlConnection(ARIES_con))
            {
                //conn.Open();

                //// 1.  create a command object identifying the stored procedure
                //SqlCommand cmd = new SqlCommand("AMP.uspUpdateTeamMarker", conn);

                //// 2. set the command object so it knows to execute a stored procedure
                //cmd.CommandType = CommandType.StoredProcedure;

                //// 3. add parameter to command, which will be passed to the stored procedure
                //cmd.Parameters.Add(new SqlParameter("@ProjectID", projectInfo.ProjectID));
                //cmd.Parameters.Add(new SqlParameter("@TeamMarker", projectInfo.TeamMarker));
                //cmd.Parameters.Add(new SqlParameter("@User", projectInfo.UserID));

                //// execute the command
                //SqlDataReader rdr = cmd.ExecuteReader();

            }
        }


        /// <summary>InsertError - Inserts an error entry for every catch senario</summary>
        /// <param name="project">A ErrorLog object</param>
        public void InsertError(ErrorLog errorlog)
        {
            ampEntities.ErrorLogs.Add(errorlog);
        }

        /// <summary>InsertLogging - Inserts an log entry for every page access</summary>
        /// <param name="project">A ErrorLog object</param>
        public void InsertLog(Logging logging)
        {
            ampEntities.Loggings.Add(logging);
        }

        /// <summary>InsertCodeLogging - Inserts an log entry for Method with code logging enabled </summary>
        /// <param name="project">A CodePerformance object</param>
        public void InsertCodeLog(CodePerformance codePerformance)
        {
            ampEntities.CodePerformances.Add(codePerformance);
        }

        /// <summary>
        /// NextProjectID - Get the next project ID in the project sequence.
        /// </summary>
        /// <returns>nextProjectID - a six digit string, all numeric</returns>
        public string NextProjectID()
        {
            string maxProjectID = ampEntities.ProjectMasters.Max(p => p.ProjectID);
            string nextProjectID;

            if (Convert.ToInt32(maxProjectID) < 300000)
            {
                nextProjectID = "300000";
            }
            else
            {
                nextProjectID = Convert.ToString((Convert.ToInt32(maxProjectID) + 1));
            }
            return nextProjectID;

        }

        public ExemptionReason GetSingleExemptionReason(string ExemptionId, string ReviewType)
        {
            try
            {
                int expid = Convert.ToInt16(ExemptionId);
                return
                    ampEntities.ExemptionReasons.Where(x => x.ExemptionID == expid && x.ExemptionType == ReviewType)
                        .SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>Save - commits a change to a project</summary>
        ///<remarks>Save is called in the service layer immeadiately after InsertProject or UpdateProject are called.</remarks>
        public void Save()
        {
            try
            {
                ampEntities.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
                // FA -
                // If you are debugging and you hit an entity framework validation error, you probably cant see the actual error message from drilling down the exception.
                // Add this to watch ((System.Data.Entity.Validation.DbEntityValidationException)$exception).EntityValidationErrors.First().ValidationErrors.First().ErrorMessage
            }

        }

        /// <summary>
        /// Insert a new Risk into the Risk table in AMP Database. 
        /// </summary>
        /// <param name="newDocument">A new Risk, details populated in the AMP ServiceLayer</param>

        public void AddProject(Portfolio portfolio)
        {
            try
            {
                ampEntities.Portfolios.Add(portfolio);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Portfolio GetProjectPortfolio(String ProjectID, String userName)
        {
            try
            {
                return
                    ampEntities.Portfolios.Where(x => x.ProjectID.Equals(ProjectID) && x.UserID.Equals(userName))
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get performance


        public IEnumerable<Performance> GetPortfolioPerformance(IEnumerable<Portfolio> portfolios)
        {
            //return ariesApiEntities.ProjectApprovedBudgets.Where(x => projects.Contains(x.ProjectID)).ToList();

            //IEnumerable<String> roles = projectTeamVm.Select(x => x.RoleId).Distinct();

            IEnumerable<String> listofProjects = portfolios.Select(x => x.ProjectID);

            return ampEntities.Performances.Where(x => listofProjects.Contains(x.ProjectID));

        }

        public void RemoveProject(Portfolio portfolio)
        {
            try
            {
                ampEntities.Portfolios.Remove(portfolio);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<ExemptionReason> GetExemptionReasons()
        {
            return ampEntities.ExemptionReasons.Where(x => x.Status == "A").ToList();
        }

        public List<Stage> GetStages()
        {
            return ampEntities.Stages.Where(x => x.Status == "A").ToList();
        }

        public List<ReviewDocument> GetReviewDocuments(string projectId, int reviewId)
        {
            return
                ampEntities.ReviewDocuments.Where(x => x.ProjectID.Equals(projectId) && x.ReviewID.Equals(reviewId))
                    .ToList();
        }

        //GetRiskRegisters
        public List<RiskDocument> GetRiskRegisters(string projectId)
        {
            return ampEntities.RiskDocuments.Where(x => x.ProjectID.Equals(projectId)).ToList();
        }

        //GetOverallRiskRatings
        public List<OverallRiskRating> GetOverallRiskRatings(string projectId)
        {
            return
                ampEntities.OverallRiskRatings.Where(x => x.ProjectID.Equals(projectId))
                    .OrderByDescending(x => x.LastUpdated)
                    .ToList();
        }

        public OverallRiskRating GetOverallRiskRating(string projectId)
        {
            IEnumerable<OverallRiskRating> overallRiskRating =
                ampEntities.OverallRiskRatings.Where(x => x.ProjectID.Equals(projectId));
            if (overallRiskRating.Count() != 0)
            {
                int maxId = overallRiskRating.Max(x => x.OverallRiskRatingId);
                return
                    ampEntities.OverallRiskRatings.Where(
                        x => x.ProjectID.Equals(projectId) && x.OverallRiskRatingId == maxId).FirstOrDefault();
            }
            else
            {
                return null;
            }

        }

        public void UpdateARPCRBasicInfo(Performance newPerformanceBasic)
        {
            try
            {
                Performance existingPerformance = ampEntities.Performances.Find(newPerformanceBasic.ProjectID);
                if (existingPerformance == null)
                {
                    ampEntities.Performances.Add(newPerformanceBasic);
                }
                else
                {
                    ampEntities.Performances.Remove(existingPerformance);
                    ampEntities.Performances.Add(newPerformanceBasic);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // Update AR PCR Due date - Performance 
        public void UpdatePerformance(Performance newPerformanceBasic)
        {
            ampEntities.Entry(newPerformanceBasic).State = System.Data.Entity.EntityState.Modified;
        }

        public void InsertReview(ReviewMaster reviewMaster)
        {
            ampEntities.ReviewMasters.Add(reviewMaster);
        }

        public void UpdateReview(ReviewMaster reviewMaster)
        {
            ampEntities.Entry(reviewMaster).State = System.Data.Entity.EntityState.Modified;
        }

        //Get review deferral 
        public ReviewDeferral GetReviewDeferral(string ProjectID, int reviewId)
        {
            return ampEntities.ReviewDeferrals.FirstOrDefault(x => x.ProjectID == ProjectID && x.ReviewID == reviewId);
        }

        //Insert ReviewDeferral
        public void InsertReviewDeferral(ReviewDeferral reviewDeferral)
        {
            ampEntities.ReviewDeferrals.Add(reviewDeferral);
        }

        //Update Review Deferral 
        public void UpdateReviewDeferral(ReviewDeferral newReviewDeferral)
        {
            ampEntities.Entry(newReviewDeferral).State = System.Data.Entity.EntityState.Modified;
        }

        public void DeleteReviewDeferral(int deferralId)
        {
            ReviewDeferral reviewDeferral = new ReviewDeferral();
            reviewDeferral = ampEntities.ReviewDeferrals.FirstOrDefault(x => x.DeferralID == deferralId);
            ampEntities.ReviewDeferrals.Remove(reviewDeferral);
        }

        public void DeleteReviewExemption(ReviewExemption reviewExemption)
        {
            ReviewExemption reviewExemptions = new ReviewExemption();
            reviewExemptions =
                ampEntities.ReviewExemptions.FirstOrDefault(
                    x => x.ProjectID == reviewExemption.ProjectID && x.ExemptionType == reviewExemption.ExemptionType);
            ampEntities.ReviewExemptions.Remove(reviewExemption);
        }

        public ReviewExemption GetReviewExemption(string ProjectID, string ExemptionType) //For AR only 
        {
            return
                ampEntities.ReviewExemptions.FirstOrDefault(
                    x => x.ProjectID == ProjectID && x.ExemptionType == ExemptionType);
        }

        public void InsertReviewExemption(ReviewExemption reviewExemption)
        {
            ampEntities.ReviewExemptions.Add(reviewExemption);
        }

        public void UpdateReviewExemption(ReviewExemption reviewExemption)
        {
            ampEntities.Entry(reviewExemption).State = System.Data.Entity.EntityState.Modified;
        }


        /// <summary>
        /// Insert a reveiw Document into the AMP database. the Review document is a document number and description.
        /// Best considered as a reference to a document, not actually a bit stream of a word doc or other document type.
        /// </summary>
        public void InsertReviewDocument(ReviewDocument reviewDocument)
        {
            try
            {
                ampEntities.ReviewDocuments.Add(reviewDocument);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void InsertRiskDocuments(RiskDocument riskRegister)
        {
            try
            {
                ampEntities.RiskDocuments.Add(riskRegister);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Delete a Review document from the AMP Database. 
        /// </summary>
        /// <param name="reviewDocumentId">Unique ID of the Review Document</param>

        public void DeleteReviewDocument(int reviewDocumentId)
        {
            ReviewDocument documentToDelete = new ReviewDocument();
            documentToDelete = ampEntities.ReviewDocuments.FirstOrDefault(x => x.ReviewDocumentsID == reviewDocumentId);
            ampEntities.ReviewDocuments.Remove(documentToDelete);
        }

        //DeleteRiskDocument
        public void DeleteRiskDocument(string docId, string projectId)
        {
            //RiskRegister documentToDelete = new RiskRegister();
            RiskDocument documentToDelete = new RiskDocument();
            documentToDelete =
                ampEntities.RiskDocuments.FirstOrDefault(x => x.DocumentID == docId && x.ProjectID == projectId);
            ampEntities.RiskDocuments.Remove(documentToDelete);
        }

        public void InsertReviewOutput(ReviewOutput reviewOutput)
        {
            ampEntities.ReviewOutputs.Add(reviewOutput);
        }

        public void RemoveReviewOutput(ReviewOutput reviewOutput)
        {
            ampEntities.ReviewOutputs.Remove(reviewOutput);
        }

        public void EditReviewOutput(ReviewOutput reveiwOutput)
        {
            ampEntities.Entry(reveiwOutput).State = System.Data.Entity.EntityState.Modified;
        }


        public ReviewOutput GetReviewOutput(string projectId, int reviewId, int outputId)
        {
            try
            {
                return
                    ampEntities.ReviewOutputs.FirstOrDefault(
                        x => x.ProjectID.Equals(projectId) && x.ReviewID.Equals(reviewId) && x.OutputID.Equals(outputId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //GetReview Total sum of weights
        public int? GetReviewOutputsWeightSum(string projectId, int reviewId, int outputId)
        {
            int? reviewOutputsSum =
                ampEntities.ReviewOutputs.Where(
                    x => x.ProjectID.Equals(projectId) && x.ReviewID.Equals(reviewId) && x.OutputID != outputId)
                    .Sum(x => x.Weight);
            return reviewOutputsSum;
        }

        //
        public IEnumerable<ReviewOutput> GetReviewOutputs(string projectId, int reviewId)
        {
            IEnumerable<ReviewOutput> reviewOutputs =
                ampEntities.ReviewOutputs.Where(x => x.ProjectID.Equals(projectId) && x.ReviewID.Equals(reviewId));
            return reviewOutputs;
        }

        //reviewoutput count 
        public int GetReviewOutputsCount(string projectId, int reviewId)
        {
            int reviewOutputsCount =
                ampEntities.ReviewOutputs.Where((x => x.ProjectID.Equals(projectId) && x.ReviewID.Equals(reviewId)))
                    .Count();
            return reviewOutputsCount;
        }

        public ReviewARScore GetReviewARScore(string projectId, int reviewId)
        {
            try
            {
                return
                    ampEntities.ReviewARScores.FirstOrDefault(
                        x => x.ProjectID.Equals(projectId) && x.ReviewID.Equals(reviewId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertReviewARScore(ReviewARScore reviewArScore)
        {
            ampEntities.ReviewARScores.Add(reviewArScore);
        }

        public void UpdateReviewARScore(ReviewARScore reviewArScore)
        {
            ampEntities.Entry(reviewArScore).State = System.Data.Entity.EntityState.Modified;
        }

        public ReviewPCRScore GetReviewPCRScore(string projectId, int reviewId)
        {
            try
            {
                return
                    ampEntities.ReviewPCRScores.FirstOrDefault(
                        x => x.ProjectID.Equals(projectId) && x.ReviewID.Equals(reviewId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertReviewPCRScore(ReviewPCRScore reviewpcrScore)
        {
            ampEntities.ReviewPCRScores.Add(reviewpcrScore);
        }

        public void UpdateReviewPCRScore(ReviewPCRScore reviewpcrScore)
        {
            ampEntities.Entry(reviewpcrScore).State = System.Data.Entity.EntityState.Modified;
        }

        public void DeleteReviewARScore(ReviewARScore ReviewARScore)
        {
            ampEntities.ReviewARScores.Remove(ReviewARScore);
        }

        public void DeleteReviewPCRScore(ReviewPCRScore reviewPCRScore)
        {
            ampEntities.ReviewPCRScores.Remove(reviewPCRScore);
        }

        public void DeleteReviewOutputs(IEnumerable<ReviewOutput> reviewOutputs)
        {
            ampEntities.ReviewOutputs.RemoveRange(reviewOutputs);
        }

        public void DeleteReviewMaster(ReviewMaster reviewMaster)
        {
            ampEntities.ReviewMasters.Remove(reviewMaster);
        }

        public void DeleteReviewDocuments(IEnumerable<ReviewDocument> reviewDocuments)
        {
            ampEntities.ReviewDocuments.RemoveRange(reviewDocuments);
        }

        public Markers1 GetProjectMarkers(String id)
        {
            return ampEntities.Markers1.Find(id);
        }

        public void UpdateProjectMarkers(Markers1 marker)
        {
            ampEntities.Entry(marker).State = System.Data.Entity.EntityState.Modified;
        }

        public void AddProjectMarkers(Markers1 marker)
        {
            ampEntities.Markers1.Add(marker);
        }

        /// <summary>
        /// Project Planned End Dates
        /// </summary>
        /// <param name="ProjectID"></param>

        //check and return the active row for the project planned end date
        public ProjectPlannedEndDate GetProjectPlannedEndDate(String id)
        {
            ProjectPlannedEndDate projectPlannedEndDate =
                ampEntities.ProjectPlannedEndDates.FirstOrDefault(x => x.ProjectID.Equals(id) && x.Status == "A");

            return projectPlannedEndDate;
        }

        // check to see if there are existing rows for planned end date with a status of A and if there are close them off
        public void CloseOffExistingActiveProjectPlannedEndDate(String id)
        {

            List<ProjectPlannedEndDate> projectPlannedEndDateExistingList =
                ampEntities.ProjectPlannedEndDates.Where(a => a.ProjectID.Equals(id) && a.Status == "A").ToList();

            if (projectPlannedEndDateExistingList.Count > 0)
            {
                // rows have been returned so we need to loop through them and update status to Closed
                foreach (var returned in projectPlannedEndDateExistingList)
                {
                    returned.Status = "C";
                    UpdatePlannedEndDate(returned);
                }

            }

        }

        //check and return the planned end date details for a specific workflow to show on the history view for workflow
        // haceto use == here rather than Equals as dealing with integers
        public ProjectPlannedEndDate GetProjectPlannedEndDatForWorkflowHistory(int wfTaskID)
        {
            return ampEntities.ProjectPlannedEndDates.FirstOrDefault(x => x.WorkTaskID == wfTaskID);

        }

        public void UpdatePlannedEndDate(ProjectPlannedEndDate projectplannededndate)
        {

            ampEntities.Entry(projectplannededndate).State = System.Data.Entity.EntityState.Modified;
        }

        //public void InsertBudgetApproval(BudgetApprovalValue budgetApproval)
        //{
        //    ampEntities.BudgetApprovalValues.Add(budgetApproval);
        //}

        //public BudgetApprovalValue GetBudgetApprovedByWorkflow(Int32 workflowId , Int32 workflowStepId )
        //{
        //    return ampEntities.BudgetApprovalValues.FirstOrDefault(x => x.WorkflowID == workflowId && x.WorkFlowStepID == workflowStepId);
        //}



        #endregion

        #region Risk Register Methods

        public RiskRegister GetRiskItem(int ID)
        {
            return ampEntities.RiskRegisters.Find(ID);
        }

        public OverallRiskRating GetOverallRisk(int overallRiskId)
        {
            return ampEntities.OverallRiskRatings.Find(overallRiskId);
        }

        public void InsertRiskItem(RiskRegister riskItem)
        {
            ampEntities.RiskRegisters.Add(riskItem);
        }

        public void UpdateRiskItem(RiskRegister riskItem)
        {
            ampEntities.Entry(riskItem).State = System.Data.Entity.EntityState.Modified;
        }

        public IEnumerable<RiskRegister> GetRiskRegister(string ProjectId)
        {
            return
                ampEntities.RiskRegisters.Where(x => x.ProjectID == ProjectId)
                    .OrderByDescending(ent => ent.ResidualRisk)
                    .ThenBy(ent => ent.RiskID);
        }

        public IEnumerable<RiskCategory> GetRiskCategories()
        {
            return ampEntities.RiskCategories;
        }

        public IEnumerable<RiskLikelihood> GetRiskLikelihoods()
        {
            return ampEntities.RiskLikelihoods;
        }

        public IEnumerable<RiskImpact> GetRiskImpacts()
        {
            return ampEntities.RiskImpacts;
        }

        public IEnumerable<Risk> GetRisks()
        {
            return ampEntities.Risks;
        }

        public IEnumerable<RiskDocument> GetRiskDocuments(string projectId)
        {
            return ampEntities.RiskDocuments.Where(x => x.ProjectID == projectId);
        }


        #endregion

        #region Components

        public IEnumerable<ComponentMaster> GetComponents(String ProjectID)
        {

            //return ampEntities.ComponentMasters.Where(x => x.ProjectID.Equals(ProjectID));
            return ampEntities.ComponentMasters.Where(x => x.ProjectID.Equals(ProjectID));

            //This version checks Portfolio + if you are the inputter and also return only active projects for the person ( status = "A")
            //return ampEntities.ProjectMasters.Where(x => x.Portfolios.Any(y => y.UserID == CurrentUser) || (x.UserID == userid && x.Status == "A")); // UserID replaced with Inputter is wrong. This needs to go to resource table.
            //  return ampEntities.ProjectMasters.Where(x => x.Portfolios.Any(y => y.UserID == CurrentUser) || (x.InputterID == userid)); 


            // FiltereddashboardVM = FiltereddashboardVM.Where(x => x.ProjectDescription.ToUpper().Contains(searchString.ToUpper())) as PagedList<UserProjectsViewModel>;
        }

        public IEnumerable<ComponentMaster> GetComponentsUsingComponentSubString(String ProjectID)
        {
            return ampEntities.ComponentMasters.Where(x => x.ComponentID.Substring(0, 6).Equals(ProjectID));

        }

        public ComponentMaster GetComponent(String id)
        {
            ComponentMaster component = ampEntities.ComponentMasters.Find(id);

            return component;
        }

        public ComponentDate GetComponentDates(String id)
        {
            ComponentDate componentdate = ampEntities.ComponentDates.Find(id);

            return componentdate;
        }

        public IEnumerable<ImplementingOrganisation> GetImplementingOrg(String id)
        {
            IEnumerable<ImplementingOrganisation> imporg =
                ampEntities.ImplementingOrganisations.Where(x => x.ComponentID.Equals(id));

            return imporg;
        }

        public IEnumerable<InputSectorCode> GetInputSectors(String id)
        {
            return ampEntities.InputSectorCodes.Where(x => x.ComponentID.Equals(id));
        }

        public InputSectorCode GetInputSector(String componentid, int sectorcode)
        {
            string sectorString = sectorcode.ToString();

            IEnumerable<InputSectorCode> sectors =
                ampEntities.InputSectorCodes.Where(x => x.ComponentID.Equals(componentid));

            InputSectorCode sector = sectors.Where(x => x.InputSectorCode1.Equals(sectorString)).FirstOrDefault();

            return sector;
        }

        /// <summary>UpdateProject - Update an existing project record</summary>
        /// <param name="project">A project Master object</param>
        public void UpdateComponent(ComponentMaster componentmaster, ComponentDate componentdate)
        {
            try
            {
                if (AMPUtilities.ARIESUpdateEnabled() == "true")
                {
                    UpdateARIESComponent(componentmaster, componentdate);
                }
                ampEntities.Entry(componentmaster).State = System.Data.Entity.EntityState.Modified;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Update Component - Update an existing project record</summary>
        /// <param name="project">A project Master object</param>
        public void CreateComponent(ComponentMaster componentmaster, ComponentDate componentdate)
        {
            //  ampEntities.Entry(componentmaster).State = System.Data.Entity.EntityState.Modified;
            try
            {
                if (AMPUtilities.ARIESUpdateEnabled() == "true")
                {
                    CreateARIESComponent(componentmaster, componentdate);
                }
                ampEntities.ComponentMasters.Add(componentmaster);
            }
            catch
            {
                throw;
            }

        }

        public void CreateComponentDate(ComponentDate componentdate)
        {
            //  ampEntities.Entry(componentmaster).State = System.Data.Entity.EntityState.Modified;
            ampEntities.ComponentDates.Add(componentdate);
        }


        public void CreateARIESComponent(ComponentMaster componenet, ComponentDate componentdate)
        {
            try
            {
                ProjectMaster project = GetProject(componenet.ProjectID);

                Team inputter = project.Teams.Where(x => x.RoleID.Equals("PI") && x.Status == "A").FirstOrDefault();

                String Inputter = inputter.TeamID;

                using (SqlConnection conn = new SqlConnection(ARIES_con))
                {
                    //conn.Open();

                    //// 1.  create a command object identifying the stored procedure
                    //SqlCommand cmd = new SqlCommand("AMP.uspCreateComponent", conn);

                    //// 2. set the command object so it knows to execute a stored procedure
                    //cmd.CommandType = CommandType.StoredProcedure;

                    //// 3. add parameter to command, which will be passed to the stored procedure
                    //cmd.Parameters.Add(new SqlParameter("@ProjectID", componenet.ProjectID));
                    //cmd.Parameters.Add(new SqlParameter("@ComponentID", componenet.ComponentID));
                    //cmd.Parameters.Add(new SqlParameter("@FinancialStartDate", componentdate.OperationalStartDate));
                    //cmd.Parameters.Add(new SqlParameter("@FinancialEndDate", componentdate.EndDate));
                    //cmd.Parameters.Add(new SqlParameter("@BudgetCentreID", componenet.BudgetCentreID));
                    //cmd.Parameters.Add(new SqlParameter("@Description", componenet.ComponentDescription));
                    //cmd.Parameters.Add(new SqlParameter("@FundingMech", componenet.FundingMechanism));
                    //cmd.Parameters.Add(new SqlParameter("@User", componenet.UserID));
                    //cmd.Parameters.Add(new SqlParameter("@AdminApprover", componenet.AdminApprover));
                    //cmd.Parameters.Add(new SqlParameter("@Inputter", Inputter));

                    //// execute the command
                    //SqlDataReader rdr = cmd.ExecuteReader();

                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public void UpdateARIESComponent(ComponentMaster component, ComponentDate componentdate)
        {
            try
            {
                ProjectMaster project = GetProject(component.ProjectID);

                Team inputter = project.Teams.Where(x => x.RoleID.Equals("PI") && x.Status == "A").FirstOrDefault();

                String Inputter = inputter.TeamID;

                if (component.AdminApprover == null)
                {
                    component.AdminApprover = "";
                }

                using (SqlConnection conn = new SqlConnection(ARIES_con))
                {
                    //conn.Open();
                    //// 1.  create a command object identifying the stored procedure
                    //SqlCommand cmd = new SqlCommand("AMP.uspEditComponent", conn);
                    //// 2. set the command object so it knows to execute a stored procedure
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //// 3. add parameter to command, which will be passed to the stored procedure
                    //cmd.Parameters.Add(new SqlParameter("@ProjectID", component.ProjectID));
                    //cmd.Parameters.Add(new SqlParameter("@ComponentID", component.ComponentID));
                    //cmd.Parameters.Add(new SqlParameter("@FinancialStartDate", componentdate.StartDate));
                    //cmd.Parameters.Add(new SqlParameter("@FinancialEndDate", componentdate.EndDate));
                    //cmd.Parameters.Add(new SqlParameter("@BudgetCentreID", component.BudgetCentreID));
                    //cmd.Parameters.Add(new SqlParameter("@Description", component.ComponentDescription));
                    //cmd.Parameters.Add(new SqlParameter("@FundingMech", component.FundingMechanism));
                    //cmd.Parameters.Add(new SqlParameter("@User", component.UserID));
                    //cmd.Parameters.Add(new SqlParameter("@AdminApprover", component.AdminApprover));
                    //cmd.Parameters.Add(new SqlParameter("@Inputter", Inputter));

                    //// execute the command
                    //SqlDataReader rdr = cmd.ExecuteReader();

                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public void ManageARIESProject(ProjectMaster projectMaster)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(ARIES_con))
                {
                    //conn.Open();

                    //// 1.  create a command object identifying the stored procedure
                    //SqlCommand cmd = new SqlCommand("AMP.uspManageProject", conn);

                    //// 2. set the command object so it knows to execute a stored procedure
                    //cmd.CommandType = CommandType.StoredProcedure;

                    //// 3. add parameter to command, which will be passed to the stored procedure
                    //cmd.Parameters.Add(new SqlParameter("@ProjectID", projectMaster.ProjectID));

                    //cmd.Parameters.Add(new SqlParameter("@FinancialStartDate",
                    //    projectMaster.ProjectDate.OperationalStartDate));
                    //cmd.Parameters.Add(new SqlParameter("@FinancialEndDate",
                    //    projectMaster.ProjectDate.OperationalEndDate));
                    //cmd.Parameters.Add(new SqlParameter("@BudgetCentreID", projectMaster.BudgetCentreID));
                    //cmd.Parameters.Add(new SqlParameter("@Description", projectMaster.Title));
                    ////cmd.Parameters.Add(new SqlParameter("@FundingMech", componenet.FundingMechanism));
                    //cmd.Parameters.Add(new SqlParameter("@User", projectMaster.UserID));
                    ////cmd.Parameters.Add(new SqlParameter("@AdminApprover", componenet.AdminApprover));
                    ////cmd.Parameters.Add(new SqlParameter("@Inputter", Inputter));

                    //// execute the command
                    //SqlDataReader rdr = cmd.ExecuteReader();

                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }


        /// <summary>UpdateProject - Update an existing project record</summary>
        /// <param name="project">A project Master object</param>



        public void AddInputSector(InputSectorCode newinputsector)
        {
            ampEntities.InputSectorCodes.Add(newinputsector);
        }

        public void DeleteSector(InputSectorCode sector)
        {
            ampEntities.InputSectorCodes.Remove(sector);
        }

        public void InsertSectors(List<InputSectorCode> sectorCodes)
        {
            ampEntities.InputSectorCodes.AddRange(sectorCodes);
        }

        public void DeleteSectors(List<InputSectorCode> sectorCodes)
        {
            ampEntities.InputSectorCodes.RemoveRange(sectorCodes);
        }



        public void UpdateMarker(Marker marker)
        {
            ampEntities.Entry(marker).State = System.Data.Entity.EntityState.Modified;
        }

        public void AddMarker(Marker marker)
        {
            ampEntities.Markers.Add(marker);
        }

        //Delivery Chain

        public List<DeliveryChain> GetDeliveryChainsByComponent(string componentId)
        {
            List<DeliveryChain> deliveryChains =
                ampEntities.DeliveryChains.Where(x => x.ComponentID == componentId && x.Status == "A").ToList();

            return deliveryChains;
        }

        public DeliveryChain GetDeliveryChain(Int32 id)
        {
            return ampEntities.DeliveryChains.Find(id);
        }

        public void InsertDeliveryChain(DeliveryChain deliveryChain)
        {
            ampEntities.DeliveryChains.Add(deliveryChain);
        }

        public void UpdateDeliveryChain(DeliveryChain deliveryChain)
        {
            ampEntities.Entry(deliveryChain).State = System.Data.Entity.EntityState.Modified;
        }
        
        public void ReplacePartnerInDeliveryChain(DeliveryChain deliveryChain)
        {
            DeliveryChain chaintoupdateChain = ampEntities.DeliveryChains.FirstOrDefault(x => x.ID == deliveryChain.ID);
            chaintoupdateChain.ChildID = deliveryChain.ChildID;
            chaintoupdateChain.ChildType = deliveryChain.ChildType;

            // need to also amend the parentid and type if this is a first tier partner replacement
            if (deliveryChain.ChildID == deliveryChain.ParentID)
            {
                chaintoupdateChain.ParentID = deliveryChain.ChildID;
                chaintoupdateChain.ParentType = deliveryChain.ChildType;
            }

            ampEntities.Entry(chaintoupdateChain).State = System.Data.Entity.EntityState.Modified;
        }
        public void DeleteDeliveryChain(DeliveryChain deliveryChain)
        {
            ampEntities.DeliveryChains.Remove(deliveryChain);
        }

        public int NextPartnerID()
        {
            //Risky as this is the anticipated next id. An other process may beat this code.
            //Instead should we commit partner master first then read to find the ID by name?
            IEnumerable<PartnerMaster> partnerMasters = ampEntities.PartnerMasters;
            if (partnerMasters.Any())
            {
                Int32 maxPartner = ampEntities.PartnerMasters.Max(p => p.PartnerID);
                return (maxPartner + 1);
            }
            else
            {
                return 1;
            }

        }

        public List<PartnerMaster> GetDeliveryChainsByIDList(List<int> iDs)
        {
            List<PartnerMaster> PartnerMasters =
                ampEntities.PartnerMasters.Where(x => iDs.Contains(x.PartnerID)).ToList();

            return PartnerMasters;
        }

        //Partners

        public void InsertPartner(PartnerMaster partnerMaster)
        {
            ampEntities.PartnerMasters.Add(partnerMaster);
        }

        public DeliveryChain GetDeliveryChainThatMatchesDeliveryChainVm(DeliveryChain deliveryChain)
        {
            List<DeliveryChain> chains =
                ampEntities.DeliveryChains.Where(x => x.ComponentID == deliveryChain.ComponentID).ToList();

            DeliveryChain matchedChain = new DeliveryChain();

            foreach (DeliveryChain chain in chains)
            {
                if (chain.ChainID == deliveryChain.ChainID && chain.ParentID == deliveryChain.ParentID &&
                    chain.ParentType == deliveryChain.ParentType && chain.ChildID == deliveryChain.ChildID &&
                    chain.ChildType == deliveryChain.ChildType && chain.UserID == deliveryChain.UserID &&
                    chain.Status == deliveryChain.Status && chain.ParentNodeID == deliveryChain.ParentNodeID)
                {
                    matchedChain = chain;
                    break;
                }
            }
            return matchedChain;
        }


        public string GetPartnerName(int id)
        {
            PartnerMaster partnerMaster = ampEntities.PartnerMasters.FirstOrDefault(x => x.PartnerID == id);
            return partnerMaster.PartnerName;
        }

        #endregion

        #region Workflow Repository Methods

        public IEnumerable<WorkflowTask> GetWorkflowTasks()
        {
            return ampEntities.WorkflowTasks.Where(x => x.Status == "A");
        }

        public IEnumerable<WorkflowStage> GetWorkflowStages()
        {
            return ampEntities.WorkflowStages.Where(x => x.Status == "A");
        }

        public Int32 NextWorkFlowId()
        {
            IEnumerable<WorkflowMaster> workflowMasters = ampEntities.WorkflowMasters;
            if (workflowMasters.Any())
            {
                return workflowMasters.Max(x => x.WorkFlowID) + 1;
            }
            else
            {
                return 1;
            }

        }

        public void InsertWorkFlowMaster(WorkflowMaster workflowMaster)
        {
            ampEntities.WorkflowMasters.Add(workflowMaster);
            //ampEntities.Entry(workflowMaster).State = System.Data.Entity.EntityState.Modified;
        }

        public void UpdateWorkFlowMaster(WorkflowMaster workflowMaster)
        {
            ampEntities.Entry(workflowMaster).State = System.Data.Entity.EntityState.Modified;
        }

        public IEnumerable<WorkflowMaster> GetWorkflowMasters(int workflowId)
        {
            IEnumerable<WorkflowMaster> workflowMasters =
                ampEntities.WorkflowMasters.Where(x => x.WorkFlowID == workflowId);
            return workflowMasters;
        }

        public WorkflowMaster GetWorkflowMaster(int workflowId, int workflowStepId)
        {
            return ampEntities.WorkflowMasters.Find(workflowId, workflowStepId);
        }


        public WorkflowMaster GetWorkflowMaster(int workflowId)
        {
            return ampEntities.WorkflowMasters.FirstOrDefault(x => x.WorkFlowID == workflowId);
        }

        public IEnumerable<WorkflowMaster> GetWorkflowMastersByProject(string projectId)
        {
            IEnumerable<WorkflowMaster> workflowMasters =
                ampEntities.WorkflowMasters.Where(x => x.ProjectID.Equals(projectId));
            return workflowMasters;
        }

        public IEnumerable<WorkflowMaster> GetWorkflowMastersByProjectandTask(string projectId, Int32 taskId)
        {
            IEnumerable<WorkflowMaster> workflowMasters =
                ampEntities.WorkflowMasters.Where(x => x.ProjectID == projectId && x.TaskID == taskId);
            return workflowMasters;
        }


        public WorkflowDocument GetWorkflowDocument(int workflowId)
        {
            WorkflowDocument workflowDocument =
                ampEntities.WorkflowDocuments.FirstOrDefault(x => x.WorkflowID == workflowId);
            return workflowDocument;
        }

        public IEnumerable<WorkflowDocument> GetWorkflowDocuments(int workflowId)
        {
            IEnumerable<WorkflowDocument> workflowDocuments =
                ampEntities.WorkflowDocuments.Where(x => x.WorkflowID == workflowId);
            return workflowDocuments;
        }

        public void InsertWorkflowDocument(WorkflowDocument workflowDocument)
        {
            ampEntities.WorkflowDocuments.Add(workflowDocument);
        }

        public void DeleteWorkflowDocument(int workflowId)
        {
            WorkflowDocument workflowDocumentToDelete =
                ampEntities.WorkflowDocuments.FirstOrDefault(x => x.WorkflowID == workflowId);
            if (workflowDocumentToDelete != null)
            {
                ampEntities.WorkflowDocuments.Remove(workflowDocumentToDelete);
            }
        }

        public IEnumerable<WorkflowDocument> GetAllWorkflowDocuments()
        {
            return ampEntities.WorkflowDocuments.Where(x => x.Status == "A");
        }

        public void DeleteWorkflowMaster(WorkflowMaster workflowMaster)
        {
            ampEntities.WorkflowMasters.Remove(workflowMaster);
        }

        public IEnumerable<vHoDBudCentLookup> GetHoDAlertRecipients(string projectId)
        {
            return ampEntities.vHoDBudCentLookups.Where(x => x.ProjectID == projectId);
        }

        #endregion

        #region Lookup Methods

        /// <summary>
        /// LookUpBudgetCentre - look up a list of valid Budget Centres.
        /// </summary>
        /// <returns>
        /// A List of Budget Centre Model objects.
        /// </returns>
        /// <remarks>Filter on Status A (Active Budget Centres only).</remarks>
        public IEnumerable<BudgetCentre> LookUpBudgetCentre()
        {
            return ampEntities.BudgetCentres.Where(b => b.Status.Equals("A"));

        }

        public IEnumerable<ProjectMaster> LookUpProjectMaster()
        {
            // return ampEntities.ProjectMasters.ToList();
            return ampEntities.ProjectMasters.Where(x => x.Stage != "7");

            //Bloodhound type ahead fails due to number of rows returned!
            //return ampEntities.ProjectMasters.Where(x => x.Stage == "5");

        }

        public IEnumerable<UserLookUp> LookUpUsers()
        {
            return ampEntities.UserLookUps;
        }

        public IEnumerable<DeliveryChain> LookUpDeliveryChains(string id, string user)
        {
            return ampEntities.DeliveryChains.Where(x => x.ComponentID.Equals(id) && x.Status == "A");
        }

        public IEnumerable<PartnerMaster> LookUpPartnerList()
        {
            return ampEntities.PartnerMasters.Where(x => x.Status == "A");
        }


        // return list of AMP partners matching search criteria entered PartneMaster table in AMP
        public IEnumerable<PartnerMaster> LookUpAMPPartnerSearchList(string searchitem)
        {
            return ampEntities.PartnerMasters.Where(x => x.PartnerName.Contains(searchitem) && x.Status == "A").ToList();
        }

       



        public IEnumerable<UserLookUp> LookUpUsers(IEnumerable<String> resourceIDs)
        {
            IEnumerable<UserLookUp> users;

            users = ampEntities.UserLookUps.Where(x => resourceIDs.Any(r => r == x.ResourceID)).ToList();

            return users;
        }


        public IEnumerable<FundingMech> LookUpFundingMechs()
        {
            return ampEntities.FundingMeches;
        }

        public IEnumerable<FundingArrangement> LookUpFundingArrangements()
        {
            return ampEntities.FundingArrangements;
        }

        public IEnumerable<PartnerOrganisation> LookUpPartnerOrganisations()
        {
            return ampEntities.PartnerOrganisations;
        }

        public IEnumerable<BenefitingCountry> LookUpBenefitingCountrys()
        {
            return
                ampEntities.BenefitingCountries.Where(x => x.Status == "A")
                    .OrderBy(a => a.BenefitingCountryDescription)
                    .ToList();
        }


        public IEnumerable<WorkflowMaster> LookUpWorkFlowMaster()
        {
            return ampEntities.WorkflowMasters;
        }

        public IEnumerable<InputSector> LookUpInputSector()
        {
            return ampEntities.InputSectors.Where(b => b.Status.Equals("A"));
        }

        public IEnumerable<Portfolio> GetPortfolios(String User)
        {
            return ampEntities.Portfolios.Where(x => x.UserID == User);
        }

        public IEnumerable<InputSector> LookUpInputSector(String FundingMech)
        {

            IEnumerable<FundingMechToSector> fundingmechtosector;

            //fundingmechtosector = ampEntities.FundingMechToSectors.Where(x => x.FundingMech.Equals(FundingMech));

            //think i need to relate the tables.
            //return ampEntities.ProjectMasters.Where(x => x.Portfolios.Any(y => y.UserID == CurrentUser);

            return ampEntities.InputSectors.Where(x => x.FundingMechToSectors.Any(y => y.FundingMech == FundingMech));

            // return ampEntities.InputSectors.Where(b => b.Status.Equals("A"));
        }

        public IEnumerable<EvaluationType> LookUpEvaluationTypes()
        {
            return ampEntities.EvaluationTypes.Where(x => x.Status == "A");
        }

        public IEnumerable<Risk> LookUpRiskTypes()
        {
            return ampEntities.Risks;
        }

        public List<string> LookUpSRO()
        {
            return
                ampEntities.Teams.Where(x => x.RoleID == "SRO" && x.Status == "A")
                    .Select(p => p.TeamID)
                    .Distinct()
                    .ToList();
        }

        public IEnumerable<EvaluationManagement> LookUpEvaluationManagements()
        {
            return ampEntities.EvaluationManagements.Where(x => x.Status == "A");
        }

        #endregion

        #region Disposal Methods

        // Dispose Methods

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AMPRepository()
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

        public int GetNextReviewOutputId(string projectId, int reviewId)
        {
            //Get all projects output
            List<ReviewOutput> reviewOutputs =
                ampEntities.ReviewOutputs.Where(m => m.ProjectID == projectId && m.ReviewID == reviewId).ToList();

            int nextReviewOutputId;

            if (reviewOutputs.Any())
            {
                //Get max review output Id and add 1 
                nextReviewOutputId = (int) (reviewOutputs.Max(r => r.OutputID) + 1);
            }
            else
            {
                nextReviewOutputId = 0;
            }
            return nextReviewOutputId;
        }

        #region Admin Methods

        public void AdminUpdateTeamMember(Team team)
        {
            ampEntities.Entry(team).State = System.Data.Entity.EntityState.Modified;

            UpdateARIESTeam(team);
        }

        public void AdminEndTeamMember(Team team)
        {
            ampEntities.Entry(team).State = System.Data.Entity.EntityState.Modified;

            EndARIESTeam(team, true);
        }

        public AdminUser GetAdmin(string empNo)
        {
            AdminUser admin = new AdminUser();

            admin = ampEntities.AdminUsers.FirstOrDefault(x => x.AdminUserID == empNo && x.Status == "A");

            return admin;

        }

        public AdminUser AdminExists(string empNo)
        {
            AdminUser adminUser = ampEntities.AdminUsers.FirstOrDefault(x => x.AdminUserID == empNo);

            return adminUser;
        }


        public void AddAdminUser(AdminUser newAdmin)
        {
            ampEntities.AdminUsers.Add(newAdmin);
        }

        public void DeleteAdminUser(AdminUser adminToDelete)
        {
            ampEntities.Entry(adminToDelete).State = System.Data.Entity.EntityState.Modified;
        }

        public IEnumerable<AdminUser> GetAdminUsers()
        {

            IEnumerable<AdminUser> adminUsers = ampEntities.AdminUsers.Where(x => x.Status == "A");

            return adminUsers;
        }

        public void UpdateAdminUser(AdminUser adminUserToUpdate)
        {
            ampEntities.Entry(adminUserToUpdate).State = System.Data.Entity.EntityState.Modified;
        }

        public ReviewMaster AdminGetReviewMaster(string id, Int32 reviewId)
        {
            return ampEntities.ReviewMasters.FirstOrDefault(x => x.ProjectID == id && x.ReviewID == reviewId);
        }

        public void AdminUpdateReviewMaster(ReviewMaster reviewMaster)
        {
            ampEntities.Entry(reviewMaster).State = System.Data.Entity.EntityState.Modified;
        }

        public void AdminUpdateProjectDate(ProjectDate projectDate)
        {
            ampEntities.Entry(projectDate).State = System.Data.Entity.EntityState.Modified;
        }

        public void AdminUpdateProjectInfo(ProjectInfo projectInfo)
        {
            ampEntities.Entry(projectInfo).State = System.Data.Entity.EntityState.Modified;
        }


        public void AdminUpdateProjectMarkers(Markers1 projectMarkers)
        {
            ampEntities.Entry(projectMarkers).State = System.Data.Entity.EntityState.Modified;
        }


        public void AdminUpdateWorkflowDocument(WorkflowDocument workflowDocument)
        {
            ampEntities.Entry(workflowDocument).State = System.Data.Entity.EntityState.Modified;
        }

        public WorkflowDocument AdminGetWorkflowDocument(int id)
        {
            WorkflowDocument workflowDocument =
                ampEntities.WorkflowDocuments.FirstOrDefault(x => x.ID == id);
            return workflowDocument;
        }

        public InputSectorCode AdminGetComponentInputSector(string componentId, Int32 lineNoInt)
        {
            return ampEntities.InputSectorCodes.Find(componentId, lineNoInt);
        }

        public void AdminUpdateComponentInputSector(InputSectorCode inputSectorCode)
        {
            ampEntities.Entry(inputSectorCode).State = System.Data.Entity.EntityState.Modified;
        }

        public void UpdateComponentMaster(ComponentMaster componentMaster)
        {
            ComponentDate componentdate = GetComponentDates(componentMaster.ComponentID);
            try
            {
                if (AMPUtilities.ARIESUpdateEnabled() == "true")
                {
                    UpdateARIESComponent(componentMaster, componentdate);
                }
                ampEntities.Entry(componentMaster).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateOverallRiskRating(OverallRiskRating overallRiskRating)
        {
            try
            {
                ampEntities.Entry(overallRiskRating).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateComponentDate(ComponentDate componentdate)
        {
            ComponentMaster componentmaster = GetComponent(componentdate.ComponentID);
            try
            {
                if (AMPUtilities.ARIESUpdateEnabled() == "true")
                {
                    UpdateARIESComponent(componentmaster, componentdate);
                }
                ampEntities.Entry(componentdate).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProjectMaster(ProjectMaster projectdetails)
        {
            ProjectDate projectDates = GetProjectDates(projectdetails.ProjectID);
            ProjectMaster PM = new ProjectMaster();
            PM = projectdetails;
            try
            {
                PM.ProjectDate = projectDates;
                ManageARIESProject(PM);
                ampEntities.Entry(projectdetails).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AdminUpdateDeliveryChain(DeliveryChain deliveryChain)
        {
            UpdateDeliveryChain(deliveryChain);
        }

        public ReviewDeferral AdminGetReviewDeferral(Int32 deferralId)
        {
            ReviewDeferral reviewDeferral = ampEntities.ReviewDeferrals.Find(deferralId);

            return reviewDeferral;
        }

        public void AdminUpdateReviewDeferral(ReviewDeferral reviewDeferral)
        {
            ampEntities.Entry(reviewDeferral).State = System.Data.Entity.EntityState.Modified;
        }

        public RiskDocument GetRiskDocument(int riskRegisterId)
        {
            return ampEntities.RiskDocuments.Find(riskRegisterId);
        }

        public void UpdateRiskDocument(RiskDocument riskDocument)
        {
            ampEntities.Entry(riskDocument).State = System.Data.Entity.EntityState.Modified;
        }

        public EvaluationDocument GetEvaluationDocument(int id)
        {
            return ampEntities.EvaluationDocuments.Find(id);
        }

        public void UpdateEvaluationDocument(EvaluationDocument evaluationDocument)
        {
            ampEntities.Entry(evaluationDocument).State = System.Data.Entity.EntityState.Modified;
        }

        public ReviewDocument GetReviewDocument(int id)
        {
            return ampEntities.ReviewDocuments.Find(id);
        }

        public void UpdateReviewDocument(ReviewDocument reviewDocument)
        {
            ampEntities.Entry(reviewDocument).State = System.Data.Entity.EntityState.Modified;
        }

        public FundingMechToSector GetFundingMechToSectorMapping(int id)
        {
            return ampEntities.FundingMechToSectors.Find(id);
        }

        public void UpdateFundingMechToSectorMapping(FundingMechToSector mappingToUpdate)
        {
            ampEntities.Entry(mappingToUpdate).State = System.Data.Entity.EntityState.Modified;
        }


        public InputSector GetIndividualInputSector(string sectorCodeId)
        {
            return ampEntities.InputSectors.Find(sectorCodeId);
        }

        public IEnumerable<FundingMechToSector> GetAllFundingMechMappingsForSectorCode(string sectorCodeID)
        {
            return ampEntities.FundingMechToSectors.Where(x => x.SectorCode.Equals(sectorCodeID));
        }

        public void AddNewFundingMechToSectorMapping(FundingMechToSector newMapping)
        {
            ampEntities.FundingMechToSectors.Add(newMapping);
        }

        public PartnerMaster AdminGetPartner(int id)
        {
            return ampEntities.PartnerMasters.Find(id);
        }

        public void AdminUpdatePartner(PartnerMaster partnerMaster)
        {
            ampEntities.Entry(partnerMaster).State = System.Data.Entity.EntityState.Modified;
        }

        #endregion
    }
}

