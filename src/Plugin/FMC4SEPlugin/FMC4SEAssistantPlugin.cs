using System;
using System.Collections.Generic;
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

        private MainViewModel _mainViewModel;

        private const string MAIN_MENU = "-&FMC4SE";

        private const string SYNCHRONIZE_REFERENCE_MENU = "&Synchronize reference element";

        private const string ABOUT_MENU = "&About FMC4SE...";

		public string EA_Connect(EAAPI.Repository repository)
		{
            _mainViewModel = new MainViewModel(repository);

			AutoCompleteTextBox textbox = new AutoCompleteTextBox();

			_channelDataTransferHelper = new ChannelDataTransferHelper(repository);

			return "";
		}

        ///
        /// Called when user Clicks Add-Ins Menu item from within EA.
        /// Populates the Menu with our desired selections.
        /// Location can be "TreeView" "MainMenu" or "Diagram".
        ///
        /// <param name="repository" />the repository
        /// <param name="location" />the location of the menu
        /// <param name="menuName" />the name of the menu
        ///
        public object EA_GetMenuItems(EAAPI.Repository repository, string location, string menuName)
        {

            switch (menuName)
            {
                // defines the top level menu option
                case "":
                    return MAIN_MENU;

                // defines the submenu options
                case MAIN_MENU:

                    List<string> subMenuList = new List<string>();

                    if(location == "Diagram")
                    {
                        subMenuList.Add(SYNCHRONIZE_REFERENCE_MENU);
                    }

                    subMenuList.Add(ABOUT_MENU);

                    return subMenuList.ToArray();
            }

            return "";
        }

        

        ///
        /// Called once Menu has been opened to see what menu items should active.
        ///
        /// <param name="repository" />the repository
        /// <param name="location" />the location of the menu
        /// <param name="menuName" />the name of the menu
        /// <param name="itemName" />the name of the menu item
        /// <param name="isEnabled" />boolean indicating whethe the menu item is enabled
        /// <param name="isChecked" />boolean indicating whether the menu is checked
        public void EA_GetMenuState(EAAPI.Repository repository, string location, string menuName, string itemName, ref bool isEnabled, ref bool isChecked)
        {
            switch (itemName)
            {
                
                
               
                default:
                    isEnabled = true;
                    break;
            }


        }

        

        ///
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        ///
        /// <param name="repository" />the repository
        /// <param name="location" />the location of the menu
        /// <param name="menuName" />the name of the menu
        /// <param name="itemName" />the name of the selected menu item
        public void EA_MenuClick(EAAPI.Repository repository, string location, string menuName, string itemName)
        {
            switch (itemName)
            {

                case ABOUT_MENU:
                    _mainViewModel.ShowAboutWindowCommand.Execute(null);
                    break;

                case SYNCHRONIZE_REFERENCE_MENU:
                    _mainViewModel.SynchronizeReferenceCommand.Execute(null);
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

			_channelDataTransferHelper.SetPropertyTypeAndDirectionOnFlowPortCreation(connectorId, "channel", "access type", "FMC4SE Channel");
			
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
