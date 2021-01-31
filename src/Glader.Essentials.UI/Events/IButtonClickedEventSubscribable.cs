using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Event subscription interface for button events.
	/// </summary>
	public interface IButtonClickedEventSubscribable
	{
		event EventHandler<ButtonClickedEventArgs> OnButtonClicked;
	}

	/// <summary>
	/// <see cref="EventArgs"/> for the button click event.
	/// </summary>
	public sealed class ButtonClickedEventArgs : EventArgs
	{
		/// <summary>
		/// The button being clicked.
		/// </summary>
		public IUIButton Button { get; }

		public ButtonClickedEventArgs(IUIButton button)
		{
			Button = button ?? throw new ArgumentNullException(nameof(button));
		}
	}
}
