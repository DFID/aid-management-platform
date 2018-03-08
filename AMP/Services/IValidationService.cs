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
    public interface IValidationService
    {
   bool IsTheARExemptionValid( string user, ProjectMaster project, Performance performance);

 
    }
}

