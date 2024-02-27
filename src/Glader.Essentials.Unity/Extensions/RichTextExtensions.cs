using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public static class RichTextExtensions
	{
		/// <summary>
		/// Extension to wrarp the provided text <see cref="value"/> in the rich color tags for the <see cref="colorDef"/>.
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <param name="colorDef">The color.</param>
		/// <returns>The text wrapped in the rich text tags for the provided color.</returns>
		public static string WrapInColor(this string value, UnityColorDefinition colorDef)
		{
			return RichTextHelpers.WrapTextInColor(value, colorDef);
		}
	}
}
