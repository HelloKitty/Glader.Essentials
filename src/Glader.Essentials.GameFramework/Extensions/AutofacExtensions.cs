using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public static class AutofacExtensions
	{
		/// <summary>
		/// Registers the provided <typeparamref name="TConcreteType"/> as <see cref="IGameTickable"/> in a typesafe way.
		/// </summary>
		/// <typeparam name="TConcreteType">The concrete type.</typeparam>
		/// <param name="register">The register interface.</param>
		/// <returns>Fluent return.</returns>
		public static IRegistrationBuilder<TConcreteType, ConcreteReflectionActivatorData, SingleRegistrationStyle> 
			AsTickable<TConcreteType>(this IRegistrationBuilder<TConcreteType, ConcreteReflectionActivatorData, SingleRegistrationStyle> register)
			where TConcreteType : IGameTickable
		{
			return register.As<IGameTickable>();
		}
	}
}
