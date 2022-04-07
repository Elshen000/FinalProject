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
    public class SpeakerController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SpeakerController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            List<Speaker> speaker = await _db.Speakers.ToListAsync();
            return View(speaker);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Speaker speaker)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (speaker.Photo != null)
            {
                if (!speaker.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Select Image!");
                    return View();

                }
                if (!speaker.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select Image Max 30mb!");
                    return View();
                }


                string folder = Path.Combine(_env.WebRootPath, "img", "event");
                speaker.Image = await speaker.Photo.SaveFileAsync(folder);
            }

            else
            {
                ModelState.AddModelError("Photo", "Please Select Image!");
                return View();

            }
            bool exist = _db.Speakers.Any(x => x.SpeakerName == speaker.SpeakerName);
            if (exist)
            {
                ModelState.AddModelError("SpeakerName", "This Name is alaready exist");
                return View();
            }
            if (speaker.SpeakerName==null)
            {
                ModelState.AddModelError("SpeakerName", "Please Enter Name");
                return View();
            }

            Speaker speaker1 = new Speaker();
            speaker1.Id = speaker.Id;
            speaker1.SpeakerName = speaker.SpeakerName;
            speaker1.SpeakerPlace = speaker.SpeakerPlace;
           


            await _db.Speakers.AddAsync(speaker);
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
            Speaker speaker = await _db.Speakers.FirstOrDefaultAsync(x => x.Id == id);
            if (speaker == null)
            {
                return BadRequest();
            }
            if (speaker.IsDeactive)
            {
                speaker.IsDeactive = false;
            }
            else
            {
                speaker.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
