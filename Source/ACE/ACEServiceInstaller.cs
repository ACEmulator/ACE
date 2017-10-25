using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;

namespace ACE
{
    [DesignerCategory("")]
    [RunInstaller(true)]
    public class ACEServiceInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public ACEServiceInstaller()
        {
            // Instantiate installer for process and service
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            // Set the service account used
            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.DisplayName = "ACEmulator";

            // Service startup type
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            // must be the same as what was set in Program's constructor
            serviceInstaller.ServiceName = "ACEmulator Service";
            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
