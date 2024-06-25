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
		public abstract bool TryRemoveRichTextBlock();

		/// <summary>
		/// Removes link rich text right before or surrounding the provided <see cref="position"/> in the
		/// input <see cref="input"/> string.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		protected static string RemoveLinkAtPosition(string input, int position)
		{
			if (string.IsNullOrWhiteSpace(input))
				return input;

			if (position >= input.Length)
				return input;

			// Create a regex object with the pattern
			Regex regex = new Regex(@"(<link.*<\/link>)");

			// Find all matches in the input string
			var matches = regex.Matches(input);

			// Iterate through the matches to find if the position is within or directly after a match
			foreach(Match match in matches)
			{
				int matchStart = match.Index;
				int matchEnd = match.Index + match.Length;

				if (position >= matchStart && position <= matchEnd)
				{
					// If the position is within the match, remove it
					return input.Remove(matchStart, match.Length);
				}
				else if(position == matchEnd + 1)
				{
					// If the position is directly after the match, remove it
					return input.Remove(matchStart, match.Length);
				}
			}

			// Return the original input if no match was found at the position
			return input;
		}
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
		public override bool TryRemoveRichTextBlock()
		{
			string currentText = Text;

			if(string.IsNullOrWhiteSpace(currentText))
				return false;

			// Nothing selected??
			if (UnityUIObject.selectionAnchorPosition == UnityUIObject.selectionFocusPosition)
			{
				Text = RemoveLinkAtPosition(currentText, UnityUIObject.caretPosition);
			}
			else
			{
				var initialString = currentText;

				// Remove any relevant at the end
				// then the front (in that order to avoid breaking the offsets)
				// anything captured fully inbetween will be deleted normally I guess?
				var newString = RemoveLinkAtPosition(initialString, UnityUIObject.selectionFocusPosition);
				newString = RemoveLinkAtPosition(newString, UnityUIObject.selectionAnchorPosition);

				Text = newString;
			}

			return Text != currentText;
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
