using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Models
{
    public class ProjectAlert
    {
        public string ProjectID { get; set; }

        public string ActionRequired { get; set; }

        public string ProjectTitle { get; set;}

        public DateTime ActionDate { get; set; }

        //Is this the right data type? Should it be a hyperlink object? Or for MVC is it better to pass a string?
        public string ActionLink { get;set; }
    }
}