using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for UI buttons.
	/// </summary>
	public interface IUIButton
	{
		/// <summary>
		/// Registers a callback for the button OnClick.
		/// </summary>
		/// <param name="action">The callback to register.</param>
		void AddOnClickListener(Action action);

		/// <summary>
		/// Registers an async callback for the button OnClick.
		/// </summary>
		/// <param name="action">The asynccallback to register.</param>
		void AddOnClickListenerAsync(Func<Task> action);

		/// <summary>
		/// Indicates if the button is interactable.
		/// </summary>
		bool IsInteractable { get; set; }

		/// <summary>
		/// Simulates a UI click on the button.
		/// </summary>
		/// <param name="eventsOnly">Indicates if ONLY the events on a click should occur and not any animations for the UI element.</param>
		void SimulateClick(bool eventsOnly);
	}
}
