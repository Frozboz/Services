using log4net.Config;
using log4net;
using System;
using System.Configuration;
using FileReaderService.Base;
using FileReaderService.Model;
using FileReaderService.Logic;

namespace FileReaderService
{
    public class Program : ServiceBase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            /*
             To Install: uncomment the first line below (the install step) and run the service in Release mode.  
                         then comment it out again and run the service again.  Check windows services, it should
                         be there.  
             To Uninstall: uncomment the second line below and run the service in Release mode.
             To test, and in development (majority of the time), uncomment the third line below and switch to Debug mode.
             */

            //args = new [] { "/a:install", "/dn:Test", "/desc:Test harness", "/sn:Tester", "/starttype:Manual" };
            //args = new[] { "/a:uninstall", "/sn:Tester" };
            args = new [] { "/a:debug" };

            foreach(string arg in args)
            {
                Console.WriteLine(arg);
            }

            var startup = new Program();
            startup.Execute(args, startup.OnStart, startup.OnStop);
        }

        public Program()
        {
            XmlConfigurator.Configure();
        }

        protected override void OnStart(string[] args)
        {
            //call logic here
            int secondsBetweenPolls, pulseTimeSeconds;
            int.TryParse(ConfigurationManager.AppSettings.Get("SecondsBetweenPolls"), out secondsBetweenPolls);
            int.TryParse(ConfigurationManager.AppSettings.Get("PulseIntervalSeconds"), out pulseTimeSeconds);

            Settings settings = new Settings
            {
                PolledDirectory = ConfigurationManager.AppSettings.Get("PolledDirectory"),
                PulseIntervalSeconds = pulseTimeSeconds,
                SecondsBetweenPolls = secondsBetweenPolls
            };

            FileReaderLogic frl = new FileReaderLogic(settings, _log);
            _log.Info($"Started");
        }

        protected override void OnStop()
        {
            _log.Info($"Stopped");
        }

    }
}
