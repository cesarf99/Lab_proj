using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HCILab
{
    public class IEEE
    {
        private readonly HttpClient _client;
        private readonly string _apiKey = "uxtnfcanpectrvmthg7zau89";

        public IEEE()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(@"http://ieeexploreapi.ieee.org/api/v1/")
            };
        }
        public Task SendAsync(object request)
        {
            throw new NotImplementedException();
        }

        public async Task<object> SearchArticlesAsync()
        {
            var result = await _client.GetAsync($"search/articles?parameter&apikey={_apiKey}");
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<object> SearchAbstract(string search)
        {
            string search_uri = $"search/articles?parameter&apikey={_apiKey}&parameter&abstract={search}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<object> SearchAuthor(string search)
        {
            string search_uri = $"search/articles?parameter&apikey={_apiKey}&parameter&author={search}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<object> SearchAffiliation(string search)
        {
            string search_uri = $"search/articles?parameter&apikey={_apiKey}&parameter&affiliation ={search}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<object> SearchTitle(string search)
        {
            string search_uri = $"search/articles?parameter&apikey={_apiKey}&parameter&article_title={search}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<object> SearchArticleNumber(string search)
        {
            string search_uri = $"search/articles?parameter&apikey={_apiKey}&parameter&article_number={search}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<object> SearchDoi(string search)
        {
            string search_uri = $"search/articles?parameter&apikey={_apiKey}&parameter&doi={search}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}