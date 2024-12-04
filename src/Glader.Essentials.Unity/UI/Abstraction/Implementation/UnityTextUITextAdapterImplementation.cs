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
	public sealed class UnityTextUITextAdapterImplementation : IUIText, IDisposable
	{
		private UnityEngine.UI.Text UnityText { get; }

		/// <inheritdoc />
		public IEventBus Bus { get; } = new EventBus();

		/// <inheritdoc />
		public string Text
		{
			get => UnityText.text;
			set => UnityText.text = value;
		}

		/// <inheritdoc />
		public bool IsObjectActive => UnityText.gameObject.activeSelf;

		/// <inheritdoc />
		public bool IsComponentActive => UnityText.enabled;

		/// <inheritdoc />
		public UnityTextUITextAdapterImplementation([NotNull] Text unityText)
		{
			UnityText = unityText ?? throw new ArgumentNullException(nameof(unityText));
		}

		/// <inheritdoc />
		public void SetObjectActive(bool state)
		{
			UnityText.gameObject.SetActive(state);
		}

		/// <inheritdoc />
		public void SetComponentActive(bool state)
		{
			UnityText.enabled = state;
		}

		/// <inheritdoc />
		public void SetColor(byte r, byte g, byte b, byte a)
		{
			UnityText.color = new Color32(r, g, b, a);
		}

		public void Dispose()
		{
			Bus?.Dispose();
		}
	}
}
