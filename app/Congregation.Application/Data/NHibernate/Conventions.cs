using System;
using System.Linq;
using Congregation.Application.Data.NHibernate.Overrides;
using Congregation.Core.Models;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;

namespace Congregation.Application.Data.NHibernate
{
	/// <summary>
	/// Applies global common conventions to the mapped entities. 
	/// For clarity configurations set here can be overriden in 
	/// an entity's specific mapping file.  For example; The Id 
	/// convention here is set to Id but if the Id column 
	/// was mapped in the entity's mapping file then the entity's 
	/// mapping file configuration will take precedence.
	/// </summary>
	internal static class Conventions
	{
		public static void WithConventions(this ConventionModelMapper mapper, Configuration configuration) {
			Type baseEntityType = typeof (Entity);

			mapper.IsEntity((type, declared) => IsEntity(type));
			mapper.IsComponent((type, b) => IsComponent(type));
			mapper.IsRootEntity((type, declared) => baseEntityType.Equals(type.BaseType));

			mapper.BeforeMapClass += (modelInspector, type, classCustomizer) =>
			{
				classCustomizer.Id(c => c.Column("Id"));
				classCustomizer.Id(c => c.Generator(Generators.HighLow));
				classCustomizer.Table(Inflector.Net.Inflector.Pluralize(type.Name.ToString()));
			};

			mapper.BeforeMapManyToOne += (modelInspector, propertyPath, map) =>
			{
				map.Column(propertyPath.LocalMember.GetPropertyOrFieldType().Name + "Id");
				map.Cascade(Cascade.Persist);
			};

			mapper.BeforeMapBag += (modelInspector, propertyPath, map) =>
			{
				map.Key(keyMapper => keyMapper.Column(propertyPath.GetContainerEntity(modelInspector).Name + "Fk"));
				map.Cascade(Cascade.All);
			};

			mapper.BeforeMapProperty += (inspector, member, customizer) => {
				// This is pure guesswork, but seems to be the only way I can think of to alter
				// the column naming of a property mapped as part of a component
				if(typeof(IComponent).IsAssignableFrom(member.LocalMember.DeclaringType)) {
					if(member.LocalMember.Name == "Value") {
						customizer.Column(member.PreviousPath.LocalMember.Name);
					}
					else {
						customizer.Column(member.PreviousPath.LocalMember.Name + member.LocalMember.Name);						
					}
				}
			};

			mapper.BeforeMapComponent += (inspector, member, customizer) => {
				// Does this actually do anything at all?
			};

			AddConventionOverrides(mapper);

			HbmMapping mapping = mapper.CompileMappingFor(
				typeof(Entity).Assembly.GetExportedTypes().Where(IsEntity));
			configuration.AddDeserializedMapping(mapping, "MyStoreMappings");
		}

		private static bool IsComponent(Type type) {
			return typeof (IComponent).IsAssignableFrom(type);
		}

		/// <summary>
		/// Determine if type implements IEntityWithTypedId<>
		/// </summary>
		public static bool IsEntity(Type type) {
			return typeof (Entity).IsAssignableFrom(type) && typeof (Entity) != type && !type.IsInterface;
		}

		/// <summary>
		/// Looks through this assembly for any IOverride classes.  If found, it creates an instance
		/// of each and invokes the Override(mapper) method, accordingly.
		/// </summary>
		private static void AddConventionOverrides(ConventionModelMapper mapper) {
			var overrideType = typeof (IOverride);
			var types = typeof (IOverride).Assembly.GetTypes()
				.Where(t => overrideType.IsAssignableFrom(t) && t != typeof (IOverride))
				.ToList();

			types.ForEach(t =>
			{
				var conventionOverride = Activator.CreateInstance(t) as IOverride;
				conventionOverride.Override(mapper);
			});
		}
	}

}