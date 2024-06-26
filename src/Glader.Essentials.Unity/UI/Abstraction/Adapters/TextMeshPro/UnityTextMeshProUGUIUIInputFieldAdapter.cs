using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

		/// <inheritdoc />
		public override void SetColor(byte r, byte g, byte b, byte a)
		{
			UnityUIObject.textComponent.color = new Color32(r, g, b, a);
		}

		/// <inheritdoc />
		public override string Text
		{
			get => UnityUIObject.text;
			set => UnityUIObject.text = value;
		}

		/// <inheritdoc />
		public override bool IsTextHighlighted => CheckIsTextSelected();

		private bool CheckIsTextSelected()
		{
			// From GPT https://chatgpt.com/c/991eb24b-86a8-443f-be6c-be5213d55b6e
			// Check if text is selected (highlighted)
			int selectionStart = Mathf.Min(UnityUIObject.selectionAnchorPosition, UnityUIObject.selectionFocusPosition);
			int selectionEnd = Mathf.Max(UnityUIObject.selectionAnchorPosition, UnityUIObject.selectionFocusPosition);

			return selectionStart != selectionEnd;
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
			UnityUIObject.stringPosition = Text.Length;
		}

		/// <inheritdoc />
		public override void ActivateInput()
		{
			// Should be enough
			UnityUIObject.ActivateInputField();
		}

		/// <inheritdoc />
		public override void InsertText(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return;

			// Simple case where the current string is empty.
			if (string.IsNullOrWhiteSpace(Text))
			{
				Text = text;

				// Move caret position to the end of the new text
				UnityUIObject.stringPosition = text.Length;
				return;
			}

			// Get the current string position (accounts for rich text tags)
			int stringPosition = UnityUIObject.stringPosition;

			// Get current text
			string currentText = UnityUIObject.text;

			// Insert the text at the caret position
			string newText = currentText.Insert(stringPosition, text);

			// Update the input field text
			Text = newText;

			// Move caret position after inserted text
			UnityUIObject.stringPosition = stringPosition + text.Length;
		}

		/// <inheritdoc />
		public override bool TryRemoveRichTextBlock(bool replace = false, char replaceChar = '-')
		{
			string originalText = Text;

			if(string.IsNullOrWhiteSpace(originalText))
				return false;

			// Nothing selected??
			if (UnityUIObject.selectionAnchorPosition == UnityUIObject.selectionFocusPosition)
			{
				if (TryRemoveLinkAtPosition(ref originalText, UnityUIObject.stringPosition, out var start, out var end, replace, replaceChar))
				{
					Text = originalText;

					// This forces the caret to the start of the link text.
					UnityUIObject.stringPosition = start;
					return true;
				}
			}
			else
			{
				// The reason we do this is because the position of the anchors is actually wrong and fucked (proven!)
				int offset = UnityUIObject.stringPosition - UnityUIObject.caretPosition;

				if (TryRemoveLinkAtPosition(ref originalText, UnityUIObject.selectionAnchorPosition + offset, UnityUIObject.selectionFocusPosition + offset, out var start, out var end, replace, replaceChar))
				{
					Text = originalText;

					// This forces the caret to the start of the link text.
					UnityUIObject.stringPosition = start;
					return true;
				}
			}

			return false;
		}

		/// <inheritdoc />
		public override void ForceUpdate()
		{
			UnityUIObject.ForceLabelUpdate();
		}
	}
}
