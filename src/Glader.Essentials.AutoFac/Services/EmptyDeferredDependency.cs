using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Empty/Null implementation of <see cref="IDeferredDependency{TDependencyType}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class EmptyDeferredDependency<T> : IDeferredDependency<T>
	{
		/// <summary>
		/// Instance of an empty.
		/// </summary>
		public static IDeferredDependency<T> Instance { get; } = new EmptyDeferredDependency<T>();

		/// <inheritdoc />
		public T Dependency => default;
	}
}
