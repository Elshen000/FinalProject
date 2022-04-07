using EduHomeFinal.DAL;
using EduHomeFinal.Models;
using EduHomeFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Controllers
{
    public class TeacherController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        public TeacherController(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
           
            List<Teacher> teachers = await _db.Teachers.OrderByDescending(x => x.Id).Where(x=>x.IsDeactive).Include(x => x.TeacherDetail).Include(x => x.TeacherPosition).Include(x => x.TeacherSkill).Include(x => x.TeacherSocialMedia).ToListAsync();
            return View(teachers);
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            TeacherVM teacherVM = new TeacherVM
            {

                TeacherDetail = await _db.TeacherDetails.FirstOrDefaultAsync(x => x.TeacherId == id),
                Teacher = await _db.Teachers.Include(x => x.TeacherPosition).FirstOrDefaultAsync(x => x.Id == id),
                TeacherSkills = await _db.TeacherSkills.Where(x => x.TeacherId == id).FirstOrDefaultAsync(),
                TeacherSocialMedia = await _db.TeacherSocialMedias.FirstOrDefaultAsync(x => x.TeacherId == id),
                TeacherPosition = await _db.TeacherPositions.FirstOrDefaultAsync(x => x.TeacherId == id)
            };
            return View(teacherVM);


        }
        #endregion

        #region TeacherSearch
        public async Task<IActionResult> TeacherSearch(string keyword1)
        {
            List<Teacher> teachers = await _db.Teachers.OrderByDescending(x => x.Id).Where(x => x.Name.Contains(keyword1) && !x.IsDeactive).Include(x => x.TeacherPosition).Include(x => x.TeacherSocialMedia).ToListAsync();
            return PartialView("_SearchTeacherPartial", teachers);
        }
        #endregion

    }
}
