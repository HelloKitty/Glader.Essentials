using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for UI that can receive events.
	/// Implementing this interface allows for the Unity3D engine to callback.
	/// </summary>
	public interface IUIMouseOverEnterEventListener
	{
		/// <summary>
		/// See: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseOver.html
		/// </summary>
		void OnMouseOver();
	}
}
