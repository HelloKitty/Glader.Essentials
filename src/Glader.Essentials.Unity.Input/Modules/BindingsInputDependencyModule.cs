using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using JetBrains.Annotations;
using Module = Autofac.Module;

namespace Glader.Essentials
{
	/// <inheritdoc />
	public sealed class BindingsInputDependencyModule<TBindingEnumType> : Module 
		where TBindingEnumType : Enum
	{
		private Assembly AssemblyToParse { get; }

		public BindingsInputDependencyModule([NotNull] Assembly assemblyToParse)
		{
			AssemblyToParse = assemblyToParse ?? throw new ArgumentNullException(nameof(assemblyToParse));
		}

		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterModule<BindingInputControlsDependencyModule>()
				.RegisterModule(new BindingInputHandlerDependencyModule<TBindingEnumType>(AssemblyToParse));
		}
	}
}
