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
		/// <returns>True if a rich text block was removed.</returns>
		bool TryRemoveRichTextBlock();
	}
}
