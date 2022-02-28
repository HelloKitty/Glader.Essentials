using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Default implementation of <see cref="IEventSubscriptionIterationStrategy"/>.
	/// </summary>
	public sealed class DefaultEventSubscriptionIterationStrategy : IEventSubscriptionIterationStrategy
	{
		/// <summary>
		/// Shared instance of <see cref="DefaultEventSubscriptionIterationStrategy"/>
		/// </summary>
		public static DefaultEventSubscriptionIterationStrategy Instance { get; } = new();

		/// <inheritdoc />
		public IEnumerable<IEventBusSubscription> Enumerate(IEventBusSubscription[] subscriptions)
		{
			foreach (var entry in subscriptions)
				yield return entry;
		}
	}
}
