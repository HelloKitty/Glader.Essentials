using System;
using UnityEngine;

namespace Glader.Essentials
{
	public abstract class BaseRegisterableUnityUI<TRegisterType> : BaseUnityUI, IUIAdapterRegisterable
	{
		//Assigned by Unity3D.
		/// <summary>
		/// Internally Unity3D serialized key.
		/// </summary>
		[Tooltip("Used to determine wiring for UI dependencies.")] [SerializeField]
#pragma warning disable 649
		private string _RegistrationKey;
#pragma warning restore 649

		/// <inheritdoc />
		public string RegistrationKey => _RegistrationKey;

		/// <inheritdoc />
		public Type UIServiceType => typeof(TRegisterType);

		/// <inheritdoc />
		public virtual bool AsKeyed { get; } = true;
	}
}