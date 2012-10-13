using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

    	public ActionResult Full() {
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
