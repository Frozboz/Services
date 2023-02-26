using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;

namespace FileReaderService.Base
{
    internal class GenericInstaller : Installer
    {
        private ServiceInstaller _serviceInstaller;
        private ServiceProcessInstaller _processInstaller;
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal GenericInstaller(CommandArguments arguments)
        {
            _processInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();

            _serviceInstaller.ServiceName = arguments.Servicename;

            if(arguments.ActionType == Enums.ActionType.Install)
            {
                _serviceInstaller.DisplayName = arguments.Displayname;
                _serviceInstaller.Description = $"{arguments.Description} ({arguments.Servicename})";
                _serviceInstaller.StartType = (ServiceStartMode)arguments.StartupType;
                if (!string.IsNullOrEmpty(arguments.Username))
                {
                    _processInstaller.Account = ServiceAccount.User;
                    _processInstaller.Username = arguments.Username;
                    _processInstaller.Password = arguments.Password;
                }
                else
                    _processInstaller.Account = ServiceAccount.LocalSystem;

                _log.Info($"Install successful.  Args:\r\n{arguments.ToString()}");
            }

            Installers.Add(_serviceInstaller);
            Installers.Add(_processInstaller);
        }
    }
}
