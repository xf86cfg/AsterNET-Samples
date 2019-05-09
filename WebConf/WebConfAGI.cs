using Asterisk.NET.FastAGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebConf.Data;

namespace WebConf
{
    public class WebConfAGI : AGIScript
    {
        public static List<Conference> Conferences;
        public static string CallRecordingPath = "<path_to_call_recordings>";

        public Conference CurrentConf;

        private static void InitData()
        {
            Conferences = new List<Conference>()
            {
                new Conference() {
                    ConferenceID = "0001",
                    PIN = "1234",
                    AdminPIN = "1234",
                    RecordConference = true
                }
            };
        }

        public override void Service(AGIRequest param1, AGIChannel param2)
        {
            try
            {
                // Load some sample data
                InitData();

                // Answer the inbound call
                Answer();

                string confPin = string.Empty;
                string confId = string.Empty;
                bool isAdmin = false;

                while (CurrentConf == null)
                {
                    // Get customers pin
                    StreamFile("webconf-thank-you-for-calling");
                    string conferencepin = GetData("webconf-enter-conf-pin", 20000);
                    if (conferencepin.Length > 0)
                    {
                        // Confirm customers PIN
                        StreamFile("webconf-you-entered");
                        SayDigits(conferencepin);

                        if (GetData("webconf-is_this_correct") == "1")
                        {
                            // validate pin
                            // pin is 4 digits conf number, 4 digits pin
                            confPin = conferencepin.Substring(conferencepin.Length - 4);
                            confId = conferencepin.Substring(0, conferencepin.Length - 4);

                            CurrentConf = Conferences.Where(c => c.ConferenceID == confId && (c.PIN == confPin || c.AdminPIN == confPin)).SingleOrDefault();
                            if (CurrentConf == null)
                            {
                                StreamFile("webconf-conf_not_found");
                            }
                            isAdmin = confPin == CurrentConf.AdminPIN;
                        }
                        else
                        {
                            // PIN was not correct
                            StreamFile("webconf-conf_not_found");
                        }
                    }
                }

                // get the user to say their name
                StreamFile("webconf-please_say_your_name");
                string nameFile = "conference/" + param1.UniqueId.Replace(".", "-");
                StreamFile("beep");
                RecordFile(nameFile, "gsm", "#", 10000);

                SetVariable("CONFBRIDGE_JOIN_SOUND", nameFile);
                SetVariable("CONFBRIDGE_LEAVE_SOUND", nameFile);

                CurrentConf.Members.Add(new ConferenceMember()
                {
                    CallerID = param1.CallerId,
                    CallUniqueID =param1.UniqueId,
                    IsAdmin = isAdmin,
                    JoinTime = DateTime.Now
                });

                Exec("ConfBridge", confId.ToString() + ",M,c");
            }
            catch (AGIHangupException hex)
            {
                // Catch hangup

            }
            catch (Exception ex)
            {
                // Something else

            }
            finally
            {
                // Remove the conference member - if null, we were unable to get as far as adding the member to the 
                // conference.
                var member = CurrentConf.Members.Where(m => m.CallUniqueID == param1.UniqueId).SingleOrDefault();
                if (member != null)
                {
                    CurrentConf.Members.Remove(member);
                }

                // Check conference member count
                if (CurrentConf != null && CurrentConf.CallerCount == 0)
                {
                    // Last person out
                    if (CurrentConf.RecordConference && !string.IsNullOrEmpty(CurrentConf.AdminEmailAddress))
                    {
                        // Get the recording and email it
                        // This assumes that you have a shared path to call recordings

                    }
                }
            }
        }
    }
}
