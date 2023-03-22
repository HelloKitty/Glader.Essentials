using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Glader.Essentials
{
	/// <summary>
	/// Basic message handling service that maps incoming message types to <see cref="IMessageHandler{TMessageType,TMessageContext}"/>'s
	/// No longer threadsafe by default, binding handlers is not a threadsafe operation anymore.
	/// </summary>
	/// <typeparam name="TMessageType"></typeparam>
	/// <typeparam name="TMessageContext"></typeparam>
	public sealed class DefaultMessageHandlerService<TMessageType, TMessageContext> 
		: IMessageHandlerService<TMessageType, TMessageContext>, ITypeBinder<IMessageHandler<TMessageType, TMessageContext>, TMessageType>
		where TMessageType : class
	{
		/// <summary>
		/// Internal routing map that maps message <see cref="Type"/> to <see cref="IMessageHandler{TMessageType,TMessageContext}"/> instance.
		/// </summary>
		private Dictionary<Type, IMessageHandler<TMessageType, TMessageContext>> HandlerRouteMap { get; } = new Dictionary<Type, IMessageHandler<TMessageType, TMessageContext>>();

		/// <inheritdoc />
		public async Task<bool> HandleMessageAsync(TMessageContext context, TMessageType message, CancellationToken token = default)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			if (message == null) throw new ArgumentNullException(nameof(message));

			//TODO: Should we log?
			IMessageHandler<TMessageType, TMessageContext> handler;
			if (!HandlerRouteMap.ContainsKey(message.GetType()))
			{
				// This gets the DEFAULT handler (I had to look at this for awhile once to remember so warning my future self here lol)
				handler = HandlerRouteMap[typeof(TMessageType)];
			}
			else
				//Route to a handler that matches the message type.
				handler = HandlerRouteMap[message.GetType()];

			//Possible there is no bound handler for this message type.
			if (handler == null)
				return false;

			await handler.HandleMessageAsync(context, message, token);
			return true;
		}

		/// <inheritdoc />
		public bool Bind<TBindType>(IMessageHandler<TMessageType, TMessageContext> target)
			where TBindType : TMessageType
		{ 
			HandlerRouteMap[typeof(TBindType)] = target;
			return true;
		}
	}
}
