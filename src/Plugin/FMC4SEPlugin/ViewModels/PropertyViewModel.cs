using System.Collections.ObjectModel;
using System.Windows.Input;
using Autocomplete.WPF.Editors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MDD4All.FMC4SE.Plugin.DataModels;
using MDD4All.FMC4SE.Plugin.Views;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
	public abstract class PropertyViewModel : ViewModelBase
	{

		protected EAAPI.Repository Repository;
		protected EAAPI.Element AgentElement;
		
		public PropertyViewModel(EAAPI.Repository repository, EAAPI.Element agentElement)
		{
			Repository = repository;
			AgentElement = agentElement;

			OperationCanceled = true;

			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);
			NewTypeCommand = new RelayCommand(AddNewType);

			FindOrCreateUnsordedTypesPackage();
		}

		private void FindOrCreateUnsordedTypesPackage()
		{

			EAAPI.Package model = null;

			for (short modelCount = 0; modelCount < Repository.Models.Count; modelCount++)
			{
				EAAPI.Package modelPackage = Repository.Models.GetAt(modelCount) as EAAPI.Package;
				if (modelPackage!=null && modelPackage.Name == "Model")
				{
					model = modelPackage;
					break;
				}
			}

			if (model == null)
			{
				model = Repository.Models.AddNew("Model", "Package") as EAAPI.Package;
                model.Update();
				Repository.Models.Refresh();
			}

			EAAPI.Package globalLibraryPackage = null;

			for (short index = 0; index < model.Packages.Count; index++)
			{
				EAAPI.Package package = model.Packages.GetAt(index) as EAAPI.Package;
				if (package != null && package.Name == "Global Model Library")
				{
					globalLibraryPackage = package;
					break;
				}
			}

			if (globalLibraryPackage == null)
			{
				globalLibraryPackage = model.Packages.AddNew("Global Model Library", "Package") as EAAPI.Package;
				globalLibraryPackage.Update();
				model.Packages.Refresh();
			}

			EAAPI.Package unsortedPackage = null;

			for (short index = 0; index < globalLibraryPackage.Packages.Count; index++)
			{
				EAAPI.Package package = globalLibraryPackage.Packages.GetAt(index) as EAAPI.Package;
				if (package != null && package.Name == "!_Unsorted")
				{
					unsortedPackage = package;
					break;
				}
			}

			if (unsortedPackage == null)
			{
                
                unsortedPackage = globalLibraryPackage.Packages.AddNew("!_Unsorted", "Package") as EAAPI.Package;
				
				globalLibraryPackage.Packages.Refresh();
                unsortedPackage.Update();
            }

            UnsortedTypesPackage = unsortedPackage;

		}


		#region Properties

		protected EAAPI.Package UnsortedTypesPackage { get; set; }

		private TypeDataModel _type;

		public TypeDataModel Type
		{
			get { return _type; }

			set
			{
				_type = value;
				RaisePropertyChanged("Type");
			}
		}

        private string _filterText = "";

        public string FilterText
        {
            get { return _filterText; }

            set {
                _filterText = value;
                RaisePropertyChanged("FilterText");
            }
        }


        private ISuggestionProvider _suggestionProvider;

		public ISuggestionProvider SuggestionProvider
		{
			get
			{
				return _suggestionProvider;
			}

			set
			{
				_suggestionProvider = value;
				RaisePropertyChanged("SuggestionProvider");
			}
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

		private string _kind;

		public string Kind
		{
			get { return _kind; }
			set
			{
				_kind = value;
				RaisePropertyChanged("Kind");
			}
		}

		public ObservableCollection<string> Kinds { get; set; }

		private string _notes;

		public string Notes
		{
			get { return _notes; }
			set
			{
				_notes = value;
				RaisePropertyChanged("Notes");
			}
		}

		private string _title = "New Agent...";

		public string Title
		{
			get { return _title; }
			set
			{
				_title = value; 
				RaisePropertyChanged("Title");
			}
		}

		#endregion

		#region CommandDefinitions

		public ICommand OkCommand { get; private set; }

		public ICommand CancelCommand { get; private set; }

		public ICommand NewTypeCommand { get; private set; }
		#endregion


		public bool OperationCanceled { get; set; }

		public bool CanClose { get; set; }


		protected void Cancel()
		{
			OperationCanceled = true;
		}

		protected abstract void Ok();

		private void AddNewType()
		{
			NewTypeWindow newTypeWindow = new NewTypeWindow();
			NewTypeViewModel newTypeViewModel = new NewTypeViewModel();

            newTypeViewModel.Name = FilterText;

			newTypeWindow.DataContext = newTypeViewModel;
			newTypeWindow.ShowDialog();

			if (!string.IsNullOrWhiteSpace(newTypeViewModel.Name) && !newTypeViewModel.IsCanceled)
			{
				AddNewTypeToModel(newTypeViewModel.Name);
			}

		}

		protected abstract void AddNewTypeToModel(string name);

	}
}
