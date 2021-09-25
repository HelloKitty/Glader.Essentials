using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event fired by the UI system when a toggle-able element has changed states.
	/// </summary>
	public record OnToggleStateChangedEventArgs(bool State) : IEventBusEventArgs;
}
