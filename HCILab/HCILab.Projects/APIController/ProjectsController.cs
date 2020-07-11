using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HCILab.Projects.Models;
using HCILab.Projects.Controllers;

namespace HCILab.Projects.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly hcilabprojectContext _context;
        private readonly OrcidController _proj;

        public ProjectsController(hcilabprojectContext context, OrcidController proj)
        {
            _context = context;
            _proj = proj;
        }

        // GET: api/Projects
        [HttpGet]
        public object GetProject([FromQuery] Query query)
        {
            return _context.Project.Where(x =>
            (string.IsNullOrEmpty(query.Financer) || x.Financer.Contains(query.Financer))
            && (string.IsNullOrEmpty(query.GrantNumber) || x.GrantNumber.Contains(query.GrantNumber))
             && (string.IsNullOrEmpty(query.Title) || x.Title.Contains(query.Title))
        );
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Project.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id,[FromBody] AuxProject project)
        {
            var t = _context.Project.FirstOrDefault(x => x.IdProject == id);

            if (t!=null)
            {
                return BadRequest();
            }

            if (project.Country != null)
            {
                t.Country = project.Country;
                _context.Project.Update(t);
                _context.SaveChanges();
            }
            if (project.Description != null)
            {
                t.Description = project.Description;
                _context.Project.Update(t);
                _context.SaveChanges();
            }
            if (project.Email != null)
            {
                t.Email = project.Email;
                _context.Project.Update(t);
                _context.SaveChanges();
            }
            if (project.Financer != null)
            {
                t.Financer = project.Financer;
                _context.Project.Update(t);
                _context.SaveChanges();
            }
            if (project.Title != null)
            {
                t.Title = project.Title;
                _context.Project.Update(t);
                _context.SaveChanges();
            }
            if (project.Url != null)
            {
                t.Url = project.Url;
                _context.Project.Update(t);
                _context.SaveChanges();
            }
            if (project.Tipo != null)
            {
                t.Tipo = project.Tipo;
                _context.Project.Update(t);
                _context.SaveChanges();
            }
            if (project.Putcode != null)
            {
                t.Putcode = project.Putcode;
                _context.Project.Update(t);
                _context.SaveChanges();
            }
            if (project.GrantNumber != null)
            {
                t.GrantNumber = project.GrantNumber;
                _context.Project.Update(t);
                _context.SaveChanges();
            }
            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<object> PostProject([FromBody] string id)
        {
            SearchViewModel ss = new SearchViewModel();
            ss.Text = id;

            var result = await _proj.PutProjects(ss);

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return result;
            }
        }
    }
}
