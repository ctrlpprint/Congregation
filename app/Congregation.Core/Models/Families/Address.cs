namespace Congregation.Core.Models.Families
{
	public class Address : Entity
	{
		public virtual string AddressLine1 { get; set; }
		public virtual string AddressLine2 { get; set; }
		public virtual string AddressLine3 { get; set; }
		public virtual string PostCode { get; set; }
		public virtual string City { get; set; }
		public virtual string Country { get; set; }
		public virtual string Description { get; set; }
		public virtual string Lat { get; set; }
		public virtual string Long { get; set; }
	}
}