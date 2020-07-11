using System;
using System.Collections.Generic;

namespace HCILab.Website.Models
{
    public partial class Author
    {
        public Author()
        {
            Authoraffiliation = new HashSet<Authoraffiliation>();
            Authorarticles = new HashSet<Authorarticles>();
        }

        public int Id { get; set; }
        public string IdOrcid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string AuthorUrl { get; set; }

        public virtual ICollection<Authoraffiliation> Authoraffiliation { get; set; }
        public virtual ICollection<Authorarticles> Authorarticles { get; set; }
    }
}
