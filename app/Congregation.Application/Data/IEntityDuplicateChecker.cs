using Congregation.Core.Models;

namespace Congregation.Application.Data
{
	public interface IEntityDuplicateChecker
	{
		bool DoesDuplicateExistOf(IEntity entity);
	}
}