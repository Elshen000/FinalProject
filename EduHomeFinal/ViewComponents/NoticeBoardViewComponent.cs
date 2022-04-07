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
    public class NoticeBoardViewComponent:ViewComponent
    {
        #region DbContext
        private readonly AppDbContext _db;
        public NoticeBoardViewComponent(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<NoticeBoard> noticeBoards = await _db.NoticeBoards.ToListAsync();
            return View(noticeBoards);


        }
        #endregion


    }
}
