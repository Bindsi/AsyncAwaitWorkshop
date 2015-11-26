using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimulateAsyncOperation.Debugging;

namespace ConsoleApplication1
{
	public class Example
	{
		static void Main(string[] args)
		{

			DoSomethingAsync();
			Console.WriteLine($"MainThread - Thread-Id {Thread.CurrentThread.ManagedThreadId}");
			Console.ReadKey();
		}

		private static async void DoSomethingAsync()
		{
			var task = ExecuteAsync();

			//DoMoreWork();

			var str = await task;

			Console.WriteLine("Result: " + str);
			Console.WriteLine($"NewThread - Thread-Id {Thread.CurrentThread.ManagedThreadId}");
		}

		private static void DoMoreWork()
		{
			Console.WriteLine($"MoreWork - Thread-Id {Thread.CurrentThread.ManagedThreadId}");
		}

		public static Task<string> ExecuteAsync()
		{
			return Task.Factory.StartNew(GetString, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current )

			return Task.Run(() =>
			{
				Console.WriteLine($"NewThread - Thread-Id {Thread.CurrentThread.ManagedThreadId}");
				var retVal = string.Empty;
				for (int i = 0; i < 100000000; ++i)
				{
					retVal = i.ToString();
				}
				File.AppendAllText(@"c:\users\marcbind\desktop\background.txt", retVal);
				return $"Loops: {retVal}";
			});
		}

		private static string GetString()
		{
			Console.WriteLine($"NewThread - Thread-Id {Thread.CurrentThread.ManagedThreadId}");
			var retVal = string.Empty;
			for (int i = 0; i < 100000000; ++i)
			{
				retVal = i.ToString();
			}
			File.AppendAllText(@"c:\users\marcbind\desktop\background.txt", retVal);
			return $"Loops: {retVal}";

		}
		#region UnusedExamples
		private static Task<int> ReadTask(Stream stream, byte[] buffer, int offset, int count, object state)
		{
			var tcs = new TaskCompletionSource<int>();
			stream.BeginRead(buffer, offset, count, ar =>
			{
				try { tcs.SetResult(stream.EndRead(ar)); }
				catch (Exception exc) { tcs.SetException(exc); }
			}, state);
			return tcs.Task;
		}

		private static void StartFromAsync()
		{
			var content = new MemoryStream();
			WebRequest wr = WebRequest.Create("http://www.bing.com");
			var webTask = Task<WebResponse>.Factory.FromAsync // StartFromAsync() not needed.
			(
				wr.BeginGetResponse(null, null),
				wr.EndGetResponse
			);
			var responseStream = webTask.Result.GetResponseStream();
			responseStream?.CopyTo(content);
			byte[] bytes = content.ToArray();
			var str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			Console.WriteLine(str);
			webTask.Wait();
		}
		#endregion
	}
}
