using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Engine/framework implementation of <see cref="EventBusListener{TEventArgsType}"/> that implements <see cref="IGameInitializable"/>
	/// virtually. Allowing the event listeners to exist in the engine system.
	/// </summary>
	/// <typeparam name="TEventArgsType">The args type.</typeparam>
	public abstract class EngineEventBusListener<TEventArgsType> : EventBusListener<TEventArgsType>, IGameInitializable
		where TEventArgsType : IEventBusEventArgs
	{
		/// <summary>
		/// Creates a new engine event bus listener that listens for events
		/// on the specified bus.
		/// </summary>
		/// <param name="bus">The event bus to listen to.</param>
		protected EngineEventBusListener(IEventBus bus)
			: base(bus)
		{

		}

		/// <inheritdoc />
		public virtual Task OnGameInitialized()
		{
			//Do nothing, this puts it into the Engine system.
			return Task.CompletedTask;
		}
	}
}
