using Asterisk.NET.FastAGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMonitor
{

    /// <summary>
    /// Simple AGI that accepts an incoming call, logs the phone number and
    /// duration and allows you to report based on the phone number the total
    /// number of minutes of 'Active' call during a given period.
    /// 
    /// This example works well for things like Solicitors who might want to 
    /// generate reports for time-usage talking to clients, along with call
    /// recordings for proof.
    /// </summary>
    public class CallMonitorAGI : AGIScript
    {
        public override void Service(AGIRequest param1, AGIChannel param2)
        {
            // Answer the call, this is important as otherwise
		    // we wont be able to perform certain action on the 
		    // call flow.
            Answer();
		
		    // Get a numeric sequence from the caller, we prompt them
		    // to enter the number by playing an audio file asking 
		    // them to do so.
		    // We're going to:
		    // # Play the file called "enter_your_acc_number"
		    // # limit the number of digits to 4
		    // # wait a fixed period of 30 seconds for them to
		    //   enter the digits.
            var digits = GetData("enter_your_acc_number", 30, 4);

            if (!string.IsNullOrEmpty(digits))
            {
                
            }
        }
    }
}
