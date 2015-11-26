using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

public class XUnit2Tests
{
    // Warning: bad code!
    [Fact]
    public void IncorrectlyPassingTest()
    {
        SystemUnderTest.FailAsync();
    }

    [Fact]
    public async Task CorrectlyFailingTest()
    {
        await SystemUnderTest.FailAsync();
    }

    // Correctly fails under xUnit 2.0.0.2738 (prerelease), due to an injected Xunit.Sdk.AsyncTestSyncContext.
    [Fact]
    public async void SchrodingerTest()
    {
        await SystemUnderTest.FailAsync();
    }

    [Fact]
    public async void SynchronizationContextDefinedForAsyncVoid()
    {
        Assert.NotNull(SynchronizationContext.Current);
        await Task.FromResult(0);
    }

    // This result is surprising, IMO.
    [Fact]
    public async Task SynchronizationContextDefinedForAsyncTask()
    {
        Assert.NotNull(SynchronizationContext.Current);
        await Task.FromResult(0);
    }

    // This result is surprising, IMO.
    [Fact]
    public void SynchronizationContextDefinedForSynchronousTest()
    {
        Assert.NotNull(SynchronizationContext.Current);
    }

    [Fact]
    public async Task FailureTest_AssertThrowsAsync()
    {
        await Assert.ThrowsAsync<Exception>(() => SystemUnderTest.FailAsync());
    }

    [Fact]
    public async Task FailureTest_AssertEx()
    {
        await AssertEx.ThrowsAsync(() => SystemUnderTest.FailAsync());
    }

    [Fact]
    public async Task RetrieveValue_SynchronousSuccess_Adds42()
    {
        var service = new Mock<IMyService>();
        service.Setup(x => x.GetAsync()).Returns(() => Task.FromResult(5));
        // Or: service.Setup(x => x.GetAsync()).ReturnsAsync(5);
        var system = new SystemUnderTest(service.Object);

        var result = await system.RetrieveValueAsync();

        Assert.Equal(47, result);
    }

    [Fact]
    public async Task RetrieveValue_AsynchronousSuccess_Adds42()
    {
        var service = new Mock<IMyService>();
        service.Setup(x => x.GetAsync()).Returns(async () =>
        {
            await Task.Yield();
            return 5;
        });
        var system = new SystemUnderTest(service.Object);

        var result = await system.RetrieveValueAsync();

        Assert.Equal(47, result);
    }

    [Fact]
    public async Task RetrieveValue_AsynchronousFailure_Adds42()
    {
        var service = new Mock<IMyService>();
        service.Setup(x => x.GetAsync()).Returns(async () =>
        {
            await Task.Yield();
            throw new Exception();
        });
        var system = new SystemUnderTest(service.Object);

        await AssertEx.ThrowsAsync(system.RetrieveValueAsync);
    }

    [Fact]
    public async Task MoqHasNiceDefaultsForAsyncMethods()
    {
        var service = new Mock<IMyService>();
        var system = new SystemUnderTest(service.Object);

        var result = await system.RetrieveValueAsync();

        Assert.Equal(42, result);
    }
}
