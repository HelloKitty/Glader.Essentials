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
		/// Sets the underlying OBJECT of the <see cref="IUIElement"/> to the provided
		/// <see cref="state"/> value.
		/// </summary>
		/// <param name="state">The state to set the UI element's OBJECT to.</param>
		void SetObjectActive(bool state);

		/// <summary>
		/// Sets the underlying COMPONENT of the <see cref="IUIElement"/> to the provided
		/// <see cref="state"/> value.
		/// </summary>
		/// <param name="state">The state to set the UI element's COMPONENT to.</param>
		void SetComponentActive(bool state);

		/// <summary>
		/// Indicates if the element's underlying OBJECT is active.
		/// </summary>
		bool IsObjectActive { get; }

		/// <summary>
		/// Indicates if the element's underlying COMPONENT is active.
		/// </summary>
		bool IsComponentActive { get; }
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
