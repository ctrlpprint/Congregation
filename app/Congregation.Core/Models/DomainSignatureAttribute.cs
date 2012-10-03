using System;

namespace Congregation.Core.Models
{
	/// <summary>
	/// Facilitates indicating which property(s) describe the unique signature of an 
	/// entity.  See Entity.GetTypeSpecificSignatureProperties() for when this is leveraged.
	/// From SharpLite
	/// </summary>
	/// <remarks>
	/// Intended for use with <see cref = "Entity" />, not with <see cref = "ValueObject" />.
	/// </remarks>
	[Serializable]
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class DomainSignatureAttribute : Attribute
	{
	}
}