using System.Web.Mvc;

namespace Congregation.Web.Mvc.Config
{
	public class FeatureBasedViewEngine : RazorViewEngine
	{
		public FeatureBasedViewEngine()
			: base() {

			// 0 = View
			// 1 = Controller
			// 2 = Area

			ViewLocationFormats = new[]
			{
				"~/Features/{1}/{0}.cshtml",
			};

			MasterLocationFormats = new[]
			{
				"~/Features/{0}/{1}.cshtml",
				"~/Features/Shared/{1}.cshtml",
				"~/Shared/Views/{1}.cshtml",
			};

			PartialViewLocationFormats = new[]
			{
				"~/Features/{1}/{0}.cshtml",
				"~/Features/Shared/{0}.cshtml",
			};

			//AreaViewLocationFormats = new[]
			//{
			//    "~/Areas/{2}/Features/{1}/Views/{0}.cshtml",
			//};

			//AreaMasterLocationFormats = new[]
			//{
			//    "~/Areas/{2}/Features/{0}/Views/{1}.cshtml",
			//    "~/Areas/{2}/Features/Shared/Views/{1}.cshtml",
			//};

			//AreaPartialViewLocationFormats = new[]
			//{
			//    "~/Areas/{2}/Features/{0}/Views/{1}.cshtml",
			//    "~/Areas/{2}/Features/Shared/Views/{1}.cshtml",
			//};
		}
	}
}