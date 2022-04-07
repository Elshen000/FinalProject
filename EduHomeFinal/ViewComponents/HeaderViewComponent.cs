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
    public class HeaderViewComponent:ViewComponent
    {
        #region DbContext
        private readonly AppDbContext _db;
        public HeaderViewComponent(AppDbContext db)
        {
            this._db = db;
        }
        #endregion

        #region InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Bio bio = await _db.Bios.FirstOrDefaultAsync();
            return View(bio);
        }
        #endregion



    }

}

