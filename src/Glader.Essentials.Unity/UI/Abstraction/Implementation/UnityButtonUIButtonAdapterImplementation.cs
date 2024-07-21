using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Glader.Essentials
{
	/// <summary>
	/// The implementation of adaptation between <see cref="Button"/> and <see cref="IUIButton"/>.
	/// </summary>
	public sealed class UnityButtonUIButtonAdapterImplementation 
		: BaseUnityUIAdapterImplementation, IUIButton, IPointerDownHandler, IPointerUpHandler, ISubmitHandler
	{
		private UnityEngine.UI.Button UnityButton { get; }

		/// <inheritdoc />
		protected override string LoggableComponentName => UnityButton.gameObject.name;

		/// <summary>
		/// Keeps track of if a click is being handled
		/// </summary>
		private bool HandlingClick = false;

		private Coroutine CurrentClickCoroutine = null;

		/// <inheritdoc />
		public UnityButtonUIButtonAdapterImplementation([NotNull] Button unityButton)
			: base(unityButton)
		{
			UnityButton = unityButton ?? throw new ArgumentNullException(nameof(unityButton));

			//AddSubmitEvent(unityButton);
			UnityButton.onClick.AddListener(() =>
			{
				// Already clicking
				if (HandlingClick)
					return;

				// Up down real quick
				OnPointerDown(new PointerEventData(EventSystem.current));
				OnPointerUp(new PointerEventData(EventSystem.current));
			});
		}

		/// <inheritdoc />
		public bool IsInteractable
		{
			get => UnityButton.interactable;
			set => UnityButton.interactable = value;
		}

		/// <inheritdoc />
		public void SimulateClick(bool eventsOnly)
		{
			if (eventsOnly)
				OnSubmit(new BaseEventData(EventSystem.current));
			else
				ExecuteEvents.Execute(UnityButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}

		/// <inheritdoc />
		public void OnPointerDown(PointerEventData eventData)
		{
			try
			{
				var button = eventData.button.ToMouseButtonType();
				Bus.Publish(this, new OnElementClickedEventArgs(button, true));
			}
			finally
			{
				// Allow calling Down and Up same frame, so don't check or start multiple of these
				if (CurrentClickCoroutine == null)
					CurrentClickCoroutine = UnityButton.StartCoroutine(PreventMultipleClickCoroutine());
			}
		}

		/// <inheritdoc />
		public void OnPointerUp(PointerEventData eventData)
		{
			try
			{
				var button = eventData.button.ToMouseButtonType();
				Bus.Publish(this, new OnElementClickedEventArgs(button, false));
			}
			finally
			{
				// Allow calling Down and Up same frame, so don't check or start multiple of these
				if (CurrentClickCoroutine == null)
					CurrentClickCoroutine = UnityButton.StartCoroutine(PreventMultipleClickCoroutine());
			}
		}

		/// <inheritdoc />
		public void OnSubmit(BaseEventData eventData)
		{
			OnPointerDown(new PointerEventData(EventSystem.current));
			OnPointerUp(new PointerEventData(EventSystem.current));
		}

		/// <summary>
		/// Call this on the frame a click happens to prevent multiple clicks from happening the same frame.
		/// </summary>
		/// <returns></returns>
		private IEnumerator PreventMultipleClickCoroutine()
		{
			HandlingClick = true;
			yield return null;
			HandlingClick = false;
			CurrentClickCoroutine = null;
		}
	}
}
