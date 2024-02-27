using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public static class UnityColorHelpers
	{
		/// <summary>
		/// Creates a color with the specified components.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Color CreateColor(float r, float g, float b, float a = 1.0f)
		{
			return new Color(r, g, b, a);
		}

		/// <summary>
		/// Creates a color with the specified components.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Color CreateColor(double r, double g, double b)
		{
			return CreateColor((float)r, (float)g, (float)b);
		}
	}
}
