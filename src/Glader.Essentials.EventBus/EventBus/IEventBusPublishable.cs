using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for EventBus publishing functionality.
	/// </summary>
	public interface IEventBusPublishable
	{
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
