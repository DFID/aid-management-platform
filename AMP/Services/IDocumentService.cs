using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Configuration;
using System.Threading.Tasks;
using AMP.Models;
using AMP.ViewModels;
using Newtonsoft.Json;
namespace AMP.Services
{
    public interface IDocumentService
    {

        string ReturnDocumentUrl(string documentID, string docSource);
    }
}