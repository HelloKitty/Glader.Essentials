using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using UnityEngine;

namespace Glader.Essentials
{
	public interface IUIParentable : IUIElement
	{
		/// <summary>
		/// Parents the provided <paramref name="go"/> to
		/// the target <see cref="IUIParentable"/>.
		/// </summary>
		/// <param name="go">The game object to parent.</param>
		void Parent(GameObject go);
	}
}
