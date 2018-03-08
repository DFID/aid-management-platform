using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ARIESModels;

namespace AMP.Utilities
{
    public class ErrorEngine : AMP.Utilities.IErrorEngine
    {

        private AMPRepository projectrepository;

        //Real method 
        public ErrorEngine(AMPRepository projectrepository)
        {
            this.projectrepository = projectrepository;
        }

        public ErrorEngine()
        {
            projectrepository = new AMPRepository();
        }


        /// <summary> Executes the Standard Error Log with exception object only</summary>
        /// <param name="id">Accepts an exception object</param>
        /// <returns>Updates AMP.System.ErrorLog</returns>
        
        public void LogError(Exception ex, string user)
        {
            //Create an new instance of the model ErrorLog
            ErrorLog errorlog = new ErrorLog();

            //Populate error log with the correct information
            //Handle nulls
            if (ex.Message == null)
            {
                errorlog.ErrorMessage = "";
                
            }
            else
            {
                errorlog.ErrorMessage = ex.Message.ToString();
            }

         
            
            //Handle nulls (Inner Exception is not always populated)
            if (ex.InnerException == null)
            {
                errorlog.InnerException = "";
            }
            else
            {
                errorlog.InnerException = ex.InnerException.ToString();
            }
            //Add the line number to the exception.


            //Handle nulls
            if (ex.StackTrace == null)
            {
                errorlog.StackTrace = "";
            }
            else
            {
                errorlog.StackTrace = ex.StackTrace.ToString();
                errorlog.InnerException = errorlog.InnerException + " - Line Number " + GetLineNumber(ex).ToString();
            }

            errorlog.LastUpdated = DateTime.Now;

            errorlog.UserID = user;

            //Execute Insert Log error method
            projectrepository.InsertError(errorlog);
            //Save
            projectrepository.Save();
        }



        /// <summary> Executes the Overloader Error Log with ProjectID and exception object only</summary>
        /// <param name="id">Accepts an exception object</param>
        /// <returns>Updates AMP.System.ErrorLog</returns>
        public void LogError(String ProjectID, Exception ex, string user)
        {
            //Create an new instance of the model ErrorLog
            ErrorLog errorlog = new ErrorLog();

            //Populate error log with the correct information
            //Handle nulls
            if (ex.Message == null)
            {
                errorlog.ErrorMessage = "";
            }
            else
            {
                errorlog.ErrorMessage = ex.Message.ToString();
            }
            //Handle nulls (Inner Exception is not always populated)
            if (ex.InnerException == null)
            {
                errorlog.InnerException = "";
            }
            else
            {
                errorlog.InnerException = ex.InnerException.ToString();
            }

            errorlog.LastUpdated = DateTime.Now;
            errorlog.ProjectID = ProjectID;

            //Handle nulls
            if (ex.StackTrace == null)
            {
                errorlog.StackTrace = "";
            }
            else
            {
                errorlog.StackTrace = ex.StackTrace.ToString();
                errorlog.InnerException = errorlog.InnerException + " - Line Number " + GetLineNumber(ex).ToString();
            }

            errorlog.UserID = user;
         
            //Execute Insert Log error method
            projectrepository.InsertError(errorlog);
            //Save
            projectrepository.Save();
        }


        public void LogError(Exception ex, String CustomError, string user)
        {
            //Create an new instance of the model ErrorLog
            ErrorLog errorlog = new ErrorLog();

            //Populate error log with the correct information
            //Handle nulls
            if (ex.Message == null)
            {
                errorlog.ErrorMessage = "";
            }
            else
            {
                errorlog.ErrorMessage = ex.Message.ToString();
            }
            //Handle nulls (Inner Exception is not always populated)
            if (ex.InnerException == null)
            {
                errorlog.InnerException = "";
            }
            else
            {
                errorlog.InnerException = ex.InnerException.ToString();
                errorlog.InnerException = errorlog.InnerException + " - Line Number " + GetLineNumber(ex).ToString();
            }

            errorlog.LastUpdated = DateTime.Now;
            

            //Handle nulls
            if (ex.StackTrace == null)
            {
                errorlog.StackTrace = "";
            }
            else
            {
                errorlog.StackTrace = ex.StackTrace.ToString();
            }

            errorlog.CustomError = CustomError;

            errorlog.UserID = user;

            //Execute Insert Log error method
            projectrepository.InsertError(errorlog);
            //Save
            projectrepository.Save();
        }

        private int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            return lineNumber;
        }

        public void LogError(String projectId, Exception ex, String CustomError, string user)
        {
            //Create an new instance of the model ErrorLog
            ErrorLog errorlog = new ErrorLog();

            //Populate error log with the correct information
            //Handle nulls
            if (ex.Message == null)
            {
                errorlog.ErrorMessage = "";
            }
            else
            {
                errorlog.ErrorMessage = ex.Message.ToString();
            }
            //Handle nulls (Inner Exception is not always populated)
            if (ex.InnerException == null)
            {
                errorlog.InnerException = "";
            }
            else
            {
                errorlog.InnerException = ex.InnerException.ToString();
            }
            errorlog.LastUpdated = DateTime.Now;
            errorlog.ProjectID = projectId;

            //Handle nulls
            if (ex.StackTrace == null)
            {
                errorlog.StackTrace = "";
            }
            else
            {
                errorlog.StackTrace = ex.StackTrace.ToString();
                errorlog.InnerException = errorlog.InnerException + " - Line Number " + GetLineNumber(ex).ToString();
            }

            errorlog.CustomError = CustomError;

            errorlog.UserID = user;

            //Execute Insert Log error method
            projectrepository.InsertError(errorlog);
            //Save
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

        ~ErrorEngine()
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