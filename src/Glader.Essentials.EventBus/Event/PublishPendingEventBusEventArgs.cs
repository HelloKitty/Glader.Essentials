using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event that can be sent to an <see cref="IEventBus"/> to force it to publish event args.
	/// </summary>
	public record PublishPendingEventBusEventArgs : IEventBusEventArgs;
}
