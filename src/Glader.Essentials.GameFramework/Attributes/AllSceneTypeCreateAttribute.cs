using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Metadata attributed used to indicate that
	/// a particular object should be created in all scenes.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class AllSceneTypeCreateAttribute : Attribute
	{

	}
}
