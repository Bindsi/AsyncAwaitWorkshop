using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimulateAsyncOperation
{
	class Program
	{
		public void Start()
		{
			DateTime t1 = DateTime.Now;
			PrintPrimaryNumbers();
			var ts1 = DateTime.Now.Subtract(t1);
			Console.WriteLine("Finished Sync and started Async");
			var t2 = DateTime.Now;
			PrintPrimaryNumbersAsync();
			var ts2 = DateTime.Now.Subtract(t2);

			Console.WriteLine(string.Format(
			  "It took {0} for the sync call and {1} for the Async one", ts1, ts2));

			Console.WriteLine("Any Key to terminate!!");
			Console.ReadKey();
		}
		private IEnumerable<int> getPrimes(int min, int count)
		{
			return Enumerable.Range(min, count).Where
			  (n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i =>
				n % i > 0));
		}
		private Task<IEnumerable<int>> getPrimesAsync(int min, int count)
		{
			return Task.Run(() => Enumerable.Range(min, count).Where
			(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i =>
			  n % i > 0)));
		}
		private void PrintPrimaryNumbers()
		{
			for (int i = 0; i < 2; i++)
				getPrimes(i * 1 + 1, i * 5)
					.ToList().
					ForEach(x => Console.WriteLine(x));
		}
		private async Task PrintPrimaryNumbersAsync()
		{
			for (int i = 0; i < 2; i++)
			{
				var result = await getPrimesAsync(i * 1 + 1, i * 5);
				result.ToList().ForEach(x => Console.WriteLine(x));
			}
		}
	}
}
