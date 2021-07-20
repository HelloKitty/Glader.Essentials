using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Glader.Essentials
{
	internal class EventBusLock<TEventType>
		where TEventType : IEventBusEventArgs
	{
		/// <summary>
		/// Eventbus lock object for the specified <typeparamref name="TEventType"/>.
		/// </summary>
		public static ReaderWriterLockSlim Lock { get; } = new ReaderWriterLockSlim();

		//WARNING: DO NOT REMOTE!
		static EventBusLock()
		{

		}
	}
}
