using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HCILab.Website.Models;
using HCILab.Website.Controllers;
using HCILab.Website.APIRequests;

namespace HCILab.Website.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly hcilabContext _context;
        private readonly OrcidController _orcid;

        public AuthorController(hcilabContext context, OrcidController orcid)
        {
            _context = context;
            _orcid = orcid;
        }

        // GET: api/Author
        [HttpGet]
        public object GetAuthor(string id, [FromQuery] QueryClass query)
        {
            if (query?.AuthorName != null)
            {
                var author = _context.Author.Where(x => x.Name.Contains(query.AuthorName));

                if (author == null)
                {
                    return NotFound();
                }

                return author;
            }
            else if (query?.OrcidId != null)
            {
                var author = _context.Author.Where(x => x.IdOrcid == query.OrcidId);

                if (author == null)
                {
                    return NotFound();
                }

                return author;
            }
            else
            {
                return _context.Author.ToList();
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutAuthor(int id, [FromBody] AuxAuthor aux)
        {
            if (aux == null)
            {
                return BadRequest();
            }

            Author author = _context.Author.FirstOrDefault(x => x.Id == id);

            if (author == null)
            {
                return BadRequest();
            }

            if (aux.IdOrcid != null)
            {
                author.IdOrcid = aux.IdOrcid;
                _context.Author.Update(author);
                _context.SaveChanges();
            }
            if (aux.Name != null)
            {
                author.Name = aux.Name;
                _context.Author.Update(author);
                _context.SaveChanges();
            }
            if (aux.Email != null)
            {
                author.Email = aux.Email;
                _context.Author.Update(author);
                _context.SaveChanges();
            }
            if (aux.Country != null)
            {
                author.Country = aux.Country;
                _context.Author.Update(author);
                _context.SaveChanges();
            }
            if (aux.AuthorUrl != null)
            {
                author.AuthorUrl = aux.AuthorUrl;
                _context.Author.Update(author);
                _context.SaveChanges();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<object> PostAuthor([FromBody] string id)
        {
            SearchViewModel s = new SearchViewModel();
            s.Text = id;

            var newauthor = await _orcid.PostPersonDetails(s);

            await _orcid.PostAffiliation(s);

            if (newauthor == null)
            {
                return NotFound();
            }
            else
            {
                return newauthor;
            }
        }
    }
}
