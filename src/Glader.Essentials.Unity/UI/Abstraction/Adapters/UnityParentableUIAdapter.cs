using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using JetBrains.Annotations;
using UnityEngine;

namespace Glader
{
	public sealed class UnityParentableUIAdapter : BaseUnityUIAdapter<Transform, IUIParentable>, IUIParentable
	{
		public void Parent([NotNull] GameObject go)
		{
			if (go == null) throw new ArgumentNullException(nameof(go));

			go.transform.parent = this.UnityUIObject;
		}
	}
}
