using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IEntityGuidMappable<TKeyType, TValue> : IDictionary<TKeyType, TValue>, IEntityCollectionRemovable<TKeyType>
	{

	}
}