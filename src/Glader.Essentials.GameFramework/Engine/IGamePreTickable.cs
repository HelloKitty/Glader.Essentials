using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Similar to <see cref="IGameTickable"/> but instead runs every tick before the tickables.
	/// This is best used for initialization required for Update/Ticks.
	/// </summary>
	public interface IGamePreTickable
	{
		/// <summary>
		/// Called right before the game ticks forward.
		/// </summary>
		void PreTick();
	}
}
