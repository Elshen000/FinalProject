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
    public class BlogController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext db, IWebHostEnvironment env)
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
            List<Blog> blogs = await _db.Blogs.OrderByDescending(x => x.Id).Skip((page - 1) * 6).Take(6).Include(x => x.BlogDetail).ToListAsync();
            return View(blogs);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (blog.Photo != null)
            {
                if (!blog.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Select Image!");
                    return View();

                }
                if (!blog.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select Image Max 30mb!");
                    return View();
                }


                string folder = Path.Combine(_env.WebRootPath, "img", "blog");
                blog.Image = await blog.Photo.SaveFileAsync(folder);
            }

            else
            {
                ModelState.AddModelError("Photo", "Please Select Image!");
                return View();

            }




            //bool exist = _db.Blogs.Any(x => x.BlogCreator == blog.BlogCreator);
            //if (exist)
            //{
            //    ModelState.AddModelError("Name", "This Name is alaready exist");
            //    return View();
            //}
            BlogDetail blogDetail = new BlogDetail();
            blogDetail.BlogId = blog.Id;
            blogDetail.SubTitle = blog.BlogDetail.SubTitle;
            blogDetail.Title = blog.BlogDetail.Title;
            blogDetail.Title2 = blog.BlogDetail.Title2;
            blogDetail.About = blog.BlogDetail.About;
            blogDetail.BlogDesciription = blog.BlogDetail.BlogDesciription;
            blogDetail.CreateTime = blog.BlogDetail.CreateTime;


            await _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public IActionResult Update(int? id)
        {
            Blog dbBlog = _db.Blogs.Include(x => x.BlogDetail).FirstOrDefault(x => x.Id == id);
            if (dbBlog == null)
            {
                return NotFound();
            }
            return View(dbBlog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Blog blog, int? id)
        {
            Blog dbBlog = await _db.Blogs.Include(x => x.BlogDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (dbBlog == null)
            {
                return NotFound();
            }
            if (blog.Photo != null)
            {
                if (!blog.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Select Image!");
                    return View(dbBlog);
                }
                if (!blog.Photo.Max30Mb())
                {
                    ModelState.AddModelError("Photo", "Please Select Image Max 30 MB!");
                }
                if (!ModelState.IsValid)
                {
                    return View(dbBlog);
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "blog");
                blog.Image = await blog.Photo.SaveFileAsync(folder);
                dbBlog.Image = blog.Image;
            }
            else
            {
                ModelState.AddModelError("Photo", "Please Select Image!");
                return View(dbBlog);
            }
            //bool exist = _db.Blogs.Any(x => x.BlogCreator == blog.BlogCreator && x.Id != id);
            //if (exist)
            //{
            //    ModelState.AddModelError("BlogCreator", "This Name already exist!");
            //    return View(dbBlog);
            //}
            if (blog.BlogCreator == null)
            {
                ModelState.AddModelError("BlogCreator", "Please Enter CreatorName");
                return View(dbBlog);
            }
            dbBlog.BlogDetail.SubTitle = blog.BlogDetail.SubTitle;
            dbBlog.BlogDetail.Title = blog.BlogDetail.Title;
            dbBlog.BlogDetail.Title2 = blog.BlogDetail.Title2;
            dbBlog.BlogDetail.CreateTime = blog.BlogDetail.CreateTime;
            dbBlog.BlogDetail.BlogDesciription = blog.BlogDetail.BlogDesciription;
            dbBlog.BlogDetail.BlogId = blog.Id;
            dbBlog.Id = blog.Id;
            dbBlog.Image = blog.Image;

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
            ;
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog blog = await _db.Blogs.Include(x => x.BlogDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null)
            {
                return BadRequest();
            }
            if (blog.IsDeactive)
            {
                blog.IsDeactive = false;
            }
            else
            {
                blog.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
