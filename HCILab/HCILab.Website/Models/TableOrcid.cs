using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Tableorcid
    {
        public int IdOrcid { get; set; }
        public int ArticleId { get; set; }
        public string Path { get; set; }
        public string SourceName { get; set; }

        public virtual Article Article { get; set; }
    }
}
