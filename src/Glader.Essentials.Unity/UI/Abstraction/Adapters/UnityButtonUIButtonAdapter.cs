using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unitysync.Async;

namespace Glader.Essentials
{
	public sealed class UnityButtonUIButtonAdapter : BaseUnityUIAdapter<Button, IUIButton>, IUIButton
	{
		//This is sorta the new design
		//Create an adapter property that will actually handle the adaptor
		//the responsibility of this class is to expose registeration and to
		//handle the internal complicated parts of exposing it to the editor.
		private Lazy<UnityButtonUIButtonAdapterImplementation> Adapter { get; set; }

		//On awake we should just create the adapter for
		//adaptation forwarding.
		public UnityButtonUIButtonAdapter()
		{
			Adapter = new Lazy<UnityButtonUIButtonAdapterImplementation>(() => new UnityButtonUIButtonAdapterImplementation(UnityUIObject));
		}

		/// <inheritdoc />
		public void AddOnClickListener(Action action)
		{
			Adapter.Value.AddOnClickListener(action);
		}

		/// <inheritdoc />
		public void AddOnClickListenerAsync(Func<Task> action)
		{
			Adapter.Value.AddOnClickListenerAsync(action);
		}

		/// <inheritdoc />
		public bool IsInteractable
		{
			get => Adapter.Value.IsInteractable;
			set => Adapter.Value.IsInteractable = value;
		}

		public void SimulateClick(bool eventsOnly)
		{
			if (eventsOnly)
				UnityUIObject.onClick?.Invoke();
			else
				ExecuteEvents.Execute(this.UnityUIObject.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}
	}
}
