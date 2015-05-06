using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Workshop.Areas.Backend.Models;
using Workshop.Models;

namespace Workshop.Areas.Backend.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		private WorkshopEntities db = new WorkshopEntities();

		[AllowAnonymous]
		public ActionResult Logon()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Logon(LogonViewModel logonModel)
		{
			if (ModelState.IsValid)
			{
				var systemuser = 
					db.SystemUsers.FirstOrDefault(x => x.Account == logonModel.Account);

				if (systemuser == null)
				{
					ModelState.AddModelError("", "請輸入正確的帳號或密碼!");
				}
				else
				{
					var password = CryptographyPassword(logonModel.Password, systemuser.Salt);

					if (systemuser.Password != password)
					{
						return View();
					}

					var now = DateTime.Now;

					var ticket = new FormsAuthenticationTicket(
						1,
						systemuser.Name,
						now,
						now.AddMinutes(30),
						logonModel.Remember,
						systemuser.ID.ToString(),
						FormsAuthentication.FormsCookiePath);

					var encryptedTicket = FormsAuthentication.Encrypt(ticket);
					var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
					Response.Cookies.Add(cookie);

					return RedirectToAction("Index", "Home");
				}
			}

			return View();
		}

		private string CryptographyPassword(string password, string salt)
		{
			string cryptographyPassword =
				FormsAuthentication.HashPasswordForStoringInConfigFile(password + salt, "sha1");

			return cryptographyPassword;
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("Logon", "Home");
		}

	}
}