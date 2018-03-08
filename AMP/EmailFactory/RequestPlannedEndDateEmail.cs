using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RequestPlannedEndDateEmail : Email
    {


        #region Constructor

        public RequestPlannedEndDateEmail()
        {
            Subject = "ACTION: Approve Project Planned End Date Amendment- {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has sent you a request to approve a planned end date change with the following comments." +
            "<br/><br/>\"{2}\"" +
            "<br/><br/> Please ensure that you are satisfied that the project information available on AMP is correct before approving or rejecting this request." +
            "<br/><br/>This approval will update: " +
            "<br/>- The planned end date." +
            "<br/>- The financial end date." +
            "<br/>- The project closure reminder date." +
            "<br/>- If the project has a PCR, then its due date will be amended. The change may also lift the AR exemption for the project if applicable." +
            "<br/><br/>Please use the link below to access the approval screen." +
            "<br/><br/>{3}<br/><br/>{4}<br/><br/></span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";
        }

        #endregion
    }
}

