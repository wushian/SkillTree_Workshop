using System.Web;
using System.Web.Mvc;
using Workshop.Infrastructure.Filters;

namespace Workshop
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new ElmahHandledErrorLoggerFilter());
			filters.Add(new HandleErrorAttribute());
		}
	}
}
