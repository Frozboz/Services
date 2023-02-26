using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderService.Base
{
    public class ServiceBase : System.ServiceProcess.ServiceBase
    {
        public event EventHandler<InstallEventArgs> ServiceInstalled;
        public event EventHandler<InstallEventArgs> ServiceUninstalled;

        protected override void OnStart(string[] args)
        {
            throw new NotImplementedException();
        }

        protected override void OnStop()
        {
            throw new NotImplementedException();
        }

        protected void Execute(string[] args, StartLocal startDebug, StopLocal stopDebug)
        {
            Execute(args, startDebug, stopDebug, Assembly.GetEntryAssembly());
        }

        protected void Execute(string[] args, StartLocal startDebug, StopLocal stopDebug, Assembly executingAssembly)
        {
            if (args.Length > 0)
            {
                CommandArguments cmdArgs = new CommandArguments();
                Parser parser = new Parser();

                cmdArgs = parser.Parse(args);

                if (cmdArgs.Validate())
                {
                    if (cmdArgs.ActionType == Enums.ActionType.Debug)
                        RunDebug(startDebug, stopDebug);
                    else
                        RunInstallTask(cmdArgs, executingAssembly);
                }
            }
            else
            {
                System.ServiceProcess.ServiceBase[] services;
                services = new ServiceBase[] { this };
                Run(services);
            }
        }

        private static void RunDebug(StartLocal startDebug, StopLocal stopDebug)
        {
            try
            {
                if(startDebug == null)
                {
                    throw new ArgumentNullException("startDebug");
                }

                if (stopDebug == null)
                {
                    throw new ArgumentNullException("stopDebug");
                }

                TextWriterTraceListener listener = new TextWriterTraceListener(Console.Out);
                Trace.Listeners.Add(listener);
                Console.WriteLine("Attempting to start the service. Hit enter to stop the service");
                startDebug.Invoke(new string[] { });
                Console.ReadKey();
                stopDebug.Invoke();
                Trace.WriteLine("Debug finishing");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void RunInstallTask(CommandArguments args, Assembly exe)
        {
            try
            {
                TransactedInstaller inst = new TransactedInstaller();
                GenericInstaller gen = new GenericInstaller(args);
                gen.AfterInstall += new InstallEventHandler(OnAfterInstallBase);
                gen.AfterUninstall += new InstallEventHandler(OnAfterUninstallBase);
                inst.Installers.Add(gen);
                string[] commandline = new string[1];
                commandline[0] = $"/assemblypath={exe.Location}";
                InstallContext context = new InstallContext("", commandline);
                inst.Context = context;
                if (args.ActionType == Enums.ActionType.Install)
                {
                    inst.Install(new Hashtable());
                }
                else
                    inst.Uninstall(null);

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {args.ActionType.ToString()}\n{ex.ToString()}");
            }
        }

        private void OnAfterInstallBase(object sender, InstallEventArgs e)
        {
            ServiceInstalled?.Invoke(sender, e);
        }

        private void OnAfterUninstallBase(object sender, InstallEventArgs e)
        {
            ServiceUninstalled?.Invoke(sender, e);
        }
    }
}
