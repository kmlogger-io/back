using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data.Hot;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HotDbContext>
{
    public HotDbContext CreateDbContext(string[] args)
    {
        try
        {
            if (string.IsNullOrEmpty("Data Source=" + Path.GetFullPath("../../dbkmlogger.db")))
                throw new Exception("A connection string must be provided.");
                
            var builder = new DbContextOptionsBuilder<HotDbContext>();
            builder.UseSqlite("Data Source=" + Path.GetFullPath("../../dbkmlogger.db"));
            var context = new HotDbContext(builder.Options);
            return context;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }
}
