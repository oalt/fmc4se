using System.ComponentModel;
using System.Windows;
using MDD4All.FMC4SE.Plugin.ViewModels;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.Views
{
	/// <summary>
	/// Interaktionslogik für NewAgentWindow.xaml
	/// </summary>
	public partial class FMCElementPropertyWindow : Window
	{
		public FMCElementPropertyWindow()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			PropertyViewModel propertyViewModel = DataContext as PropertyViewModel;

			if (propertyViewModel != null)
			{
				propertyViewModel.OkCommand.Execute(null);
				if (propertyViewModel.CanClose)
				{
					Close();
				}
				else
				{
					MessageBox.Show(this, "Please set a Type!", "No Type set", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}


		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void NewAgentWindow_OnClosing(object sender, CancelEventArgs e)
		{
			AgentPropertyViewModel propertyViewModel = DataContext as AgentPropertyViewModel;

			if (propertyViewModel != null)
			{
				if (propertyViewModel.OperationCanceled)
				{
					propertyViewModel.CancelCommand.Execute(null);
				}
			}
		}
	}
}

	
