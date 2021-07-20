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

		//TODO: This is a total hack for perf when consumer of the library never uses ForwardedSubscriptionMap
		private bool UsedForwardedEvents { get; set; } = false;

		/// <inheritdoc />
		public SubscriptionToken Subscribe<TEventType>(EventHandler<TEventType> action, EventBusSubscriptionMode mode = EventBusSubscriptionMode.Default) 
			where TEventType : IEventBusEventArgs
		{
			//Assume enum is valid for perf reasons
			switch (mode)
			{
				case EventBusSubscriptionMode.Default:
					return Subscribe(action, DefaultSubscriptionMap);
				case EventBusSubscriptionMode.Forwarded:
					UsedForwardedEvents = true;
					return Subscribe(action, ForwardedSubscriptionMap);
				default:
					throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private SubscriptionToken Subscribe<TEventType>(EventHandler<TEventType> action, IDictionary<Type, IEventBusSubscription[]> subscriptionMap) 
			where TEventType : IEventBusEventArgs
		{
			if (action == null) throw new ArgumentNullException(nameof(action));
			if (subscriptionMap == null) throw new ArgumentNullException(nameof(subscriptionMap));

			//TODO: Figure out how we might be able to do some array pooling.
			//Only one thread can be in upgradeable mode at any time.
			//However this will allow Read/Publish threads to publish until we absolutely MUST write to the array
			EventBusLock<TEventType>.Lock.EnterUpgradeableReadLock();
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
							EventBusLock<TEventType>.Lock.EnterWriteLock();
							try
							{
								array[i] = CreateNewSubscription(action);
								return array[i].Token; //if users somehow unsubs before we return the token (impossible currently) then we get exception if we don't return in the write block
							}
							finally
							{
								EventBusLock<TEventType>.Lock.ExitWriteLock();
							}
						}

					//If there is no null open slot then we need to reallocation just
					//like a List would
					var newArray = new IEventBusSubscription[array.Length + 1];
					array.AsSpan().CopyTo(newArray);

					//Simply the last element can now become the new subscription
					newArray[newArray.Length - 1] = CreateNewSubscription(action);

					//now we write lock in the secondary case where we need to replace the existing subscription array
					EventBusLock<TEventType>.Lock.EnterWriteLock();
					try
					{
						//Just replace the array, won't interrupt iterating Publishers
						//in a thread-unsafe way.
						subscriptionMap[typeof(TEventType)] = newArray;
						return newArray[newArray.Length - 1].Token; //if users somehow unsubs before we return the token (impossible currently) then we get exception if we don't return in the write block
					}
					finally
					{
						EventBusLock<TEventType>.Lock.ExitWriteLock();
					}
				}
				else
				{
					//We can allocate outside the write block for perf gain
					array = new IEventBusSubscription[] { CreateNewSubscription(action) };

					//Case where there wasn't even an initial entry
					//We don't have to write lock here but we do anyway just incase I'm
					//not as smart in modeling the concurrency and thread safety in my head as I think I am
					EventBusLock<TEventType>.Lock.EnterWriteLock();
					try
					{
						subscriptionMap[typeof(TEventType)] = array;
						return array[0].Token; //if users somehow unsubs before we return the token (impossible currently) then we get exception if we don't return in the write block
					}
					finally
					{
						EventBusLock<TEventType>.Lock.ExitWriteLock();
					}
				}
			}
			finally
			{
				EventBusLock<TEventType>.Lock.ExitUpgradeableReadLock();
			}
		}

		/// <inheritdoc />
		public bool Unsubscribe<TEventType>(SubscriptionToken token) 
			where TEventType : IEventBusEventArgs
		{
			//TODO: Optimized forwarded subscription unsubscribe
			//First try to remove from the default subscriptions, otherwise could be a forwarded subscription!
			if (!Unsubscribe<TEventType>(token, DefaultSubscriptionMap))
				return Unsubscribe<TEventType>(token, ForwardedSubscriptionMap);

			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool Unsubscribe<TEventType>(SubscriptionToken token, IDictionary<Type, IEventBusSubscription[]> subscriptionMap) 
			where TEventType : IEventBusEventArgs
		{
			if (token == null) throw new ArgumentNullException(nameof(token));

			//WARNING: We cannot remove read lock like we did with Publish here. See below warning above the loop.
			//Even though we may write (set an index null) 
			EventBusLock<TEventType>.Lock.EnterReadLock();
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
				EventBusLock<TEventType>.Lock.ExitReadLock();
			}

			//No token found
			return false;
		}

		private EventBusSubscription<TEventType> CreateNewSubscription<TEventType>(EventHandler<TEventType> action) where TEventType : IEventBusEventArgs
		{
			return new EventBusSubscription<TEventType>(action, this);
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

			foreach (IEventBusSubscription sub in array) //this will enumerate the returned reference, so it won't change
			{
				//The subscription could be null because we REUSE indexes and set them as null
				//to avoid allocations if possible on unsubscription or subscriptions
				sub?.Publish(sender, eventData);
			}
		}
	}
}
