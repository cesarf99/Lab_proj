using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HCILab.Interface.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HCILab.Interface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(@"http://localhost:61059/api/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public Task SendAsync(object request)
        {
            throw new NotImplementedException();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ArticleMenu()
        {
            return View();
        }
        public IActionResult ArticleSearch()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ArticleSearch(IFormCollection data)
        {
            string id = data["ID"];

            if (id != null)
            {
                string authors = data["group3"];
                if (authors == null)
                {
                    authors = "false";
                }

                string Key = data["group4"];
                if (Key == null)
                {
                    Key = "false";
                }

                string search_uri = $"Article/{id}?Authors={authors}&Keywords={Key}";

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);

                var response = await _client.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                ViewBag.r = responseContent;
            }
            else
            {
                string search_uri = $"Article?";

                string orcidid = data["OrcidID"];
                string year = data["year"];
                string Journal = data["journal"];
                string Conference = data["conference"];
                string Book = data["book"];
                string Keyword = data["keywords"];
                string AuthorName = data["name"];

                if (!String.IsNullOrEmpty(orcidid))
                {
                    string a = "orcidid=" + orcidid + "&";
                    search_uri = search_uri + a;
                }
                if (!String.IsNullOrEmpty(year))
                {
                    string a = "year=" + year + "&";
                    search_uri = search_uri + a;
                }
                if (!String.IsNullOrEmpty(Journal))
                {
                    string a = "Journal=" + Journal + "&";
                    search_uri = search_uri + a;
                }
                if (!String.IsNullOrEmpty(Conference))
                {
                    string a = "Conference=" + Conference + "&";
                    search_uri = search_uri + a;
                }
                if (!String.IsNullOrEmpty(Book))
                {
                    string a = "Book=" + Book + "&";
                    search_uri = search_uri + a;
                }
                if (!String.IsNullOrEmpty(Keyword))
                {
                    string a = "keyword=" + Keyword + "&";
                    search_uri = search_uri + a;
                }
                if (!String.IsNullOrEmpty(AuthorName))
                {
                    string a = "AuthorName=" + AuthorName + "&";
                    search_uri = search_uri + a;
                }

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);

                var response = await _client.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                ViewBag.r = responseContent;
            }


            return View("ListArticleSearch");
        }
        public IActionResult ArticleInsert()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ArticleInsert(IFormCollection data)
        {
            var contentform = new Dictionary<string, string>()
            {
                { data["group1"], data["Text"] },
                { "source", data["group2"] }
            };

            string content = JsonConvert.SerializeObject(contentform, Formatting.Indented);

            string search_uri = $"Article";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, search_uri);
            request.Content = new StringContent(content, System.Text.UnicodeEncoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            ViewBag.r = responseContent;

            return View("ListArticleInsert");
        }
        public IActionResult ArticleEdit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ArticleEdit(IFormCollection data)
        {
            var id = data["Id"];

            var contentform = new Dictionary<string, string>()
            {
                { "Title", data["Title"] },
                { "Year", data["Year"] },
                { "Putcode", data["Putcode"] },
                { "Abstract" , data["Abstract"] },
                { "Startpage" , data["Startpage"] },
                { "Endpage" , data["Endpage"] }
            };

            string content = JsonConvert.SerializeObject(contentform, Formatting.Indented);

            string search_uri = $"Article/{id}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, search_uri);
            request.Content = new StringContent(content, System.Text.UnicodeEncoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            ViewBag.r = responseContent;

            return View("ListArticleEdit");
        }
        public IActionResult AuthorSearch()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AuthorSearch(IFormCollection data)
        {
            var orcidid = data["OrcidID"];
            var authorname = data["name"];
            string search_uri;

            if (!String.IsNullOrEmpty(orcidid))
            {
                search_uri = $"Author?orcidid={orcidid}";
            }
            else if (!String.IsNullOrEmpty(authorname))
            {
                search_uri = $"Author?authorname={authorname}";
            }
            else if (!String.IsNullOrEmpty(orcidid) && !String.IsNullOrEmpty(authorname))
            {
                search_uri = $"Author?orcidid={orcidid}&authorname={authorname}";
            }
            else
            {
                search_uri = $"Author";
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            ViewBag.r = responseContent;

            return View("ListAuthorSearch");
        }
        public IActionResult AuthorInsert()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AuthorInsert(IFormCollection data)
        {
            string content = data["IdOrcid"];

            string search_uri = $"Author";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, search_uri);
            request.Content = new StringContent(content, System.Text.UnicodeEncoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            ViewBag.r = responseContent;

            return View("ListAuthorInsert");
        }
        public IActionResult AuthorEdit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AuthorEdit(IFormCollection data)
        {
            var id = data["Id"];

            var contentform = new Dictionary<string, string>()
            {
            };

            string orcid = data["IdOrcid"];
            string name = data["Name"];
            string email = data["Email"];
            string country = data["Country"];
            string authorurl = data["AuthorUrl"];

            if (!String.IsNullOrEmpty(orcid))
            {
                contentform.Add("idorcid", orcid);
            }
            if (!String.IsNullOrEmpty(name))
            {
                contentform.Add("name", name);
            }
            if (!String.IsNullOrEmpty(email))
            {
                contentform.Add("email", email);
            }
            if (!String.IsNullOrEmpty(country))
            {
                contentform.Add("country", country);
            }
            if (!String.IsNullOrEmpty(authorurl))
            {
                contentform.Add("authorurl", authorurl);
            }

            string content = JsonConvert.SerializeObject(contentform, Formatting.Indented);

            string search_uri = $"Author/{id}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, search_uri);
            request.Content = new StringContent(content, System.Text.UnicodeEncoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            ViewBag.r = responseContent;

            return View("ListAuthorEdit");
        }
        public IActionResult AffiliationSearch()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AffiliationSearchAsync(IFormCollection data)
        {
            var country = data["country"];
            var name = data["name"];
            string search_uri = $"Affiliations?Country={country}&Name={name}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, search_uri);

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            ViewBag.r = responseContent;

            return View("ListAffiliationSearch");
        }
        public IActionResult ListAffiliationSearch()
        {
            return View();
        }
        public IActionResult ListArticleSearch()
        {
            return View();
        }
        public IActionResult ListAuthorSearch()
        {
            return View();
        }
        public IActionResult ListAuthorInsert()
        {
            return View();
        }
        public IActionResult ListArticleInsert()
        {
            return View();
        }
        public IActionResult ListAffiliationInsert()
        {
            return View();
        }
        public IActionResult ListArticleEdit()
        {
            return View();
        }
        public IActionResult ListAuthorEdit()
        {
            return View();
        }

    }
}
