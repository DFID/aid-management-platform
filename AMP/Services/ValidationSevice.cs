using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AMP.ARIESModels;
using AMP.Models;
using AMP.Utilities;
using AMP.ViewModels;

using MoreLinq;


namespace AMP.Services
{
    public class ValidationSevice : IValidationService, IDisposable
    {
        private IAMPRepository _ampRepository;


        //Real method 
        public ValidationSevice(IAMPRepository ampRepository)
        {
            _ampRepository = ampRepository;


        }


        public bool IsTheARExemptionValid( string user, ProjectMaster project, Performance performance)
        {
            //Checks if ar was exempt as project length was under 15 months - if now over 15 months, returns false
            if (performance.ARExcemptReason ==
                _ampRepository.GetSingleExemptionReason("3", "AR").ExemptionReason1 &&
                performance.ARRequired == "No")
            {
                DateTime actualPlus15Months = project.ProjectDate.ActualStartDate.Value.AddMonths(15);
                //Compares actual end date plus 15 months to planned end date (if planned end date is later, would give an int greater than 0)
                // DateTime.Compare(project.ProjectDate.OperationalEndDate.Value, actualPlus15Months) >0
                if (project.ProjectDate.OperationalEndDate.Value.CompareTo(actualPlus15Months) > 0)
                    {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            // Checks if project was exempt as AR due less than 4 months before the project end date (3 months per rule plus a one month tolerance) - if AR is now due more than 4 months before new project planned end date, unexempts AR
             if (performance.ARExcemptReason ==
                     _ampRepository.GetSingleExemptionReason("7", "AR").ExemptionReason1 &&
                     performance.ARRequired == "No")
            {
                DateTime nextARPlus4Months;
                // Gathers existing approved reviews
                List<ReviewMaster> approvedReviews = new List<ReviewMaster>();
                foreach (var review in project.ReviewMasters)
                {
                    if (review.Approved == "1")
                        approvedReviews.Add(review);
                }

                // Checks if any existing approved reviews - if not, assumes AR was due 12 months after actual start date & adds 4 months to this for comparison (16 months in total)
                //int? existingReviews = approvedReviews.Count;
                if  (approvedReviews.Count == 0) 
                {
                    nextARPlus4Months = project.ProjectDate.ActualStartDate.Value.AddMonths(16);
                }
                // If existing approved review, takes latest & adds 4 months for the comparison
                else
                {
                    DateTime lastReviewDate =
                        approvedReviews.MaxBy(x => x.ReviewDate.Value).ReviewDate.Value;
                    nextARPlus4Months = lastReviewDate.AddMonths(16);
                }

                //Compares ar due date plus 4 months to new planned end date (if planned end date is later, would give an int greater than 0)
                
               // if (DateTime.Compare(project.ProjectDate.OperationalEndDate.Value, nextARPlus4Months) > 0)
                if (project.ProjectDate.OperationalEndDate.Value.CompareTo(nextARPlus4Months) > 0)

                {
                    return false;
                }
                else
                {
                    return true;
                }
                
            }
           else
            
            {
                return true;
            }
        }

        #region Disposal
        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ValidationSevice()
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