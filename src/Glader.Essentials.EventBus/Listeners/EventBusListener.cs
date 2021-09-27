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
		/// Internally called event method raised by the event bus.
		/// Handles before/after logic.
		/// </summary>
		/// <param name="sender">The sender of the event (may be null).</param>
		/// <param name="args">The event args.</param>
		private void InternalOnEventFired(object sender, TEventArgsType args)
		{
			// If OnBefore returns false then we don't continue handling the event.
			if(!OnBeforeEventFired(sender, args))
				return;

			HandleEvent(sender, args);
		}

		/// <summary>
		/// Called internally to service an event after <see cref="OnBeforeEventFired"/>.
		/// </summary>
		/// <param name="sender">The sender of the event (may be null).</param>
		/// <param name="args">The event args.</param>
		protected void HandleEvent(object sender, TEventArgsType args)
		{
			bool successful = true;
			try
			{
				OnEventFired(sender, args);
			}
			catch (Exception e)
			{
				successful = false;
				OnException(sender, args, e);
			}
			finally
			{
				OnAfterEventFired(sender, args, successful);
			}
		}

		/// <summary>
		/// Implementer can override this function to handle encountered exceptions.
		/// </summary>
		/// <param name="sender">The sender of the event (may be null).</param>
		/// <param name="args">The event args.</param>
		/// <param name="error"></param>
		protected virtual void OnException(object sender, TEventArgsType args, Exception error)
		{
			return;
		}

		/// <summary>
		/// Implementer can override this function to handle post-event handling logic.
		/// Always called, regardless of if an error was encountered.
		/// </summary>
		/// <param name="sender">The sender of the event (may be null).</param>
		/// <param name="args">The event args.</param>
		/// <param name="successful">Indicates if the event was handled without exception.</param>
		protected virtual void OnAfterEventFired(object sender, TEventArgsType args, bool successful)
		{
			return;
		}

		/// <summary>
		/// Implementer can override this function to handle pre-event handling logic.
		/// Return false to prevent continue of handling of the event.
		/// Exceptions thrown here are uncaught by the handler.
		/// </summary>
		/// <param name="sender">The sender of the event (may be null).</param>
		/// <param name="args">The event args.</param>
		/// <returns>Indicates if the event should be handled.</returns>
		protected virtual bool OnBeforeEventFired(object sender, TEventArgsType args)
		{
			return true;
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
				return bus.Subscribe<TEventArgsType>(InternalOnEventFired);
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