using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unitysync.Async;

namespace Glader.Essentials
{
	public abstract class BaseUnityUIAdapter<TAdaptedUnityEngineType, TAdaptedToType> : BaseUnityUI, IUIElement, IDisposable
		where TAdaptedToType : IUIElement
		where TAdaptedUnityEngineType : UnityEngine.Component
	{
		/// <inheritdoc />
		public IEventBus Bus => Element.Bus;

		private Lazy<BaseUnityUIAdapterImplementation> _Element;

		/// <summary>
		/// The implementer will expose access to the element implementation 
		/// </summary>
		protected virtual IUIElement Element => _Element.Value;

		/// <inheritdoc />
		public virtual bool IsObjectActive => Element.IsObjectActive;

		/// <inheritdoc />
		public bool IsComponentActive => Element.IsComponentActive;

		[SerializeField]
		private TAdaptedUnityEngineType _UnityUIObject;

		/// <summary>
		/// The Unity engine UI object being adapted.
		/// </summary>
		protected TAdaptedUnityEngineType UnityUIObject => _UnityUIObject;

		protected BaseUnityUIAdapter()
		{
			_Element = new Lazy<BaseUnityUIAdapterImplementation>(() => new BaseUnityUIAdapterImplementation(UnityUIObject));
		}

		static BaseUnityUIAdapter()
		{
			if(typeof(TAdaptedToType) == typeof(TAdaptedUnityEngineType))
				throw new InvalidOperationException($"Type: BaseUnityUIAdapter<{typeof(TAdaptedUnityEngineType).Name}, {typeof(TAdaptedToType).Name}> must not have the same parameter for both generic type parameters.");

			//TODO: Check that TAdaptedUnityEngineType is in the Unity namespace.
		}

		/// <summary>
		/// Validates that the provided <see cref="obj"/>
		/// is valid to be initialized as the adapted engine object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>True if it's valid.</returns>
		protected virtual bool ValidateInitializedObject(TAdaptedUnityEngineType obj)
		{
			return obj != null;
		}

		/// <summary>
		/// Attempts to locate the <typeparamref name="TAdaptedUnityEngineType"/> component
		/// and assign it.
		/// </summary>
		[Button]
		public void LocateComponent()
		{
			TAdaptedUnityEngineType obj = GetComponent<TAdaptedUnityEngineType>();

			if (ValidateInitializedObject(obj))
				_UnityUIObject = obj;
			else
				Debug.LogError($"Failed to find Component Type: {typeof(TAdaptedUnityEngineType).Name}");
		}

		/// <inheritdoc />
		public virtual void SetObjectActive(bool state)
		{
			Element.SetObjectActive(state);
		}

		/// <inheritdoc />
		public void SetComponentActive(bool state)
		{
			Element.SetComponentActive(state);
		}

		/// <summary>
		/// Disposes the resources (mostly event buses).
		/// </summary>
		public virtual void Dispose()
		{
			if (_Element.IsValueCreated)
				_Element.Value.Dispose();
		}

		/// <summary>
		/// Called by Unity3D when the component is destroyed.
		/// </summary>
		public virtual void OnDestroy()
		{
			Dispose();
		}
	}
}
