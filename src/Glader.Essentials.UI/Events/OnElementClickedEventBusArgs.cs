using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements;

namespace Glader.Essentials
{
	/// <summary>
	/// Event fired by the UI system when a clickable element has been clicked.
	/// </summary>
	public record OnElementClickedEventArgs(MouseButton Button, bool IsDown) 
		: IEventBusEventArgs;
}
