using Congregation.Application.Data.NHibernate.ConfigurationCaching;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;

namespace Congregation.Application.Data.NHibernate
{
	public class NHibernateInitializer
	{
		public static Configuration Initialize() {
			var cache = new NHibernateConfigurationFileCache();

			var mappingAssemblies = new[] { 
                typeof(NHibernateInitializer).Assembly.GetName().Name
            };

			var configuration = cache.LoadConfiguration(CONFIG_CACHE_KEY, null, mappingAssemblies);

			if (configuration == null) {
				configuration = new Configuration();

				configuration
					.Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
					.DataBaseIntegration(db => {
						db.ConnectionStringName = "MyStoreConnectionString";
						db.Dialect<MsSql2008Dialect>();
					})
					.AddAssembly(typeof(NHibernateInitializer).Assembly)
					.CurrentSessionContext<LazySessionContext>();

				var mapper = new ConventionModelMapper();
				mapper.WithConventions(configuration);

				cache.SaveConfiguration(CONFIG_CACHE_KEY, configuration);
			}

			return configuration;
		}

		private const string CONFIG_CACHE_KEY = "MyStore";
	 
	}
}