using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Authorarticles
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
        public virtual Author Author { get; set; }
    }
}
