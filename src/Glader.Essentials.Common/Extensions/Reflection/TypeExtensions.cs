using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Glader.Essentials
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Indicates if the provided type parameter is exactly the specified generic type parameter.
		/// (Warning: If B : A then B.Is{A} is false. Only exact matching.
		/// </summary>
		/// <typeparam name="T">The parameter type to check.</typeparam>
		/// <param name="type"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Is<T>(this Type type)
		{
			return type == typeof(T);
		}

		/// <summary>
		/// Returns a generic-friendly string that describes the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The type name.</returns>
		public static string GetFriendlyName(this Type type)
		{
			if (type == null)
				return null;

			string friendlyName = type.Name;
			if(type.IsGenericType)
			{
				int iBacktick = friendlyName.IndexOf('`');

				if(iBacktick > 0)
					friendlyName = friendlyName.Remove(iBacktick);

				friendlyName += "<";
				Type[] typeParameters = type.GetGenericArguments();

				for(int i = 0; i < typeParameters.Length; ++i)
				{
					string typeParamName = GetFriendlyName(typeParameters[i]);
					friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
				}

				friendlyName += ">";
			}

			return friendlyName;
		}
	}
}
