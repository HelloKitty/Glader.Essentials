using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for handlers that handle a message of type <typeparamref name="TMessageType"/>
	/// </summary>
	/// <typeparam name="TMessageType">The type of message to be handled.</typeparam>
	/// <typeparam name="TMessageContext">The context associated with the message.</typeparam>
	public interface IMessageHandler<TMessageType, in TMessageContext> 
		: ITypeBindable<IMessageHandler<TMessageType, TMessageContext>, TMessageType>
		where TMessageType : class
	{
		/// <summary>
		/// Handles the message with <see cref="context"/> provided and correctly typed
		/// <see cref="message"/>.
		/// </summary>
		/// <param name="context">The message context.</param>
		/// <param name="message">The payload to handle.</param>
		/// <param name="token">The cancel token for the handle operation.</param>
		Task HandleMessageAsync(TMessageContext context, TMessageType message, CancellationToken token = default);
	}
}
