using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for registerable UI adapters.
	/// </summary>
	public interface IUIAdapterRegisterable
	{
		/// <summary>
		/// The  key value.
		/// </summary>
		string RegistrationKey { get; }

		/// <summary>
		/// The actual type to register this UI adapter as.
		/// </summary>
		Type UIServiceType { get; }

		/// <summary>
		/// Indicates if it should use the key or register it directly.
		/// </summary>
		bool AsKeyed { get; }
	}
}
