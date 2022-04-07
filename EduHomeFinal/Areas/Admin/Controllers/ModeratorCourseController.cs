using EduHomeFinal.DAL;
using EduHomeFinal.Extensions;
using EduHomeFinal.Models;
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
    [Authorize(Roles = "Moderator,Admin")]
    public class ModeratorCourseController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;


        public ModeratorCourseController(AppDbContext db, IWebHostEnvironment env, UserManager<AppUser> userManager,
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
        public async Task<IActionResult> Index()
        {
            string userid = _userManager.GetUserId(User);
            List<Course> courses = await _db.Courses.Where(x=>x.AppUserId ==userid).OrderByDescending(x => x.Id).Include(x => x.CourseDetail).Include(x => x.CourseFeatures).Include(x => x.CourseCategories).ThenInclude(X => X.Category).ToListAsync();
            return View(courses);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Category = await _db.Categories.Include(x => x.CourseCategories).ToListAsync();

            Course dbCourse = await _db.Courses.Include(x => x.CourseDetail).Include(x => x.CourseFeatures).Include(x => x.CourseCategories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (dbCourse == null)
            {
                return NotFound();
            }
            return View(dbCourse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Course course, int? id, int[] CourseId)
        {
            ViewBag.Category = await _db.Categories.Include(x => x.CourseCategories).ToListAsync();

            Course dbCourse = await _db.Courses.Include(x => x.CourseDetail).Include(x => x.CourseFeatures).Include(x => x.CourseCategories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (dbCourse == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {

                return View(dbCourse);
            }
            if (course.Photo != null)
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
            bool exist = _db.Courses.Any(x => x.CourseName == course.CourseName && x.Id != id);
            if (exist)
            {
                ModelState.AddModelError("CourseName", "This Name is already exist!");
                return View(dbCourse);
            }
            if (course.CourseName == null)
            {
                ModelState.AddModelError("CourseName", "Please Enter Name");
                return View(dbCourse);
            }
            dbCourse.CourseName = course.CourseName;
            dbCourse.CourseDetail.CourseAbout = course.CourseDetail.CourseAbout;
            dbCourse.CourseDetail.CourseApply = course.CourseDetail.CourseApply;
            dbCourse.CourseDetail.CourseSertification = course.CourseDetail.CourseSertification;
            dbCourse.CourseFeatures.Language = course.CourseFeatures.Language;

            dbCourse.CourseFeatures.SkillLevel = course.CourseFeatures.SkillLevel;
            dbCourse.CourseFeatures.Assestment = course.CourseFeatures.Language;
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


            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
