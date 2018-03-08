using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Models
{
    public class PersonDetails
    {
        public string EmpNo { get; set; }
        public string IsEmployed { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IsHeadofDepartment { get; set; }
    }
}