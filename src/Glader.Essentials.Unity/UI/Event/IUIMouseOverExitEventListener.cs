using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for UI that can receive events.
	/// Implementing this interface allows for the Unity3D engine to callback.
	/// </summary>
	public interface IUIMouseOverExitEventListener
	{
		/// <summary>
		/// See: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseExit.html
		/// </summary>
		void OnMouseExit();
	}
}
