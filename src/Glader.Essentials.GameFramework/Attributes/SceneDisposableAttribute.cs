using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Indicates a <see cref="SceneTypeCreateAttribute"/> marked with <see cref="IDisposable"/>
	/// should have it registered as Disposable.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class SceneDisposableAttribute : Attribute
	{

	}
}
