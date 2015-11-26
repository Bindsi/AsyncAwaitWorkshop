using System;
using System.Threading;
using System.Threading.Tasks;
using Global.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class MSStubTests
{
    [TestMethod]
    public async Task RetrieveValue_SynchronousSuccess_Adds42()
    {
        var stub = new StubIMyService
        {
            GetAsync = () => Task.FromResult(5),
        };
        var system = new SystemUnderTest(stub);

        var result = await system.RetrieveValueAsync();

        Assert.AreEqual(47, result);
    }

    [TestMethod]
    public async Task RetrieveValue_AsynchronousSuccess_Adds42()
    {
        var service = new StubIMyService
        {
            GetAsync = async () =>
            {
                await Task.Yield();
                return 5;
            },
        };
        var system = new SystemUnderTest(service);

        var result = await system.RetrieveValueAsync();

        Assert.AreEqual(47, result);
    }

    [TestMethod]
    public async Task RetrieveValue_AsynchronousFailure_Adds42()
    {
        var service = new StubIMyService
        {
            GetAsync = async () =>
            {
                await Task.Yield();
                throw new Exception();
            },
        };
        var system = new SystemUnderTest(service);

        await AssertEx.ThrowsAsync(system.RetrieveValueAsync);
    }

    [TestMethod]
    public async Task Stubs_NotAsNiceDefaultsForAsyncMethods()
    {
        var service = new StubIMyService();
        service.BehaveAsDefaultValue();
        var system = new SystemUnderTest(service);

        var result = await system.RetrieveValueAsync();

        Assert.AreEqual(42, result);
    }
}