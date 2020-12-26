using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for services/components that need to be ticked
	/// forward in time in a fixed way.
	/// </summary>
	public interface IGameFixedTickable
	{
		/// <summary>
		/// Called to in a fixed time step.
		/// </summary>
		void OnGameFixedTick();
	}
}
