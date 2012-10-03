using System;
using System.Linq;
using System.Web.Mvc;

namespace Congregation.Web.Mvc.Binders
{
	public class EntityCollectionValueBinder : DefaultModelBinder
	{
		/// <summary>
		/// Binds the model to a value by using the specified controller context and binding context.
		/// </summary>
		/// <returns>
		/// The bound value.
		/// </returns>
		/// <param name = "controllerContext">The controller context.</param>
		/// <param name = "bindingContext">The binding context.</param>
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
			var collectionType = bindingContext.ModelType;
			var collectionEntityType = collectionType.GetGenericArguments().First();

			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

			if (valueProviderResult == null) {
				return base.BindModel(controllerContext, bindingContext);
			}

			var rawValue = valueProviderResult.RawValue as string[];

			var countOfEntityIds = rawValue.Length;
			var entities = Array.CreateInstance(collectionEntityType, countOfEntityIds);

			for (var i = 0; i < countOfEntityIds; i++) {
				string rawId = rawValue[i];

				if (string.IsNullOrEmpty(rawId)) {
					return null;
				}

				object typedId = Convert.ChangeType(rawId, typeof(int));
				object entity = EntityRetriever.GetEntityFor(collectionEntityType, typedId, typeof(int));
				entities.SetValue(entity, i);
			}

			return entities;
		}
	}
}