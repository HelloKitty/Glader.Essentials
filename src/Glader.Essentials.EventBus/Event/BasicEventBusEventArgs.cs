using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Generic <see cref="IEventBusEventArgs"/> that can adapt a non-<see cref="IEventBusEventArgs"/> object as the event.
	/// </summary>
	public class BasicEventBusEventArgs<TPayload> : IEventBusEventArgs
	{
		/// <summary>
		/// The adapted payload for the event.
		/// </summary>
		public TPayload Payload { get; }

		public BasicEventBusEventArgs(TPayload payload)
		{
			Payload = payload ?? throw new ArgumentNullException(nameof(payload));
		}
	}
}
