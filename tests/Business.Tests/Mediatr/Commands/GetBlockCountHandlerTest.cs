using BetterSteamBrowser.Business.Mediatr.Commands;
using NUnit.Framework;
using Moq;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Tests.Mediatr.Commands;

[TestFixture]
[TestOf(typeof(GetBlockCountHandler))]
public class GetBlockCountHandlerTest
{
    private Mock<IBlockRepository> _mockBlockRepository;

    [SetUp]
    public void Setup()
    {
        _mockBlockRepository = new Mock<IBlockRepository>();
    }

    [Test]
    public async Task Handle_GetBlockCountCommand_ReturnsBlockCount()
    {
        // Arrange
        var handler = new GetBlockCountHandler(_mockBlockRepository.Object);
        _mockBlockRepository.Setup(repo => repo.CountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(5);
        var request = new GetBlockCountCommand();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(5));
        _mockBlockRepository.Verify(repo => repo.CountAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task Handle_GetBlockCountCommand_ZeroBlocks_ReturnsZero()
    {
        // Arrange
        var handler = new GetBlockCountHandler(_mockBlockRepository.Object);
        _mockBlockRepository.Setup(repo => repo.CountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
        var request = new GetBlockCountCommand();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(0));
        _mockBlockRepository.Verify(repo => repo.CountAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
