using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for UI types that can be clicked.
	/// These elements can publish <see cref="OnElementClickedEventArgs"/>.
	/// </summary>
	public interface IUIClickable : IUIEventListenable
	{
		/// <summary>
		/// Simulates a UI click on the clickable.
		/// </summary>
		/// <param name="eventsOnly">Indicates if ONLY the events on a click should occur and not any animations for the UI element.</param>
		void SimulateClick(bool eventsOnly);
	}
}
