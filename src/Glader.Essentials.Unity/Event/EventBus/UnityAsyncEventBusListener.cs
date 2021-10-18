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
	public abstract class UnityAsyncEventBusListener<TEventArgsType, TSourceType> : EngineEventBusListener<TEventArgsType, TSourceType>
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

			if(Settings.ShouldLock)
				LockObj = new AsyncLock();
		}

		/// <inheritdoc />
		protected override bool OnBeforeEventFired(TSourceType sender, TEventArgsType args)
		{
			Interlocked.Increment(ref _ActiveEventCount);

			// For async purposes we actually schedule it to run in the sync context
			Task.Run(async () =>
			{
				if(Settings.RequiresMainThread && UnityAsyncHelper.UnityMainThreadContext != SynchronizationContext.Current)
					await new UnityYieldAwaitable();

				try
				{
					if(Settings.ShouldLock)
					{
						using(await LockObj.LockAsync())
							await HandleEventAsync(sender, args)
								.ConfigureAwait(Settings.RequiresMainThread);
					}
					else
						await HandleEventAsync(sender, args)
							.ConfigureAwait(Settings.RequiresMainThread);
				}
				finally
				{
					Interlocked.Decrement(ref _ActiveEventCount);
				}
			});

			return false;
		}

		/// <inheritdoc />
		protected sealed override void OnEventFired(TSourceType sender, TEventArgsType args)
		{
			throw new NotSupportedException($"Non-async event should not be called by async handler.");
		}

		/// <summary>
		/// Called when the event bus fires an event.
		/// </summary>
		/// <param name="sender">The sender of the event (may be null).</param>
		/// <param name="args">The event args.</param>
		protected internal abstract Task OnEventFiredAsync(TSourceType sender, TEventArgsType args);

		/// <summary>
		/// Called internally to service an event after <see cref="OnBeforeEventFired"/>.
		/// </summary>
		/// <param name="sender">The sender of the event (may be null).</param>
		/// <param name="args">The event args.</param>
		private async Task HandleEventAsync(TSourceType sender, TEventArgsType args)
		{
			bool successful = true;
			try
			{
				await OnEventFiredAsync(sender, args)
					.ConfigureAwait(Settings.RequiresMainThread);
			}
			catch(Exception e)
			{
				successful = false;
				OnException(sender, args, e);
			}
			finally
			{
				OnAfterEventFired(sender, args, successful);
			}
		}
	}

	/// <inheritdoc />
	public abstract class UnityAsyncEventBusListener<TEventArgsType> : UnityAsyncEventBusListener<TEventArgsType, object>
		where TEventArgsType : IEventBusEventArgs
	{
		/// <inheritdoc />
		protected UnityAsyncEventBusListener(IEventBus bus) 
			: base(bus)
		{
		}

		/// <inheritdoc />
		protected UnityAsyncEventBusListener(IEventBus bus, [NotNull] UnityAsyncEventBusHandlerSettings settings) 
			: base(bus, settings)
		{
		}
	}
}
