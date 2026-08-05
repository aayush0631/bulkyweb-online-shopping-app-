using FileSync.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSync.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<SyncTask> SyncTasks { get; set; }

    public DbSet<Credential> Credentials { get; set; }

    public DbSet<Schedule> Schedules { get; set; }

    public DbSet<CopyHistory> CopyHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fluent API configurations will go here.
    }
}