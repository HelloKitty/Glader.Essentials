using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Glader.Essentials
{
	// From SwanSong
	/// <summary>
	/// Attach to a component and assign the prefix and the text input fields.
	/// This will auto-scale and adjust the size of the text input.
	/// </summary>
	public sealed class PrefixedInputFieldSizer : MonoBehaviour
	{
		public RectTransform rootRectTransform;
		public TextMeshProUGUI textPrefix;     // Assign your Text's RectTransform
		public RectTransform spacerRectTransform;   // Assign your Spacer's RectTransform
		public RectTransform textRectTransform;

		private float lastSize = 0;

		void Update()
		{
			// Simpliest way to tell if our text thingy is not enabled.
			if (String.IsNullOrWhiteSpace(textPrefix.text) 
			    || !textPrefix.isActiveAndEnabled)
				return;

			float totalWidth = rootRectTransform.rect.width;
			float textWidth = textPrefix.rectTransform.rect.width;
			float spacerWidth = spacerRectTransform.rect.width;
			float availableWidth = totalWidth - textWidth - spacerWidth;

			textRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, availableWidth);

			if(Math.Abs(lastSize - availableWidth) > 0.1f)
			{
				availableWidth = 0;
				lastSize = availableWidth;
				LayoutRebuilder.MarkLayoutForRebuild(textRectTransform);
			}
		}
	}
}
