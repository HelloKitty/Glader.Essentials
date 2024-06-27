using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

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
			//Debug.LogError($"Validate Pos: {pos} Ch: {ch} Text: {text}");

			if (InputField.IsTextHighlighted
				&& InputField.IsCaretWithinLink)
			{
				//Debug.LogError("Failed highlight validate");
				return '\0';
			}
			else if (text.Contains('<') // Attempt to semi-optimize link checks
				   && InputField.IsWithinLinkTagAtPosition(text, pos))
			{
				//Debug.LogError("Failed in link validate");
				return '\0';
			}

			text = text.Insert(pos, ch.ToString());

			pos += 1;
			return ch;
		}
	}
}
