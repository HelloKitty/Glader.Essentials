using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader
{
	public interface IUIParentable
	{
		/// <summary>
		/// Parents the provided <see cref="go"/> to
		/// the target <see cref="IUIParentable"/>.
		/// </summary>
		/// <param name="go">The game object to parent.</param>
		void Parent(GameObject go);
	}
}
