using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderService.Model
{
    public class Settings
    {
        public string PolledDirectory { get; set; }
        public int SecondsBetweenPolls { get; set; }
        public int PulseIntervalSeconds { get; set; }

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
