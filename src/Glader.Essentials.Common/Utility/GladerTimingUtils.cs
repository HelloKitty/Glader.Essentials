using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	public static class GladerTimingUtils
	{
		/// <summary>
		/// Times an async action and returns the time.
		/// </summary>
		/// <param name="asyncTask">The async function/task.</param>
		/// <param name="timerName">The name of the timer.</param>
		/// <returns></returns>
		public static async Task<TimeSpan> PerformTimedActionAsync(Func<Task> asyncTask, string timerName)
		{
			Stopwatch watch = new();
			watch.Start();

			try
			{
				await asyncTask();

				watch.Stop();
				return watch.Elapsed;
			}
			catch(InvalidOperationException e)
			{
				throw new InvalidOperationException($"Failed to finish timed Task: {timerName}. Reason: {e}", e);
			}
			finally
			{
				if(watch.IsRunning)
					watch.Stop();
			}
		}

		/// <summary>
		/// Times an async action and returns the time.
		/// </summary>
		/// <param name="asyncTask">The async function/task.</param>
		/// <param name="timerName">The name of the timer.</param>
		/// <param name="logFunc">The logging function.</param>
		/// <returns></returns>
		public static async Task PerformTimedActionAsync(Func<Task> asyncTask, string timerName, Action<TimeSpan, string> logFunc)
		{
			Stopwatch watch = new();
			watch.Start();

			try
			{
				await asyncTask();

				watch.Stop();
				logFunc(watch.Elapsed, $"Timed {timerName}: {watch.Elapsed.TotalSeconds}");
			}
			catch(InvalidOperationException e)
			{
				throw new InvalidOperationException($"Failed to finish timed Task: {timerName}. Reason: {e}", e);
			}
			finally
			{
				if(watch.IsRunning)
					watch.Stop();
			}
		}

		/// <summary>
		/// Times an action and returns the time.
		/// </summary>
		/// <param name="func">The function.</param>
		/// <param name="timerName">The name of the timer.</param>
		/// <param name="logFunc">The logging function.</param>
		/// <returns></returns>
		public static void PerformTimedAction(Action func, string timerName, Action<TimeSpan, string> logFunc)
		{
			Stopwatch watch = new();
			watch.Start();

			try
			{
				func();

				watch.Stop();
				logFunc(watch.Elapsed, $"Timed {timerName}: {watch.Elapsed.TotalSeconds}");
			}
			catch(InvalidOperationException e)
			{
				throw new InvalidOperationException($"Failed to finish timed Task: {timerName}. Reason: {e}", e);
			}
			finally
			{
				if(watch.IsRunning)
					watch.Stop();
			}
		}
	}
}
