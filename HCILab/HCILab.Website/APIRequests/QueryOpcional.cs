using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCILab.Website.APIRequests
{
    public class QueryOpcional
    {
        public bool Authors { get; set; }
        public bool Keywords { get; set; }
        public bool TableOrcid { get; set; }
        public bool TableIeee { get; set; }
        public bool Book { get; set; }
        public bool Journal { get; set; }
        public bool Conference { get; set; }

    }
}
