using System;
using BetterSteamBrowser.Infrastructure.Repositories;
using BetterSteamBrowser.Domain.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tests.Repositories;

[TestFixture]
[TestOf(typeof(ServerRepository))]
public class ServerRepositoryTest
{
    private IServerRepository _serverRepo;

    [SetUp]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
    
        serviceCollection.AddDbContext<DataContext>(options => 
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

        serviceCollection.AddLogging();
        serviceCollection.AddScoped<IServerRepository, ServerRepository>();
    
        var serviceProvider = serviceCollection.BuildServiceProvider();
    
        _serverRepo = serviceProvider.GetRequiredService<IServerRepository>();               
    }

    [Test]
    public void AddServerListAsyncTest()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _serverRepo.AddServerListAsync(null, CancellationToken.None));
        // Create a mock server data
        var newServers = new List<EFServer>
        {
            new()
            {
                Hash = "hash1",
                IpAddress = "127.0.0.1",
                Port = 27015,
                Name = "Test Server",
            }
        };
        Assert.ThrowsAsync<DbUpdateException>(async () => await _serverRepo.AddServerListAsync(newServers, CancellationToken.None));
    }

    [Test]
    public async Task UpdateServerListAsyncTest()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => _serverRepo.UpdateServerListAsync(null, CancellationToken.None));
    }
}
