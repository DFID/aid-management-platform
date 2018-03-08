using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;

namespace AMP.ViewModels
{
    public class AdminUsersVM
    {
        public IEnumerable<AdminUser> adminUsers { get; set; }

        public string AdminToAdd { get; set; }
    }
}