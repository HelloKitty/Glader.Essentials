using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IUIUnityColorable : IUIColorable
	{
		UnityEngine.Color ElementColor { get; set; }
	}
}
