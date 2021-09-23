using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Strategy for determining if you're on the main thread of the application.
	/// </summary>
	public interface IMainThreadDeterminable
	{
		/// <summary>
		/// Indicates if the current thread is the main thread.
		/// </summary>
		abstract bool IsMainThread { get; }
	}
}
