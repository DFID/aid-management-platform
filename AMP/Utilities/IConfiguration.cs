namespace AMP.Utilities
{
    public interface IConfiguration
    {
        string EDRMWebServiceUrl { get; }

        string IATIWebServiceUrl { get; }

        string PersonWebServiceUrl {get ;}

        string FinanceWebServiceUrl { get; }

        string BaseUrl { get; }

        string GeoURL { get; }

        string AppMode { get; }

        string TestEmail { get; }

        string SMTPClient { get; }

        string SenderEmail { get; }

        string CreateProjectFolderInVault { get; }

        string EDRMUser { get; }

    }
}