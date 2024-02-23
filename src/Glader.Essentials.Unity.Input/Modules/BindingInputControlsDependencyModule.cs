using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Glader.Essentials
{
	//Based on GladMMO: InputDependencyAutofacModule
	/// <inheritdoc />
	public sealed class BindingInputControlsDependencyModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// For WoW we use the new Binding input
			// BindingSystemMovementInputController : IBindingSystemMovementInputController
			builder.RegisterType<BindingSystemMovementInputController>()
				.As<IMovementInputController>()
				.As<IBindingSystemMovementInputController>()
				.SingleInstance();

			builder.RegisterType<BindingSystemInputCameraInputController>()
				.As<ICameraInputController>()
				.As<IBindingSystemCameraInputController>()
				.SingleInstance();
		}
	}
}
