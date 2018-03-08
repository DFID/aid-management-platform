using System;
using System.Threading.Tasks;
using AMP.ViewModels;

namespace AMP.Services
{
    public interface IRiskService
    {
        Task<RiskItemVM> GetRiskItem(Int32 Id, string user);
    }
}