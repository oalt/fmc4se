using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MDD4All.FMC4SE.Plugin.Controllers;
using MDD4All.FMC4SE.Plugin.Views;
using System.Windows;
using System.Windows.Input;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
    /// <summary>
    /// Main view model for the FMC4SE EA plugin.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        private EAAPI.Repository _repository;

        public MainViewModel(EAAPI.Repository repository)
        {
            _repository = repository;

            ShowAboutWindowCommand = new RelayCommand(ExcuteShowAboutWindow);
            SynchronizeReferenceCommand = new RelayCommand(ExecuteSynchronizeReference);
        }

        public ICommand ShowAboutWindowCommand { get; private set; }
        public ICommand SynchronizeReferenceCommand { get; private set; }

        private void ExcuteShowAboutWindow()
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void ExecuteSynchronizeReference()
        {
            EAAPI.Diagram diagram = _repository.GetCurrentDiagram();

            if (diagram != null)
            {
                
                if(diagram.SelectedObjects.Count == 1)
                {
                    EAAPI.DiagramObject diagramObject = (EAAPI.DiagramObject)diagram.SelectedObjects.GetAt(0);

                    EAAPI.Element referenceElement = _repository.GetElementByID(diagramObject.ElementID);

                    ReferenceSynchronizer referenceSynchronizer = new ReferenceSynchronizer(_repository, referenceElement);

                    referenceSynchronizer.SynchronizeReferenceWithOriginal();

                    MessageBox.Show("Synchronization finished.", "Reference Synchronization");
                }
               
            }
        }
    }
}
