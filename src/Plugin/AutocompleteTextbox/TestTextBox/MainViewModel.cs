using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using GalaSoft.MvvmLight;

namespace TestTextBox
{
	/// <summary>
	/// This class contains properties that a View can data bind to.
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
		{
			TestItems = new ObservableCollection<string>();

			TestItems.Add("USB");
			TestItems.Add("USB2");
			TestItems.Add("USB3");
			TestItems.Add("USB4");
			TestItems.Add("VGA");
			TestItems.Add("UART");
		}

		private string _text = "USB";

		public string Text
		{
			get { return _text; }
			set
			{
				_text = value;
				RaisePropertyChanged("Text");
			}
		}


		public ObservableCollection<string> TestItems { get; set; }


	}
}