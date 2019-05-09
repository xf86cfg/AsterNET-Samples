using System.IO;
using AsterNET.FastAGI;

namespace AccCheckExample
{
    class AccCheckAGI : AGIScript
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
            // # Play the file called "acccheck-enter_your_acc_number"
            // # wait a fixed period of 10 seconds (10000ms) for them to
            //   enter the digits.
            var digits = GetData("acccheck-enter_your_acc_number", 10000);

            if (!string.IsNullOrEmpty(digits))
            {
                // Do some work on an external source to query the
                // balance of this account. We're just pretend it
                // returns a balance of £4.99
                var balance = "4.99";
                var pounds = balance.Split(new char[] {'.'})[0];
                var pence = balance.Split(new char[] { '.' })[1];

                StreamFile("acccheck-your_balance_is");

                SayNumber(pounds);
                StreamFile("acccheck-pounds");
                StreamFile("acccheck-and");

                SayNumber(pence);
                StreamFile("acccheck-pence");
            }

            Hangup();
        }
    }
}
