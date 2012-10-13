using System.Collections.Generic;
using Congregation.Core.Models.Privacy;

namespace Congregation.Core.Models.Families
{
	public class Address : Entity
	{
		public virtual Family Family { get; set; }
		public virtual string StreetNumber { get; set; }
		public virtual string Street { get; set; }
		public virtual string Suburb { get; set; }
		public virtual string PostCode { get; set; }
		public virtual string City { get; set; }
		public virtual string Country { get; set; }
		public virtual string Lat { get; set; }
		public virtual string Lng { get; set; }

		public override string ToString() {
			return new List<string>
			{
				StreetNumber, 
				Street, 
				Suburb,
				new List<string>{ PostCode, City }.ToDelimitedList(" ")
			}.ToCommaSeparatedList();
		}
	}
}