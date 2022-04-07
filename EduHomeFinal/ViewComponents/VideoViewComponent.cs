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
    public class VideoViewComponent:ViewComponent
    {
        #region DbContext
        private readonly AppDbContext _db;
        public VideoViewComponent(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Video video = await _db.Videos.FirstOrDefaultAsync();
            return View(video);


        }
        #endregion


    }
}
