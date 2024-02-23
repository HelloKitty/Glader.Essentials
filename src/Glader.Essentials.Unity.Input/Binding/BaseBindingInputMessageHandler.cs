using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.Essentials.Binding
{
	/// <summary>
	/// Base class implementation of <see cref="IBindingInputMessageHandler{TKeyBindingEnumType}"/>.
	/// Bind handlers should inherit from this type and handle binding press state change logic in <see cref="HandleBindPressState"/>.
	/// </summary>
	public abstract class BaseBindingInputMessageHandler<TKeybindingEnumType> : IBindingInputMessageHandler<TKeybindingEnumType>
		where TKeybindingEnumType : Enum
	{
		private HashSet<TKeybindingEnumType> _BindingsSet { get; } = new();

		/// <inheritdoc />
		public IEnumerable<TKeybindingEnumType> Bindings => _BindingsSet;

		private BindingInputHandlingFlags HandleFlags { get; }

		protected BaseBindingInputMessageHandler(TKeybindingEnumType binding,
			BindingInputHandlingFlags handleFlags = BindingInputHandlingFlags.OnPressed | BindingInputHandlingFlags.OnReleased)
		{ 
			_BindingsSet.Add(binding);
			HandleFlags = handleFlags;
		}

		/// <summary>
		/// Creates a handler that handles multiple <see cref="KeybindingType"/>s.
		/// </summary>
		/// <param name="handleFlags">Handler Flags.</param>
		/// <param name="bindings">The bindings.</param>
		protected BaseBindingInputMessageHandler(BindingInputHandlingFlags handleFlags = BindingInputHandlingFlags.OnPressed | BindingInputHandlingFlags.OnReleased, params TKeybindingEnumType[] bindings)
		{
			foreach (var binding in bindings.Distinct())
				_BindingsSet.Add(binding);

			HandleFlags = handleFlags;
		}

		/// <inheritdoc />
		public void BindTo(ITypeBinder<IMessageHandler<KeyBindingInputChangedEventArgs<TKeybindingEnumType>, string>, KeyBindingInputChangedEventArgs<TKeybindingEnumType>> bindTarget)
		{
			bindTarget.Bind<KeyBindingInputChangedEventArgs<TKeybindingEnumType>>(this);
		}

		/// <inheritdoc />
		public virtual Task HandleMessageAsync(string context, KeyBindingInputChangedEventArgs<TKeybindingEnumType> message, CancellationToken token = default)
		{
			// Anything that happens when not connected or not in world should be considered a reset only.
			// TODO: Remember SwanSong has a connected game of the gamestate, override for that maybe.

			if (message.Down)
			{
				if (HandleFlags.HasAnyFlags(BindingInputHandlingFlags.OnPressed))
					HandleBindPressState(message);
			}
			else
			{
				if (HandleFlags.HasAnyFlags(BindingInputHandlingFlags.OnReleased))
					HandleBindPressState(message);
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Implementer should handle the provided bind data change.
		/// </summary>
		/// <param name="args">The bind info.</param>
		protected abstract void HandleBindPressState(KeyBindingInputChangedEventArgs<TKeybindingEnumType> args);

		/// <summary>
		/// Implementer should handle reset state here when disconnected.
		/// </summary>
		/// <param name="args">The args (will be release/not down)</param>
		protected abstract void Reset(KeyBindingInputChangedEventArgs<TKeybindingEnumType> args);
	}
}
