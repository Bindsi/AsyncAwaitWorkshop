using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SimulateAsyncOperation.Debugging
{
	public class StepOut
	{
		public void Start()
		{
			ProcessAsync().Wait();
		}

		async Task ProcessAsync()
		{
			var theTask = DoSomethingAsync();
			int z = 0;
			var result = await theTask;
		}

		async Task<int> DoSomethingAsync()
		{
			Debug.WriteLine("before");  // Step Out from here
			await Task.Delay(1000);
			Debug.WriteLine("after");
			return 5;
		}

	}
}