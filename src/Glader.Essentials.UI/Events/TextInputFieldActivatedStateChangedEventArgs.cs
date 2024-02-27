using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event fired by text input elements when the activation state of the input field changes.
	/// </summary>
	public sealed record TextInputFieldActivatedStateChangedEventArgs(bool Active)
		: IEventBusEventArgs;
}
