using EduHomeFinal.DAL;
using EduHomeFinal.Extensions;
using EduHomeFinal.Models;
using EduHomeFinal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
    public class CourseController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;


        public CourseController(AppDbContext db, IWebHostEnvironment env, UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<AppUser> signInManager)
        {
            _db = db;
            _env = env;

            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int page=1)
        {
           
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Blogs.Count() / 6);
            List<Course> courses = await _db.Courses.OrderByDescending(x => x.Id).Skip((page - 1) * 6).Take(6).Include(x => x.CourseDetail).Include(x=>x.CourseFeatures).Include(x=>x.CourseCategories).ThenInclude(X=>X.Category).ToListAsync();
            return View(courses);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            List<ModeratorVM> moderators = new List<ModeratorVM>();
            var items = await _userManager.GetUsersInRoleAsync("Moderator");
            foreach (var item in items)
            {
                moderators.Add(new ModeratorVM { Id = item.Id, Name = item.Name, Surname = item.Surname });
            }
            ViewBag.moderators = moderators;
            ViewBag.Category = await _db.Categories.Include(x => x.CourseCategories).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course, int?id,int[] CourseId,string modeId)
        {
           
            ViewBag.Category = await _db.Categories.Include(x => x.CourseCategories).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            bool exist = _db.Courses.Any(x => x.CourseName == course.CourseName);
            if (exist)
            {
                ModelState.AddModelError("Name", "This Name is already exist!");
                return View();
            }
            if (course.Photo!=null)
            {
                if (!course.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please  Select Image !");
                    return View();

                }
                if (!course.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select Image max 30 MB !");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "course");
                course.Image = await course.Photo.SaveFileAsync(folder);

            }
            else
            {
                ModelState.AddModelError("Photo", "Please  Select Image !");
                return View();
            }
            if (course.CourseName == null)
            {
                ModelState.AddModelError("CourseName", "Please Enter Name");
                return View();
            }



            CourseDetail courseDetail = new CourseDetail();
            courseDetail.CourseId = course.CourseDetail.CourseId;
            courseDetail.CourseAbout = course.CourseDetail.CourseAbout;
            courseDetail.CourseApply = course.CourseDetail.CourseApply;
            courseDetail.CourseSertification = course.CourseDetail.CourseSertification;
            CourseFeature courseFeatures = new CourseFeature();
            courseFeatures.CourseId = course.CourseFeatures.CourseId;
            courseFeatures.ClassDuration = course.CourseFeatures.ClassDuration;
            courseFeatures.SkillLevel = course.CourseFeatures.SkillLevel;
            courseFeatures.StartTime = course.CourseFeatures.StartTime;
           
            courseFeatures.Duration = course.CourseFeatures.Duration;
            courseFeatures.Language = course.CourseFeatures.Language;
            courseFeatures.Assestment = course.CourseFeatures.Assestment;
            courseFeatures.StudentsCount = course.CourseFeatures.StudentsCount;
           
            List<CourseCategory> courseCategories = new List<CourseCategory>();

            foreach (var item in CourseId )
            {
                CourseCategory coursecategory = new CourseCategory();
                coursecategory.CategoryId = item;
                courseCategories.Add(coursecategory);
            }
            course.CourseCategories = courseCategories;

            List<ModeratorVM> moderators = new List<ModeratorVM>();
            var items = await _userManager.GetUsersInRoleAsync("Moderator");
            foreach (var item in items)
            {
                moderators.Add(new ModeratorVM { Id = item.Id, Name = item.Name, Surname = item.Surname });
               
            }
            ViewBag.moderators = moderators;
            course.AppUserId =  modeId;
            


           

            await _db.Courses.AddAsync(course);
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
            Course course = await _db.Courses.Include(x => x.CourseDetail).Include(x => x.CourseFeatures).FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return BadRequest();
            }
            if (course.IsDeactive)
            {
                course.IsDeactive = false;
            }
            else
            {
                course.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int?id)
        {
           
            ViewBag.Category = await _db.Categories.Include(x => x.CourseCategories).ToListAsync();


            Course dbCourse = await _db.Courses.Include(x=>x.CourseDetail).Include(x=>x.CourseFeatures).Include(x=>x.CourseCategories).ThenInclude(x=>x.Category).FirstOrDefaultAsync(x=>x.Id==id);
            if (dbCourse == null)
            {
                return NotFound();
            }
            List<ModeratorVM> moderators = new List<ModeratorVM>();
            var items = await _userManager.GetUsersInRoleAsync("Moderator");
            foreach (var item in items)
            {
                moderators.Add(new ModeratorVM { Id = item.Id, Name = item.Name, Surname = item.Surname });
            }
            ViewBag.moderators = moderators;
            return View(dbCourse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Course course,int?id, int[] CourseId, string modeId)
        {
           
            ViewBag.Category = await _db.Categories.Include(x => x.CourseCategories).ToListAsync();

            Course dbCourse = await _db.Courses.Include(x => x.CourseDetail).Include(x => x.CourseFeatures).Include(x => x.CourseCategories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (dbCourse==null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {

                return View(dbCourse);
            }
            if (course.Photo!=null)
            {
                if (!course.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Select Image");
                    return View(dbCourse);
                }
                if (!course.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select Image Max 30MB !");
                    return View(dbCourse);
                }
                
                string folder = Path.Combine(_env.WebRootPath, "img", "course");
                course.Image = await course.Photo.SaveFileAsync(folder);
                dbCourse.Image = course.Image;
            }
            else
            {
                ModelState.AddModelError("Photo", "Please Select Image!");
                return View(dbCourse);
            }
            bool exist = _db.Courses.Any(x => x.CourseName == course.CourseName && x.Id!= id);
            if (exist)
            {
                ModelState.AddModelError("CourseName", "This Name is already exist!");
                return View(dbCourse);
            }
            if (course.CourseName==null)
            {
                ModelState.AddModelError("CourseName", "Please Enter Name");
                return View(dbCourse);
            }
            dbCourse.CourseName = course.CourseName;
            dbCourse.CourseDetail.CourseAbout = course.CourseDetail.CourseAbout;
            dbCourse.CourseDetail.CourseApply = course.CourseDetail.CourseApply;
            dbCourse.CourseDetail.CourseSertification = course.CourseDetail.CourseSertification;
            dbCourse.CourseFeatures.Language = course.CourseFeatures.Language;
            dbCourse.CourseDescription = course.CourseDescription;
          
            dbCourse.CourseFeatures.SkillLevel = course.CourseFeatures.SkillLevel;
            dbCourse.CourseFeatures.Assestment = course.CourseFeatures.Assestment;
            dbCourse.CourseFeatures.ClassDuration = course.CourseFeatures.ClassDuration;
            dbCourse.CourseFeatures.CourseFee = course.CourseFeatures.CourseFee;
            dbCourse.CourseFeatures.StartTime = course.CourseFeatures.StartTime;
            dbCourse.CourseFeatures.StudentsCount = course.CourseFeatures.StudentsCount;
            dbCourse.CourseFeatures.Duration = course.CourseFeatures.Duration;

            List<CourseCategory> courseCategories = new List<CourseCategory>();

            foreach (var item in CourseId)
            {
                CourseCategory coursecategory = new CourseCategory();
                coursecategory.CategoryId = item;
                courseCategories.Add(coursecategory);
            }
            dbCourse.CourseCategories = courseCategories;

            List<ModeratorVM> moderators = new List<ModeratorVM>();
            var items = await _userManager.GetUsersInRoleAsync("Moderator");
            foreach (var item in items)
            {
                moderators.Add(new ModeratorVM { Id = item.Id, Name = item.Name, Surname = item.Surname });
            }
            ViewBag.moderators = moderators;
            dbCourse.AppUserId = modeId;


            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

    }
}