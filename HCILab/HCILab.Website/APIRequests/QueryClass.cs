using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace HCILab.Website.Models
{
    public class QueryClass
    {
        public int Year { get; set; }
        public string Journal { get; set; }
        public string Conference { get; set; }
        public string Book { get; set; }
        public string Keyword { get; set; }
        public string OrcidId { get; set; }
        public string AuthorName { get; set; }

    }
}
