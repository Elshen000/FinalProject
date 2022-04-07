using EduHomeFinal.DAL;
using EduHomeFinal.Models;
using EduHomeFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        #region DbContext
        private readonly AppDbContext _db;
        public FooterViewComponent(AppDbContext db)
        {
            this._db = db;
        }
        #endregion

        #region InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HomeVM homeVM = new HomeVM
            {
                Bio = await _db.Bios.FirstOrDefaultAsync()
            };

            return View(homeVM);
        }
        #endregion


    }
}
