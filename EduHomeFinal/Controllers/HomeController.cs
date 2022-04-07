
using EduHomeFinal.DAL;
using EduHomeFinal.Models;
using EduHomeFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Controllers
{
    public class HomeController : Controller
    {
        #region AppDbContext
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {

            HomeVM homeVM = new HomeVM
            {
                About = await _db.Abouts.FirstOrDefaultAsync(),
                Course = await _db.Courses.OrderByDescending(x => x.Id).Where(x => !x.IsDeactive).Include(x => x.CourseDetail).Include(x => x.CourseFeatures).Take(3).ToListAsync(),
                Event = await _db.Events.OrderByDescending(x => x.Id).Where(x => !x.IsDeactive).Include(x => x.EventDetail).Include(x => x.EventSpeaker).Take(3).ToListAsync(),
                Blog = await _db.Blogs.OrderByDescending(x => x.Id).Where(x => !x.IsDeactive).Include(x => x.BlogDetail).Take(3).ToListAsync(),
                Slider = await _db.Sliders.OrderByDescending(x=>x.Id).Where(x=>!x.IsDeactive).ToListAsync(),
                
            };
            return View(homeVM);
        }
        #endregion

        #region Subscribe
        public async Task<ContentResult>Subscribe(string email)
        {
            Subscribe subscribe = new Subscribe();
            subscribe.Email = email;
            bool exist = _db.Subscribes.Any(x => x.Email == email);
            if (exist)
            {
                return Content("Bu adda artıq mövcuddur!");
            }
            await _db.Subscribes.AddAsync(subscribe);
            await _db.SaveChangesAsync();
            return Content("Abunə olundu.");
        }
        #endregion

        #region SiteCreator
        public IActionResult Creator()
        {
            return View ();
        }
        #endregion

        #region GlobalSearch
        public async Task<IActionResult> GlobalSearch(string keywordd)
        {
            GlobalSearchVM globalSearchVM = new GlobalSearchVM
            {
                Courses = await _db.Courses.OrderByDescending(x => x.Id).Where(x => x.CourseName.Contains(keywordd) && !x.IsDeactive).Include(x => x.CourseDetail).Include(x=>x.CourseFeatures).Take(2).ToListAsync(),
                Teachers = await _db.Teachers.OrderByDescending(x => x.Id).Where(x => x.Name.Contains(keywordd) && !x.IsDeactive).Include(x => x.TeacherDetail).Include(x=>x.TeacherPosition).Include(x=>x.TeacherSkill).Include(x=>x.TeacherSocialMedia).Take(2).ToListAsync(),
                Events = await _db.Events.OrderByDescending(x=>x.Id).Where(x=>x.EventName.Contains(keywordd) && !x.IsDeactive).Include(x=>x.EventDetail).Include(x=>x.EventSpeaker).Take(2).ToListAsync(),
                Blogs = await _db.Blogs.OrderByDescending(x => x.Id).Where(x => x.BlogCreator.Contains(keywordd) && !x.IsDeactive).Include(x => x.BlogDetail).Take(2).ToListAsync()
            };
            return PartialView("_SearchGlobalPartial", globalSearchVM);
        }
        #endregion


    }
}
