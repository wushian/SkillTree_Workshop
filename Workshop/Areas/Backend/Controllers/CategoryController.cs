using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Workshop.Helpers;
using Workshop.Models;
using Workshop.ViewModels;

namespace Workshop.Areas.Backend.Controllers
{
	[Authorize]
    public class CategoryController : Controller
    {
        private WorkshopEntities db = new WorkshopEntities();

        // GET: Backend/Category
        public ActionResult Index(QueryOption<Category> queryOption)
		{
			var query = db.Categories.AsQueryable();

            if (string.IsNullOrEmpty(queryOption.Keyword) == false)
            {
                query = query.Where(x => x.Name.Contains(queryOption.Keyword));
            }

		    queryOption.SetSource(query);

			return View(queryOption);
        }

        // GET: Backend/Category/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Backend/Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Category/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.ID = Guid.NewGuid();

				category.CreateUser = WebSiteHelper.CurrentUserID;
				category.CreateDate = DateTime.Now;
				category.UpdateDate = DateTime.Now;


                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Backend/Category/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Backend/Category/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
			var isExists = db.Categories
				.Any(x => x.ID != category.ID && x.Name == category.Name);

			if (isExists)
			{
				ModelState.AddModelError("Name", "Category Name Double");
				return View(category);
			}

            if (ModelState.IsValid)
            {
				var target = db.Categories.FirstOrDefault(x=>x.ID == category.ID);

				if (target == null)
				{
					ModelState.AddModelError("ID", "Is Not Exists.");
					return View(category);
				}

				target.Name = category.Name;
				target.UpdateUser = WebSiteHelper.CurrentUserID;
				target.UpdateDate = DateTime.Now;

                db.Entry(target).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Backend/Category/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Backend/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(Guid id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
