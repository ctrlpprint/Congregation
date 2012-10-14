using System;
using System.Collections.Generic;
using System.Linq;
using Congregation.Core.Models.Privacy;

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
			return Address != null && !string.IsNullOrEmpty(Address.ToString());
		}

		public virtual bool HasLocation() {
			return Address != null && Address.Lat.IsNotEmpty() && Address.Lng.IsNotEmpty();
		}

		public virtual string Names() {
			return string.Join(",", Contacts.Select(c => c.FirstName));
		}
		 
	}
}