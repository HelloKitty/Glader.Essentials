using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public sealed class GlobalPhysicsEventSystem : IPhysicsTriggerEventSubscribable, IPhysicsTriggerEventDispatcher
	{
		private object SyncObj = new object();

		private Dictionary<PhysicsTriggerEventType, Action<object, PhysicsTriggerEventArgs>> PhysicsEnterCallbackMap { get; }

		private Dictionary<PhysicsTriggerEventType, Action<object, PhysicsTriggerEventArgs>> PhysicsExitCallbackMap { get; }

		/// <summary>
		/// The global physics callback system.
		/// </summary>
		public static GlobalPhysicsEventSystem Instance { get; } = new GlobalPhysicsEventSystem();

		public GlobalPhysicsEventSystem()
		{
			PhysicsEnterCallbackMap = new Dictionary<PhysicsTriggerEventType, Action<object, PhysicsTriggerEventArgs>>();
			PhysicsExitCallbackMap = new Dictionary<PhysicsTriggerEventType, Action<object, PhysicsTriggerEventArgs>>();
		}

		/// <inheritdoc />
		public void RegisterTriggerEnterEventSubscription(PhysicsTriggerEventType physicsType, [JetBrains.Annotations.NotNull] Action<object, PhysicsTriggerEventArgs> physicsCallback)
		{
			if(physicsCallback == null) throw new ArgumentNullException(nameof(physicsCallback));
			if(!Enum.IsDefined(typeof(PhysicsTriggerEventType), physicsType)) throw new InvalidEnumArgumentException(nameof(physicsType), (int)physicsType, typeof(PhysicsTriggerEventType));

			lock(SyncObj)
			{
				if(PhysicsEnterCallbackMap.ContainsKey(physicsType))
				{
					PhysicsEnterCallbackMap[physicsType] += physicsCallback;
				}
				else
					PhysicsEnterCallbackMap.Add(physicsType, physicsCallback);
			}
		}

		/// <inheritdoc />
		public void RegisterTriggerExitEventSubscription(PhysicsTriggerEventType physicsType, [JetBrains.Annotations.NotNull] Action<object, PhysicsTriggerEventArgs> physicsCallback)
		{
			if(physicsCallback == null) throw new ArgumentNullException(nameof(physicsCallback));
			if(!Enum.IsDefined(typeof(PhysicsTriggerEventType), physicsType)) throw new InvalidEnumArgumentException(nameof(physicsType), (int)physicsType, typeof(PhysicsTriggerEventType));

			lock(SyncObj)
			{
				if(PhysicsExitCallbackMap.ContainsKey(physicsType))
				{
					PhysicsExitCallbackMap[physicsType] += physicsCallback;
				}
				else
					PhysicsExitCallbackMap.Add(physicsType, physicsCallback);
			}
		}

		/// <inheritdoc />
		public void DispatchTriggerEnter(PhysicsTriggerEventType physicsType, [JetBrains.Annotations.NotNull] GameObject objectTrigerRanOn, [JetBrains.Annotations.NotNull] Collider colliderThatTriggered)
		{
			if(objectTrigerRanOn == null) throw new ArgumentNullException(nameof(objectTrigerRanOn));
			if(colliderThatTriggered == null) throw new ArgumentNullException(nameof(colliderThatTriggered));

			if(PhysicsEnterCallbackMap.ContainsKey(physicsType))
			{
				Action<object, PhysicsTriggerEventArgs> callback = null;
				lock(SyncObj)
				{
					if(PhysicsEnterCallbackMap.ContainsKey(physicsType))
						callback = PhysicsEnterCallbackMap[physicsType];
				}

				callback?.Invoke(this, new PhysicsTriggerEventArgs(objectTrigerRanOn, colliderThatTriggered));
			}
		}

		/// <inheritdoc />
		public void DispatchTriggerExit(PhysicsTriggerEventType physicsType, [JetBrains.Annotations.NotNull] GameObject objectTrigerRanOn, [JetBrains.Annotations.NotNull] Collider colliderThatTriggered)
		{
			if(objectTrigerRanOn == null) throw new ArgumentNullException(nameof(objectTrigerRanOn));
			if(colliderThatTriggered == null) throw new ArgumentNullException(nameof(colliderThatTriggered));

			if(PhysicsExitCallbackMap.ContainsKey(physicsType))
			{
				Action<object, PhysicsTriggerEventArgs> callback = null;
				lock(SyncObj)
				{
					if(PhysicsExitCallbackMap.ContainsKey(physicsType))
						callback = PhysicsExitCallbackMap[physicsType];
				}

				callback?.Invoke(this, new PhysicsTriggerEventArgs(objectTrigerRanOn, colliderThatTriggered));
			}
		}
	}
}
