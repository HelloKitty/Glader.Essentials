using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using UnityEngine;
using TMPro;

namespace Glader.Essentials
{
	// WARNING: Critical projects depend on us not adopting the "TextMeshProUITextAdapter" name, we put Unity in front for compatibility
	// WARNING: Don't seal because the legacy implementations in critical projects inherit from them now.
	public class UnityTextMeshProUITextAdapter : BaseUnityUIAdapter<TextMeshPro, IUIText>, IUIText
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
