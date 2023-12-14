using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.Services;
using BetterSteamBrowser.Business.Utilities;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Infrastructure.Context;
using BetterSteamBrowser.Infrastructure.Identity;
using BetterSteamBrowser.Infrastructure.Repositories;
using BetterSteamBrowser.Infrastructure.Repositories.Pagination;
using BetterSteamBrowser.Infrastructure.Services;
using BetterSteamBrowser.Infrastructure.SignalR;
using BetterSteamBrowser.WebCore.Components;
using Blazored.LocalStorage;
using ClipLazor.Extention;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Serilog;
using Serilog.Events;

namespace BetterSteamBrowser.WebCore;

// TODO: If previously authenticated and privileged, if they're demoted they will still have access to privileged pages until they log out and back in again.
//      And the ability action moderation stuff.

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = SetupConfiguration.ReadConfiguration();

#if DEBUG
        builder.WebHost.ConfigureKestrel(options => { options.ListenLocalhost(8123); });
#else
        builder.WebHost.UseKestrel(options =>
        {
            options.ListenAnyIP(configuration.WebBind, configure =>
                configure.UseHttps(configuration.CertificatePath, configuration.CertificatePassword));
        });
#endif

#if DEBUG
        configuration.DatabaseName = "SteamBrowserTest1";
#endif

        builder.Services.AddDbContextFactory<DataContext>(options =>
        {
            options.UseNpgsql($"{configuration.ConnectionString};Database={configuration.DatabaseName}");
        });

        // Custom Services
        builder.Services.AddSingleton(configuration);
        builder.Services.AddSingleton<ScheduledTaskRunner>();
        builder.Services.AddSingleton<ServerContextCache>();

        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();
        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddScoped<ISignalRNotification, SignalRNotificationFactory>();
        builder.Services.AddScoped<IGeoIpService, GeoIpService>();
        builder.Services.AddScoped<BsbClientHub>();
        builder.Services.AddScoped<ISteamGameRepository, SteamGameRepository>();
        builder.Services.AddScoped<IServerRepository, ServerRepository>();
        builder.Services.AddScoped<IBlockRepository, BlockRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISnapshotRepository, SnapshotRepository>();
        builder.Services.AddScoped<ISteamServerService, SteamServerService>();
        builder.Services.AddScoped<IDatabaseCleanupService, DatabaseCleanupService>();
        builder.Services.AddScoped<IStatisticsService, StatisticsService>();
        builder.Services.AddScoped<IGameServerPlayerService, GameServerPlayerService>();
        builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();

        builder.Services.AddScoped<IResourceQueryHelper<GetServerListCommand, Server>, ServersPaginationQueryHelper>();
        builder.Services.AddScoped<IResourceQueryHelper<GetBlockListCommand, Block>, BlocksPaginationQueryHelper>();
        builder.Services.AddScoped<IResourceQueryHelper<GetSteamGameListCommand, SteamGame>, SteamGamesPaginationQueryHelper>();
        builder.Services.AddScoped<IResourceQueryHelper<GetUserListCommand, User>, UsersPaginationQueryHelper>();

        // Services
        builder.Services.AddHttpClient("BSBClient", options => options.DefaultRequestHeaders.UserAgent
            .ParseAdd("BetterSteamBrowser/1.0.0"));
        builder.Services.AddLogging();
        builder.Services.AddClipboard();
        builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(GetServerListHandler).Assembly); });

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Identity
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>();
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
        });
        // Identity end

        if (!Directory.Exists(Path.Join(AppContext.BaseDirectory, "Log")))
            Directory.CreateDirectory(Path.Join(AppContext.BaseDirectory, "Log"));

        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Information()
            .MinimumLevel.Override("BanHub", LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
#else
            .MinimumLevel.Warning()
#endif
            .Enrich.FromLogContext()
            .Enrich.With<ShortSourceContextEnricher>()
            .Filter.ByExcluding(logEvent =>
                logEvent.Exception is HttpRequestException httpRequestEx &&
                httpRequestEx.Message.Contains("SSL connection could not be established"))
            .WriteTo.Console()
            .WriteTo.File(
                Path.Join(AppContext.BaseDirectory, "Log", "bsb-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 10,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] [{ShortSourceContext}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        builder.Host.UseSerilog();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<DataContext>();
            dbContext.Database.Migrate();
        }

        app.Services.GetRequiredService<ScheduledTaskRunner>().StartTimer();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.MapHub<BsbServerHub>("/SignalR/MainHub");

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
