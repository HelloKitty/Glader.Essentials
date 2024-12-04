using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Unitysync.Async;

namespace Glader.Essentials
{
	public class BaseUnityUIAdapterImplementation : IUIElement
	{
		// TODO: Hope changing this didn't break stuff
		[NotNull] 
		private Component AdaptedObject { get; }

		protected virtual string LoggableComponentName => AdaptedObject.gameObject.name;

		/// <inheritdoc />
		public IEventBus Bus { get; } = new EventBus();

		/// <inheritdoc />
		public bool IsObjectActive => AdaptedObject.gameObject.activeSelf;

		/// <inheritdoc />
		public bool IsComponentActive => GetIsComponentActive();

		private bool GetIsComponentActive()
		{
			if (AdaptedObject is Behaviour behavior)
				return behavior.enabled;
			else
				return IsObjectActive;
		}

		/// <inheritdoc />
		public void SetComponentActive(bool state)
		{
			if (AdaptedObject is Behaviour behavior)
				behavior.enabled = state;
			else
				SetObjectActive(state);
		}

		/// <inheritdoc />
		public void SetObjectActive(bool state)
		{
			AdaptedObject.gameObject.SetActive(state);
		}

		public BaseUnityUIAdapterImplementation([NotNull] UnityEngine.Component adaptedObject)
		{
			AdaptedObject = adaptedObject ?? throw new ArgumentNullException(nameof(adaptedObject));
		}

		/// <inheritdoc />
		public virtual void Dispose()
		{
			Bus?.Dispose();
		}
	}
}
