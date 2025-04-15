using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for UI elements that implement Input Field functionality.
	/// </summary>
	public interface IUIInputField : IUIText, IUITextChangedCallbackRegistrable
	{
		/// <summary>
		/// Indicates if any text is highlighted.
		/// </summary>
		bool IsTextHighlighted { get; }

		/// <summary>
		/// The mutable position of the Caret.
		/// </summary>
		int CaretPosition { get; set; }

		/// <summary>
		/// Indicates if the current caret position is within a link.
		/// </summary>
		bool IsCaretWithinLink { get; }

		/// <summary>
		/// Mutable character limit that can be set on the input field.
		/// </summary>
		int CharacterLimit { get; set; }

		/// <summary>
		/// Sets the input field, if it has a caret or enter position, to the end of the text.
		/// </summary>
		void SetInputToEnd();

		/// <summary>
		/// Activates the input field.
		/// </summary>
		void ActivateInput();

		/// <summary>
		/// Inserts the text into the string at the current caret positions.
		/// </summary>
		/// <param name="text">The text to insert.</param>
		void InsertText(string text);

		/// <summary>
		/// Attempts to remove a rich text block if the cursor is within or right behind it.
		/// It will do nothing if it doesn't find any RichText that matches that condition.
		/// </summary>
		/// <param name="replace">Indicates if the text should be replaced instead of removed.</param>
		/// <param name="replaceChar">The character to use for replacement.</param>
		/// <returns>True if a rich text block was removed.</returns>
		bool TryRemoveRichTextBlock(bool replace = false, char replaceChar = '-');

		/// <summary>
		/// Forces an update on the input field.
		/// </summary>
		void ForceUpdate();

		/// <summary>
		/// Indicates if at the string position <see cref="position"/> it's within a link tag for the provided string
		/// <see cref="input"/>.
		/// </summary>
		/// <param name="input">The string to check.</param>
		/// <param name="position">The string position to check for.</param>
		/// <returns>True if the position is within a link tag for the provided string.</returns>
		public bool IsWithinLinkTagAtPosition(string input, int position);
	}
}
