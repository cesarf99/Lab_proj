using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Tableieee
    {
        public int IdIeee { get; set; }
        public int ArticleId { get; set; }
        public int? CitingPaperCount { get; set; }
        public int? CitingPatentCount { get; set; }

        public virtual Article Article { get; set; }
    }
}
