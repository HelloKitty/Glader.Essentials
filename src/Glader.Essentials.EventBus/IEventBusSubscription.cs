using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IEventBusSubscription
	{
		/// <summary>
		/// Unique token that 
		/// </summary>
		SubscriptionToken Token { get; }

		/// <summary>
		/// Publishes the provided event data.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="eventData"></param>
		void Publish(object sender, IEventBusEventArgs eventData);
	}
}
