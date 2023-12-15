using System;
using System.Collections.Generic;
using System.Text;
using Generic.Math;

namespace Glader.Essentials
{
	public static class EnumExtensions
	{
		// Ported from FreecraftCore.
		/// <summary>
		/// Indicates if any of the provided <see cref="flags"/> are set on the provided <see cref="FlagsAttribute"/>
		/// <see cref="Enum"/> <see cref="enumValue"/>.
		/// </summary>
		/// <typeparam name="T">The enum type.</typeparam>
		/// <param name="enumValue">The enum value.</param>
		/// <param name="flags">Flags to check for.</param>
		/// <returns>True if any flags/bits are shared between the two provided flags values.</returns>
		public static bool HasAnyFlags<T>(this T enumValue, T flags)
			where T : Enum
		{
			return (GenericMath.Convert<T, int>(enumValue) & GenericMath.Convert<T, int>(flags)) != 0;
		}
	}
}
