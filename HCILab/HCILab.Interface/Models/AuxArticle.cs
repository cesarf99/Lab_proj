using Newtonsoft.Json;

namespace HCILab.Website.APIRequests
{
    public class AuxArticle
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("year")]
        public int Year { get; set; }
        [JsonProperty("putcode")]
        public int PutCode { get; set; }
        [JsonProperty("abstract")]
        public string Abstract { get; set; }
        [JsonProperty("startpage")]
        public string StartPage { get; set; }
        [JsonProperty("endpage")]
        public string EndPage { get; set; }
    }
}
