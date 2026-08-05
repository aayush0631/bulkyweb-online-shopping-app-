using FileSync.DataAccess.Data;
using FileSync.DataAccess.Repository;
using FileSync.DataAccess.Repository.IRepository;
using FileSync.Services.Implementations;
using FileSync.Services.Interface;
using FileSync.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.WindowsServices;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<SchedulerBackgroundService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISyncTaskService, SyncTaskService>();
builder.Services.AddScoped<ICredentialService, CredentialService>();
builder.Services.AddScoped<ISchedulerService, SchedulerService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();

// SMB services
builder.Services.AddScoped<NetworkConnectionService>();
builder.Services.AddScoped<FileCopyService>();
builder.Services.AddScoped<INetworkConnectionService, NetworkConnectionService>();
builder.Services.AddScoped<IFileCopyService, FileCopyService>();

// FTP services
builder.Services.AddScoped<FtpConnectionService>();
builder.Services.AddScoped<FtpFileCopyService>();

// Factories
builder.Services.AddScoped<IConnectionServiceFactory, ConnectionServiceFactory>();
builder.Services.AddScoped<IFileCopyServiceFactory, FileCopyServiceFactory>();

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "FileSync Worker";
});

var host = builder.Build();
host.Run();
