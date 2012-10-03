using System.Web.Mvc;
using Congregation.Application.Data;
using Congregation.Application.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace Congregation.Application.Infrastructure
{
	/// <summary>
	/// Adapted from SharpLite
	/// </summary>
	public class DependencyResolverInitializer
	{
		public static void Initialize() {
			var container = new Container(x => {
				x.For<ISessionFactory>()
					.Singleton()
					.Use(() => NHibernateInitializer.Initialize().BuildSessionFactory());
				x.For<IEntityDuplicateChecker>().Use<EntityDuplicateChecker>();
				x.For(typeof(IRepository)).Use(typeof(Repository));
			});

			DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
		}

	}
}