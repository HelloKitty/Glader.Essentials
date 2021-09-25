using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for root-element type. All things that exist within a Window can be considered
	/// <see cref="IUIElement"/>. Even sub-windows (Ex <see cref="IUIFrame"/>).
	/// </summary>
	public interface IUIElement : IUIEventListenable
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
		bool IsActive { get; }
	}

	/// <summary>
	/// Contract for root-element type. All things that exist within a Window can be considered
	/// <see cref="IUIElement"/>. Even sub-windows (Ex <see cref="IUIFrame"/>).
	/// </summary>
	public interface IUIElement<TUIElementType> : IUIElement, IUIEventListenable<TUIElementType> 
		where TUIElementType : IUIEventListenable<TUIElementType>
	{

	}
}
