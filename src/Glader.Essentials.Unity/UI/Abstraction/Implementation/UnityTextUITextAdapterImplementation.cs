using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Glader.Essentials
{
	public sealed class UnityTextUITextAdapterImplementation : IUIText
	{
		private UnityEngine.UI.Text UnityText { get; }

		/// <inheritdoc />
		public IEventBus Bus { get; } = new EventBus();

		/// <inheritdoc />
		public UnityTextUITextAdapterImplementation([NotNull] Text unityText)
		{
			UnityText = unityText ?? throw new ArgumentNullException(nameof(unityText));
		}

		/// <inheritdoc />
		public string Text
		{
			get => UnityText.text;
			set => UnityText.text = value;
		}

		/// <inheritdoc />
		public void SetElementActive(bool state)
		{
			UnityText.gameObject.SetActive(state);
		}

		/// <inheritdoc />
		public bool IsActive => UnityText.gameObject.activeSelf;

		/// <inheritdoc />
		public void SetColor(byte r, byte g, byte b, byte a)
		{
			UnityText.color = new Color32(r, g, b, a);
		}
	}
}
