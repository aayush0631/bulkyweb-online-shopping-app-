using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SftpFileSync.Worker;
using SftpFileSync.Worker.Models;
using SftpFileSync.Worker.Services;

// Create the application builder.
// This loads configuration (appsettings.json), logging,
// dependency injection container, and host settings.
var builder = Host.CreateApplicationBuilder(args);

// Configure Serilog as the application's logging provider.
builder.Services.AddSerilog((services, loggerConfiguration) =>
{
    loggerConfiguration
        // Record Information, Warning, Error, and Fatal logs.
        .MinimumLevel.Information()

        // Display logs in the console (useful while developing).
        .WriteTo.Console()

        // Save logs to a file.
        // A new log file is created every day.
        .WriteTo.File(
            "logs/sync-.log",
            rollingInterval: RollingInterval.Day);
});


// Configure the application to run as a Windows Service.
// Once installed, Windows Service Manager will control
// starting, stopping, and restarting this application.
builder.Services.AddWindowsService();


// Load the "Sftp" section from appsettings.json
// into the SftpSettings class.
builder.Services.Configure<SftpConfiguration>(
    builder.Configuration);

// Load the "Schedule" section from appsettings.json
// into the ScheduleSettings class.
builder.Services.Configure<ScheduleSettings>(
    builder.Configuration.GetSection("Schedule"));


// Register application services with Dependency Injection.
//
// Singleton means only ONE instance of each service
// exists during the entire lifetime of the application.
builder.Services.AddSingleton<ISftpSyncService, SftpSyncService>();
builder.Services.AddSingleton<ISftpClientService, SftpClientService>();


// Register the background worker.
// This class contains the application's main execution loop.
builder.Services.AddHostedService<Worker>();


// Build the configured host.
var host = builder.Build();

// Start the application.
// This call blocks until the Windows Service is stopped.
host.Run();