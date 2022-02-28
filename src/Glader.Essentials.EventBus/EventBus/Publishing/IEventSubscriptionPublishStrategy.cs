using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for event the subscription publishing strategy.
	/// </summary>
	public interface IEventSubscriptionPublishStrategy
	{
		/// <summary>
		/// Publishes the event data through the provided subscription.
		/// </summary>
		/// <typeparam name="TEventType">The event args.</typeparam>
		/// <param name="subscription">The event subscription.</param>
		/// <param name="sender">The event sender.</param>
		/// <param name="eventData">The event data.</param>
		/// <returns>True if publishing should continue.</returns>
		bool Publish<TEventType>(IEventBusSubscription subscription, object sender, TEventType eventData)
			where TEventType : IEventBusEventArgs;
	}
}
