using System;
using Autocomplete.WPF.Editors;
using MDD4All.FMC4SE.Plugin.Controllers;
using MDD4All.FMC4SE.Plugin.ViewModels;
using MDD4All.FMC4SE.Plugin.Views;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin
{
	public class FMC4SEAssistantPlugin
	{
		private ChannelDataTransferHelper _channelDataTransferHelper;

		public string EA_Connect(EAAPI.Repository repository)
		{
			AutoCompleteTextBox textbox = new AutoCompleteTextBox();


			_channelDataTransferHelper = new ChannelDataTransferHelper(repository);
			return "";
		}

		public bool EA_OnPostNewElement(EAAPI.Repository repository, EAAPI.EventProperties info)
		{
			bool result = true;

			int elementId = Convert.ToInt32(info.Get(0).Value.ToString());

			EAAPI.Element newElement = repository.GetElementByID(elementId);

			if (newElement.Stereotype == "agent")
			{
				repository.SuppressEADialogs = true;
				FMCElementPropertyWindow newAgentWindow = new FMCElementPropertyWindow();
				newAgentWindow.DataContext = new AgentPropertyViewModel(repository, newElement);
				newAgentWindow.ShowDialog();
				
			}
			else if (newElement.Stereotype == "channel")
			{
				repository.SuppressEADialogs = true;
				FMCElementPropertyWindow newAgentWindow = new FMCElementPropertyWindow();
				newAgentWindow.DataContext = new ChannelPropertyViewModel(repository, newElement);
				newAgentWindow.ShowDialog();
			}

			return result;
		}

		public bool EA_OnPostNewConnector(EAAPI.Repository repository, EAAPI.EventProperties info)
		{
			bool result = true;
			int connectorId = Convert.ToInt32(info.Get(0).Value.ToString());

			_channelDataTransferHelper.SetPropertyTypeAndDirectionOnFlowPortCreation(connectorId, "channel", "access type", "Port");
			
			return result;
		}

		public bool EA_OnContextItemDoubleClicked(EAAPI.Repository repository, string guid, EAAPI.ObjectType objectType)
		{
			bool result = false;
			if (objectType == EAAPI.ObjectType.otElement)
			{
				EAAPI.Element element = repository.GetElementByGuid(guid);
				if (element.Stereotype == "agent")
				{
					FMCElementPropertyWindow newAgentWindow = new FMCElementPropertyWindow();
					newAgentWindow.DataContext = new AgentPropertyViewModel(repository, element);
					newAgentWindow.ShowDialog();
					result = true;
				}
				else if (element.Stereotype == "channel")
				{
					FMCElementPropertyWindow newAgentWindow = new FMCElementPropertyWindow();
					newAgentWindow.DataContext = new ChannelPropertyViewModel(repository, element);
					newAgentWindow.ShowDialog();
					result = true;
				}
			}
			return result;
		}
	}
}
