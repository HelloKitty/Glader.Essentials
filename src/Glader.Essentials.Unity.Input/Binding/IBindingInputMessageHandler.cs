using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a handler for <see cref="KeyBindingInputChangedEventArgs{TBindingEnumType}"/>s.
	/// </summary>
	public interface IBindingInputMessageHandler<TKeyBindingEnumType> : IMessageHandler<KeyBindingInputChangedEventArgs<TKeyBindingEnumType>, string> 
		where TKeyBindingEnumType : Enum
	{
		/// <summary>
		/// The bindings this <see cref="IBindingInputMessageHandler{TKeyBindingEnumType}"/>
		/// can handle <see cref="KeyBindingInputChangedEventArgs{TBindingEnumType}"/>s for.
		/// </summary>
		IEnumerable<TKeyBindingEnumType> Bindings { get; }
	}
}
