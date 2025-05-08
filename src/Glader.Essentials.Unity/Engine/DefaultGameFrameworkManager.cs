using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Glader.Essentials;
using JetBrains.Annotations;
using SceneJect.Common;
using UnityEngine;

namespace Glader.Essentials.Unity
{
	/// <summary>
	/// Default game framework manager that calls init
	/// and ticks tickables.
	/// </summary>
	[Injectee]
	public class DefaultGameFrameworkManager : GladerBehaviour
	{
		/// <summary>
		/// Collection of all game initializables.
		/// </summary>
		[Inject]
		private IEnumerable<IGameInitializable> Initializables { get; set; }

		[Inject]
		private IEnumerable<IGameTickable> Tickables { get; set; }

		[Inject]
		private IEnumerable<IGameLateTickable> LateTickables { get; set; }

		[Inject]
		private IEnumerable<IGameStartable> Startables { get; set; }

		[Inject]
		private IEnumerable<IGameFixedTickable> FixedTickables { get; set; }

		[Inject]
		private IEnumerable<IGameVariableRateTickable> VariableRateTickables { get; set; }

		[Inject]
		private IEnumerable<IGamePreTickable> PreTickables { get; set; }

		[Inject]
		private ILog Logger { get; set; }

		[Inject]
		private IEventBus Bus { get; set; }

		private bool isInitializationFinished = false;

		private async Task Start()
		{
			if (GladerEssentialsGameFrameworkUnityConstants.GAME_FRAMEWORK_TIMING_ENABLED)
			{
				await GladerTimingUtils.PerformTimedActionAsync(async () =>
				{
					await ExecuteIntializablesAsync();
				}, nameof(ExecuteIntializablesAsync), (time, s) =>
				{
					if (time.TotalSeconds > 0.001f)
						if (Logger.IsErrorEnabled)
							Logger.Error(s);
				});

				await GladerTimingUtils.PerformTimedActionAsync(async () =>
				{
					await ExecuteStartablesAsync();
				}, nameof(ExecuteStartablesAsync), (time, s) =>
				{
					if(time.TotalSeconds > 0.001f)
						if(Logger.IsErrorEnabled)
							Logger.Error(s);
				});
			}
			else
			{
				await ExecuteIntializablesAsync();
				await ExecuteStartablesAsync();
			}

			Tickables = OrderTickables(Tickables);

			try
			{
				// Fire this so anything that needs to run after init but before tickables will run.
				Bus.Publish(this, GameFrameworkInitializationFinishedEventArgs.Instance);
			}
			catch (Exception e)
			{
				if (Logger.IsErrorEnabled)
					Logger.Error($"Encountered Exception in Framework Initialization Finished Event. Reason: {e.Message}\n\nStack: {e.StackTrace}");
				throw;
			}

			StartAllVariableRateTickables();

			// @HelloKitty: Hack to reduce same frame allocation in WebGL which increases memory usage.
			if(Application.platform == RuntimePlatform.WebGLPlayer)
				await new UnityYieldAwaitable();

			isInitializationFinished = true;
		}

		private async Task ExecuteIntializablesAsync()
		{
			//The default way to handle this is to just await all initializables.
			//Preferably you'd want this to always run on the main thread, or continue to the main thread
			//but called code could avoid caputring the sync context, so it's out of our control
			foreach (var init in Initializables)
				try
				{
					if (GladerEssentialsGameFrameworkUnityConstants.GAME_FRAMEWORK_TIMING_ENABLED)
					{
						await GladerTimingUtils.PerformTimedActionAsync(async () =>
						{
							await init.OnGameInitialized()
								.ConfigureAwait(true);

							// This increased loading time too much tbh, it's like 1.5 seconds just because of damn EventListeners registering at that point.
							// @HelloKitty: Hack to reduce same frame allocation in WebGL which increases memory usage.
							/*if (Application.platform == RuntimePlatform.WebGLPlayer)
								await new UnityYieldAwaitable();*/

						}, init.GetType().Name, (time, s) =>
						{
							if(time.TotalSeconds > 0.001f)
								if(Logger.IsErrorEnabled)
									Logger.Error(s);
						});
					}
					else
					{
						await init.OnGameInitialized()
							.ConfigureAwait(true);

						// This increased loading time too much tbh, it's like 1.5 seconds just because of damn EventListeners registering at that point.
						// @HelloKitty: Hack to reduce same frame allocation in WebGL which increases memory usage.
						/*if(Application.platform == RuntimePlatform.WebGLPlayer)
							await new UnityYieldAwaitable();*/
					}
				}
				catch (Exception e)
				{
					if (Logger.IsErrorEnabled)
						Logger.Error($"Encountered Exception in {nameof(IGameInitializable.OnGameInitialized)} for Type: {init.GetType().Name}. Reason: {e.Message}\n\nStack: {e.StackTrace}");
					throw;
				}
		}

		private async Task ExecuteStartablesAsync()
		{
			//After Init has been called we should call all starts
			foreach (var startable in Startables)
				try
				{
					if (GladerEssentialsGameFrameworkUnityConstants.GAME_FRAMEWORK_TIMING_ENABLED)
					{
						await GladerTimingUtils.PerformTimedActionAsync(async () =>
						{
							await startable.OnGameStart()
								.ConfigureAwait(true);

							// This increased loading time too much tbh, it's like 1.5 seconds just because of damn EventListeners registering at that point.
							// @HelloKitty: Hack to reduce same frame allocation in WebGL which increases memory usage.
							/*if (Application.platform == RuntimePlatform.WebGLPlayer)
								await new UnityYieldAwaitable();*/

						}, startable.GetType().Name, (time, s) =>
						{
							if(time.TotalSeconds > 0.001f)
								if(Logger.IsErrorEnabled)
									Logger.Error(s);
						});
					}
					else
					{
						await startable.OnGameStart()
							.ConfigureAwait(true);

						// This increased loading time too much tbh, it's like 1.5 seconds just because of damn EventListeners registering at that point.
						// @HelloKitty: Hack to reduce same frame allocation in WebGL which increases memory usage.
						/*if (Application.platform == RuntimePlatform.WebGLPlayer)
							await new UnityYieldAwaitable();*/
					}
				}
				catch (Exception e)
				{
					if (Logger.IsErrorEnabled)
						Logger.Error($"Encountered Exception in {nameof(IGameStartable.OnGameStart)} for Type: {startable.GetType().Name}. Reason: {e.Message}\n\nStack: {e.StackTrace}");
					throw;
				}
		}

		protected virtual IEnumerable<IGameTickable> OrderTickables(IEnumerable<IGameTickable> tickables)
		{
			return tickables;
		}

		private void StartAllVariableRateTickables()
		{
			foreach (var tickable in VariableRateTickables)
				StartCoroutine(VariableRateUpdateCoroutine(tickable));
		}

		private void Update()
		{
			//The reason we don't update until initialization is finished
			//is because we CAN'T let potential tickables that may depend on
			//initializablizes having run, so to avoid this issue we don't run them until they are init
			if (!isInitializationFinished)
			{
				// Even if we don't have init finished we should set the next tickable to handle async events
				UnityAsyncHelper.SetNextTickableFrame();
				return;
			}

			foreach(IGamePreTickable preTickable in PreTickables)
				try
				{
					preTickable.PreTick();
				}
				catch (Exception e)
				{
					if (Logger.IsErrorEnabled)
						Logger.Error($"Encountered Exception in {nameof(IGamePreTickable.PreTick)} for Type: {preTickable.GetType().Name}. Reason: {e.Message}\n\nStack: {e.StackTrace}");
				}
				

			foreach(IGameTickable tickable in Tickables)
				try
				{
					tickable.Tick();
				}
				catch (Exception e)
				{
					if(Logger.IsErrorEnabled)
						Logger.Error($"Encountered Exception in {nameof(IGameTickable.Tick)} for Type: {tickable.GetType().Name}. Reason: {e.Message}\n\nStack: {e.StackTrace}");
				}

			foreach(IGameLateTickable lateTickable in LateTickables)
				try
				{
					lateTickable.LateTick();
				}
				catch (Exception e)
				{
					if(Logger.IsErrorEnabled)
						Logger.Error($"Encountered Exception in {nameof(IGameLateTickable.LateTick)} for Type: {lateTickable.GetType().Name}. Reason: {e.Message}\n\nStack: {e.StackTrace}");
				}

			//After a tickable is finished, we should set tickable compeltion
			UnityAsyncHelper.SetNextTickableFrame();
		}

		private void FixedUpdate()
		{
			if (!isInitializationFinished)
				return;

			foreach(var tickable in FixedTickables)
				tickable.OnGameFixedTick();
		}

		private IEnumerator VariableRateUpdateCoroutine([NotNull] IGameVariableRateTickable tickable)
		{
			if (tickable == null) throw new ArgumentNullException(nameof(tickable));

			var waitForSeconds = new WaitForSeconds((float) tickable.TickFrequency.TotalSeconds);

			// Wait until the next frame to start.
			yield return null;

			while (true)
			{
				yield return waitForSeconds;

				try
				{
					tickable.OnGameVariableRateTick();
				}
				catch (Exception e)
				{
					if (Logger.IsErrorEnabled)
						Logger.Error($"Failed to update: {tickable.GetType().Name} Reason: {e}");
				}
			}
		}
	}
}
