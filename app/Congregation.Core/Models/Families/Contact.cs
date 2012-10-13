using Congregation.Core.Models.Privacy;

namespace Congregation.Core.Models.Families
{
	public class Contact : Entity
	{
		public virtual Family Family { get; set; }
		public virtual string Gender { get; set; }

		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }

		public virtual RestrictedVisibility<string> Mobile { get; set; }
		public virtual RestrictedVisibility<string> Email { get; set; }
		public virtual RestrictedVisibility<string> FacebookId { get; set; }

		// Whatever we use for login.
		//public virtual string UserName { get; set; }
		 
	}
}