using System;
using System.Linq;
using System.Web.Mvc;

namespace Congregation.Web.Mvc.Binders
{
	public class EntityValueBinder : SharpModelBinder
	{
		/// <summary>
		/// Binds the model value to an entity by using the specified controller context and binding context.
		/// </summary>
		/// <returns>
		/// The bound value.
		/// </returns>
		/// <param name = "controllerContext">The controller context.</param>
		/// <param name = "bindingContext">The binding context.</param>
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
			var modelType = bindingContext.ModelType;

			// Will look for the entity Id either named "ModelName" or "ModelName.Id"
			var valueProviderResult =
				bindingContext.ValueProvider.GetValue(bindingContext.ModelName) ??
				bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".Id");

			if (valueProviderResult == null) {
				return base.BindModel(controllerContext, bindingContext);
			}

			string rawId = (valueProviderResult.RawValue as string[]).First();

			if (string.IsNullOrEmpty(rawId)) {
				return null;
			}

			try {
				var typedId = Convert.ChangeType(rawId, typeof(int));
				return EntityRetriever.GetEntityFor(modelType, typedId, typeof(int));
			}
			catch (Exception) {
				// If the Id conversion failed for any reason, just return null
				return null;
			}
		}
	}
}