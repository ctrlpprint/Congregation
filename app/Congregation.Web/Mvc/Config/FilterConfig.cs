﻿using System.Web.Mvc;

namespace Congregation.Web.Mvc.Config
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new HandleErrorAttribute());
		}
	}
}