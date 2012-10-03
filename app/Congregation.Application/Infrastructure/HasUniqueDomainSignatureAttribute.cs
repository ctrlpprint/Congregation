using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Congregation.Application.Data;
using Congregation.Core.Common;
using Congregation.Core.Models;

namespace Congregation.Application.Infrastructure
{
	/// <summary>
	/// Tag a class as having a domain signature.
	/// Enables the <see cref="IEntityDuplicateChecker"/> to check for duplicates.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class HasUniqueDomainSignatureAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
			if (value == null)
				return null;

			var entityToValidate = value as IEntity;

			Check.Not(entityToValidate == null)
				.OrThrow<InvalidOperationException>(
					"This validator must be used at the class level of an IEntity. " +
					"The type you provided was " + value.GetType());

			var duplicateChecker = DependencyResolver.Current.GetService<IEntityDuplicateChecker>();

			if (duplicateChecker == null)
				throw new TypeLoadException("IEntityDuplicateChecker has not been registered with IoC");

			if (duplicateChecker.DoesDuplicateExistOf(entityToValidate))
				return new ValidationResult(String.Empty);

			return null;
		}
	}
}