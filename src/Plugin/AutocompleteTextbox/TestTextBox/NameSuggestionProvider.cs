using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autocomplete.WPF.Editors;

namespace TestTextBox
{
	public class NameSuggestionProvider : ISuggestionProvider
	{
		public IEnumerable GetSuggestions(string filter)
		{
			IEnumerable result = null;

			if (string.IsNullOrEmpty(filter))
			{
				result = null;
			}
			else
			{
				result = new List<string>
				{
					"USB",
					"USB1",
					"User Name",
					"Upload Directory",
					"VGA"
				};
			}


			return result;
		}
	}
}
