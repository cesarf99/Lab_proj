using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Journal
    {
        public int IdJournal { get; set; }
        public string JournalTitle { get; set; }

        public virtual Article IdJournalNavigation { get; set; }
    }
}
