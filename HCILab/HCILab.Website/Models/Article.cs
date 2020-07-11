using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Article
    {
        public Article()
        {
            Articlekeyword = new HashSet<Articlekeyword>();
            Authorarticles = new HashSet<Authorarticles>();
            ExternalId = new HashSet<ExternalId>();
            Tableieee = new HashSet<Tableieee>();
            Tableorcid = new HashSet<Tableorcid>();
        }

        public int IdArticle { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public int? PutCode { get; set; }
        public string Abstract { get; set; }
        public string StartPage { get; set; }
        public string EndPage { get; set; }

        public virtual Book Book { get; set; }
        public virtual Conference Conference { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual ICollection<Articlekeyword> Articlekeyword { get; set; }
        public virtual ICollection<Authorarticles> Authorarticles { get; set; }
        public virtual ICollection<ExternalId> ExternalId { get; set; }
        public virtual ICollection<Tableieee> Tableieee { get; set; }
        public virtual ICollection<Tableorcid> Tableorcid { get; set; }
    }
}
