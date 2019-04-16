using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvcapp.Models;

namespace mvcapp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy(int? id)
        {
            if(id.HasValue)
                throw new Exception("privacy page exception");

            return View();
        }

        public IActionResult Ajax(int? id)
        {
            if(id.HasValue)
                throw new Exception("ajax exception");

            return Json(new {name="ajax"});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
