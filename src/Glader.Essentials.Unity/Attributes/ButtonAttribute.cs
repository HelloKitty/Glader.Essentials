using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Unity3D meta-data attribute that indicates the function should be a clickable button
	/// in the Unity3D Editor.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class ButtonAttribute : Attribute
	{

	}
}
