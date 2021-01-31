using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Simple <see cref="IGameStartable"/> that implements <see cref="IButtonClickedEventSubscribable"/>
	/// which can be used to glue <see cref="IUIButton"/> adapters to event subscribable interfaces.
	/// </summary>
	public abstract class ButtonClickedGlue : IGameStartable, IButtonClickedEventSubscribable
	{
		/// <inheritdoc />
		public event EventHandler<ButtonClickedEventArgs> OnButtonClicked;

		/// <summary>
		/// The button to glue the <see cref="IButtonClickedEventSubscribable"/> event to.
		/// </summary>
		private IUIButton ReferenceButton { get; }

		/// <summary>
		/// Creates a new glue to bind a specified <see cref="IButtonClickedEventSubscribable"/> subscriable type
		/// to a <paramref name="referenceButton"/>'s clickable event.
		/// </summary>
		/// <param name="referenceButton"></param>
		protected ButtonClickedGlue(IUIButton referenceButton)
		{
			ReferenceButton = referenceButton ?? throw new ArgumentNullException(nameof(referenceButton), "Maybe you're missing autofac key filter.");
		}

		/// <inheritdoc />
		public Task OnGameStart()
		{
			ReferenceButton.AddOnClickListener(() => { OnButtonClicked?.Invoke(ReferenceButton, new ButtonClickedEventArgs(ReferenceButton)); });
			return Task.CompletedTask;
		}
	}
}
