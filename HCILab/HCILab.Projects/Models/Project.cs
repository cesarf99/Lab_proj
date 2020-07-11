using System;
using System.Collections.Generic;

namespace HCILab.Projects.Models
{
    public partial class Project
    {
        public Project()
        {
            Connectionresearcher = new HashSet<Connectionresearcher>();
        }

        public int IdProject { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Financer { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Url { get; set; }
        public string Tipo { get; set; }
        public string GrantNumber { get; set; }
        public string Putcode { get; set; }

        public virtual ICollection<Connectionresearcher> Connectionresearcher { get; set; }
    }
}
