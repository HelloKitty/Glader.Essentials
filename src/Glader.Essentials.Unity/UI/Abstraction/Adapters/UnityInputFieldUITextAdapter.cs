using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
		public abstract bool IsTextHighlighted { get; }

		/// <inheritdoc />
		public abstract void SetInputToEnd();

		/// <inheritdoc />
		public abstract void ActivateInput();

		/// <inheritdoc />
		public abstract void InsertText(string text);

		/// <inheritdoc />
		public abstract bool CheckCaretRichTextTagState(out bool isWithinTag, out bool isRightAfterTag);

		/// <inheritdoc />
		public abstract bool TryRemoveRichTextBlock();
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

		/// <inheritdoc />
		public override bool CheckCaretRichTextTagState(out bool isWithinTag, out bool isRightAfterTag)
		{
			int caretPosition = UnityUIObject.caretPosition;

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

			if (isWithinTag)
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
			if (textSelected
			    && !IsPartiallyHighlightingRichTextBlock())
				return false;

			// Same if we're not behind or within it.
			if (!CheckCaretRichTextTagState(out var _, out var _))
				return false;

			// Get the current selection range
			int selectionStart = Mathf.Min(UnityUIObject.selectionAnchorPosition, UnityUIObject.selectionFocusPosition);
			int selectionEnd = Mathf.Max(UnityUIObject.selectionAnchorPosition, UnityUIObject.selectionFocusPosition);

			// If there's no selection, set selection range to caret position for single delete action
			if (!textSelected)
			{
				int caretPosition = UnityUIObject.caretPosition;
				if (caretPosition > 0)
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
			UnityUIObject.caretPosition = selectionStart;
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
			if (openingTagMatchInRange.Success || closingTagMatchInRange.Success)
			{
				return true;
			}

			return false;
		}
	}
}
