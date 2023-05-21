using AutoFixture;
using EFCore.Extensions.AESEncrypt.Tests.Helper;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.AESEncrypt.Tests.Configurations;

public class SQLiteEntityFrameworkCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var options = new DbContextOptionsBuilder<TestContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new TestContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        fixture.Register<TestContext>(() => context);
    }
}