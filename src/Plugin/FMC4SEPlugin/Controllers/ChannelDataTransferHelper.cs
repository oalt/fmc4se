using MDD4All.EnterpriseArchitect.Manipulations;
using EAShared = EA;

namespace MDD4All.FMC4SE.Plugin.Controllers
{
    public class ChannelDataTransferHelper
    {
        private EAShared.Repository Rep;


        public ChannelDataTransferHelper(EAShared.Repository rep)
        {
            this.Rep = rep;
        }

        /// <summary>
        /// Set attributes and tags of supplier port when port is created or connected.
        /// </summary>
        /// <param name="connectorId">The created item flow connector.</param>
        /// <param name="portStereotype">The stereotype of the port.</param>
        /// <param name="connectorStereotype">The stereotype of the port connectiong flow connector.</param>
        /// <param name="supplierName">The EA default name for the new created supplier port.</param>
        public void SetPropertyTypeAndDirectionOnFlowPortCreation(int connectorId,
                                                                                       string portStereotype,
                                                                                      string connectorStereotype,
                                                                                       string supplierName)
        {
            EAShared.Connector connector = Rep.GetConnectorByID(connectorId);

            EAShared.Element client = Rep.GetElementByID(connector.ClientID);
            EAShared.Element supplier = Rep.GetElementByID(connector.SupplierID);

            if ((supplier.Stereotype == portStereotype) &&
                 (connector.Stereotype == connectorStereotype))
            {
                CopyTaggedValuesFromClientToSupplierPort(client, supplier);

                if (supplier.PropertyType == 0)
                {
                    supplier.PropertyType = client.PropertyType;
                    if (supplier.Alias == "")
                    {
                        supplier.Alias = client.Alias;
                    }

                    if (supplier.Multiplicity == "")
                    {
                        supplier.Multiplicity = client.Multiplicity;

                    }

                    // Copy notes text from client to supplier
                    if (supplier.Notes == "")
                    {
                        supplier.Notes = client.Notes;

                    }
                }
                else if (supplier.PropertyType == client.PropertyType)
                {
                    if (supplier.Alias == "")
                    {
                        supplier.Alias = client.Alias;
                    }

                    if (supplier.Multiplicity == "")
                    {
                        supplier.Multiplicity = client.Multiplicity;
                    }

                    // Copy notes text from client to supplier
                    if (supplier.Notes == "")
                    {
                        supplier.Notes = client.Notes;
                    }
                }

                if (supplier.Name.StartsWith(supplierName) || supplier.Name == "")
                {
                    supplier.Name = client.Name;

                }


                supplier.Update();
                supplier.Refresh();

            } // if
        }

        /// <summary>
        /// Enumeration required to determine compartement direction.
        /// </summary>
        private enum CompartmentDirection
        {
            In, Out, Unknown
        }

        /// <summary>
        /// Determine the component location in relation to the other component.  
        /// </summary>
        /// <param name="srcComponentElementId">The source component.</param>
        /// <param name="destComponentElementId">The destination component.</param>
        /// <returns>The comoartment location. in = internal component.</returns>
        private CompartmentDirection DetermineDestinationDecomposition(
             int srcComponentElementId,
             int destComponentElementId)
        {
            EAShared.Element srcEl = Rep.GetElementByID(srcComponentElementId);
            for (short i = 0; i < srcEl.Connectors.Count; i++)
            {
                EAShared.Connector con = (EAShared.Connector)srcEl.Connectors.GetAt(i);
                if (con.Type == "Aggregation")
                {
                    if (con.ClientID == srcComponentElementId && con.SupplierID == destComponentElementId)
                    {
                        return CompartmentDirection.Out;
                    }

                    if (con.ClientID == destComponentElementId && con.SupplierID == srcComponentElementId)
                    {
                        return CompartmentDirection.In;
                    }
                }
            }
            return CompartmentDirection.Unknown;
        }

        /// <summary>
        /// Copy tagged values - except direction and ASIL Level - from client port to supplier port.
        /// </summary>
        /// <param name="client">The client port.</param>
        /// <param name="supplier">The supplier port.</param>
        private void CopyTaggedValuesFromClientToSupplierPort(EAShared.Element client, EAShared.Element supplier)
        {
            for (short i = 0; i < client.TaggedValues.Count; i++)
            {
                EAShared.TaggedValue clientTag = client.TaggedValues.GetAt(i) as EAShared.TaggedValue;
                if (clientTag != null && (clientTag.Name != "direction" && clientTag.Name != "ASIL Level"))
                {
                    supplier.SetTaggedValueString(clientTag.Name, clientTag.Value, false);
                }
            }
        }
    }
}
