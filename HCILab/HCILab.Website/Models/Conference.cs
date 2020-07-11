using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Conference
    {
        public int IdConference { get; set; }
        public string ConferenceName { get; set; }
        public string ConferenceDates { get; set; }
        public string ConferenceLocation { get; set; }

        public virtual Article IdConferenceNavigation { get; set; }
    }
}
