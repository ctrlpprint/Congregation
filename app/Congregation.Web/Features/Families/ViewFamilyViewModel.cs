using System.Collections.Generic;
using Congregation.Core.Models.Families;

namespace Congregation.Web.Features.Families
{
	public class ViewFamilyViewModel
	{
		public Family Family { get; set; }
		public IList<NearestNeighbour> Neighbours { get; set; }

		public ViewFamilyViewModel(Family family, IList<NearestNeighbour> neighbours) {
			Family = family;
			Neighbours = neighbours;
		}
	}
}