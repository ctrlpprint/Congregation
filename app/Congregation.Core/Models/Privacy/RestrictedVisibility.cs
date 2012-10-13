using Congregation.Core.Models.Families;
using System.Linq;

namespace Congregation.Core.Models.Privacy
{
	public class RestrictedVisibility<T> : IComponent
	{
		public virtual T Value { get; set; }
		public virtual Visibility Visibility { get; set; }

		// Can a component include a reference to its parent?
		public virtual object Owner { get; set; }

		public override string ToString() {
			return Value.ToString();
		}

		public static implicit operator T (RestrictedVisibility<T> restrictedVisibility) {
			// Show if user has rights
			if (restrictedVisibility.Visibility == Visibility.Congregation)
				return restrictedVisibility.Value;

			var currentUser = System.Threading.Thread.CurrentPrincipal;
			if (currentUser.IsInRole("Admin"))
				return restrictedVisibility.Value;
			if (currentUser.IsInRole("Staff") && restrictedVisibility.Visibility == Visibility.Staff)
				return restrictedVisibility.Value;

			// if currentUser is in this aggregate, return value
			var family = restrictedVisibility.PartOfFamily();
			if (family == null) return default(T);
			// Sub in below whatever the login name is
			return family.Contacts.Any(c => c.FacebookId == currentUser.Identity.Name) ? restrictedVisibility.Value : default(T);
		}

		private Family PartOfFamily() {
			var contact = Owner as Contact;
			if (contact != null) return contact.Family;

			var family = Owner as Family;
			if (family != null) return family;

			var address = Owner as Address;
			if (address != null) return address.Family;

			return null;
		}
	}
}