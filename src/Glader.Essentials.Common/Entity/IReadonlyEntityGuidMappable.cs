using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IReadonlyEntityGuidMappable<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
	{

	}
}