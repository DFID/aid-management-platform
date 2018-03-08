using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RequestApprovalEmail:Email
    {
        #region Constructor

        public RequestApprovalEmail()
        {
            Subject = "ACTION: Approve Project - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has sent you a request to approve the above project in the Aid Management Platform with the following comments." +
            "<br/><br/>\"{2}\"" +

          "<br/><br/> Please ensure that you are satisfied that the project information available on AMP is correct and all relevant Smart Rules have been complied with – including that the programme is consistent with relevant UK legislation such as Gender Equality requirements - before approving or rejecting this request."+
          "<br/><br/>This approval will have the following impact: " +

          "<br/><br/> - The project will move from ‘Appraisal and Design’ to ‘Implementation’ stage. All funds will be approved and made available to spend." +

          "<br/><br/> If appropriate, the following project documents will be published and made publicly available on the Development Tracker. Along with the SRO, you must make sure that all documents meet DFID’s transparency standards and are fit for publication. "+
          "<br/><br/>Please check the Smart Guides on " + "<a href =\"\">Transparency</a>" +

          "<br/><br/> &emsp;&emsp;&emsp; - Business Case / Addendum" +
          "<br/> &emsp;&emsp;&emsp; - Inventory Summary" +
          "<br/> &emsp;&emsp;&emsp; - Logframe" +
          "<br/> &emsp;&emsp;&emsp; - Memorandum of Understanding/ Amendment letter" +
          "<br/> &emsp;&emsp;&emsp; - Accountable Grant" +

          "<br/><br/>Please use the link below to access the approval screen." + "<br/><br/>{3}<br/><br/>{4}<br/><br/></span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";
        }

        #endregion

    }
}