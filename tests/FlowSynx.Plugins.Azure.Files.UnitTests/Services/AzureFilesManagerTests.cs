using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using FlowSynx.PluginCore;
using FlowSynx.Plugins.Azure.Files.Services;
using Moq;

namespace FlowSynx.Plugins.Azure.Files.UnitTests.Services;

public class AzureFilesManagerTests
{
    private readonly Mock<IPluginLogger> _mockLogger;
    private readonly Mock<ShareClient> _mockClient;
    private readonly AzureFilesManager _azureFilesManager;

    public AzureFilesManagerTests()
    {
        _mockLogger = new Mock<IPluginLogger>();
        _mockClient = new Mock<ShareClient>();
        _azureFilesManager = new AzureFilesManager(_mockLogger.Object, _mockClient.Object);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AzureFilesManager(null, _mockClient.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenClientIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AzureFilesManager(_mockLogger.Object, null));
    }

    [Fact]
    public async Task Create_ShouldCallCreateEntity_WhenValidParametersArePassed()
    {
        // Arrange
        var parameters = new PluginParameters
        {
            { "path", "test/test/" }
        };
        var cancellationToken = new CancellationToken();

        _mockClient.Setup(client => client
            .GetDirectoryClient(It.IsAny<string>()))
            .Returns(Mock.Of<ShareDirectoryClient>());

        // Act
        await _azureFilesManager.Create(parameters, cancellationToken);

        // Assert
        _mockClient.Verify(client => client.GetDirectoryClient(It.IsAny<string>()));
    }

    [Fact]
    public async Task Delete_ShouldCallDeleteEntity_WhenValidParametersArePassed()
    {
        // Arrange
        var path = "test/test/test.dat";
        var parameters = new PluginParameters
        {
            { "path", path }
        };
        var cancellationToken = new CancellationToken();

        var mockRootDirectoryClient = new Mock<ShareDirectoryClient>();
        var mockFileClient = new Mock<ShareFileClient>();

        mockRootDirectoryClient.Setup(client => client.GetFileClient(path))
                               .Returns(mockFileClient.Object);

        _mockClient.Setup(client => client.GetRootDirectoryClient())
                   .Returns(mockRootDirectoryClient.Object);

        mockFileClient.Setup(client => client.DeleteIfExistsAsync(It.IsAny<ShareFileRequestConditions>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Response.FromValue(true, Mock.Of<Response>()));

        // Act
        await _azureFilesManager.Delete(parameters, cancellationToken);

        // Assert
        mockFileClient.Verify(client => client.DeleteIfExistsAsync(null, cancellationToken), Times.Once());
    }

    [Fact]
    public async Task Exist_ShouldReturnTrue_WhenEntityExists()
    {
        // Arrange
        var path = "test/test/test.dat";
        var parameters = new PluginParameters
        {
            { "path", path }
        };
        var cancellationToken = new CancellationToken();

        var mockRootDirectoryClient = new Mock<ShareDirectoryClient>();
        var mockFileClient = new Mock<ShareFileClient>();

        mockRootDirectoryClient.Setup(client => client.GetFileClient(path))
                               .Returns(mockFileClient.Object);

        _mockClient.Setup(client => client.GetRootDirectoryClient())
                   .Returns(mockRootDirectoryClient.Object);

        mockFileClient.Setup(client => client.ExistsAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Response.FromValue(true, Mock.Of<Response>()));

        // Act
        var result = await _azureFilesManager.Exist(parameters, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ListEntities_ShouldThrowException_WhenPathIsNotDirectory()
    {
        // Arrange
        var parameters = new PluginParameters
        {
            { "path", "test/test/" }
        };
        var cancellationToken = new CancellationToken();

        _mockClient.Setup(client => client.GetDirectoryClient(It.IsAny<string>())).Throws(new Exception("Not a directory"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _azureFilesManager.List(parameters, cancellationToken));
    }

    [Fact]
    public async Task ListEntities_ShouldHandleCancellation()
    {
        // Arrange
        var parameters = new PluginParameters
        {
            { "path", "test/test/" }
        };
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        var cancellationToken = cancellationTokenSource.Token;

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => _azureFilesManager.List(parameters, cancellationToken));
    }

    [Fact]
    public async Task Purge_ShouldCallPurgeEntity_WhenValidParametersArePassed()
    {
        // Arrange
        var parameters = new PluginParameters
        {
            { "path", "test/test/" }
        };
        var cancellationToken = new CancellationToken();

        _mockClient.Setup(client => client
            .GetDirectoryClient(It.IsAny<string>()))
            .Returns(Mock.Of<ShareDirectoryClient>());

        // Act
        await _azureFilesManager.Purge(parameters, cancellationToken);

        // Assert
        _mockClient.Verify(client => client.GetDirectoryClient(It.IsAny<string>()), Times.Once);
    }
}