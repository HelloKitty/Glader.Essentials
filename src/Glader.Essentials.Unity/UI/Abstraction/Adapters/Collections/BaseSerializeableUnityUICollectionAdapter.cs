using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public abstract class BaseSerializeableUnityUICollectionAdapter<TInterfaceType, ImplementationType> : BaseCollectionUnityUIAdapter<TInterfaceType>
		where ImplementationType  : TInterfaceType
	{
		[SerializeField]
		[Tooltip("The collection of toggles to aggregate.")]
		private ImplementationType[] _elements;

		/// <inheritdoc />
		protected override IEnumerable<TInterfaceType> Elements => InterfaceTypes();

		//The below is a hack that exists to get around generic co/contra variance issues.
		protected ImplementationType[] SerializedElements => _elements;

		protected abstract TInterfaceType[] InterfaceTypes();
	}
}
