using System;
using System.Collections.Generic;

namespace HCILab.Researchers.Models
{
    public partial class Researcher
    {
        public int IdResearcher { get; set; }
        public string Name { get; set; }
        public string OrcidId { get; set; }
        public string ScopusId { get; set; }
    }
}
