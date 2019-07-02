using System.Collections.Generic;
using EAAPI = EA;
using MDD4All.EnterpriseArchitect.Manipulations;
using NLog;

namespace MDD4All.FMC4SE.Plugin.Controllers
{
    public class ReferenceSynchronizer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private EAAPI.Repository _repository;
        private EAAPI.Element _referenceElement;

        public ReferenceSynchronizer(EAAPI.Repository repository, EAAPI.Element referenceElement)
        {
            _repository = repository;
            _referenceElement = referenceElement;
        }

        public void SynchronizeReferenceWithOriginal()
        {
            List<EAAPI.Element> originals = new List<EAAPI.Element>();

            EAAPI.Element original = null;

            // search for originals
            for (short connectorCount = 0; connectorCount < _referenceElement.Connectors.Count; connectorCount++)
            {
                EAAPI.Connector connector = (EAAPI.Connector)_referenceElement.Connectors.GetAt(connectorCount);

                if (connector.ClientID == _referenceElement.ElementID && connector.SupplierID != _referenceElement.ElementID)
                {
                    if (connector.Type == "Dependency" && connector.Stereotype == "referenceOf")
                    {
                        original = _repository.GetElementByID(connector.SupplierID);

                        if (original.Type == "Object" && original.ClassifierID == _referenceElement.ClassifierID)
                        {
                            originals.Add(original);
                        }
                    }
                }
            }

            if (originals.Count == 1)
            {
                original = originals[0];

                if (original.ClassifierID != _referenceElement.ClassfierID)
                {
                    logger.Info("Synchronizing element type.");
                    _referenceElement.ClassifierID = _referenceElement.ClassifierID;
                    _referenceElement.Update();
                }

                DeleteWrongPortReferenceOfConnections(original.ElementID);

                SynchronizePortsOfReferenceElement(original);

                // synchronize descriptional notes
                logger.Info("Synchronizing descriptional notes.");
                _referenceElement.Notes = original.Notes;
                _referenceElement.Update();

                _referenceElement.Stereotype = original.Stereotype;
                _referenceElement.Update();

                SynchronizeTaggedValues(original, _referenceElement);
                
                logger.Info("Port synchoronization finished.");
            }
            else if (originals.Count == 0)
            {
                // return if no original was found
                logger.Info("No original element found.");
                return;
            }
            else
            {
                logger.Error("Error: Reference is not unique.");
            }
        }
        
        private void SynchronizePortsOfReferenceElement(EAAPI.Element originalElement)
        {
            List<int> orphanPorts = new List<int>();

            // search for orphan ports

            for (short count = 0; count < originalElement.EmbeddedElements.Count; count++)
            {
                EAAPI.Element embeddedElement = (EA.Element)originalElement.EmbeddedElements.GetAt(count);

                if (embeddedElement.Type == "Port")
                {
                    bool found = false;
                    for (short connectorCount = 0; connectorCount < embeddedElement.Connectors.Count; connectorCount++)
                    {
                        EAAPI.Connector originalPortConnector = (EAAPI.Connector)embeddedElement.Connectors.GetAt(connectorCount);

                        if (originalPortConnector.SupplierID == embeddedElement.ElementID &&
                            originalPortConnector.ClientID != embeddedElement.ElementID &&
                            originalPortConnector.Type == "Dependency" &&
                            originalPortConnector.Stereotype == "referenceOf")
                        {
                            EAAPI.Element referencePort = _repository.GetElementByID(originalPortConnector.ClientID);
                            if (referencePort.Type == "Port" && referencePort.ParentID == _referenceElement.ElementID)
                            {
                                // reference port found - synch data by connector
                                logger.Info("ReferenceOf connection found for port, synchronizing data for element " + embeddedElement.Name);

                                SynchronizePortData(embeddedElement, referencePort);

                                found = true;
                                break;
                            }

                        }

                    } // for
                    if (!found)
                    {
                        orphanPorts.Add(embeddedElement.ElementID);
                    }
                }

            }

            // synchronize the orphan ports

            foreach (int orphanPortID in orphanPorts)
            {
                bool match = false;

                for (short referencePortCounter = 0; referencePortCounter < _referenceElement.EmbeddedElements.Count; referencePortCounter++)
                {
                    EAAPI.Element embeddedElement = (EAAPI.Element)_referenceElement.EmbeddedElements.GetAt(referencePortCounter);
                    if (embeddedElement.Type == "Port")
                    {
                        bool synchByConnector = false;
                        for (short referenceConnectorCount = 0; referenceConnectorCount < embeddedElement.Connectors.Count; referenceConnectorCount++)
                        {
                            EAAPI.Connector portConnector = (EAAPI.Connector)embeddedElement.Connectors.GetAt(referenceConnectorCount);

                            if (portConnector.ClientID == embeddedElement.ElementID && 
                                portConnector.SupplierID != embeddedElement.ElementID && 
                                portConnector.Type == "Dependency" && 
                                portConnector.Stereotype == "referenceOf")
                            {
                                EAAPI.Element originalPort = _repository.GetElementByID(portConnector.SupplierID);
                                if (originalPort.ParentID == originalElement.ElementID)
                                {
                                    synchByConnector = true;
                                }
                            }
                        }

                        if (!synchByConnector)
                        {
                            if (embeddedElement.ElementID == orphanPortID)
                            {
                                match = true;
                                //SyncReferenceData(opd, embeddedElement);

                                EAAPI.Element originalPort = _repository.GetElementByID(orphanPortID);
                                SynchronizePortReferenceOfConnector(originalPort, embeddedElement);

                                break;
                            }
                        }
                    }
                }

                if (!match) // port is in original but not in reference
                {
                    logger.Info("Adding new Port to reference. ID=" + orphanPortID);

                    EAAPI.Element originalPort = _repository.GetElementByID(orphanPortID);
                    EAAPI.Element newReferencePort = _referenceElement.EmbeddedElements.AddNew("unnamed", "Port") as EAAPI.Element;
                    _referenceElement.EmbeddedElements.Refresh();

                    SynchronizePortData(originalPort, newReferencePort);

                    SynchronizePortReferenceOfConnector(originalPort, newReferencePort);
                }
            }

        }

        private void SynchronizePortData(EAAPI.Element originalPort, EAAPI.Element referencePort)
        {
            referencePort.Name = originalPort.Name;
            referencePort.PropertyType = originalPort.PropertyType;
            referencePort.Notes = originalPort.Notes;
            referencePort.Stereotype = originalPort.Stereotype;
            referencePort.Update();

            SynchronizeTaggedValues(originalPort, referencePort);
        }

        private void SynchronizeTaggedValues(EAAPI.Element originalElement, EAAPI.Element referenceElement)
        {
            for (short tagCount = 0; tagCount < originalElement.TaggedValues.Count; tagCount++)
            {
                EAAPI.TaggedValue taggedValue = originalElement.TaggedValues.GetAt(tagCount) as EAAPI.TaggedValue;

                referenceElement.SetTaggedValueString(taggedValue.Name, taggedValue.Value, false);
            }
        }

        private void SynchronizePortReferenceOfConnector(EAAPI.Element originalPort, EAAPI.Element referencePort)
        {
            for (short i = 0; i < referencePort.Connectors.Count; i++)
            {
                EAAPI.Connector con = (EA.Connector)referencePort.Connectors.GetAt(i);

                if (con.Type == "Dependency" && 
                    con.Stereotype == "referenceOf" && 
                    con.ClientID == referencePort.ElementID && 
                    con.SupplierID == originalPort.ElementID)
                {
                    return;
                }
            }

            // no match
            logger.Info("Adding referenceOf connector to port.");

            EAAPI.Connector newConnector = referencePort.AddConnector(originalPort, "Dependency");

            newConnector.Stereotype = "referenceOf";
            newConnector.Update();
            originalPort.Connectors.Refresh();
            referencePort.Connectors.Refresh();
        }

        private void DeleteWrongPortReferenceOfConnections(int originalElementID)
        {
            for (short connectorCount = 0; connectorCount < _referenceElement.EmbeddedElements.Count; connectorCount++)
            {
                EAAPI.Element embeddedElement = _referenceElement.EmbeddedElements.GetAt(connectorCount) as EAAPI.Element;
                if (embeddedElement.Type == "Port")
                {
                    for (short portCounter = 0; portCounter < embeddedElement.Connectors.Count; portCounter++)
                    {
                        EAAPI.Connector referencePortConnector = (EAAPI.Connector)embeddedElement.Connectors.GetAt(portCounter);

                        if (referencePortConnector.ClientID == embeddedElement.ElementID && 
                            referencePortConnector.SupplierID != embeddedElement.ElementID && 
                            referencePortConnector.Type == "Dependency" && 
                            referencePortConnector.Stereotype == "referenceOf")
                        {
                            EAAPI.Element referencedElement = _repository.GetElementByID(referencePortConnector.SupplierID);

                            if (referencedElement.Type == "Port" && 
                                referencedElement.ParentID == originalElementID)
                            {
                                // correct reference
                            }
                            else
                            {
                                logger.Info("Deleting wrong referenceOf connector.");
                                _repository.DeleteConnector(referencePortConnector);
                                
                            }
                        }
                    }
                }
            }
        }
    }
}
