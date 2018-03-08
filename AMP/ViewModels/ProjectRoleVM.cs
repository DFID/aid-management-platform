using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ProjectRoleVM
    {
        public IEnumerable<ProjectRoleValuesVM> ProjectRoleValues { get; set; }
        public string SelectedRoleValue { get; set; }
    }
}