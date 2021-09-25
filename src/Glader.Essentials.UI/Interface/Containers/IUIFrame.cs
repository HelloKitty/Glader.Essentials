using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a <see cref="IUIElement"/> container.
	/// Frames are nearly top-level UI elements themselves, basically the conceptual
	/// roots of UI sub-windows (window being the toplevel entire screen).
	/// </summary>
	public interface IUIFrame : IUIElement<IUIFrame>
	{
		/// <summary>
		/// Enumerable of all sub-<see cref="IUIElement"/>s.
		/// </summary>
		IEnumerable<IUIElement> Elements { get; }
	}

	/// <summary>
	/// <see cref="IUIFrame"/> implementation with known/named/keyed <typeparamref name="TElementKeyType"/> elements.
	/// </summary>
	/// <typeparam name="TElementKeyType">The key type.</typeparam>
	public interface IUIFrame<in TElementKeyType> : IUIFrame
		where TElementKeyType : Enum
	{
		/// <summary>
		/// Indexer that produces the specified <see cref="IUIElement"/> from the key type <typeparamref name="TElementKeyType"/>
		/// </summary>
		/// <param name="key">The UI key.</param>
		/// <returns></returns>
		IUIElement this[TElementKeyType key] { get; }
	}
}
