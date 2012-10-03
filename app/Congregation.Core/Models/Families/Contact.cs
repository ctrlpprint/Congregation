using Congregation.Core.Models.Privacy;

namespace Congregation.Core.Models.Families
{
	public class Contact : Entity
	{
		public virtual string Gender { get; set; }

		public virtual string FirstName { get; set; }

		public virtual Secured<string> Mobile { get; set; }
		public virtual Secured<string> Email { get; set; }
		public virtual string FacebookId { get; set; }

		public virtual bool MobileInDirectory { get; set; }
		public virtual bool Txt { get; set; }
		public virtual bool MailingList { get; set; }
		 
	}
}