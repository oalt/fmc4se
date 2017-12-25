using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
	/// <summary>
	/// This class contains properties that a View can data bind to.
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class NewTypeViewModel : ViewModelBase
	{
		/// <summary>
		/// Initializes a new instance of the NewTypeViewModel class.
		/// </summary>
		public NewTypeViewModel()
		{
			IsCanceled = true;

			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);
		}

		private string _name;

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value; 
				RaisePropertyChanged("Name");
			}
		}

		public bool IsCanceled { get; set; }

		public ICommand CancelCommand { get; }

		public ICommand OkCommand { get; }
		
		public void Cancel()
		{
			IsCanceled = true;
		}

		public void Ok()
		{
			IsCanceled = false;
		}
	}
}