using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	/// <inheritdoc />
	public sealed class BindingSystemInputCameraInputController : IBindingSystemCameraInputController
	{
		private bool _CameraLookState = false;

		/// <inheritdoc />
		public float CurrentHorizontal => Input.GetAxis("Mouse X");

		/// <inheritdoc />
		public float CurrentVertical => Input.GetAxis("Mouse Y");

		/// <inheritdoc />
		public bool IsCameraDirectionMoving => _CameraLookState || IsCameraControllingFacing;

		/// <inheritdoc />
		public bool IsCameraControllingFacing { get; private set; } = false;

		/// <inheritdoc />
		public void SetCameraLook(bool state)
		{
			_CameraLookState = state;
		}

		/// <inheritdoc />
		public void SetCameraTurning(bool state)
		{
			IsCameraControllingFacing = state;
		}
	}
}
