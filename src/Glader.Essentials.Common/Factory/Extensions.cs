using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Glader
{
	/// <summary>
	/// Extension methods for <see cref="IFactoryCreatable{TCreateType,TContextType}"/>.
	/// </summary>
	public static class IFactoryCreatableExtensions
	{
		/// <summary>
		/// Creates a new instance of the <typeparamref name="TCreateType"/>.
		/// (Simplified overload for <see cref="EmptyFactoryContext"/>-based factories.
		/// </summary>
		/// <returns>A new instance.</returns>
		public static TCreateType Create<TCreateType>(this IFactoryCreatable<TCreateType, EmptyFactoryContext> factory)
		{
			if (factory == null) throw new ArgumentNullException(nameof(factory));

			return factory.Create(EmptyFactoryContext.Instance);
		}
	}
}
