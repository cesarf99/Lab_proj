using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Book
    {
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public string Publisher { get; set; }
        public string Volume { get; set; }

        public virtual Article IdBookNavigation { get; set; }
    }
}
