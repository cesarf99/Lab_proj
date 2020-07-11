using System;
using System.Collections.Generic;

namespace HCILab.Projects.Models
{
    public partial class Connectionresearcher
    {
        public int Id { get; set; }
        public string OrcidId { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
