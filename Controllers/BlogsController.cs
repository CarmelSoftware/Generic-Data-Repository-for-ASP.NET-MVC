using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericRepository.Models;

namespace GenericRepository.Controllers
{
    public class BlogsController : Controller
    {
        private DataRepository db = new DataRepository();

        //
        // GET: /Blogs/

        public ActionResult Index()
        {
            var blogs = db.Retrieve<Blog>(null,null);//.Include(b => b.Blogger);
            return View(blogs.ToList());
        }

        //
        // GET: /Blogs/Details/5

        public ActionResult Details(int id = 0)
        {
            Blog blog = db.Retrieve<Blog>(id,null).SingleOrDefault ();
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        //
        // GET: /Blogs/Create

        public ActionResult Create()
        {
            ViewBag.BloggerID = new SelectList(db.Retrieve<Blogger>(null,null), "BloggerID", "Name");
            return View();
        }

        //
        // POST: /Blogs/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Create<Blog>(blog);
                db.Save(blog, 1); 
                return RedirectToAction("Index");
            }

            ViewBag.BloggerID = new SelectList(db.Retrieve<Blogger>(null, null), "BloggerID", "Name", blog.BloggerID);
            return View(blog);
        }

        //
        // GET: /Blogs/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Blog blog = db.Retrieve<Blog>( id, null).SingleOrDefault ();
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.BloggerID = new SelectList(db.Retrieve<Blogger>(null, null), "BloggerID", "Name", blog.BloggerID);
            return View(blog);
        }

        //
        // POST: /Blogs/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Update<Blog>(blog);
                db.Save(blog,1);
                return RedirectToAction("Index");
            }
            ViewBag.BloggerID = new SelectList(db.Retrieve<Blogger>(null, null), "BloggerID", "Name", blog.BloggerID);
            return View(blog);
        }

        //
        // GET: /Blogs/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Blog blog = db.Retrieve<Blog>(id, null).SingleOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        //
        // POST: /Blogs/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Retrieve<Blog>(id, null).SingleOrDefault();
            db.Delete<Blog>(blog);
            db.Save(blog, 1);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
