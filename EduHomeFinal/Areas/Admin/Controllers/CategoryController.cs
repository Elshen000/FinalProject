using EduHomeFinal.DAL;
using EduHomeFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
           
            List<Category> categories = await _db.Categories.Include(x => x.CourseCategories).ThenInclude(x => x.Course).ToListAsync();
            return View(categories);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.Where(x => x.IsDeactive).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            ViewBag.Categories = await _db.Categories.Where(x => x.IsDeactive).ToListAsync();
            bool exist = _db.Categories.Any(x => x.Name == category.Name);
            if (exist)
            {
                ModelState.AddModelError("Name", "This Category is already exist!");
                return View();
            }
            if (category.Name==null)
            {
                ModelState.AddModelError("Name", "Please Enter Name");
                return View();
            }

            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category category = await _db.Categories.Include(x => x.CourseCategories).ThenInclude(x => x.Course).FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return BadRequest();
            }
            if (category.IsDeactive)
            {
                category.IsDeactive = false;
                foreach (CourseCategory pCat in category.CourseCategories)
                {
                    pCat.Course.IsDeactive = false;
                }
            }
            else
            {
                category.IsDeactive = true;
                foreach (CourseCategory pCat in category.CourseCategories)
                {
                    pCat.Course.IsDeactive = true;
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

       


    }
}
