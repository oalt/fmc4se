using MDD4All.FMC4SE.Plugin.ViewModels;
using System.Windows;

namespace MDD4All.FMC4SE.Plugin.Views
{

    public partial class ConnectorDirectionWindow : Window
    {
        public ConnectorDirectionWindow()
        {
            InitializeComponent();
        }

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			ConnectorDirectionViewModel connectorDirectionViewModel = DataContext as ConnectorDirectionViewModel;

			if (connectorDirectionViewModel != null)
			{
				connectorDirectionViewModel.OkCommand.Execute(null);

				Close();
				
			}
		}


		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
