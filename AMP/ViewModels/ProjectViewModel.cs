using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ARIESModels;

namespace AMP.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectMasterVM ProjectMaster { get; set; }
        public ProjectCoreVM ProjectCore { get; set; }
        public ProjectDateVM ProjectDates { get; set; }
        public ProjectStaticDataVM ProjectStatic { get; set; }
        public List<ReviewVM> ProjectReviews { get; set; }
        public ReviewVM ReviewVm { get; set; }
        public ReviewPCRScore ProjectPcrScore { get; set; }
        public PerformanceVM Performance { get; set; }
        public Deferral deferral { get; set; }
        public List<ProjectTeamMemberVM> CurrentProjectTeam { get; set; }
        public List<ProjectTeamMemberVM> OtherProjectTeam { get; set; }
        public List<ProjectTeamMemberVM> FormerProjectTeam { get; set; }
        public ProjectTeamVM NewTeamMember { get; set; }
        public Stage Stages { get; set; }
        //The Approved Project Budget returned from the Finance Web Service
        public decimal ApprovedBudget { get; set; }
        //Error message returned from Finance Web Service
        public string FinanceWebServiceMessage { get; set; }

        //I've left this as a model rather than converting it to a Finance ViewModel. It is read only and is never updated.
        public IEnumerable<ProjectFinanceRecordVM> ProjectFinance { get; set; }

        public IEnumerable<ProcurementRecordVM> ProjectProcurement { get; set; }

        public IEnumerable<DocumentRecordVM> ProjectDocument { get; set; }

        public IEnumerable<StatementRecordVM> ProjectStatement { get; set; }
       
        public ProjectEvaluationVM ProjectEvaluation { get; set; }


        //To be replaced with a Component ViewModel
        public IEnumerable<ComponentMaster> ComponentMaster { get; set; }


    }
}