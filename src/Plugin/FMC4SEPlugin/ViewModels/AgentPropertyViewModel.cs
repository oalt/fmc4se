using System;
using System.Collections.ObjectModel;

using System.Linq;
using MDD4All.EnterpriseArchitect.Manipulations;
using MDD4All.FMC4SE.Plugin.DataModels;
using MDD4All.FMC4SE.Plugin.SuggestionProviders;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
	/// <summary>
	/// This class contains properties that a View can data bind to.
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class AgentPropertyViewModel : PropertyViewModel
	{
		/// <summary>
		/// Initializes a new instance of the NewAgentViewModel class.
		/// </summary>
		public AgentPropertyViewModel(EAAPI.Repository repository, EAAPI.Element agentElement) : base(repository, agentElement)
		{
			SuggestionProvider = new TypeSuggestionProvider(repository);

			Kinds = new ObservableCollection<string>
			{
				"Standard", "Software", "Chain", "Electronic", "Mechanical", "WebService"
			};
			InitializeData();

			Title = "Agent Properties";
		}

		private void InitializeData()
		{
			string typeName = AgentElement.GetClassifierName(Repository);

			TypeSuggestionProvider typeSuggestionProvider = SuggestionProvider as TypeSuggestionProvider;

			try
			{
				TypeDataModel setDataModel = typeSuggestionProvider.AvailableTypes.ToList().Find(model => model.Name.Equals(typeName));
				if (setDataModel != null)
				{
					Type = setDataModel;
				}
			}
			catch (Exception)
			{
				
				
			}
			

			string name = AgentElement.Name;
			if (!name.StartsWith("FMC4SE Agent"))
			{ 
				Name = name;
			}
            else
            {
                Name = "";
            }

			Kind = AgentElement.GetTaggedValueString("Type");
			Notes = AgentElement.Notes;
		}

		



		#region CommandImplementations


		protected override void Ok()
		{
			OperationCanceled = false;
			CanClose = true;

			if (Type != null)
			{
				AgentElement.ClassfierID = Type.ElementID;
				AgentElement.Name = Name;
				AgentElement.Notes = Notes;
				AgentElement.Update();
				AgentElement.SetTaggedValueString("Type", Kind, false);
			}
			else
			{
				CanClose = false;
			}
		}

		protected override void AddNewTypeToModel(string name)
		{
			if (UnsortedTypesPackage != null)
			{
				EAAPI.Element typeElement = UnsortedTypesPackage.Elements.AddNew(name, "Class") as EAAPI.Element;
				UnsortedTypesPackage.Elements.Refresh();

				if (typeElement != null)
				{
					typeElement.Update();

					TypeDataModel dataModel = new TypeDataModel()
					{
						Name = name,
						ElementID = typeElement.ElementID
					};

					Type = dataModel;
				}
			}
		}

		#endregion

	}
}