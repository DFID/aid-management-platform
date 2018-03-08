using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RequestArchiveEmail:Email
    {
        #region Constructor

        public RequestArchiveEmail()
        {
            Subject = "ACTION: Archive Project - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has sent you a request to Archive the project above in the Aid Management Platform with the following comments." +
            "<br/><br/>\"{2}\"" +
            "<br/><br/> By approving this request you confirm the following:" +
            "<br/>- There are no outstanding financial transactions;" +
            "<br/>- All financial audit statements have been received, with all funds accounted for appropriately;" +
            "<br/>- All assets have been transferred or disposed of according to the SmartRules." +
            "<br/><br/>Please ensure that you are satisfied that the project information available on AMP is correct before approving or rejecting this request. Once approved you will no longer be able to edit or carry out transactions for this project." +
            "<br/><br/>Please use the link below to access the approval screen." +
            "<br/><br/>{3}<br/><br/>{4}<br/><br/></span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";
        }

        #endregion
    }
}