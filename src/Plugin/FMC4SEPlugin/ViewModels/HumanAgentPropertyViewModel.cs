using System;
using System.Collections.ObjectModel;
using System.Linq;
using MDD4All.EnterpriseArchitect.Manipulations;
using MDD4All.FMC4SE.Plugin.DataModels;
using MDD4All.FMC4SE.Plugin.SuggestionProviders;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
    public class HumanAgentPropertyViewModel : PropertyViewModel
    {
        public HumanAgentPropertyViewModel(EAAPI.Repository repository, EAAPI.Element agentElement) : base(repository, agentElement)
        {
            SuggestionProvider = new ActorSuggestionProvider(repository);

            Kinds = new ObservableCollection<string>
            {
                "<Nothing to select>"
            };
            InitializeData();
            
            Title = "Human Agent Properties";
        }

        private void InitializeData()
        {
            string typeName = AgentElement.GetClassifierName(Repository);

            ActorSuggestionProvider typeSuggestionProvider = SuggestionProvider as ActorSuggestionProvider;

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
            if (!name.StartsWith("FMC4SE Human Agent"))
            {
                Name = name;
            }
            else
            {
                Name = "";
            }

            Kind = "<Nothing to select>";
            Notes = AgentElement.Notes;
        }

        #region CommandImplementations

        protected override void Ok()
        {
            OperationCanceled = false;
            CanClose = true;

            if (Type != null)
            {
                AgentElement.ClassifierID = Type.ElementID;
                AgentElement.Name = Name;
                AgentElement.Notes = Notes;
                AgentElement.Update();
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
                EAAPI.Element typeElement = UnsortedTypesPackage.Elements.AddNew(name, "Actor") as EAAPI.Element;
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
