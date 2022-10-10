using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a text rendered in the UI.
	/// </summary>
	public interface IUIText : IUIElement<IUIText>, IUIColorable
	{
		/// <summary>
		/// The text field of the UI text.
		/// </summary>
		string Text { get; set; }
	}
}
