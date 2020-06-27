using System.Windows;
using MDD4All.FMC4SE.Plugin.ViewModels;

namespace MDD4All.FMC4SE.Plugin.Views
{
	/// <summary>
	/// Interaktionslogik für NewTypeWindow.xaml
	/// </summary>
	public partial class NewTypeWindow : Window
	{
		public NewTypeWindow()
		{
			InitializeComponent();

            
        }

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			NewTypeViewModel viewModel = DataContext as NewTypeViewModel;
			if (viewModel != null)
			{
				viewModel.OkCommand.Execute(null);
			}
			Close();
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			NewTypeViewModel viewModel = DataContext as NewTypeViewModel;
			if (viewModel != null)
			{
				viewModel.CancelCommand.Execute(null);
			}
			Close();
		}
	}
}
