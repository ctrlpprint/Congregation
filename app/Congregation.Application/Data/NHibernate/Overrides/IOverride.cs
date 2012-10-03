using NHibernate.Mapping.ByCode;

namespace Congregation.Application.Data.NHibernate.Overrides
{
	internal interface IOverride
	{
		void Override(ModelMapper mapper);
	}
}