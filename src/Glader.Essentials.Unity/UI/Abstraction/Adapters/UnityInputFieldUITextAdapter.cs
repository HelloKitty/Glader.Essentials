﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Glader.Essentials
{
	/// <summary>
	/// Base adapter for Input Field <see cref="IUIInputField"/> types.
	/// </summary>
	/// <typeparam name="TComponentType">The backing component type.</typeparam>
	public abstract class BaseUnityInputFieldAdapter<TComponentType> : BaseUnityUIAdapter<TComponentType, IUIInputField>, IUIInputField
		where TComponentType : UnityEngine.EventSystems.UIBehaviour
	{
		/// <inheritdoc />
		public abstract string Text { get; set; }

		/// <inheritdoc />
		public abstract void SetColor(byte r, byte g, byte b, byte a);

		/// <inheritdoc />
		public abstract void RegisterTextChangeCallback(Action<string> callback);

		/// <inheritdoc />
		public abstract void SetInputToEnd();

		/// <inheritdoc />
		public abstract void ActivateInput();

		/// <inheritdoc />
		public abstract void InsertText(string text);
	}

	/// <summary>
	/// Adapter for <see cref="InputField"/> to <see cref="IUIInputField"/>
	/// </summary>
	public class UnityInputFieldUITextAdapter : BaseUnityInputFieldAdapter<InputField>, IUIInputField
	{
		/// <inheritdoc />
		public override string Text
		{
			get => UnityUIObject.text;
			set => UnityUIObject.text = value;
		}

		/// <inheritdoc />
		public override void SetColor(byte r, byte g, byte b, byte a)
		{
			UnityUIObject.textComponent.color = new Color32(r, g, b, a);
		}

		/// <inheritdoc />
		public override void RegisterTextChangeCallback(Action<string> callback)
		{
			UnityUIObject.onValueChanged.AddListener(args =>
			{
				callback(args);
			});
		}

		/// <inheritdoc />
		public override void SetInputToEnd()
		{
			UnityUIObject.MoveTextEnd(false);
		}

		/// <inheritdoc />
		public override void ActivateInput()
		{
			// Should be enough I hope.
			UnityUIObject.ActivateInputField();
		}

		/// <inheritdoc />
		public override void InsertText(string text)
		{
			int caretPosition = UnityUIObject.caretPosition;

			// Get current text
			string currentText = UnityUIObject.text;

			// Insert the text at the caret position
			string newText = currentText.Insert(caretPosition, text);

			// Update the input field text
			Text = newText;

			// Move caret position after inserted text
			UnityUIObject.caretPosition = caretPosition + text.Length;
		}
	}
}
