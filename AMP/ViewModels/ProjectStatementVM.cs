using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using System.ComponentModel.DataAnnotations;


namespace AMP.ViewModels
{
    public class ProjectStatementVM
    {
        public IEnumerable<StatementRecordVM> ProjectStatement { get; set; }
        public NewStatement NewProjectStatement { get; set; }
        public ProjectHeaderVM ProjectHeader { get; set; }

        public IEnumerable<String> StatementTypes { get; set; }

        public ProjectWFCheckVM WFCheck { get; set; }
    }
}