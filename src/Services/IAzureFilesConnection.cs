using Azure.Storage.Files.Shares;
using FlowSynx.Plugins.Azure.Files.Models;

namespace FlowSynx.Plugins.Azure.Files.Services;

internal interface IAzureFilesConnection
{
    ShareClient Connect(AzureFilesSpecifications specifications);

}