using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ARIESModels;


namespace AMP.Utilities
{
    public class FeedbackEngine
    {
        private AMPRepository projectrepository;

        //Real method 
        public FeedbackEngine(AMPRepository projectrepository)
        {
            this.projectrepository = projectrepository;
        }

        public FeedbackEngine()
        {
            projectrepository = new AMPRepository();
        }

        //public void InsertFeedback(String ViewName, String user, String FeedbackValue)
        //{
        //    Feedback feedback = new Feedback();

        //    //Populate Model
        //    feedback.LastUpdated = DateTime.Now;
        //    feedback.UserID = user;
        //    feedback.ViewName = ViewName;
        //    feedback.Feedback1 = FeedbackValue;


        //    //Execute Insert Log error method
        //    projectrepository.InsertFeedback(feedback);
        //    //Save
        //    projectrepository.Save();
        //}

        //public void InsertFeedback(String ViewName, String user, String FeedbackValue,String ProjectID)
        //{
        //    Feedback feedback = new Feedback();

        //    //Populate Model
        //    feedback.LastUpdated = DateTime.Now;
        //    feedback.UserID = user;
        //    feedback.ViewName = ViewName;
        //    feedback.Feedback1 = FeedbackValue;
        //    feedback.ProjectID = ProjectID;

        //    //Execute Insert Log error method
        //    projectrepository.InsertFeedback(feedback);
        //    //Save
        //    projectrepository.Save();
        //}

    }
}