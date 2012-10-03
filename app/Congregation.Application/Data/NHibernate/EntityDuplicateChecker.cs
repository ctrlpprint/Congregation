using System;
using System.Linq;
using System.Reflection;
using Congregation.Core.Common;
using Congregation.Core.Models;
using NHibernate;
using NHibernate.Criterion;

namespace Congregation.Application.Data.NHibernate
{
	/// <summary>
	/// Check for duplicates of a given entity based on Id and Domain Signature
	/// Based on SharpLite
	/// </summary>
	public class EntityDuplicateChecker : IEntityDuplicateChecker
	{	
		public EntityDuplicateChecker(ISessionFactory sessionFactory) {
			Check.NotNull(sessionFactory, "sessionFactory");

			this.sessionFactory = sessionFactory;
		}

		/// <summary>
		/// Provides a behavior specific repository for checking if a duplicate exists of an existing entity.
		/// </summary>
		public bool DoesDuplicateExistOf(IEntity entity) {
			Check.NotNull(entity, "entity");

			var session = sessionFactory.GetCurrentSession();
			var previousFlushMode = session.FlushMode;

			// We do NOT want this to flush pending changes as checking for a duplicate should 
			// only compare the object against data that's already in the database
			session.FlushMode = FlushMode.Never;

			var criteria =
				session.CreateCriteria(entity.GetType()).Add(Restrictions.Not(Restrictions.Eq("Id", entity.Id))).
					SetMaxResults(1);

			AppendSignaturePropertyCriteriaTo(criteria, entity);
			bool doesDuplicateExist = criteria.List().Count > 0;
			session.FlushMode = previousFlushMode;
			return doesDuplicateExist;
		}

		private static void AppendEntityCriteriaTo(
			ICriteria criteria, PropertyInfo signatureProperty, object propertyValue) {
			criteria.Add(
				propertyValue != null
					? Restrictions.Eq(signatureProperty.Name + ".Id", ((IEntity)propertyValue).Id)
					: Restrictions.IsNull(signatureProperty.Name + ".Id"));
		}

		private static void AppendStringPropertyCriteriaTo(
			ICriteria criteria, PropertyInfo signatureProperty, object propertyValue) {
			criteria.Add(
				propertyValue != null
					? Restrictions.InsensitiveLike(signatureProperty.Name, propertyValue.ToString(), MatchMode.Exact)
					: Restrictions.IsNull(signatureProperty.Name));
		}

		private static void AppendValuePropertyCriteriaTo(
			ICriteria criteria, PropertyInfo signatureProperty, object propertyValue) {
			criteria.Add(
				propertyValue != null
					? Restrictions.Eq(signatureProperty.Name, propertyValue)
					: Restrictions.IsNull(signatureProperty.Name));
		}

		private void AppendDateTimePropertyCriteriaTo(
			ICriteria criteria, PropertyInfo signatureProperty, object propertyValue) {
			criteria.Add(
				(DateTime)propertyValue > this.uninitializedDatetime
					? Restrictions.Eq(signatureProperty.Name, propertyValue)
					: Restrictions.IsNull(signatureProperty.Name));
		}

		private void AppendSignaturePropertyCriteriaTo(ICriteria criteria, IEntity entity) {
			foreach (var signatureProperty in entity.GetSignatureProperties()) {
				var propertyType = signatureProperty.PropertyType;
				var propertyValue = signatureProperty.GetValue(entity, null);

				if (propertyType.IsEnum) {
					criteria.Add(Restrictions.Eq(signatureProperty.Name, (int)propertyValue));
				}
				else if (
					propertyType.GetInterfaces().Any(
						x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntity))) {
					AppendEntityCriteriaTo(criteria, signatureProperty, propertyValue);
				}
				else if (propertyType == typeof(DateTime)) {
					this.AppendDateTimePropertyCriteriaTo(criteria, signatureProperty, propertyValue);
				}
				else if (propertyType == typeof(string)) {
					AppendStringPropertyCriteriaTo(criteria, signatureProperty, propertyValue);
				}
				else if (propertyType.IsValueType) {
					AppendValuePropertyCriteriaTo(criteria, signatureProperty, propertyValue);
				}
				else {
					throw new ApplicationException(
						"Can't determine how to use " + entity.GetType() + "." + signatureProperty.Name +
						" when looking for duplicate entries. To remedy this, " +
						"you can create a custom validator or report an issue to the S#arp Architecture " +
						"project, detailing the type that you'd like to be accommodated.");
				}
			}
		}

		private readonly ISessionFactory sessionFactory;
		private readonly DateTime uninitializedDatetime = default(DateTime);
	}
}