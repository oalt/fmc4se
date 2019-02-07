using System;
using System.Collections.ObjectModel;
using System.Linq;
using MDD4All.EnterpriseArchitect.Manipulations;
using MDD4All.FMC4SE.Plugin.DataModels;
using MDD4All.FMC4SE.Plugin.SuggestionProviders;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
    public class StoragePropertyViewModel : ChannelPropertyViewModel
    {
        public StoragePropertyViewModel(EAAPI.Repository repository, EAAPI.Element agentElement) : base(repository, agentElement)
        {
            Kinds = new ObservableCollection<string>
            {
                "<Nothing to select>"
            };
            InitializeData();
            
            Title = "Storage Properties";
        }

        private void InitializeData()
        {
            string typeName = AgentElement.GetClassifierName(Repository);

            ChannelTypeSuggestionProvider typeSuggestionProvider = SuggestionProvider as ChannelTypeSuggestionProvider;

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
            if (!name.StartsWith("FMC4SE Storage"))
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


        #endregion
    }
}
