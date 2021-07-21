using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public static class IEventBusExtensions
	{
		/// <summary>
		/// Unsubscribe from the Event type related to the specified <see cref="EventBusSubscriptionToken"/>
		/// </summary>
		/// <param name="bus">The event bus to unsubscribe from.</param>
		/// <param name="token">The <see cref="EventBusSubscriptionToken"/> received from calling the Subscribe method</param>
		/// <returns>Indicates if a subscription was removed (if token actually was registered)</returns>
		public static bool Unsubscribe(this IEventBus bus, EventBusSubscriptionToken token)
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
		public static void PublishSimple<TEventType>(this IEventBus bus, object sender)
			where TEventType : IEventBusEventArgs, new()
		{
			if (bus == null) throw new ArgumentNullException(nameof(bus));

			bus.Publish(sender, new TEventType());
		}

		/// <summary>
		/// Subscribes to the specified event type to forward the event to the provided <see cref="IEventBus"/> <see cref="bus"/>.
		/// </summary>
		/// <typeparam name="TEventType">The type of event</typeparam>
		/// <param name="bus">The event bus to subscribe to.</param>
		/// <param name="forwardTarget">The event bus that the events should be forwarded to.</param>
		/// <returns>A <see cref="EventBusSubscriptionToken"/> to be used when calling <see cref="IEventBus.Unsubscribe{TEventType}"/></returns>
		public static EventBusSubscriptionToken SubscribeForwarded<TEventType>(this IEventBus bus, IEventBus forwardTarget)
			where TEventType : IEventBusEventArgs
		{
			if (bus == null) throw new ArgumentNullException(nameof(bus));
			if (forwardTarget == null) throw new ArgumentNullException(nameof(forwardTarget));

			return bus.Subscribe<TEventType>(forwardTarget.Publish, EventBusSubscriptionMode.Forwarded);
		}

		/// <summary>
		/// Subscribes to the specified event type to automatically produce a <see cref="EventBusEventForwardable{TEventType}"/> event message
		/// published to the specified handler. Rather than publish a the event directly the <see cref="IEventBus"/> will publish a <see cref="EventBusEventForwardable{TEventType}"/> message.
		/// (Ex. You may want to use this if you don't want to automatically forward a message to another <see cref="IEventBus"/>.
		/// </summary>
		/// <typeparam name="TEventType">The type of event</typeparam>
		/// <param name="bus">The event bus to subscribe to.</param>
		/// <param name="action">The forwardable event message handler.</param>
		/// <returns>A <see cref="EventBusSubscriptionToken"/> to be used when calling <see cref="IEventBus.Unsubscribe{TEventType}"/></returns>
		public static EventBusSubscriptionToken SubscribeForwarded<TEventType>(this IEventBus bus, EventHandler<EventBusEventForwardable<TEventType>> action)
			where TEventType : IEventBusEventArgs
		{
			if(bus == null) throw new ArgumentNullException(nameof(bus));
			if (action == null) throw new ArgumentNullException(nameof(action));

			return bus.Subscribe<TEventType>((s, e) => action(s, new EventBusEventForwardable<TEventType>(s, e)));
		}

		/// <summary>
		/// Subscribes all event types to the specified action.
		/// </summary>
		/// <param name="bus"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static EventBusSubscriptionToken SubscribeAll(this IEventBus bus, EventHandler<IEventBusEventArgs> action)
		{
			if (bus == null) throw new ArgumentNullException(nameof(bus));
			if (action == null) throw new ArgumentNullException(nameof(action));

			//Important to specify mode all
			return bus.Subscribe<IEventBusEventArgs>(action, EventBusSubscriptionMode.All);
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
