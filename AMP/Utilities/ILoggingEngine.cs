using System;
namespace AMP.Utilities
{
    public interface ILoggingEngine
    {
        void InsertLog(string ViewName, string user);
        void InsertLog(string ViewName, string user, string ProjectID);

        void InsertCodeLog(String MethodName, String Description, DateTime From, DateTime To, TimeSpan Result, String User);

        void InsertCodeLog(String MethodName, String Description, DateTime From, DateTime To, TimeSpan Result, String User, String Value);
    }
}
