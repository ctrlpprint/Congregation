using System.Collections.Generic;
using System.Reflection;

namespace Congregation.Core.Models
{
	public interface IEntity : IComparableObject
	{
		int Id { get; }
		bool IsTransient();
	}

	public interface IComparableObject
	{
		IEnumerable<PropertyInfo> GetSignatureProperties();		
	}
}