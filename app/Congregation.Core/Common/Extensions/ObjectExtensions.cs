using System.Collections.Generic;
using System.Reflection;

namespace System
{
	public static class ObjectExtensions
	{
		public static bool IsNull(this object obj) {
			return obj == null;
		}

		public static bool IsNotNull(this object obj) {
			return obj != null;
		}

		public static string SafeToString(this object obj) {
			return obj.IsNull() ? String.Empty : obj.ToString();
		}

		public static void IsRequired(this object obj, string variableName) {
			if (obj == null)
				throw new ArgumentNullException(variableName);
		}

		public static T As<T>(this object obj) {
			if (obj.IsNull()) return default(T);
			if (!(obj is T)) return default(T);
			return (T)obj;
		}

		public static IList<T> InList<T>(this T t) {
			return new List<T> { t };
		}


		#region Reflection
		// Source http://stackoverflow.com/questions/721441/c-how-to-iterate-through-classes-fields-and-set-properties/721761#721761
		private class TargetProperty
		{
			public object Target { get; set; }
			public PropertyInfo Property { get; set; }

			public bool IsValid { get { return Target != null && Property != null; } }
		}

		private static TargetProperty GetTargetProperty(object source, string propertyName) {
			if (!propertyName.Contains("."))
				return new TargetProperty { Target = source, Property = source.GetType().GetProperty(propertyName) };

			string[] propertyPath = propertyName.Split('.');

			var targetProperty = new TargetProperty();

			targetProperty.Target = source;
			targetProperty.Property = source.GetType().GetProperty(propertyPath[0]);

			// Prevent NRE below.  
			if (targetProperty.Property == null || targetProperty.Target == null)
				return targetProperty;

			for (int propertyIndex = 1; propertyIndex < propertyPath.Length; propertyIndex++) {
				propertyName = propertyPath[propertyIndex];
				if (!propertyName.IsNullOrEmpty()) {
					targetProperty.Target = targetProperty.Property.GetValue(targetProperty.Target, null);
					targetProperty.Property = targetProperty.Target.GetType().GetProperty(propertyName);
				}
			}

			return targetProperty;
		}


		public static bool HasProperty(this object source, string propertyName) {
			return GetTargetProperty(source, propertyName).Property != null;
		}

		public static object GetPropertyValue(this object source, string propertyName) {
			var targetProperty = GetTargetProperty(source, propertyName);
			if (targetProperty.IsValid) {
				return targetProperty.Property.GetValue(targetProperty.Target, null);
			}
			return null;
		}

		public static void SetPropertyValue(this object source, string propertyName, object value) {
			var targetProperty = GetTargetProperty(source, propertyName);
			if (targetProperty.IsValid) {
				targetProperty.Property.SetValue(targetProperty.Target, value, null);
			}
		}
		#endregion Reflection
	}
}