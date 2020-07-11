using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Reference
    {
        public int IdRef { get; set; }
        public int ArticleId { get; set; }
        public string CitingPaperId { get; set; }

        public virtual Article Article { get; set; }
    }
}
