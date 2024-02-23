using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a message handling service that can handle <see cref="IBindingInputMessageHandler{TBindingEnumType}"/>
	/// </summary>
	public interface IBindingInputMessageHandlingService<TBindingEnumType> 
		where TBindingEnumType : Enum
	{
		/// <summary>
		/// Processes the provided binding input data <see cref="message"/>.
		/// </summary>
		/// <param name="message"></param>
		void Process(KeyBindingInputChangedEventArgs<TBindingEnumType> message);
	}
}
