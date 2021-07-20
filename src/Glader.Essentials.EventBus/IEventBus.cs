using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public static class IEventBusExtensions
	{
		/// <summary>
		/// Unsubscribe from the Event type related to the specified <see cref="SubscriptionToken"/>
		/// </summary>
		/// <param name="bus">The event bus to unsubscribe from.</param>
		/// <param name="token">The <see cref="SubscriptionToken"/> received from calling the Subscribe method</param>
		/// <returns>Indicates if a subscription was removed (if token actually was registered)</returns>
		public static bool Unsubscribe(this IEventBus bus, SubscriptionToken token)
		{
			if (bus == null) throw new ArgumentNullException(nameof(bus));
			if (token == null) throw new ArgumentNullException(nameof(token));

			return token.Unsubscribe();
		}

		/// <summary>
		/// Publishes the specified event to any subscribers for the event type.
		/// Creates a new instance of the parameterlessly constructed <typeparamref name="TEventType"/> as the event to publish.
		/// </summary>
		/// <typeparam name="TEventType">The type of event</typeparam>
		/// <param name="bus">The event bus to publish from.</param>
		/// <param name="sender">The sender of the event.</param>
		public static void Publish<TEventType>(this IEventBus bus, object sender)
			where TEventType : IEventBusEventArgs, new()
		{
			if (bus == null) throw new ArgumentNullException(nameof(bus));

			bus.Publish(sender, new TEventType());
		}
	}

	/// <summary>
	/// Contract for types that implement the concept of an EventBus.
	/// Supports publishing, subscribing and unsubscribing to event types.
	/// </summary>
	public interface IEventBus
	{
		/// <summary>
		/// Subscribes to the specified event type with the specified action.
		/// </summary>
		/// <typeparam name="TEventType">The type of event</typeparam>
		/// <param name="action">The Action to invoke when an event of this type is published</param>
		/// <returns>A <see cref="SubscriptionToken"/> to be used when calling <see cref="Unsubscribe"/></returns>
		SubscriptionToken Subscribe<TEventType>(EventHandler<TEventType> action) 
			where TEventType : IEventBusEventArgs;

		/// <summary>
		/// Unsubscribe from the Event type related to the specified <see cref="SubscriptionToken"/>
		/// </summary>
		/// <param name="token">The <see cref="SubscriptionToken"/> received from calling the Subscribe method</param>
		/// <returns>Indicates if a subscription was removed (if token actually was registered)</returns>
		bool Unsubscribe<TEventType>(SubscriptionToken token)
			where TEventType : IEventBusEventArgs;

		/// <summary>
		/// Publishes the specified event to any subscribers for the event type.
		/// </summary>
		/// <typeparam name="TEventType">The type of event</typeparam>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="eventData">Event to publish</param>
		void Publish<TEventType>(object sender, TEventType eventData) 
			where TEventType : IEventBusEventArgs;
	}
}
