using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for services/components that need to be ticked
	/// forward in time at the end of the frame.
	/// </summary>
	public interface IGameLateTickable
	{
		/// <summary>
		/// Called to tick the game forward at the end of the frame.
		/// </summary>
		void LateTick();
	}
}
