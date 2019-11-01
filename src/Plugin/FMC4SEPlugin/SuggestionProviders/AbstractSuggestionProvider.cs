using Autocomplete.WPF.Editors;
using MDD4All.FMC4SE.Plugin.DataModels;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.SuggestionProviders
{
    public abstract class AbstractSuggestionProvider : ISuggestionProvider
    {
        public ObservableCollection<TypeDataModel> AvailableTypes { get { return _typesList; } }

        protected string _queryString = "";

        protected ObservableCollection<TypeDataModel> _typesList;

        protected EAAPI.Repository _repository;

        protected void InitializeTypesListFromModel()
        {
            _typesList = new ObservableCollection<TypeDataModel>();
            try
            {
                string sqlResult = _repository.SQLQuery(_queryString);

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
                        int.TryParse(obejctID.Value, out id);

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

        public abstract IEnumerable GetSuggestions(string filter);
    }
}
