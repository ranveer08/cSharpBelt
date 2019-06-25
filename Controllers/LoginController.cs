using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using cBelt2.Models;

namespace cBelt2.Controllers
{
    public class LoginController : Controller
    {
        private int? UserSession
        {
            get {return HttpContext.Session.GetInt32("UserId");}
            set { HttpContext.Session.SetInt32("UserId", (int)value);}

        }
        private MyContext dbContext;
        public LoginController(MyContext Context)
        {
            dbContext = Context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.valErrors = ModelState.Values;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public IActionResult Register(RegisterViewModel NewUser)
        {
            User Validate = dbContext.Users.Where(user => user.FirstName == NewUser.FirstName).SingleOrDefault(); 
            if (ModelState.IsValid && Validate == null)
            {

                User ValidUser = new User()  {
                    FirstName = NewUser.FirstName,
                    LastName = NewUser.LastName,
                    Email = NewUser.Email,
                    Password = NewUser.Password
                };

                dbContext.Users.Add(ValidUser);
                dbContext.SaveChanges();
                Validate = dbContext.Users.Where(user => user.FirstName == ValidUser.FirstName).SingleOrDefault();
                HttpContext.Session.SetInt32("UserId", (int)Validate.UserId);
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                ViewBag.valErrors = ModelState.Values;
            if(dbContext.Users.Any(u => u.Email == NewUser.Email))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("Email", "Email already in use!");
                
                // You may consider returning to the View at this point
                return View("Index");
            }
                return View("Index");
            }
        }
    [HttpGet("login")]
    public IActionResult Login()
        {
            return View();
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("loginUser")]
        public IActionResult LoginUser(LoginViewModel User)
        {
            if (ModelState.IsValid)
            {
                User Validate = dbContext.Users.Where(user => user.Email == User.Email).SingleOrDefault(); 
                if (Validate != null){
                    if ((string)Validate.Password == User.Password){
                        HttpContext.Session.SetInt32("UserId", (int)Validate.UserId);
                        return RedirectToAction("Index", "Home");
                    }
                    else 
                    {
                        ModelState.AddModelError("Password", "Invalid Email/Password");   
                        ViewBag.valErrors = ModelState.Values;              
                        return View("Login");
                    }
                }
                else {
                    ModelState.AddModelError("Email", "Invalid Email/Password");   
                        ViewBag.valErrors = ModelState.Values;              
                        return View("Login");
                }
            }
            else 
            {
                ViewBag.valErrors = ModelState.Values;                
                return View("Login");
            }
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}

