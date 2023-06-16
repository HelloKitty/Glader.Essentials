using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Glader.Essentials
{
	/// <summary>
	/// See <see cref="IUILabeledButton"/>.
	/// </summary>
	public sealed class UnityTexhMeshProLabeledButtonAdapter : UnityButtonUIButtonAdapter, IUILabeledButton
	{
		[SerializeField]
		private UnityTextMeshProUGUIUITextAdapter TextUIObject;

		/// <inheritdoc />
		public string Text
		{
			get => TextUIObject.Text;
			set => TextUIObject.Text = value;
		}

		/// <inheritdoc />
		public void SetColor(byte r, byte g, byte b, byte a)
		{
			TextUIObject.SetColor(r, g, b, a);
		}
	}
}
