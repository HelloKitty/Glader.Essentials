using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Default implementation of <see cref="IEventSubscriptionPublishStrategy"/>.
	/// </summary>
	public sealed class DefaultEventSubscriptionPublishStrategy : IEventSubscriptionPublishStrategy
	{
		/// <summary>
		/// Shared instance of <see cref="DefaultEventSubscriptionPublishStrategy"/>
		/// </summary>
		public static DefaultEventSubscriptionPublishStrategy Instance { get; } = new();

		/// <inheritdoc />
		public bool Publish<TEventType>(IEventBusSubscription subscription, object sender, TEventType eventData) where TEventType : IEventBusEventArgs
		{
			//The subscription could be null because we REUSE indexes and set them as null
			//to avoid allocations if possible on unsubscription or subscriptions
			subscription?.Publish(sender, eventData);
			return true;
		}
	}
}
