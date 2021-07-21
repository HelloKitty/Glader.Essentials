using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Glader.Essentials
{
	internal class EventBusLock
	{
		private object SyncObj { get; } = new object();

		private ConcurrentDictionary<Type, ReaderWriterLockSlim> LockMap { get; } = new();

		public ReaderWriterLockSlim GetLock<TEventType>()
			where TEventType : IEventBusEventArgs
		{
			//We can try to load it directly without locking but we must lock if we
			//want to create a new one
			if (LockMap.TryGetValue(typeof(TEventType), out var lockValue))
				return lockValue;

			lock (SyncObj)
			{
				//Double check lock
				if (LockMap.TryGetValue(typeof(TEventType), out lockValue))
					return lockValue;

				return LockMap[typeof(TEventType)] = new ReaderWriterLockSlim();
			}
		}
	}
}
