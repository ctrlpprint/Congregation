using System.Collections.Generic;

namespace Congregation.Web.Features.Directory
{
	public class DirectorySearchViewModel
	{
		public IList<MemberStub> Members { get; set; } 
	}

	public class MemberStub
	{
		public string Name { get; set; }
		public int FamilyId { get; set; }
		public int ContactId { get; set; }
	}

}