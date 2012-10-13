using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System
{
	public static class StringExtensions
	{
		public static bool IsNullOrEmpty(this string value) {
			return string.IsNullOrEmpty(value);
		}

		public static bool IsNullOrOnlyContainsSpaces(this string value) {
			return string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim());
		}

		public static bool IsNotEmpty(this string value) {
			return !IsNullOrOnlyContainsSpaces(value);
		}

		public static string Left(this string value, int characters) {
			if (value.IsNullOrEmpty())
				return value;
			if (value.Length <= characters)
				return value;
			return value.Substring(0, characters);
		}

		public static string Right(this string value, int characters) {
			if (value.IsNullOrEmpty())
				return value;
			if (characters < 0)
				return value.Substring(-characters, value.Length + characters);
			if (value.Length <= characters)
				return value;
			return value.Substring(value.Length - characters);
		}

		public static string Remove(this string value, string characters) {
			return value.Replace(characters, string.Empty);
		}

		/// <summary>
		/// returns part of string before subString.  If subString is not found, returns the whole string.
		/// </summary>
		public static string Before(this string value, string subString) {
			int index = value.IndexOf(subString);
			if (index < 0) return value;
			return value.Substring(0, index);
		}

		/// <summary>
		/// returns part of string after subString.  If subString is not found, returns an empty string.
		/// </summary>
		public static string After(this string value, string subString) {
			int index = value.IndexOf(subString);
			if (index < 0) return string.Empty;
			return value.Right(value.Length - subString.Length - index);
		}

		public static bool IsAnInteger(this string value) {
			int intValue;
			return int.TryParse(value, out intValue);
		}


		public static int ToInt(this string value) {
			return value.IsAnInteger() ? int.Parse(value) : 0;
		}

		public static bool IsAmong(this string value, IEnumerable<string> values) {
			return values.Any(val => value == val);
		}

		/// <summary>
		/// Validates the string looks like an email format.
		/// </summary>
		/// <remarks>
		/// This is very liberal in what it accepts. Trying to be too precise tends to result
		/// in false negatives, which are much more damaging than false positives.
		/// </remarks>
		public static bool IsAnEmail(this string value) {
			if (string.IsNullOrEmpty(value))
				return false;

			// http://www.regular-expressions.info/email.html
			// Barfs on apostrophes.
			// const string regex = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";

			// http://stackoverflow.com/a/1457579/424788
			//const string regex = @"^[^.].*@(?:[-a-z0-9]+\.)+[-a-z0-9]+$";

			// or best bet, the one used by asp.net http://stackoverflow.com/questions/1710505/asp-net-email-validator-regex
			const string regex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

			return Regex.Match(value, regex, RegexOptions.IgnoreCase).Success;
		}

		public static bool IsACreditCardNumber(this string value) {
			if (string.IsNullOrEmpty(value))
				return false;

			// http://www.regular-expressions.info/creditcard.html
			const string regex = @"^(?:\d[ -]*?){13,16}$";

			return Regex.Match(value, regex, RegexOptions.IgnoreCase).Success;
		}

		public static string GetDigitsOnly(this string value) {
			return Regex.Replace(value, @"[^\d]", "");
		}

		/// <summary>
		/// Formats the passed string with the passed values
		/// </summary>
		/// <param name="inStr">A string containing placeholders</param>
		/// <param name="args">Values for the placeholders</param>
		/// <returns></returns>
		public static string With(this string inStr, params object[] args) {
			return string.Format(inStr, args);
		}

		/// <summary>
		/// Splits "TitleCase" to "Title Case"
		/// </summary>
		public static string SplitTitleCase(this string inputString) {
			// http://weblogs.asp.net/jgalloway/archive/2005/09/27/426087.aspx - read through comments
			return Regex.Replace(inputString, @"([A-Z][A-Z]*)", @" $1").Trim();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="numberToConvert"></param>
		/// <returns></returns>
		/// <remarks>
		/// See: http://byatool.com/index.php/utilities/duck-typing-my-way-to-a-universal-string-convert
		/// </remarks>
		public static T? ConvertTo<T>(this String numberToConvert) where T : struct {
			T? returnValue = null;

			MethodInfo neededInfo = GetCorrectMethodInfo(typeof(T));

			if (neededInfo != null && !numberToConvert.IsNullOrEmpty()) {
				T output = default(T);
				object[] paramsArray = new object[2] { numberToConvert, output };
				returnValue = new T();

				object returnedValue = neededInfo.Invoke(returnValue.Value, paramsArray);

				if (returnedValue is Boolean && (Boolean)returnedValue) {
					returnValue = (T)paramsArray[1];
				}
				else {
					returnValue = null;
				}
			}

			return returnValue;
		}

		private static Dictionary<string, MethodInfo> someCache = new Dictionary<string, MethodInfo>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeToCheck"></param>
		/// <returns></returns>
		/// <remarks>
		/// See: http://byatool.com/general-coding/getting-back-out-parameters-using-getmethod/
		/// </remarks>
		private static MethodInfo GetCorrectMethodInfo(Type typeToCheck) {
			//This line may not mean much but with reflection, it's usually a good idea to store
			//things like method info or property info in a cache somewhere so that you don't have
			//have to use reflection every time to get what you need.  That's what this is doing.
			//Basically I am using the passed in type name as the key and the value is the methodInfo
			//for that type.
			MethodInfo returnValue = someCache[typeToCheck.FullName];

			//Ok, now for the reflection part.
			if (returnValue == null) {
				Type[] paramTypes = new Type[2] { typeof(string), typeToCheck.MakeByRefType() };
				returnValue = typeToCheck.GetMethod("TryParse", paramTypes);
				if (returnValue != null) {
					someCache.Add(typeToCheck.FullName, returnValue);
				}
			}

			return returnValue;
		}

		public static bool EndsWith(this string str, string value) {
			if (str.IsNullOrEmpty()) return false;
			if (value.IsNullOrEmpty()) return false;
			return str.Right(value.Length) == value;
		}

		public static string EnsureEndsWith(this string str, string value) {
			if (str.EndsWith(value))
				return str;
			return str + value;
		}

		public static string EnsureDoesNotEndWith(this string str, string value) {
			if (str.EndsWith(value))
				return str.RemoveLast(1);
			return str;
		}

		public static string RemoveLast(this string str, int value) {
			if (str.Length < value)
				return str;
			return str.Remove(str.Length - value);
		}

		public static Dictionary<string, string> ToDictionary(this string str, char itemDelimeter, char keyValueDelimeter) {
			return str
				.Split(itemDelimeter)
				.Select(s => s.Split(keyValueDelimeter))
				.ToDictionary(sa => sa[0], sa => sa[1]);
		}

		public static bool IsAllUpper(this string str) {
			return str.ToUpper() == str;
		}

		public static string ToTitleCase(this string str) {
			if (str.IsNullOrOnlyContainsSpaces()) return string.Empty;
			if (str.Length == 1) return str.ToUpper();
			// TextInfo.ToTitleCase treats ALLCAPS as abbreviations so need to lowercase first.
			return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str.Trim().ToLower());
		}

		public static string OrIfEmpty(this string str, string alternative) {
			return !str.IsNullOrOnlyContainsSpaces() ? str : alternative;
		}
	 
	}
}