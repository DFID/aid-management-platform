using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ProjectTeamVM
    {
        public List<ProjectTeamMemberVM> CurrentProjectTeam { get; set; }

        public List<ProjectTeamMemberVM> OtherProjectTeam { get; set; }

        public List<ProjectTeamMemberVM> FormerProjectTeam { get; set; }
        
        public string TeamMarker { get; set; }

        public NewTeamMemberVM NewTeamMember { get; set; }

        public EditTeamMemberVM EditTeamMember { get; set; }

        public ProjectHeaderVM ProjectHeader { get; set; }

        public ProjectWFCheckVM WFCheck {get; set; }
        public bool ReadOnly { get; set; }
    }
}