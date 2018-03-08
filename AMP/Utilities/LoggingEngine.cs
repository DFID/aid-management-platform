using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ARIESModels;

namespace AMP.Utilities
{
    public class LoggingEngine : AMP.Utilities.ILoggingEngine
    {
        private AMPRepository projectrepository;

        //Real method 
        public LoggingEngine(AMPRepository projectrepository)
        {
            this.projectrepository = projectrepository;
        }

        public LoggingEngine()
        {
            projectrepository = new AMPRepository();
        }

        public void InsertLog(String ViewName, String user)
        {
            Logging logging = new Logging();

            logging.ViewName = ViewName;
            logging.LastUpdated = DateTime.Now;
            logging.UserID = user;
           
            projectrepository.InsertLog(logging);
            projectrepository.Save();
        }

        public void InsertLog(String ViewName,String user, String ProjectID)
        {
            Logging logging = new Logging();

            logging.ProjectID = ProjectID;
            logging.ViewName = ViewName;
            logging.LastUpdated = DateTime.Now;
            logging.UserID = user;

            projectrepository.InsertLog(logging);
            projectrepository.Save();
        }

        public void InsertCodeLog(String MethodName, String Description, DateTime From, DateTime To, TimeSpan Result, String User)
        {
            CodePerformance codePerformance = new CodePerformance();

            double ResultLong = (Result.Seconds * 1000) + Result.Milliseconds;

            codePerformance.MethodName = MethodName;
            codePerformance.Description = Description;
            codePerformance.From = From;
            codePerformance.To = To;
            codePerformance.Result = ResultLong;
            codePerformance.LastUpdated = DateTime.Now;
            codePerformance.UserID = User;
            codePerformance.Value = "";

            projectrepository.InsertCodeLog(codePerformance);
            projectrepository.Save();
        }

        public void InsertCodeLog(String MethodName, String Description, DateTime From, DateTime To, TimeSpan Result, String User, String Value)
        {
            CodePerformance codePerformance = new CodePerformance();

            double ResultLong = (Result.Seconds * 1000) + Result.Milliseconds;

            codePerformance.MethodName = MethodName;
            codePerformance.Description = Description;
            codePerformance.From = From;
            codePerformance.To = To;
            codePerformance.Result = ResultLong;
            codePerformance.LastUpdated = DateTime.Now;
            codePerformance.UserID = User;
            codePerformance.Value = Value;

            projectrepository.InsertCodeLog(codePerformance);
            projectrepository.Save();
        }
        #region Disposal Methods
        // Dispose Methods

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~LoggingEngine()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        #endregion

    }

}