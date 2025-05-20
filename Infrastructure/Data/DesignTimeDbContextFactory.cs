using System;
using ClickHouse.EntityFrameworkCore.Extensions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<KmloggerDbContext>
{
    public KmloggerDbContext CreateDbContext(string[] args)
    {
        try
        {
            var builder = new DbContextOptionsBuilder<KmloggerDbContext>();
            builder.UseClickHouse(Environment.GetEnvironmentVariable("ClickHouseConnectionString")
                 ?? throw new Exception("A connection string must be provided."));
            var context = new KmloggerDbContext(builder.Options);
            return context;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }
}
