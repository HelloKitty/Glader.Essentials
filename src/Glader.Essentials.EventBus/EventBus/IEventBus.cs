using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for types that implement the concept of an EventBus.
	/// Supports publishing, subscribing and unsubscribing to event types.
	/// </summary>
	public interface IEventBus : IEventBusPublishable
	{
		/// <summary>
		/// Subscribes to the specified event type with the specified action.
		/// </summary>
		/// <typeparam name="TEventType">The type of event</typeparam>
		/// <param name="action">The Action to invoke when an event of this type is published</param>
		/// <param name="mode">The subscription mode to use.</param>
		/// <returns>A <see cref="EventBusSubscriptionToken"/> to be used when calling <see cref="Unsubscribe{TEventType}"/></returns>
		EventBusSubscriptionToken Subscribe<TEventType>(EventHandler<TEventType> action, EventBusSubscriptionMode mode = EventBusSubscriptionMode.Default) 
			where TEventType : IEventBusEventArgs;

		/// <summary>
		/// Unsubscribe from the Event type related to the specified <see cref="EventBusSubscriptionToken"/>
		/// </summary>
		/// <param name="token">The <see cref="EventBusSubscriptionToken"/> received from calling the Subscribe method</param>
		/// <returns>Indicates if a subscription was removed (if token actually was registered)</returns>
		bool Unsubscribe<TEventType>(EventBusSubscriptionToken token)
			where TEventType : IEventBusEventArgs;
	}
}
