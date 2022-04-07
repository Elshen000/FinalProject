using EduHomeFinal.DAL;
using EduHomeFinal.Extensions;
using EduHomeFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AboutController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            About about = await _db.Abouts.FirstOrDefaultAsync();
            return View(about);
        }
        #endregion

        #region Update
        public IActionResult Update(int? id)
        {
            About dbAbout = _db.Abouts.FirstOrDefault(x => x.Id == id);
            if (dbAbout == null)
            {
                return NotFound();
            }
            return View(dbAbout);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(About about, int? id)
        {
            About dbAbout = _db.Abouts.FirstOrDefault(x => x.Id == id);
            if (dbAbout == null)
            {
                return NotFound();
            }
            if (about.Photo != null)
            {
                if (!about.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Select Image!");
                    return View(dbAbout);
                }
                if (!about.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select Image Max 30 MB!");
                    return View(dbAbout);
                }
                if (!ModelState.IsValid)
                {
                    return View(dbAbout);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "blog");
                about.Image = await about.Photo.SaveFileAsync(folder);
                dbAbout.Image = about.Image;
            }
            else
            {
                ModelState.AddModelError("Photo", "Please Select Image!");
                return View(dbAbout);
            }
            bool exist = _db.Abouts.Any(x => x.Title == about.Title && x.Id != id);
            if (exist)
            {
                ModelState.AddModelError("BlogCreator", "This Name already exist!");
                return View(dbAbout);
            }
            dbAbout.Title = about.Title;
            dbAbout.Image = about.Image;
            dbAbout.Description = about.Description;
            dbAbout.Information = about.Information;
            dbAbout.Information = about.Information;

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
            ;
        }
        #endregion



    }
}
