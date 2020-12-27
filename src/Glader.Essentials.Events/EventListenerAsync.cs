using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Glader.Essentials;

namespace Glader
{
	/// <summary>
	/// Async implementation of the <see cref="EventListener{TSubscribableType}"/> which runs
	/// on the thread pool.
	/// </summary>
	/// <typeparam name="TSubscribableType">The subscribable type.</typeparam>
	public abstract class EventListenerAsync<TSubscribableType> : EventListener<TSubscribableType> 
		where TSubscribableType : class
	{
		protected EventListenerAsync(TSubscribableType subscriptionService) 
			: base(subscriptionService)
		{

		}

		protected sealed override void OnEventFired(object source, EventArgs args)
		{
			//Dispatches the event to the threadpool and then handles it async.
			Task.Run(async () => await OnEventFiredAsync(source, args));
		}

		/// <summary>
		/// Called when the subscription service fires an event.
		/// Runs on the threadpool async.
		/// </summary>
		/// <param name="source">The calling source.</param>
		/// <param name="args">The event args</param>
		protected abstract Task OnEventFiredAsync(object source, EventArgs args);
	}

	/// <summary>
	/// Async implementation of the <see cref="EventListener{TSubscribableType}"/> which runs
	/// on the thread pool.
	/// </summary>
	/// <typeparam name="TSubscribableType">The subscribable type.</typeparam>
	/// <typeparam name="TEventHandlerArgsType">The args type.</typeparam>
	public abstract class EventListenerAsync<TSubscribableType, TEventHandlerArgsType> : EventListener<TSubscribableType, TEventHandlerArgsType>
		where TSubscribableType : class 
		where TEventHandlerArgsType : EventArgs
	{
		protected EventListenerAsync(TSubscribableType subscriptionService)
			: base(subscriptionService)
		{

		}

		protected sealed override void OnEventFired(object source, TEventHandlerArgsType args)
		{
			//Dispatches the event to the threadpool and then handles it async.
			Task.Run(async () => await OnEventFiredAsync(source, args));
		}

		/// <summary>
		/// Called when the subscription service fires an event.
		/// Runs on the threadpool async.
		/// </summary>
		/// <param name="source">The calling source.</param>
		/// <param name="args">The event args</param>
		protected abstract Task OnEventFiredAsync(object source, TEventHandlerArgsType args);
	}
}
