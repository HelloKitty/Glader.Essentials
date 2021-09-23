using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Glader.Essentials
{
	/// <summary>
	/// Info providing object that implements <see cref="IMainThreadDeterminable"/>.
	/// </summary>
	public sealed class UnityThreadInfoProvider : IMainThreadDeterminable
	{
		/// <inheritdoc />
		public bool IsMainThread => UnityAsyncHelper.UnityMainThreadContext == SynchronizationContext.Current;
	}
}
