﻿using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Glader.Essentials
{
	/// <inheritdoc />
	public sealed class GameInputDependencyModule<TBindingEnumType> : Module 
		where TBindingEnumType : Enum
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<DefaultFrameworkInputServices>()
				.As<IFrameworkInputServices>()
				.SingleInstance();

			builder.RegisterModule<BindingsInputDependencyModule<TBindingEnumType>>();
		}
	}
}
