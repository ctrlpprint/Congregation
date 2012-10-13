// Place in same namespace as extended class to improve discoverability.
using System.Linq;

namespace System.Collections.Generic
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// The List T has a useful ForEach method. This extension method adds the same capability to the IEnumerable T  class.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		/// <remarks><see cref="http://www.codekeep.net/snippets/e66d1dec-826a-4457-9083-7dc323aad325.aspx"/></remarks>
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action) {
			if (source == null) return null;

			if (action == null)
				throw new ArgumentNullException("action", "The parameter 'action' cannot be null!");

			foreach (var obj in source) {
				action(obj);
			}

			// return source to enable chaining.
			return source;
		}

		/// <summary>
		/// Display a string representation of a collection.
		/// Returns a comma-separated list of elements, each represented by its ToString() value.
		/// </summary>
		public static string ToCommaSeparatedList<T>(this IEnumerable<T> source) {
			return ToCommaSeparatedList(source, t => t.SafeToString());
		}

		/// <summary>
		/// Display a string representation of a collection.
		/// Returns a comma-separated list of elements, each represented by the function passed in the <param name="toString"/> parameter.
		/// </summary>
		public static string ToCommaSeparatedList<T>(this IEnumerable<T> source, Func<T, string> toString) {
			return ToDelimitedList(source, toString, ", ");
		}

		/// <summary>
		/// Display a string representation of a collection.
		/// Returns a comma-separated list of elements, each represented by its ToString() value.
		/// </summary>
		public static string ToPipeSeparatedList<T>(this IEnumerable<T> source) {
			return ToPipeSeparatedList(source, t => t.SafeToString());
		}

		/// <summary>
		/// Display a string representation of a collection.
		/// Returns a comma-separated list of elements, each represented by the function passed in the <param name="toString"/> parameter.
		/// </summary>
		public static string ToPipeSeparatedList<T>(this IEnumerable<T> source, Func<T, string> toString) {
			return ToDelimitedList(source, toString, "|");
		}

		/// Display a string representation of a collection.
		/// Returns a comma-separated list of elements, each represented by the function passed in the <param name="toString"/> parameter.
		/// </summary>
		public static string ToDelimitedList<T>(this IEnumerable<T> source, string delimiter) {
			return ToDelimitedList(source, s => s.SafeToString(), delimiter);
		}

		/// <summary>
		/// Display a string representation of a collection.
		/// Returns a comma-separated list of elements, each represented by the function passed in the <param name="toString"/> parameter.
		/// </summary>
		public static string ToDelimitedList<T>(this IEnumerable<T> source, Func<T, string> toString, string delimiter) {
			return source.Aggregate("",
				(soFar, next) => next.SafeToString().IsNotEmpty()
					? soFar.IsNotEmpty()
						? string.Format("{0}{1}{2}", soFar, delimiter, toString(next))
						: toString(next)
					: soFar
				);
		}

		public static bool IsEmpty<T>(this IEnumerable<T> source) {
			// Potential issue with this code?
			// http://haacked.com/archive/2010/06/10/checking-for-empty-enumerations.aspx
			// http://ayende.com/Blog/archive/2010/06/10/checking-for-empty-enumerations.aspx
			// Ayende alleges it may cause the first item to be omitted in subsequent uses of the enumerable.
			return source == null || !source.Any();
		}

		/// <summary>
		/// Pivot function, works similarly to Excel Pivot Tables.
		/// Pivots a flat list (source) on key1Selector (=> rows) and 
		/// </summary>
		/// <remarks>
		/// From http://www.extensionmethod.net/Details.aspx?ID=147
		/// </remarks>
		public static Dictionary<TKey1, Dictionary<TKey2, TValue>> Pivot<TSource, TKey1, TKey2, TValue>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey1> key1Selector,
			Func<TSource, TKey2> key2Selector,
			Func<IEnumerable<TSource>, TValue> aggregate) {

			return source.GroupBy(key1Selector).Select(x => new {
				X = x.Key,
				Y = x.GroupBy(key2Selector).Select(
					z => new {
						Z = z.Key,
						V = aggregate(z)
					}
				).ToDictionary(e => e.Z, o => o.V)
			}
			).ToDictionary(e => e.X, o => o.Y);
		}

		/// <summary>
		/// Adds sub-totals to a list of items, along with a grand total for the whole list.
		/// </summary>
		/// <param name="elements">Group and/or sort this yourself before calling WithRollup.</param>
		/// <param name="primaryKeyOfElement">Given a TElement, return the property that you want sub-totals for.</param>
		/// <param name="calculateSubTotalElement">Given a group of elements, return a TElement that represents the sub-total.</param>
		/// <param name="grandTotalElement">A TElement that represents the grand total.</param>
		/// <remarks>
		/// Source http://stackoverflow.com/questions/1343487/linq-to-sql-version-of-group-by-with-rollup/1401623#1401623
		/// </remarks>
		public static List<TElement> WithRollup<TElement, TKey>(
			this IEnumerable<TElement> elements,

			Func<TElement, TKey> primaryKeyOfElement,
			Func<IGrouping<TKey, TElement>, TElement> calculateSubTotalElement,
			TElement grandTotalElement) {

			// Create a new list the items, subtotals, and the grand total.
			List<TElement> results = new List<TElement>();
			var lookup = elements.ToLookup(primaryKeyOfElement);
			foreach (var group in lookup) {
				// Add items in the current group
				results.AddRange(group);
				// Add subTotal for current group
				results.Add(calculateSubTotalElement(group));
			}
			// Add grand total
			results.Add(grandTotalElement);

			return results;
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
			(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
			// Source: http://stackoverflow.com/questions/489258/linq-distinct-on-a-particular-property

			var seenKeys = new HashSet<TKey>();
			foreach (TSource element in source) {
				if (seenKeys.Add(keySelector(element))) {
					yield return element;
				}
			}
		}
	}
}
