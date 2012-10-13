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
			cache.Evict(CONFIG_CACHE_KEY);
		}

		public static Configuration Initialize() {
			var cache = new NHibernateConfigurationFileCache();

			var mappingAssemblies = new[] { 
                typeof(Entity).Assembly.GetName().Name
            };

			var configuration = cache.LoadConfiguration(CONFIG_CACHE_KEY, null, mappingAssemblies);
			
			if (configuration == null) {

				configuration = CreateConfiguration();

				var mapper = new ConventionModelMapper();
				mapper.WithConventions(configuration);

				cache.SaveConfiguration(CONFIG_CACHE_KEY, configuration);
			}

			return configuration;
		}

		public static Configuration CreateConfiguration() {
			var configuration = new Configuration();

			configuration
				.Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
				.DataBaseIntegration(db => {
					db.ConnectionStringName = "DefaultConnection";
					db.Dialect<MsSql2008Dialect>();
				})
				.AddAssembly(typeof(Entity).Assembly)
				.CurrentSessionContext<LazySessionContext>();
			return configuration;
		}

		private const string CONFIG_CACHE_KEY = "Congregation";
	 
	}
}