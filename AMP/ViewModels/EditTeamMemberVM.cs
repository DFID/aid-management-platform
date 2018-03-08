using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class EditTeamMemberVM
    {
        public int ID { get; set; }
        public string TeamID { get; set; }
        public string RoleID { get; set; }
        public ProjectRoleVM ProjectRolesVm { get; set; }
        public string DISPLAY_NAME_FORENAME_FIRST { get; set; }
        public string ProjectID { get; set; }

    }
}