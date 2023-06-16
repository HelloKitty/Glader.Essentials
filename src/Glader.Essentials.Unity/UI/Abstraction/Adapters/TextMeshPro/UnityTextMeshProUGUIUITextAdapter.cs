using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using UnityEngine;
using TMPro;

namespace Glader.Essentials
{
	public class UnityTextMeshProUGUIUITextAdapter : BaseUnityUIAdapter<TextMeshProUGUI, IUIText>, IUIText
	{
		public string Text
		{
			get => UnityUIObject.text;
			set => UnityUIObject.text = value;
		}

		/// <inheritdoc />
		public void SetColor(byte r, byte g, byte b, byte a)
		{
			UnityUIObject.color = new Color32(r, g, b, a);
		}
	}
}
