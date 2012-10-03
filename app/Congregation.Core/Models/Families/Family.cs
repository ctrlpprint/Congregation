using System.Collections.Generic;
using System.Linq;

namespace Congregation.Core.Models.Families
{
	public class Family : Entity
	{
		public virtual int Id { get; set; }
		public virtual string FamilyName { get; set; }
		public virtual IList<Contact> Contacts { get; set; }
		public virtual string Children { get; set; }
		public virtual Address Address { get; set; }
		public virtual string Phone { get; set; }
		public virtual bool ShowInDirectory { get; set; }

		public Family() {
			Address = new Address();
			Contacts = new List<Contact>();
		}

		public virtual bool HasAddress() {
			return Address != null && !string.IsNullOrEmpty(Address.Description);
		}

		public virtual string Names() {
			return string.Join(",", Contacts.Select(c => c.FirstName));
		}
		 
	}
}