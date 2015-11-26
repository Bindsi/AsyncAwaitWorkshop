using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShowIAsyncResultPattern
{
	class Program
	{
		static void Start()
		{

			// The asynchronous method puts the thread id here.
			int threadId;

			// Create an instance of the test class.
			var ad = new AsyncDemo();

			// Create the delegate.
			var caller = new AsyncMethodCaller(ad.TestMethod);

			// Initiate the asychronous call.
			var result = caller.BeginInvoke(3000,
				out threadId, null, null);

			Thread.Sleep(0);
			Console.WriteLine("Main thread {0} does some work.",
				Thread.CurrentThread.ManagedThreadId);

			// Wait for the WaitHandle to become signaled.
			result.AsyncWaitHandle.WaitOne();

			// Perform additional processing here.
			// Call EndInvoke to retrieve the results.
			var returnValue = caller.EndInvoke(out threadId, result);

			// Close the wait handle.
			result.AsyncWaitHandle.Close();

			Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
				threadId, returnValue);

			Console.ReadKey();

		}

	}

	// The delegate must have the same signature as the method
	// it will call asynchronously.
	public delegate string AsyncMethodCaller(int callDuration, out int threadId);

}
