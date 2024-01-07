using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for types that should be ticked like <see cref="IGameTickable"/> but with only a specified <see cref="TickFrequency"/>.
	/// </summary>
	public interface IGameVariableRateTickable
	{
		/// <summary>
		/// The duration of the wait/frequency of the tick.
		/// </summary>
		TimeSpan TickFrequency { get; }

		/// <summary>
		/// Update tick called based on <see cref="TickFrequency"/>.
		/// </summary>
		void OnGameVariableRateTick();
	}
}
