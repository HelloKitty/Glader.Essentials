using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event fired by the UI system when a clickable element has been clicked.
	/// </summary>
	public record OnElementClickedEventArgs : IEventBusEventArgs;
}
