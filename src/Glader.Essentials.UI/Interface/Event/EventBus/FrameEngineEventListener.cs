using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Implementation of <see cref="EngineEventBusListener{TEventArgsType, TSourceType}"/> that uses
	/// the provided <see cref="IUIFrame"/>'s <see cref="IEventBus"/> to listen to the events.
	/// </summary>
	/// <typeparam name="TEventArgsType">The args type.</typeparam>
	/// <typeparam name="TSourceType">The Frame source type.</typeparam>
	public abstract class FrameEngineEventListener<TEventArgsType, TSourceType> : EngineEventBusListener<TEventArgsType, TSourceType>
		where TEventArgsType : IEventBusEventArgs
		where TSourceType : IUIFrame
	{
		/// <summary>
		/// The frame the <see cref="EngineEventListener{TSubscribableType}"/> is attached to.
		/// </summary>
		protected TSourceType Frame { get; }

		/// <inheritdoc />
		protected FrameEngineEventListener(TSourceType frame) 
			: base(frame.Bus)
		{
			Frame = frame ?? throw new ArgumentNullException(nameof(frame));
		}
	}
}
