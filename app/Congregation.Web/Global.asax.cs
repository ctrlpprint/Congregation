using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Congregation.Application.Infrastructure;
using Congregation.Web.Mvc.Binders;
using Congregation.Web.Mvc.Config;

namespace Congregation.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			DependencyResolverInitializer.Initialize();

			ModelBinders.Binders.DefaultBinder = new SharpModelBinder();
			// Custom types, add binders as follows
			//ModelBinders.Binders.Add(typeof(Money), new MoneyBinder());
		}
	}
}