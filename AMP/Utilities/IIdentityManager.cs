using System;
using AMP.Models;

namespace AMP.Utilities
{
    public interface IIdentityManager
    {
        Person GetPersonByUserName(string userName);
    }
}
