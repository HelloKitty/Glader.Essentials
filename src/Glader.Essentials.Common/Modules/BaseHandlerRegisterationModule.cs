using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Module = Autofac.Module;

namespace Glader.Essentials
{
	public sealed class BaseHandlerRegisterationModule<THandlerType> : Module
	{
		static BaseHandlerRegisterationModule()
		{
			if(!typeof(THandlerType).IsGenericType)
				throw new InvalidOperationException($"Specified {nameof(THandlerType)} is not generic. Is Type: {typeof(THandlerType).Name}");
		}

		/// <summary>
		/// Optional additional types to register the handlers as.
		/// </summary>
		private Type[] AdditionalTypesToRegisterHandlersAs { get; }

		/// <summary>
		/// The scene to load handlers for.
		/// </summary>
		private int SceneType { get; }

		/// <summary>
		/// The assembly to search the handler types for.
		/// </summary>
		private Assembly AssemblyForHandlerTypes { get; }

		/// <inheritdoc />
		public BaseHandlerRegisterationModule(int sceneType, [NotNull] Assembly assemblyForHandlerTypes, params Type[] additionalTypesToRegisterHandlersAs)
		{
			if(additionalTypesToRegisterHandlersAs == null)
				AdditionalTypesToRegisterHandlersAs = new Type[0];

			SceneType = sceneType;
			AssemblyForHandlerTypes = assemblyForHandlerTypes ?? throw new ArgumentNullException(nameof(assemblyForHandlerTypes));
			AdditionalTypesToRegisterHandlersAs = additionalTypesToRegisterHandlersAs;
		}

		protected static bool DetermineIfHandlerIsForSceneType(Type handlerType, int sceneType)
		{
			//We don't want to get base attributes
			//devs may want to inherit from a handler and change some stuff. But not register it as a handler
			//for the same stuff obviously.
			foreach(SceneTypeCreateAttribute attris in handlerType.GetTypeInfo().GetCustomAttributes<SceneTypeCreateAttribute>(false))
			{
				if(attris.SceneType == sceneType)
					return true;
			}

			return false;
		}

		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			IEnumerable<Type> handlerTypes = LoadHandlerTypes().ToArray();

			//Registers each type.
			foreach(Type handlerType in handlerTypes)
			{
				//TODO: Improve efficiency of all this reflection we are doing.
				IEnumerable<SceneTypeCreateAttribute> attributes = handlerType.GetCustomAttributes<SceneTypeCreateAttribute>(false);

				//We just skip now instead. For ease, maybe revert
				if(attributes == null || !attributes.Any())  //don't use base attributes
					continue;

				bool isForSceneType = DetermineIfHandlerIsForSceneType(handlerType, SceneType);

				//if it's not for the specified scene type, then skip.
				if(!isForSceneType)
					continue;

				var handlerRegisterationBuilder = builder.RegisterType(handlerType)
					.AsSelf()
					.As<THandlerType>();

				if(AdditionalTypesToRegisterHandlersAs.Any())
					foreach(Type additionalHandlerTypeRegisteration in AdditionalTypesToRegisterHandlersAs)
					{
						handlerRegisterationBuilder = handlerRegisterationBuilder
							.As(additionalHandlerTypeRegisteration);
					}

				//Now we need to register it as the additional specified types
				foreach(var additionalServiceTypeAttri in handlerType.GetCustomAttributes<AdditionalRegisterationAsAttribute>(true))
				{
					handlerRegisterationBuilder = handlerRegisterationBuilder
						.As(additionalServiceTypeAttri.ServiceType);
				}

				//Only ever want one handler, otherwise... things get werid with AdditionalRegisterationAsAttributes.
				handlerRegisterationBuilder = handlerRegisterationBuilder.InstancePerLifetimeScope();
			}
		}

		private IReadOnlyCollection<Type> LoadHandlerTypes()
		{
			return AssemblyForHandlerTypes
				.GetTypes()
				.Where(t => t.GetInterfaces().Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(THandlerType).GetGenericTypeDefinition()) && !t.IsAbstract)
				.Where(t => typeof(THandlerType).IsAssignableFrom(t))
				.ToArray();
		}
	}
}
