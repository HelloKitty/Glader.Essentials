using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for types that can build an instance of
	/// <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The buildable type.</typeparam>
	public interface IBuildable<out T>
	{
		/// <summary>
		/// Builds an instance of the type.
		/// </summary>
		/// <returns>Returns the instance built.</returns>
		T Build();
	}
}
