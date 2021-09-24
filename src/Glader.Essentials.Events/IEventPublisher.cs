using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	// Created this simplified version to support event interface-less event publishing.
	/// <summary>
	/// Contract for a type that publishes a particular event interface type.
	/// </summary>
	/// <typeparam name="TEventArgsType">The event args type name.</typeparam>
	public interface IEventPublisher<in TEventArgsType>
	{
		/// <summary>
		/// Publishes the event with the args <see cref="eventArgs"/>
		/// with sender <see cref="sender"/>.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="eventArgs">The arguments for the event.</param>
		void PublishEvent(object sender, TEventArgsType eventArgs);
	}

	/// <summary>
	/// Contract for a type that publishes a particular event interface type.
	/// </summary>
	/// <typeparam name="TEventArgsType">The event args type name.</typeparam>
	/// <typeparam name="TEventPublisherInterface">The even publisher interface.</typeparam>
	public interface IEventPublisher<TEventPublisherInterface, in TEventArgsType> : IEventPublisher<TEventArgsType>
		where TEventArgsType : EventArgs
	{

	}
}
