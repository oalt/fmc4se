using System.Collections;
using System.Linq;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.SuggestionProviders
{
    public class TypeSuggestionProvider : AbstractSuggestionProvider
    {

        public TypeSuggestionProvider(EAAPI.Repository repository)
        {
            _repository = repository;

            _queryString = @"select Name, Object_ID from t_object element where
element.Stereotype is null and
element.Object_Type = 'Class'";

            InitializeTypesListFromModel();
        }


        public override IEnumerable GetSuggestions(string filter)
        {
            IEnumerable result = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = _typesList.ToList().FindAll(type => type.Name.ToLower().Contains(filter.ToLower()));
            }
            else if (filter == " ")
            {
                result = _typesList;
            }

            return result;
        }
    }
}
