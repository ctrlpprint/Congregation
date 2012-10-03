using System;
using System.Web.Mvc;
using Congregation.Application.Data;

namespace Congregation.Web.Mvc.Binders
{
	internal class EntityRetriever
	{
		internal static object GetEntityFor(Type entityType, object typedId, Type idType) {
			var repository = DependencyResolver.Current.GetService(typeof (IRepository));

			return repository.GetType().GetMethod("Get").MakeGenericMethod(entityType).Invoke(
				repository, new[] { typedId });
		}

	}
}