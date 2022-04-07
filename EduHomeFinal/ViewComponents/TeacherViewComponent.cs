using EduHomeFinal.DAL;
using EduHomeFinal.Models;
using EduHomeFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.ViewComponents
{
    public class TeacherViewComponent:ViewComponent
    {
        #region AppDbContext
        private readonly AppDbContext _db;
        public TeacherViewComponent(AppDbContext db)
        {
            this._db = db;
        }
        #endregion

        #region InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<Teacher> teachers = await _db.Teachers.Where(x => !x.IsDeactive).Include(x => x.TeacherPosition).OrderByDescending(x=>x.Id).Include(x => x.TeacherSocialMedia).ToListAsync();
              
            
            return View(teachers);
        }
        #endregion


    }
}
