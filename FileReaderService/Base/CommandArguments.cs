using FileReaderService.Base.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderService.Base
{
    public class CommandArguments
    {
        public string Action { get; set; }
        public string Displayname { get; set; }
        public string Description { get; set; }
        public string Servicename { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Starttype { get; set; }

        internal ActionType ActionType { get; set; }
        public StartupType StartupType { get; set; }

        public CommandArguments()
        {
            StartupType = StartupType.Automatic;
        }

        public bool Validate()
        {
            if (!ValidateAction())
            {
                return false;
            }

            if (ActionType == ActionType.Debug)
                return true;

            if (ActionType == ActionType.Uninstall)
                if (!CheckRequiredInstallUninstallValues())
                    return false;
                else
                    return true;

            if (!CheckRequiredInstallValues())
                return false;

            if (!ValidatateUser())
                return false;

            if (!ValidateStartType())
                return false;

            return true;
        }

        #region validation 

        private bool ValidateAction()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(Action))
            {
                Console.WriteLine("Action is a required parameter");
                valid = false;
            }
            else
            {
                if (string.Compare(Action, "install", true) == 0)
                    ActionType = ActionType.Install;
                else if (string.Compare(Action, "uninstall", true) == 0)
                    ActionType = ActionType.Uninstall;
                else if (string.Compare(Action, "debug", true) == 0)
                    ActionType = ActionType.Debug;
                else
                {
                    Console.WriteLine("Unrecognized value specified for Action: {0}", Action);
                    valid = false;
                }
            }
            return valid;
        }

        private bool CheckRequiredInstallUninstallValues()
        {
            if (string.IsNullOrEmpty(Servicename))
            {
                Console.WriteLine("Missing required parameter when action is install or uninstall: servicename");
                return false;
            }
            return true;
        }

        private bool CheckRequiredInstallValues()
        {
            if (string.IsNullOrEmpty(Displayname))
            {
                Console.WriteLine("Missing required parameter when action is install: displayname");
                return false;
            }

            if (string.IsNullOrEmpty(Description))
            {
                Console.WriteLine("Missing required parameter when action is install: description");
                return false;
            }

            return true;
        }

        private bool ValidatateUser()
        {
            if (!string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password))
            {
                Console.WriteLine("Missing required parameter when username specified: password");
                return false;
            }
            return true;
        }

        private bool ValidateStartType()
        {
            bool validStartType = true;
            if (string.Compare(Starttype, "automatic", true) == 0)
                StartupType = StartupType.Automatic;
            else if (string.Compare(Starttype, "disabled", true) == 0)
                StartupType = StartupType.Disabled;
            else if (string.Compare(Starttype, "manual", true) == 0)
                StartupType = StartupType.Manual;
            else
            {
                Console.WriteLine("Unrecognized value specified for Action: {0}", Action);
                validStartType = false;
            }
            return validStartType;
        }

        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PropertyDescriptor d in TypeDescriptor.GetProperties(this))
            {
                var name = d.Name;
                object value = d.GetValue(this);
                if (value != null && (value.GetType() != typeof(string) || value.GetType() != typeof(int)))
                    sb.AppendLine($"   {name} = {JsonConvert.SerializeObject(value)}");
                else
                    sb.AppendLine($"   {name} = {value}");
            }

            return sb.ToString();
        }
    }
}