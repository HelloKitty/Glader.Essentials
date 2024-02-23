using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Input binding based implementation of <see cref="IMovementInputController"/>.
	/// </summary>
	public interface IBindingSystemMovementInputController : IMovementInputController
	{
		/// <summary>
		/// Starts forward movement.
		/// </summary>
		void MoveForwardStart();

		/// <summary>
		/// Stops forward movement.
		/// </summary>
		void MoveForwardStop();


		/// <summary>
		/// Starts backwards movement.
		/// </summary>
		void MoveBackwardStart();

		/// <summary>
		/// Stops backwards movement.
		/// </summary>
		void MoveBackwardStop();


		/// <summary>
		/// Starts left strafe.
		/// </summary>
		void StrafeLeftStart();

		/// <summary>
		/// Stops left strafe.
		/// </summary>
		void StrafeLeftStop();


		/// <summary>
		/// Starts right strafe.
		/// </summary>
		void StrafeRightStart();

		/// <summary>
		/// Stops righ strafe.
		/// </summary>
		void StrafeRightStop();

		/// <summary>
		/// Toggles autorun.
		/// </summary>
		void ToggleAutoRun();

		/// <summary>
		/// Sets the moving/steer state to <see cref="state"/>.
		/// </summary>
		/// <param name="state">State to set the move steer state to.</param>
		void SetMoveSteerState(bool state);

		/// <summary>
		/// Sets the autorun state directly to <see cref="state"/>
		/// </summary>
		/// <param name="state">The state</param>
		void SetAutoRun(bool state);
	}
}
