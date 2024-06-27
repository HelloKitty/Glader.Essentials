using System;
using System.Collections.Generic;
using System.Linq;
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
		public abstract int CaretPosition { get; set; }

		/// <inheritdoc />
		public abstract void SetInputToEnd();

		/// <inheritdoc />
		public abstract void ActivateInput();

		/// <inheritdoc />
		public abstract void InsertText(string text);

		/// <inheritdoc />
		public abstract bool TryRemoveRichTextBlock(bool replace = false, char replaceChar = '-');

		/// <inheritdoc />
		public abstract void ForceUpdate();

		/// <summary>
		/// Removes link rich text right before or surrounding the provided <see cref="position"/> in the
		/// input <see cref="input"/> string.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		protected static bool TryRemoveLinkAtPosition(ref string input, int position,
			out int editStartIndex, out int editEndIndex, bool replace = false, char replaceChar = '-')
		{
			editStartIndex = 0;
			editEndIndex = 0;

			if (string.IsNullOrWhiteSpace(input))
				return false;

			if (position >= input.Length)
				return false;

			// Find all matches in the input string
			var match = ComputeLinkTagMatchesForPosition(input);

			if (!match.Success)
				return false;

			int matchStart = match.Index;
			int matchEnd = match.Index + match.Length;

			if (position >= matchStart && position <= matchEnd
			   || position == matchEnd)
			{
				// If the position is within the match, remove it
				// OR
				// If the position is directly after the match, remove it
				input = input.Remove(matchStart, match.Length);

				if(replace)
					input = input.Insert(matchStart, new string(Enumerable.Repeat(replaceChar, match.Length).ToArray()));

				editStartIndex = match.Index;
				editEndIndex = match.Index + match.Length;
				return true;
			}

			// Return the original input if no match was found at the position
			return false;
		}

		private static Match ComputeLinkTagMatchesForPosition(string input)
		{
			return Regex.Match(input, @"(<link[^>]*?>.*?<\/link>)");
		}

		public static bool TryRemoveLinkAtPosition(ref string input, int startPosition, int endPosition, 
			out int editStartIndex, out int editEndIndex, bool replace = false, char replaceChar = '-')
		{
			editStartIndex = 0;
			editEndIndex = 0;

			// Safeguard against positions greater than the length of the input string
			if (startPosition >= input.Length || endPosition >= input.Length)
				return false;

			// Ensure startPosition is less than or equal to endPosition
			if (startPosition > endPosition)
			{
				int temp = startPosition;
				startPosition = endPosition;
				endPosition = temp;
			}

			// m.Index = Start
			// (m.Index + m.Length) = End
			// From AI: m.Index < endPosition && (m.Index + m.Length) > startPosition
			var matches = ComputeLinkTagMatchesForRange(input, startPosition, endPosition)
				.ToArray();

			if(!matches.Any())
				return false;

			// Iterate through the matches to find if the selection overlaps with a match
			foreach(Match match in matches)
			{
				// startPosition <= matchEnd && endPosition >= matchStart
				int matchStart = match.Index;

				// If the selection overlaps with the match, remove it
				input = input.Remove(matchStart, match.Length);

				if(replace)
					input = input.Insert(matchStart, new string(Enumerable.Repeat(replaceChar, match.Length).ToArray()));
			}

			editStartIndex = matches.Min(match => match.Index);
			editEndIndex = matches.Max(match => match.Index + match.Length);
			return true;
		}

		private static IEnumerable<Match> ComputeLinkTagMatchesForRange(string input, int startPosition, int endPosition)
		{
			return Regex.Matches(input, @"(<link[^>]*?>.*?<\/link>)")
				.OrderByDescending(m => m.Index)
				.Where(m => m.Index < endPosition && (m.Index + m.Length) > startPosition);
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

		/// <inheritdoc />
		public override int CaretPosition
		{
			get => UnityUIObject.caretPosition;
			set => UnityUIObject.caretPosition = value;
		}

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
		public override bool TryRemoveRichTextBlock(bool replace = false, char replaceChar = '-')
		{
			string originalText = Text;

			if(string.IsNullOrWhiteSpace(originalText))
				return false;

			// Nothing selected??
			if (UnityUIObject.selectionAnchorPosition == UnityUIObject.selectionFocusPosition)
			{
				if (TryRemoveLinkAtPosition(ref originalText, UnityUIObject.caretPosition, out var start, out var end, replace, replaceChar))
				{
					Text = originalText;

					// This will make it so that the caret position is at the
					// begining of the match meaning where the user basically expects/hopes it will be
					UnityUIObject.caretPosition = start;
					return true;
				}
			}
			else
			{
				if (TryRemoveLinkAtPosition(ref originalText, UnityUIObject.selectionAnchorPosition, UnityUIObject.selectionFocusPosition, out var start, out var end, replace, replaceChar))
				{
					Text = originalText;
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
