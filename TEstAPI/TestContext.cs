using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TEstAPI.Entities;

namespace TEstAPI;

public class TestContext : DbContext
{
    public TestContext(DbContextOptions<TestContext> options)
        : base (options)
    {
        
    }

    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
