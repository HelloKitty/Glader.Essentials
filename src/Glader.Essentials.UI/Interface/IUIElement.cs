using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IUIElement
	{
		/// <summary>
		/// Sets the <see cref="IUIElement"/> to the provided
		/// <see cref="state"/> value.
		/// </summary>
		/// <param name="state">The state to set the UI element to.</param>
		void SetElementActive(bool state);

		/// <summary>
		/// Indicates if the element is active.
		/// </summary>
		bool isActive { get; }
	}
}
