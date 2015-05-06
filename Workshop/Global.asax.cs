using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Workshop.Controllers;

namespace Workshop
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			// 發生未處理錯誤時執行的程式碼

			var app = (MvcApplication)sender;
			var ex = app.Server.GetLastError();

			var context = app.Context;
			context.Response.Clear();
			context.ClearError();

			var httpException = ex as HttpException;
			if (httpException == null)
			{
				httpException = new HttpException(null, ex);
			}

			var routeData = new RouteData();

			routeData.Values["controller"] = "Errors";
			routeData.Values["action"] = "Index";

			routeData.Values["exception"] = ex;
			routeData.Values["from_Application_Error_Event"] = true;

			if (httpException != null)
			{
				switch (httpException.GetHttpCode())
				{
					case 404:
						routeData.Values["action"] = "PageNotFound";
						break;
					default:
						routeData.Values["action"] = "Index";
						break;
				}
			}

			// Pass exception details to the target error View.
			routeData.Values.Add("error", ex.Message);

			// Avoid IIS7 getting in the middle
			context.Response.TrySkipIisCustomErrors = true;
			IController controller = new ErrorsController();
			controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
		}

		void ErrorLog_Filtering(object sender, Elmah.ExceptionFilterEventArgs e)
		{
			if (e.Exception.GetBaseException() is HttpRequestValidationException)
			{
				e.Dismiss();
			}

			var httpException = e.Exception as HttpException;
			if (httpException != null && httpException.GetHttpCode() == 404)
			{
				e.Dismiss();
			}
		}

	}
}
