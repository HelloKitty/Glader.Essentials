using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Glader.Essentials
{
	/// <summary>
	/// Registers <see cref="IDeferredDependency{TDependencyType}"/> using the <see cref="LazilyDeferredDependency{TDependencyType}"/>.
	/// </summary>
	public sealed class DeferredDependencyLazyDependencyModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// For lazily loadded/deferred dependencies
			builder.RegisterGeneric(typeof(LazilyDeferredDependency<>))
				.As(typeof(IDeferredDependency<>))
				.SingleInstance();
		}
	}
}
