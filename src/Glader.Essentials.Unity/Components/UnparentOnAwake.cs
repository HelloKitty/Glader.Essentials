using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public sealed class UnparentOnAwake : MonoBehaviour
	{
		private void Awake()
		{
			gameObject.transform.parent = null;
		}
	}
}
