using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssertExUnitTests
{
    [TestClass]
    public class AssertExMSUnitTests
    {
        [TestMethod]
        public async Task NoException_Synchronous_Fails()
        {
            try
            {
                await AssertEx.ThrowsAsync(() => Task.FromResult(0));
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Delegate did not throw expected exception Exception.", ex.Message);
            }
        }

        [TestMethod]
        public async Task NoException_Asynchronous_Fails()
        {
            try
            {
                await AssertEx.ThrowsAsync(() => Task.Delay(100));
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Delegate did not throw expected exception Exception.", ex.Message);
            }
        }

        [TestMethod]
        public async Task ExpectedException_Synchronous_Passes()
        {
            var thrown = new Exception();
            var result = await AssertEx.ThrowsAsync(() => { throw thrown; });
            Assert.AreSame(thrown, result);
        }

        [TestMethod]
        public async Task ExpectedException_Asynchronous_Passes()
        {
            var thrown = new Exception();
            var result = await AssertEx.ThrowsAsync(async () =>
            {
                await Task.Delay(100);
                throw thrown;
            });
            Assert.AreSame(thrown, result);
        }

        [TestMethod]
        public async Task ExpectedDerivedException_Synchronous_Passes()
        {
            var thrown = new InvalidOperationException();
            var result = await AssertEx.ThrowsAsync<Exception>(() => { throw thrown; });
            Assert.AreSame(thrown, result);
        }

        [TestMethod]
        public async Task ExpectedDerivedException_Asynchronous_Passes()
        {
            var thrown = new InvalidOperationException();
            var result = await AssertEx.ThrowsAsync<Exception>(async () =>
            {
                await Task.Delay(100);
                throw thrown;
            });
            Assert.AreSame(thrown, result);
        }

        [TestMethod]
        public async Task UnexpectedDerivedException_Synchronous_Fails()
        {
            try
            {
                var thrown = new InvalidOperationException();
                var result = await AssertEx.ThrowsAsync<Exception>(() => { throw thrown; }, false);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Delegate threw exception of type InvalidOperationException, but Exception was expected.", ex.Message);
            }
        }

        [TestMethod]
        public async Task UnexpectedDerivedException_Asynchronous_Fails()
        {
            try
            {
                var thrown = new InvalidOperationException();
                var result = await AssertEx.ThrowsAsync<Exception>(async () =>
                {
                    await Task.Delay(100);
                    throw thrown;
                }, false);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Delegate threw exception of type InvalidOperationException, but Exception was expected.", ex.Message);
            }
        }

        [TestMethod]
        public async Task UnexpectedException_Synchronous_Fails()
        {
            try
            {
                var thrown = new InvalidOperationException();
                var result = await AssertEx.ThrowsAsync<ArgumentException>(() => { throw thrown; });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Delegate threw exception of type InvalidOperationException, but ArgumentException or a derived type was expected.", ex.Message);
            }
        }

        [TestMethod]
        public async Task UnexpectedException_Asynchronous_Fails()
        {
            try
            {
                var thrown = new InvalidOperationException();
                var result = await AssertEx.ThrowsAsync<ArgumentException>(async () =>
                {
                    await Task.Delay(100);
                    throw thrown;
                });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Delegate threw exception of type InvalidOperationException, but ArgumentException or a derived type was expected.", ex.Message);
            }
        }
    }
}
