using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using JetBrains.Annotations;

namespace Glader.Essentials
{
	// TODO: Doc
	/// <inheritdoc />
	public sealed class DefaultBindingInputMessageHandlingService<TBindingEnumType> 
		: IBindingInputMessageHandlingService<TBindingEnumType> 
		where TBindingEnumType : Enum
	{
		private Dictionary<TBindingEnumType, IBindingInputMessageHandler<TBindingEnumType>> HandlerMap { get; } = new();

		private ILog Logger { get; }

		public DefaultBindingInputMessageHandlingService([NotNull] IEnumerable<IBindingInputMessageHandler<TBindingEnumType>> handlers,
			[NotNull] ILog logger)
		{
			if(handlers == null) throw new ArgumentNullException(nameof(handlers));
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));

			foreach(var handler in handlers)
				foreach(var binding in handler.Bindings)
					if(!HandlerMap.TryAdd(binding, handler))
						if(Logger.IsWarnEnabled)
							Logger.Warn($"Duplicate handler with Type: {binding} tried to add to binding handlers.");
		}

		/// <inheritdoc />
		public void Process(KeyBindingInputChangedEventArgs<TBindingEnumType> message)
		{
			if (!HandlerMap.ContainsKey(message.Type))
			{
				if(Logger.IsErrorEnabled)
					Logger.Error($"KeyBind: {message.Type} is unhandled.");

				return;
			}

			// TODO: Come up with a more important context type for the message.
			HandlerMap[message.Type].HandleMessageAsync(String.Empty, message);
		}
	}
}
