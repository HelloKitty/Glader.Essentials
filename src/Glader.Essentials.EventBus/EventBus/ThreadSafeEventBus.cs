using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// <see cref="IEventBus"/> implementation with the concept of thread safety.
	/// Maintains the concept of a Main Thread which is the only thread that can publish and
	/// publishing from a non-main thread will enqueue it for publishing.
	/// </summary>
	public sealed class ThreadSafeEventBus : IEventBus
	{
		/// <summary>
		/// The decorated event bus.
		/// </summary>
		private IEventBus DecoratedEventBus { get; }

		/// <summary>
		/// Queue of pending events that could not be serviced due to not being on the main thread.
		/// </summary>
		private ConcurrentQueue<IEventBusEventForwardable> EnqueuedEvents { get; } = new();

		private IMainThreadDeterminable ThreadStrategy { get; }

		/// <summary>
		/// Creates a new <see cref="IEventBus"/> decorated around the provided bus.
		/// Will manage threading and thread safety issues.
		/// </summary>
		/// <param name="bus">Event bus.</param>
		/// <param name="threadStrategy">Strategy to determine if calls to publish are on the main thread.</param>
		public ThreadSafeEventBus(IEventBus bus, IMainThreadDeterminable threadStrategy)
		{
			DecoratedEventBus = bus ?? throw new ArgumentNullException(nameof(bus));
			ThreadStrategy = threadStrategy ?? throw new ArgumentNullException(nameof(threadStrategy));
			bus.Subscribe<PublishPendingEventBusEventArgs>(PublishPendingEvents);
		}

		private void PublishPendingEvents(object sender, PublishPendingEventBusEventArgs e)
		{
			// WARNING: If this throws before forwarding it will silently fail!
			// If it's the main thread we can publish
			int count = EnqueuedEvents.Count;
			for(int i = 0; i < count; i++)
				if (EnqueuedEvents.TryDequeue(out var enqueuedEvent))
					enqueuedEvent.ForwardTo(DecoratedEventBus);
		}

		/// <inheritdoc />
		public EventBusSubscriptionToken Subscribe<TEventType>(EventHandler<TEventType> action, EventBusSubscriptionMode mode = EventBusSubscriptionMode.Default) where TEventType : IEventBusEventArgs
		{
			return DecoratedEventBus.Subscribe(action, mode);
		}

		/// <inheritdoc />
		public bool Unsubscribe<TEventType>(EventBusSubscriptionToken token) where TEventType : IEventBusEventArgs
		{
			return DecoratedEventBus.Unsubscribe<TEventType>(token);
		}

		/// <inheritdoc />
		public void UnsubscribeAll()
		{
			DecoratedEventBus.UnsubscribeAll();
		}

		/// <inheritdoc />
		public void Publish<TEventType>(object sender, TEventType eventData) where TEventType : IEventBusEventArgs
		{
			if (ThreadStrategy.IsMainThread)
				DecoratedEventBus.Publish(sender, eventData);
			else
				EnqueuedEvents.Enqueue(new EventBusEventForwardable<TEventType>(sender, eventData));
		}

		/// <inheritdoc />
		public void Dispose()
		{
			DecoratedEventBus?.Dispose();

			// TODO: More might be enqueued, so this could leak.
			EnqueuedEvents.Clear();
		}
	}
}
