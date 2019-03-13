using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Metadata attributed used to indicate that
	/// a particular object should be created in a specified Scene type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public abstract class SceneTypeCreateAttribute : Attribute
	{
		/// <summary>
		/// Indicates the scene type this handler is for.
		/// </summary>
		public int SceneType { get; }

		/// <inheritdoc />
		protected SceneTypeCreateAttribute(int sceneType)
		{
			if(sceneType < 0) throw new ArgumentOutOfRangeException(nameof(sceneType));

			SceneType = sceneType;
		}
	}
}
