using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// A Token representing a Subscription.
	/// </summary>
	public abstract class SubscriptionToken : IDisposable
	{
		/// <summary>
		/// The event type of the subscription.
		/// </summary>
		public Type EventType { get; }

		/// <summary>
		/// The <see cref="IEventBus"/> reference this subscription token was produced from.
		/// </summary>
		protected IEventBus Bus { get; }

		// To detect redundant calls
		public bool Disposed { get; protected set; } = false;

		protected SubscriptionToken(Type eventType, IEventBus bus)
		{
			EventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
			Bus = bus ?? throw new ArgumentNullException(nameof(bus));
		}

		/// <summary>
		/// Dispatching the token unsubscription to the associated <see cref="bus"/>.
		/// </summary>
		/// <returns></returns>
		public abstract bool Unsubscribe();

		/// <inheritdoc />
		public abstract void Dispose();
	}

	internal class GenericSubscriptionToken<TEventType> : SubscriptionToken 
		where TEventType : IEventBusEventArgs
	{
		public GenericSubscriptionToken(IEventBus bus) 
			: base(typeof(TEventType), bus)
		{

		}

		/// <inheritdoc />
		public override bool Unsubscribe()
		{ 
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

		~GenericSubscriptionToken() => Dispose(false);

		protected void Dispose(bool disposing)
		{
			if (Disposed)
				return;

			Bus.Unsubscribe<TEventType>(this);
			Disposed = true;
		}
	}
}
