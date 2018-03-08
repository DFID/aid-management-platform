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
using Microsoft.Ajax.Utilities;

namespace AMP.Services
{
    public class DocumentService : IDocumentService, IDisposable
    {
        private IAMPRepository _ampRepository;


        //Real method 
        public DocumentService(IAMPRepository ampRepository)
        {
            _ampRepository = ampRepository;

        }


        public string ReturnDocumentUrl(string documentID, string docSource)
        {
            string url;
            if (docSource == "Q")
            {
                url =
                    "http://oldDocumentSystem/GetDocument.aspx?docid = + documentID";
            }

            else if (docSource == "V")
            {
                url = "http://newDocumentSystem/GetDocument.aspx?docid = + documentID";
            }
            else
            {
                url = "";
            }

            return url;
        }


        #region Disposal
        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DocumentService()
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