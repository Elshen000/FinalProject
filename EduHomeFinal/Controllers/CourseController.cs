using EduHomeFinal.DAL;
using EduHomeFinal.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Controllers
{
    public class CourseController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;



        public CourseController(AppDbContext db)
        {
            _db = db;

        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
           
            List<Course> courses = await _db.Courses.OrderByDescending(x => x.Id).Where(x => !x.IsDeactive).Take(9).Include(x => x.CourseDetail).Include(x => x.CourseFeatures).Include(x => x.CourseCategories).ToListAsync();
            return View(courses);
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int?id)
        {
            Course courses = await _db.Courses.Include(x => x.CourseDetail).Include(x => x.CourseFeatures).Include(x=>x.CourseCategories).ThenInclude(x=>x.Category).Where(x=>x.Id==id).FirstOrDefaultAsync(x=>x.Id==id);
            return View(courses);
        }
        #endregion

        #region CourseSearch
        public async Task<IActionResult> CourseSearch(string keyword3)
        {
            List<Course> courses = await _db.Courses.OrderByDescending(x => x.Id).Where(x => x.CourseName.Contains(keyword3) && !x.IsDeactive).Include(x => x.CourseDetail).Include(x => x.CourseFeatures).ToListAsync();
            if (keyword3==null)
            {
                List<Course> courses1 = await _db.Courses.OrderByDescending(x => x.Id).Where(x => x.CourseName.Contains(keyword3) && !x.IsDeactive).Include(x => x.CourseDetail).Include(x => x.CourseFeatures).ToListAsync();
                return PartialView("_SearchCoursePartial", courses1);

            }
            return PartialView("_SearchCoursePartial", courses);
        }
        #endregion

        #region CourseCategory
        public async Task<IActionResult> CourseCategory(int id)
        {

            List<Course> courses = await _db.Courses.Where(x=>x.CourseCategories.Any(x=>x.CategoryId==id)).Include(x=>x.CourseCategories).ThenInclude(x=>x.Category).ToListAsync();
           
            return View(courses);
        }
        #endregion



    }
}
