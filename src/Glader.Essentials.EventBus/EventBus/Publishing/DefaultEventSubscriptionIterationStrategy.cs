using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IEnumerable<IEventBusSubscription> Enumerate(IEventBusSubscription[] subscriptions)
		{
			return subscriptions;
		}
	}
}
