using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a handler for <see cref="KeyBindingInputChangedEventArgs{TBindingEnumType}"/>s.
	/// </summary>
	public interface IBindingInputMessageHandler<TBindingEnumType> : IMessageHandler<KeyBindingInputChangedEventArgs<TBindingEnumType>, string> 
		where TBindingEnumType : Enum
	{
		/// <summary>
		/// The bindings this <see cref="IBindingInputMessageHandler{TKeyBindingEnumType}"/>
		/// can handle <see cref="KeyBindingInputChangedEventArgs{TBindingEnumType}"/>s for.
		/// </summary>
		IEnumerable<TBindingEnumType> Bindings { get; }
	}
}
