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
	}
}
