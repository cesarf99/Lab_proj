using Newtonsoft.Json;

namespace HCILab.Website.APIRequests
{
    public class AuxAuthor
    {
        [JsonProperty("idorcid")]
        public string IdOrcid { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("authorurl")]
        public string AuthorUrl { get; set; }
    }
}
