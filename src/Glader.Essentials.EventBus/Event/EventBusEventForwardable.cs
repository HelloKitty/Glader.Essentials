using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// A data container for forwardable events.
	/// </summary>
	/// <typeparam name="TEventType">The event type.</typeparam>
	public sealed class EventBusEventForwardable<TEventType> : IEventBusEventForwardable 
		where TEventType : IEventBusEventArgs
	{
		/// <inheritdoc />
		public object Sender { get; }

		/// <summary>
		/// The event data.
		/// </summary>
		public TEventType EventData { get; }

		/// <summary>
		/// Creates a new forwardable event that can be forwarded to a new <see cref="IEventBus"/> using <see cref="ForwardTo"/>.
		/// </summary>
		/// <param name="sender">The original event sender.</param>
		/// <param name="eventData">The original event data.</param>
		public EventBusEventForwardable(object sender, TEventType eventData)
		{
			Sender = sender;
			EventData = eventData ?? throw new ArgumentNullException(nameof(eventData));
		}

		/// <inheritdoc />
		public void ForwardTo(IEventBus bus)
		{
			if (bus == null) throw new ArgumentNullException(nameof(bus));
			bus.Publish(Sender, EventData);
		}
	}
}
