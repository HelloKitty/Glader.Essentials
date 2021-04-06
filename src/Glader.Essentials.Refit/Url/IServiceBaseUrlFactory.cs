using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for types that can build a <see cref="string"/> service path from an input <see cref="Uri"/>.
	/// </summary>
	public interface IServiceBaseUrlFactory : IFactoryCreatable<string, Uri>
	{

	}
}
