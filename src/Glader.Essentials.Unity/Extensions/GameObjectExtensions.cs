using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Glader
{
	public static class GameObjectExtensions
	{
		/// <summary>
		/// Gets or adds the component.
		/// Will try to get it but if it doesn't exist will call AddComponent.
		/// </summary>
		/// <typeparam name="TComponentType">The component type.</typeparam>
		/// <param name="go">The gameobject.</param>
		/// <returns>The component.</returns>
		public static TComponentType GetOrAddComponent<TComponentType>([NotNull] this GameObject go) 
			where TComponentType : Component
		{
			if (go == null) throw new ArgumentNullException(nameof(go));

			if (go.TryGetComponent<TComponentType>(out var comp))
				return comp;

			return go.AddComponent<TComponentType>();
		}
	}
}
