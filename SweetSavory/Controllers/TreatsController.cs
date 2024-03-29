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
    public class TreatsController : Controller
    {
        private readonly SweetSavoryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TreatsController(UserManager<ApplicationUser> userManager, SweetSavoryContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Treats
                .OrderBy(treats => treats.Name)
                .ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(Treat treat, int FlavorId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            treat.User = currentUser;
            _db.Treats.Add(treat);
            if (FlavorId != 0)
            {
                _db.FlavorTreat.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId });
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var thisTreat = _db.Treats
                .Include(treat => treat.Flavors)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(treat => treat.TreatId == id);
            return View(thisTreat);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
            var thisTreat = _db.Treats
                .Include(treat => treat.Flavors)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(treats => treats.TreatId == id);
            return View(thisTreat);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Treat treat, int FlavorId)
        {
            _db.Entry(treat).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Edit", new { id = treat.TreatId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddFlavor(Treat treat, int FlavorId)
        {
            if (FlavorId != 0)
            {
                _db.FlavorTreat.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId });
            }
            _db.Entry(treat).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Edit", new { id = treat.TreatId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteFlavor(int joinId, int treatId)
        {
            var joinEntry = _db.FlavorTreat.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
            _db.FlavorTreat.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Edit", new { id = treatId });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var thisTreat = _db.Treats
                .FirstOrDefault(treats => treats.TreatId == id);
            return View(thisTreat);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisTreat = _db.Treats
                .FirstOrDefault(treats => treats.TreatId == id);
            _db.Treats.Remove(thisTreat);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}