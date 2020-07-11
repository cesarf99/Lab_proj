using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HCILab.Projects.Models;

namespace HCILab.Projects.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Orcid _orcidClient;

        public HomeController(ILogger<HomeController> logger, Orcid orcidClient)
        {
            _logger = logger;
            _orcidClient = orcidClient;
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

        public IActionResult InserirProjeto()
        {
            return View();
        }
    }
}
