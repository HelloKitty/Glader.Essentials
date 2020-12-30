using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a type that provides mappings between the specified generic type parameters,
	/// </summary>
	/// <typeparam name="TObjectType1"></typeparam>
	/// <typeparam name="TObjectType2"></typeparam>
	public interface IObjectMapper<TObjectType1, TObjectType2>
	{
		/// <summary>
		/// Maps from <typeparamref name="TObjectType2"/> to <typeparamref name="TObjectType1"/>.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns>Mapped instance of <typeparamref name="TObjectType1"/></returns>
		TObjectType1 Convert(TObjectType2 instance);

		/// <summary>
		/// Maps from <typeparamref name="TObjectType1"/> to <typeparamref name="TObjectType2"/>.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns>Mapped instance of <typeparamref name="TObjectType2"/></returns>
		TObjectType2 Convert(TObjectType1 instance);
	}
}
