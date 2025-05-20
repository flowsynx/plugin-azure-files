using Azure.Storage;
using Azure.Storage.Files.Shares;
using FlowSynx.Plugins.Azure.Files.Models;

namespace FlowSynx.Plugins.Azure.Files.Services;

internal class AzureFilesConnection: IAzureFilesConnection
{
    public ShareClient Connect(AzureFilesSpecifications specifications)
    {
        ValidateSpecifications(specifications);

        if (!string.IsNullOrEmpty(specifications.ConnectionString))
        {
            try
            {
                return new ShareClient(specifications.ConnectionString, specifications.ShareName);
            }
            catch (FormatException ex)
            {
                throw new Exception(Resources.AzureFilesConnectionInvalidConnectionStringFormat, ex);
            }
        }

        var uri = BuildServiceUri(specifications.AccountName, specifications.ShareName);
        var credential = CreateCredential(specifications.AccountName, specifications.AccountKey);

        return new ShareClient(shareUri: uri, credential: credential);
    }

    protected void ValidateSpecifications(AzureFilesSpecifications specifications)
    {
        if (string.IsNullOrEmpty(specifications.ShareName))
            throw new Exception(Resources.ShareNameInSpecificationShouldBeNotEmpty);

        if (string.IsNullOrEmpty(specifications.ConnectionString))
        {
            if (string.IsNullOrEmpty(specifications.AccountName) || string.IsNullOrEmpty(specifications.AccountKey))
                throw new Exception(Resources.OnePropertyShouldHaveValue);
        }
    }

    protected Uri BuildServiceUri(string accountName, string shareName)
    {
        return new Uri($"https://{accountName}.file.core.windows.net/{shareName}");
    }

    protected StorageSharedKeyCredential CreateCredential(string accountName, string accountKey)
    {
        return new StorageSharedKeyCredential(accountName, accountKey);
    }
}