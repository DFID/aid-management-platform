using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class NewTeamMemberVM
    {
        public string TeamID { get; set; }
        public string PersonName { get; set; }
        public string RoleID { get; set; }
        public string Status { get; set; }
        public System.DateTime StartDate { get; set; }

        public int StartDate_Day { get; set; }
        public int StartDate_Month { get; set; }
        public int StartDate_Year { get; set; }
        public System.DateTime EndDate { get; set; }
        public int EndDate_Day { get; set; }
        public int EndDate_Month { get; set; }
        public int EndDate_Year { get; set; }
        public ProjectRoleVM ProjectRolesVm { get; set; }
    }
}