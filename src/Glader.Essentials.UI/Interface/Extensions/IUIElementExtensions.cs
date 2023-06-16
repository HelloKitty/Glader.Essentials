using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Glader.Essentials
{
	public static class IUIElementExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static EventBusSubscriptionToken CreateSubscription<TEventType>(IUIEventListenable listenable, Action<object, TEventType> callback)
			where TEventType : IEventBusEventArgs
		{
			return listenable.Bus
				.Subscribe<TEventType>((sender, args) => callback?.Invoke(sender, args));
		}

		/// <summary>
		/// Subscribes a callback for the button's click.
		/// </summary>
		/// <param name="element">The button to subscribe to.</param>
		/// <param name="callback">The callback to register.</param>
		public static EventBusSubscriptionToken OnClick(this IUIClickable element, Action<object, OnElementClickedEventArgs> callback)
		{
			return CreateSubscription(element, callback);
		}

		/// <summary>
		/// Subscribes a callback for the toggle's state change.
		/// </summary>
		/// <param name="element">The toggle to subscribe to.</param>
		/// <param name="callback">The callback to register.</param>
		public static EventBusSubscriptionToken OnToggle(this IUIToggle element, Action<object, OnToggleStateChangedEventArgs> callback)
		{
			return CreateSubscription(element, callback);
		}
	}
}
