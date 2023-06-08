using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event fired when a <see cref="IUIDropDown"/> selected changes.
	/// </summary>
	public sealed record DropDownSelectionChangedEventArgs(int Index, string Value)
		: IEventBusEventArgs;

}
