using System;
namespace AMP.Utilities
{
   public interface IErrorEngine
    {
        void LogError(Exception ex, String user);
        void LogError(string ProjectID, Exception ex, string user);
        void LogError(Exception ex, String CustomException, string user);
       void LogError(String projectId, Exception ex, String CustomError, string user);

    }
}
