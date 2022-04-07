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
    public class AboutViewComponent:ViewComponent
    {
        
        #region DbContext
        private readonly AppDbContext _db;
        public AboutViewComponent(AppDbContext db)
        {
            this._db = db;
        }
        #endregion

        #region InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {

            About about = await _db.Abouts.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return View(about);
        }
        #endregion
    }
}
