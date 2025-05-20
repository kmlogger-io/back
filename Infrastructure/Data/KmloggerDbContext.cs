using Domain.Entities;
using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.FluentMapping;

namespace Infrastructure.Data;

public class KmloggerDbContext : DbContext
{
    public KmloggerDbContext(DbContextOptions<KmloggerDbContext> options) : base(options) { }
    public DbSet<LogApp> Logs {get; init;}
    public DbSet<App> Apps {get; init;}
    public DbSet<Category> Categories {get; init;}
    public DbSet<Picture> Pictures {get; init;}
    public DbSet<Role> Roles {get; init;}
    public DbSet<User> Users {get; init;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<Notification>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogAppMapping).Assembly);
    }
}
