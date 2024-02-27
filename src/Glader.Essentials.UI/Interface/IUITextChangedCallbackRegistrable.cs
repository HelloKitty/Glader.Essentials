using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Interface for types that provide a callback mechanism for listening to text changes.
	/// </summary>
	public interface IUITextChangedCallbackRegistrable
	{
		/// <summary>
		/// Registers a <see cref="callback"/> which is called when the underlying text changes.
		/// </summary>
		/// <param name="callback">The callback.</param>
		void RegisterTextChangeCallback(Action<string> callback);
	}
}
