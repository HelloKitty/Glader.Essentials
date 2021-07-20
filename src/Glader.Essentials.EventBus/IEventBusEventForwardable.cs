using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
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
