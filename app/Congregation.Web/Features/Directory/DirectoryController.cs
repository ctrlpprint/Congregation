using System.Linq;
using System.Web.Mvc;
using Congregation.Application.Data;
using Congregation.Core.Models.Families;
using NHibernate.Transform;

namespace Congregation.Web.Features.Directory
{
    public class DirectoryController : CommonController
    {
    	private readonly IRepository repository;

    	public DirectoryController(IRepository repository) {
    		this.repository = repository;
    	}

		public ActionResult Index() {
			var members = repository.Session.QueryOver<Contact>()
				.Fetch(c => c.Family).Eager
				.List();

			// There should be a way of doing this all via QueryOver, but 
			// it looks like there isn't.
			// http://stackoverflow.com/questions/7723461/can-nhibernate-queryover-load-dtos-with-associations
			var members2 = members
				.Select(c => new MemberStub()
				{
					Name = c.FirstName + " " + c.Family.FamilyName,
					ContactId = c.Id,
					FamilyId = c.Family.Id
				})
				.OrderBy(m => m.Name).ToList();

			return View(new DirectorySearchViewModel(){Members = members2});
		}

    	public ActionResult Full() {
    		var families = repository.Session.QueryOver<Family>()
    			.Fetch(f => f.Contacts).Eager
    			.Fetch(f => f.Address).Eager
				.TransformUsing(Transformers.DistinctRootEntity)
    			.OrderBy(f => f.FamilyName).Asc
    			.List();
				
				
            return View(families);
        }

		public ActionResult Map() {
			var families = repository.Session.QueryOver<Family>()
				.Fetch(f => f.Contacts).Eager
				.Fetch(f => f.Address).Eager
				.TransformUsing(Transformers.DistinctRootEntity)
				.OrderBy(f => f.FamilyName).Asc
				.List();


			return View(families);
		}

    }
}
