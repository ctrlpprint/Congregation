using Congregation.Application.Data.NHibernate.ConfigurationCaching;
using Congregation.Core.Models;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;

namespace Congregation.Application.Data.NHibernate
{
	public class NHibernateInitializer
	{
		public static void ResetCache() {
			var cache = new NHibernateConfigurationFileCache();
			cache.Evict(ConfigCacheKey);
		}

		public static Configuration Initialize() {
			var cache = new NHibernateConfigurationFileCache();

			var mappingAssemblies = new[] { 
                typeof(Entity).Assembly.GetName().Name
            };

			var configuration = cache.LoadConfiguration(ConfigCacheKey, null, mappingAssemblies);

			if (configuration == null) {
				configuration = new Configuration();

				configuration
					.Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
					.DataBaseIntegration(db => {
						db.ConnectionStringName = "LocalSqlServer";
						db.Dialect<MsSql2008Dialect>();
					})
					.AddAssembly(typeof(Entity).Assembly)
					.CurrentSessionContext<LazySessionContext>();

				var mapper = new ConventionModelMapper();
				mapper.WithConventions(configuration);

				cache.SaveConfiguration(ConfigCacheKey, configuration);
			}

			return configuration;
		}

		private const string ConfigCacheKey = "Congregation";
	 
	}
}