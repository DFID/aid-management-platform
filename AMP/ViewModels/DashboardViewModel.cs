using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using PagedList;

namespace AMP.ViewModels
{
    public partial class DashboardViewModel
    {
        public PagedList<UserProjectsViewModel> userprojects { get; set; }
        public IEnumerable<ProjectAlert> projectalerts { get; set; }
        public string ARIESWebServiceMessage { get; set; }

        public String NewProjectID { get; set; }
        public String SearchKeyWord { get; set; }

        public String Stages { get; set; }
    }
}