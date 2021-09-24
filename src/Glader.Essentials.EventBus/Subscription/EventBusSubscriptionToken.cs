using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// A Token representing a Subscription.
	/// </summary>
	public abstract class EventBusSubscriptionToken : IDisposable
	{
		/// <summary>
		/// The event type of the subscription.
		/// </summary>
		public Type EventType { get; }

		/// <summary>
		/// The <see cref="IEventBus"/> reference this subscription token was produced from.
		/// </summary>
		protected IEventBus Bus { get; set; }

		// To detect redundant calls
		public bool Disposed { get; protected set; } = false;

		/// <summary>
		/// The mode of the subscription
		/// </summary>
		internal EventBusSubscriptionMode Mode { get; }

		protected EventBusSubscriptionToken(Type eventType, IEventBus bus, EventBusSubscriptionMode mode)
		{
			//We don't verify enum mode for perf
			EventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
			Bus = bus ?? throw new ArgumentNullException(nameof(bus));
			Mode = mode;
		}

		/// <summary>
		/// Dispatching the token unsubscription to the associated <see cref="bus"/>.
		/// </summary>
		/// <returns></returns>
		public abstract bool Unsubscribe();

		/// <inheritdoc />
		public abstract void Dispose();
	}

	internal class GenericSubscriptionToken<TEventType> : EventBusSubscriptionToken 
		where TEventType : IEventBusEventArgs
	{
		public GenericSubscriptionToken(IEventBus bus, EventBusSubscriptionMode mode) 
			: base(typeof(TEventType), bus, mode)
		{

		}

		/// <inheritdoc />
		public override bool Unsubscribe()
		{
			if(Disposed)
				return true;

			return Bus.Unsubscribe<TEventType>(this);
		}

		//Dispose pattern: https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
		public override void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}

		//A correctly-written program cannot assume that finalizers will ever run.
		~GenericSubscriptionToken() => Dispose(false);

		protected void Dispose(bool disposing)
		{
			if (Disposed)
				return;

			Unsubscribe();
			Bus = null; // important for if anyone is holding a ref to this token still the Bus can de allocate.
			Disposed = true;
		}
	}
}
