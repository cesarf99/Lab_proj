using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HCILab.Researchers.Models;
using HCILab.Researchers.APIRequests;

namespace HCILab.Researchers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResearchersController : ControllerBase
    {
        private readonly hcilabresearcherContext _context;

        public ResearchersController(hcilabresearcherContext context)
        {
            _context = context;
        }

        // GET: api/Researchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Researcher>>> GetResearcher(string Name, string id)
        {
            if (Name != null || id != null)
            {
                return await _context.Researcher.Where(x => x.Name == Name || x.OrcidId == id).ToListAsync();
            }
            else
            {
                return await _context.Researcher.ToListAsync();
            }
        }


        // PUT: api/Researchers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutResearcher(int id, [FromBody] AuxResearcher request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var a = _context.Researcher.FirstOrDefault(x => x.IdResearcher == id);

            if (a != null)
            {
                if (request.Name != null)
                {
                    a.Name = request.Name;
                    _context.Researcher.Add(a);
                    _context.SaveChanges();

                    return Ok();
                }
                if (request.OrcidId != null)
                {
                    a.OrcidId = request.OrcidId;
                    _context.Researcher.Add(a);
                    _context.SaveChanges();

                    return Ok();
                }
                if (request.ScopusId != null)
                {
                    a.ScopusId = request.ScopusId;
                    _context.Researcher.Add(a);
                    _context.SaveChanges();

                    return Ok();
                }
            }
            else
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Researchers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public object PostResearcher([FromBody] string id, [FromBody] string name)
        {
            if (id != null && name != null)
            {
                var a = _context.Researcher.FirstOrDefault(x => x.OrcidId == id);

                if (a == null)
                {
                    Researcher researcher = new Researcher();
                    researcher.OrcidId = id;
                    researcher.Name = name;

                    _context.Researcher.Add(researcher);
                    _context.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                return BadRequest();
            }
        }
    }
}
