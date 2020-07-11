using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HCILab
{
    public class Orcid
    {
        private readonly HttpClient _client;
        private static string access_token;
        
        public Orcid()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(@"https://orcid.org/")
            };
        }

        public Task SendAsync(object request)
        {
            throw new NotImplementedException();
        }

        public async Task<object> AuthenticateAsync()
        {
            var content = new Dictionary<string, string>()
            {
                { "client_id", "APP-C2INP415OM3UZWYA" },
                { "client_secret", "495dc594-0524-4b7f-be90-f1c8790b2efc" },
                { "grant_type", "client_credentials" },
                { "scope" , "/read-public" }
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "oauth/token");
            request.Headers.Add("Accept", "application/json");
            request.Content = new FormUrlEncodedContent(content);

            var response = await _client.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            dynamic authentication = JsonConvert.DeserializeObject(responseContent);

            access_token = authentication.access_token;
            string refreshToken = authentication.refresh_token;
            DateTime expirationDate = DateTime.UtcNow.AddSeconds((long)authentication.expires_in);

            return responseContent;
        }
       
        public async Task<object> OrcidAllAsync(string search)
        {

            string target_url = $"https://pub.orcid.org/{search}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target_url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", access_token);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        public async Task<object> WorksAsync(string search)
        {

            string target_url = $"https://pub.orcid.org/{search}/works";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target_url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", access_token);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        public async Task<object> WorkDetailsAsync(string search, string code)
        {

            string target_url = $"https://pub.orcid.org/{search}/work/{code}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target_url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", access_token);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        public async Task<object> RecordAsync(string search)
        {

            string target_url = $"https://pub.orcid.org/{search}/record";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target_url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", access_token);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
       
        public async Task<object> PersonAsync(string search)
        {

            string target_url = $"https://pub.orcid.org/{search}/person";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target_url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", access_token);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
       
        public async Task<object> EmploymentsAsync(string search)
        {

            string target_url = $"https://pub.orcid.org/{search}/employments";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target_url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", access_token);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
        
        public async Task<object> EducationsAsync(string search)
        {

            string target_url = $"https://pub.orcid.org/{search}/educations";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target_url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", access_token);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
        
        public async Task<object> Search(string search)
        {
            string search_uri = $"https://pub.sandbox.orcid.org/v3.0/search/?q={search}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<object> PersonDetailsAsync(string search)
        {

            string target_url = $"https://pub.orcid.org/{search}/personal-details";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target_url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", access_token);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        public async Task<object> FundingsAsync(string search)
        {

            string target_url = $"https://pub.orcid.org/{search}/fundings";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target_url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", access_token);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}