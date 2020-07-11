using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Articlekeyword
    {
        public int Id { get; set; }
        public int IdKeyword { get; set; }
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
        public virtual Keyword IdKeywordNavigation { get; set; }
    }
}
