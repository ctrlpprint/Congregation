﻿using System.IO;
using Congregation.Application.Data.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Congregation.Tests.NHibernateTests
{
	/// <summary>
	/// Provides a means to verify that the target database is in compliance with all mappings.
	/// Taken from http://ayende.com/Blog/archive/2006/08/09/NHibernateMappingCreatingSanityChecks.aspx.
	/// 
	/// If this is failing, the error will likely inform you that there is a missing table or column
	/// which needs to be added to your database.
	/// </summary>
	[TestFixture]
	[Category("DB Tests")]
	public class MappingIntegrationTests
	{
		[SetUp]
		public virtual void SetUp() {
			configuration = NHibernateInitializer.Initialize();
			sessionFactory = configuration.BuildSessionFactory();
		}

		[Test]
		public void CanConfirmDatabaseMatchesMappings() {
			var allClassMetadata = sessionFactory.GetAllClassMetadata();

			foreach (var entry in allClassMetadata) {
				sessionFactory.OpenSession().CreateCriteria(entry.Value.GetMappedClass(EntityMode.Poco))
					.SetMaxResults(0).List();
			}
		}

		/// <summary>
		/// Generates and outputs the database schema SQL to the console
		/// </summary>
		[Test]
		public void CanGenerateDatabaseSchema() {
			using (ISession session = sessionFactory.OpenSession()) {
				using (TextWriter stringWriter = new StreamWriter("../../../../app/Congrgation.Application/Data/Sql/UnitTestGeneratedSchema.sql")) {
					new SchemaExport(configuration).Execute(true, false, false, session.Connection, stringWriter);
				}
			}
		}

		private Configuration configuration;
		private ISessionFactory sessionFactory;
	}
}