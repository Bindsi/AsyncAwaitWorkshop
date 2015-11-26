using System;
using System.Threading;

namespace ShowIAsyncResultPattern
{
	public class AsyncDemo
	{
		// The method to be executed asynchronously.
		public string TestMethod(int callDuration, out int threadId)
		{
			Console.WriteLine("Test method begins.");
			Thread.Sleep(callDuration);
			threadId = Thread.CurrentThread.ManagedThreadId;
			return $"My call time was {callDuration}.";
		}
	}
}