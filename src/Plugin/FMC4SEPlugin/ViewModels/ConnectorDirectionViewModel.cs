using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using EAAPI = EA;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
    public class ConnectorDirectionViewModel : ViewModelBase
    {

        public ConnectorDirectionViewModel()
        {
            OkCommand = new RelayCommand(ExecuteOkcommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
        }

        public ConnectorDirectionViewModel(EAAPI.Connector connector) : this()
        {
            _connectorUnderEdit = connector;
            ConnectorDirection = connector.Direction;
        }

        private EAAPI.Connector _connectorUnderEdit;

        public string Title
        {
            get
            {
                return "Set connector direction";
            }
        }

        private string _connectorDirection = "Unspecified";

        public string ConnectorDirection
        {
            get { return _connectorDirection; }
            set
            {
                _connectorDirection = value;
                RaisePropertyChanged("ConnectorDirection");
            }
        }



        public ICommand OkCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        private void ExecuteOkcommand()
        {
            if(_connectorUnderEdit != null)
            {
                string actualDirection = _connectorUnderEdit.Direction;

                if(actualDirection != ConnectorDirection)
                {
                    _connectorUnderEdit.Direction = ConnectorDirection;
                    _connectorUnderEdit.Update();
                }
            }
        }

        private void ExecuteCancelCommand()
        {
            ;
        }

        
    }
}
