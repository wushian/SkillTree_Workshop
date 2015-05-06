using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using Workshop.Infrastructure.CustomResults;
using Workshop.Models;

namespace Workshop.Controllers
{
    public class FeedController : Controller
    {
		private WorkshopEntities db = new WorkshopEntities();

		public ActionResult Rss()
		{
			var feed = this.GetFeedData();
			return new RssActionResult(feed);
		}

		private SyndicationFeed GetFeedData()
		{
			string hostUrl = string.Format("{0}://{1}", Request.Url.Scheme, Request.Headers["host"]);

			SyndicationFeed feed = new SyndicationFeed(
				"SkillTree Workshop",
				"This is a feed from SkillTree Workshop",
				new Uri(string.Concat(hostUrl, "/Rss/")));

			List<SyndicationItem> items = new List<SyndicationItem>();

			var articles = db.Articles
				.Where(x => x.IsPublish && x.PublishDate <= DateTime.Now)
				.OrderByDescending(x => x.CreateDate);

			foreach (var article in articles)
			{
				SyndicationItem item = new SyndicationItem(
					article.Subject,
					article.Summary,
					new Uri(string.Concat(hostUrl, "/Article/Details?id=", article.ID)),
					"ID",
					DateTime.Now);

				items.Add(item);
			}

			feed.Items = items;
			return feed;
		}
    }
}