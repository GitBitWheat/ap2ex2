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
    public class UserController : Controller
    {
        private IUserService service;

        public UserController()
        {
            service = new UserService();
        }

        public IActionResult Index()
        {
            return View(service.GetAllUsers());
        }

        public IActionResult Details(int id)
        {
            return View(service.GetUser(id));
        }
        
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string nickname, string password, string image)
        {
            service.Register(username, nickname, password, image);
            return Redirect(nameof(Login));
        }
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {

            if (service.Login(username, password)) 
            {
                return RedirectToAction(nameof(Index), "User");
            }
            else
            {
                ViewData["Error"] = "User or password is incorrect.";
            }
            return View();
        }
        
        

        public IActionResult Edit(int id)
        {
            return View(service.GetUser(id));
        }
        [HttpPost]
        public IActionResult Edit(int id, string username, string nickname, string password, string image)
        {
            service.EditUser(id, username, nickname, password, image);
            return RedirectToAction(nameof(Index));
        }
    }
}

