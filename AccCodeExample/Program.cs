using AsterNET.FastAGI;
using AsterNET.FastAGI.MappingStrategies;
using System;
using System.Collections.Generic;

namespace AccCheckExample
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
				    ScriptName = "acccheck",
				    ScriptClass = "AccCheckExample.AccCheckAGI"
			    }
		    });

            Console.WriteLine("Press ctrl-c to exit...");
            agiServer.Start();
        }
    }
}
