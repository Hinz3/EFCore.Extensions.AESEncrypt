using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoFixture;

namespace EFCore.Extensions.AESEncrypt.Tests.Configurations;

public class AutoMoqSQLiteDataAttribute : AutoDataAttribute
{
    public AutoMoqSQLiteDataAttribute()
        : base(() => GetFixture())
    {
    }

    public static IFixture GetFixture()
    {
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Customize(new AutoMoqCustomization());
        fixture.Customize(new SQLiteEntityFrameworkCustomization());

        return fixture;
    }
}