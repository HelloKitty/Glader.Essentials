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
		/// Checks if the caret is within a rich text tag or right after a rich text tag.
		/// </summary>
		/// <param name="isWithinTag">Output parameter that is true if the caret is within a rich text tag.</param>
		/// <param name="isRightAfterTag">Output parameter that is true if the caret is right after a rich text tag.</param>
		/// <returns>True if any of the parameters are not false.</returns>
		bool CheckCaretRichTextTagState(out bool isWithinTag, out bool isRightAfterTag);
	}
}
