using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCILab.Projects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuickType_PD;
using QuickTypeReadFundings;

namespace HCILab.Projects.Controllers
{
    public class OrcidController : Controller
    {
        private readonly ILogger<OrcidController> _logger;
        private readonly Orcid _orcidClient;
        private readonly hcilabprojectContext _context;

        public OrcidController(ILogger<OrcidController> logger, Orcid orcidClient, hcilabprojectContext context)
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
        public async Task<object> GetProjects(SearchViewModel search)
        {
            var result = await _orcidClient.FundingsAsync(search.Text);
            return result;
        }

        [HttpGet]
        public async Task<object> PutProjects(SearchViewModel search)
        {
            var result = await _orcidClient.FundingsAsync(search.Text);
            string result1 = result.ToString();
            var fundings = Fundings.FromJson(result1);

            var r = fundings.Group.SelectMany(x => x.FundingSummary);

            foreach (var s in r)
            {
                foreach (var e in s.ExternalIds.ExternalId)
                {
                    Project pro2 = _context.Project.FirstOrDefault(x => x.Putcode == s.PutCode.ToString());
                    Project pro = _context.Project.FirstOrDefault(x => x.GrantNumber == e.ExternalIdValue);

                    if (e.ExternalIdType == "grant_number")
                    {
                        if (pro == null && pro2 == null)
                        {
                            Project projeto = new Project();
                            projeto.Title = s.Title.TitleTitle.Value;
                            projeto.Putcode = s.PutCode.ToString();
                            projeto.Tipo = s.Type.ToString();
                            projeto.Financer = s.Organization.Name;
                            projeto.Country = s.Organization.Address.Country.ToString();
                            projeto.GrantNumber = e.ExternalIdValue;

                            if (s.Url != null)
                            {
                                projeto.Url = s.Url.Value;
                            }

                            _context.Project.Add(projeto);
                            _context.SaveChanges();

                            Connectionresearcher cr = new Connectionresearcher();
                            cr.OrcidId = search.Text;
                            cr.ProjectId = projeto.IdProject;

                            _context.Connectionresearcher.Add(cr);
                            _context.SaveChanges();
                        }
                    }                    
                } 
            }

            return result;
        }
    }
}