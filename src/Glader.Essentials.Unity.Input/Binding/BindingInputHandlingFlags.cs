using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	[Flags]
	public enum BindingInputHandlingFlags
	{
		None = 0,
		OnPressed = 1 << 0,
		OnReleased = 1 << 1
	}
}
