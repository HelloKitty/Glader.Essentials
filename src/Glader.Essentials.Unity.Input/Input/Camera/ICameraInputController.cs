using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Camera input implementation of <see cref="IAxisInputController"/>
	/// </summary>
	public interface ICameraInputController : IAxisInputController
	{
		/// <summary>
		/// Indicates if the camera direction is moving.
		/// </summary>
		bool IsCameraDirectionMoving { get; }

		/// <summary>
		/// Indicates if the camera is in a controlling 
		/// </summary>
		bool IsCameraControllingFacing { get; }
	}
}
