using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a type that provide binding key information.
	/// </summary>
	/// <typeparam name="TBindingEnumType">The binding key type.</typeparam>
	public interface IBindingKeyCache<TBindingEnumType>
		where TBindingEnumType : Enum
	{
		/// <summary>
		/// Retrieves the first associated <see cref="KeyCode"/> with the provided <see cref="binding"/>.
		/// </summary>
		/// <param name="binding">The binding to check.</param>
		/// <returns>The first associated keycode.</returns>
		KeyCode GetFirstKeyBinding(TBindingEnumType binding);

		/// <summary>
		/// Indicates if the binding cache contains a binding for the provided <see cref="binding"/> value.
		/// </summary>
		/// <param name="binding"></param>
		/// <returns>True if the cache contains the value.</returns>
		bool ContainsKey(TBindingEnumType binding);

		/// <summary>
		/// Retrieves a list of all defined bindings in the cache.
		/// </summary>
		/// <returns>List of bindings.</returns>
		TBindingEnumType[] DefinedBindList();

		/// <summary>
		/// Provides the <see cref="BindingDefiniton"/>s for the <see cref="bind"/> key.
		/// </summary>
		/// <param name="bind">The binding.</param>
		/// <returns>The list of binding defs for the binding key.</returns>
		BindingDefinition[] this[TBindingEnumType bind] { get; }
	}
}
