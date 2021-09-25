using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unitysync.Async;

namespace Glader.Essentials
{
	public class UnityButtonUIButtonAdapter : BaseUnityUIAdapter<Button, IUIButton>, IUIButton
	{
		/// <inheritdoc />
		protected override IUIElement Element => Adapter.Value;

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
		public bool IsInteractable
		{
			get => Adapter.Value.IsInteractable;
			set => Adapter.Value.IsInteractable = value;
		}

		/// <inheritdoc />
		public void SimulateClick(bool eventsOnly)
		{
			Adapter.Value.SimulateClick(eventsOnly);
		}
	}
}
