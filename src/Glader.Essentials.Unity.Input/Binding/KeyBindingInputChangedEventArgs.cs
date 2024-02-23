using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event fired when a keybinding is pressed.
	/// </summary>
	public sealed record KeyBindingInputChangedEventArgs<TBindingEnumType>(TBindingEnumType Type, bool Down) : IEventBusEventArgs
		where TBindingEnumType : Enum;
}
