using AsterNET.FastAGI;

namespace ReDialer
{
    public class ReDialerAGI : AGIScript
    {
        /*
         *  Asterisk ReDialer Script
         *  Allows a remote caller to enter a PIN and then a number to call
         *  
        */

        public string securityPIN = "1234";
        public string pstnTrunk = "ZAP/trunk1/";
        public bool RecordCalls = true;

        public override void Service(AGIRequest param1, AGIChannel param2)
        {
            Answer();
            var pin = GetData("redialer-enter-pin-number", 10000, securityPIN.Length);
            if(pin == securityPIN) 
            {
                while (GetChannelStatus() == 6)     // caller is connected
                {
                    var opt = GetData("redialer-welcome", 5000, 1);
                    switch (opt)
                    {
                        case "1":
                            // Get the number to be dialled
                            var numberToDial = GetData("redialer-enter-phone-number", 15000, 14);

                            // TODO: Remember this number for the given inbound CallerID
                            
                            // Dial the number
                            var dialString = pstnTrunk + numberToDial;

                            // Enable Call Recording
                            if (RecordCalls)
                                Exec("mixmonitor", string.Format("{0}.{1},b", param1.UniqueId, "wav"));

                            Exec("Dial", pstnTrunk + numberToDial);
                            break;
                        case "2":
                            // Get last number dialled for this inbound CallerID
                            StreamFile("redialer-redialling");
                            break;
                        case "3":
                            var clid = GetData("redialer-enter-callerid", 15000, 14);
                            SetCallerId(clid);
                            StreamFile("redialer-thankyou-clid");
                            break;
                        default:

                            break;
                    }
                }
            }else{
                // Invalid PIN number
                StreamFile("redialer-invalid-pin");
            }

            Hangup();
        }
    }
}
