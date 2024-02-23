using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	// From SwanSong
	/// <summary>
	/// Represents a simple axis-based input controller providing Vertical and Horizontal input info.
	/// </summary>
	public interface IAxisInputController
	{
		/// <summary>
		/// Horizontal input value.
		/// </summary>
		float CurrentHorizontal { get; }

		/// <summary>
		/// Vertical input value.
		/// </summary>
		float CurrentVertical { get; }
	}
}
