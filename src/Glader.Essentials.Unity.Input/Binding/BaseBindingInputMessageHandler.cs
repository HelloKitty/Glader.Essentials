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
	public abstract class BaseBindingInputMessageHandler<TBindingEnumType> : IBindingInputMessageHandler<TBindingEnumType>
		where TBindingEnumType : Enum
	{
		private HashSet<TBindingEnumType> _BindingsSet { get; } = new();

		/// <inheritdoc />
		public IEnumerable<TBindingEnumType> Bindings => _BindingsSet;

		private BindingInputHandlingFlags HandleFlags { get; }

		protected BaseBindingInputMessageHandler(TBindingEnumType binding,
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
		protected BaseBindingInputMessageHandler(BindingInputHandlingFlags handleFlags = BindingInputHandlingFlags.OnPressed | BindingInputHandlingFlags.OnReleased, params TBindingEnumType[] bindings)
		{
			foreach (var binding in bindings.Distinct())
				_BindingsSet.Add(binding);

			HandleFlags = handleFlags;
		}

		/// <inheritdoc />
		public void BindTo(ITypeBinder<IMessageHandler<KeyBindingInputChangedEventArgs<TBindingEnumType>, string>, KeyBindingInputChangedEventArgs<TBindingEnumType>> bindTarget)
		{
			bindTarget.Bind<KeyBindingInputChangedEventArgs<TBindingEnumType>>(this);
		}

		/// <inheritdoc />
		public virtual Task HandleMessageAsync(string context, KeyBindingInputChangedEventArgs<TBindingEnumType> message, CancellationToken token = default)
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
		protected abstract void HandleBindPressState(KeyBindingInputChangedEventArgs<TBindingEnumType> args);

		/// <summary>
		/// Implementer should handle reset state here when disconnected.
		/// </summary>
		/// <param name="args">The args (will be release/not down)</param>
		protected abstract void Reset(KeyBindingInputChangedEventArgs<TBindingEnumType> args);
	}
}
