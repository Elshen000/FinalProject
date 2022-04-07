using EduHomeFinal.DAL;
using EduHomeFinal.Extensions;
using EduHomeFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeacherController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public TeacherController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
           
            List<Teacher> teachers = await _db.Teachers.OrderByDescending(x => x.Id).Include(x => x.TeacherDetail).Include(x => x.TeacherPosition).Include(x=>x.TeacherSkill).Include(x=>x.TeacherSocialMedia).ToListAsync();
            return View(teachers);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher,int?id)
        {
            if (teacher==null)
            {
                return NotFound();
            }

           
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (teacher.Photo!=null)
            {
                if (!teacher.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Select Image");
                    return View();
                }
                if (!teacher.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select Image Max 30Mb");
                    return View();
                }

                string folder = Path.Combine(_env.WebRootPath, "img", "teacher");
                await teacher.Photo.SaveFileAsync(folder);
                teacher.Image=await teacher.Photo.SaveFileAsync(folder);
            }
            else
            {
                ModelState.AddModelError("Photo", "Please Select Image");
                return View();
            }
          

            bool exist = _db.Teachers.Any(x => x.Name == teacher.Name);
            if (exist)
            {
                ModelState.AddModelError("Name", "This Teacher Name is Already Exist !");
                return View();
            }
            if (teacher.Name==null)
            {
                ModelState.AddModelError("Name", "Please Enter Name");
                return View();
            }


            TeacherDetail teacherDetail = new TeacherDetail();
            teacherDetail.Hobbies = teacher.TeacherDetail.Hobbies;
            teacherDetail.Experience = teacher.TeacherDetail.Experience;
            teacherDetail.Degree = teacher.TeacherDetail.Degree;
            teacherDetail.Description = teacher.TeacherDetail.Description;
            teacherDetail.Email = teacher.TeacherDetail.Email;
            teacherDetail.Faculty = teacher.TeacherDetail.Faculty;
            teacherDetail.PhoneNumber = teacher.TeacherDetail.PhoneNumber;
            teacherDetail.Skype = teacher.TeacherDetail.Skype;
            teacherDetail.TeacherId = teacher.Id;
            teacher.TeacherDetail = teacherDetail;
            TeacherPosition teacherPosition = new TeacherPosition();
            teacherPosition.TeacherId = teacher.Id;
            teacherPosition = teacher.TeacherPosition;
            teacherPosition.Position = teacher.TeacherPosition.Position;
            teacherPosition.Id = teacher.TeacherPosition.Id;
           
            TeacherSocialMedia teacherSocialMedia = new TeacherSocialMedia();
            teacherSocialMedia.TeacherId = teacher.Id;
            teacherSocialMedia.Facebook = teacher.TeacherSocialMedia.Facebook;
            teacherSocialMedia.Pinterest = teacher.TeacherSocialMedia.Pinterest;
            teacherSocialMedia.Vimeo = teacher.TeacherSocialMedia.Vimeo;
            teacherSocialMedia.Twitter = teacher.TeacherSocialMedia.Twitter;
            TeacherSkill teacherSkills = new TeacherSkill();
            teacherSkills.TeacherId = teacher.Id;
            teacherSkills.SkillLevel1 = teacher.TeacherSkill.SkillLevel1;
            teacherSkills.SkillName1 = teacher.TeacherSkill.SkillName1;
            teacherSkills.SkillLevel2 = teacher.TeacherSkill.SkillLevel2;
            teacherSkills.SkillName2 = teacher.TeacherSkill.SkillName2;
            teacherSkills.SkillLevel3 = teacher.TeacherSkill.SkillLevel3;
            teacherSkills.SkillName3 = teacher.TeacherSkill.SkillName3;
            teacherSkills.SkillLevel4 = teacher.TeacherSkill.SkillLevel4;
            teacherSkills.SkillName4 = teacher.TeacherSkill.SkillName4;
            teacherSkills.SkillLevel5 = teacher.TeacherSkill.SkillLevel5;
            teacherSkills.SkillName5 = teacher.TeacherSkill.SkillName5;
            teacherSkills.SkillLevel6 = teacher.TeacherSkill.SkillLevel6;
            teacherSkills.SkillName6 = teacher.TeacherSkill.SkillName6;


            await _db.Teachers.AddAsync(teacher);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

           
        }
        #endregion

        #region Update
        public IActionResult Update(int ? id)
        {
            Teacher dbTeacher = _db.Teachers.Include(x => x.TeacherPosition).Include(x => x.TeacherDetail).Include(x=>x.TeacherSocialMedia).FirstOrDefault(x => x.Id == id);
            if (dbTeacher == null)
            {
                return NotFound();
            }
            return View(dbTeacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Teacher teacher,int? id)
        {
            Teacher dbTeacher = _db.Teachers.Include(x => x.TeacherPosition).Include(x => x.TeacherDetail).Include(x=>x.TeacherSkill).Include(x=>x.TeacherSocialMedia).FirstOrDefault(x => x.Id == id);
            if (dbTeacher==null)
            {
                return NotFound();
            }
            if (teacher.Photo!=null)
            {
                if (!teacher.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Select Image");
                    return View(dbTeacher);
                }
                if (!teacher.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select max 30 MB!");
                    return View(dbTeacher);
                }
                if (!ModelState.IsValid)
                {
                    return View(dbTeacher);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "teacher");
                teacher.Image = await teacher.Photo.SaveFileAsync(folder);
                dbTeacher.Image = teacher.Image;
            }
            else
            {
                ModelState.AddModelError("Photo", "Please Select Image");
                return View(dbTeacher);
            }
            bool exist = _db.Teachers.Any(x => x.Name == teacher.Name && x.Id != id);
            if (exist)
            {
                ModelState.AddModelError("Name", "This Name is already exist");
                return View(dbTeacher);
            }
            dbTeacher.Name = teacher.Name;
            dbTeacher.TeacherDetail.TeacherId = teacher.Id;
            dbTeacher.TeacherDetail.Hobbies = teacher.TeacherDetail.Hobbies;
            dbTeacher.TeacherDetail.Description = teacher.TeacherDetail.Description;
            dbTeacher.TeacherDetail.Degree = teacher.TeacherDetail.Degree;
            dbTeacher.TeacherDetail.Email = teacher.TeacherDetail.Email;
            dbTeacher.TeacherDetail.Experience = teacher.TeacherDetail.Experience;
            dbTeacher.TeacherDetail.Faculty = teacher.TeacherDetail.Faculty;
            dbTeacher.TeacherDetail.PhoneNumber = teacher.TeacherDetail.PhoneNumber;
            dbTeacher.TeacherDetail.Skype = teacher.TeacherDetail.Skype;
            dbTeacher.TeacherSocialMedia.Facebook = teacher.TeacherSocialMedia.Facebook;
            dbTeacher.TeacherSocialMedia.Vimeo = teacher.TeacherSocialMedia.Vimeo;
            dbTeacher.TeacherSocialMedia.Twitter = teacher.TeacherSocialMedia.Twitter;
            dbTeacher.TeacherSocialMedia.Pinterest = teacher.TeacherSocialMedia.Pinterest;
            dbTeacher.TeacherPosition.Position = teacher.TeacherPosition.Position;

            dbTeacher.TeacherSkill.TeacherId = teacher.Id;
            dbTeacher.TeacherSkill.SkillName1 = teacher.TeacherSkill.SkillName1;
            dbTeacher.TeacherSkill.SkillLevel1 = teacher.TeacherSkill.SkillLevel1;

            dbTeacher.TeacherSkill.SkillName2 = teacher.TeacherSkill.SkillName2;
            dbTeacher.TeacherSkill.SkillLevel2 = teacher.TeacherSkill.SkillLevel2;

            dbTeacher.TeacherSkill.SkillName3 = teacher.TeacherSkill.SkillName3;
            dbTeacher.TeacherSkill.SkillLevel3 = teacher.TeacherSkill.SkillLevel3;

            dbTeacher.TeacherSkill.SkillName4 = teacher.TeacherSkill.SkillName4;
            dbTeacher.TeacherSkill.SkillLevel4 = teacher.TeacherSkill.SkillLevel4;

            dbTeacher.TeacherSkill.SkillName5 = teacher.TeacherSkill.SkillName5;
            dbTeacher.TeacherSkill.SkillLevel5 = teacher.TeacherSkill.SkillLevel5;

            dbTeacher.TeacherSkill.SkillName6 = teacher.TeacherSkill.SkillName6;
            dbTeacher.TeacherSkill.SkillLevel6 = teacher.TeacherSkill.SkillLevel6;

            dbTeacher.Id = teacher.Id;
            dbTeacher.Image = teacher.Image;

            await _db.SaveChangesAsync();
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
            Teacher teacher = await _db.Teachers.Include(x => x.TeacherDetail).Include(x => x.TeacherPosition).Include(x=>x.TeacherSkill).Include(x=>x.TeacherSocialMedia).FirstOrDefaultAsync(x => x.Id == id);
            if (teacher == null)
            {
                return BadRequest();
            }
            if (teacher.IsDeactive)
            {
                teacher.IsDeactive = false;
            }
            else
            {
                teacher.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        


    }
}
