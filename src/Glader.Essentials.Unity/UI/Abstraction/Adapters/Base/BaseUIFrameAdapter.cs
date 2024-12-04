using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public class BaseUIFrameAdapter<TFrameType> : BaseUnityUI, IUIFrame, IUIAdapterRegisterable, IDisposable
		where TFrameType : IUIFrame, IUIEventListenable
	{
		//Assigned by Unity3D.
		/// <summary>
		/// Internally Unity3D serialized key.
		/// </summary>
		[Tooltip("Used to determine wiring for UI dependencies.")]
		[SerializeField]
#pragma warning disable 649
		private string _RegistrationKey;
#pragma warning restore 649

		/// <inheritdoc />
		public string RegistrationKey => _RegistrationKey;

		/// <inheritdoc />
		public Type UIServiceType => typeof(TFrameType);

		public virtual bool AsKeyed { get; } = true;

		/// <inheritdoc />
		public IEventBus Bus { get; } = new EventBus();

		/// <inheritdoc />
		public virtual bool IsObjectActive => gameObject.activeSelf;

		// Frames don't have a single component.
		/// <inheritdoc />
		public virtual bool IsComponentActive => IsObjectActive;

		/// <inheritdoc />
		public IEnumerable<IUIElement> Elements => throw new NotImplementedException("TODO: Implement.");

		/// <inheritdoc />
		public virtual void SetObjectActive(bool state)
		{
			gameObject.SetActive(state);
		}

		/// <inheritdoc />
		public virtual void SetComponentActive(bool state)
		{
			// Frames don't have a single component.
			SetObjectActive(state);
		}

		/// <inheritdoc />
		public virtual void Dispose()
		{
			Bus?.Dispose();
		}

		/// <summary>
		/// Called by Unity on the component destruction.
		/// </summary>
		public virtual void OnDestroy()
		{
			Dispose();
		}
	}
}
