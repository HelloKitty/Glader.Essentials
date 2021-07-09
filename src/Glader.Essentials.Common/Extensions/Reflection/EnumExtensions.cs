using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Glader.Essentials
{
	public static class EnumExtensions
	{
		/// <summary>
		/// Retrieves the attribute from the enum field value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The attribute on the enum field value if it exists.</returns>
		public static TAttributeType GetEnumFieldAttribute<TAttributeType>(this Enum value)
			where TAttributeType : Attribute
		{
			return value.GetType()
				.GetField(value.ToString())
				?.GetCustomAttribute<TAttributeType>(false);
		}
	}
}
