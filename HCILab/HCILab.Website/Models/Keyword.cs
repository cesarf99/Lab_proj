using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Keyword
    {
        public Keyword()
        {
            Articlekeyword = new HashSet<Articlekeyword>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Articlekeyword> Articlekeyword { get; set; }
    }
}
