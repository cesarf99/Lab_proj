using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HCILab.Website.Models;

namespace HCILab.Website.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AffiliationsController : ControllerBase
    {
        private readonly hcilabContext _context;

        public AffiliationsController(hcilabContext context)
        {
            _context = context;
        }

        // GET: api/Affiliations
        [HttpGet]
        public object GetAffiliation(string Name, string Country)
        {
            return _context.Affiliation.Where(x => (string.IsNullOrEmpty(Name) || x.InstitutionName.Contains(Name))
                        && (string.IsNullOrEmpty(Country) || x.Country.Contains(Country)));
        }
    }
}
