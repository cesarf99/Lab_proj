using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCILab.Website.Models
{
    public class BodyClass
    {
        [JsonProperty("doi", NullValueHandling = NullValueHandling.Ignore)]
        public string? Doi { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string? Title { get; set; }

        [JsonProperty("authorname", NullValueHandling = NullValueHandling.Ignore)]
        public string? AuthorName { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }
}
