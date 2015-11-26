using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

[TestFixture]
public class NUnit2Tests
{
    // Warning: bad code!
    [Test]
    public void IncorrectlyPassingTest()
    {
        SystemUnderTest.FailAsync();
    }

    [Test]
    public async Task CorrectlyFailingTest()
    {
        await SystemUnderTest.FailAsync();
    }

    // Correctly fails under NUnit 2.6.3, due to an injected NUnit.Framework.AsyncSynchronizationContext.
    // NOTE: NUnit 2.9.7 is expected to remove support for async void unit tests.
    [Test]
    public async void SchrodingerTest()
    {
        await SystemUnderTest.FailAsync();
    }

    // NOTE: NUnit 2.9.7 is expected to remove support for async void unit tests.
    [Test]
    public async void SynchronizationContextDefinedForAsyncVoid()
    {
        Assert.IsNotNull(SynchronizationContext.Current);
        await Task.FromResult(0);
    }

    [Test]
    public async Task SynchronizationContextNotDefinedForAsyncTask()
    {
        Assert.IsNull(SynchronizationContext.Current);
        await Task.FromResult(0);
    }

    [Test]
    public void SynchronizationContextNotDefinedForSynchronousTest()
    {
        Assert.IsNull(SynchronizationContext.Current);
    }

    [Test]
    public void FailureTest_AssertThrows()
    {
        // This works, though it actually implements a nested loop,
        //  synchronously blocking the Assert.Throws call until the asynchronous
        //  FailAsync call completes.
        Assert.Throws<Exception>(async () => await SystemUnderTest.FailAsync());
    }

    // Does NOT pass.
    [Test]
    public void BadFailureTest_AssertThrows()
    {
        Assert.Throws<Exception>(() => SystemUnderTest.FailAsync());
    }

    [Test]
    [ExpectedException]
    public async Task FailureTest_ExpectedException()
    {
        await SystemUnderTest.FailAsync();
    }

    [Test]
    public void FailureTest_AssertThat()
    {
        // This also works, though it actually implements a nested loop,
        //  synchronously blocking the Assert.That call until the asynchronous
        //  FailAsync call completes.
        Assert.That(async () => await SystemUnderTest.FailAsync(), Throws.InstanceOf<Exception>());
    }

    // Does NOT pass.
    [Test]
    public void BadFailureTest_AssertThat()
    {
        Assert.That(() => SystemUnderTest.FailAsync(), Throws.InstanceOf<Exception>());
    }

    [Test]
    public async Task FailureTest_AssertEx()
    {
        await AssertEx.ThrowsAsync(() => SystemUnderTest.FailAsync());
    }

    [Test]
    public async Task RetrieveValue_SynchronousSuccess_Adds42()
    {
        var service = new Mock<IMyService>();
        service.Setup(x => x.GetAsync()).Returns(() => Task.FromResult(5));
        // Or: service.Setup(x => x.GetAsync()).ReturnsAsync(5);
        var system = new SystemUnderTest(service.Object);

        var result = await system.RetrieveValueAsync();

        Assert.AreEqual(47, result);
    }

    [Test]
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

        Assert.AreEqual(47, result);
    }

    [Test]
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

    [Test]
    public async Task MoqHasNiceDefaultsForAsyncMethods()
    {
        var service = new Mock<IMyService>();
        var system = new SystemUnderTest(service.Object);

        var result = await system.RetrieveValueAsync();

        Assert.AreEqual(42, result);
    }
}