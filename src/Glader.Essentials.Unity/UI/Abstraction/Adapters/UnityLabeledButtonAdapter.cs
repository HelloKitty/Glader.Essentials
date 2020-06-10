﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Glader.Essentials
{
	/// <summary>
	/// See <see cref="IUILabeledButton"/>.
	/// </summary>
	public sealed class UnityLabeledButtonAdapter : BaseTwoComponentUnityUIAdapter<Button, Text, IUILabeledButton>, IUILabeledButton
	{
		private UnityTextUITextAdapterImplementation TextAdapter { get; set; }

		private UnityButtonUIButtonAdapterImplementation ButtonAdapter { get; set; }

		/// <inheritdoc />
		protected override void Initialize()
		{
			TextAdapter = new UnityTextUITextAdapterImplementation(UnityUIObject2);
			ButtonAdapter = new UnityButtonUIButtonAdapterImplementation(UnityUIObject);
		}

		/// <inheritdoc />
		public string Text
		{
			get
			{
				EnsureInitialized();
				return TextAdapter.Text;
			}
			set
			{
				EnsureInitialized();
				TextAdapter.Text = value;
			}
		}

		/// <inheritdoc />
		public void AddOnClickListener(Action action)
		{
			EnsureInitialized();

			ButtonAdapter.AddOnClickListener(action);
		}

		/// <inheritdoc />
		public void AddOnClickListenerAsync(Func<Task> action)
		{
			EnsureInitialized();

			ButtonAdapter.AddOnClickListenerAsync(action);
		}

		/// <inheritdoc />
		public bool IsInteractable
		{
			get
			{
				EnsureInitialized();
				return ButtonAdapter.IsInteractable;
			}
			set
			{
				EnsureInitialized();
				ButtonAdapter.IsInteractable = value;
			}
		}

		public void SimulateClick(bool eventsOnly)
		{
			if(eventsOnly)
				UnityUIObject.onClick?.Invoke();
			else
				ExecuteEvents.Execute(this.UnityUIObject.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}

		/// <inheritdoc />
		public void SetElementActive(bool state)
		{
			gameObject.SetActive(state);
		}

		public void RemoveOnClickListener(Action action)
		{
			ButtonAdapter.RemoveOnClickListener(action);
		}

		public bool isActive => UnityUIObject.gameObject.activeSelf;
	}
}
