using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Business.Services;
using MediatR;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Tests.Mediatr.Commands;

[TestFixture]
[TestOf(typeof(GetInformationHandler))]
public class GetInformationHandlerTest
{
    private Mock<IServerContextCache> _contextCache;
    private Mock<IPublisher> _publisher;
    private GetInformationCommand _getInformationCommand;
    private GetInformationHandler _getInformationHandler;

    [SetUp]
    public void Setup()
    {
        _contextCache = new Mock<IServerContextCache>();
        _publisher = new Mock<IPublisher>();
        _getInformationCommand = new GetInformationCommand();
        _getInformationHandler = new GetInformationHandler(_contextCache.Object, _publisher.Object);
    }

    [Test]
    public async Task Handle_WhenContextCacheIsLoaded_ReturnsCacheInfo()
    {
        // Arrange
        _contextCache.Setup(x => x.Loaded).Returns(true);
        _contextCache.Setup(x => x.ServerCount).Returns(5);
        _contextCache.Setup(x => x.PlayerCount).Returns(10);

        // Act
        var result = await _getInformationHandler.Handle(_getInformationCommand, CancellationToken.None);

        // Assert
        Assert.That(result.ServerCount, Is.EqualTo(5));
        Assert.That(result.PlayerCount, Is.EqualTo(10));
    }

    [Test]
    public async Task Handle_WhenContextCacheIsNotLoaded_UpdatesCache()
    {
        // Arrange
        _contextCache.Setup(x => x.Loaded).Returns(false);

        // Act
        await _getInformationHandler.Handle(_getInformationCommand, CancellationToken.None);

        // Assert
        _publisher.Verify(x => x.Publish(It.IsAny<UpdateInformationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
