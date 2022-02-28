using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for UI that can receive events.
	/// Implementing this interface allows for the Unity3D engine to callback.
	/// </summary>
	public interface IUIMouseOverEnterEventListener : IPointerEnterHandler
	{

	}
}
