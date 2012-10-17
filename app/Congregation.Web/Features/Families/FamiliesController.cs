using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Congregation.Application.Data;
using Congregation.Core.Models.Families;
using NHibernate.Criterion;
using NHibernate.Transform;
using PetaPoco;

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

			var neighbours = GetNeighbours(family);
    		var familes = repository.Session.QueryOver<Family>()
    			.Where(f => f.Id.IsIn(neighbours.Select(n => n.FamilyId).ToArray()))
    			.Fetch(f => f.Address).Eager
    			.Fetch(f => f.Contacts).Eager
    			.TransformUsing(Transformers.DistinctRootEntity)
    			.List();

    		foreach (var neighbour in neighbours) {
    			neighbour.Family = familes.Single(f => f.Id == neighbour.FamilyId);
    		}
    		return View(new ViewFamilyViewModel(family, neighbours));
        }

    	private List<NearestNeighbour> GetNeighbours(Family family) {
			// Using PetaPoco rather than NHibernate because
			// - Writing a sproc to query a sql geography type looks a heck of a lot easier than upgrading
			//	 NHibernate Spatial to work with NH 3.3
			//	 See http://build-failed.blogspot.co.nz/2012/02/nhibernate-spatial-part-1.html
			// - NH Mapping-by-Code doesn't support Named Queries/Sprocs. Configuration has an AddNamedQuery
			//	 method, but it's only implemented for querying in-memory.
			//	 See https://groups.google.com/forum/?fromgroups=#!topic/nhusers/gxi225tG8aI
			// - NH Mapping-by-Code does support mixing conventions with hbm.xml files, but to do so you need
			//	 to add an xml configuration file. Since this would duplicate the fluent configuration, it's
			//	 not clear what this achieves or what needs to go in it.

    		var databaseContext = new Database("DefaultConnection")
    		{
    			EnableAutoSelect = false // otherwise it'll put "select..." in front of "exec..."
    		};

    		var sql = string.Format("exec GetNearestNeighbours {0}, {1}",
    		                        family.Address.Id, // @AddressID
    		                        5); // @NumberOfNeighbours
    		var neighbours = databaseContext.Query<NearestNeighbour>(sql).ToList();
    		return neighbours;
    	}
    }
}
