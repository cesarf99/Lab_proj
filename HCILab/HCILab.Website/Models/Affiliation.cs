using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Affiliation
    {
        public Affiliation()
        {
            Authoraffiliation = new HashSet<Authoraffiliation>();
        }

        public int Id { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionPosition { get; set; }

        public virtual ICollection<Authoraffiliation> Authoraffiliation { get; set; }
    }
}
