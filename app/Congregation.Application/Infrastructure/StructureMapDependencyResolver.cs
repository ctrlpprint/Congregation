using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace Congregation.Application.Infrastructure
{
	/// <summary>
	/// Taken from http://stevesmithblog.com/blog/how-do-i-use-structuremap-with-asp-net-mvc-3/ via SharpLite
	/// </summary>
	public class StructureMapDependencyResolver : IDependencyResolver
	{
		public StructureMapDependencyResolver(IContainer container) {
			this.container = container;
		}

		public object GetService(Type serviceType) {
			return serviceType.IsAbstract || serviceType.IsInterface
			       	? container.TryGetInstance(serviceType)
			       	: container.GetInstance(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType) {
			return container.GetAllInstances(serviceType).Cast<object>();
		}

		private readonly IContainer container;
	}
}