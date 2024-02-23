using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	/// <summary>
	/// From SwanSong.
	/// Component will make the transform always face the camera.
	/// </summary>
	[RequireComponent(typeof(Transform))]
	public sealed class AlwaysFaceCameraComponent : MonoBehaviour
	{
		[field: SerializeField]
		public bool YAxisOnly { get; set; } = false;

		private void LateUpdate()
		{
			if (Camera.main == null)
				return;

			if(YAxisOnly)
			{
				var currentRotation = transform.rotation.eulerAngles;
				transform.rotation = Quaternion.Euler(currentRotation.x, Camera.main.transform.eulerAngles.y, currentRotation.z);
			}
			else
			{
				// Based on: https://forum.unity.com/threads/make-textmesh-face-camera.251840/
				transform.rotation = Camera.main.transform.rotation;
			}
		}
	}
}
