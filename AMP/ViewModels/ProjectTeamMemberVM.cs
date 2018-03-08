using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ProjectTeamMemberVM
    {
        public int ID { get; set; }
        public string TeamId { get; set; }
        public string RoleId { get; set; }
        public string RoleDescription { get; set; }
        public string Status { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool CurrentRoleHolder { get; set; }
        public string EmpNo { get; set; }
        public string IsEmployed { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IsHeadofDepartment { get; set; }

        public string ProjectID { get; set; }
    }
}