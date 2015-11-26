using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShowDebugging
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private async void btnStepInto_Click(object sender, RoutedEventArgs e)
		{
			await ProcessAsync();
		}

		async Task ProcessAsync()
		{
			var result = await DoSomethingAsync();  // Step Into or Step Over from here

			int y = 0;
		}

		async Task<int> DoSomethingAsync()
		{
			await Task.Delay(5000);
			return 5;
		}

		private async void btnStepOut_Click(object sender, RoutedEventArgs e)
		{
			await ExecuteAsync();
		}

		async Task ExecuteAsync()
		{
			var theTask = DoAsync();
			int z = 0;
			var result = await theTask;
		}

		async Task<int> DoAsync()
		{
			resultsTextBox.Text += "\nbefore";  // Step Out from here
			Debug.WriteLine("before");  // Step Out from here
			await Task.Delay(1000);
			resultsTextBox.Text += "\nafter";
			return 5;
		}
	}
}
