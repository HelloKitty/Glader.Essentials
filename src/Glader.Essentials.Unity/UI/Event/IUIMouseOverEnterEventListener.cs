using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for UI that can receive events.
	/// </summary>
	public interface IUIMouseOverEnterEventListener
	{
		/// <summary>
		/// See: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseOver.html
		/// </summary>
		void OnMouseOver();
	}
}
