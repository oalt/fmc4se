using MDD4All.FMC4SE.Plugin.SuggestionProviders;
using System.Collections.ObjectModel;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
    public class ToolPropertyViewModel : AgentPropertyViewModel
    {
        public ToolPropertyViewModel(EAAPI.Repository repository, EAAPI.Element agentElement) : base(repository, agentElement)
        {
            SuggestionProvider = new TypeSuggestionProvider(repository);

            Kinds = new ObservableCollection<string>
            {
                "<Nothing to select>"
            };

            string name = AgentElement.Name;
            if (!name.StartsWith("Tool"))
            {
                Name = name;
            }
            else
            {
                Name = "";
            }

            Title = "Tool Properties";
        }
    }
}
