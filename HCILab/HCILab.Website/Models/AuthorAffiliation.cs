using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Authoraffiliation
    {
        public int Id { get; set; }
        public int IdAuthor { get; set; }
        public int IdAffiliation { get; set; }

        public virtual Affiliation IdAffiliationNavigation { get; set; }
        public virtual Author IdAuthorNavigation { get; set; }
    }
}
