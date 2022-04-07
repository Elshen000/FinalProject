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
    public class AboutController : Controller
    {


        #region DbContext
        private readonly AppDbContext _db;
        public AboutController(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int? id)
        {
            AboutVM aboutVM = new AboutVM
            {
                About = await _db.Abouts.FirstOrDefaultAsync(),
                Teachers = await _db.Teachers.OrderByDescending(x=>x.Id).Where(x=>!x.IsDeactive).Take(4).Include(x=>x.TeacherPosition).Include(x=>x.TeacherDetail).Include(x=>x.TeacherDetail).Include(x=>x.TeacherSocialMedia).ToListAsync()

            };
            return View(aboutVM);
        }
        #endregion


       
    }
}
