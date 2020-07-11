using HCILab.Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Author = HCILab.Website.Models.Author;

namespace HCILab.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Orcid _orcidClient;
        private readonly IEEE _ieeeClient;
        private readonly hcilabContext _context;
        
        public HomeController(ILogger<HomeController> logger, Orcid orcidClient, IEEE ieeeClient, hcilabContext context)
        {
            _logger = logger;
            _orcidClient = orcidClient;
            _ieeeClient = ieeeClient;
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            await _orcidClient.AuthenticateAsync();
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

    }
}
