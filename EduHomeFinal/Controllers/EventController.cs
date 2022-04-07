using EduHomeFinal.DAL;
using EduHomeFinal.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Controllers
{
    public class EventController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;

        public EventController(AppDbContext db)
        {
            _db = db;

        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
           
            List<Event> events = await _db.Events.OrderByDescending(x => x.Id).Where(x => !x.IsDeactive).Include(x => x.EventDetail).Include(x => x.EventSpeaker).ToListAsync();
            return View(events);
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int?id)
        {
            Event events = await _db.Events.Include(x => x.EventDetail).Include(x => x.EventSpeaker).ThenInclude(x=>x.Speaker).FirstOrDefaultAsync(x=>x.Id==id);
            return View(events);
        }
        #endregion

        #region EventSearch
        public async Task<IActionResult> EventSearch(string keyword2)
        {
            List<Event> events = await _db.Events.OrderByDescending(x => x.Id).Where(x => x.EventName.Contains(keyword2) && !x.IsDeactive).Include(x => x.EventDetail).Include(x => x.EventSpeaker).ToListAsync();

            if (keyword2==null)
            {
                List<Event> events1 = await _db.Events.OrderByDescending(x => x.Id).Where(x => x.EventName.Contains(keyword2) && !x.IsDeactive).Include(x => x.EventDetail).Include(x => x.EventSpeaker).ToListAsync();
                return PartialView("_SearchEventPartial", events1);
            }
            return PartialView("_SearchEventPartial", events);
        }
        #endregion





    }
}
