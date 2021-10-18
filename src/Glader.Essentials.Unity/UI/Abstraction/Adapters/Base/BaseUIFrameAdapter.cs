using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public class BaseUIFrameAdapter<TFrameType> : BaseUnityUI, IUIFrame, IUIAdapterRegisterable
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
		public virtual bool IsActive => gameObject.activeSelf;

		/// <inheritdoc />
		public IEnumerable<IUIElement> Elements => throw new NotImplementedException("TODO: Implement.");

		/// <inheritdoc />
		public virtual void SetElementActive(bool state)
		{
			gameObject.SetActive(state);
		}
	}
}
