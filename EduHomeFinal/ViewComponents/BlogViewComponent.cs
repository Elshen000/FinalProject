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
    public class BlogViewComponent:ViewComponent
    {
        #region DbContext
        private readonly AppDbContext _db;
        public BlogViewComponent(AppDbContext db)
        {
            this._db = db;
        }
        #endregion

        #region InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<Blog> blog = await _db.Blogs.OrderByDescending(x => x.Id).Where(x => !x.IsDeactive).Take(3).Include(x=>x.BlogDetail).ToListAsync();
            return View(blog);
        }
        #endregion

    }
}
