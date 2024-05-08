using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Glader.Essentials;

namespace Glader.Essentials
{
	public class UnityTextMeshProUGUIUIInputFieldAdapter : BaseUnityInputFieldAdapter<TMP_InputField>
	{
		/// <summary>
		/// Indicates if the input is focused.
		/// </summary>
		public bool IsFocused => UnityUIObject.isFocused;

		/// <summary>
		/// The width of the caret.
		/// </summary>
		public int CaretWidth
		{
			get => UnityUIObject.caretWidth;
			set => UnityUIObject.caretWidth = value;
		}

		public override void SetColor(byte r, byte g, byte b, byte a)
		{
			UnityUIObject.textComponent.color = new Color32(r, g, b, a);
		}

		public override string Text
		{
			get => UnityUIObject.text;
			set => UnityUIObject.text = value;
		}

		/// <inheritdoc />
		public override void RegisterTextChangeCallback(Action<string> callback)
		{
			UnityUIObject.onValueChanged.AddListener(args =>
			{
				callback(args);
			});
		}

		/// <summary>
		/// Registers a selection callback for the text input.
		/// Calls <see cref="callback"/> on select.
		/// </summary>
		/// <param name="callback">The callback to register/call on select.</param>
		public void RegisterSelectCallback(Action<string> callback)
		{
			UnityUIObject.onSelect.AddListener(args =>
			{
				callback(args);
			});
		}

		/// <summary>
		/// Registers the <see cref="OnTextFieldSubmitEventArgs"/> event for publishing through
		/// the <see cref="Bus"/>.
		/// </summary>
		public void SetupSubmitPublishing()
		{
			UnityUIObject.onSubmit.AddListener(value =>
			{
				Bus.Publish(this, new OnTextFieldSubmitEventArgs(this, value));
			});
		}

		/// <inheritdoc />
		public override void SetInputToEnd()
		{
			// Don't count on MoveToEnd, typical Unity3D doesn't work lol
			// directly set the caret position
			UnityUIObject.caretPosition = Text.Length;
		}

		/// <inheritdoc />
		public override void ActivateInput()
		{
			// Should be enough
			UnityUIObject.ActivateInputField();
		}
	}
}
