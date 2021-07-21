using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Enumeration of all supported event bus subscription modes.
	/// </summary>
	public enum EventBusSubscriptionMode
	{
		/// <summary>
		/// The default subscription mode.
		/// </summary>
		Default = 1,

		/// <summary>
		/// The forwarded subscription mode.
		/// </summary>
		Forwarded = 2,

		/// <summary>
		/// The subscription mode that will subscribe to all published events.
		/// </summary>
		All = 3,

		/// <summary>
		/// The subscription mode that will publish exception events when a subscriber raises an exception.
		/// </summary>
		Exception = 4
	}
}
