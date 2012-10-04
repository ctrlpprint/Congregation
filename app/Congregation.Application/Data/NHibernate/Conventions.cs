using System;
using System.Linq;
using Congregation.Application.Data.NHibernate.Overrides;
using Congregation.Core.Models;
using Congregation.Core.Models.Privacy;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;

namespace NHibernate.Mapping.ByCode
{
	/// <summary>
	/// Applies global common conventions to the mapped entities. 
	/// For clarity configurations set here can be overriden in 
	/// an entity's specific mapping file.  For example; The Id 
	/// convention here is set to Id but if the Id column 
	/// was mapped in the entity's mapping file then the entity's 
	/// mapping file configuration will take precedence.
	/// </summary>
	public static class Conventions
	{
		public static void WithConventions(this ConventionModelMapper mapper, Configuration configuration) {
			
			var baseEntityType = typeof (Entity);

			mapper.IsEntity((type, declared) => IsEntity(type));
			mapper.IsComponent((type, b) => IsComponent(type));
			mapper.IsRootEntity((type, declared) => baseEntityType.Equals(type.BaseType));

			mapper.BeforeMapClass += (modelInspector, type, classCustomizer) =>
			{
				classCustomizer.Id(c => c.Column("Id"));
				classCustomizer.Id(c => c.Generator(Generators.HighLow));
				classCustomizer.Table(Inflector.Net.Inflector.Pluralize(type.Name.ToString()));
			};

			mapper.IsPersistentProperty((memberinfo, currentlyPersistent) =>
			{
				return memberinfo.Name != "Owner";
			});

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
					else if(member.LocalMember.Name != "Owner") {
						customizer.Column(member.PreviousPath.LocalMember.Name + member.LocalMember.Name);						
					}
				}
			};


			mapper.Component<Secured<string>>(x => {
				x.Property(c => c.Value);
				x.Property(c => c.Visibility);
				x.Parent(c => c.Owner);
			});

			mapper.Component<Secured<bool>>(x => {
				x.Property(c => c.Value);
				x.Property(c => c.Visibility);
				x.Parent(c => c.Owner);
			});

			// The following probably works, but not if we stop "Owner" from being a persistent property
			// using mapping.IsPersistentProperty, and if we don't do that we get a Too Many Properties exception.
			// There's an old thread on nhusers about how there's no documentation in this area...
			//mapper.BeforeMapComponent += (inspector, member, customizer) => {
			//    if (member.LocalMember.Name == "Owner") {
			//        customizer.Parent(member.LocalMember);
			//    }
			//};

			AddConventionOverrides(mapper);

			HbmMapping mapping = mapper.CompileMappingFor(
				typeof(Entity).Assembly.GetExportedTypes().Where(IsEntity));
			configuration.AddDeserializedMapping(mapping, "MyStoreMappings");
		}

		private static bool IsComponent(System.Type type) {
			return typeof (IComponent).IsAssignableFrom(type);
		}

		/// <summary>
		/// Determine if type implements IEntityWithTypedId<>
		/// </summary>
		public static bool IsEntity(System.Type type) {
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