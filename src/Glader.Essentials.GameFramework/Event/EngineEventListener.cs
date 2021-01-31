using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{

	/// <summary>
	/// Engine/framework implementation of <see cref="EventListener{TSubscribableType}"/> that implements <see cref="IGameInitializable"/>
	/// virtually. Allowing the event listeners to exist in the engine system.
	/// </summary>
	/// <typeparam name="TSubscribableType">The subscription interface.</typeparam>
	public abstract class EngineEventListener<TSubscribableType> : EventListener<TSubscribableType>, IGameInitializable
		where TSubscribableType : class
	{
		/// <inheritdoc />
		protected EngineEventListener(TSubscribableType subscriptionService)
			: base(subscriptionService)
		{

		}

		public virtual Task OnGameInitialized()
		{
			//Do nothing, this puts it into the Engine system.
			return Task.CompletedTask;
		}
	}

	/// <summary>
	/// Engine/framework implementation of <see cref="EventListener{TSubscribableType, TEventHandlerArgsType}"/> that implements <see cref="IGameInitializable"/>
	/// virtually. Allowing the event listeners to exist in the engine system.
	/// </summary>
	/// <typeparam name="TSubscribableType">The subscription interface.</typeparam>
	/// <typeparam name="TEventHandlerArgsType">The args type.</typeparam>
	public abstract class EngineEventListener<TSubscribableType, TEventHandlerArgsType> : EventListener<TSubscribableType, TEventHandlerArgsType>, IGameInitializable
		where TSubscribableType : class
		where TEventHandlerArgsType : EventArgs
	{
		/// <inheritdoc />
		protected EngineEventListener(TSubscribableType subscriptionService)
			: base(subscriptionService)
		{

		}

		public virtual Task OnGameInitialized()
		{
			//Do nothing, this puts it into the Engine system.
			return Task.CompletedTask;
		}
	}
}
