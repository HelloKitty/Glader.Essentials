﻿using System;
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
			if(bus == null) throw new ArgumentNullException(nameof(bus));
			if(token == null) throw new ArgumentNullException(nameof(token));

			return token.Unsubscribe();
		}

		/// <summary>
		/// Publishes the specified event to any subscribers for the event type.
		/// Creates a new instance of the parameterlessly constructed <typeparamref name="TEventType"/> as the event to publish.
		/// </summary>
		/// <typeparam name="TEventType">The type of event</typeparam>
		/// <param name="bus">The event bus to publish from.</param>
		/// <param name="sender">The sender of the event.</param>
		public static void PublishSimple<TEventType>(this IEventBusPublishable bus, object sender)
			where TEventType : IEventBusEventArgs, new()
		{
			if(bus == null) throw new ArgumentNullException(nameof(bus));

			bus.Publish(sender, new TEventType());
		}

		/// <summary>
		/// Subscribes to the specified event type to forward the event to the provided <see cref="IEventBusSubscribable"/> <see cref="bus"/>.
		/// </summary>
		/// <typeparam name="TEventType">The type of event</typeparam>
		/// <param name="bus">The event bus to subscribe to.</param>
		/// <param name="forwardTarget">The event bus that the events should be forwarded to.</param>
		/// <returns>A <see cref="EventBusSubscriptionToken"/> to be used when calling <see cref="IEventBusSubscribable.Unsubscribe{TEventType}"/></returns>
		public static EventBusSubscriptionToken SubscribeForwarded<TEventType>(this IEventBusSubscribable bus, IEventBusPublishable forwardTarget)
			where TEventType : IEventBusEventArgs
		{
			if(bus == null) throw new ArgumentNullException(nameof(bus));
			if(forwardTarget == null) throw new ArgumentNullException(nameof(forwardTarget));

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
		/// <returns>A <see cref="EventBusSubscriptionToken"/> to be used when calling <see cref="IEventBusSubscribable.Unsubscribe{TEventType}"/></returns>
		public static EventBusSubscriptionToken SubscribeForwarded<TEventType>(this IEventBusSubscribable bus, EventHandler<EventBusEventForwardable<TEventType>> action)
			where TEventType : IEventBusEventArgs
		{
			if(bus == null) throw new ArgumentNullException(nameof(bus));
			if(action == null) throw new ArgumentNullException(nameof(action));

			return bus.Subscribe<TEventType>((s, e) => action(s, new EventBusEventForwardable<TEventType>(s, e)));
		}

		/// <summary>
		/// Subscribes all event types to the specified action.
		/// </summary>
		/// <param name="bus">The event bus to subscribe to.</param>
		/// <param name="action">The Action to invoke when an event of this type is published</param>
		/// <returns></returns>
		public static EventBusSubscriptionToken SubscribeAll(this IEventBusSubscribable bus, EventHandler<IEventBusEventArgs> action)
		{
			if(bus == null) throw new ArgumentNullException(nameof(bus));
			if(action == null) throw new ArgumentNullException(nameof(action));

			//Important to specify mode all
			return bus.Subscribe<IEventBusEventArgs>(action, EventBusSubscriptionMode.All);
		}

		/// <summary>
		/// Subscribes to all exceptions generated during Publishing events of the provided <see cref="IEventBusSubscribable"/> <see cref="bus"/>.
		/// </summary>
		/// <param name="bus">The event bus to subscribe to.</param>
		/// <param name="action">The Action to invoke when an event of this type is published</param>
		/// <returns></returns>
		public static EventBusSubscriptionToken SubscribeException(this IEventBusSubscribable bus, EventHandler<ExceptionEventBusEventArgs> action)
		{
			if(bus == null) throw new ArgumentNullException(nameof(bus));
			if(action == null) throw new ArgumentNullException(nameof(action));

			//Important to specify mode all
			return bus.Subscribe<ExceptionEventBusEventArgs>(action, EventBusSubscriptionMode.Exception);
		}

		/// <summary>
		/// Subscribes to the specified event type with the specified action.
		/// Only publishes the event if the sender of the event is the provided <see cref="sender"/>.
		/// This is computed by reference.
		/// </summary>
		/// <typeparam name="TEventType">The type of event</typeparam>
		/// <typeparam name="TSenderType">The type of the sender to subscribe the event for.</typeparam>
		/// <param name="bus">The event bus to subscribe to.</param>
		/// <param name="sender">The sender to pair to the subscription.</param>
		/// <param name="action">The Action to invoke when an event of this type is published</param>
		/// <param name="mode">The subscription mode to use.</param>
		/// <returns>A <see cref="EventBusSubscriptionToken"/> to be used when calling <see cref="IEventBusSubscribable.Unsubscribe{TEventType}"/></returns>
		public static EventBusSubscriptionToken Subscribe<TEventType, TSenderType>(this IEventBusSubscribable bus, TSenderType sender, GenericSenderEventHandler<TSenderType, TEventType> action, EventBusSubscriptionMode mode = EventBusSubscriptionMode.Default)
			where TEventType : IEventBusEventArgs
			where TSenderType : class
		{
			if(action == null) throw new ArgumentNullException(nameof(action));

			//TODO: Find a more efficient way to subscribe sender-specific events
			return bus.Subscribe<TEventType>((s, args) =>
			{
				//Don't fire the event unless the sender if the specified sender
				if(ReferenceEquals(s, sender))
					action(sender, args);
			}, mode);
		}
	}
}
