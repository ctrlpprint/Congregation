using System.IO;
using Congregation.Application.Data.NHibernate;
using Congregation.Core.Models;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
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
			NHibernateInitializer.ResetCache();
			configuration = NHibernateInitializer.Initialize();
			sessionFactory = configuration.BuildSessionFactory();
		}

		[Test]
		public void CanConfirmDatabaseMatchesMappings() {
			var allClassMetadata = sessionFactory.GetAllClassMetadata();

			foreach (var entry in allClassMetadata) {
				sessionFactory
					.OpenSession()
					.CreateCriteria(entry.Value.GetMappedClass(EntityMode.Poco))
					.SetMaxResults(0).List();
			}
		}

		/// <summary>
		/// Generates and outputs the database schema SQL to the console
		/// </summary>
		[Test]
		public void CanGenerateDatabaseSchema() {
			using (ISession session = sessionFactory.OpenSession()) {
				using (TextWriter stringWriter = new StreamWriter("../../NHibernateTests/UnitTestGeneratedSchema.sql")) {
					new SchemaExport(configuration).Execute(true, false, false, session.Connection, stringWriter);
				}
			}
		}


		[Test]
		public void CanGenerateMappingDoc() {
			// Need a separate config so we can get hold of the mapper after using it.
			// If we use the existing config we get a duplicate mapping exception.
			var config = NHibernateInitializer.CreateConfiguration();

			var mapper = new ConventionModelMapper();
			mapper.WithConventions(config);

			var mapping = mapper.CompileMappingFor(typeof(Entity).Assembly.GetExportedTypes());
			var x = mapping.AsString();
			File.WriteAllText("../../NHibernateTests/Output.xml", x);
		}


		private Configuration configuration;
		private ISessionFactory sessionFactory;

	}
}