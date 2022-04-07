using EduHomeFinal.DAL;
using EduHomeFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Controllers
{
    public class BlogController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        public BlogController(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int page=1)
        {
            ViewBag.Page = page;
            ViewBag.Pagecount=Math.Ceiling((decimal)_db.Blogs.Count()/9);
            List<Blog> blogs = await _db.Blogs.OrderByDescending(x=>x.Id).Skip((page-1)*9).Take(9).Where(x =>! x.IsDeactive).Include(x => x.BlogDetail).ToListAsync();
            return View(blogs);
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            Blog blogs = await _db.Blogs.Include(x => x.BlogDetail).FirstOrDefaultAsync(x => x.Id == id);
            return View(blogs);
        }
        #endregion

        #region BlogSearch
        public async Task<IActionResult> BlogSearch(string keyword4)
        {
            List<Blog> blogs = await _db.Blogs.OrderByDescending(x => x.Id).Where(x => x.BlogCreator.Contains(keyword4) && !x.IsDeactive).Include(x => x.BlogDetail).ToListAsync();

            if (keyword4==null)
            {
                List<Blog> blogs1 = await _db.Blogs.OrderByDescending(x => x.Id).Where(x => x.BlogCreator.Contains(keyword4) && !x.IsDeactive).Include(x => x.BlogDetail).ToListAsync();
                return PartialView("_SearchBlogPartial", blogs1);

            }
            return PartialView("_SearchBlogPartial", blogs);
        }
        #endregion

    }



}
