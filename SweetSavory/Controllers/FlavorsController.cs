using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Collections.Generic;
using SweetSavory.Models;
using SweetSavory.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SweetSavory.Controllers
{
    public class FlavorsController : Controller
    {
        private readonly SweetSavoryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public FlavorsController(UserManager<ApplicationUser> userManager, SweetSavoryContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Flavors.ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(Flavor flavor)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            flavor.User = currentUser;
            _db.Flavors.Add(flavor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> Details(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var thisFlavor = _db.Flavors
                .Include(flavor => flavor.Treats)
                .ThenInclude(join => join.Treat)
                .Where(flavor => flavor.User.Id == currentUser.Id)
                .FirstOrDefault(flavor => flavor.FlavorId == id);
            if (thisFlavor != null)
            {
                return View(thisFlavor);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var thisFlavor = _db.Flavors
                .Where(r => r.User.Id == currentUser.Id)
                .FirstOrDefault(flavors => flavors.FlavorId == id);
            if (thisFlavor != null)
            {
                return View(thisFlavor);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Flavor flavor)
        {
            _db.Entry(flavor).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var thisFlavor = _db.Flavors
                .Where(r => r.User.Id == currentUser.Id)
                .FirstOrDefault(flavors => flavors.FlavorId == id);
            if (thisFlavor != null)
            {
                return View(thisFlavor);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisFlavor = _db.Flavors
                .FirstOrDefault(flavors => flavors.FlavorId == id);
            _db.Flavors.Remove(thisFlavor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}