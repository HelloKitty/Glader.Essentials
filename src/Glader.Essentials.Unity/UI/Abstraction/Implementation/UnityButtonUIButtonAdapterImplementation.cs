using System;
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
		: BaseUnityUIAdapterImplementation, IUIButton, IPointerDownHandler, IPointerUpHandler
	{
		private UnityEngine.UI.Button UnityButton { get; }

		/// <inheritdoc />
		protected override string LoggableComponentName => UnityButton.gameObject.name;

		/// <summary>
		/// Keeps track of if a click is being handled
		/// </summary>
		private bool HandlingClick = false;

		/// <inheritdoc />
		public UnityButtonUIButtonAdapterImplementation([NotNull] Button unityButton)
			: base(unityButton)
		{
			UnityButton = unityButton ?? throw new ArgumentNullException(nameof(unityButton));

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
			if(eventsOnly)
				UnityButton.onClick?.Invoke();
			else
				ExecuteEvents.Execute(UnityButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}

		/// <inheritdoc />
		public void OnPointerDown(PointerEventData eventData)
		{
			HandlingClick = true;
			try
			{
				var button = eventData.button.ToMouseButtonType();
				Bus.Publish(this, new OnElementClickedEventArgs(button, true));
			}
			finally
			{
				HandlingClick = false;
			}
		}

		/// <inheritdoc />
		public void OnPointerUp(PointerEventData eventData)
		{
			HandlingClick = true;
			try
			{
				var button = eventData.button.ToMouseButtonType();
				Bus.Publish(this, new OnElementClickedEventArgs(button, false));
			}
			finally
			{
				HandlingClick = false;
			}
		}
	}
}
