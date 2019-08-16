using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Collections.Generic;
using RecipeBox.Models;
using RecipeBox.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecipeBox.Controllers
{
    [Authorize]
    public class TagsController : Controller
    {
     private readonly RecipeBoxContext _db;
     private readonly UserManager<ApplicationUser> _userManager;

     public TagsController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
     {
         _userManager = userManager;
         _db = db;
     }

     public async Task<ActionResult> Index()
     {
          var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            return View(_db.Tags
                .Where(x => x.User.Id == currentUser.Id).ToList());
     }

      public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Tag tag)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            tag.User = currentUser;
            _db.Tags.Add(tag);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var thisTag = _db.Tags
                .Include(tag => tag.Recipes)
                .ThenInclude(join => join.Recipe)
                .Where(tag => tag.User.Id == currentUser.Id)  // queries for only tags with the current user's Id
                .FirstOrDefault(tag => tag.TagId == id);
            if (thisTag != null)
            {
                return View(thisTag);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        public async Task<ActionResult> Edit(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var thisTag = _db.Tags
                .Where(r => r.User.Id == currentUser.Id)
                .FirstOrDefault(tags => tags.TagId == id);
            if (thisTag != null)
            {
                return View(thisTag);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult Edit(Tag tag)
        {
            _db.Entry(tag).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");               
        }

      public async Task<ActionResult> Delete(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var thisTag = _db.Tags
                .Where(r => r.User.Id == currentUser.Id)
                .FirstOrDefault(tags => tags.TagId == id);
            if (thisTag != null)
            {
                return View(thisTag);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisTag = _db.Tags
                .FirstOrDefault(tags => tags.TagId == id);
            _db.Tags.Remove(thisTag);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}