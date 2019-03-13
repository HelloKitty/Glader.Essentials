using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Glader.Essentials
{
	public static class UnityAsyncHelper
	{
		public static SynchronizationContext UnityMainThreadContext { get; private set; }

		//TODO: Make internal set
		public static MonoBehaviour UnityUIAsyncContinuationBehaviour { get; set; }

		private static readonly object SyncObj = new object();

		private static TaskCompletionSource<object> NextFrameCompletionSource { get; set; }

		/// <summary>
		/// Sets the <see cref="UnityMainThreadContext"/> with the current
		/// thread's (caller thread) sync context.
		/// </summary>
		public static void InitializeSyncContext()
		{
			UnityMainThreadContext = SynchronizationContext.Current;
		}

		/// <summary>
		/// Can be awaited to yield running until
		/// the next tickable frame has ended.
		/// </summary>
		/// <returns></returns>
		public static Task AwaitNextTickableFrameAsync()
		{
			lock(SyncObj)
			{
				if(NextFrameCompletionSource != null)
					return NextFrameCompletionSource.Task;
				else
					return (NextFrameCompletionSource = new TaskCompletionSource<object>()).Task;
			}
		}

		/// <summary>
		/// Sets the completion of the current running tickable frame.
		/// </summary>
		public static void SetNextTickableFrame()
		{
			lock(SyncObj)
			{
				if(NextFrameCompletionSource != null)
				{
					NextFrameCompletionSource.SetResult(null);
					NextFrameCompletionSource = null;
				}

				//If it's null then we don't need to do anything, nothing was awaiting this.
			}
		}
	}
}
