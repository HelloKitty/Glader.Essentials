using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <inheritdoc />
	public interface IBindingSystemCameraInputController : ICameraInputController
	{
		/// <summary>
		/// Sets the camera look state to <see cref="state"/>.
		/// </summary>
		/// <param name="state">The look state.</param>
		void SetCameraLook(bool state);

		/// <summary>
		/// Sets the camera turning state to <see cref="state"/>.
		/// </summary>
		/// <param name="state">The camera turn state.</param>
		void SetCameraTurning(bool state);
	}
}
