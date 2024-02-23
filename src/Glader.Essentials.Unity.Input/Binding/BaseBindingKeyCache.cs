using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Glader.Essentials
{
	/// <summary>
	/// Base implementation of <see cref="IBindingKeyCache{TBindingEnumType}"/> where implementers can provide the binding data dictionary.
	/// </summary>
	/// <typeparam name="TBindingEnumType">The binding enum type.</typeparam>
	public abstract class BaseBindingKeyCache<TBindingEnumType> : IBindingKeyCache<TBindingEnumType> 
		where TBindingEnumType : Enum
	{
		private Dictionary<TBindingEnumType, BindingDefinition[]> MappedBindings { get; }

		/// <inheritdoc />
		public BindingDefinition[] this[TBindingEnumType bind] => MappedBindings[bind];

		/// <summary>
		/// Creates a new <see cref="BaseBindingKeyCache{TBindingEnumType}"/> with the provided bindings data.
		/// </summary>
		/// <param name="mappedBindings"></param>
		protected BaseBindingKeyCache([NotNull] Dictionary<TBindingEnumType, BindingDefinition[]> mappedBindings)
		{
			MappedBindings = mappedBindings ?? throw new ArgumentNullException(nameof(mappedBindings));
		}

		/// <inheritdoc />
		public KeyCode GetFirstKeyBinding(TBindingEnumType binding)
		{
			if(!MappedBindings.TryGetValue(binding, out var bindingInfo))
				return KeyCode.None;

			if(!bindingInfo.Any())
				return KeyCode.None;

			if(bindingInfo.First() is KeyBasedBindingDefinition keyBasedBinding)
				return keyBasedBinding.Code;

			return KeyCode.None;
		}

		/// <inheritdoc />
		public bool ContainsKey(TBindingEnumType binding)
		{
			return MappedBindings.ContainsKey(binding);
		}

		/// <inheritdoc />
		public TBindingEnumType[] DefinedBindList()
		{
			return MappedBindings
				.Keys
				.Distinct()
				.ToArray();
		}
	}
}
