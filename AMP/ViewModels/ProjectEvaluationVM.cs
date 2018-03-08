using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AMP.Models;


namespace AMP.ViewModels
{
    public class ProjectEvaluationVM
    {
        public string EvaluationID { get; set; }
        public string ProjectID { get; set; }
        public string EvaluationTypeID { get; set; }
        public string IfOther { get; set; }
        public string ManagementOfEvaluation { get; set; }
        public Nullable<decimal> EstimatedBudget { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<int> StartDate_Day { get; set; }
        public Nullable<int> StartDate_Month { get; set; }
        public Nullable<int> StartDate_Year { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> EndDate_Day { get; set; }
        public Nullable<int> EndDate_Month { get; set; }
        public Nullable<int> EndDate_Year { get; set; }
        public string AdditionalInfo { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }
        public virtual EvaluationType EvaluationType { get; set; }
        public virtual EvaluationManagement EvaluationManagement { get; set; }
        public IEnumerable<EvaluationDocumentVM> EvaluationDocuments { get; set; }
        public NewEvaluationDocumentVM NewEvaluationDocument { get; set; }
        public EvaluationTypeVM EvaluationTypes { get; set; }
        public EvaluationManagementVM EvaluationManagements { get; set; }
        public ProjectHeaderVM projectHeader { get; set; }
        public ProjectWFCheckVM WFCheck { get; set; }

    }
}