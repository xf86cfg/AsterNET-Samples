using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConf.Data
{
    public class Conference
    {
        /// <summary>
        /// 4 Digit ID of the conference
        /// </summary>
        public string ConferenceID { get; set; }
        /// <summary>
        /// General access PIN
        /// </summary>
        public string PIN { get; set; }
        /// <summary>
        /// PIN for Administrator of conference
        /// </summary>
        public string AdminPIN { get; set; }
        /// <summary>
        /// Boolean, do you want the conference recorded
        /// </summary>
        public bool RecordConference { get; set; }
        /// <summary>
        /// If conference is recorded, when everyone has left, email the recording to this email address
        /// </summary>
        public string AdminEmailAddress { get; set; }


        /// <summary>
        /// Current caller count for the conference
        /// </summary>
        public int CallerCount
        {
            get { return Members.Count; }
        }

        public List<ConferenceMember> Members { get; set; }

        public Conference()
        {
            Members = new List<ConferenceMember>();
        }
    }

    public class ConferenceMember
    {
        public bool IsAdmin { get; set; }
        public string CallerID { get; set; }
        public DateTime JoinTime { get; set; }
        public string CallUniqueID { get; set; }
    }
}
