using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Glader.Essentials
{
	public static class EnumDatabaseExtensions
	{
		//From: https://stackoverflow.com/questions/4367723/get-enum-from-description-attribute
		/// <summary>
		/// Retrieves the <see cref="DescriptionAttribute"/> from the enum field value.
		/// </summary>
		/// <typeparam name="TEnumType">The enum type.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="DescriptionAttribute"/> on the enum field value if it exists.</returns>
		public static DescriptionAttribute GetEnumDescription<TEnumType>(this TEnumType value) 
			where TEnumType : Enum
		{
			return value.GetEnumFieldAttribute<DescriptionAttribute>();
		}

		//From: https://stackoverflow.com/questions/4367723/get-enum-from-description-attribute
		/// <summary>
		/// Retrieves the <see cref="DisplayAttribute"/> from the enum field value.
		/// </summary>
		/// <typeparam name="TEnumType">The enum type.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="DisplayAttribute"/> on the enum field value if it exists.</returns>
		public static DisplayAttribute GetEnumDisplay<TEnumType>(this TEnumType value)
			where TEnumType : Enum
		{
			return value.GetEnumFieldAttribute<DisplayAttribute>();
		}
	}
}
