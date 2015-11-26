using System;
using System.Threading.Tasks;
using Xunit;

namespace AssertExUnitTests
{
    public class AssertExXUnit1Tests
    {
        [Fact]
        public async Task NoException_Synchronous_Fails()
        {
            try
            {
                await AssertEx.ThrowsAsync(() => Task.FromResult(0));
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.Equal("Delegate did not throw expected exception Exception.", ex.Message);
            }
        }

        [Fact]
        public async Task NoException_Asynchronous_Fails()
        {
            try
            {
                await AssertEx.ThrowsAsync(() => Task.Delay(100));
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.Equal("Delegate did not throw expected exception Exception.", ex.Message);
            }
        }

        [Fact]
        public async Task ExpectedException_Synchronous_Passes()
        {
            var thrown = new Exception();
            var result = await AssertEx.ThrowsAsync(() => { throw thrown; });
            Assert.Same(thrown, result);
        }

        [Fact]
        public async Task ExpectedException_Asynchronous_Passes()
        {
            var thrown = new Exception();
            var result = await AssertEx.ThrowsAsync(async () =>
            {
                await Task.Delay(100);
                throw thrown;
            });
            Assert.Same(thrown, result);
        }

        [Fact]
        public async Task ExpectedDerivedException_Synchronous_Passes()
        {
            var thrown = new InvalidOperationException();
            var result = await AssertEx.ThrowsAsync<Exception>(() => { throw thrown; });
            Assert.Same(thrown, result);
        }

        [Fact]
        public async Task ExpectedDerivedException_Asynchronous_Passes()
        {
            var thrown = new InvalidOperationException();
            var result = await AssertEx.ThrowsAsync<Exception>(async () =>
            {
                await Task.Delay(100);
                throw thrown;
            });
            Assert.Same(thrown, result);
        }

        [Fact]
        public async Task UnexpectedDerivedException_Synchronous_Fails()
        {
            try
            {
                var thrown = new InvalidOperationException();
                var result = await AssertEx.ThrowsAsync<Exception>(() => { throw thrown; }, false);
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.Equal("Delegate threw exception of type InvalidOperationException, but Exception was expected.", ex.Message);
            }
        }

        [Fact]
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
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.Equal("Delegate threw exception of type InvalidOperationException, but Exception was expected.", ex.Message);
            }
        }

        [Fact]
        public async Task UnexpectedException_Synchronous_Fails()
        {
            try
            {
                var thrown = new InvalidOperationException();
                var result = await AssertEx.ThrowsAsync<ArgumentException>(() => { throw thrown; });
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.Equal("Delegate threw exception of type InvalidOperationException, but ArgumentException or a derived type was expected.", ex.Message);
            }
        }

        [Fact]
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
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.Equal("Delegate threw exception of type InvalidOperationException, but ArgumentException or a derived type was expected.", ex.Message);
            }
        }
    }
}
