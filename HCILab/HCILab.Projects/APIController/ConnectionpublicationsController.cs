using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HCILab.Projects.Models;

namespace HCILab.Projects.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionpublicationsController : ControllerBase
    {
        private readonly hcilabprojectContext _context;

        public ConnectionpublicationsController(hcilabprojectContext context)
        {
            _context = context;
        }

        // GET: api/Connectionpublications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Connectionpublication>>> GetConnectionpublication()
        {
            return await _context.Connectionpublication.ToListAsync();
        }

        // GET: api/Connectionpublications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Connectionpublication>> GetConnectionpublication(int id)
        {
            var connectionpublication = await _context.Connectionpublication.FindAsync(id);

            if (connectionpublication == null)
            {
                return NotFound();
            }

            return connectionpublication;
        }

        // PUT: api/Connectionpublications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutConnectionpublication(int id, AuxConPub aux)
        {
            var t = _context.Connectionpublication.FirstOrDefault(x => x.Id == id);

            if (t == null)
            {
                return BadRequest();
            }
            if (aux != null)
            {
                if (aux.Doi != null)
                {
                    t.Doi = aux.Doi;
                    _context.Connectionpublication.Update(t);
                    _context.SaveChanges();
                }
                if (aux.ProjectId != 0)
                {
                    t.ProjectId = aux.ProjectId;
                    _context.Connectionpublication.Update(t);
                    _context.SaveChanges();
                }
            }
            else
            {
                return BadRequest();
            }
            return NoContent();
        }

        // POST: api/Connectionpublications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Connectionpublication> PostConnectionpublication([FromBody] ConPubBody conPub)
        {
            if (conPub == null)
            {
                return BadRequest();
            }

            Connectionpublication cn = new Connectionpublication();
            cn.ProjectId = conPub.ProjectId;
            cn.Doi = conPub.Doi;

            _context.Connectionpublication.Add(cn);
            _context.SaveChanges();

            return cn;
        }
    }
}
