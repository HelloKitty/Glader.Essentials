﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	//From GladMMO: Based on AvatarLerper
	public sealed class ObjectLerper : MonoBehaviour
	{
		private Vector3 lastPosition;

		private Quaternion lastRotation;

		[SerializeField] 
		private Transform TransformToFollow;

		[SerializeField] 
		private float LerpPower = 0.7f;

		[SerializeField] 
		private bool UseLateUpdate = true;

		void Start()
		{
			lastPosition = transform.position;
			lastRotation = transform.rotation;
		}

		void Update()
		{
			if (!UseLateUpdate)
				Lerp();
		}

		void LateUpdate()
		{
			if (UseLateUpdate)
				Lerp();
		}

		private void Lerp()
		{
			lastPosition = transform.position = Vector3.Slerp(lastPosition, TransformToFollow.position, LerpPower * Time.deltaTime);
			lastRotation = transform.rotation = Quaternion.Slerp(lastRotation, TransformToFollow.rotation, LerpPower * Time.deltaTime);
		}
	}
}
