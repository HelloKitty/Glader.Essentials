using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using TMPro;

namespace Glader.Essentials
{
	/// <summary>
	/// <see cref="TMP_InputValidator"/> which prevents inserting characters within a string
	/// if 
	/// </summary>
	public sealed class TMP_LinkTagTextInputValidator : TMP_InputValidator
	{
		private IUIInputField InputField { get; }

		public TMP_LinkTagTextInputValidator([NotNull] IUIInputField inputField)
		{
			InputField = inputField ?? throw new ArgumentNullException(nameof(inputField));
		}

		/// <inheritdoc />
		public override char Validate(ref string text, ref int pos, char ch)
		{
			// Don't allow any typing when within a link
			if (InputField.IsWithinLinkTagAtPosition(text, pos))
				return '\0';

			pos += 1;
			return ch;
		}
	}
}
