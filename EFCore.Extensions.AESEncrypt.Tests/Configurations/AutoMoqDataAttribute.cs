using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using EFCore.Extensions.AESEncrypt.Tests.Helper;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.AESEncrypt.Tests.Configurations;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(() => GetFixture())
    {
    }

    public static IFixture GetFixture()
    {
        var fixture = new Fixture();

        var options = new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new TestContext(options);

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Register<TestContext>(() => context);
        fixture.Customize(new AutoMoqCustomization());

        return fixture;
    }
}
