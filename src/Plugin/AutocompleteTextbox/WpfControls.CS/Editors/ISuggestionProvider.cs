

using System.Collections;

namespace Autocomplete.WPF.Editors
{
	public interface ISuggestionProvider
    {

        #region Public Methods

        IEnumerable GetSuggestions(string filter);

        #endregion Public Methods

    }
}
