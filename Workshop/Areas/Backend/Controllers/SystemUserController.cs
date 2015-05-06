using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Workshop.Helpers;
using Workshop.Models;
using Workshop.ViewModels;

namespace Workshop.Areas.Backend.Controllers
{
	[Authorize]
    public class SystemUserController : Controller
    {
        private WorkshopEntities db = new WorkshopEntities();

        // GET: Backend/SystemUser
        public ActionResult Index(QueryOption<SystemUser> queryOption)
		{
			var query = db.SystemUsers.AsQueryable();

            if (string.IsNullOrEmpty(queryOption.Keyword) == false)
            {
                query = query.Where(x => x.Name.Contains(queryOption.Keyword)
                                         ||
                                         x.Account.Contains(queryOption.Keyword)
                                         ||
                                         x.Email.Contains(queryOption.Keyword));
            }

		    queryOption.SetSource(query);

			return View(queryOption);
        }

        // GET: Backend/SystemUser/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(SystemUser systemUser)
		{
			//檢查登入帳號是否重複
			if (db.SystemUsers.Any(x => x.Account == systemUser.Account))
			{
				ModelState.AddModelError("Account", "登入帳號不可重複");
				return View(systemUser);
			}

			if (ModelState.IsValid)
			{
				systemUser.ID = Guid.NewGuid();

				systemUser.Salt = GenerateSalt();
				systemUser.Password = CryptographyPassword(systemUser.Password, systemUser.Salt);

				systemUser.CreateUser = WebSiteHelper.CurrentUserID;
				systemUser.CreateDate = DateTime.Now;
				systemUser.UpdateDate = DateTime.Now;

				db.SystemUsers.Add(systemUser);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(systemUser);
		}

		private string GenerateSalt()
		{
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] buf = new byte[16];
			rng.GetBytes(buf);
			return Convert.ToBase64String(buf);
		}

		private string CryptographyPassword(string password, string salt)
		{
			string cryptographyPassword =
				FormsAuthentication.HashPasswordForStoringInConfigFile(password + salt, "sha1");

			return cryptographyPassword;
		}

		public ActionResult Edit(Guid id)
		{
			SystemUser systemUser = db.SystemUsers.Find(id);
			if (systemUser == null)
			{
				return HttpNotFound();
			}
			return View(systemUser);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(SystemUser systemUser)
		{
			//檢查登入帳號是否重複
			if (db.SystemUsers.Any(x => x.ID != systemUser.ID && x.Account == systemUser.Account))
			{
				ModelState.AddModelError("Account", "登入帳號不可重複");
				return View(systemUser);
			}

			if (ModelState.IsValid)
			{
				SystemUser user = db.SystemUsers.FirstOrDefault(x => x.ID == systemUser.ID);

				if (!string.IsNullOrWhiteSpace(systemUser.Password))
				{
					user.Salt = GenerateSalt();
					user.Password = CryptographyPassword(systemUser.Password, user.Salt);
				}

				user.Name = systemUser.Name;
				user.Account = systemUser.Account;
				user.Email = systemUser.Email;

				user.UpdateUser = WebSiteHelper.CurrentUserID;
				user.UpdateDate = DateTime.Now;

				db.Entry(user).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(systemUser);
		}

        // GET: Backend/SystemUser/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // POST: Backend/SystemUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SystemUser systemUser = db.SystemUsers.Find(id);
            db.SystemUsers.Remove(systemUser);
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
