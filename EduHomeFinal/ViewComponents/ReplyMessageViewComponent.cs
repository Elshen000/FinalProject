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
    public class ReplyMessageViewComponent:ViewComponent
    {
        #region AppDbContext
        private readonly AppDbContext _db;
        public ReplyMessageViewComponent(AppDbContext db)
        {
            this._db = db;
        }
        #endregion

        #region InvokeAsync
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ReplyMessage replyMessage = await _db.ReplyMessages.FirstOrDefaultAsync();
            return View(replyMessage);
        }
        #endregion



    }
}
