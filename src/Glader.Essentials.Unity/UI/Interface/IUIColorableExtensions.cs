using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Glader.Essentials
{
	public static class IUIColorableExtensions
	{
		/// <summary>
		/// Sets the Element's color the provided <see cref="color"/>.
		/// </summary>
		/// <param name="colorable"></param>
		/// <param name="color"></param>
		public static void SetColor([NotNull] this IUIColorable colorable, Color32 color)
		{
			if (colorable == null) throw new ArgumentNullException(nameof(colorable));
			colorable.SetColor(color.r, color.g, color.b, color.a);
		}
	}
}
