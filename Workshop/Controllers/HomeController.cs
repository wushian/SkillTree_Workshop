using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workshop.Models;

namespace Workshop.Controllers
{
	public class HomeController : Controller
	{
		private WorkshopEntities db = new WorkshopEntities();

		private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public ActionResult Index()
		{
			//最新五篇文章
			var articles =
				db.Articles.Where(x => x.IsPublish && x.PublishDate <= DateTime.Now)
				  .OrderByDescending(x => x.CreateDate)
				  .Take(5);

			return View(articles.ToList());
		}

		public ActionResult Demo()
		{
			try
			{
				int a = 1;
				int b = 0;
				int result = a / b;
			}
			catch (Exception ex)
			{
				logger.Fatal(ex);
			}

			return View();
		}

	}
}