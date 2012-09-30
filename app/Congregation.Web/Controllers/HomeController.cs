using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Congregation.Web.Controllers
{
	public class HomeController : Controller
	{
		// GET: /Home/
		public ActionResult Index() {
			return View();
		}

		// GET: /Home/Details/5
		//public ActionResult Details(int id) {
		//    return View();
		//}

		// GET: /Home/Create
		//public ActionResult Create() {
		//    return View();
		//}

		// POST: /Home/Create
		//[HttpPost]
		//public ActionResult Create(FormCollection collection) {
		//    try {
		//        // TODO: Add insert logic here

		//        return RedirectToAction("Index");
		//    }
		//    catch {
		//        return View();
		//    }
		//}

	}
}