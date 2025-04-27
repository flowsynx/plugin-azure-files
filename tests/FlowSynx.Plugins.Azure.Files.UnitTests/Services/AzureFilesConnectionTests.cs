using Azure.Storage.Files.Shares;
using FlowSynx.Plugins.Azure.Files.Models;
using FlowSynx.Plugins.Azure.Files.Services;

namespace FlowSynx.Plugins.Azure.Files.UnitTests.Services;

public class AzureFilesConnectionTests
{
    private readonly AzureFilesConnection _azureFilesConnection;

    public AzureFilesConnectionTests()
    {
        _azureFilesConnection = new AzureFilesConnection();
    }

    [Fact]
    public void Connect_WithValidConnectionString_ReturnsShareClient()
    {
        // Arrange
        var specifications = new AzureFilesSpecifications
        {
            ConnectionString = "DefaultEndpointsProtocol=https;AccountName=TestAccount;" +
            "AccountKey=dGVzdGtleQ==;EndpointSuffix=core.windows.net",
            ShareName = "TestShare"
        };

        // Act
        var result = _azureFilesConnection.Connect(specifications);

        // Assert
        Assert.IsType<ShareClient>(result);
    }

    [Fact]
    public void Connect_WithInvalidConnectionString_ThrowsException()
    {
        // Arrange
        var specifications = new AzureFilesSpecifications
        {
            ConnectionString = "InvalidConnectionString",
            ShareName = "TestShare"
        };

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _azureFilesConnection.Connect(specifications));
        Assert.Equal("Invalid connection string format.", exception.Message);
    }

    [Fact]
    public void Connect_WithNullConnectionString_UsesAccountNameAndKey()
    {
        // Arrange
        var specifications = new AzureFilesSpecifications
        {
            AccountName = "TestAccount",
            AccountKey = "dGVzdGtleQ==",
            ShareName = "TestShare"
        };

        // Act
        var result = _azureFilesConnection.Connect(specifications);

        // Assert
        Assert.IsType<ShareClient>(result);
    }

    [Fact]
    public void Connect_WithMissingShareName_ThrowsException()
    {
        // Arrange
        var specifications = new AzureFilesSpecifications
        {
            ConnectionString = "ValidConnectionString"
        };

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _azureFilesConnection.Connect(specifications));
        Assert.Equal("The ShareName value in azure file specifications should be not empty.", exception.Message);
    }

    [Fact]
    public void Connect_WithMissingAccountNameAndKey_ThrowsException()
    {
        // Arrange
        var specifications = new AzureFilesSpecifications
        {
            ShareName = "TestShare"
        };

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _azureFilesConnection.Connect(specifications));
        Assert.Equal("One of the ConnectionString or both AccountKey and AccountName properties " +
            "in the azure file specifications should have value.", exception.Message);
    }
}