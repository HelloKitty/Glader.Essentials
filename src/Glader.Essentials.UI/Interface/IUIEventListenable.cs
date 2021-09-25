using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IUIEventBusContainable
	{
		/// <summary>
		/// The associated event bus for the listenable.
		/// </summary>
		IEventBus Bus { get; }
	}

	/// <summary>
	/// Contract for types that have UI events that can be listened to.
	/// </summary>
	public interface IUIEventListenable : IUIEventBusContainable
	{

	}

	/// <summary>
	/// <see cref="IUIEventListenable{TUIEventListenableType}"/> that contains the child-type
	/// as a type parameter.
	/// </summary>
	/// <typeparam name="TUIEventListenableType"></typeparam>
	public interface IUIEventListenable<TUIEventListenableType> : IUIEventListenable
		where TUIEventListenableType : IUIEventListenable<TUIEventListenableType>
	{

	}
}
