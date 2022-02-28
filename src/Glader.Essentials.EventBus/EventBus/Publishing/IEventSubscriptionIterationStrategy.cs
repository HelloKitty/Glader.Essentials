using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Glader.Essentials
{
	/// <summary>
	/// Strategy for iteration of the event subscription.
	/// </summary>
	public interface IEventSubscriptionIterationStrategy
	{
		/// <summary>
		/// Enumerates the provided subscriptions.
		/// </summary>
		/// <param name="subscriptions">The subscriptions to iterate.</param>
		/// <returns>Enumerate subscription.</returns>
		IEnumerable<IEventBusSubscription> Enumerate(IEventBusSubscription[] subscriptions);
	}
}
