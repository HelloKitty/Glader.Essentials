using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Default implementation of <see cref="IEventBusSubscription"/>.
	/// </summary>
	/// <typeparam name="TEventType">The event type to publish.</typeparam>
	internal sealed class EventBusSubscription<TEventType> : IEventBusSubscription 
		where TEventType : IEventBusEventArgs
	{
		/// <inheritdoc />
		public SubscriptionToken Token { get; }

		/// <summary>
		/// Internally managed event reference.
		/// </summary>
		private EventHandler<TEventType> EventReference { get; }

		public EventBusSubscription(EventHandler<TEventType> eventReference, SubscriptionToken token)
		{
			EventReference = eventReference ?? throw new ArgumentNullException(nameof(eventReference));
			Token = token ?? throw new ArgumentNullException(nameof(token));
		}

		public EventBusSubscription(EventHandler<TEventType> eventReference, IEventBus bus)
			: this(eventReference, new GenericSubscriptionToken<TEventType>(bus))
		{

		}

		/// <inheritdoc />
		public void Publish(object sender, IEventBusEventArgs eventData)
		{
			EventReference?.Invoke(sender, (TEventType) eventData);
		}
	}
}
