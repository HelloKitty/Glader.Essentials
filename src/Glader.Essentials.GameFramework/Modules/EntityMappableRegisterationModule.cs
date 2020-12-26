using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Glader.Essentials
{
	public sealed class EntityMappableRegisterationModule<TEntityKey> : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			//The below is kinda a hack to register the non-generic types in the
			//removabale collection
			List<IEntityCollectionRemovable<TEntityKey>> removableComponentsList = new List<IEntityCollectionRemovable<TEntityKey>>(10);

			builder.RegisterGeneric(typeof(EntityGuidDictionary<,>))
				.AsSelf()
				.As(typeof(IReadonlyEntityGuidMappable<,>))
				.As(typeof(IEntityGuidMappable<,>))
				.OnActivated(args =>
				{
					if(args.Instance is IEntityCollectionRemovable<TEntityKey> removable)
						removableComponentsList.Add(removable);
				})
				.SingleInstance();

			//This will allow everyone to register the removable collection collection.
			builder.RegisterInstance(removableComponentsList)
				.As<IReadOnlyCollection<IEntityCollectionRemovable<TEntityKey>>>()
				.AsSelf()
				.SingleInstance();
		}
	}
}
