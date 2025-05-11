using System;
using System.Reflection;
using Domain.Entities;
using Flunt.Notifications;
using SQLitePCL;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Hot.FluentMapping;

namespace Infrastructure.Data.Hot;

public class HotDbContext : DbContext
{
    static HotDbContext()
    {
        Batteries_V2.Init(); 
    }
    public HotDbContext(DbContextOptions<HotDbContext> options) : base(options) { }
    public DbSet<LogApp> Logs {get; init;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<Notification>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogAppMapping).Assembly);
    }
}
