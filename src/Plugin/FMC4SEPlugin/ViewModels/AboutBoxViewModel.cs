using GalaSoft.MvvmLight;
using System.Reflection;

namespace MDD4All.FMC4SE.Plugin.ViewModels
{
    public class AboutBoxViewModel : ViewModelBase
    {
        public string Title
        {
            get
            {
                return "About FMC4SE";
            }
        }

        public string Description
        {
            get
            {
                return "Fundamental Modeling Concepts for Systems Engineering - Enterprise Architect Plugin";
            }
        }

        public string Copyright
        {
            get
            {
                return "(c) MDD4All.de, Dr. Oliver Alt";
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
    }
}
