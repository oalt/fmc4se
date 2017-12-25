using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Autocomplete.WPF.Editors;
using MDD4All.FMC4SE.Plugin.DataModels;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.SuggestionProviders
{
	public class TypeSuggestionProvider : ISuggestionProvider
    {
		private const string TYPE_QUERY =
@"select Name, Object_ID from t_object element where
element.Stereotype is null and
element.Object_Type = 'Class'";

		private ObservableCollection<TypeDataModel> _typesList;

		public ObservableCollection<TypeDataModel> AvailableTypes { get { return _typesList; } }

		private EAAPI.Repository _repository;

		public TypeSuggestionProvider(EAAPI.Repository repository)
		{
			_repository = repository;
			InitializeTypesListFromModel();
		}

		private void InitializeTypesListFromModel()
		{
			_typesList = new ObservableCollection<TypeDataModel>();
			try
			{
				string sqlResult = _repository.SQLQuery(TYPE_QUERY);

				XDocument doc = XDocument.Parse(sqlResult);

				XElement dataSetElement = doc.Element("EADATA").Element("Dataset_0");
				
				XElement dataElement = dataSetElement.Element("Data");

				foreach (XElement row in dataElement.Elements("Row"))
				{
					TypeDataModel dataModel = new TypeDataModel();

					XElement name = row.Element("Name");

					if (name != null)
					{
						dataModel.Name = name.Value;
					}

					XElement obejctID = row.Element("Object_ID");

					if (obejctID != null)
					{
						int id = 0;
						int.TryParse(obejctID.Value, out id );

						dataModel.ElementID = id;
					}
					_typesList.Add(dataModel);
				}
			}
			catch (Exception exception)
			{
				;
			}
			


		}

		public IEnumerable GetSuggestions(string filter)
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
