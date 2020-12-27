using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Base event listener type. Implements reflection-based event subscription.
	/// </summary>
	/// <typeparam name="TSubscribableType">The subscribable type.</typeparam>
	/// <typeparam name="THandlerType">The handler type.</typeparam>
	/// <typeparam name="TEventArgsType">The event args for the event.</typeparam>
	public abstract class SharedBaseEventListener<TSubscribableType, THandlerType, TEventArgsType>
		where TSubscribableType : class
		where THandlerType : Delegate
		where TEventArgsType : EventArgs
	{
		private object SyncObj = new object();

		/// <summary>
		/// The cached efficient delegate pointing to the Add method of an Event for registering a handler.
		/// </summary>
		private static Action<TSubscribableType, THandlerType> CachedEventRegistrationDelegate { get; }

		/// <summary>
		/// The cached efficient delegate pointing to the Add method of an Event for registering a handler.
		/// </summary>
		private static Action<TSubscribableType, THandlerType> CachedEventRemoveDelegate { get; }

		/// <summary>
		/// Subscription service containing a <see cref="EventHandler"/>.
		/// </summary>
		protected internal TSubscribableType SubscriptionService { get; }

		/// <summary>
		/// Indicates if the listener is subscribed to the <see cref="SubscriptionService"/>.
		/// </summary>
		protected bool isSubscribed { get; private set; }

		/// <inheritdoc />
		internal SharedBaseEventListener(TSubscribableType subscriptionService)
		{
			SubscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
		}

		static SharedBaseEventListener()
		{
			EventInfo[] events = typeof(TSubscribableType)
				.GetEvents(BindingFlags.Public | BindingFlags.Instance);

			events = events
				.Where(e => IsCorrectEventSignature(e))
				.ToArray();

			//This supports the case of base interface: https://social.msdn.microsoft.com/Forums/vstudio/en-US/8ebd55e5-d89a-452b-b2db-deebb7a97b4d/how-can-i-get-methods-defined-in-base-interface-via-typegetmethods-while-the-type-is-a-derived?forum=csharpgeneral
			//If none are found, there may be the case of base events.
			if(events.Length == 0)
				foreach(var baseInterfaceType in typeof(TSubscribableType).GetInterfaces())
				{
					events = baseInterfaceType
						.GetEvents(BindingFlags.Public | BindingFlags.Instance);

					events = events
						.Where(e => IsCorrectEventSignature(e))
						.ToArray();

					//Go through each interface until we find one.
					if(events.Length > 0)
						break;
				}

			if(events.Length != 1)
				throw new InvalidOperationException($"Events Found: {events.Length} Cannot specify: {typeof(TSubscribableType).Name} as SingleEvent with Args: {typeof(EventArgs)} because: {ComputeErrorMessage(events)}");

			//If we've made it here, there is ONE event in the collection
			//and it fits the requirements
			CachedEventRegistrationDelegate = (Action<TSubscribableType, THandlerType>)events[0]
				.AddMethod.CreateDelegate(typeof(Action<TSubscribableType, THandlerType>));

			CachedEventRemoveDelegate = (Action<TSubscribableType, THandlerType>)events[0]
				.RemoveMethod.CreateDelegate(typeof(Action<TSubscribableType, THandlerType>));
		}

		private static bool IsCorrectEventSignature(EventInfo e)
		{
			return e.EventHandlerType == typeof(THandlerType);
		}

		private static string ComputeErrorMessage(EventInfo[] events)
		{
			return events.Length > 1 ? "Multiple events have the same Type signature" : "No event matches the type signature";
		}

		/// <summary>
		/// Called when the subscription service fires an event.
		/// </summary>
		/// <param name="source">The calling source.</param>
		/// <param name="args"></param>
		protected abstract void OnEventFired(object source, TEventArgsType args);

		//TODO: Doc exceptions/warnings.
		/// <summary>
		/// Unregisters the event handler <see cref="OnEventFired"/> from the
		/// <see cref="SubscriptionService"/>.
		/// </summary>
		protected void Unsubscribe()
		{
			lock(SyncObj)
			{
				if(!isSubscribed)
					throw new InvalidOperationException($"Cannot {nameof(Unsubscribe)} in {GetType().Name} without already being subscribed.");

				HandleOnEventFiredCast(CachedEventRemoveDelegate);
				isSubscribed = false;
			}
		}

		//TODO: Doc exceptions/warnings.
		/// <summary>
		/// Registers the event handler <see cref="OnEventFired"/> to the
		/// <see cref="SubscriptionService"/>.
		/// </summary>
		protected void Subscribe()
		{
			lock(SyncObj)
			{
				if(isSubscribed)
					throw new InvalidOperationException($"Cannot {nameof(Subscribe)} multiple times in {GetType().Name}. Subscriptions should only occur once..");

				HandleOnEventFiredCast(CachedEventRegistrationDelegate);
				isSubscribed = true;
			}
		}

		protected internal abstract void HandleOnEventFiredCast(Action<TSubscribableType, THandlerType> targetSubscriptionMethod);
	}
}
