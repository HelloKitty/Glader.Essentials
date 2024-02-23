using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <inheritdoc />
	public sealed class BindingSystemMovementInputController : IBindingSystemMovementInputController
	{
		/// <summary>
		/// Based on FreecraftCore's move flags, for compatibility/ease of implementation with porting the SwanSong implementation.
		/// </summary>
		[Flags]
		private enum MovementFlag
		{
			MOVEMENTFLAG_NONE = 0x00000000,
			MOVEMENTFLAG_FORWARD = 0x00000001,
			MOVEMENTFLAG_BACKWARD = 0x00000002,
			MOVEMENTFLAG_STRAFE_LEFT = 0x00000004,
			MOVEMENTFLAG_STRAFE_RIGHT = 0x00000008,
			MOVEMENTFLAG_LEFT = 0x00000010,
			MOVEMENTFLAG_RIGHT = 0x00000020,
		}

		// in the future, if you're wondering why we use flags it's because
		// raw values won't work in the case of WoW input because multiple binds/inputs should activate moving forward
		// for example and there is no way to sync/track this with just raw values. It'll desync and end up leaving the player running without
		// buttons pressed sometimes or the opposite.
		private MovementFlag Flags = MovementFlag.MOVEMENTFLAG_NONE;

		/// <inheritdoc />
		public float CurrentHorizontal => CalculateHorizontal();

		/// <inheritdoc />
		public float CurrentVertical => CalculateVertical();

		private bool AutoRun = false;

		private bool MoveFromSteer = false;

		private float CalculateHorizontal()
		{
			int horizontal = 0;

			if(Flags.HasAnyFlags(MovementFlag.MOVEMENTFLAG_STRAFE_LEFT))
				horizontal -= 1;

			if(Flags.HasAnyFlags(MovementFlag.MOVEMENTFLAG_STRAFE_RIGHT))
				horizontal += 1;

			return horizontal;
		}

		private float CalculateVertical()
		{
			int vertical = 0;

			if(Flags.HasAnyFlags(MovementFlag.MOVEMENTFLAG_FORWARD) || AutoRun || MoveFromSteer)
				vertical += 1;

			if(Flags.HasAnyFlags(MovementFlag.MOVEMENTFLAG_BACKWARD))
				vertical -= 1;

			return vertical;
		}

		/// <inheritdoc />
		public void MoveForwardStart()
		{
			Flags |= MovementFlag.MOVEMENTFLAG_FORWARD;
			TurnOffAutoRun();
		}

		/// <inheritdoc />
		public void MoveForwardStop()
		{
			Flags &= ~MovementFlag.MOVEMENTFLAG_FORWARD;
		}

		/// <inheritdoc />
		public void MoveBackwardStart()
		{
			Flags |= MovementFlag.MOVEMENTFLAG_BACKWARD;
			TurnOffAutoRun();
		}

		/// <inheritdoc />
		public void MoveBackwardStop()
		{
			Flags &= ~MovementFlag.MOVEMENTFLAG_BACKWARD;
		}

		/// <inheritdoc />
		public void StrafeLeftStart()
		{
			Flags |= MovementFlag.MOVEMENTFLAG_STRAFE_LEFT;
		}

		/// <inheritdoc />
		public void StrafeLeftStop()
		{
			Flags &= ~MovementFlag.MOVEMENTFLAG_STRAFE_LEFT;
		}

		/// <inheritdoc />
		public void StrafeRightStart()
		{
			Flags |= MovementFlag.MOVEMENTFLAG_STRAFE_RIGHT;
		}

		/// <inheritdoc />
		public void StrafeRightStop()
		{
			Flags &= ~MovementFlag.MOVEMENTFLAG_STRAFE_RIGHT;
		}

		/// <inheritdoc />
		public void ToggleAutoRun()
		{
			AutoRun = !AutoRun;
		}

		/// <inheritdoc />
		public void SetMoveSteerState(bool state)
		{
			MoveFromSteer = state;

			// If we start movesteer it turns off autorun BUT
			// if we activate autorun during move steer we shouldn't stop it.
			if(MoveFromSteer)
				TurnOffAutoRun();
		}

		/// <inheritdoc />
		public void SetAutoRun(bool state)
		{
			AutoRun = state;
		}

		private void TurnOffAutoRun()
		{
			AutoRun = false;
		}
	}
}
