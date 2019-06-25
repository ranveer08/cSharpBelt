using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using cBelt2.Models;

namespace cBelt2.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext Context)
        {
            dbContext = Context;
        }
        [Route("bright_ideas")]
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId")== null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.CurrentUser = dbContext.Users
            .Where(u => u.UserId == (int)HttpContext.Session
            .GetInt32("UserId"))
            .Single();
            ViewBag.AllIdeas = dbContext.Ideas
            .Include( u => u.Owner)
            .Include(l => l.LikedBy)
            .OrderByDescending(l => l.LikedBy.Count).ToList();
            return View();
        }

        [HttpPost("addIdea")]
        public IActionResult AddNewIdea(string UserIdea)
        {
            if (HttpContext.Session.GetInt32("UserId")== null)
            {
                return RedirectToAction("Index", "Login");
            }
            if(ModelState.IsValid)
            {
                if(UserIdea == null)
                {
                    ModelState.AddModelError("UserIdea","Required Field");
                    return RedirectToAction("Index");
                }
                if(UserIdea.Length < 5)
                {
                    ModelState.AddModelError("UserIdea","Required Field");
                    return RedirectToAction("Index");
                }
            if (UserIdea.Length > 0)
            {
                Idea NewIdea = new Idea()
                {
                    UserId = (int)HttpContext.Session.GetInt32("UserId"),
                    UserIdea = UserIdea
                };
                dbContext.Ideas.Add(NewIdea);
                dbContext.SaveChanges();
            }

            }
            return RedirectToAction("Index");
        }

        [HttpGet("like/{ideaId}")]
        public IActionResult AddLike(int ideaId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Like NewLike = new Like()
            {
                IdeaId = ideaId,
                UserId = (int)HttpContext.Session.GetInt32("UserId")
            };
            dbContext.Likes.Add(NewLike);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("deleteidea/{ideaId}")]
        public IActionResult DeleteIdea(int ideaId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null ){
                return RedirectToAction("Index", "Login");
            } 
            var Delete = new Idea { IdeaId = ideaId};
            dbContext.Ideas.Remove(Delete);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("users/{userId}")]
        public IActionResult UserDetails(int userId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
            return RedirectToAction("Index", "Login");

            }
            ViewBag.UserInfo = dbContext.Users
            .Where(u => u.UserId == userId)
            .Include(i => i.UsersIdeas)
            .Include(l => l.UserLikes).Single();
            return View(); 
        }

        [HttpGet("bright_ideas/{ideaId}")]
        public IActionResult IdeaInfo(int ideaId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.PostedIdeas = dbContext.Ideas
            .Where(i => i.IdeaId == ideaId)
            .Include(u => u.Owner)
            .Include( l => l.LikedBy)
            .ThenInclude(u => u.User)
            .Single();

            ViewBag.LikedBy = dbContext.Likes
            .Where(i => i.IdeaId == ideaId)
            .ToList();
            return View();
        }

    }
}
