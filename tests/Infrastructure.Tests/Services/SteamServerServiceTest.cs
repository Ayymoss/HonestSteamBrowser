using System;
using System.Collections.Generic;
using BetterSteamBrowser.Infrastructure.Services;
using NUnit.Framework;
using Moq;
using System.Threading;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tests.Services;

[TestFixture]
[TestOf(typeof(SteamServerService))]
public class SteamServerServiceTest
{
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<IServerRepository> _serverRepositoryMock;
    private Mock<SetupConfigurationContext> _configMock;
    private Mock<IBlockRepository> _blockRepositoryMock;
    private Mock<ISteamGameRepository> _steamGameRepositoryMock;
    private Mock<IGeoIpService> _geoIpServiceMock;
    private Mock<ILogger<SteamServerService>> _loggerMock;
    private SteamServerService _steamServerService;

    [SetUp]
    public void SetUp()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _serverRepositoryMock = new Mock<IServerRepository>();
        _configMock = new Mock<SetupConfigurationContext>();
        _blockRepositoryMock = new Mock<IBlockRepository>();
        _steamGameRepositoryMock = new Mock<ISteamGameRepository>();
        _geoIpServiceMock = new Mock<IGeoIpService>();
        _loggerMock = new Mock<ILogger<SteamServerService>>();
        _steamServerService = new SteamServerService(_httpClientFactoryMock.Object, _serverRepositoryMock.Object, _configMock.Object,
            _blockRepositoryMock.Object, _steamGameRepositoryMock.Object, _geoIpServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetServerListAsync_EmptyResponse_ReturnsNull()
    {
        //Arrange
        var cancellationToken = CancellationToken.None;

        //Act
        var result = await _steamServerService.GetServerListAsync(string.Empty, cancellationToken);

        //Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void BuildBlockList_Always_ReturnsServers()
    {
        //Arrange
        List<EFServer> servers = [];

        //Act
        var result = _steamServerService.BuildBlockList(servers);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(servers.Count));
    }

    [Test]
    public void ProcessExistingServers_Always_ReturnsServers()
    {
        //Arrange
        List<(EFServer Server, ServerListItem ServerItem)> servers =
            [new ValueTuple<EFServer, ServerListItem>(new EFServer(), new ServerListItem())];

        var standardDeviations = new Dictionary<string, double> {[string.Empty] = 0.0};

        //Act
        var result = _steamServerService.ProcessExistingServers(servers, standardDeviations);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(servers.Count));
    }

    [Test]
    public void ProcessNewServers_Always_ReturnsServers()
    {
        //Arrange
        List<(ServerListItem ServerItem, string ServerHash)> steamServers =
            [new ValueTuple<ServerListItem, string>(new ServerListItem {Name = "Test", Map = "Test", Address = "127.0.0.1:27015"}, "Test")];

        //Act
        var result = _steamServerService.ProcessNewServers(steamServers);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(steamServers.Count));
    }
}
