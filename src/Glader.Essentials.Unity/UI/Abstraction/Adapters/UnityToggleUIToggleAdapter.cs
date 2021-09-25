using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.UI;
using Unitysync.Async;

namespace Glader.Essentials
{
	public sealed class UnityToggleUIToggleAdapter : BaseUnityUIAdapter<Toggle, IUIToggle>, IUIToggle
	{
		/// <inheritdoc />
		public bool IsInteractable
		{
			get => UnityUIObject.interactable;
			set => UnityUIObject.interactable = value;
		}
	}
}
