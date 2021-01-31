using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Glader.Essentials;
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
		private IEnumerable<IGameStartable> Startables { get; set; }

		[Inject]
		private ILog Logger { get; set; }

		private bool isInitializationFinished = false;

		private async Task Start()
		{
			//The default way to handle this is to just await all initializables.
			//Preferably you'd want this to always run on the main thread, or continue to the main thread
			//but called code could avoid caputring the sync context, so it's out of our control
			foreach(var init in Initializables)
				try
				{
					await init.OnGameInitialized()
						.ConfigureAwait(true);
				}
				catch(Exception e)
				{
					if(Logger.IsErrorEnabled)
						Logger.Error($"Encountered Exception in {nameof(IGameInitializable.OnGameInitialized)} for Type: {init.GetType().Name}. Reason: {e.Message}\n\nStack: {e.StackTrace}");
					throw;
				}

			//After Init has been called we should call all starts
			foreach(var startable in Startables)
				try
				{
					await startable.OnGameStart()
						.ConfigureAwait(true);
				}
				catch(Exception e)
				{
					if(Logger.IsErrorEnabled)
						Logger.Error($"Encountered Exception in {nameof(IGameStartable.OnGameStart)} for Type: {startable.GetType().Name}. Reason: {e.Message}\n\nStack: {e.StackTrace}");
					throw;
				}

			Tickables = OrderTickables(Tickables);
			isInitializationFinished = true;
		}

		protected virtual IEnumerable<IGameTickable> OrderTickables(IEnumerable<IGameTickable> tickables)
		{
			return tickables;
		}

		private void Update()
		{
			//The reason we don't update until initialization is finished
			//is because we CAN'T let potential tickables that may depend on
			//initializablizes having run, so to avoid this issue we don't run them until they are init
			if(!isInitializationFinished)
				return;

			foreach(IGameTickable tickable in Tickables)
				tickable.Tick();

			//After a tickable is finished, we should set tickable compeltion
			UnityAsyncHelper.SetNextTickableFrame();
		}
	}
}
