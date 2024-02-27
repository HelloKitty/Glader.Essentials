using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	//From SwanSong
	/// <summary>
	/// Contract for an object that provides access to a dependency in a deferred way.
	/// </summary>
	/// <typeparam name="TDependencyType">The dependency type.</typeparam>
	public interface IDeferredDependency<out TDependencyType>
	{
		/// <summary>
		/// The dependency.
		/// </summary>
		TDependencyType Dependency { get; }
	}
}
