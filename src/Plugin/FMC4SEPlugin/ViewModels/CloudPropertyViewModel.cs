using MDD4All.FMC4SE.Plugin.DataModels;
using MDD4All.FMC4SE.Plugin.SuggestionProviders;
using System.Collections.ObjectModel;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
    public class CloudPropertyViewModel : AgentPropertyViewModel
    {
        public CloudPropertyViewModel(EAAPI.Repository repository, EAAPI.Element agentElement) : base(repository, agentElement)
        {
            SuggestionProvider = new TypeSuggestionProvider(repository);

            Kinds = new ObservableCollection<string>
            {
                "<Nothing to select>"
            };

            string name = AgentElement.Name;
            if (!name.StartsWith("FMC4SE Cloud"))
            {
                Name = name;
            }
            else
            {
                Name = "";
            }

            Title = "Cloud Properties";
        }

        
    }
}
