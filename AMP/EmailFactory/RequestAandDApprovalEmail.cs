using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RequestAandDApprovalEmail:Email
    {
        #region Constructor

        public RequestAandDApprovalEmail()
        {
            Subject = "ACTION: Approve Project - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has sent you a request to approve the above project in the Aid Management Platform with the following comments." +
            "<br/><br/>\"{2}\"" +
            "<br/><br/>This approval will have the following impact: <br/>" +
            "<br/>-	The project will move from ‘Pre-Pipeline’ to ‘Appraisal and Design’ stage." +
            "<br/>-	Any design funds will be approved and made available to spend." +
            "<br/>- The following project information will be published and made publicly available on the Development Tracker " +
            "<br/>   Along with the SRO, you must make sure that this information meets " +
            "<a href =\"https://dfid-live.saas.hp.com/saw/ess/viewResult/574979?TENANTID=658803389\"> DFID transparency standards </a>" + 
            "and is fit for publication" +
            "<br/> &emsp;&emsp;&emsp; - Project Title " +
            "<br/> &emsp;&emsp;&emsp; - Project Description " +
            "<br/> &emsp;&emsp;&emsp; - Project Component" +

             "<br/><br/>Please use the link below to access the approval screen." +
            "<br/><br/>{3}<br/><br/>{4}<br/><br/></span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";
        }

        #endregion

    }
}