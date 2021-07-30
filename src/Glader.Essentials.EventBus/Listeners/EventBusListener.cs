using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Base type for an event listener that listens to a single event type.
	/// Will register a callback <see cref="OnEventFired"/> to the event on <see cref="TEventArgsType"/>.
	/// </summary>
	/// <typeparam name="TEventArgsType">The event type.</typeparam>
	public abstract class EventBusListener<TEventArgsType> : IDisposable
		where TEventArgsType : IEventBusEventArgs
	{
		private object _SyncObj { get; } = new object();

		/// <summary>
		/// Indicates if the listener is subscribed to the <see cref="IEventBus"/>.
		/// </summary>
		protected bool IsSubscribed { get; private set; } = false;

		/// <summary>
		/// The event subscription.
		/// </summary>
		private IDisposable Subscription { get; }

		/// <summary>
		/// Creates an registers the event in the provided <see cref="bus"/>.
		/// </summary>
		/// <param name="bus">The bus to register to.</param>
		protected EventBusListener(IEventBus bus)
		{
			if (bus == null) throw new ArgumentNullException(nameof(bus));

			//This implementation doesn't depend on IGameInitializable and subscribes in the ctor.
			Subscription = Subscribe(bus);
		}

		/// <summary>
		/// Called when the event bus fires an event.
		/// </summary>
		/// <param name="sender">The sender of the event (may be null).</param>
		/// <param name="args">The event args.</param>
		protected internal abstract void OnEventFired(object sender, TEventArgsType args);

		//TODO: Doc exceptions/warnings.
		/// <summary>
		/// Registers the event handler <see cref="OnEventFired"/> to the
		/// <see cref="IEventBus"/>.
		/// </summary>
		private EventBusSubscriptionToken Subscribe(IEventBus bus)
		{
			if (bus == null) throw new ArgumentNullException(nameof(bus));

			lock(_SyncObj)
			{
				if(IsSubscribed)
					throw new InvalidOperationException($"Cannot {nameof(Subscribe)} multiple times in {GetType().Name}. Subscriptions should only occur once..");

				IsSubscribed = true;
				return bus.Subscribe<TEventArgsType>(OnEventFired);;
			}
		}

		/// <summary>
		/// Unregister the event handler <see cref="OnEventFired"/> from the
		/// <see cref="IEventBus"/>.
		/// </summary>
		protected void Unsubscribe()
		{
			lock(_SyncObj)
			{
				if(!IsSubscribed)
					throw new InvalidOperationException($"Cannot {nameof(Unsubscribe)} in {GetType().Name} without already being subscribed.");

				InternalUnsubscribe();
			}
		}

		/// <summary>
		/// Handles unsubscription from the event.
		/// </summary>
		private void InternalUnsubscribe()
		{
			lock (_SyncObj)
			{
				Subscription?.Dispose();
				IsSubscribed = false;
			}
		}

		/// <inheritdoc />
		public virtual void Dispose()
		{
			InternalUnsubscribe();
		}
	}
}