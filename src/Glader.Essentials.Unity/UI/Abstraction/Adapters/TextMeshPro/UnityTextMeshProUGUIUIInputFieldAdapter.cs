using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Glader.Essentials;

namespace Glader.Essentials
{
	public class UnityTextMeshProUGUIUIInputFieldAdapter : BaseUnityInputFieldAdapter<TMP_InputField>
	{
		public override void SetColor(byte r, byte g, byte b, byte a)
		{
			UnityUIObject.textComponent.color = new Color32(r, g, b, a);
		}

		public override string Text
		{
			get => UnityUIObject.text;
			set => UnityUIObject.text = value;
		}
	}
}
