using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Glader
{
	/// <summary>
	/// Event fired by the framework when a TextField has a submit event.
	/// </summary>
	public sealed record OnTextFieldSubmitEventArgs(IUIInputField InputField, string Text)
		: IEventBusEventArgs;
}
