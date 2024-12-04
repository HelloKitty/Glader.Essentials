using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for types that implement the concept of an EventBus.
	/// Supports publishing, subscribing and unsubscribing to event types.
	/// </summary>
	public interface IEventBus : IEventBusPublishable, IEventBusSubscribable, IDisposable
	{
		
	}
}
