using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Glader
{
	public sealed class ToggleCollectionUnityUIAdapter : BaseSerializeableUnityUICollectionAdapter<IUIToggle, UnityToggleUIToggleAdapter>
	{
		protected override IUIToggle[] InterfaceTypes()
		{
			return SerializedElements;
		}
	}
}
