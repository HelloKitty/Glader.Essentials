using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public static class RichTextHelpers
	{
		/// <summary>
		/// Wraps the provided <see cref="text"/> in rich text color tags with the color <see cref="colorDef"/>.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="colorDef">The color.</param>
		/// <returns>The rich text string wrapped in the color.</returns>
		public static string WrapTextInColor(string text, UnityColorDefinition colorDef)
		{
			return $"<color=#{colorDef.HexCode}>{text}</color>";
		}

		public static string WrapTextInColor(string text, Color color)
		{
			return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
		}
	}
}
