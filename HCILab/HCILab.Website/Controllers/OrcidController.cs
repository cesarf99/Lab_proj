using HCILab.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orcid_Work;
using QuickType;
using QuickType_Educations;
using QuickType_Employments;
using QuickType_PD;
using SearchOrcid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article = HCILab.Website.Models.Article;
using Author = HCILab.Website.Models.Author;
using ExternalId = HCILab.Website.Models.ExternalId;


namespace HCILab.Website.Controllers
{
    public class OrcidController : Controller
    {
        private readonly ILogger<OrcidController> _logger;
        private readonly Orcid _orcidClient;
        private readonly hcilabContext _context;
        readonly string NoName = "No name";
        private int CreatedID;
        private string name;
        private int CreatedSingleID;

        public OrcidController(ILogger<OrcidController> logger, Orcid orcidClient, hcilabContext context)
        {
            _logger = logger;
            _orcidClient = orcidClient;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task Authenticateasync()
        {
            await _orcidClient.AuthenticateAsync();
        }

        [HttpGet]
        public async Task<object> Search(SearchViewModel search)
        {
            var result = await _orcidClient.Search(search.Text);

            string result1 = result.ToString();
            var src = SearchOr.FromJson(result1);

            var orcidids = new List<string>();

            foreach (var obj in src.Result)
            {
                orcidids.Add(obj.OrcidIdentifier.Path.ToString());
            }

            return result;
        }

        [HttpGet]
        public async Task<object> GetWorks(SearchViewModel search)
        {
            var result = await _orcidClient.WorksAsync(search.Text);
            return result;
        }

        [HttpGet]
        public async Task<object> GetPersonDetails(SearchViewModel search)
        {
            var result = await _orcidClient.PersonDetailsAsync(search.Text);
            return result;
        }

        [HttpGet]
        public async Task<object> GetAffiliation(SearchViewModel search)
        {
            var result = await _orcidClient.EducationsAsync(search.Text);
            return result;
        }

        [HttpGet]
        public async Task<object> GetWorkDetails(SearchViewModel search, SearchViewModel putcode)
        {
            var result = await _orcidClient.WorkDetailsAsync(search.Text, putcode.Text);
            return result;
        }

        [HttpGet]
        public async Task<string> GetNameAsync(SearchViewModel search)
        {
            var result = await _orcidClient.PersonDetailsAsync(search.Text);
            string result1 = result.ToString();
            var personalDetails = OrcidPersonalDetails.FromJson(result1);

            if (personalDetails.Name.CreditName != null)
            {
                name = personalDetails.Name.CreditName.Value;
                return name;
            }
            else
            {
                string primeiro = personalDetails.Name.GivenNames.Value;
                string segundo = personalDetails.Name.FamilyName.Value;
                name = string.Join(" ", primeiro, segundo);
                return name;
            }
        }


        [HttpGet]
        public async Task<object> PostWorks(SearchViewModel search)
        {
            var result = await _orcidClient.WorksAsync(search.Text);
            string result1 = result.ToString();
            var readwork = OrcidWorks.FromJson(result1);

            var summaries = readwork.Group.SelectMany(x => x.WorkSummary).Where(x => x.ExternalIds != null);

            Author author = _context.Author.FirstOrDefault(x => x.IdOrcid == search.Text);
            if (author == null)
            {
                author = new Author();
                author.Name = GetNameAsync(search).Result;
                author.IdOrcid = search.Text;

                _context.Author.Add(author);
                _context.SaveChanges();

                foreach (var c in summaries)
                {
                    foreach (var ExIDs in c.ExternalIds.ExternalId.Where(p => p.ExternalIdValue != null))
                    {
                        ExternalId externalId = _context.ExternalId.FirstOrDefault(x => x.Value == ExIDs.ExternalIdValue);

                        Article article = _context.Article.FirstOrDefault(x => x.Title == c.Title.TitleTitle.Value);

                        if (article == null && externalId == null)
                        {
                            CreateArticle(c);

                            foreach (var i in c.ExternalIds.ExternalId)
                            {
                                ExternalId ex = _context.ExternalId.FirstOrDefault(x => x.Value == i.ExternalIdValue);
                                if (ex == null)
                                {
                                    externalId = new ExternalId();
                                    externalId.Type = i.ExternalIdType.ToString();
                                    externalId.Value = i.ExternalIdValue;

                                    externalId.ArticleId = CreatedID;

                                    _context.ExternalId.Add(externalId);
                                    _context.SaveChanges();
                                }

                            }
                            Authorarticles au = new Authorarticles();
                            au.ArticleId = CreatedID;
                            au.AuthorId = author.Id;
                            _context.Authorarticles.Add(au);
                            _context.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                foreach (var c in summaries)
                {
                    foreach (var ExIDs in c.ExternalIds.ExternalId.Where(x => x.ExternalIdValue != null))
                    {
                        ExternalId externalId = _context.ExternalId.FirstOrDefault(x => x.Value == ExIDs.ExternalIdValue);
                        Article article = _context.Article.FirstOrDefault(x => x.Title == c.Title.TitleTitle.Value);
                        if (article == null && externalId == null)
                        {

                            CreateArticle(c);

                            foreach (var i in c.ExternalIds.ExternalId)
                            {
                                ExternalId ex = _context.ExternalId.FirstOrDefault(x => x.Value == i.ExternalIdValue);
                                if (ex == null)
                                {
                                    externalId = new ExternalId();
                                    externalId.Type = i.ExternalIdType.ToString();
                                    externalId.Value = i.ExternalIdValue;

                                    externalId.ArticleId = CreatedID;

                                    _context.ExternalId.Add(externalId);
                                    _context.SaveChanges();
                                }

                            }

                            Authorarticles au = new Authorarticles();
                            au.ArticleId = CreatedID;
                            au.AuthorId = author.Id;
                            _context.Authorarticles.Add(au);
                            _context.SaveChanges();

                        }
                    }
                }
            }

            return result;
        }

        [HttpGet]
        public async Task<object> PostPersonDetails(SearchViewModel search)
        {
            var result = await _orcidClient.PersonDetailsAsync(search.Text);
            string result1 = result.ToString();
            var personalDetails = OrcidPersonalDetails.FromJson(result1);

            if (personalDetails.Name.CreditName != null)
            {
                Author au = _context.Author.FirstOrDefault(x => x.Name == personalDetails.Name.CreditName.Value);
                ModelState.AddModelError("Name", "Já existe");
            }
            if (personalDetails.Name.GivenNames != null && personalDetails.Name.FamilyName != null)
            {
                string primeiro = personalDetails.Name.GivenNames.Value;
                string segundo = personalDetails.Name.FamilyName.Value;
                Author au = _context.Author.FirstOrDefault(x => x.Name == string.Join(" ", primeiro, segundo));
                ModelState.AddModelError("Name", "Já existe");
            }
            if (ModelState.IsValid)
            {
                Author au = new Author();
                if (personalDetails.Name.CreditName != null)
                {
                    au.Name = personalDetails.Name.CreditName.Value;

                }
                if (personalDetails.Name.GivenNames != null && personalDetails.Name.FamilyName != null)
                {
                    string primeiro = personalDetails.Name.GivenNames.Value;
                    string segundo = personalDetails.Name.FamilyName.Value;
                    au.Name = string.Join(" ", primeiro, segundo);
                    ModelState.AddModelError("Name", "Já existe");
                }
                _context.Author.Add(au);
                _context.SaveChanges();
            }

            return result;
        }

        [HttpGet]
        public async Task<object> PostAffiliation(SearchViewModel search)
        {
            var result = await _orcidClient.EducationsAsync(search.Text);
            string result1 = result.ToString();
            var educations = OrcidEducation.FromJson(result1);

            Author au = _context.Author.FirstOrDefault(x => x.IdOrcid == search.Text);

            if (au == null)
            {
                au = new Author();
                au.IdOrcid = search.Text;
                var name = await _orcidClient.PersonDetailsAsync(search.Text);
                string name1 = name.ToString();
                var personalDetails = OrcidPersonalDetails.FromJson(name1);

                string primeiro = personalDetails.Name.GivenNames.Value;
                string segundo = personalDetails.Name.FamilyName.Value;
                au.Name = string.Join(" ", primeiro, segundo);


                _context.Author.Add(au);
                _context.SaveChanges();
                foreach (var obj in educations.AffiliationGroup)
                {
                    if (obj.Summaries != null)
                    {
                        foreach (var obj1 in obj.Summaries)
                        {
                            Affiliation aff = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.EducationSummary.Organization.Name);

                            if (aff == null)
                            {
                                Affiliation af = new Affiliation();
                                af.InstitutionName = obj1.EducationSummary.Organization.Name;
                                af.InstitutionPosition = obj1.EducationSummary.RoleTitle;

                                _context.Affiliation.Add(af);
                                _context.SaveChanges();

                                Authoraffiliation auff = new Authoraffiliation();
                                auff.IdAuthor = au.Id;
                                auff.IdAffiliation = af.Id;

                                _context.Authoraffiliation.Add(auff);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                var result2 = await _orcidClient.EmploymentsAsync(search.Text);
                string result3 = result2.ToString();
                var employments = OrcidEmployments.FromJson(result3);

                foreach (var obj2 in employments.AffiliationGroup)
                {
                    if (obj2.Summaries != null)
                    {
                        foreach (var obj3 in obj2.Summaries)
                        {
                            Affiliation aff = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj3.EmploymentSummary.Organization.Name);

                            if (aff == null)
                            {
                                Affiliation af = new Affiliation();
                                af.InstitutionName = obj3.EmploymentSummary.Organization.Name;
                                af.InstitutionPosition = obj3.EmploymentSummary.RoleTitle;

                                _context.Affiliation.Add(af);
                                _context.SaveChanges();

                                Authoraffiliation auff = new Authoraffiliation();
                                auff.IdAuthor = au.Id;
                                auff.IdAffiliation = af.Id;

                                _context.Authoraffiliation.Add(auff);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
            }

            else
            {
                foreach (var obj in educations.AffiliationGroup)
                {
                    if (obj.Summaries != null)
                    {
                        foreach (var obj1 in obj.Summaries)
                        {
                            Affiliation aff = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj1.EducationSummary.Organization.Name);

                            if (aff == null)
                            {
                                Affiliation af = new Affiliation();
                                af.InstitutionName = obj1.EducationSummary.Organization.Name;
                                af.InstitutionPosition = obj1.EducationSummary.RoleTitle;

                                _context.Affiliation.Add(af);
                                _context.SaveChanges();

                                Authoraffiliation auff = new Authoraffiliation();
                                auff.IdAuthor = au.Id;
                                auff.IdAffiliation = af.Id;

                                _context.Authoraffiliation.Add(auff);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                var result2 = await _orcidClient.EmploymentsAsync(search.Text);
                string result3 = result2.ToString();
                var employments = OrcidEmployments.FromJson(result3);

                foreach (var obj2 in employments.AffiliationGroup)
                {
                    if (obj2.Summaries != null)
                    {
                        foreach (var obj3 in obj2.Summaries)
                        {
                            Affiliation aff = _context.Affiliation.FirstOrDefault(x => x.InstitutionName == obj3.EmploymentSummary.Organization.Name);

                            if (aff == null)
                            {
                                Affiliation af = new Affiliation();
                                af.InstitutionName = obj3.EmploymentSummary.Organization.Name;
                                af.InstitutionPosition = obj3.EmploymentSummary.RoleTitle;

                                _context.Affiliation.Add(af);
                                _context.SaveChanges();

                                Authoraffiliation auff = new Authoraffiliation();
                                auff.IdAuthor = au.Id;
                                auff.IdAffiliation = af.Id;

                                _context.Authoraffiliation.Add(auff);
                                _context.SaveChanges();
                            }
                        }
                    }
                }

            }

            return result;
        }

        [HttpGet]
        public async Task<object> PostWorkDetails(SearchViewModel search, SearchViewModel putcode)
        {
            var result = await _orcidClient.WorkDetailsAsync(search.Text, putcode.Text);
            string result1 = result.ToString();
            var onework = OrcidWork.FromJson(result1);

            Author au = _context.Author.FirstOrDefault(x => x.IdOrcid == search.Text);

            if (au == null)
            {
                au = new Author();
                au.IdOrcid = search.Text;
                var result2 = await _orcidClient.PersonDetailsAsync(search.Text);
                string result3 = result2.ToString();
                var personalDetails = OrcidPersonalDetails.FromJson(result3);

                string primeiro = personalDetails.Name.GivenNames.Value;
                string segundo = personalDetails.Name.FamilyName.Value;
                au.Name = string.Join(" ", primeiro, segundo);

                _context.Author.Add(au);
                _context.SaveChanges();

                Article a = _context.Article.FirstOrDefault(x => x.PutCode == Convert.ToInt32(putcode.Text));
                if (a == null)
                {
                    CreateSingleArticle(onework);

                    Authorarticles aa = new Authorarticles();
                    aa.AuthorId = au.Id;
                    aa.ArticleId = CreatedSingleID;
                    _context.Authorarticles.Add(aa);
                    _context.SaveChanges();
                }
                else
                {
                    a.Abstract = onework.ShortDescription;
                    _context.Article.Update(a);
                    _context.SaveChanges();

                    Authorarticles aa = new Authorarticles();
                    aa.AuthorId = au.Id;
                    aa.ArticleId = a.IdArticle;
                    _context.Authorarticles.Add(aa);
                    _context.SaveChanges();
                }
            }
            else
            {
                Article a = _context.Article.FirstOrDefault(x => x.PutCode == Convert.ToInt32(putcode.Text));
                if (a == null)
                {
                    CreateSingleArticle(onework);

                    Authorarticles aa = new Authorarticles();
                    aa.AuthorId = au.Id;
                    aa.ArticleId = CreatedSingleID;
                    _context.Authorarticles.Add(aa);
                    _context.SaveChanges();
                }
            }
            return result;
        }




        public int CreateSingleArticle(OrcidWork z)
        {
            Article a = new Article();
            a.Title = z.Title.TitleTitle.Value;
            a.Abstract = z.ShortDescription;
            a.PutCode = Convert.ToInt32(z.PutCode);
            a.Year = Convert.ToInt32(z.PublicationDate.Year.Value);

            string tipo = z.Type.ToString();

            foreach (var h in z.ExternalIds.ExternalId.Where(x => x.ExternalIdValue != null))
            {
                ExternalId externalId = _context.ExternalId.FirstOrDefault(x => x.Value == h.ExternalIdValue);
                if (h == null)
                {
                    externalId = new ExternalId();
                    externalId.Type = h.ExternalIdType.ToString();
                    externalId.Value = h.ExternalIdValue;

                    externalId.ArticleId = CreatedID;

                    _context.ExternalId.Add(externalId);
                    _context.SaveChanges();
                }
            }

            if (tipo.Contains("conference") == true)
            {
                if (z.JournalTitle != null)
                {
                    Conference co = _context.Conference.FirstOrDefault(x => x.ConferenceName == z.JournalTitle.Value);
                    if (co == null)
                    {
                        co = new Conference();
                        co.ConferenceName = z.JournalTitle.Value;
                        co.IdConference = a.IdArticle;

                        _context.Conference.Add(co);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    Conference co = new Conference();
                    co.ConferenceName = NoName;
                    co.IdConference = a.IdArticle;

                    _context.Conference.Add(co);
                    _context.SaveChanges();
                }
            }
            if (tipo.Contains("journal") == true)
            {
                if (z.JournalTitle != null)
                {
                    Journal jo = _context.Journal.FirstOrDefault(x => x.JournalTitle == z.JournalTitle.Value);
                    if (jo == null)
                    {
                        jo = new Journal();
                        jo.JournalTitle = z.JournalTitle.Value;
                        jo.IdJournal = a.IdArticle;

                        _context.Journal.Add(jo);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    Journal jo = new Journal();
                    jo.JournalTitle = NoName;
                    jo.IdJournal = a.IdArticle;

                    _context.Journal.Add(jo);
                    _context.SaveChanges();
                }
            }
            if (tipo.Contains("book") == true)
            {
                if (z.JournalTitle != null)
                {
                    Book bo = _context.Book.FirstOrDefault(x => x.BookName == z.JournalTitle.Value);
                    if (bo == null)
                    {
                        bo = new Book();
                        bo.BookName = z.JournalTitle.Value;
                        bo.IdBook = a.IdArticle;

                        _context.Book.Add(bo);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    Book bo = new Book();
                    bo.BookName = NoName;
                    bo.IdBook = a.IdArticle;

                    _context.Book.Add(bo);
                    _context.SaveChanges();
                }
            }
            return CreatedSingleID;
        }

        public void CreateNewAuthor(SearchViewModel search)
        {
            Author author = new Author();
            author.Name = GetNameAsync(search).Result;
        }

        public int CreateArticle(WorkSummary c)
        {
            Article article = new Article();
            article.PutCode = Convert.ToInt32(c.PutCode);
            article.Title = c.Title.TitleTitle.Value;
            if (c.PublicationDate != null)
            {
                article.Year = Convert.ToInt32(c.PublicationDate.Year.Value);
            }
            _context.Article.Add(article);
            _context.SaveChanges();
            CreatedID = article.IdArticle;

            string tipo = c.Type.ToString();

            if (tipo.Contains("conference") == true)
            {
                if (c.JournalTitle != null)
                {
                    Conference co = new Conference();
                    co.ConferenceName = c.JournalTitle.Value;
                    co.IdConference = article.IdArticle;

                    _context.Conference.Add(co);
                    _context.SaveChanges();
                }
                else
                {
                    Conference co = new Conference();
                    co.ConferenceName = NoName;
                    co.IdConference = article.IdArticle;

                    _context.Conference.Add(co);
                    _context.SaveChanges();
                }
            }
            if (tipo.Contains("journal") == true)
            {
                if (c.JournalTitle != null)
                {
                    Journal jo = new Journal();
                    jo.JournalTitle = c.JournalTitle.Value;
                    jo.IdJournal = article.IdArticle;

                    _context.Journal.Add(jo);
                    _context.SaveChanges();

                }
                else
                {
                    Journal jo = new Journal();
                    jo.JournalTitle = NoName;
                    jo.IdJournal = article.IdArticle;

                    _context.Journal.Add(jo);
                    _context.SaveChanges();
                }
            }
            if (tipo.Contains("book") == true)
            {
                if (c.JournalTitle != null)
                {
                    Book bo = new Book();
                    bo.BookName = c.JournalTitle.Value;
                    bo.IdBook = article.IdArticle;

                    _context.Book.Add(bo);
                    _context.SaveChanges();

                }
                else
                {
                    Book bo = new Book();
                    bo.BookName = NoName;
                    bo.IdBook = article.IdArticle;

                    _context.Book.Add(bo);
                    _context.SaveChanges();
                }
            }
            if (c.Source.SourceName != null)
            {
                if (c.Source.SourceOrcid != null)
                {
                    Tableorcid to = _context.Tableorcid.FirstOrDefault(x => x.Path == c.Source.SourceOrcid.Path);
                    if (to == null)
                    {
                        to = new Tableorcid();
                        to.Path = c.Source.SourceOrcid.Path;
                        to.SourceName = c.Source.SourceName.Value;

                        to.ArticleId = article.IdArticle;

                        _context.Tableorcid.Add(to);
                        _context.SaveChanges();
                    }
                }
                if (c.Source.SourceClientId != null)
                {
                    Tableorcid to = _context.Tableorcid.FirstOrDefault(x => x.Path == c.Source.SourceClientId.Path);
                    if (to == null)
                    {
                        to = new Tableorcid();
                        to.Path = c.Source.SourceClientId.Path;
                        to.SourceName = c.Source.SourceName.Value;

                        to.ArticleId = article.IdArticle;

                        _context.Tableorcid.Add(to);
                        _context.SaveChanges();
                    }
                }
            }
            if (c.Source.AssertionOriginName != null)
            {
                if (c.Source.AssertionOriginOrcid != null)
                {
                    Tableorcid to = _context.Tableorcid.FirstOrDefault(x => x.Path == c.Source.AssertionOriginOrcid.Path);
                    if (to == null)
                    {
                        to = new Tableorcid();
                        to.Path = c.Source.AssertionOriginOrcid.Path;
                        to.SourceName = c.Source.AssertionOriginName.Value;

                        to.ArticleId = article.IdArticle;

                        _context.Tableorcid.Add(to);
                        _context.SaveChanges();
                    }
                }
            }
            return CreatedID;
        }

    }
}