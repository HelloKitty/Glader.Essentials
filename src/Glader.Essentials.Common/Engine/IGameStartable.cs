using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glader
{
	/// <summary>
	/// Contracts for types that require being informed when
	/// the game actually starts.
	/// </summary>
	public interface IGameStartable
	{
		/// <summary>
		/// Called when the game starts.
		/// Should not be used for initialization.
		/// </summary>
		Task Start();
	}
}
