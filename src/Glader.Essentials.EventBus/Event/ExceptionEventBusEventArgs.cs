using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event type raised when an exception occurs during publishing.
	/// </summary>
	public sealed class ExceptionEventBusEventArgs : IEventBusEventArgs
	{
		/// <summary>
		/// The exception data.
		/// </summary>
		public Exception ExceptionData { get; }

		/// <summary>
		/// The subscription token of the subscription that generated the exception.
		/// </summary>
		public EventBusSubscriptionToken Token { get; }

		/// <summary>
		/// Event that caused the exception.
		/// </summary>
		public IEventBusEventArgs EventData { get; }

		/// <summary>
		/// The sender of the original event.
		/// </summary>
		public object Sender { get; }

		public ExceptionEventBusEventArgs(Exception exceptionData, EventBusSubscriptionToken token, object sender, IEventBusEventArgs eventData)
		{
			ExceptionData = exceptionData ?? throw new ArgumentNullException(nameof(exceptionData));
			Token = token ?? throw new ArgumentNullException(nameof(token));
			Sender = sender;
			EventData = eventData ?? throw new ArgumentNullException(nameof(eventData));
		}
	}
}
