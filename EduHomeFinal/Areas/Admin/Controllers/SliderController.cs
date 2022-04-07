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
    public class SliderController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private int id;

        public SliderController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _db.Sliders.OrderByDescending(x => x.Id == id).ToListAsync();
            return View(sliders);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.Photo != null && slider.Photo2!=null)
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Select Image!");
                    return View();
                }
                if (!slider.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select Image Max 30Mb!");
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                slider.Image = await slider.Photo.SaveFileAsync(folder);
                slider.BGImage = await slider.Photo2.SaveFileAsync(folder);
            }
            else
            {
                ModelState.AddModelError("Photo", "Please Select Image!");
                return View();
            }
           

            Slider Slider = new Slider();
            Slider.Id = slider.Id;
            Slider.Image = slider.Image;
            Slider.BGImage = slider.BGImage;
            Slider.Title = slider.Title;
            Slider.SubTitle = slider.SubTitle;
            Slider.Description = slider.Description;
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int?id)
        {
            if (id==null)
            {
                return NotFound();
            }
            Slider slider = await _db.Sliders.FirstOrDefaultAsync(x=>x.Id==id);
            if (slider==null)
            {
                return NotFound();
            }
            string path = Path.Combine(_env.WebRootPath, "img", slider.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _db.Remove(slider);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
