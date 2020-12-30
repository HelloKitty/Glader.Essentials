using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	public interface IAsyncFactoryCreatable<TCreateType, in TContextType> : IFactoryCreatable<Task<TCreateType>, TContextType>
	{

	}
}
