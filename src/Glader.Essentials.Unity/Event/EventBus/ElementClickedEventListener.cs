using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event listener that will subscribe to the provided element's click event.
	/// Using <see cref="OnElementClickedEventArgs"/> with <see cref="IUIElement"/> as the source.
	/// </summary>
	public abstract class ElementClickedEventListener : EventBusListener<OnElementClickedEventArgs, IUIElement>
	{
		/// <summary>
		/// Creates a new click listener on the provided element.
		/// </summary>
		/// <param name="element">Element to listen on.</param>
		protected ElementClickedEventListener(IUIElement element) 
			: base(element.Bus)
		{

		}
	}

	/// <summary>
	/// Async Event listener that will subscribe to the provided element's click event.
	/// Using <see cref="OnElementClickedEventArgs"/> with <see cref="IUIElement"/> as the source.
	/// </summary>
	public abstract class ElementClickedEventListenerAsync : UnityAsyncEventBusListener<OnElementClickedEventArgs, IUIElement>
	{
		/// <summary>
		/// Creates a new click listener on the provided element.
		/// </summary>
		/// <param name="element">Element to listen on.</param>
		protected ElementClickedEventListenerAsync(IUIElement element)
			: base(element.Bus)
		{

		}
	}
}
