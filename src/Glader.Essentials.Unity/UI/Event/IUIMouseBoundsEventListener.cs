using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for UI that can receive events.
	/// </summary>
	public interface IUIMouseBoundsEventListener 
		: IUIMouseOverExitEventListener, IUIMouseOverEnterEventListener
	{

	}
}
