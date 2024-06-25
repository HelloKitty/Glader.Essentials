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
		public override bool CheckCaretRichTextTagState(out bool isWithinTag, out bool isRightAfterTag)
		{
			int caretPosition = UnityUIObject.stringPosition;

			// Get current text
			string currentText = UnityUIObject.text;

			// Define regex patterns
			string openingTagPattern = @"<[^\/>][^>]*?>";
			string closingTagPattern = @"<\/[^>]+?>";

			// Get the text before and after the caret position
			string textBeforeCaret = currentText.Substring(0, caretPosition);
			string textAfterCaret = currentText.Substring(caretPosition);

			// Find matches for opening and closing tags
			MatchCollection openingTagsBeforeCaret = Regex.Matches(textBeforeCaret, openingTagPattern);
			MatchCollection closingTagsBeforeCaret = Regex.Matches(textBeforeCaret, closingTagPattern);
			MatchCollection closingTagsAfterCaret = Regex.Matches(textAfterCaret, closingTagPattern);

			// Check if there is an unmatched opening tag before the caret
			bool hasUnmatchedOpeningTagBeforeCaret = openingTagsBeforeCaret.Count > closingTagsBeforeCaret.Count;
			bool hasClosingTagAfterCaret = closingTagsAfterCaret.Count > 0;

			// Check if the caret is within a rich text tag
			isWithinTag = hasUnmatchedOpeningTagBeforeCaret && hasClosingTagAfterCaret;

			if(isWithinTag)
			{
				isRightAfterTag = false;
				return true;
			}

			// GPT explained this check as: The check if (caretPosition >= closingTagPattern.Length) ensures that there is enough text before the caret position to match a closing tag. This prevents potential errors when trying to extract a substring that is too short to contain a full closing tag.
			// Check if the caret is right after a closing rich text tag
			if(caretPosition >= closingTagPattern.Length)
			{
				string textBeforeCaretWithPadding = currentText.Substring(0, caretPosition);
				isRightAfterTag = Regex.IsMatch(textBeforeCaretWithPadding, closingTagPattern + "$");
			}
			else
				isRightAfterTag = false;

			return isWithinTag || isRightAfterTag;
		}

		/// <inheritdoc />
		public override bool TryRemoveRichTextBlock()
		{
			// Case where we have text selected
			// if we do and we're not highlighting a rich text block partially then
			// we don't need to do anything fancy.
			bool textSelected = CheckIsTextSelected();
			if(textSelected
				&& !IsPartiallyHighlightingRichTextBlock())
				return false;

			// Same if we're not behind or within it.
			if(!CheckCaretRichTextTagState(out var _, out var _))
				return false;

			// Get the current selection range
			int selectionStart = Mathf.Min(UnityUIObject.selectionAnchorPosition, UnityUIObject.selectionFocusPosition);
			int selectionEnd = Mathf.Max(UnityUIObject.selectionAnchorPosition, UnityUIObject.selectionFocusPosition);

			// If there's no selection, set selection range to caret position for single delete action
			if(!textSelected)
			{
				int caretPosition = UnityUIObject.stringPosition;
				if(caretPosition > 0)
				{
					selectionStart = caretPosition - 1;
					selectionEnd = caretPosition;
				}
				else if(caretPosition < UnityUIObject.text.Length)
				{
					selectionStart = caretPosition;
					selectionEnd = caretPosition + 1;
				}
			}

			// Get current text
			string currentText = Text;

			// Define regex patterns
			string openingTagPattern = @"<[^\/>][^>]*?>";
			string closingTagPattern = @"<\/[^>]+?>";

			// Get the text within the selection range
			string textInRange = currentText.Substring(selectionStart, selectionEnd - selectionStart);

			// Find any partial tags within the selection range
			Match openingTagMatchInRange = Regex.Match(textInRange, openingTagPattern);
			Match closingTagMatchInRange = Regex.Match(textInRange, closingTagPattern);

			// If a partial opening tag is found within the selection range
			if(openingTagMatchInRange.Success)
			{
				int openingTagStart = currentText.LastIndexOf('<', selectionStart);
				int openingTagEnd = currentText.IndexOf('>', openingTagStart) + 1;
				selectionStart = openingTagStart;
				selectionEnd = Mathf.Max(selectionEnd, openingTagEnd);
			}

			// If a partial closing tag is found within the selection range
			if(closingTagMatchInRange.Success)
			{
				int closingTagStart = currentText.IndexOf('<', selectionEnd);
				int closingTagEnd = currentText.IndexOf('>', closingTagStart) + 1;
				selectionStart = Mathf.Min(selectionStart, closingTagStart);
				selectionEnd = closingTagEnd;
			}

			// Remove the text within the adjusted selection range
			string newText = currentText.Remove(selectionStart, selectionEnd - selectionStart);
			Text = newText;
			UnityUIObject.stringPosition = selectionStart;
			return true;
		}

		/// <summary>
		/// Checks if the highlighted text partially includes a rich text block.
		/// </summary>
		/// <returns>True if the highlighted text partially includes a rich text block, otherwise false.</returns>
		private bool IsPartiallyHighlightingRichTextBlock()
		{
			// Get the current selection range
			int selectionStart = Mathf.Min(UnityUIObject.selectionAnchorPosition, UnityUIObject.selectionFocusPosition);
			int selectionEnd = Mathf.Max(UnityUIObject.selectionAnchorPosition, UnityUIObject.selectionFocusPosition);

			// If there's no selection, return false
			if(selectionStart == selectionEnd)
			{
				return false;
			}

			// Get current text
			string currentText = Text;

			// Define regex patterns
			string openingTagPattern = @"<[^\/>][^>]*?>";
			string closingTagPattern = @"<\/[^>]+?>";

			// Get the text within the selection range
			string textInRange = currentText.Substring(selectionStart, selectionEnd - selectionStart);

			// Find any partial tags within the selection range
			Match openingTagMatchInRange = Regex.Match(textInRange, openingTagPattern);
			Match closingTagMatchInRange = Regex.Match(textInRange, closingTagPattern);

			// Check if a partial opening or closing tag is found within the selection range
			if(openingTagMatchInRange.Success || closingTagMatchInRange.Success)
			{
				return true;
			}

			return false;
		}
	}
}
