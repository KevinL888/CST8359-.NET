using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab3.Core.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab3.Controllers
{
    public class Home : Controller
    {
        // GET: /<controller>/

        [Route("[action]"), Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Razor()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreatePerson()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DisplayPerson(Person person)
        {
            return View(person);
        }
    }
}
