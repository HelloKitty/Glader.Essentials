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
		/// <summary>
		/// Event fired when the button is clicked.
		/// </summary>
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

		/// <inheritdoc />
		public ButtonClickedEventArgs(IUIButton button)
		{
			Button = button ?? throw new ArgumentNullException(nameof(button));
		}
	}

	/// <summary>
	/// Base event listener that can listen for named/typed button events.
	/// </summary>
	/// <typeparam name="TButtonClickedEventType">The button event type.</typeparam>
	public abstract class ButtonClickedEventListener<TButtonClickedEventType> : EngineEventListener<TButtonClickedEventType, ButtonClickedEventArgs>
		where TButtonClickedEventType : class, IButtonClickedEventSubscribable
	{
		/// <inheritdoc />
		protected ButtonClickedEventListener(TButtonClickedEventType subscriptionService)
			: base(subscriptionService)
		{

		}
	}

	/// <summary>
	/// Base event listener that can listen for named/typed button events and dispatch their click handling async.
	/// </summary>
	/// <typeparam name="TButtonClickedEventType">The button event type.</typeparam>
	public abstract class ButtonClickedEventListenerAsync<TButtonClickedEventType> : EngineEventListenerAsync<TButtonClickedEventType, ButtonClickedEventArgs>
		where TButtonClickedEventType : class, IButtonClickedEventSubscribable
	{
		/// <inheritdoc />
		protected ButtonClickedEventListenerAsync(TButtonClickedEventType subscriptionService)
			: base(subscriptionService)
		{

		}
	}
}
