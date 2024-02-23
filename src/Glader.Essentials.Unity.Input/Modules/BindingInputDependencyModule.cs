using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using JetBrains.Annotations;
using Module = Autofac.Module;

namespace Glader.Essentials
{
	/// <summary>
	/// Autofac module for registering <see cref="IBindingInputMessageHandler{TBindingEnumType}"/> and related services.
	/// </summary>
	/// <typeparam name="TBindingEnumType"></typeparam>
	public class BindingInputDependencyModule<TBindingEnumType> : Module where TBindingEnumType : Enum
	{
		/// <summary>
		/// Overrideable ignored handler types.
		/// </summary>
		protected virtual HashSet<Type> IgnoredHandlerTypes { get; } = new()
		{

		};

		private Assembly AssemblyToParse { get; }

		public BindingInputDependencyModule([NotNull] Assembly assemblyToParse)
		{
			AssemblyToParse = assemblyToParse ?? throw new ArgumentNullException(nameof(assemblyToParse));
		}

		public BindingInputDependencyModule()
			: this(typeof(BindingInputDependencyModule<TBindingEnumType>).Assembly)
		{
			
		}

		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<DefaultBindingInputMessageHandlingService<TBindingEnumType>>()
				.As<IBindingInputMessageHandlingService<TBindingEnumType>>()
				.SingleInstance();

			// Finds all simple handlers
			foreach(var type in AssemblyToParse
				.GetTypes()
				.Where(t => !t.IsAbstract && !t.IsInterface && t.IsAssignableTo<IBindingInputMessageHandler<TBindingEnumType>>() && !IgnoredHandlerTypes.Contains(t)))
			{
				var reg = builder.RegisterType(type)
					.As<IBindingInputMessageHandler<TBindingEnumType>>()
					.SingleInstance();

				// TODO: Any better way to do this?
				if (type.IsAssignableTo<IGameTickable>())
					reg.As<IGameTickable>();

				if (type.IsAssignableTo<IDisposable>())
					reg.As<IDisposable>();
			}
		}
	}
}
