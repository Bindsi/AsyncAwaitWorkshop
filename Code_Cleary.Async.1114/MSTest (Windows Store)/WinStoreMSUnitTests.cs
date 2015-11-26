using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using AsyncAssert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.Assert;

[TestClass]
public class WinStoreMSUnitTests
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

    // New-style; only works on Windows Store.
    [TestMethod]
    public void SynchronousFailureTest_AssertThrowsException()
    {
        var ex = Assert.ThrowsException<Exception>(() => { SystemUnderTest.Fail(); });
    }

    public async Task FailureTest_AssertThrowsException()
    {
        var x = await AsyncAssert.ThrowsException<Exception>(() => SystemUnderTest.FailAsync());
        Assert.IsNotNull(x);
    }

    [TestMethod]
    public async Task FailureTest_AssertEx()
    {
        var x = await AssertEx.ThrowsAsync(() => SystemUnderTest.FailAsync());
        Assert.IsNotNull(x);
    }
}