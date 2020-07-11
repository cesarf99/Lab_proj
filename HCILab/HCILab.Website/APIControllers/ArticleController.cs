using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HCILab.Website.Models;
using Newtonsoft.Json;
using HCILab.Website.Controllers;
using HCILab.Website.APIRequests;
using Google.Protobuf.WellKnownTypes;

namespace HCILab.Website.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly hcilabContext _context;
        private readonly IEEEController _ieee;
        private readonly OrcidController _orcid;

        public ArticleController(hcilabContext context, IEEEController ieee, OrcidController orcid)
        {
            _context = context;
            _ieee = ieee;
            _orcid = orcid;
        }

        // GET: api/Article
        [HttpGet]
        public object GetArticle([FromQuery] QueryClass query)
        {
            //instanciar valores por defeitos para os atributos
            //pesquisar objeto query os parametros introduzidos pelo utilizador

            if (query?.Keyword != null)
            {
                return _context.Articlekeyword.Where(x => x.IdKeywordNavigation.Value.Contains(query.Keyword))
                    .Include(x => x.Article).Include(x=>x.IdKeywordNavigation);
            }
            else if (query?.AuthorName != null)
            {
                return _context.Authorarticles.Where(x => x.Author.Name.Contains(query.AuthorName)).Include(x => x.Article).Include(x => x.Author);
            }
            else if (query?.Year != 0)
            {
                return _context.Article.Where(x => x.Year == query.Year);
            }
            else
            {
                return _context.Article.Where(x =>
                (string.IsNullOrEmpty(query.Book) || x.Book.BookName.Contains(query.Book))
            && (string.IsNullOrEmpty(query.Conference) || x.Conference.ConferenceName.Contains(query.Conference))
             && (string.IsNullOrEmpty(query.Journal) || x.Journal.JournalTitle.Contains(query.Journal))
        ).Include(x => x.Conference).Include(x => x.Book).Include(x => x.Journal).Include(x => x.Tableorcid).Include(x => x.Tableieee);
            }
        }

        // GET: api/Article/id
        [HttpGet("{id}")]
        public object GetArticle(int id, [FromQuery] QueryOpcional query)
        {
            if (query.Keywords == true)
            {
                return _context.Articlekeyword.Where(x => x.ArticleId == id).Include(x => x.Article).Include(x=>x.IdKeywordNavigation);
            }
            else if (query.Authors == true)
            {
                return _context.Authorarticles.Where(x => x.ArticleId == id).Include(x => x.Article).Include(X => X.Author);
            }
            else
            {
                return _context.Article.Where(x => x.IdArticle == id).Include(x => x.Journal).Include(x => x.Conference).Include(x => x.Book).Include(x => x.Tableorcid).Include(x => x.Tableieee);
            }
        }

        // PUT: api/Article/5
        [HttpPut("{id}")]
        public IActionResult PutArticle(int id, [FromBody] AuxArticle request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            Article article = _context.Article.FirstOrDefault(x => x.IdArticle == id);

            if (article == null)
            {
                return NotFound();
            }

            if (request.Year != 0)
            {
                article.Year = request.Year;
                _context.Article.Update(article);
                _context.SaveChanges();
            }
            if (request.Title != null)
            {
                article.Title = request.Title;
                _context.Article.Update(article);
                _context.SaveChanges();
            }
            if (request.StartPage != null)
            {
                article.StartPage = request.StartPage;
                _context.Article.Update(article);
                _context.SaveChanges();
            }
            if (request.EndPage != null)
            {
                article.EndPage = request.EndPage;
                _context.Article.Update(article);
                _context.SaveChanges();
            }
            if (request.PutCode != 0)
            {
                article.PutCode = request.PutCode;
                _context.Article.Update(article);
                _context.SaveChanges();
            }
            if (request.Abstract != null)
            {
                article.Abstract = request.Abstract;
                _context.Article.Update(article);
                _context.SaveChanges();
            }

            return NoContent();
        }

        // POST: api/Article
        [HttpPost]
        public async Task<object> PostArticle([FromBody] object request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            string request1 = request.ToString();
            var s = JsonConvert.DeserializeObject<BodyClass>(request1);

            dynamic re = s.ToString();

            if (s.Source == "orcid")
            {
                SearchViewModel a = new SearchViewModel();
                a.Text = s.Id;

                var result = await _orcid.PostWorks(a);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            else if (s.Source == "ieee")
            {
                if (s.Doi != null)
                {
                    SearchViewModel d = new SearchViewModel();
                    d.Text = s.Doi;

                    var result = await _ieee.PostDoi(d);

                    if (result == null)
                    {
                        return NotFound();
                    }
                    return result;

                }
                else if (s.Title != null)
                {
                    SearchViewModel search = new SearchViewModel();
                    search.Text = s.Title;

                    var result = await _ieee.PostTitle(search);

                    if (result == null)
                    {
                        return NotFound();
                    }

                    return result;
                }
                else if (s.AuthorName != null)
                {
                    SearchViewModel search = new SearchViewModel();
                    search.Text = s.Title;

                    var result = await _ieee.PostAuthor(search);

                    if (result == null)
                    {
                        return NotFound();
                    }

                    return result;
                }
            }
            return re;
        }
    }
}
