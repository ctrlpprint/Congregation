namespace Congregation.Core.Models.Families
{
	public class NearestNeighbour
	{
		public virtual int AddressId { get; set; }
		public virtual int FamilyId { get; set; }
		public virtual decimal Distance { get; set; }

		public virtual Family Family { get; set; }
	}
}