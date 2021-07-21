using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Glader.Essentials
{
	internal class EventBusLock
	{
		/// <summary>
		/// Internal sync object to ensure multiple <see cref="ReaderWriterLockSlim"/> never exist for the same event type.
		/// </summary>
		private object SyncObj { get; } = new object();

		/// <summary>
		/// The lock event map.
		/// </summary>
		private ConcurrentDictionary<Type, ReaderWriterLockSlim> LockMap { get; } = new();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReaderWriterLockSlim GetLock<TEventType>()
			where TEventType : IEventBusEventArgs
		{
			return GetLock(typeof(TEventType));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReaderWriterLockSlim GetLock(Type eventType)
		{
			if (eventType == null) throw new ArgumentNullException(nameof(eventType));

			//We can try to load it directly without locking but we must lock if we
			//want to create a new one
			if(LockMap.TryGetValue(eventType, out var lockValue))
				return lockValue;

			lock(SyncObj)
			{
				//Double check lock
				if(LockMap.TryGetValue(eventType, out lockValue))
					return lockValue;

				return LockMap[eventType] = new ReaderWriterLockSlim();
			}
		}
	}
}
