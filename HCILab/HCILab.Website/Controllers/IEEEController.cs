using HCILab.Website.Models;
using IEEEAbstract;
using IEEEAny;
using IEEEAuthor;
using IEEETitle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadIEEE;
using System;
using System.Linq;
using System.Threading.Tasks;
using Article = HCILab.Website.Models.Article;
using Author = HCILab.Website.Models.Author;
using ExternalId = HCILab.Website.Models.ExternalId;

namespace HCILab.Website.Controllers
{
    public class IEEEController : Controller
    {
        private readonly ILogger<IEEEController> _logger;
        private readonly IEEE _ieeeClient;
        private readonly hcilabContext _context;

        public IEEEController(ILogger<IEEEController> logger, IEEE ieeeClient, hcilabContext context)
        {
            _logger = logger;
            _ieeeClient = ieeeClient;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<object> GetAny()
        {
            var result = await _ieeeClient.SearchArticlesAsync();
            return result;
        }

        [HttpGet]
        public async Task<object> GetAuthor(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchAuthor(search.Text);
            return result;
        }

        [HttpGet]
        public async Task<object> GetAffiliation(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchAffiliation(search.Text);
            return result;
        }

        [HttpGet]
        public async Task<object> GetAbstract(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchAbstract(search.Text);
            return result;
        }

        [HttpGet]
        public async Task<object> GetDoi(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchDoi(search.Text);

            return result;
        }

        [HttpGet]
        public async Task<object> GetNumber(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchArticleNumber(search.Text);

            return result;
        }

        [HttpGet]
        public async Task<object> GetTitle(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchTitle(search.Text);

            return result;
        }

        [HttpGet]
        public async Task<object> PostAny()
        {
            var result = await _ieeeClient.SearchArticlesAsync();

            string result1 = result.ToString();
            var read = IeeeAny.FromJson(result1);

            if (read != null)
            {
                foreach (var obj in read.Articles)
                {
                    Article a = _context.Article.FirstOrDefault(x => x.Title == obj.Title);
                    if (a == null)
                    {
                        ExternalId ex = _context.ExternalId.FirstOrDefault(x => x.Value == obj.Doi);
                        if (ex == null)
                        {
                            a = new Article();
                            a.Title = obj.Title;
                            a.Year = Convert.ToInt32(obj.PublicationYear);
                            a.StartPage = obj.StartPage;
                            a.EndPage = obj.EndPage;
                            a.Abstract = obj.Abstract;
                            a.PutCode = Convert.ToInt32(obj.PublicationNumber);
                            _context.Article.Add(a);
                            _context.SaveChanges();

                            string tipo = obj.ContentType.ToString();
                            if (tipo.Contains("conference") == true)
                            {
                                Conference co = new Conference();
                                co.ConferenceDates = obj.ConferenceDates;
                                co.ConferenceLocation = obj.ConferenceLocation;
                                co.ConferenceName = obj.PublicationTitle;
                                co.IdConference = a.IdArticle;

                                _context.Conference.Add(co);
                                _context.SaveChanges();
                            }

                            if (tipo.Contains("journal") == true)
                            {
                                Journal jo = new Journal();
                                jo.JournalTitle = obj.PublicationTitle;
                                jo.IdJournal = a.IdArticle;

                                _context.Journal.Add(jo);
                                _context.SaveChanges();
                            }
                            if (tipo.Contains("book") == true)
                            {
                                Book bo = _context.Book.FirstOrDefault(x => x.BookName == obj.PublicationTitle);
                                if (bo == null)
                                {
                                    bo = new Book();
                                    bo.BookName = obj.PublicationTitle;
                                    bo.IdBook = a.IdArticle;

                                    _context.Book.Add(bo);
                                    _context.SaveChanges();
                                }
                            }

                            if (obj.Doi != null)
                            {
                                ExternalId ext = new ExternalId();
                                ext.Type = "Doi";
                                ext.Value = obj.Doi;
                                ext.ArticleId = a.IdArticle;

                                _context.ExternalId.Add(ext);
                                _context.SaveChanges();

                            }
                            if (obj.Isbn != null)
                            {
                                ExternalId ext = new ExternalId();
                                ext.Type = "Isbn";
                                ext.Value = obj.Isbn;
                                ext.ArticleId = a.IdArticle;

                                _context.ExternalId.Add(ext);
                                _context.SaveChanges();
                            }
                            if (obj.Issn != null)
                            {
                                ExternalId ext = new ExternalId();
                                ext.Type = "Issn";
                                ext.Value = obj.Issn;
                                ext.ArticleId = a.IdArticle;

                                _context.ExternalId.Add(ext);
                                _context.SaveChanges();
                            }

                            if (obj.IndexTerms != null)
                            {
                                if (obj.IndexTerms.IeeeTerms != null)
                                {
                                    foreach (var ieeterms in obj.IndexTerms.IeeeTerms.TermsTerms)
                                    {
                                        Keyword kw = _context.Keyword.FirstOrDefault(x => x.Value == ieeterms);
                                        if (kw == null)
                                        {
                                            kw = new Keyword();
                                            kw.Value = ieeterms;

                                            _context.Keyword.Add(kw);
                                            _context.SaveChanges();

                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kw.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kw.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                    }
                                }

                                if (obj.IndexTerms.AuthorTerms != null)
                                {
                                    foreach (var authorterms in obj.IndexTerms.AuthorTerms.TermsTerms)
                                    {
                                        Keyword kwa = _context.Keyword.FirstOrDefault(x => x.Value == authorterms);
                                        if (kwa == null)
                                        {
                                            kwa = new Keyword();
                                            kwa.Value = authorterms;

                                            _context.Keyword.Add(kwa);
                                            _context.SaveChanges();

                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kwa.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kwa.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                    }
                                }
                            }


                            Tableieee table = new Tableieee();
                            table.CitingPaperCount = Convert.ToInt32(obj.CitingPaperCount);
                            table.CitingPatentCount = Convert.ToInt32(obj.CitingPatentCount);
                            table.ArticleId = a.IdArticle;

                            _context.Tableieee.Add(table);
                            _context.SaveChanges();

                            foreach (var obj1 in obj.Authors.AuthorsAuthors)
                            {
                                Author au = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                                if (au == null)
                                {
                                    au = new Author();
                                    au.Name = obj1.FullName;
                                    if (obj1.AuthorUrl != null)
                                    {
                                        au.AuthorUrl = obj1.AuthorUrl.ToString();
                                    }

                                    _context.Author.Add(au);
                                    _context.SaveChanges();

                                    if (obj1 != null)
                                    {
                                        Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                        if (af == null)
                                        {
                                            af = new Affiliation();
                                            af.InstitutionName = obj1.Affiliation;

                                            _context.Affiliation.Add(af);
                                            _context.SaveChanges();

                                            Authoraffiliation aa = new Authoraffiliation();
                                            aa.IdAffiliation = af.Id;
                                            aa.IdAuthor = au.Id;

                                            _context.Authoraffiliation.Add(aa);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            Authoraffiliation aa = new Authoraffiliation();
                                            aa.IdAffiliation = af.Id;
                                            aa.IdAuthor = au.Id;

                                            _context.Authoraffiliation.Add(aa);
                                            _context.SaveChanges();
                                        }
                                    }

                                    Authorarticles at = new Authorarticles();
                                    at.AuthorId = au.Id;
                                    at.ArticleId = a.IdArticle;

                                    _context.Authorarticles.Add(at);
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    Authorarticles at = new Authorarticles();
                                    at.AuthorId = au.Id;
                                    at.ArticleId = a.IdArticle;

                                    _context.Authorarticles.Add(at);
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                    //se o artigo estiver na base de dados associa os autores aos artigos
                    else
                    {
                        foreach (var obj1 in obj.Authors.AuthorsAuthors)
                        {
                            //se o autor nao estiver na base de dados, cria um e associa o ao artigo
                            Author aut = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                            if (aut == null)
                            {
                                Author au = new Author();
                                au.Name = obj1.FullName;
                                if (obj1.AuthorUrl != null)
                                {
                                    au.AuthorUrl = obj1.AuthorUrl.ToString();
                                }

                                _context.Author.Add(au);
                                _context.SaveChanges();
                                if (obj1 != null)
                                {
                                    Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                    if (af == null)
                                    {
                                        af = new Affiliation();
                                        af.InstitutionName = obj1.Affiliation;

                                        _context.Affiliation.Add(af);
                                        _context.SaveChanges();

                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                }

                                Authorarticles at = new Authorarticles();
                                at.AuthorId = au.Id;
                                at.ArticleId = a.IdArticle;

                                _context.Authorarticles.Add(at);
                                _context.SaveChanges();

                            }
                        }
                    }
                }
                return result;
            }
            else
            {
                return NotFound();
            }


        }

        [HttpGet]
        public async Task<object> PostAuthor(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchAuthor(search.Text);

            string result1 = result.ToString();
            var read = IeeeAuthor.FromJson(result1);
            if (read != null)
            {

            }
            else
            {
                return NotFound();
            }
            foreach (var obj in read.Articles)
            {
                Article a = _context.Article.FirstOrDefault(x => x.Title == obj.Title);
                if (a == null)
                {
                    ExternalId ex = _context.ExternalId.FirstOrDefault(x => x.Value == obj.Doi);
                    if (ex == null)
                    {
                        a = new Article();
                        a.Title = obj.Title;
                        a.Year = Convert.ToInt32(obj.PublicationYear);
                        a.StartPage = obj.StartPage;
                        a.EndPage = obj.EndPage;
                        a.Abstract = obj.Abstract;
                        a.PutCode = Convert.ToInt32(obj.PublicationNumber);
                        _context.Article.Add(a);
                        _context.SaveChanges();

                        string tipo = obj.ContentType.ToString();
                        if (tipo.Contains("conference") == true)
                        {
                            Conference co = new Conference();
                            co.ConferenceDates = obj.ConferenceDates;
                            co.ConferenceLocation = obj.ConferenceLocation;
                            co.ConferenceName = obj.PublicationTitle;
                            co.IdConference = a.IdArticle;

                            _context.Conference.Add(co);
                            _context.SaveChanges();
                        }

                        if (tipo.Contains("journal") == true)
                        {
                            Journal jo = new Journal();
                            jo.JournalTitle = obj.PublicationTitle;
                            jo.IdJournal = a.IdArticle;

                            _context.Journal.Add(jo);
                            _context.SaveChanges();
                        }
                        if (tipo.Contains("book") == true)
                        {
                            Book bo = _context.Book.FirstOrDefault(x => x.BookName == obj.PublicationTitle);
                            if (bo == null)
                            {
                                bo = new Book();
                                bo.BookName = obj.PublicationTitle;
                                bo.IdBook = a.IdArticle;

                                _context.Book.Add(bo);
                                _context.SaveChanges();
                            }
                        }

                        if (obj.Doi != null)
                        {
                            ExternalId ext = new ExternalId();
                            ext.Type = "Doi";
                            ext.Value = obj.Doi;
                            ext.ArticleId = a.IdArticle;

                            _context.ExternalId.Add(ext);
                            _context.SaveChanges();

                        }
                        if (obj.Isbn != null)
                        {
                            ExternalId ext = new ExternalId();
                            ext.Type = "Isbn";
                            ext.Value = obj.Isbn;
                            ext.ArticleId = a.IdArticle;

                            _context.ExternalId.Add(ext);
                            _context.SaveChanges();
                        }
                        if (obj.Issn != null)
                        {
                            ExternalId ext = new ExternalId();
                            ext.Type = "Issn";
                            ext.Value = obj.Issn;
                            ext.ArticleId = a.IdArticle;

                            _context.ExternalId.Add(ext);
                            _context.SaveChanges();
                        }

                        if (obj.IndexTerms != null)
                        {
                            if (obj.IndexTerms.IeeeTerms != null)
                            {
                                foreach (var ieeterms in obj.IndexTerms.IeeeTerms.TermsTerms)
                                {
                                    Keyword kw = _context.Keyword.FirstOrDefault(x => x.Value == ieeterms);
                                    if (kw == null)
                                    {
                                        kw = new Keyword();
                                        kw.Value = ieeterms;

                                        _context.Keyword.Add(kw);
                                        _context.SaveChanges();

                                        Articlekeyword ak = new Articlekeyword();
                                        ak.ArticleId = a.IdArticle;
                                        ak.IdKeyword = kw.Id;
                                        _context.Articlekeyword.Add(ak);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        Articlekeyword ak = new Articlekeyword();
                                        ak.ArticleId = a.IdArticle;
                                        ak.IdKeyword = kw.Id;
                                        _context.Articlekeyword.Add(ak);
                                        _context.SaveChanges();
                                    }
                                }
                            }

                            if (obj.IndexTerms.AuthorTerms != null)
                            {
                                foreach (var authorterms in obj.IndexTerms.AuthorTerms.TermsTerms)
                                {
                                    Keyword kwa = _context.Keyword.FirstOrDefault(x => x.Value == authorterms);
                                    if (kwa == null)
                                    {
                                        kwa = new Keyword();
                                        kwa.Value = authorterms;

                                        _context.Keyword.Add(kwa);
                                        _context.SaveChanges();

                                        Articlekeyword ak = new Articlekeyword();
                                        ak.ArticleId = a.IdArticle;
                                        ak.IdKeyword = kwa.Id;
                                        _context.Articlekeyword.Add(ak);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        Articlekeyword ak = new Articlekeyword();
                                        ak.ArticleId = a.IdArticle;
                                        ak.IdKeyword = kwa.Id;
                                        _context.Articlekeyword.Add(ak);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                        }


                        Tableieee table = new Tableieee();
                        table.CitingPaperCount = Convert.ToInt32(obj.CitingPaperCount);
                        table.CitingPatentCount = Convert.ToInt32(obj.CitingPatentCount);
                        table.ArticleId = a.IdArticle;

                        _context.Tableieee.Add(table);
                        _context.SaveChanges();

                        foreach (var obj1 in obj.Authors.AuthorsAuthors)
                        {
                            Author au = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                            if (au == null)
                            {
                                au = new Author();
                                au.Name = obj1.FullName;
                                if (obj1.AuthorUrl != null)
                                {
                                    au.AuthorUrl = obj1.AuthorUrl.ToString();
                                }

                                _context.Author.Add(au);
                                _context.SaveChanges();

                                if (obj1 != null)
                                {
                                    Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                    if (af == null)
                                    {
                                        af = new Affiliation();
                                        af.InstitutionName = obj1.Affiliation;

                                        _context.Affiliation.Add(af);
                                        _context.SaveChanges();

                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                }



                                Authorarticles at = new Authorarticles();
                                at.AuthorId = au.Id;
                                at.ArticleId = a.IdArticle;

                                _context.Authorarticles.Add(at);
                                _context.SaveChanges();
                            }
                            else
                            {
                                Authorarticles at = new Authorarticles();
                                at.AuthorId = au.Id;
                                at.ArticleId = a.IdArticle;

                                _context.Authorarticles.Add(at);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                //se o artigo estiver na base de dados associa os autores aos artigos
                else
                {
                    foreach (var obj1 in obj.Authors.AuthorsAuthors)
                    {
                        //se o autor nao estiver na base de dados, cria um e associa o ao artigo
                        Author aut = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                        if (aut == null)
                        {
                            Author au = new Author();
                            au.Name = obj1.FullName;
                            if (obj1.AuthorUrl != null)
                            {
                                au.AuthorUrl = obj1.AuthorUrl.ToString();
                            }

                            _context.Author.Add(au);
                            _context.SaveChanges();
                            if (obj1 != null)
                            {
                                Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                if (af == null)
                                {
                                    af = new Affiliation();
                                    af.InstitutionName = obj1.Affiliation;

                                    _context.Affiliation.Add(af);
                                    _context.SaveChanges();

                                    Authoraffiliation aa = new Authoraffiliation();
                                    aa.IdAffiliation = af.Id;
                                    aa.IdAuthor = au.Id;

                                    _context.Authoraffiliation.Add(aa);
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    Authoraffiliation aa = new Authoraffiliation();
                                    aa.IdAffiliation = af.Id;
                                    aa.IdAuthor = au.Id;

                                    _context.Authoraffiliation.Add(aa);
                                    _context.SaveChanges();
                                }
                            }


                            Authorarticles at = new Authorarticles();
                            at.AuthorId = au.Id;
                            at.ArticleId = a.IdArticle;

                            _context.Authorarticles.Add(at);
                            _context.SaveChanges();

                        }
                        //se o autor estiver na base de dados associa o ao artigo
                        else
                        {

                            Authorarticles at = new Authorarticles();
                            at.AuthorId = aut.Id;
                            at.ArticleId = a.IdArticle;

                            _context.Authorarticles.Add(at);
                            _context.SaveChanges();
                        }
                    }
                }
            }
            return result;

        }

        [HttpGet]
        public async Task<object> PostAffiliation(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchAffiliation(search.Text);
            string result1 = result.ToString();
            var read = IEEERead.FromJson(result1);

            if (read != null)
            {
                SaveWorks(read);
            }
            else
            {
                return NotFound();
            }


            return result;

        }

        [HttpGet]
        public async Task<object> PostAbstract(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchAbstract(search.Text);
            string result1 = result.ToString();
            var read = IeeeAbstract.FromJson(result1);

            if (read != null)
            {
                foreach (var obj in read.Articles)
                {
                    Article a = _context.Article.FirstOrDefault(x => x.Title == obj.Title);
                    if (a == null)
                    {
                        ExternalId ex = _context.ExternalId.FirstOrDefault(x => x.Value == obj.Doi);
                        if (ex == null)
                        {
                            a = new Article();
                            a.Title = obj.Title;
                            a.Year = Convert.ToInt32(obj.PublicationYear);
                            a.StartPage = obj.StartPage;
                            a.EndPage = obj.EndPage;
                            a.Abstract = obj.Abstract;
                            a.PutCode = Convert.ToInt32(obj.PublicationNumber);
                            _context.Article.Add(a);
                            _context.SaveChanges();

                            string tipo = obj.ContentType.ToString();
                            if (tipo.Contains("conference") == true)
                            {
                                Conference co = new Conference();
                                co.ConferenceDates = obj.ConferenceDates;
                                co.ConferenceLocation = obj.ConferenceLocation;
                                co.ConferenceName = obj.PublicationTitle;
                                co.IdConference = a.IdArticle;

                                _context.Conference.Add(co);
                                _context.SaveChanges();
                            }

                            if (tipo.Contains("journal") == true)
                            {
                                Journal jo = new Journal();
                                jo.JournalTitle = obj.PublicationTitle;
                                jo.IdJournal = a.IdArticle;

                                _context.Journal.Add(jo);
                                _context.SaveChanges();
                            }
                            if (tipo.Contains("book") == true)
                            {
                                Book bo = _context.Book.FirstOrDefault(x => x.BookName == obj.PublicationTitle);
                                if (bo == null)
                                {
                                    bo = new Book();
                                    bo.BookName = obj.PublicationTitle;
                                    bo.IdBook = a.IdArticle;

                                    _context.Book.Add(bo);
                                    _context.SaveChanges();
                                }
                            }

                            if (obj.Doi != null)
                            {
                                ExternalId ext = new ExternalId();
                                ext.Type = "Doi";
                                ext.Value = obj.Doi;
                                ext.ArticleId = a.IdArticle;

                                _context.ExternalId.Add(ext);
                                _context.SaveChanges();

                            }
                            if (obj.Isbn != null)
                            {
                                ExternalId ext = new ExternalId();
                                ext.Type = "Isbn";
                                ext.Value = obj.Isbn;
                                ext.ArticleId = a.IdArticle;

                                _context.ExternalId.Add(ext);
                                _context.SaveChanges();
                            }
                            if (obj.Issn != null)
                            {
                                ExternalId ext = new ExternalId();
                                ext.Type = "Issn";
                                ext.Value = obj.Issn;
                                ext.ArticleId = a.IdArticle;

                                _context.ExternalId.Add(ext);
                                _context.SaveChanges();
                            }

                            if (obj.IndexTerms != null)
                            {
                                if (obj.IndexTerms.IeeeTerms != null)
                                {
                                    foreach (var ieeterms in obj.IndexTerms.IeeeTerms.TermsTerms)
                                    {
                                        Keyword kw = _context.Keyword.FirstOrDefault(x => x.Value == ieeterms);
                                        if (kw == null)
                                        {
                                            kw = new Keyword();
                                            kw.Value = ieeterms;

                                            _context.Keyword.Add(kw);
                                            _context.SaveChanges();

                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kw.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kw.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                    }
                                }

                                if (obj.IndexTerms.AuthorTerms != null)
                                {
                                    foreach (var authorterms in obj.IndexTerms.AuthorTerms.TermsTerms)
                                    {
                                        Keyword kwa = _context.Keyword.FirstOrDefault(x => x.Value == authorterms);
                                        if (kwa == null)
                                        {
                                            kwa = new Keyword();
                                            kwa.Value = authorterms;

                                            _context.Keyword.Add(kwa);
                                            _context.SaveChanges();

                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kwa.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kwa.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                    }
                                }
                            }


                            Tableieee table = new Tableieee();
                            table.CitingPaperCount = Convert.ToInt32(obj.CitingPaperCount);
                            table.CitingPatentCount = Convert.ToInt32(obj.CitingPatentCount);
                            table.ArticleId = a.IdArticle;

                            _context.Tableieee.Add(table);
                            _context.SaveChanges();

                            foreach (var obj1 in obj.Authors.AuthorsAuthors)
                            {
                                Author au = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                                if (au == null)
                                {
                                    au = new Author();
                                    au.Name = obj1.FullName;
                                    if (obj1.AuthorUrl != null)
                                    {
                                        au.AuthorUrl = obj1.AuthorUrl.ToString();
                                    }

                                    _context.Author.Add(au);
                                    _context.SaveChanges();

                                    if (obj1 != null)
                                    {
                                        Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                        if (af == null)
                                        {
                                            af = new Affiliation();
                                            af.InstitutionName = obj1.Affiliation;

                                            _context.Affiliation.Add(af);
                                            _context.SaveChanges();

                                            Authoraffiliation aa = new Authoraffiliation();
                                            aa.IdAffiliation = af.Id;
                                            aa.IdAuthor = au.Id;

                                            _context.Authoraffiliation.Add(aa);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            Authoraffiliation aa = new Authoraffiliation();
                                            aa.IdAffiliation = af.Id;
                                            aa.IdAuthor = au.Id;

                                            _context.Authoraffiliation.Add(aa);
                                            _context.SaveChanges();
                                        }
                                    }



                                    Authorarticles at = new Authorarticles();
                                    at.AuthorId = au.Id;
                                    at.ArticleId = a.IdArticle;

                                    _context.Authorarticles.Add(at);
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    Authorarticles at = new Authorarticles();
                                    at.AuthorId = au.Id;
                                    at.ArticleId = a.IdArticle;

                                    _context.Authorarticles.Add(at);
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                    //se o artigo estiver na base de dados associa os autores aos artigos
                    else
                    {
                        foreach (var obj1 in obj.Authors.AuthorsAuthors)
                        {
                            //se o autor nao estiver na base de dados, cria um e associa o ao artigo
                            Author aut = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                            if (aut == null)
                            {
                                Author au = new Author();
                                au.Name = obj1.FullName;
                                if (obj1.AuthorUrl != null)
                                {
                                    au.AuthorUrl = obj1.AuthorUrl.ToString();
                                }

                                _context.Author.Add(au);
                                _context.SaveChanges();
                                if (obj1 != null)
                                {
                                    Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                    if (af == null)
                                    {
                                        af = new Affiliation();
                                        af.InstitutionName = obj1.Affiliation;

                                        _context.Affiliation.Add(af);
                                        _context.SaveChanges();

                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                }


                                Authorarticles at = new Authorarticles();
                                at.AuthorId = au.Id;
                                at.ArticleId = a.IdArticle;

                                _context.Authorarticles.Add(at);
                                _context.SaveChanges();

                            }
                            //se o autor estiver na base de dados associa o ao artigo
                            else
                            {

                                Authorarticles at = new Authorarticles();
                                at.AuthorId = aut.Id;
                                at.ArticleId = a.IdArticle;

                                _context.Authorarticles.Add(at);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                return result;
            }
            else
            {
                return NotFound();
            }


        }

        [HttpGet]
        public async Task<object> PostDoi(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchDoi(search.Text);
            string result1 = result.ToString();
            var read = IEEERead.FromJson(result1);
            if (read != null)
            {
                SaveWorks(read);
            }
            else
            {
                return NotFound();
            }


            return result;
        }

        [HttpGet]
        public async Task<object> PostNumber(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchArticleNumber(search.Text);
            string result1 = result.ToString();
            var read = IEEERead.FromJson(result1);
            if (read != null)
            {
                SaveWorks(read);
            }
            else
            {
                return NotFound();
            }


            return result;

        }

        [HttpGet]
        public async Task<object> PostTitle(SearchViewModel search)
        {
            var result = await _ieeeClient.SearchTitle(search.Text);

            string result1 = result.ToString();
            var read = IeeeAuthor.FromJson(result1);
            if (read != null)
            {
                foreach (var obj in read.Articles)
                {
                    Article a = _context.Article.FirstOrDefault(x => x.Title == obj.Title);
                    if (a == null)
                    {
                        ExternalId ex = _context.ExternalId.FirstOrDefault(x => x.Value == obj.Doi);
                        if (ex == null)
                        {
                            a = new Article();
                            a.Title = obj.Title;
                            a.Year = Convert.ToInt32(obj.PublicationYear);
                            a.StartPage = obj.StartPage;
                            a.EndPage = obj.EndPage;
                            a.Abstract = obj.Abstract;
                            a.PutCode = Convert.ToInt32(obj.PublicationNumber);
                            _context.Article.Add(a);
                            _context.SaveChanges();

                            string tipo = obj.ContentType.ToString();
                            if (tipo.Contains("conference") == true)
                            {
                                Conference co = new Conference();
                                co.ConferenceDates = obj.ConferenceDates;
                                co.ConferenceLocation = obj.ConferenceLocation;
                                co.ConferenceName = obj.PublicationTitle;
                                co.IdConference = a.IdArticle;

                                _context.Conference.Add(co);
                                _context.SaveChanges();
                            }

                            if (tipo.Contains("journal") == true)
                            {
                                Journal jo = new Journal();
                                jo.JournalTitle = obj.PublicationTitle;
                                jo.IdJournal = a.IdArticle;

                                _context.Journal.Add(jo);
                                _context.SaveChanges();
                            }
                            if (tipo.Contains("book") == true)
                            {
                                Book bo = _context.Book.FirstOrDefault(x => x.BookName == obj.PublicationTitle);
                                if (bo == null)
                                {
                                    bo = new Book();
                                    bo.BookName = obj.PublicationTitle;
                                    bo.IdBook = a.IdArticle;

                                    _context.Book.Add(bo);
                                    _context.SaveChanges();
                                }
                            }

                            if (obj.Doi != null)
                            {
                                ExternalId ext = new ExternalId();
                                ext.Type = "Doi";
                                ext.Value = obj.Doi;
                                ext.ArticleId = a.IdArticle;

                                _context.ExternalId.Add(ext);
                                _context.SaveChanges();

                            }
                            if (obj.Isbn != null)
                            {
                                ExternalId ext = new ExternalId();
                                ext.Type = "Isbn";
                                ext.Value = obj.Isbn;
                                ext.ArticleId = a.IdArticle;

                                _context.ExternalId.Add(ext);
                                _context.SaveChanges();
                            }
                            if (obj.Issn != null)
                            {
                                ExternalId ext = new ExternalId();
                                ext.Type = "Issn";
                                ext.Value = obj.Issn;
                                ext.ArticleId = a.IdArticle;

                                _context.ExternalId.Add(ext);
                                _context.SaveChanges();
                            }

                            if (obj.IndexTerms != null)
                            {
                                if (obj.IndexTerms.IeeeTerms != null)
                                {
                                    foreach (var ieeterms in obj.IndexTerms.IeeeTerms.TermsTerms)
                                    {
                                        Keyword kw = _context.Keyword.FirstOrDefault(x => x.Value == ieeterms);
                                        if (kw == null)
                                        {
                                            kw = new Keyword();
                                            kw.Value = ieeterms;

                                            _context.Keyword.Add(kw);
                                            _context.SaveChanges();

                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kw.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kw.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                    }
                                }

                                if (obj.IndexTerms.AuthorTerms != null)
                                {
                                    foreach (var authorterms in obj.IndexTerms.AuthorTerms.TermsTerms)
                                    {
                                        Keyword kwa = _context.Keyword.FirstOrDefault(x => x.Value == authorterms);
                                        if (kwa == null)
                                        {
                                            kwa = new Keyword();
                                            kwa.Value = authorterms;

                                            _context.Keyword.Add(kwa);
                                            _context.SaveChanges();

                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kwa.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            Articlekeyword ak = new Articlekeyword();
                                            ak.ArticleId = a.IdArticle;
                                            ak.IdKeyword = kwa.Id;
                                            _context.Articlekeyword.Add(ak);
                                            _context.SaveChanges();
                                        }
                                    }
                                }
                            }


                            Tableieee table = new Tableieee();
                            table.CitingPaperCount = Convert.ToInt32(obj.CitingPaperCount);
                            table.CitingPatentCount = Convert.ToInt32(obj.CitingPatentCount);
                            table.ArticleId = a.IdArticle;

                            _context.Tableieee.Add(table);
                            _context.SaveChanges();


                            foreach (var obj1 in obj.Authors.AuthorsAuthors)
                            {
                                Author au = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                                if (au == null)
                                {
                                    au = new Author();
                                    au.Name = obj1.FullName;
                                    if (obj1.AuthorUrl != null)
                                    {
                                        au.AuthorUrl = obj1.AuthorUrl.ToString();
                                    }

                                    _context.Author.Add(au);
                                    _context.SaveChanges();

                                    if (obj1 != null)
                                    {
                                        Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                        if (af == null)
                                        {
                                            af = new Affiliation();
                                            af.InstitutionName = obj1.Affiliation;

                                            _context.Affiliation.Add(af);
                                            _context.SaveChanges();

                                            Authoraffiliation aa = new Authoraffiliation();
                                            aa.IdAffiliation = af.Id;
                                            aa.IdAuthor = au.Id;

                                            _context.Authoraffiliation.Add(aa);
                                            _context.SaveChanges();
                                        }
                                        else
                                        {
                                            Authoraffiliation aa = new Authoraffiliation();
                                            aa.IdAffiliation = af.Id;
                                            aa.IdAuthor = au.Id;

                                            _context.Authoraffiliation.Add(aa);
                                            _context.SaveChanges();
                                        }
                                    }



                                    Authorarticles at = new Authorarticles();
                                    at.AuthorId = au.Id;
                                    at.ArticleId = a.IdArticle;

                                    _context.Authorarticles.Add(at);
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    Authorarticles at = new Authorarticles();
                                    at.AuthorId = au.Id;
                                    at.ArticleId = a.IdArticle;

                                    _context.Authorarticles.Add(at);
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                    //se o artigo estiver na base de dados associa os autores aos artigos
                    else
                    {
                        foreach (var obj1 in obj.Authors.AuthorsAuthors)
                        {
                            //se o autor nao estiver na base de dados, cria um e associa o ao artigo
                            Author au = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                            if (au == null)
                            {
                                au = new Author();
                                au.Name = obj1.FullName;
                                if (obj1.AuthorUrl != null)
                                {
                                    au.AuthorUrl = obj1.AuthorUrl.ToString();
                                }

                                _context.Author.Add(au);
                                _context.SaveChanges();
                                if (obj1 != null)
                                {
                                    Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                    if (af == null)
                                    {
                                        af = new Affiliation();
                                        af.InstitutionName = obj1.Affiliation;

                                        _context.Affiliation.Add(af);
                                        _context.SaveChanges();

                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                }


                                Authorarticles at = new Authorarticles();
                                at.AuthorId = au.Id;
                                at.ArticleId = a.IdArticle;

                                _context.Authorarticles.Add(at);
                                _context.SaveChanges();

                            }
                            //se o autor estiver na base de dados associa o ao artigo
                            else
                            {

                                Authorarticles at = new Authorarticles();
                                at.AuthorId = au.Id;
                                at.ArticleId = a.IdArticle;

                                _context.Authorarticles.Add(at);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                return result;
            }
            else
            {
                return NotFound();
            }
        }

        public void SaveWorks(IEEERead read)
        {
            foreach (var obj in read.Articles)
            {
                Article a = _context.Article.FirstOrDefault(x => x.Title == obj.Title);
                if (a == null)
                {
                    ExternalId ex = _context.ExternalId.FirstOrDefault(x => x.Value == obj.Doi);
                    if (ex == null)
                    {
                        a = new Article();
                        a.Title = obj.Title;
                        a.Year = Convert.ToInt32(obj.PublicationYear);
                        a.StartPage = obj.StartPage.ToString();
                        a.EndPage = obj.EndPage;
                        a.Abstract = obj.Abstract;
                        a.PutCode = Convert.ToInt32(obj.PublicationNumber);
                        _context.Article.Add(a);
                        _context.SaveChanges();

                        string tipo = obj.ContentType.ToString();
                        if (tipo.Contains("conference") == true)
                        {
                            Conference co = new Conference();
                            co.ConferenceDates = obj.ConferenceDates;
                            co.ConferenceLocation = obj.ConferenceLocation;
                            co.ConferenceName = obj.PublicationTitle;
                            co.IdConference = a.IdArticle;

                            _context.Conference.Add(co);
                            _context.SaveChanges();
                        }

                        if (tipo.Contains("journal") == true)
                        {
                            Journal jo = new Journal();
                            jo.JournalTitle = obj.PublicationTitle;
                            jo.IdJournal = a.IdArticle;

                            _context.Journal.Add(jo);
                            _context.SaveChanges();
                        }
                        if (tipo.Contains("book") == true)
                        {
                            Book bo = _context.Book.FirstOrDefault(x => x.BookName == obj.PublicationTitle);
                            if (bo == null)
                            {
                                bo = new Book();
                                bo.BookName = obj.PublicationTitle;
                                bo.IdBook = a.IdArticle;

                                _context.Book.Add(bo);
                                _context.SaveChanges();
                            }
                        }

                        if (obj.Doi != null)
                        {
                            ExternalId ext = new ExternalId();
                            ext.Type = "Doi";
                            ext.Value = obj.Doi;
                            ext.ArticleId = a.IdArticle;

                            _context.ExternalId.Add(ext);
                            _context.SaveChanges();

                        }
                        if (obj.Isbn != null)
                        {
                            ExternalId ext = new ExternalId();
                            ext.Type = "Isbn";
                            ext.Value = obj.Isbn;
                            ext.ArticleId = a.IdArticle;

                            _context.ExternalId.Add(ext);
                            _context.SaveChanges();
                        }
                        if (obj.Issn != null)
                        {
                            ExternalId ext = new ExternalId();
                            ext.Type = "Issn";
                            ext.Value = obj.Issn;
                            ext.ArticleId = a.IdArticle;

                            _context.ExternalId.Add(ext);
                            _context.SaveChanges();
                        }

                        if (obj.IndexTerms != null)
                        {
                            if (obj.IndexTerms.IeeeTerms != null)
                            {
                                foreach (var ieeterms in obj.IndexTerms.IeeeTerms.TermsTerms)
                                {
                                    Keyword kw = _context.Keyword.FirstOrDefault(x => x.Value == ieeterms);
                                    if (kw == null)
                                    {
                                        kw = new Keyword();
                                        kw.Value = ieeterms;

                                        _context.Keyword.Add(kw);
                                        _context.SaveChanges();

                                        Articlekeyword ak = new Articlekeyword();
                                        ak.ArticleId = a.IdArticle;
                                        ak.IdKeyword = kw.Id;
                                        _context.Articlekeyword.Add(ak);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        Articlekeyword ak = new Articlekeyword();
                                        ak.ArticleId = a.IdArticle;
                                        ak.IdKeyword = kw.Id;
                                        _context.Articlekeyword.Add(ak);
                                        _context.SaveChanges();
                                    }
                                }
                            }

                            if (obj.IndexTerms.AuthorTerms != null)
                            {
                                foreach (var authorterms in obj.IndexTerms.AuthorTerms.TermsTerms)
                                {
                                    Keyword kwa = _context.Keyword.FirstOrDefault(x => x.Value == authorterms);
                                    if (kwa == null)
                                    {
                                        kwa = new Keyword();
                                        kwa.Value = authorterms;

                                        _context.Keyword.Add(kwa);
                                        _context.SaveChanges();

                                        Articlekeyword ak = new Articlekeyword();
                                        ak.ArticleId = a.IdArticle;
                                        ak.IdKeyword = kwa.Id;
                                        _context.Articlekeyword.Add(ak);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        Articlekeyword ak = new Articlekeyword();
                                        ak.ArticleId = a.IdArticle;
                                        ak.IdKeyword = kwa.Id;
                                        _context.Articlekeyword.Add(ak);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                        }


                        Tableieee table = new Tableieee();
                        table.CitingPaperCount = Convert.ToInt32(obj.CitingPaperCount);
                        table.CitingPatentCount = Convert.ToInt32(obj.CitingPatentCount);
                        table.ArticleId = a.IdArticle;

                        _context.Tableieee.Add(table);
                        _context.SaveChanges();

                        foreach (var obj1 in obj.Authors.AuthorsAuthors)
                        {
                            Author au = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                            if (au == null)
                            {
                                au = new Author();
                                au.Name = obj1.FullName;
                                if (obj1.AuthorUrl != null)
                                {
                                    au.AuthorUrl = obj1.AuthorUrl.ToString();
                                }

                                _context.Author.Add(au);
                                _context.SaveChanges();

                                if (obj1 != null)
                                {
                                    Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                    if (af == null)
                                    {
                                        af = new Affiliation();
                                        af.InstitutionName = obj1.Affiliation;

                                        _context.Affiliation.Add(af);
                                        _context.SaveChanges();

                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        Authoraffiliation aa = new Authoraffiliation();
                                        aa.IdAffiliation = af.Id;
                                        aa.IdAuthor = au.Id;

                                        _context.Authoraffiliation.Add(aa);
                                        _context.SaveChanges();
                                    }
                                }


                                Authorarticles at = new Authorarticles();
                                at.AuthorId = au.Id;
                                at.ArticleId = a.IdArticle;

                                _context.Authorarticles.Add(at);
                                _context.SaveChanges();
                            }
                            else
                            {
                                Authorarticles at = new Authorarticles();
                                at.AuthorId = au.Id;
                                at.ArticleId = a.IdArticle;

                                _context.Authorarticles.Add(at);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                //se o artigo estiver na base de dados associa os autores aos artigos
                else
                {
                    foreach (var obj1 in obj.Authors.AuthorsAuthors)
                    {
                        //se o autor nao estiver na base de dados, cria um e associa o ao artigo
                        Author aut = _context.Author.FirstOrDefault(x => x.Name == obj1.FullName);
                        if (aut == null)
                        {
                            Author au = new Author();
                            au.Name = obj1.FullName;
                            if (obj1.AuthorUrl != null)
                            {
                                au.AuthorUrl = obj1.AuthorUrl.ToString();
                            }

                            _context.Author.Add(au);
                            _context.SaveChanges();
                            if (obj1 != null)
                            {
                                Affiliation af = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.Affiliation);
                                if (af == null)
                                {
                                    af = new Affiliation();
                                    af.InstitutionName = obj1.Affiliation;

                                    _context.Affiliation.Add(af);
                                    _context.SaveChanges();

                                    Authoraffiliation aa = new Authoraffiliation();
                                    aa.IdAffiliation = af.Id;
                                    aa.IdAuthor = au.Id;

                                    _context.Authoraffiliation.Add(aa);
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    Authoraffiliation aa = new Authoraffiliation();
                                    aa.IdAffiliation = af.Id;
                                    aa.IdAuthor = au.Id;

                                    _context.Authoraffiliation.Add(aa);
                                    _context.SaveChanges();
                                }
                            }


                            Authorarticles at = new Authorarticles();
                            at.AuthorId = au.Id;
                            at.ArticleId = a.IdArticle;

                            _context.Authorarticles.Add(at);
                            _context.SaveChanges();

                        }
                        //se o autor estiver na base de dados associa o ao artigo
                        else
                        {
                            Authorarticles at = new Authorarticles();
                            at.AuthorId = aut.Id;
                            at.ArticleId = a.IdArticle;

                            _context.Authorarticles.Add(at);
                            _context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
