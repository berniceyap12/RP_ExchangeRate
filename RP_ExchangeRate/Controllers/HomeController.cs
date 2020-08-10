using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FYPExchangeRateModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FYP_ExchangeRate.Models;
using RestSharp;
using Microsoft.AspNetCore.Http;

namespace FYP_ExchangeRate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public FYPExchangeRateContext dbContext;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            dbContext = new FYPExchangeRateContext();
        }

        public IActionResult Index()
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
