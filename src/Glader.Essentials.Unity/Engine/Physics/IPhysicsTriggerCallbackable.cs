using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public interface IPhysicsTriggerCallbackable
	{
		/// <summary>
		/// Unity3D engine callback when a <see cref="other"/> <see cref="Collider"/>
		/// enters the trigger.
		/// </summary>
		/// <param name="other">The other collider entering the trigger.</param>
		void OnTriggerEnter(Collider other);

		/// <summary>
		/// Unity3D engine callback when a <see cref="other"/> <see cref="Collider"/>
		/// exits the trigger.
		/// </summary>
		/// <param name="other">The other collider exiting the trigger.</param>
		void OnTriggerExit(Collider other);
	}
}
