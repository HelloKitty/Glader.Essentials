using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	/// <summary>
	/// Represents the definition of a color including the <see cref="Color"/> and the <see cref="HexCode"/>.
	/// </summary>
	public sealed record UnityColorDefinition(Color Color, string HexCode)
	{
		/// <summary>
		/// Internal lazily computed ToLower version of <see cref="HexCode"/>
		/// </summary>
		private Lazy<string> HexCodeLowerLazy { get; } = new(HexCode.ToLowerInvariant);

		/// <summary>
		/// Cached lower-case <see cref="HexCode"/>.
		/// </summary>
		public string HexCodeLower => HexCodeLowerLazy.Value;

		/// <summary>
		/// Creates a new <see cref="UnityColorDefinition"/> with the provided <see cref="color"/>.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>A <see cref="UnityColorDefinition"/> based on the provided <see cref="color"/>.</returns>
		public static UnityColorDefinition Create(Color color)
		{
			return new UnityColorDefinition(color, ColorUtility.ToHtmlStringRGB(color));
		}
	}
}
