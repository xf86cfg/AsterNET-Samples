using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsterNET.FastAGI;
using AsterNET.FastAGI.MappingStrategies;

namespace ReDialer
{
    public class Program
    {
        static void Main()
        {
            AsteriskFastAGI agiServer = new AsteriskFastAGI();
            agiServer.MappingStrategy = new GeneralMappingStrategy(
                new List<ScriptMapping>()
                {
                    new ScriptMapping() {
                        ScriptName = "redialer",
                        ScriptClass = "ReDialer.ReDialerAGI"
                    }
                });

            Console.WriteLine("Press ctrl-c to exit...");
            agiServer.Start();
        }
    }
}
