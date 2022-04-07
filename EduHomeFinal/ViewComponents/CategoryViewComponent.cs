using EduHomeFinal.DAL;
using EduHomeFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.ViewComponents
{
    public class CategoryViewComponent:ViewComponent
    {
        #region DbContext
        private readonly AppDbContext _db;
        public CategoryViewComponent(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<Category> category = await _db.Categories.OrderByDescending(x => x.Id).Include(x => x.CourseCategories).ToListAsync();
            return View(category);
        }
        #endregion
    }
}
