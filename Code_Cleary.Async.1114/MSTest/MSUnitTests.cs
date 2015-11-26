using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class MSUnitTests
{
    // Warning: bad code!
    [TestMethod]
    public void IncorrectlyPassingTest()
    {
        SystemUnderTest.FailAsync();
    }

    [TestMethod]
    public async Task CorrectlyFailingTest()
    {
        await SystemUnderTest.FailAsync();
    }

    // MSTest will refuse to execute this test (UTA007).
    [TestMethod]
    public async void SchrodingerTest()
    {
        await SystemUnderTest.FailAsync();
    }

    // MSTest will refuse to execute this test (UTA007).
    [TestMethod]
    public async void SynchronizationContextNotDefinedForAsyncVoid()
    {
        Assert.IsNull(SynchronizationContext.Current);
        await Task.FromResult(0);
    }

    [TestMethod]
    public async Task SynchronizationContextNotDefinedForAsyncTask()
    {
        Assert.IsNull(SynchronizationContext.Current);
        await Task.FromResult(0);
    }

    [TestMethod]
    public void SynchronizationContextNotDefinedForSynchronousTest()
    {
        Assert.IsNull(SynchronizationContext.Current);
    }

    // Old-style; only works on desktop.
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void SynchronousFailureTest_ExpectedException()
    {
        SystemUnderTest.Fail();
    }

    // Cannot currently compile with MSTest; ThrowsException only exists for Windows Store unit test projects.
    //[TestMethod]
    //public async Task FailureTest_Ideal()
    //{
    //    await Assert.ThrowsException<Exception>(() => SystemUnderTest.FailAsync());
    //}

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public async Task FailureTest_ExpectedException()
    {
        await SystemUnderTest.FailAsync();
    }

    [TestMethod]
    public async Task FailureTest_AssertEx()
    {
        var ex = await AssertEx.ThrowsAsync(() => SystemUnderTest.FailAsync());
        Assert.IsNotNull(ex);
    }

    [TestMethod]
    public async Task RetrieveValue_SynchronousSuccess_Adds42()
    {
        var service = new Mock<IMyService>();
        service.Setup(x => x.GetAsync()).Returns(() => Task.FromResult(5));
        // Or: service.Setup(x => x.GetAsync()).ReturnsAsync(5);
        var system = new SystemUnderTest(service.Object);

        var result = await system.RetrieveValueAsync();

        Assert.AreEqual(47, result);
    }

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
    public async Task MoqHasNiceDefaultsForAsyncMethods()
    {
        var service = new Mock<IMyService>();
        var system = new SystemUnderTest(service.Object);

        var result = await system.RetrieveValueAsync();

        Assert.AreEqual(42, result);
    }
}