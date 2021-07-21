using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for types that represent forwardable event bus events.
	/// (Ex. You may want to use this if you need to persist a forwardable message that might not be able to use IEventBus's Forwarding feature right away. Maybe on another thread)
	/// </summary>
	public interface IEventBusEventForwardable
	{
		/// <summary>
		/// The sender of the event.
		/// </summary>
		public object Sender { get; }

		/// <summary>
		/// Forwards the forwardable event to the specified <see cref="IEventBus"/> bus.
		/// </summary>
		/// <param name="bus">The event bus to forward to.</param>
		void ForwardTo(IEventBus bus);
	}
}
