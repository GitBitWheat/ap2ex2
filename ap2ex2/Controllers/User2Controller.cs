using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ap2ex2.Models;
using ap2ex2.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ap2ex2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class User2Controller : Controller
    {
        private IUserService service;

        public User2Controller()
        {
            service = new UserService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Json((service.GetAllUsers()));
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            User? user = service.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return Json(user);
        }
    }
}

