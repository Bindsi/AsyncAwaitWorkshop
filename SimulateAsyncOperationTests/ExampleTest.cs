using System;
using System.Diagnostics;
using ConsoleApplication1;
using Xunit;

namespace SimulateAsyncOperationTests
{
	public class ExampleTest
	{
		[Fact]
		public async void TestMethod1()
		{
			var result = await Example.ExecuteAsync();
			Assert.NotNull(result);
		}

		[Fact]
		public async void TestMethod2()
		{
			Assert.NotNull("dk");
		}
	}
}
