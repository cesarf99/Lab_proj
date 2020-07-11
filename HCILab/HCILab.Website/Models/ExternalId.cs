using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class ExternalId
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        public virtual Article Article { get; set; }
    }
}
