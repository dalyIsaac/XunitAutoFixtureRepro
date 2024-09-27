using AutoFixture;

namespace XunitAutoFixtureRepro;

public record Window(string Name, int Height, int Width);
public record Monitor(int Height, int Width);

public class ExampleCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Inject(new Window("Main", 100, 200));
        fixture.Inject(new Monitor(100, 200));
    }
}

public class UnitTest1
{
    [Fact]
    public void PlainTest()
    {
        Assert.Equal(123, 123);
    }

    [Theory, AutoSubstituteData<ExampleCustomization>]
    public void TestWithAutoFixture(Window window)
    {
        Assert.Equal("Main", window.Name);
        Assert.Equal(100, window.Height);
        Assert.Equal(200, window.Width);
    }

    [Theory, InlineAutoSubstituteData<ExampleCustomization>("hello world")]
    public void TestWithAutoFixtureInline(string value, Window window, Monitor monitor)
    {
        Assert.Equal("hello world", value);
        Assert.Equal("Main", window.Name);
        Assert.Equal(100, window.Height);
        Assert.Equal(200, window.Width);
        Assert.Equal(100, monitor.Height);
        Assert.Equal(200, monitor.Width);
    }

    public static TheoryData<string, int> GetData() => new()
    {
            { "this is the first string", 24 },
            { "this is the second string with a much longer length", 51 }
        };

    [Theory, MemberAutoSubstituteData<ExampleCustomization>(nameof(GetData))]
    public void TestWithAutoFixtureMember(string value, int length, Window window, Monitor monitor)
    {
        Assert.Equal(value.Length, length);
        Assert.Equal("Main", window.Name);
        Assert.Equal(100, window.Height);
        Assert.Equal(200, window.Width);
        Assert.Equal(100, monitor.Height);
        Assert.Equal(200, monitor.Width);
    }
}