using Asterisk.NET.FastAGI;
using Asterisk.NET.FastAGI.MappingStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            AsteriskFastAGI agiServer = new AsteriskFastAGI();
            agiServer.MappingStrategy = new GeneralMappingStrategy(
                new List<ScriptMapping>()
                {
                    new ScriptMapping() {
                        ScriptName = "callmonitor",
                        ScriptClass = "CallMonitor.CallMonitorAGI"
                    }
                });

            Console.WriteLine("Press ctrl-c to exit...");
            agiServer.Start();
        }
    }
}
