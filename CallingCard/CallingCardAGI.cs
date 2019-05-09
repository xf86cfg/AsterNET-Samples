using AsterNET.Extensions.FastAGI.Helpers;
using AsterNET.FastAGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CallingCardDAL;

namespace CallingCard
{
    public class CallingCardAGI : AsterNET.Extensions.FastAGI.AGIScript.AGIScript
    {
        protected static int maxAttempts = 4;
        protected static string Trunk = "pstn_trunk_here";
        protected static List<CallingCardDAL.CallingCard> ActiveCards;
        
        public override void Service(AGIRequest param1, AGIChannel param2)
        {
            int attempts = 0;
            double callCost = 0;
            CallingCardDAL.CallingCard card = null;
            
            while (card == null && attempts < maxAttempts)
            {
                // Get 8 Digit card number
                var num = GetData("card-number", 10000, 8);
                card = CallingCardDAL.CallingCard.GetCardByNum(num);
                attempts++;
            }

            if (ActiveCards.Contains(card))
            {
                // Card is already in use
                return;
            }

            try
            {
                // Inform customer of balance
                StreamFile("card-balance-is");
                // SayDigits(card.CardBalance);

                // Get the number they wish to dial
                var numToDial = GetData("ccard-enter-telephone-number", 15000);

                // Calculate the remaining time for this dialing code
                var rate = CallRate.GetCallRate(numToDial);

                // get the total seconds remaining for this call rate
                var seconds = (int) card.CardBalance/rate;

                // Lock the card so no other calls can be made while the call is active
                ActiveCards.Add(card);

                // Record call start time
                var callStart = DateTime.Now;

                // Make the call, fix the maximum duration
                // values are in ms, so warn with 30 seconds remaining, and repeat every 10 seconds
                var dialResult = Dial("", 60, null, DialOptions.LimitAndWarn(seconds*1000, 30000, 10000));
                if (dialResult == DialStatus.Answer)
                {
                    // Record call duration and cost as the call was answered
                    var duration = (DateTime.Now - callStart).TotalSeconds;
                    callCost = duration*rate;
                }
            }
            catch (AGIHangupException hangupException)
            {

            }
            finally
            {
                // Update balance as whole number
                card.CardBalance -= (int) callCost;
                // Ensure card is removed from active cards lock
                ActiveCards.Remove(card);
            }
        }
    }
}
