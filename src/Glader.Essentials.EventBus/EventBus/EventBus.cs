using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Glader.Essentials
{
	/// <summary>
	/// Implements <see cref="IEventBus"/>.
	/// </summary>
	public sealed class EventBus : IEventBus
	{
		//The reasoning for using an Array instead of a List is we don't ever mutate the event subscription array directly
		//This helps us avoid dirty/threadunsafe reads and avoids having to allocate everytime we publish an event.
		//Allocations matter alot in GameDev and Unity3D.
		//Conceptually it's Subscription is rare but Publish is common. Optimize for publish case!
		/// <summary>
		/// Default event subscription map that maintains regular/default subscriptions.
		/// </summary>
		private IDictionary<Type, IEventBusSubscription[]> DefaultSubscriptionMap { get; } = new ConcurrentDictionary<Type, IEventBusSubscription[]>();

		//The reason auto-forwarded subscriptions are managed in a separate subscription map is because we want to only publish these after all default subscriptions have
		//occured. We don't want default subscriptions publishing to interrupted and become unpredictably ordered for forwarding
		/// <summary>
		/// Event subscription map that manages and maintains the forwarding/routing subscriptions that are forwarded to another <see cref="IEventBus"/>.
		/// </summary>
		private IDictionary<Type, IEventBusSubscription[]> ForwardedSubscriptionMap { get; } = new ConcurrentDictionary<Type, IEventBusSubscription[]>();

		/// <summary>
		/// Event subscription map that maintains the subscriptions that are subscribed to all events.
		/// </summary>
		private IDictionary<Type, IEventBusSubscription[]> AllSubscriptionMap { get; } = new ConcurrentDictionary<Type, IEventBusSubscription[]>();

		/// <summary>
		/// Event subscription map that maintains subscriptions for exception events within the <see cref="IEventBus"/>.
		/// </summary>
		private IDictionary<Type, IEventBusSubscription[]> ExceptionSubscriptionMap { get; } = new ConcurrentDictionary<Type, IEventBusSubscription[]>();

		private EventBusLock BusLock { get; } = new();

		//TODO: This is a total hack for perf when consumer of the library never uses ForwardedSubscriptionMap
		private bool UsedForwardedEvents { get; set; } = false;

		private bool UsedAllEvents { get; set; } = false;

		private IEventSubscriptionIterationStrategy PublishIterationStrategy { get; }

		private IEventSubscriptionPublishStrategy PublishStrategy { get; }

		private readonly bool IsPublishIterationStrategyDefault;

		/// <summary>
		/// Event fired by all <see cref="EventBus"/>'s if no <see cref="ExceptionEventBusEventArgs"/> listener
		/// is registered. This is a global static shared event that can be listened to.
		/// Suggested to listen to this for logging uncaught/unlistened to exceptions.
		/// </summary>
		public static event EventHandler<ExceptionEventBusEventArgs> SharedUncaughtExceptionEvent;

		/// <summary>
		/// Creates <see cref="EventBus"/> with the default configuration.
		/// </summary>
		public EventBus()
			: this(DefaultEventSubscriptionIterationStrategy.Instance, DefaultEventSubscriptionPublishStrategy.Instance)
		{

		}

		/// <summary>
		/// Creates <see cref="EventBus"/> with a custom <see cref="IEventSubscriptionIterationStrategy"/> and custom configuration.
		/// </summary>
		/// <param name="publishIterationStrategy">The publish strategy.</param>
		/// <param name="publishStrategy">The publish strategy.</param>
		public EventBus(IEventSubscriptionIterationStrategy publishIterationStrategy,
			IEventSubscriptionPublishStrategy publishStrategy)
		{
			PublishIterationStrategy = publishIterationStrategy ?? throw new ArgumentNullException(nameof(publishIterationStrategy));
			PublishStrategy = publishStrategy ?? throw new ArgumentNullException(nameof(publishStrategy));

			IsPublishIterationStrategyDefault = PublishIterationStrategy is DefaultEventSubscriptionIterationStrategy;
		}

		/// <inheritdoc />
		public EventBusSubscriptionToken Subscribe<TEventType>(EventHandler<TEventType> action, EventBusSubscriptionMode mode = EventBusSubscriptionMode.Default) 
			where TEventType : IEventBusEventArgs
		{
			//Assume enum is valid for perf reasons
			switch (mode)
			{
				case EventBusSubscriptionMode.Default:
					return Subscribe(action, DefaultSubscriptionMap, mode);
				case EventBusSubscriptionMode.Forwarded:
					UsedForwardedEvents = true;
					return Subscribe(action, ForwardedSubscriptionMap, mode);
				case EventBusSubscriptionMode.All:
					UsedAllEvents = true;
					if (typeof(TEventType) != typeof(IEventBusEventArgs))
						throw new InvalidOperationException($"Cannot subscribe with Mode: {mode} Type: {typeof(TEventType).Name} for Action: {action}.");

					return Subscribe<IEventBusEventArgs>((s, e) => action(s, (TEventType)e), AllSubscriptionMap, mode);
				case EventBusSubscriptionMode.Exception:
					if(typeof(TEventType) != typeof(ExceptionEventBusEventArgs))
						throw new InvalidOperationException($"Cannot subscribe with Mode: {mode} Type: {typeof(TEventType).Name} for Action: {action}.");

					return Subscribe<ExceptionEventBusEventArgs>((s, e) => action(s, (TEventType)(IEventBusEventArgs)e), ExceptionSubscriptionMap, mode);
				default:
					throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private EventBusSubscriptionToken Subscribe<TEventType>(EventHandler<TEventType> action, IDictionary<Type, IEventBusSubscription[]> subscriptionMap, EventBusSubscriptionMode mode) 
			where TEventType : IEventBusEventArgs
		{
			if (action == null) throw new ArgumentNullException(nameof(action));
			if (subscriptionMap == null) throw new ArgumentNullException(nameof(subscriptionMap));

			var newSubscription = CreateNewSubscription(action, mode);

			//TODO: Figure out how we might be able to do some array pooling.
			//Only one thread can be in upgradeable mode at any time.
			//However this will allow Read/Publish threads to publish until we absolutely MUST write to the array
			BusLock.GetLock<TEventType>().EnterUpgradeableReadLock();
			try
			{
				//Using TryGetValue instead of ContainsKey because ConcurrentDictionary won't lock as seen here: https://github.com/microsoft/referencesource/blob/master/mscorlib/system/collections/Concurrent/ConcurrentDictionary.cs#L498
				//And TryGetValue can get us the value directly without having to x2 call TryGetValue since it's called internally by indexer
				if(subscriptionMap.TryGetValue(typeof(TEventType), out var array))
				{
					//Let us try to find an opening in the existing array
					for (int i = 0; i < array.Length; i++)
						if (array[i] == null)
						{
							//At this point we know there is a null entry *and* we know that we must write
							//so we enter the write lock and don't need to double check locking because only write locks should be able
							//to set NULL and we're within an upgradeable lock at the moment.
							BusLock.GetLock<TEventType>().EnterWriteLock();
							try
							{
								//Because we only readlock for unsub we need to actually double-check lock here
								if (array[i] != null)
									continue;

								array[i] = newSubscription;
								return array[i].Token; //if users somehow unsubs before we return the token (impossible currently) then we get exception if we don't return in the write block
							}
							finally
							{
								BusLock.GetLock<TEventType>().ExitWriteLock();
							}
						}

					//If there is no null open slot then we need to reallocation just
					//like a List would
					var newArray = new IEventBusSubscription[array.Length + 1];
					//Simply the last element can now become the new subscription
					newArray[newArray.Length - 1] = newSubscription;

					//now we write lock in the secondary case where we need to replace the existing subscription array
					BusLock.GetLock<TEventType>().EnterWriteLock();
					try
					{
						//Because in Unsub we only do a read lock to set null we cannot safely copy
						//until we're in the write block sadly
						array.AsSpan().CopyTo(newArray);

						//Just replace the array, won't interrupt iterating Publishers
						//in a thread-unsafe way.
						subscriptionMap[typeof(TEventType)] = newArray;
						return newSubscription.Token; //if users somehow unsubs before we return the token (impossible currently) then we get exception if we don't return in the write block
					}
					finally
					{
						BusLock.GetLock<TEventType>().ExitWriteLock();
					}
				}
				else
				{
					//We can allocate outside the write block for perf gain
					array = new IEventBusSubscription[] { newSubscription };

					//Case where there wasn't even an initial entry
					//We don't have to write lock here but we do anyway just incase I'm
					//not as smart in modeling the concurrency and thread safety in my head as I think I am
					BusLock.GetLock<TEventType>().EnterWriteLock();
					try
					{
						subscriptionMap[typeof(TEventType)] = array;
						return array[0].Token; //if users somehow unsubs before we return the token (impossible currently) then we get exception if we don't return in the write block
					}
					finally
					{
						BusLock.GetLock<TEventType>().ExitWriteLock();
					}
				}
			}
			finally
			{
				BusLock.GetLock<TEventType>().ExitUpgradeableReadLock();
			}
		}

		/// <inheritdoc />
		public bool Unsubscribe<TEventType>(EventBusSubscriptionToken token) 
			where TEventType : IEventBusEventArgs
		{
			//Case where users might try to remove subscription multiple times from the save token.
			if(token.Disposed)
				return false;

			switch (token.Mode)
			{
				case EventBusSubscriptionMode.Default:
					return Unsubscribe<TEventType>(token, DefaultSubscriptionMap);
				case EventBusSubscriptionMode.Forwarded:
					return Unsubscribe<TEventType>(token, ForwardedSubscriptionMap);
				case EventBusSubscriptionMode.All:
					return Unsubscribe<IEventBusEventArgs>(token, AllSubscriptionMap);
				case EventBusSubscriptionMode.Exception:
					return Unsubscribe<ExceptionEventBusEventArgs>(token, ExceptionSubscriptionMap);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <inheritdoc />
		public void UnsubscribeAll()
		{
			// TODO: Warning, things may still subscribe after this or even during this.
			// So that could be a problem I suppose.
			UnSubscribeAll(DefaultSubscriptionMap);
			UnSubscribeAll(ForwardedSubscriptionMap);
			UnSubscribeAll(AllSubscriptionMap);
			UnSubscribeAll(ExceptionSubscriptionMap);
		}

		private void UnSubscribeAll(IDictionary<Type, IEventBusSubscription[]> container)
		{
			foreach (var subs in container.Values)
			{
				// Apparently these can be null, I forgot? Or something
				if (subs == null)
					continue;

				// Apparently these can be null, I forgot? Or something
				foreach (var sub in subs)
					if (sub != null)
						sub.Token.Dispose();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool Unsubscribe<TEventType>(EventBusSubscriptionToken token, IDictionary<Type, IEventBusSubscription[]> subscriptionMap) 
			where TEventType : IEventBusEventArgs
		{
			if (token == null) throw new ArgumentNullException(nameof(token));

			//Case where users might try to remove subscription multiple times from the save token.
			if (token.Disposed)
				return false;

			//WARNING: We cannot remove read lock like we did with Publish here. See below warning above the loop.
			//Even though we may write (set an index null) 
			BusLock.GetLock<TEventType>().EnterReadLock();
			try
			{
				//Using TryGetValue instead of ContainsKey because ConcurrentDictionary won't lock as seen here: https://github.com/microsoft/referencesource/blob/master/mscorlib/system/collections/Concurrent/ConcurrentDictionary.cs#L498
				//And TryGetValue can get us the value directly without having to x2 call TryGetValue since it's called internally by indexer
				if(!subscriptionMap.TryGetValue(typeof(TEventType), out var array))
					return false;

				//WARNING: Don't try to read outside of this and set null in a write lock afterwards because Subscribe may create an Array copy and the null set won't carry over due to race conditions
				//Theortically we could read this array initially outside
				//of a read lock to see if the token exists before we lock and set it null
				//but that's basically 0 chance in our design, so we stay locked the entire time
				for (var i = 0; i < array.Length; i++)
					if (array[i]?.Token == token)
					{
						array[i] = null;
						return true;
					}
			}
			finally
			{
				BusLock.GetLock<TEventType>().ExitReadLock();
			}

			//No token found
			return false;
		}

		private EventBusSubscription<TEventType> CreateNewSubscription<TEventType>(EventHandler<TEventType> action, EventBusSubscriptionMode mode) where TEventType : IEventBusEventArgs
		{
			return new EventBusSubscription<TEventType>(action, this, mode);
		}

		/// <inheritdoc />
		public void Publish<TEventType>(object sender, TEventType eventData) 
			where TEventType : IEventBusEventArgs
		{
			//Don't bother checking, just assume default exists some
			Publish(sender, eventData, DefaultSubscriptionMap);

			//TODO: This is not optimal but better than Count because it's lockless.
			//TODO: We can only do this because we ASSUME it's ConcurrentDictionary. May not be in the future!!
			if (UsedForwardedEvents)
				Publish(sender, eventData, ForwardedSubscriptionMap);

			if (UsedAllEvents)
				Publish<IEventBusEventArgs>(sender, eventData, AllSubscriptionMap);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Publish<TEventType>(object sender, TEventType eventData, IDictionary<Type, IEventBusSubscription[]> subscriptionMap) 
			where TEventType : IEventBusEventArgs
		{
			//We can avoid read lots by just reading the current value directly, if there is any, this is actually threadsafe still!
			//Already checked that it contains the key but this could have changed since it wasn't locked
			//in the calling function. Checking it avoided doing a lock or waiting for a contended lock potentially.
			//Using TryGetValue instead of ContainsKey because ConcurrentDictionary won't lock as seen here: https://github.com/microsoft/referencesource/blob/master/mscorlib/system/collections/Concurrent/ConcurrentDictionary.cs#L498
			//And TryGetValue can get us the value directly without having to x2 call TryGetValue since it's called internally by indexer
			if(!subscriptionMap.TryGetValue(typeof(TEventType), out var array))
				return;

			//3x speed up from manually using the default
			if (IsPublishIterationStrategyDefault)
			{
				//this will enumerate the returned reference, so it won't change
				foreach(IEventBusSubscription sub in array)
					PublishEventSubscription(sender, eventData, sub);
			}
			else
			{
				foreach(IEventBusSubscription sub in PublishIterationStrategy.Enumerate(array))
					PublishEventSubscription(sender, eventData, sub);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void PublishEventSubscription<TEventType>(object sender, TEventType eventData, IEventBusSubscription sub) where TEventType : IEventBusEventArgs
		{
			try
			{
				//The subscription could be null because we REUSE indexes and set them as null
				//to avoid allocations if possible on unsubscription or subscriptions
				PublishStrategy.Publish(sub, sender, eventData);
			}
			catch (Exception e)
			{
				//Don't publish exceptions caused by subscribers of actual exception event types
				if (typeof(TEventType) == typeof(ExceptionEventBusEventArgs))
					throw;

				//Only if we have any exceptions listeners registered should we bother publishing
				//otherwise we should fall back to the default exception listener.
				if (ExceptionSubscriptionMap.Any())
					Publish<ExceptionEventBusEventArgs>(sender, new ExceptionEventBusEventArgs(e, sub?.Token, sender, eventData), ExceptionSubscriptionMap);
				else
					SharedUncaughtExceptionEvent?.Invoke(sender, new ExceptionEventBusEventArgs(e, sub?.Token, sender, eventData));
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			// TODO: Still a leak if something subs to it after dispose. We have no good way to lock to check this though.
			// Dispose just means unsub all.
			UnsubscribeAll();
		}
	}
}
