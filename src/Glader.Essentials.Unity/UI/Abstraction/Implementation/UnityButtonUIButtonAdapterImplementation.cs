﻿using System;
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
	public sealed class UnityButtonUIButtonAdapterImplementation : BaseUnityUIAdapterImplementation, IUIButton
	{
		private UnityEngine.UI.Button UnityButton { get; }

		/// <inheritdoc />
		protected override string LoggableComponentName => UnityButton.gameObject.name;

		/// <inheritdoc />
		public UnityButtonUIButtonAdapterImplementation([NotNull] Button unityButton)
			: base(unityButton)
		{
			UnityButton = unityButton ?? throw new ArgumentNullException(nameof(unityButton));
			UnityButton.onClick.AddListener(() => Bus.PublishSimple<OnElementClickedEventArgs>(this));
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
	}
}
