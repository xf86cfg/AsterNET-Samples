using Asterisk.NET.FastAGI;
using Asterisk.NET.FastAGI.MappingStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConf
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
                        ScriptName = "webconf",
                        ScriptClass = "WebConf.WebConfAGI"
                    }
                });

            agiServer.SCHANGUP_CAUSES_EXCEPTION = true;
            agiServer.SC511_CAUSES_EXCEPTION = true;

            Console.WriteLine("Press ctrl-c to exit...");
            agiServer.Start();
        }
    }
}
