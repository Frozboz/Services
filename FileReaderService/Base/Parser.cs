using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderService.Base
{
    public class Parser
    {
        public Parser() { }
        public CommandArguments Parse(string[] args)
        {
            CommandArguments commandArgs = new CommandArguments();
            if (args.Length == 0)
            {
                return null;
            }

            foreach(string arg in args)
            {
                //args = new [] { "/a:install", "/dn:Test", "/desc:Test harness", "/sn:Tester", "/starttype:Manual" };
                if (arg.Contains("/a:"))
                {
                    commandArgs.Action = arg.Substring(3);

                    switch (commandArgs.Action)
                    {
                        case "install":
                            commandArgs.ActionType = Enums.ActionType.Install;
                            break;
                        case "uninstall":
                            commandArgs.ActionType = Enums.ActionType.Uninstall;
                            break;
                        case "debug":
                            commandArgs.ActionType = Enums.ActionType.Debug;
                            break;

                        default:
                            break;
                    }
                }

                if (arg.Contains("/sn:"))
                    commandArgs.Servicename = arg.Substring(4);

                if (arg.Contains("/dn:"))
                    commandArgs.Displayname = arg.Substring(4);

                if (arg.Contains("/desc:"))
                    commandArgs.Description = arg.Substring(6);

                if (arg.Contains("/starttype:"))
                {
                    commandArgs.Starttype = arg.Substring(11);

                    switch (commandArgs.Starttype.ToLower())
                    {
                        case "manual":
                            commandArgs.StartupType = Enums.StartupType.Manual;
                            break;
                        case "automatic":
                            commandArgs.StartupType = Enums.StartupType.Automatic;
                            break;
                        case "disabled":
                            commandArgs.StartupType = Enums.StartupType.Disabled;
                            break;
                        case "none":
                        default:
                            commandArgs.StartupType = Enums.StartupType.None;
                            break;
                    }
                }

            }

            return commandArgs;
        }
    }
}
