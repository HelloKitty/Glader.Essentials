using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Glader.Essentials
{
	/// <summary>
	/// Implements <see cref="IEventBus"/>.
	/// </summary>
	public class EventBus : IEventBus
	{
		//The reasoning for using an Array instead of a List is we don't ever mutate the event subscription array directly
		//This helps us avoid dirty/threadunsafe reads and avoids having to allocate everytime we publish an event.
		//Allocations matter alot in GameDev and Unity3D.
		//Conceptually it's Subscription is rare but Publish is common. Optimize for publish case!
		private IDictionary<Type, IEventBusSubscription[]> SubscriptionMap { get; } = new ConcurrentDictionary<Type, IEventBusSubscription[]>();

		/// <inheritdoc />
		public SubscriptionToken Subscribe<TEventType>(EventHandler<TEventType> action) 
			where TEventType : IEventBusEventArgs
		{
			if(action == null)
				throw new ArgumentNullException(nameof(action));

			//TODO: Figure out how we might be able to do some array pooling.
			//Only one thread can be in upgradeable mode at any time.
			//However this will allow Read/Publish threads to publish until we absolutely MUST write to the array
			EventBusLock<TEventType>.Lock.EnterUpgradeableReadLock();
			try
			{
				if (SubscriptionMap.ContainsKey(typeof(TEventType)))
				{
					var array = SubscriptionMap[typeof(TEventType)];

					//Let us try to find an opening in the existing array
					for(int i = 0; i < array.Length; i++)
						if (array[i].IsNull())
						{
							//At this point we know there is a null entry *and* we know that we must write
							//so we enter the write lock and don't need to double check locking because only write locks should be able
							//to set NULL and we're within an upgradeable lock at the moment.
							EventBusLock<TEventType>.Lock.EnterWriteLock();
							try
							{
								array[i] = CreateNewSubscription(action);
							}
							finally
							{
								EventBusLock<TEventType>.Lock.ExitWriteLock();
							}
							
							return array[i].Token;
						}

					//If there is no null open slot then we need to reallocation just
					//like a List would
					var newArray = new IEventBusSubscription[array.Length + 1];
					Array.Copy(array, newArray, array.Length);

					//Simply the last element can now become the new subscription
					newArray[newArray.Length - 1] = CreateNewSubscription(action);

					//now we write lock in the secondary case where we need to replace the existing subscription array
					EventBusLock<TEventType>.Lock.EnterWriteLock();
					try
					{
						//Just replace the array, won't interrupt iterating Publishers
						//in a thread-unsafe way.
						SubscriptionMap[typeof(TEventType)] = newArray;
					}
					finally
					{
						EventBusLock<TEventType>.Lock.ExitWriteLock();
					}

					return newArray[newArray.Length - 1].Token;
				}
				else
				{
					//Case where there wasn't even an initial entry
					//We don't have to write lock here but we do anyway just incase I'm
					//not as smart in modeling the concurrency and thread safety in my head as I think I am
					EventBusLock<TEventType>.Lock.EnterWriteLock();
					try
					{
						SubscriptionMap[typeof(TEventType)] = new IEventBusSubscription[]{ CreateNewSubscription(action) };
					}
					finally
					{
						EventBusLock<TEventType>.Lock.ExitWriteLock();
					}

					return SubscriptionMap[typeof(TEventType)].First().Token;
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
			if (token == null) throw new ArgumentNullException(nameof(token));

			//Even though we may write (set an index null) 
			EventBusLock<TEventType>.Lock.EnterReadLock();
			try
			{
				if (!SubscriptionMap.ContainsKey(typeof(TEventType)))
					return false;

				IEventBusSubscription[] array = SubscriptionMap[typeof(TEventType)];

				//WARNING: Don't try to read outside of this and set null in a write lock afterwards because Subscribe may create an Array copy and the null set won't carry over due to race conditions
				//Theortically we could read this array initially outside
				//of a read lock to see if the token exists before we lock and set it null
				//but that's basically 0 chance in our design, so we stay locked the entire time
				for(var i = 0; i < array.Length; i++)
					if(array[i]?.Token == token)
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
			IEventBusSubscription[] array;

			//We only read lock to retrieve the current array of subscriptions
			//We don't have to worry about the array changing because we check null on publishing
			//through the subscription
			EventBusLock<TEventType>.Lock.EnterReadLock();
			try
			{
				if (!SubscriptionMap.ContainsKey(typeof(TEventType)))
					return;

				array = SubscriptionMap[typeof(TEventType)];
			}
			finally
			{
				EventBusLock<TEventType>.Lock.ExitReadLock();
			}

			foreach(IEventBusSubscription sub in array) //this will enumerate the returned reference, so it won't change
			{
				//The subscription could be null because we REUSE indexes and set them as null
				//to avoid allocations if possible on unsubscription or subscriptions
				sub?.Publish(sender, eventData);
			}
		}
	}
}
