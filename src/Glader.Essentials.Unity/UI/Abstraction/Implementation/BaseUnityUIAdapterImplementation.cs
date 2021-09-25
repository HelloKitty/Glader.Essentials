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
		[NotNull] 
		private Component AdaptedObject { get; }

		protected virtual string LoggableComponentName => AdaptedObject.gameObject.name;

		/// <inheritdoc />
		public IEventBus Bus { get; } = new EventBus();

		/// <inheritdoc />
		public bool IsActive => AdaptedObject.gameObject.activeSelf;

		/// <inheritdoc />
		public void SetElementActive(bool state)
		{
			AdaptedObject.gameObject.SetActive(state);
		}

		public BaseUnityUIAdapterImplementation([NotNull] UnityEngine.Component adaptedObject)
		{
			AdaptedObject = adaptedObject ?? throw new ArgumentNullException(nameof(adaptedObject));
		}
	}
}
