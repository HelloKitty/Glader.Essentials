using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nito.AsyncEx;

namespace Glader.Essentials
{
	public record UnityAsyncEventBusHandlerSettings(bool RequiresMainThread, bool ShouldLock)
	{
		/// <summary>
		/// Default settings.
		/// </summary>
		public static UnityAsyncEventBusHandlerSettings Default = new UnityAsyncEventBusHandlerSettings(false, false);
	}

	/// <inheritdoc />
	public abstract class UnityAsyncEventBusListener<TEventArgsType> : EventBusListener<TEventArgsType>
		where TEventArgsType : IEventBusEventArgs
	{
		private UnityAsyncEventBusHandlerSettings Settings { get; }

		/// <summary>
		/// Optional lock object. (If Settings indicate locking isn't required this can be null.)
		/// </summary>
		[CanBeNull]
		private AsyncLock LockObj { get; }

		private int _ActiveEventCount = 0;

		/// <summary>
		/// Indicates if other events are actively being handled.
		/// </summary>
		protected bool MultipleEventsActive => _ActiveEventCount - 1 > 0;

		/// <inheritdoc />
		protected UnityAsyncEventBusListener(IEventBus bus) 
			: this(bus, UnityAsyncEventBusHandlerSettings.Default)
		{

		}

		/// <inheritdoc />
		protected UnityAsyncEventBusListener(IEventBus bus, [NotNull] UnityAsyncEventBusHandlerSettings settings)
			: base(bus)
		{
			Settings = settings ?? throw new ArgumentNullException(nameof(settings));

			if (Settings.ShouldLock)
				LockObj = new AsyncLock();
		}

		/// <inheritdoc />
		protected override bool OnBeforeEventFired(object sender, TEventArgsType args)
		{
			Interlocked.Increment(ref _ActiveEventCount);

			// For async purposes we actually schedule it to run in the sync context
			Task.Run(async () =>
			{
				if (Settings.RequiresMainThread && UnityAsyncHelper.UnityMainThreadContext != SynchronizationContext.Current)
					await new UnityYieldAwaitable();

				try
				{
					if (Settings.ShouldLock)
					{
						using(await LockObj.LockAsync())
							HandleEvent(sender, args);
					}
					else
						HandleEvent(sender, args);
				}
				finally
				{
					Interlocked.Decrement(ref _ActiveEventCount);
				}
			});

			return false;
		}
	}
}
