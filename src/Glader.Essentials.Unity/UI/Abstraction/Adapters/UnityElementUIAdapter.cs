using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using JetBrains.Annotations;
using UnityEngine;

namespace Glader
{
	public sealed class UnityElementUIAdapter : BaseUnityUIAdapter<Transform, IUIElement>, IUIElement
	{
		public void SetElementActive(bool state)
		{
			gameObject.SetActive(state);
		}

		public bool isActive => gameObject.activeSelf;
	}
}
