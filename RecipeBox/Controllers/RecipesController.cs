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
    public class RecipesController : Controller
    {

        private readonly RecipeBoxContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecipesController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            return View(_db.Recipes
                .Where(x => x.User.Id == currentUser.Id).ToList());
        }

        public ActionResult Create()
        {
            // Selectlist type - need DataList? Is it a thing?
            ViewBag.TagId = new SelectList(_db.Tags, "TagId");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Recipe recipe, int TagId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            recipe.User = currentUser;
            _db.Recipes.Add(recipe);
            if (TagId != 0)
            {
                _db.TagRecipe.Add(new TagRecipe() { TagId = TagId, RecipeId = recipe.RecipeId});
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var thisRecipe = _db.Recipes
                .Include(recipe => recipe.Tags)
                .ThenInclude(join => join.Tag)
                .Where(recipe => recipe.User.Id == currentUser.Id)  // queries for only recipes with the current user's Id
                .FirstOrDefault(recipe => recipe.RecipeId == id);
            if (thisRecipe != null)
            {
                return View(thisRecipe);
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
            ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Name");
            var thisRecipe = _db.Recipes
                .Where(r => r.User.Id == currentUser.Id)
                .FirstOrDefault(recipes => recipes.RecipeId == id);
            if (thisRecipe != null)
            {
                return View(thisRecipe);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult Edit(Recipe recipe, int TagId)
        {
            if (TagId != 0)
            {
                _db.TagRecipe.Add(new TagRecipe() { TagId = TagId, RecipeId = recipe.RecipeId});
            }
            _db.Entry(recipe).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");               
        }

        public async Task<ActionResult> Delete(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var thisRecipe = _db.Recipes
                .Where(r => r.User.Id == currentUser.Id)
                .FirstOrDefault(recipes => recipes.RecipeId == id);
            if (thisRecipe != null)
            {
                return View(thisRecipe);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisRecipe = _db.Recipes
                .FirstOrDefault(recipes => recipes.RecipeId == id);
            _db.Recipes.Remove(thisRecipe);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteTag(int joinId)
        {
            var joinEntry = _db.TagRecipe.FirstOrDefault(entry => entry.TagRecipeId == joinId);
            _db.TagRecipe.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}