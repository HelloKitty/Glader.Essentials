using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace Glader.Essentials
{
	public class UnityTextMeshProUGUIUITextAdapter : BaseUnityUIAdapter<TextMeshProUGUI, IUIText>, IUIText, IPointerClickHandler
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

		/// <inheritdoc />
		public void OnPointerClick(PointerEventData eventData)
		{
			// Really this should only be for links
			int linkIndex = TMP_TextUtilities.FindIntersectingLink(UnityUIObject, eventData.position, null);
			if (linkIndex != -1)
			{
				TMP_LinkInfo linkInfo = UnityUIObject.textInfo.linkInfo[linkIndex];
				Bus.Publish(this, new TextLinkClickedEventArgs(eventData.button.ToMouseButtonType(), linkInfo.GetLinkID(), linkInfo.GetLinkText()));
			}
		}
	}
}
