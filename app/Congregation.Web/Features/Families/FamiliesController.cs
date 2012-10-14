using System.Web.Mvc;
using Congregation.Application.Data;
using Congregation.Core.Models.Families;

namespace Congregation.Web.Features.Families
{
    public class FamiliesController : CommonController
    {
    	private readonly IRepository repository;

    	public FamiliesController(IRepository repository) {
			this.repository = repository;
		}

    	public ActionResult View(int id) {
    		var family = repository.Get<Family>(id);

            return View(family);
        }

    }
}
