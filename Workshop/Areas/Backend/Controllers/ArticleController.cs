using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Workshop.Helpers;
using Workshop.Models;
using Workshop.ViewModels;

namespace Workshop.Areas.Backend.Controllers
{
	[Authorize]
	public class ArticleController : Controller
	{
		private WorkshopEntities db = new WorkshopEntities();

		public ActionResult Index(QueryOption<Article> queryOption)
		{
			var query = db.Articles.Include(a => a.Category);

			if (string.IsNullOrEmpty(queryOption.Keyword) == false)
			{
				query = query.Where(x => x.Subject.Contains(queryOption.Keyword)
										 ||
										 x.Category.Name.Contains(queryOption.Keyword)
										 ||
										 x.ContentText.Contains(queryOption.Keyword));
			}

			queryOption.SetSource(query);

			return View(queryOption);
		}

		// GET: Backend/Articles/Details/5
		public ActionResult Details(Guid? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Article article = db.Articles.Find(id);
			if (article == null)
			{
				return HttpNotFound();
			}
			return View(article);
		}

		// GET: Backend/Articles/Create
		public ActionResult Create()
		{
			ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
			return View();
		}

		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Article article, HttpPostedFileBase[] uploads)
		{
			CheckFiles(uploads);

			if (ModelState.IsValid)
			{
				article.ID = Guid.NewGuid();
				article.CreateUser = WebSiteHelper.CurrentUserID;
				article.CreateDate = DateTime.Now;
				article.UpdateDate = DateTime.Now;

				db.Articles.Add(article);

				HandleFiles(article, uploads);

				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", article.CategoryID);
			return View(article);
		}

		public ActionResult Edit(Guid? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Article article = db.Articles.Find(id);
			if (article == null)
			{
				return HttpNotFound();
			}
			ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", article.CategoryID);
			return View(article);
		}

		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Article article, HttpPostedFileBase[] uploads)
		{
			CheckFiles(uploads);

			if (ModelState.IsValid)
			{
				var instance = db.Articles.FirstOrDefault(x => x.ID == article.ID);

				instance.CategoryID = article.CategoryID;
				instance.Subject = article.Subject;
				instance.Summary = article.Summary;
				instance.ContentText = article.ContentText;
				instance.PublishDate = article.PublishDate;
				instance.IsPublish = article.IsPublish;

				instance.UpdateUser = WebSiteHelper.CurrentUserID;
				instance.UpdateDate = DateTime.Now;

				db.Entry(instance).State = EntityState.Modified;

				HandleFiles(article, uploads);

				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", article.CategoryID);
			return View(article);
		}

		// GET: Backend/Articles/Delete/5
		public ActionResult Delete(Guid? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Article article = db.Articles.Find(id);
			if (article == null)
			{
				return HttpNotFound();
			}
			return View(article);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(Guid id)
		{
			Article article = db.Articles.Find(id);

			//刪除圖片
			var photos = article.Photos.ToList();
			foreach (var photo in photos)
			{
				this.DeletePhoto(photo.ID);
			}

			db.Articles.Remove(article);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		private void CheckFiles(HttpPostedFileBase[] uploads)
		{
			if (uploads != null)
			{
				for (int i = 0; i < uploads.Length; i++)
				{
					var upload = uploads[i];

					//檢查附檔名
					//檢查檔案格式（只檢查附檔名並不安全）
					if (upload != null && !Regex.IsMatch(upload.FileName, @"\.(jpg|jpeg|gif|png)$"))
					{
						ModelState.AddModelError("Uploads[" + i + "]", "僅可上傳圖片檔");
					}
				}
			}
		}

		private void HandleFiles(Article article, HttpPostedFileBase[] uploads)
		{
			var timeStamp = DateTime.Now;

			//在 Create 的時候，自己 new 出來的 Article 物件 Photo 屬性尚未被初始化
			if (article.Photos == null)
			{
				article.Photos = new List<Photo>();
			}

			//逐一處理上傳檔案
			foreach (var upload in uploads)
			{
				if (upload == null)
				{
					continue;
				}

				//相同檔名已上傳記錄
				var photo = article.Photos.FirstOrDefault(x => x.FileName == upload.FileName);

				if (photo == null)
				{
					photo = new Photo
					{
						ID = Guid.NewGuid(),
						ArticleID = article.ID,
						FileName = upload.FileName,
						CreateDate = timeStamp,
						UpdateDate = timeStamp
					};
					db.Photos.Add(photo);
					article.Photos.Add(photo);
				}
				else
				{
					photo.UpdateDate = timeStamp;
					db.Entry(photo).State = EntityState.Modified;
				}

				//指定檔案存放位置
				var path = Server.MapPath(string.Concat("~/Uploads/", article.ID, "/"));

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				upload.SaveAs(Path.Combine(path, photo.FileName));
			}
		}

		public ActionResult ArticlePhoto(Guid id, int w, int h)
		{
			var photo = db.Photos.FirstOrDefault(x => x.ID == id);
			if (photo == null)
			{
				return Content(string.Concat("http://placehold.it/", w, "x", h));
			}
			var filePath = Server.MapPath(string.Concat(
				"~/Uploads/", 
				photo.ArticleID, 
				"/", 
				photo.FileName));

			var image = new WebImage(filePath).Resize(w, h);

			return File(image.GetBytes(), "image/jpeg");
		}

		public ActionResult DeletePhoto(Guid id)
		{
			var photo = db.Photos.FirstOrDefault(x => x.ID == id);

			db.Photos.Remove(photo);
			db.SaveChanges();

			var path = Server.MapPath(string.Concat(
				"~/Uploads/", photo.ArticleID, "/"));

			var filePath = Server.MapPath(string.Concat(
				"~/Uploads/", photo.ArticleID, "/", photo.FileName));

			System.IO.File.Delete(filePath);

			if (Directory.EnumerateFiles(path).Any() == false)
			{
				System.IO.Directory.Delete(path);
			}

			return new EmptyResult();
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
