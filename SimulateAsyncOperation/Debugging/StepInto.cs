using System.Threading.Tasks;

namespace SimulateAsyncOperation.Debugging
{
	public class StepInto
	{
		public void Start()
		{
			ProcessAsync().Wait();
		}

		async Task ProcessAsync()
		{
			var result = await DoSomethingAsync();  // Step Into or Step Over from here

			int y = 0;
		}

		async Task<int> DoSomethingAsync()
		{
			await Task.Delay(1000);
			return 5;
		}
	}
}