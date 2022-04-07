using EduHomeFinal.DAL;
using EduHomeFinal.Extensions;
using EduHomeFinal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EduHomeFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EventController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
       

        public EventController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Blogs.Count() / 6);
            List<Event> events = await _db.Events.OrderByDescending(x=>x.Id).Skip((page - 1) * 6).Take(6).Include(x => x.EventDetail ).Include(x => x.EventSpeaker).ToListAsync();
            return View(events);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Speakers = await _db.Speakers.Include(x=>x.EventSpeaker).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event even,int[]speakerId)
        {
            ViewBag.Speakers = await _db.Speakers.Include(x => x.EventSpeaker).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }


            bool exist = _db.Events.Any(x => x.EventName == even.EventName);
            if (exist)
            {
                ModelState.AddModelError("EventName", "Bu adda artıq mövcuddur.");
                return View();
            }

            if (even.Photo != null)
            {
                if (!even.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin!");
                    return View();
                }
                if (!even.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select Image Max 30MB !");
                    return View();
                }
                if (!ModelState.IsValid)
                {

                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "event");
                even.Image = await even.Photo.SaveFileAsync(folder);
            }
            else
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin!");
                return View();
            }
            if (even.EventName==null)
            {
                ModelState.AddModelError("EventName", "Zəhmət olmasa ad daxil edin!");
                return View();
            }



            EventDetail eventDetail = new EventDetail();
            eventDetail.EventId = even.Id; 
            eventDetail.DateTime = even.EventDetail.DateTime;
            eventDetail.StartTime = even.EventDetail.StartTime;
            eventDetail.EndTime = even.EventDetail.EndTime;
            eventDetail.EventPlace = even.EventDetail.EventPlace;
            eventDetail.Description = even.EventDetail.Description;
            eventDetail.Title = even.EventDetail.Title;
            eventDetail.SubTitle = even.EventDetail.SubTitle;
            eventDetail.SubTitle2 = even.EventDetail.SubTitle2;

            List<EventSpeaker> EventSpeaker = new List<EventSpeaker>();
            foreach (int item in speakerId)
            {
                EventSpeaker Eventspeaker = new EventSpeaker();
                Eventspeaker.EventId = even.Id;
                Eventspeaker.SpeakerId = item;
                EventSpeaker.Add(Eventspeaker);
            }
            even.EventSpeaker = EventSpeaker;

           
           
           
            try
            {

                string subject = "New Event";
                var message = $"<p>Event Name : {even.EventName}</p></br> <p> Vaxt : {even.EventDetail.StartTime.ToString("dd/MM  HH-mm")}/{even.EventDetail.EndTime.ToString("HH-mm")}</p></br> <p> Ünvan : {even.EventDetail.EventPlace}</p>";
                foreach (Subscribe subscribe in _db.Subscribes)
                {
                    await Helper.SendMessage(subject, message, subscribe.Email);
                }
            }
            catch (Exception)
            {
                
                await _db.Events.AddAsync(even);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");


            }
            return RedirectToAction("Index");


        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Event even = await _db.Events.Include(x => x.EventDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (even == null)
            {
                return BadRequest();
            }
            if (even.IsDeactive)
            {
                even.IsDeactive = false;
            }
            else
            {
                even.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Speakers = await _db.Speakers.Include(x => x.EventSpeaker).ToListAsync();
            Event dbEvent = await _db.Events.Include(x => x.EventDetail).Include(x=>x.EventSpeaker).ThenInclude(x=>x.Speaker).FirstOrDefaultAsync(x => x.Id == id);
            if (dbEvent == null)
            {
                return NotFound();
            }
            return View(dbEvent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Event even, int? id,int[] SpeakerId)
        {
            ViewBag.Speakers = await _db.Speakers.Include(x => x.EventSpeaker).ToListAsync();
            Event dbEvent = await _db.Events.Include(x => x.EventDetail).Include(x=>x.EventSpeaker).ThenInclude(x=>x.Speaker).FirstOrDefaultAsync(x => x.Id == id);
            if (dbEvent == null)
            {
                return NotFound();
            }
            if (even.Photo != null)
            {
                if (!even.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin!");
                    return View(dbEvent);
                }
                if (!even.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa ölçüsü max 30MB olan şəkil seçin !");
                    return View(dbEvent);
                }
                if (!ModelState.IsValid)
                {

                    return View(dbEvent);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "event");
                even.Image = await even.Photo.SaveFileAsync(folder);
                dbEvent.Image = even.Image;
            }
            else
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin!");
                return View(dbEvent);
            }
            bool exist = _db.Events.Any(x => x.EventName == even.EventName && x.Id != id);
            if (exist)
            {
                ModelState.AddModelError("EventName", "Bu adda artıq mövcuddur.");
                return View(dbEvent);
            }
            dbEvent.EventName = even.EventName;
            dbEvent.EventDetail.DateTime = even.EventDetail.DateTime;
            dbEvent.EventDetail.StartTime = even.EventDetail.StartTime;
            dbEvent.EventDetail.EndTime = even.EventDetail.EndTime;
            dbEvent.EventDetail.StartTime = even.EventDetail.StartTime;
            dbEvent.EventDetail.Description = even.EventDetail.Description;
            dbEvent.EventDetail.Title = even.EventDetail.Title;
            dbEvent.EventDetail.SubTitle = even.EventDetail.SubTitle;
            dbEvent.EventDetail.SubTitle2 = even.EventDetail.SubTitle2;
            dbEvent.EventDetail.EventPlace = even.EventDetail.EventPlace;

            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
            foreach (int item in SpeakerId )
            {
                EventSpeaker EventSpeaker = new EventSpeaker();
                EventSpeaker.EventId = even.Id;
                EventSpeaker.SpeakerId = item;
                eventSpeakers.Add(EventSpeaker);
            }
            dbEvent.EventSpeaker = eventSpeakers;
           
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion





    }
}
