using FileSync.DataAccess.Data;
using FileSync.DataAccess.Repository;
using FileSync.DataAccess.Repository.IRepository;
using FileSync.Services.Implementations;
using FileSync.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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

// Factories (choose SMB vs FTP at runtime)
builder.Services.AddScoped<IConnectionServiceFactory, ConnectionServiceFactory>();
builder.Services.AddScoped<IFileCopyServiceFactory, FileCopyServiceFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

