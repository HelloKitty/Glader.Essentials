using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public static class StringExtensions
	{
		// From: https://stackoverflow.com/a/141076
		/// <summary>
		/// Replaces the first instance of the <see cref="search"/> string with the <see cref="replace"/> string.
		/// </summary>
		/// <param name="text">The text to modify.</param>
		/// <param name="search">The search string.</param>
		/// <param name="replace">The replacement string.</param>
		/// <returns>The new string.</returns>
		public static string ReplaceFirst(this string text, string search, string replace)
		{
			int pos = text.IndexOf(search, StringComparison.Ordinal);
			if(pos < 0)
			{
				return text;
			}
			return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
		}

		/// <summary>
		/// Similar to <see cref="string"/>.Replace.
		/// Replaces all %s format specifiers from WoW with the provided <see cref="value"/>.
		/// </summary>
		/// <param name="text">Text to modify.</param>
		/// <param name="value">Value to replace %s with.</param>
		/// <returns>The modified string.</returns>
		public static string ReplaceS(this string text, string value)
		{
			return text.Replace("%s", value);
		}

		/// <summary>
		/// Similar to <see cref="string"/>.Replace.
		/// Replaces all %d format specifiers from WoW with the provided <see cref="value"/>.
		/// </summary>
		/// <param name="text">Text to modify.</param>
		/// <param name="value">Value to replace %d with.</param>
		/// <returns>The modified string.</returns>
		public static string ReplaceD(this string text, int value)
		{
			return text.Replace("%d", value.ToString());
		}

		/// <summary>
		/// Similar to <see cref="string"/>.Replace.
		/// Replaces the first %d format specifiers from WoW with the provided <see cref="value"/>.
		/// </summary>
		/// <param name="text">Text to modify.</param>
		/// <param name="value">Value to replace %d with.</param>
		/// <returns>The modified string.</returns>
		public static string ReplaceFirstD(this string text, int value)
		{
			return text.ReplaceFirst("%d", value.ToString());
		}

		/// <summary>
		/// Similar to <see cref="string"/>.Replace.
		/// Replaces the first %s format specifiers from WoW with the provided <see cref="value"/>.
		/// </summary>
		/// <param name="text">Text to modify.</param>
		/// <param name="value">Value to replace %s with.</param>
		/// <returns>The modified string.</returns>
		public static string ReplaceFirstS(this string text, string value)
		{
			return text.ReplaceFirst("%s", value);
		}
	}
}
