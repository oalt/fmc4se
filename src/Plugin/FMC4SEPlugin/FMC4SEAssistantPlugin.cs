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

        private const string MAIN_MENU = "-&FMC4SE";

        private const string ABOUT_MENU = "&About FMC4SE...";

		public string EA_Connect(EAAPI.Repository repository)
		{
			AutoCompleteTextBox textbox = new AutoCompleteTextBox();

			_channelDataTransferHelper = new ChannelDataTransferHelper(repository);
			return "";
		}

        ///
        /// Called when user Clicks Add-Ins Menu item from within EA.
        /// Populates the Menu with our desired selections.
        /// Location can be "TreeView" "MainMenu" or "Diagram".
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        ///
        public object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {

            switch (MenuName)
            {
                // defines the top level menu option
                case "":
                    return MAIN_MENU;
                // defines the submenu options
                case MAIN_MENU:
                    string[] subMenus = { ABOUT_MENU };
                    return subMenus;
            }

            return "";
        }

        

        ///
        /// Called once Menu has been opened to see what menu items should active.
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the menu item
        /// <param name="IsEnabled" />boolean indicating whethe the menu item is enabled
        /// <param name="IsChecked" />boolean indicating whether the menu is checked
        public void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {
            switch (ItemName)
            {
                
                case ABOUT_MENU:
                    IsEnabled = true;
                    break;
               
                default:
                    IsEnabled = false;
                    break;
            }


        }

        private void ShowAboutWindow()
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        ///
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the selected menu item
        public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName)
        {
            switch (ItemName)
            {

                case ABOUT_MENU:
                    ShowAboutWindow();
                    break;

                default:

                    break;
            }
            
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
            else if(newElement.Stereotype == "storage")
            {
                repository.SuppressEADialogs = true;
                FMCElementPropertyWindow newAgentWindow = new FMCElementPropertyWindow();
                newAgentWindow.DataContext = new StoragePropertyViewModel(repository, newElement);
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
                else if (element.Stereotype == "storage")
                {
                    FMCElementPropertyWindow newAgentWindow = new FMCElementPropertyWindow();
                    newAgentWindow.DataContext = new StoragePropertyViewModel(repository, element);
                    newAgentWindow.ShowDialog();
                    result = true;
                }
            }
			return result;
		}
	}
}
