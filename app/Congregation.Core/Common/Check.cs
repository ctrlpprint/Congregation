using System;

namespace Congregation.Core.Common
{
	public class Check
	{
		public static void That(bool assertion, string message) {
			if (!assertion)
				throw new AssertionException(message);
		}

		public static Check That(bool assertion) {
			return !assertion ? new Check() : null;
		}

		public static void Not(bool assertion, string message) {
			if (assertion)
				throw new AssertionException(message);
		}

		public static Check Not(bool assertion) {
			return assertion ? new Check() : null;
		}

		public static void NotNull(object obj, string variableName) {
			if (obj == null)
				throw new NullReferenceException(variableName + " was null");
		}

		private Check() {
		}

	}

	public static class CheckExtensions
	{
		public static void OrThrow(this Check check, string message) {
			if (check == null) return;

			throw new AssertionException(message);
		}

		public static void OrThrow(this Check check, string message, params object[] args) {
			if (check == null) return;

			throw new AssertionException(string.Format(message, args));
		}

		public static void OrThrow<T>(this Check check, string message) where T : Exception {
			if (check == null) return;

			throw Activator.CreateInstance(typeof(T), message) as T;
		}

		public static void Or(this Check check, Action action) {
			if (check == null) return;

			action();
		}
	}


	public class AssertionException : ApplicationException
	{
		public AssertionException() { }
		public AssertionException(string message) : base(message) { }
		public AssertionException(string message, Exception inner) : base(message, inner) { }
	}
}