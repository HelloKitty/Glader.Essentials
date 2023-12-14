using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public static class MaterialExtensions
	{
		/// <summary>
		/// Sets the bool value on the provided <see cref="Material"/>'s shader.
		/// Using internally SetInt.
		/// </summary>
		/// <param name="mat">Material.</param>
		/// <param name="id">The property id.</param>
		/// <param name="value">The value.</param>
		public static void SetBool(this Material mat, int id, bool value)
		{
			mat.SetInt(id, value ? 1 : 0);
		}

		/// <summary>
		/// Sets the bool value on the provided <see cref="Material"/>'s shader.
		/// Using internally SetInt.
		/// </summary>
		/// <param name="mat">Material.</param>
		/// <param name="id">The property id.</param>
		/// <param name="value">The value.</param>
		public static void SetBool(this Material mat, string id, bool value)
		{
			mat.SetInt(id, value ? 1 : 0);
		}
	}
}
