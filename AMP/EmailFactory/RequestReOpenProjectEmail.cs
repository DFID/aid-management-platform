using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.EmailFactory
{
    public class RequestReOpenProjectEmail : Email
    {
        #region Constructor

        public RequestReOpenProjectEmail()
        {
            Subject = "ACTION: Re-open Project - {0} {1}";

            Body = "<span style='font-family:Arial;font-size: 12pt;'>{0}<br/><br/>{1} has sent you a request to re-open the above project in the Aid Management Platform with the following comments." +
            "<br/><br/>\"{2}\"" +
            "<br/><br/> Please ensure that you are satisfied that the project information available on AMP is correct before approving or rejecting this request." +
            "<br/><br/>This approval will have the following impact: " +
            "<br/>-	The project will move from ‘Completion’ to ‘Implementation’ stage." +
            "<br/>-	Oustanding financial transactions can be completed." +
            "<br/><br/>Please use the link below to access the approval screen." +
            "<br/><br/>{3}<br/><br/>{4}<br/><br/></span>";

            AMPlink = "{0}/Workflow/Edit/{1}/{2}";
        }

        #endregion
    }
}