using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Base type for an event listener that listens to a single event.
	/// Will register a callback <see cref="OnEventFired"/> to the event on <see cref="TSubscribableType"/>
	/// that has an event signature with either EventArgs or <see cref="EventHandler"/>.
	/// </summary>
	/// <typeparam name="TSubscribableType">The subscription interface.</typeparam>
	public abstract class EventListener<TSubscribableType> : SharedBaseEventListener<TSubscribableType, EventHandler, EventArgs>
		where TSubscribableType : class
	{
		/// <inheritdoc />
		protected EventListener(TSubscribableType subscriptionService)
			: base(subscriptionService)
		{
			//This implementation doesn't depend on IGameInitializable and subscribes in the ctor.
			Subscribe();
		}

		/// <inheritdoc />
		protected internal override void HandleOnEventFiredCast(Action<TSubscribableType, EventHandler> targetSubscriptionMethod)
		{
			targetSubscriptionMethod.Invoke(SubscriptionService, OnEventFired);
		}
	}

	/// <summary>
	/// Base type for an event listener that listens to a single event.
	/// Will register a callback <see cref="OnEventFired"/> to the event on <see cref="TSubscribableType"/>
	/// that has an event signature with args <see cref="TEventHandlerArgsType"/>.
	/// </summary>
	/// <typeparam name="TSubscribableType">The subscription interface.</typeparam>
	/// <typeparam name="TEventHandlerArgsType">The type of args the event publishes.</typeparam>
	public abstract class EventListener<TSubscribableType, TEventHandlerArgsType> : SharedBaseEventListener<TSubscribableType, EventHandler<TEventHandlerArgsType>, TEventHandlerArgsType>
		where TSubscribableType : class
		where TEventHandlerArgsType : EventArgs
	{
		/// <inheritdoc />
		protected EventListener(TSubscribableType subscriptionService)
			: base(subscriptionService)
		{
			//This implementation doesn't depend on IGameInitializable and subscribes in the ctor.
			Subscribe();
		}

		/// <inheritdoc />
		protected internal override void HandleOnEventFiredCast(Action<TSubscribableType, EventHandler<TEventHandlerArgsType>> targetSubscriptionMethod)
		{
			targetSubscriptionMethod.Invoke(SubscriptionService, OnEventFired);
		}
	}
}