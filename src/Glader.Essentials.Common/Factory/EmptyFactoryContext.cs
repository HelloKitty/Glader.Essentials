using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Represents an empty factory creation context.
	/// For usage with <see cref="IFactoryCreatable{TCreateType,TContextType}"/>.
	/// </summary>
	public sealed class EmptyFactoryContext
	{
		/// <summary>
		/// Singleton empty creation context.
		/// </summary>
		public static EmptyFactoryContext Instance { get; } = new EmptyFactoryContext();

		/// <summary>
		/// Enforces initialization order.
		/// Do not remove.
		/// </summary>
		static EmptyFactoryContext()
		{

		}

		/// <summary>
		/// Private ctor makes it un-constructable outside of the class.
		/// </summary>
		private EmptyFactoryContext()
		{

		}
	}
}
