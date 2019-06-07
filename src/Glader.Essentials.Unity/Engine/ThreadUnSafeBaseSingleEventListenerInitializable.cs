using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event listener type that will handle the complexities of thread-unsafe events.
	/// This can be used for events that are only suppose to run on Unity3D's mainthread.
	/// Getting this to run on the main thread is actually a complex process so this abstracts
	/// that complexity away from the consumers.
	/// </summary>
	/// <typeparam name="TSubscribableType"></typeparam>
	/// <typeparam name="TEventArgsType"></typeparam>
	public abstract class ThreadUnSafeBaseSingleEventListenerInitializable<TSubscribableType, TEventArgsType> : BaseSingleEventListenerInitializable<TSubscribableType, TEventArgsType>
		where TSubscribableType : class 
		where TEventArgsType : EventArgs
	{
		/// <inheritdoc />
		protected ThreadUnSafeBaseSingleEventListenerInitializable(TSubscribableType subscriptionService) 
			: base(subscriptionService)
		{

		}

		/// <inheritdoc />
		protected sealed override void OnEventFired(object source, TEventArgsType args)
		{
			try
			{
				UnityAsyncHelper.UnityMainThreadContext.Post(state =>
				{
					OnThreadUnSafeEventFired(source, args);
				}, null);
			}
			catch(NullReferenceException)
			{
				UnityEngine.Debug.LogError($"Null reference likely to {nameof(UnityAsyncHelper.UnityMainThreadContext)} in {nameof(UnityAsyncHelper)} which must be set.");
				throw;
			}
		}

		/// <summary>
		/// Called on the main thread of Unity3D.
		/// Requires <see cref="UnityAsyncHelper"/>'s UnityMainThreadContext to be initialized.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="args">The args for the event.</param>
		protected abstract void OnThreadUnSafeEventFired(object source, TEventArgsType args);
	}

	/// <summary>
	/// Event listener type that will handle the complexities of thread-unsafe events.
	/// This can be used for events that are only suppose to run on Unity3D's mainthread.
	/// Getting this to run on the main thread is actually a complex process so this abstracts
	/// that complexity away from the consumers.
	/// </summary>
	/// <typeparam name="TSubscribableType"></typeparam>
	public abstract class ThreadUnSafeBaseSingleEventListenerInitializable<TSubscribableType> : BaseSingleEventListenerInitializable<TSubscribableType> 
		where TSubscribableType : class
	{
		/// <inheritdoc />
		protected ThreadUnSafeBaseSingleEventListenerInitializable(TSubscribableType subscriptionService)
			: base(subscriptionService)
		{
		}

		/// <inheritdoc />
		protected override void OnEventFired(object source, EventArgs args)
		{
			try
			{
				UnityAsyncHelper.UnityMainThreadContext.Post(state =>
				{
					OnThreadUnSafeEventFired(source, args);
				}, null);
			}
			catch(NullReferenceException)
			{
				UnityEngine.Debug.LogError($"Null reference likely to {nameof(UnityAsyncHelper.UnityMainThreadContext)} in {nameof(UnityAsyncHelper)} which must be set.");
				throw;
			}
		}

		/// <summary>
		/// Called on the main thread of Unity3D.
		/// Requires <see cref="UnityAsyncHelper"/>'s UnityMainThreadContext to be initialized.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="args">The args for the event.</param>
		protected abstract void OnThreadUnSafeEventFired(object source, EventArgs args);
	}
}
