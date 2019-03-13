using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public interface IPhysicsTriggerEventSubscribable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="physicsType">The type of physics.</param>
		/// <param name="physicsCallback">The callback to register.</param>
		void RegisterTriggerEnterEventSubscription(PhysicsTriggerEventType physicsType, Action<object, PhysicsTriggerEventArgs> physicsCallback);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="physicsType">The type of physics.</param>
		/// <param name="physicsCallback">The callback to register.</param>
		void RegisterTriggerExitEventSubscription(PhysicsTriggerEventType physicsType, Action<object, PhysicsTriggerEventArgs> physicsCallback);

		//TODO: Add an unregister function.
	}

	public enum PhysicsTriggerEventType
	{
		Interest = 1,
	}

	public sealed class PhysicsTriggerEventArgs : EventArgs
	{
		/// <summary>
		/// The collider that ran the trigger event.
		/// </summary>
		public GameObject GameObjectTriggered { get; }
		
		/// <summary>
		/// The collider that triggered the event.
		/// (By going into the volume)
		/// </summary>
		public Collider ColliderThatTriggered { get; }

		/// <inheritdoc />
		public PhysicsTriggerEventArgs([JetBrains.Annotations.NotNull] GameObject gameObjectTriggered, [JetBrains.Annotations.NotNull] Collider colliderThatTriggered)
		{
			GameObjectTriggered = gameObjectTriggered ?? throw new ArgumentNullException(nameof(gameObjectTriggered));
			ColliderThatTriggered = colliderThatTriggered ?? throw new ArgumentNullException(nameof(colliderThatTriggered));
		}
	}
}
